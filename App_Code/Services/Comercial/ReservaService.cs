using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class ReservaService
    {
        private ReservaDAO _dao;
        private ReservaOficinaDAO _oficinaDao;
        private ReservaInternetDAO _internetDao;
        private PasajeroDAO _pasajeroDAO;
        private AsientoDAO _asientoDAO;
        private PagoDAO _pagoDAO;
        private BoletoDAO _boletoDAO;

        public ReservaService()
        {
            _dao = new ReservaDAO();
            _oficinaDao = new ReservaOficinaDAO();
            _internetDao = new ReservaInternetDAO();
            _pasajeroDAO = new PasajeroDAO();
            _asientoDAO = new AsientoDAO();
            _pagoDAO = new PagoDAO();
            _boletoDAO = new BoletoDAO();
        }

        public List<Reserva> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Reserva ObtenerPorId(string codReserva)
        {
            if (string.IsNullOrWhiteSpace(codReserva)) return null;
            return _dao.ObtenerPorId(codReserva);
        }

        // Venta en oficina: brisky.registrar_venta crea reserva + boleto + pago
        // en una sola transacción.
        public string RegistrarVenta(ReservaOficina reserva)
        {
            ValidarCamposBase(reserva);

            if (string.IsNullOrWhiteSpace(reserva.CodEmpleado))
                throw new ArgumentException("Debe indicar el empleado que atiende la venta.");
            if (string.IsNullOrWhiteSpace(reserva.NumBoleto))
                throw new ArgumentException("Debe indicar el número de boleto a emitir.");
            if (string.IsNullOrWhiteSpace(reserva.MetodoPago))
                throw new ArgumentException("Debe indicar el método de pago.");
            if (string.IsNullOrWhiteSpace(reserva.CodPago))
                throw new ArgumentException("Debe indicar el código de pago.");

            try
            {
                return _oficinaDao.Insertar(reserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva por internet: brisky.crear_reserva_internet solo crea la
        // reserva (queda PENDIENTE); el boleto y el pago se generan después
        // con GenerarBoleto()/Pago por separado.
        public string RegistrarReservaInternet(ReservaInternet reserva)
        {
            ValidarCamposBase(reserva);

            try
            {
                return _internetDao.Insertar(reserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva.confirmar()
        public void Confirmar(string codReserva)
        {
            try
            {
                _dao.Confirmar(codReserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva.cancelar()
        public void Cancelar(string codReserva)
        {
            try
            {
                _dao.Cancelar(codReserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva.generarBoleto() -- pensado para el flujo de Internet, ya que
        // en oficina el boleto se genera dentro de RegistrarVenta().
        public string GenerarBoleto(string codReserva, string numBoleto)
        {
            if (string.IsNullOrWhiteSpace(numBoleto))
                throw new ArgumentException("Debe indicar el número de boleto a generar.");

            try
            {
                return _dao.GenerarBoleto(codReserva, numBoleto);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        private string GenerarCodigoReservaUnico()
        {
            return "RES-" + DateTime.Now.ToString("MMddHHmmss");
        }

        public bool ProcesarCompraMultipasajero(int idVuelo, List<Pasajero> pasajeros, List<string> asientosElegidos, string codTarifa, string ipOrigen)
        {
            // Validación de negocio fundamental
            if (pasajeros.Count != asientosElegidos.Count)
            {
                throw new Exception("La cantidad de pasajeros no coincide con los asientos seleccionados.");
            }

            // Se recomienda usar TransactionScope para asegurar que si falla el pasajero 2, 
            // se haga Rollback del pasajero 1 y no queden datos huérfanos.
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    for (int i = 0; i < pasajeros.Count; i++)
                    {
                        Pasajero pax = pasajeros[i];
                        string numAsiento = asientosElegidos[i];
                        string codReserva = GenerarCodigoReservaUnico() + i.ToString(); // Añadimos i para evitar duplicidad si es rápido

                        // 1. Guardar Pasajero (Si no existe)
                        if (_pasajeroDAO.ObtenerPorDocumento(pax.NumDocumento) == null)
                        {
                            _pasajeroDAO.Insertar(pax);
                        }
                        else
                        {
                            // Si ya existe, usamos el código existente
                            pax.CodPasajero = _pasajeroDAO.ObtenerPorDocumento(pax.NumDocumento).CodPasajero;
                        }

                        // 2. Crear la reserva web llamando al SP
                        // Este SP ejecuta INSERT a reserva y reserva_internet
                        _dao.LlamarSpCrearReservaInternet(codReserva, pax.CodPasajero, idVuelo, codTarifa, ipOrigen);

                        // 3. Bloquear el asiento físicamente en la BD
                        // Este SP utiliza UPDLOCK y ROWLOCK para evitar colisiones
                        _asientoDAO.LlamarSpReservarAsiento(idVuelo, numAsiento);
                        
                        // Nota: Aquí también llamarías al SP de pago y generación de boleto según tu flujo
                    }
                    
                    scope.Complete(); // Confirma toda la transacción
                    return true;
                }
                catch (Exception ex)
                {
                    // El Rollback es automático si no se llama a scope.Complete()
                    throw new Exception("Error al procesar la compra: " + ex.Message);
                }
            }
        }

        private void ValidarCamposBase(Reserva r)
        {
            if (r == null) throw new ArgumentException("Datos de reserva inválidos.");
            if (string.IsNullOrWhiteSpace(r.CodReserva)) throw new ArgumentException("El código de reserva es obligatorio.");
            if (string.IsNullOrWhiteSpace(r.CodPasajero)) throw new ArgumentException("Debe seleccionar un pasajero.");
            if (r.IdVuelo <= 0) throw new ArgumentException("Debe seleccionar un vuelo válido.");
            if (string.IsNullOrWhiteSpace(r.CodTarifa)) throw new ArgumentException("Debe seleccionar una tarifa.");
        }

        private Exception TraducirError(SqlException ex)
        {
            if (ex.Number == 2627 || ex.Number == 2601)
                return new Exception("Alguno de los códigos (reserva, boleto o pago) ya existe en la base de datos.");
            if (ex.Number == 547)
                return new Exception("Verifique que el pasajero, empleado, vuelo o tarifa existan (violación de llave foránea).");
            return new Exception("Error de base de datos: " + ex.Message);
        }

        public string EjecutarCheckoutWeb(int idVuelo, List<Pasajero> pasajeros, List<string> asientos, string codTarifa, string metodoPago)
        {
            string codReservaMaster = "";

            using (TransactionScope scope = new TransactionScope())
            {
                for (int i = 0; i < pasajeros.Count; i++)
                {
                    Pasajero pax = pasajeros[i];
                    string numAsiento = asientos[i];
                    
                    // Generación de códigos internos de manera segura (evitando errores de Substring con Ticks)
                    string uuid = Guid.NewGuid().ToString("N").ToUpper();
                    string codReserva = "RW" + uuid.Substring(0, 8);
                    string codPago = "PW" + uuid.Substring(8, 8);
                    string numBoleto = "TKT" + uuid.Substring(16, 12);

                    if (i == 0) codReservaMaster = codReserva;

                    // PASO 1: Guardar al Pasajero (DAO ejecuta INSERT en brisky.pasajero)
                    if (_pasajeroDAO.ObtenerPorDocumento(pax.NumDocumento) == null)
                    {
                        _pasajeroDAO.Insertar(pax);
                    }
                    else
                    {
                        pax.CodPasajero = _pasajeroDAO.ObtenerPorDocumento(pax.NumDocumento).CodPasajero;
                    }

                    // PASO 2: Crear Reserva Internet
                    // Llama al SP brisky.crear_reserva_internet que crea el registro PENDIENTE
                    _dao.LlamarSpCrearReservaInternet(codReserva, pax.CodPasajero, idVuelo, codTarifa, "IP_WEB");

                    // PASO 3: Procesar el Pago
                    // Llama al SP brisky.procesar_pago insertando en brisky.pago
                    _pagoDAO.ProcesarPago(codPago, codReserva, 350.00m, metodoPago);

                    // PASO 4: Generar Boleto
                    // Llama al SP brisky.generar_boleto (cambia estado a CONFIRMADA)
                    _dao.GenerarBoleto(codReserva, numBoleto);

                    // PASO 5: Emitir Boleto y Reservar Asiento
                    // Llama al SP brisky.emitir_boleto, el cual internamente llama a brisky.reservar_asiento.
                    // Si el asiento ya fue tomado por otro hilo, lanzará un THROW 50000.
                    _boletoDAO.EmitirBoleto(numBoleto, idVuelo, numAsiento);
                }

                // Si todos los pasajeros pasaron sin lanzar excepciones, consolidamos.
                scope.Complete();
                return codReservaMaster;
            }
        }
    }
}
