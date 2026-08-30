using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BriSky.Services.Ubicaciones;
using BriSky.Services.Operaciones;
using BriSky.Services.Comercial;
using BriSky.Models.Comercial;

namespace BriSky.Pages.Cliente
{
    public partial class Compra : System.Web.UI.Page
    {
        // Servicios instanciados a nivel de clase para reuso
        private AeropuertoService _aeropuertoService = new AeropuertoService();
        private VueloService _vueloService = new VueloService();
        private AsientoService _asientoService = new AsientoService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Iniciar el flujo en la vista del buscador
                mvCompra.ActiveViewIndex = 0;
                
                // Restricción HTML5 para la fecha (mínimo hoy)
                txtFecha.Attributes["min"] = DateTime.Today.ToString("yyyy-MM-dd");
                
                CargarAeropuertos();
            }
        }

        #region FASE 1: BUSCADOR Y RESULTADOS

        private void CargarAeropuertos()
        {
            var aeropuertos = _aeropuertoService.ObtenerTodos();
            
            ddlOrigen.DataSource = aeropuertos;
            ddlOrigen.DataTextField = "Nombre"; 
            ddlOrigen.DataValueField = "CodAeropuerto";
            ddlOrigen.DataBind();
            ddlOrigen.Items.Insert(0, new ListItem("Seleccionar", ""));

            ddlDestino.DataSource = aeropuertos;
            ddlDestino.DataTextField = "Nombre";
            ddlDestino.DataValueField = "CodAeropuerto";
            ddlDestino.DataBind();
            ddlDestino.Items.Insert(0, new ListItem("Seleccionar", ""));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string origen = ddlOrigen.SelectedValue;
            string destino = ddlDestino.SelectedValue;
            
            if (string.IsNullOrEmpty(origen) || string.IsNullOrEmpty(destino))
            {
                lblMensajeBusqueda.Text = "Por favor, selecciona un aeropuerto de origen y uno de destino.";
                lblMensajeBusqueda.Visible = true;
                return;
            }

            int cantidadPasajeros = Convert.ToInt32(ddlPasajeros.SelectedValue);
            DateTime fecha;

            if (DateTime.TryParse(txtFecha.Text, out fecha))
            {
                // Validación del lado del servidor
                if (fecha.Date < DateTime.Today)
                {
                    lblMensajeBusqueda.Text = "La fecha seleccionada no puede ser en el pasado.";
                    lblMensajeBusqueda.Visible = true;
                    return;
                }

                // 1. Guardar contexto inicial de la reserva
                Session["Origen"] = origen;
                Session["Destino"] = destino;
                Session["Fecha"] = fecha;
                Session["CantidadPasajeros"] = cantidadPasajeros;

                var vuelos = _vueloService.ConsultarRutas(origen, destino, fecha, cantidadPasajeros);

                if (vuelos.Count > 0)
                {
                    rptVuelos.DataSource = vuelos;
                    rptVuelos.DataBind();
                    lblMensajeBusqueda.Visible = false;
                    mvCompra.SetActiveView(viewVuelos); // Transición
                }
                else
                {
                    rptVuelos.DataSource = null;
                    rptVuelos.DataBind();
                    lblMensajeBusqueda.Text = "No se encontraron vuelos disponibles para esta fecha.";
                    lblMensajeBusqueda.Visible = true;
                }
            }
        }

        protected void rptVuelos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                // Guardamos específicamente el vuelo elegido
                Session["VueloSeleccionado"] = e.CommandArgument.ToString();

                // Llamaríamos al método que dibuja los campos de texto según la cantidad
                CargarFormulariosPasajeros(); 
                
                // Transición ininterrumpida
                mvCompra.SetActiveView(viewPasajeros);
            }
        }

        private void CargarFormulariosPasajeros()
        {
            int cantidad = Convert.ToInt32(Session["CantidadPasajeros"]);
            
            // Generamos un arreglo falso solo para que el Repeater itere 'cantidad' de veces
            int[] iterador = new int[cantidad];
            
            rptPasajeros.DataSource = iterador;
            rptPasajeros.DataBind();
        }

        protected void btnContinuarAsientos_Click(object sender, EventArgs e)
        {
            lblErrorPasajeros.Visible = false;
            List<Pasajero> listaPasajeros = new List<Pasajero>();
            HashSet<string> documentosIngresados = new HashSet<string>();

            // Recorremos cada formulario generado por el Repeater
            foreach (RepeaterItem item in rptPasajeros.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    // 1. Extraemos los controles usando FindControl
                    TextBox txtNombre = (TextBox)item.FindControl("txtNombre");
                    TextBox txtApellido = (TextBox)item.FindControl("txtApellido");
                    TextBox txtDocumento = (TextBox)item.FindControl("txtDocumento");
                    TextBox txtFechaNacimiento = (TextBox)item.FindControl("txtFechaNacimiento");
                    DropDownList ddlNacionalidad = (DropDownList)item.FindControl("ddlNacionalidad");
                    TextBox txtTelefono = (TextBox)item.FindControl("txtTelefono");
                    TextBox txtCorreo = (TextBox)item.FindControl("txtCorreo");

                    // 2. Mapeamos los datos al objeto de la BD
                    Pasajero pax = new Pasajero
                    {
                        // Generamos un código de exactamente 10 caracteres
                        CodPasajero = "PAX" + Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper(), 
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        NumDocumento = txtDocumento.Text.Trim(),
                        // Se asume que el HTML5 Date siempre manda un formato válido, pero en prod se valida con TryParse
                        FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text), 
                        Nacionalidad = ddlNacionalidad.SelectedValue,
                        Telefono = txtTelefono.Text.Trim(),
                        Correo = txtCorreo.Text.Trim()
                    };

                    // Validación: No pueden haber pasajeros duplicados en la misma reserva
                    if (documentosIngresados.Contains(pax.NumDocumento))
                    {
                        lblErrorPasajeros.Text = $"El documento {pax.NumDocumento} está duplicado. No puedes registrar a la misma persona más de una vez en esta reserva.";
                        lblErrorPasajeros.Visible = true;
                        return; // Detiene el avance y muestra el error
                    }
                    documentosIngresados.Add(pax.NumDocumento);

                    // 3. Añadimos el pasajero a la lista de esta compra
                    listaPasajeros.Add(pax);
                }
            }

            // 4. Guardamos la lista consolidada en Sesión
            Session["DatosPasajeros"] = listaPasajeros;

            // 5. Preparamos la Fase 4 y avanzamos
            CargarMapaAsientos();
            mvCompra.SetActiveView(viewAsientos);
        }

        #region FASE 4: SELECCIÓN DE ASIENTOS

        private void CargarMapaAsientos()
        {
            // Variables estandarizadas de sesión
            int idVuelo = Convert.ToInt32(Session["VueloSeleccionado"]);
            List<Pasajero> pasajeros = (List<Pasajero>)Session["DatosPasajeros"];
            int cantidadPasajeros = pasajeros.Count;

            // Configuración para el JS
            hfCantidadPasajeros.Value = cantidadPasajeros.ToString();
            hfAsientosSeleccionados.Value = ""; 

            // Repeater Izquierdo (Pasajeros)
            rptAsignacionPasajeros.DataSource = pasajeros;
            rptAsignacionPasajeros.DataBind();

            // Repeater Derecho (Mapa desde el Servicio)
            var mapaAsientos = _asientoService.ObtenerMapaVuelo(idVuelo); 
            rptMapaAvion.DataSource = mapaAsientos;
            rptMapaAvion.DataBind();
        }

        protected void btnContinuarResumen_Click(object sender, EventArgs e)
        {
            lblErrorAsientos.Visible = false;
            
            int idVuelo = Convert.ToInt32(Session["VueloSeleccionado"]);
            int cantidadPasajeros = Convert.ToInt32(Session["CantidadPasajeros"]);
            
            string seleccionRaw = hfAsientosSeleccionados.Value;
            List<string> asientosElegidos = new List<string>(seleccionRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            
            // Validación de integridad de la petición web
            if (asientosElegidos.Count != cantidadPasajeros)
            {
                lblErrorAsientos.Text = "La cantidad de asientos no coincide con los pasajeros.";
                lblErrorAsientos.Visible = true;
                return;
            }

            // VALIDACIÓN DEFINITIVA DE CONCURRENCIA
            // Verifica en tiempo real si otro usuario tomó el asiento en los últimos segundos
            bool siguenDisponibles = _asientoService.ValidarDisponibilidad(idVuelo, asientosElegidos);

            if (siguenDisponibles)
            {
                // Todo está perfecto, pasamos a memoria
                Session["AsientosSeleccionados"] = asientosElegidos;
                
                // Transición a Fase 5
                MostrarResumen(); 
                mvCompra.SetActiveView(viewResumen);
            }
            else
            {
                // Alguien más tomó el asiento. Recargamos el mapa y avisamos.
                lblErrorAsientos.Text = "Uno o más asientos seleccionados acaban de ser ocupados. Por favor, selecciona otros asientos.";
                lblErrorAsientos.Visible = true;
                CargarMapaAsientos(); // Refresca el mapa con la nueva realidad de la BD
            }
        }

        protected void btnVolverPasajeros_Click(object sender, EventArgs e)
        {
            lblErrorAsientos.Visible = false;
            mvCompra.SetActiveView(viewPasajeros);
        }

        #endregion

        #region FASE 5: RESUMEN Y PAGO

        private void MostrarResumen()
        {
            // 1. Recuperamos toda la data de la sesión
            int idVuelo = Convert.ToInt32(Session["VueloSeleccionado"]);
            List<Pasajero> pasajeros = (List<Pasajero>)Session["DatosPasajeros"];
            List<string> asientos = (List<string>)Session["AsientosSeleccionados"];
            
            // 2. Cargamos el detalle del vuelo
            var vuelo = _vueloService.ObtenerDetallePorId(idVuelo);
            lblRutaResumen.Text = $"{vuelo.CodigoOrigen} → {vuelo.CodigoDestino}";
            lblVueloResumen.Text = vuelo.NumVuelo;
            lblFechaResumen.Text = vuelo.Fecha.ToString("dd MMMM yyyy");
            lblHoraResumen.Text = vuelo.HoraSalida.ToString(@"hh\:mm");

            // 3. Fusionamos Pasajeros y Asientos (Asumimos tarifa estática web de Bs 350)
            decimal tarifaWeb = 350.00m;
            decimal totalVenta = 0;
            List<ResumenPasajero> listaResumen = new List<ResumenPasajero>();

            for (int i = 0; i < pasajeros.Count; i++)
            {
                listaResumen.Add(new ResumenPasajero
                {
                    NombreCompleto = $"{pasajeros[i].Nombre} {pasajeros[i].Apellido}",
                    Documento = pasajeros[i].NumDocumento,
                    Asiento = asientos[i],
                    Precio = tarifaWeb
                });
                totalVenta += tarifaWeb;
            }

            // 4. Bindeamos a la UI
            rptResumenPasajeros.DataSource = listaResumen;
            rptResumenPasajeros.DataBind();
            
            lblTotal.Text = totalVenta.ToString("N2");
        }

        protected void lnkVolverAsientos_Click(object sender, EventArgs e)
        {
            mvCompra.SetActiveView(viewAsientos);
        }

        protected void btnPagar_Click(object sender, EventArgs e)
        {
            try
            {
                int idVuelo = Convert.ToInt32(Session["VueloSeleccionado"]);
                List<Pasajero> pasajeros = (List<Pasajero>)Session["DatosPasajeros"];
                List<string> asientos = (List<string>)Session["AsientosSeleccionados"];
                string metodoPago = ddlMetodoPago.SelectedValue; // Ej: "TARJETA_CREDITO"
                
                // Instanciamos el servicio de reserva
                var _reservaService = new ReservaService();
                
                // Ejecutamos la transacción y recuperamos el PNR
                string pnrGenerado = _reservaService.EjecutarCheckoutWeb(idVuelo, pasajeros, asientos, "TAR01", metodoPago);

                if (!string.IsNullOrEmpty(pnrGenerado))
                {
                    // Si llegó aquí, el COMMIT fue exitoso. Limpiamos la memoria de la compra activa.
                    Session.Remove("VueloSeleccionado");
                    Session.Remove("DatosPasajeros");
                    Session.Remove("AsientosSeleccionados");
                    Session.Remove("CantidadPasajeros");
                    
                    // Mostramos el código en la pantalla final
                    lblCodigoReserva.Text = pnrGenerado;

                    // Pasamos a la última pantalla
                    mvCompra.SetActiveView(viewConfirmacion);
                }
            }
            catch (Exception ex)
            {
                lblErrorPago.Text = $"Error en el pago: {ex.Message}";
                lblErrorPago.Visible = true;
            }
        }

        #endregion

        #region FASE 7: CONFIRMACIÓN

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            // Limpiamos los campos del buscador por si quieren hacer una búsqueda totalmente nueva
            txtFecha.Text = "";
            ddlPasajeros.SelectedIndex = 0;
            
            // Regresamos el MultiView a la vista inicial (Fase 1)
            mvCompra.ActiveViewIndex = 0;
        }

        #endregion

        protected void btnVolverBuscador_Click(object sender, EventArgs e)
        {
            mvCompra.SetActiveView(viewBusqueda);
        }

        protected void btnVolverVuelos_Click(object sender, EventArgs e)
        {
            mvCompra.SetActiveView(viewVuelos);
        }

        #endregion
    }
}
