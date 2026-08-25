La consigna solamente dice:

“En base a las tareas anteriores, implementar una BD OO. Usar el lenguaje de interfaz y DBMS de tu preferencia.”

Ustedes tienen:

Modelo OO: el UML que compartiste.
Implementación: SQL Server.
Interfaz: C# + ASP.NET Web Forms.
Acceso a BD: ADO.NET.
Clases OO: Models.
Herencia del UML: Empleado → Piloto, TripulanteCabina, EmpleadoOficina, etc.
Relaciones del UML: implementadas mediante relaciones entre tablas.
Operaciones: DAO + Services + ADO.NET.
Interfaz: las páginas .aspx que estamos planificando.


ESTRUCTURA DEL PROYECTO
BriSky/
│
├── App_Start/
│   └── BundleConfig.cs
│
├── Models/
│   │
│   ├── Ubicaciones/
│   │   ├── Ciudad.cs
│   │   ├── Aeropuerto.cs
│   │   └── Oficina.cs
│   │
│   ├── Personal/
│   │   ├── Area.cs
│   │   ├── Empleado.cs
│   │   ├── EmpleadoOficina.cs
│   │   ├── Piloto.cs
│   │   ├── TripulanteCabina.cs
│   │   └── PersonalMantenimiento.cs
│   │
│   ├── Flota/
│   │   ├── ModeloAvion.cs
│   │   ├── Avion.cs
│   │   └── CompatibilidadAeropuertoModelo.cs
│   │
│   ├── Operaciones/
│   │   ├── Mantenimiento.cs
│   │   ├── Ruta.cs
│   │   ├── Vuelo.cs
│   │   └── AsignacionTripulacion.cs
│   │
│   └── Comercial/
│       ├── Tarifa.cs
│       ├── Pasajero.cs
│       ├── Reserva.cs
│       ├── ReservaOficina.cs
│       ├── ReservaInternet.cs
│       ├── Boleto.cs
│       ├── Asiento.cs
│       ├── Equipaje.cs
│       └── Pago.cs
│
├── Data/
│   │
│   ├── Conexion.cs
│   │
│   ├── Ubicaciones/
│   │   ├── CiudadDAO.cs
│   │   ├── AeropuertoDAO.cs
│   │   └── OficinaDAO.cs
│   │
│   ├── Personal/
│   │   ├── AreaDAO.cs
│   │   ├── EmpleadoDAO.cs
│   │   ├── EmpleadoOficinaDAO.cs
│   │   ├── PilotoDAO.cs
│   │   ├── TripulanteCabinaDAO.cs
│   │   └── PersonalMantenimientoDAO.cs
│   │
│   ├── Flota/
│   │   ├── ModeloAvionDAO.cs
│   │   ├── AvionDAO.cs
│   │   └── CompatibilidadAeropuertoModeloDAO.cs
│   │
│   ├── Operaciones/
│   │   ├── MantenimientoDAO.cs
│   │   ├── RutaDAO.cs
│   │   ├── VueloDAO.cs
│   │   └── AsignacionTripulacionDAO.cs
│   │
│   └── Comercial/
│       ├── TarifaDAO.cs
│       ├── PasajeroDAO.cs
│       ├── ReservaDAO.cs
│       ├── ReservaOficinaDAO.cs
│       ├── ReservaInternetDAO.cs
│       ├── BoletoDAO.cs
│       ├── AsientoDAO.cs
│       ├── EquipajeDAO.cs
│       └── PagoDAO.cs
│
├── Services/
│   │
│   ├── Ubicaciones/
│   │   ├── CiudadService.cs
│   │   ├── AeropuertoService.cs
│   │   └── OficinaService.cs
│   │
│   ├── Personal/
│   │   ├── AreaService.cs
│   │   ├── EmpleadoService.cs
│   │   ├── PilotoService.cs
│   │   ├── TripulanteCabinaService.cs
│   │   └── PersonalMantenimientoService.cs
│   │
│   ├── Flota/
│   │   ├── ModeloAvionService.cs
│   │   ├── AvionService.cs
│   │   └── CompatibilidadAeropuertoModeloService.cs
│   │
│   ├── Operaciones/
│   │   ├── MantenimientoService.cs
│   │   ├── RutaService.cs
│   │   ├── VueloService.cs
│   │   └── TripulacionService.cs
│   │
│   └── Comercial/
│       ├── TarifaService.cs
│       ├── PasajeroService.cs
│       ├── ReservaService.cs
│       ├── BoletoService.cs
│       ├── AsientoService.cs
│       ├── EquipajeService.cs
│       └── PagoService.cs
│
├── Pages/
│   │
│   ├── Ubicaciones.aspx
│   ├── Ubicaciones.aspx.cs
│   │
│   ├── Empleados.aspx
│   ├── Empleados.aspx.cs
│   │
│   ├── Flota.aspx
│   ├── Flota.aspx.cs
│   │
│   ├── Mantenimiento.aspx
│   ├── Mantenimiento.aspx.cs
│   │
│   ├── Rutas.aspx
│   ├── Rutas.aspx.cs
│   │
│   ├── Vuelos.aspx
│   ├── Vuelos.aspx.cs
│   │
│   ├── Tripulacion.aspx
│   ├── Tripulacion.aspx.cs
│   │
│   ├── Tarifas.aspx
│   ├── Tarifas.aspx.cs
│   │
│   ├── Pasajeros.aspx
│   ├── Pasajeros.aspx.cs
│   │
│   ├── Reservas.aspx
│   ├── Reservas.aspx.cs
│   │
│   ├── Boletos.aspx
│   └── Boletos.aspx.cs
│
├── Content/
│   ├── Site.css
│   ├── Sidebar.css
│   ├── Dashboard.css
│   ├── Forms.css
│   ├── Tables.css
│   └── Responsive.css
│
├── Scripts/
│   ├── site.js
│   ├── sidebar.js
│   ├── empleados.js
│   ├── reservas.js
│   ├── vuelos.js
│   └── boletos.js
│
├── Database/
│   ├── 01_CrearBaseDatos.sql
│   ├── 02_CrearTablas.sql
│   ├── 03_Restricciones.sql
│   ├── 04_DatosIniciales.sql
│   ├── 05_Procedimientos.sql
│   ├── 06_Funciones.sql
│   ├── 07_Triggers.sql
│   ├── 08_Vistas.sql
│   └── 09_Pruebas.sql
│
├── Default.aspx
├── Default.aspx.cs
│
├── Site.Master
├── Site.Master.cs
│
├── Site.Mobile.Master
├── Site.Mobile.Master.cs
│
├── Global.asax
├── Global.asax.cs
│
├── Web.config
├── Web.Debug.config
├── Web.Release.config
│
├── Bundle.config
│
├── BriSky.csproj
└── BriSky.sln