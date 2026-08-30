<%@ Page Title="Comprar Vuelo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Compra.aspx.cs" Inherits="BriSky.Pages.Cliente.Compra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .seat { width: 35px; height: 35px; line-height: 35px; text-align: center; border-radius: 6px 6px 2px 2px; font-size: 0.75em; font-weight: bold; user-select: none; }
        .seat.available { cursor: pointer; background-color: #ecf0f1; color: #2c3e50; border: 1px solid #bdc3c7; }
        .seat.occupied { cursor: not-allowed; background-color: #bdc3c7; color: #fff; border: 1px solid #95a5a6; }

        /* Corrección de contraste para los paneles de fondo blanco en Compra.aspx */
        .resumen-container, .resumen-container p, .resumen-container span, .resumen-container div,
        .tarjeta-pasajero, .tarjeta-pasajero label,
        .buscador-moderno, .buscador-moderno label {
            color: #1e293b !important;
        }

        /* Solo las filas blancas de pasajero necesitan texto oscuro */
        .pasajero-asiento-row, .pasajero-asiento-row p, .pasajero-asiento-row span, .pasajero-asiento-row small, .pasajero-asiento-row strong {
            color: #1e293b !important;
        }
    </style>
    <div class="compra-container" style="padding: 20px;">
        <asp:MultiView ID="mvCompra" runat="server" ActiveViewIndex="0">
            
            <!-- ETAPA 1: BUSCADOR -->
            <asp:View ID="viewBusqueda" runat="server">
                <div class="buscador-moderno" style="max-width: 800px; margin: 40px auto; background: rgba(255, 255, 255, 0.95); padding: 40px; border-radius: 16px; box-shadow: 0 10px 40px rgba(0,0,0,0.08); border: 1px solid rgba(220,225,230,0.8);">
                    <h2 style="color: #003366; text-align: center; margin-bottom: 30px; font-weight: 700; font-size: 2.2em;">¿A dónde quieres viajar?</h2>
                    
                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px; margin-bottom: 25px;">
                        <div class="form-group">
                            <label style="font-weight: 600; color: #555; display: block; margin-bottom: 8px;">Origen</label>
                            <asp:DropDownList ID="ddlOrigen" runat="server" CssClass="form-control" style="width: 100%; padding: 14px; border-radius: 8px; border: 1px solid #dcdde1; font-size: 1.05em;"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label style="font-weight: 600; color: #555; display: block; margin-bottom: 8px;">Destino</label>
                            <asp:DropDownList ID="ddlDestino" runat="server" CssClass="form-control" style="width: 100%; padding: 14px; border-radius: 8px; border: 1px solid #dcdde1; font-size: 1.05em;"></asp:DropDownList>
                        </div>
                    </div>

                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px; margin-bottom: 30px;">
                        <div class="form-group">
                            <label style="font-weight: 600; color: #555; display: block; margin-bottom: 8px;">Fecha de Salida</label>
                            <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control" style="width: 100%; padding: 14px; border-radius: 8px; border: 1px solid #dcdde1; font-size: 1.05em;"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label style="font-weight: 600; color: #555; display: block; margin-bottom: 8px;">Pasajeros</label>
                            <asp:DropDownList ID="ddlPasajeros" runat="server" CssClass="form-control" style="width: 100%; padding: 14px; border-radius: 8px; border: 1px solid #dcdde1; font-size: 1.05em;">
                                <asp:ListItem Value="1" Text="1 Pasajero"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2 Pasajeros"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3 Pasajeros"></asp:ListItem>
                                <asp:ListItem Value="4" Text="4 Pasajeros"></asp:ListItem>
                                <asp:ListItem Value="5" Text="5 Pasajeros"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <asp:Label ID="lblMensajeBusqueda" runat="server" ForeColor="#e74c3c" Font-Bold="true" Visible="false" style="display: block; text-align: center; margin-bottom: 15px; font-size: 1.1em;"></asp:Label>
                    
                    <div style="text-align: center;">
                        <asp:Button ID="btnBuscar" runat="server" Text="BUSCAR VUELOS ➔" CssClass="btn-primary" OnClick="btnBuscar_Click" style="background-color: #E67E22; color: white; border: none; padding: 16px 45px; font-size: 1.15em; font-weight: bold; border-radius: 30px; cursor: pointer; box-shadow: 0 6px 20px rgba(230, 126, 34, 0.4); transition: transform 0.2s, box-shadow 0.2s;" />
                    </div>
                </div>
            </asp:View>

            <!-- ETAPA 2: RESULTADOS DE VUELOS -->
            <asp:View ID="viewVuelos" runat="server">
                <h2>Vuelos Disponibles</h2>
                <asp:Repeater ID="rptVuelos" runat="server" OnItemCommand="rptVuelos_ItemCommand">
                    <ItemTemplate>
                        <div class="flight-card" style="display: flex; border: 1px solid #e0e6ed; border-radius: 12px; margin-bottom: 20px; font-family: Arial, sans-serif; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.05);">
                            
                            <!-- Sección Izquierda: Detalles del Vuelo -->
                            <div class="flight-info" style="flex: 2; padding: 20px; display: flex; justify-content: space-between; align-items: center; background-color: #ffffff;">
                                
                                <div class="origin" style="text-align: left;">
                                    <span style="color: #555; font-size: 0.9em;"><%# Eval("Fecha", "{0:ddd dd MMM yyyy}") %></span><br/>
                                    <span style="font-size: 1.8em; font-weight: bold; color: #003366;"><%# Eval("HoraSalida", "{0:hh\\:mm}") %></span><br/>
                                    <span style="font-size: 1.2em; font-weight: bold; color: #003366;"><%# Eval("CodigoOrigen") %></span><br/>
                                    <span style="color: #777; font-size: 0.85em;"><%# Eval("AeropuertoOrigen") %></span>
                                </div>

                                <div class="path" style="text-align: center; color: #0078D7;">
                                    <span style="font-weight: bold; font-size: 0.9em;">Vuelo Directo</span><br/>
                                    <span style="letter-spacing: 2px;">───── ✈ ─────</span><br/>
                                    <span style="color: #777; font-size: 0.85em;">Vuelo <%# Eval("NumVuelo") %> | <%# Eval("ModeloAvion") %></span>
                                </div>

                                <div class="destination" style="text-align: right;">
                                    <span style="color: #555; font-size: 0.9em;"><%# Eval("Fecha", "{0:ddd dd MMM yyyy}") %></span><br/>
                                    <span style="font-size: 1.8em; font-weight: bold; color: #003366;"><%# Eval("HoraLlegada", "{0:hh\\:mm}") %></span><br/>
                                    <span style="font-size: 1.2em; font-weight: bold; color: #003366;"><%# Eval("CodigoDestino") %></span><br/>
                                    <span style="color: #777; font-size: 0.85em;"><%# Eval("AeropuertoDestino") %></span>
                                </div>
                            </div>

                            <!-- Sección Derecha: Precio y Selección -->
                            <div class="flight-action" style="flex: 1; padding: 20px; background-color: #f0f7fd; border-left: 1px solid #e0e6ed; display: flex; flex-direction: column; justify-content: center;">
                                <div style="text-align: right; margin-bottom: 10px;">
                                    <span style="background-color: #003366; color: white; padding: 4px 8px; border-radius: 4px; font-size: 0.8em; font-weight: bold;">Económica Flex</span>
                                </div>
                                <span style="color: #555; font-size: 0.9em;">Desde</span>
                                <span style="font-size: 1.6em; font-weight: bold; color: #003366; margin-bottom: 5px;">BOB <%# Eval("PrecioBase", "{0:N0}") %></span>
                                <span style="color: #28B463; font-size: 0.85em; font-weight: bold; margin-bottom: 15px;"><%# Eval("AsientosDisponibles") %> asientos disponibles</span>
                                
                                <asp:Button ID="btnSeleccionar" runat="server" Text="SELECCIONAR" 
                                            CommandName="Seleccionar" 
                                            CommandArgument='<%# Eval("IdVuelo") %>' 
                                            style="background-color: #003366; color: white; border: none; padding: 12px; border-radius: 6px; font-weight: bold; cursor: pointer; width: 100%;" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                
                <asp:Label ID="lblMensajeVuelos" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <br />
                <asp:Button ID="btnVolverBuscador" runat="server" Text="Volver" OnClick="btnVolverBuscador_Click" CssClass="btn btn-secondary" />
            </asp:View>

            <!-- ETAPA 3: PASAJEROS -->
            <asp:View ID="viewPasajeros" runat="server">
                <div class="header-pasajeros" style="margin-bottom: 20px;">
                    <h2>Datos de los Pasajeros</h2>
                    <p>Ingresa la información tal como aparece en el documento de identidad.</p>
                </div>

                <!-- El Repeater multiplicará este bloque N veces -->
                <asp:Repeater ID="rptPasajeros" runat="server">
                    <ItemTemplate>
                        <div class="tarjeta-pasajero" style="border: 1px solid #e0e6ed; padding: 25px; border-radius: 8px; margin-bottom: 25px; background-color: #ffffff; box-shadow: 0 2px 4px rgba(0,0,0,0.02);">
                            <h4 style="color: #003366; border-bottom: 2px solid #E67E22; padding-bottom: 8px; margin-top: 0;">
                                Pasajero <%# Container.ItemIndex + 1 %>
                            </h4>
                            
                            <div style="display: flex; gap: 20px; margin-top: 20px;">
                                <div style="flex: 1;">
                                    <label style="font-weight: bold; color: #555;">Nombre(s)</label>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Width="100%" required="true"></asp:TextBox>
                                </div>
                                <div style="flex: 1;">
                                    <label style="font-weight: bold; color: #555;">Apellido(s)</label>
                                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" Width="100%" required="true"></asp:TextBox>
                                </div>
                            </div>

                            <div style="display: flex; gap: 20px; margin-top: 20px;">
                                <div style="flex: 1;">
                                    <label style="font-weight: bold; color: #555;">Número de Documento</label>
                                    <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control" Width="100%" required="true"></asp:TextBox>
                                </div>
                                <div style="flex: 1;">
                                    <label style="font-weight: bold; color: #555;">Fecha de Nacimiento</label>
                                    <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" CssClass="form-control" Width="100%" required="true"></asp:TextBox>
                                </div>
                            </div>

                            <div style="display: flex; gap: 20px; margin-top: 20px;">
                                <div style="flex: 1;">
                                    <label style="font-weight: bold; color: #555;">Nacionalidad</label>
                                    <asp:DropDownList ID="ddlNacionalidad" runat="server" CssClass="form-control" Width="100%">
                                        <asp:ListItem Value="Boliviana" Text="Boliviana"></asp:ListItem>
                                        <asp:ListItem Value="Argentina" Text="Argentina"></asp:ListItem>
                                        <asp:ListItem Value="Chilena" Text="Chilena"></asp:ListItem>
                                        <asp:ListItem Value="Peruana" Text="Peruana"></asp:ListItem>
                                        <asp:ListItem Value="Otra" Text="Otra"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div style="flex: 1;">
                                    <label style="font-weight: bold; color: #555;">Teléfono (Opcional)</label>
                                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div style="flex: 1;">
                                    <label style="font-weight: bold; color: #555;">Correo Electrónico</label>
                                    <asp:TextBox ID="txtCorreo" runat="server" TextMode="Email" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <!-- Botonera de navegación -->
                <div style="text-align: right; margin-top: 30px; border-top: 1px solid #e0e6ed; padding-top: 20px;">
                    <asp:Label ID="lblErrorPasajeros" runat="server" ForeColor="#e74c3c" Font-Bold="true" Visible="false" style="display: block; text-align: right; margin-bottom: 15px; font-size: 1.1em;"></asp:Label>
                    
                    <asp:Button ID="btnVolverVuelos" runat="server" Text="Volver a Vuelos" OnClick="btnVolverVuelos_Click" CssClass="btn-secondary" style="padding: 12px 24px; margin-right: 15px; background-color: #f8f9fa; border: 1px solid #ddd; cursor: pointer; border-radius: 4px;" formnovalidate="formnovalidate" />
                    
                    <asp:Button ID="btnContinuarAsientos" runat="server" Text="Continuar a Asientos" OnClick="btnContinuarAsientos_Click" CssClass="btn-primary" style="background-color: #003366; color: white; border: none; padding: 12px 24px; border-radius: 4px; font-weight: bold; cursor: pointer;" />
                </div>
            </asp:View>

            <!-- ETAPA 4: ASIENTOS -->
            <asp:View ID="viewAsientos" runat="server">
                <div class="asientos-container" style="display: flex; gap: 40px; justify-content: center; margin-top: 20px;">
                    
                    <!-- PANEL IZQUIERDO: Asignación de Pasajeros -->
                    <div class="panel-asignacion" style="flex: 1; max-width: 300px;">
                        <h3>Asignación de Asientos</h3>
                        <p style="color: #cbd5e1; font-size: 0.9em;">Selecciona un asiento en el mapa para el pasajero correspondiente.</p>
                        
                        <!-- Etiqueta de error de concurrencia -->
                        <asp:Label ID="lblErrorAsientos" runat="server" ForeColor="#e74c3c" Font-Bold="true" Visible="false" style="display: block; margin-bottom: 15px;"></asp:Label>

                        <div id="lista-pasajeros-ui">
                            <asp:Repeater ID="rptAsignacionPasajeros" runat="server">
                                <ItemTemplate>
                                    <div class="pasajero-asiento-row" data-index="<%# Container.ItemIndex %>" style="display: flex; justify-content: space-between; padding: 15px; border: 1px solid #ddd; border-radius: 8px; margin-bottom: 10px; background: #fff;">
                                        <div>
                                            <strong style="color: #003366;">Pasajero <%# Container.ItemIndex + 1 %></strong><br />
                                            <small><%# Eval("Nombre") %> <%# Eval("Apellido") %></small>
                                        </div>
                                        <div class="asiento-asignado" style="font-size: 1.2em; font-weight: bold; color: #E67E22; display: flex; align-items: center;">
                                            --
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <!-- Controles ocultos para la lógica JS y C# -->
                        <asp:HiddenField ID="hfCantidadPasajeros" runat="server" />
                        <asp:HiddenField ID="hfAsientosSeleccionados" runat="server" />
                        
                        <div style="margin-top: 30px;">
                            <asp:Button ID="btnVolverPasajeros" runat="server" Text="Volver" OnClick="btnVolverPasajeros_Click" CssClass="btn-secondary" style="width: 100%; margin-bottom: 10px; padding: 12px; border-radius: 6px;" />
                            <asp:Button ID="btnContinuarResumen" runat="server" Text="Continuar a Resumen" OnClick="btnContinuarResumen_Click" CssClass="btn-primary" style="background-color: #003366; color: white; border: none; padding: 12px; width: 100%; border-radius: 6px; font-weight: bold; cursor: pointer;" OnClientClick="return validarAsientos();" />
                        </div>
                    </div>

                    <!-- PANEL DERECHO: El Mapa del Avión -->
                    <div class="panel-avion">
                        <!-- Punta del avión -->
                        <div class="avion-nose" style="width: 260px; height: 100px; background: #e0e6ed; border-radius: 130px 130px 0 0; margin: 0 auto; border: 2px solid #bdc3c7; border-bottom: none; position: relative;">
                            <div style="position: absolute; bottom: 10px; width: 100%; text-align: center; color: #7f8c8d; font-size: 0.8em; font-weight: bold;">FRENTE</div>
                        </div>
                        
                        <!-- Fuselaje -->
                        <div class="avion-fuselaje" style="width: 260px; background: #f8f9fa; margin: 0 auto; padding: 20px 10px; border-left: 2px solid #bdc3c7; border-right: 2px solid #bdc3c7; box-shadow: inset 0 0 10px rgba(0,0,0,0.05);">
                            
                            <div style="display: grid; grid-template-columns: 35px 35px 35px 20px 35px 35px 35px; gap: 5px; text-align: center; font-weight: bold; color: #7f8c8d; margin-bottom: 10px;">
                                <div>A</div><div>B</div><div>C</div><div></div><div>D</div><div>E</div><div>F</div>
                            </div>

                            <div class="seat-grid" style="display: grid; grid-template-columns: 35px 35px 35px 20px 35px 35px 35px; gap: 5px 5px; justify-items: center;">
                                <asp:Repeater ID="rptMapaAvion" runat="server">
                                    <ItemTemplate>
                                        <!-- Dibujamos el asiento y usamos el booleano Disponible -->
                                        <div class='seat <%# Convert.ToBoolean(Eval("Disponible")) ? "available" : "occupied" %>' 
                                             data-seat='<%# Eval("NumAsiento") %>'>
                                            <%# Eval("NumAsiento") %>
                                        </div>
                                        
                                        <!-- Pasillo simulado para distribución 3-3 -->
                                        <%# (Container.ItemIndex + 1) % 6 == 3 ? "<div></div>" : "" %>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        
                        <!-- Cola -->
                        <div class="avion-tail" style="width: 260px; height: 30px; background: #e0e6ed; margin: 0 auto; border: 2px solid #bdc3c7; border-top: none; border-radius: 0 0 50px 50px;"></div>
                        
                        <!-- Leyenda -->
                        <div style="display: flex; justify-content: center; gap: 15px; margin-top: 15px; font-size: 0.85em; color: #555;">
                            <div style="display: flex; align-items: center; gap: 5px;"><div style="width:15px; height:15px; background: #ecf0f1; border: 1px solid #bdc3c7; border-radius: 3px;"></div> Disponible</div>
                            <div style="display: flex; align-items: center; gap: 5px;"><div style="width:15px; height:15px; background: #bdc3c7; border-radius: 3px;"></div> Ocupado</div>
                            <div style="display: flex; align-items: center; gap: 5px;"><div style="width:15px; height:15px; background: #2ecc71; border-radius: 3px;"></div> Seleccionado</div>
                        </div>
                    </div>
                </div>

                <script>
                    document.addEventListener("DOMContentLoaded", function () {
                        const seats = document.querySelectorAll('.seat.available');
                        const hiddenField = document.getElementById('<%= hfAsientosSeleccionados.ClientID %>');
                        const hfCantidad = document.getElementById('<%= hfCantidadPasajeros.ClientID %>');
                        
                        // Si no hay hfCantidad o está vacío, no hacer nada para evitar errores
                        if (!hfCantidad || !hfCantidad.value) return;
                        
                        const maxSeats = parseInt(hfCantidad.value);
                        const passengerRows = document.querySelectorAll('.pasajero-asiento-row');
                        
                        let selectionMap = []; 

                        seats.forEach(seat => {
                            seat.addEventListener('click', function () {
                                const seatNum = this.getAttribute('data-seat');
                                const existingIndex = selectionMap.findIndex(s => s.seat === seatNum);

                                if (existingIndex > -1) {
                                    this.style.backgroundColor = '#ecf0f1';
                                    this.style.color = '#2c3e50';
                                    const pIndex = selectionMap[existingIndex].index;
                                    passengerRows[pIndex].querySelector('.asiento-asignado').innerText = '--';
                                    selectionMap.splice(existingIndex, 1);
                                } else {
                                    if (selectionMap.length < maxSeats) {
                                        let nextAvailableIndex = -1;
                                        for (let i = 0; i < maxSeats; i++) {
                                            if (!selectionMap.some(s => s.index === i)) {
                                                nextAvailableIndex = i;
                                                break;
                                            }
                                        }
                                        if (nextAvailableIndex > -1) {
                                            this.style.backgroundColor = '#2ecc71';
                                            this.style.color = 'white';
                                            passengerRows[nextAvailableIndex].querySelector('.asiento-asignado').innerText = seatNum;
                                            selectionMap.push({ index: nextAvailableIndex, seat: seatNum });
                                        }
                                    } else {
                                        alert(`Ya has seleccionado los ${maxSeats} asientos permitidos.`);
                                    }
                                }
                                const orderedSeats = [...selectionMap].sort((a, b) => a.index - b.index).map(s => s.seat);
                                hiddenField.value = orderedSeats.join(',');
                            });
                        });
                    });

                    function validarAsientos() {
                        const hiddenField = document.getElementById('<%= hfAsientosSeleccionados.ClientID %>');
                        const maxSeats = parseInt(document.getElementById('<%= hfCantidadPasajeros.ClientID %>').value);
                        const currentSelected = hiddenField.value ? hiddenField.value.split(',').length : 0;
                        
                        if (currentSelected !== maxSeats) {
                            alert(`Debes asignar un asiento a cada uno de los ${maxSeats} pasajeros antes de continuar.`);
                            return false; 
                        }
                        return true;
                    }
                </script>
            </asp:View>

            <!-- ETAPA 5: RESUMEN -->
            <asp:View ID="viewResumen" runat="server">
                <div class="resumen-container" style="max-width: 600px; margin: 0 auto; background: #fff; padding: 30px; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.1);">
                    
                    <h2>Resumen de Compra</h2>
                    
                    <!-- Detalle del Vuelo -->
                    <div class="resumen-vuelo" style="border-bottom: 2px dashed #ddd; padding-bottom: 20px; margin-bottom: 20px;">
                        <h3 style="color: #003366;"><asp:Label ID="lblRutaResumen" runat="server"></asp:Label></h3>
                        <p><strong>Vuelo:</strong> <asp:Label ID="lblVueloResumen" runat="server"></asp:Label></p>
                        <p><strong>Fecha:</strong> <asp:Label ID="lblFechaResumen" runat="server"></asp:Label></p>
                        <p><strong>Hora:</strong> <asp:Label ID="lblHoraResumen" runat="server"></asp:Label></p>
                    </div>

                    <!-- Detalle de Pasajeros y Asientos -->
                    <div class="resumen-pasajeros">
                        <asp:Repeater ID="rptResumenPasajeros" runat="server">
                            <ItemTemplate>
                                <div style="display: flex; justify-content: space-between; margin-bottom: 15px;">
                                    <div>
                                        <strong>Pasajero <%# Container.ItemIndex + 1 %></strong><br />
                                        <%# Eval("NombreCompleto") %> <br />
                                        <small style="color: #666;">Asiento <%# Eval("Asiento") %></small>
                                    </div>
                                    <div style="font-weight: bold; color: #2E86C1; text-align: right;">
                                        Bs <%# Eval("Precio", "{0:N0}") %>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- Total -->
                    <div class="resumen-total" style="border-top: 2px solid #003366; padding-top: 15px; margin-top: 15px; display: flex; justify-content: space-between; font-size: 1.5em; font-weight: bold;">
                        <span>TOTAL:</span>
                        <span>Bs <asp:Label ID="lblTotal" runat="server"></asp:Label></span>
                    </div>

                    <!-- Simulación de Pago -->
                    <div class="seccion-pago" style="background: #f8f9fa; padding: 20px; border-radius: 8px; margin-top: 30px; border: 1px solid #e0e6ed;">
                        <h3 style="margin-top: 0; color: #333;">Método de Pago</h3>
                        
                        <div style="margin-bottom: 15px;">
                            <label>Método</label>
                            <asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="form-control" Width="100%" onchange="togglePago()">
                                <asp:ListItem Value="TARJETA_DEBITO" Text="Tarjeta de Débito"></asp:ListItem>
                                <asp:ListItem Value="QR" Text="Pago con QR"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <div id="pagoTarjeta" style="margin-bottom: 20px;">
                            <label>Número de Tarjeta</label>
                            <asp:TextBox ID="txtNumTarjeta" runat="server" CssClass="form-control" Width="100%" placeholder="**** **** **** ****"></asp:TextBox>
                        </div>

                        <div id="pagoQR" style="margin-bottom: 20px; text-align: center; display: none;">
                            <p style="color: #666; font-size: 0.9em; margin-bottom: 10px;">Escanea este código con la app de tu banco.</p>
                            <img src="https://upload.wikimedia.org/wikipedia/commons/d/d0/QR_code_for_mobile_English_Wikipedia.svg" alt="QR Genérico" style="width: 150px; height: 150px; border-radius: 8px; border: 1px solid #ddd; padding: 10px; background: white; margin-bottom: 15px;" />
                        </div>
                        
                        <asp:Label ID="lblErrorPago" runat="server" ForeColor="#e74c3c" Font-Bold="true" Visible="false" style="display: block; margin-bottom: 15px;"></asp:Label>
                        
                        <asp:Button ID="btnPagar" runat="server" Text="Confirmar y pagar" OnClick="btnPagar_Click" CssClass="btn-primary" style="background-color: #27ae60; color: white; border: none; padding: 15px; width: 100%; font-size: 1.1em; font-weight: bold; border-radius: 6px; cursor: pointer;" />
                        
                        <script>
                            function togglePago() {
                                var ddl = document.getElementById('<%= ddlMetodoPago.ClientID %>');
                                var btn = document.getElementById('<%= btnPagar.ClientID %>');
                                if (ddl.value === "QR") {
                                    document.getElementById('pagoTarjeta').style.display = 'none';
                                    document.getElementById('pagoQR').style.display = 'block';
                                    btn.value = "Ya pagué";
                                } else {
                                    document.getElementById('pagoTarjeta').style.display = 'block';
                                    document.getElementById('pagoQR').style.display = 'none';
                                    btn.value = "Confirmar y pagar";
                                }
                            }
                        </script>
                        
                        <div style="text-align: center; margin-top: 15px;">
                            <asp:LinkButton ID="lnkVolverAsientos" runat="server" OnClick="lnkVolverAsientos_Click" style="color: #666; text-decoration: none;">← Volver a selección de asientos</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </asp:View>

            <!-- ETAPA 6: CONFIRMACIÓN -->
            <asp:View ID="viewConfirmacion" runat="server">
                <div class="confirmacion-container" style="max-width: 500px; margin: 40px auto; text-align: center; background: #ffffff; padding: 40px; border-radius: 12px; box-shadow: 0 10px 25px rgba(0,0,0,0.05); border-top: 5px solid #2ecc71;">
                    
                    <!-- Icono de Éxito -->
                    <div style="width: 80px; height: 80px; background: #e8f8f5; border-radius: 50%; margin: 0 auto 20px auto; display: flex; align-items: center; justify-content: center;">
                        <span style="color: #2ecc71; font-size: 40px;">✈️</span>
                    </div>
                    
                    <h2 style="color: #2c3e50; margin-bottom: 10px;">¡Compra Exitosa!</h2>
                    <p style="color: #7f8c8d; font-size: 1.1em; margin-bottom: 30px;">Tus boletos han sido emitidos correctamente.</p>
                    
                    <div class="codigo-reserva" style="background: #f8f9fa; border: 1px dashed #bdc3c7; padding: 20px; border-radius: 8px; margin-bottom: 30px;">
                        <span style="display: block; color: #7f8c8d; font-size: 0.9em; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 5px;">Código de Reserva</span>
                        <asp:Label ID="lblCodigoReserva" runat="server" style="font-size: 2em; font-weight: bold; color: #003366; letter-spacing: 3px;"></asp:Label>
                    </div>

                    <p style="color: #555; font-size: 0.9em; margin-bottom: 30px;">
                        Se ha enviado un comprobante con el detalle del itinerario al correo electrónico registrado.
                    </p>

                    <asp:Button ID="btnFinalizar" runat="server" Text="Volver al Inicio" OnClick="btnFinalizar_Click" CssClass="btn-primary" style="background-color: #003366; color: white; border: none; padding: 15px 30px; font-size: 1em; font-weight: bold; border-radius: 6px; cursor: pointer; width: 100%; transition: background 0.3s;" />
                </div>
            </asp:View>

        </asp:MultiView>
    </div>
</asp:Content>
