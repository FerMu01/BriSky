<%@ Page Title="Reservas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Reservas.aspx.cs" Inherits="Pages_Reservas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .crud-container { display: flex; gap: 20px; flex-wrap: wrap; margin-top: 15px; }
        .grid-section { flex: 1 1 60%; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); overflow-x: auto; }
        .form-section { flex: 1 1 35%; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }

        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; margin-bottom: 5px; font-weight: 500; color: #1e293b; }
        .form-control { width: 100%; padding: 8px 12px; border: 1px solid #cbd5e1; border-radius: 4px; box-sizing: border-box; font-family: inherit; }

        .btn { padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; font-weight: 500; }
        .btn-primary { background: #3b82f6; color: white; }
        .btn-primary:hover { background: #2563eb; }
        .btn-secondary { background: #94a3b8; color: white; }
        .btn-secondary:hover { background: #64748b; }

        .styled-grid { width: 100%; border-collapse: collapse; margin-top: 15px; }
        .styled-grid th, .styled-grid td { padding: 10px; text-align: left; border-bottom: 1px solid #e2e8f0; font-size: 0.9rem; }
        .styled-grid th { background-color: #f8fafc; color: #1e293b; font-weight: 600; }
        .styled-grid tr:hover { background-color: #f1f5f9; }

        .action-link { color: #3b82f6; text-decoration: none; margin-right: 10px; cursor: pointer; }
        .action-delete { color: #ef4444; }

        .alert { padding: 10px 15px; border-radius: 4px; margin-bottom: 15px; }
        .alert-error { background: #fef2f2; color: #b91c1c; border: 1px solid #f87171; }
        .alert-success { background: #ecfdf5; color: #047857; border: 1px solid #34d399; }

        .badge { padding: 4px 8px; border-radius: 12px; font-size: 0.8rem; font-weight: 600; }
        .badge-pend { background-color: #fef9c3; color: #854d0e; }
        .badge-conf { background-color: #dcfce7; color: #166534; }
        .badge-canc { background-color: #fee2e2; color: #b91c1c; }

        .panel-dinamico { padding: 15px; background-color: #f8fafc; border-left: 4px solid #3b82f6; margin-top: 10px; margin-bottom: 15px; border-radius: 4px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-bottom: 20px;">
        <h2 style="margin-top:0; color: #0f172a;">Reservas</h2>
        <p style="color: #64748b;">Gestión de reservas de pasajeros, ya sea por oficina o por internet.</p>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="crud-container">
        <!-- Columna Izquierda: Listado -->
        <div class="grid-section">
            <h3>Reservas Registradas</h3>
            <asp:GridView ID="gvReservas" runat="server" AutoGenerateColumns="False" CssClass="styled-grid"
                          DataKeyNames="CodReserva" OnRowCommand="gvReservas_RowCommand">
                <Columns>
                    <asp:BoundField DataField="CodReserva" HeaderText="Código" />
                    <asp:BoundField DataField="NombrePasajero" HeaderText="Pasajero" />
                    <asp:BoundField DataField="RutaFormateadaVuelo" HeaderText="Vuelo" />
                    <asp:BoundField DataField="NombreTarifa" HeaderText="Tarifa" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C2}" />
                    <asp:TemplateField HeaderText="Canal">
                        <ItemTemplate>
                            <%# Eval("TipoReserva").ToString() == "OFICINA" ? "Oficina (" + Eval("NombreEmpleado") + ")" : "Internet" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span class='<%# ClaseBadgeEstado(Eval("Estado").ToString()) %>'><%# Eval("Estado") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnConfirmar" runat="server" CommandName="ConfirmarReserva" CommandArgument='<%# Eval("CodReserva") %>' CssClass="action-link" ToolTip="Confirmar"><i class="fa-solid fa-check"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnCancelar" runat="server" CommandName="CancelarReserva" CommandArgument='<%# Eval("CodReserva") %>' CssClass="action-link" ToolTip="Cancelar" OnClientClick="return confirm('¿Cancelar esta reserva?');"><i class="fa-solid fa-ban"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnGenerarBoleto" runat="server" Visible='<%# Eval("TipoReserva").ToString() == "INTERNET" %>' CommandName="GenerarBoletoReserva" CommandArgument='<%# Eval("CodReserva") %>' CssClass="action-link" ToolTip="Generar Boleto"><i class="fa-solid fa-ticket"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Columna Derecha: Formulario -->
        <div class="form-section">
            <h3>Nueva Reserva</h3>

            <div class="form-group">
                <label>Código de Reserva</label>
                <asp:TextBox ID="txtCodReserva" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Pasajero</label>
                <asp:DropDownList ID="ddlPasajero" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label>Vuelo</label>
                <asp:DropDownList ID="ddlVuelo" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label>Tarifa</label>
                <asp:DropDownList ID="ddlTarifa" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <p style="color:#64748b; font-size:0.85rem;">El precio se calcula automáticamente según la tarifa seleccionada.</p>

            <div class="form-group">
                <label>Canal de Venta</label>
                <asp:DropDownList ID="ddlTipoReserva" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoReserva_SelectedIndexChanged">
                    <asp:ListItem Text="Oficina" Value="OFICINA"></asp:ListItem>
                    <asp:ListItem Text="Internet" Value="INTERNET"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Específico Reserva Oficina: registrar_venta crea reserva + boleto + pago -->
            <asp:Panel ID="pnlOficina" runat="server" CssClass="panel-dinamico">
                <div class="form-group">
                    <label>Empleado que Atiende</label>
                    <asp:DropDownList ID="ddlEmpleado" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>N° de Boleto a Emitir</label>
                    <asp:TextBox ID="txtNumBoleto" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                </div>
                <div style="display:flex; gap:10px;">
                    <div class="form-group" style="flex:1;">
                        <label>Código de Pago</label>
                        <asp:TextBox ID="txtCodPago" runat="server" CssClass="form-control" MaxLength="12"></asp:TextBox>
                    </div>
                    <div class="form-group" style="flex:1;">
                        <label>Método de Pago</label>
                        <asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Efectivo" Value="EFECTIVO"></asp:ListItem>
                            <asp:ListItem Text="Tarjeta de Crédito" Value="TARJETA_CREDITO"></asp:ListItem>
                            <asp:ListItem Text="Tarjeta de Débito" Value="TARJETA_DEBITO"></asp:ListItem>
                            <asp:ListItem Text="Transferencia" Value="TRANSFERENCIA"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <p style="color:#64748b; font-size:0.85rem; margin:0;">La venta en oficina emite el boleto y registra el pago de inmediato.</p>
            </asp:Panel>

            <!-- Específico Reserva Internet: crear_reserva_internet solo genera la reserva (queda pendiente) -->
            <asp:Panel ID="pnlInternet" runat="server" CssClass="panel-dinamico" Visible="false">
                <div class="form-group">
                    <label>IP de Origen</label>
                    <asp:TextBox ID="txtIpOrigen" runat="server" CssClass="form-control" MaxLength="45" placeholder="Se registra automáticamente si se deja en blanco"></asp:TextBox>
                </div>
                <p style="color:#64748b; font-size:0.85rem; margin:0;">Queda como reserva pendiente. El boleto se genera después con el botón <i class="fa-solid fa-ticket"></i> en la grilla, una vez confirmada.</p>
            </asp:Panel>

            <div style="margin-top: 20px; display:flex; gap:10px;">
                <asp:Button ID="btnGuardar" runat="server" Text="Registrar Reserva" CssClass="btn btn-primary" OnClick="btnGuardar_Click" style="flex:1;" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
