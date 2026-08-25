<%@ Page Title="Control de Vuelos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Vuelos.aspx.cs" Inherits="Pages_Vuelos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .crud-container { display: flex; gap: 20px; flex-wrap: wrap; margin-top: 15px; }
        .grid-section { flex: 1 1 65%; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); overflow-x: auto; }
        .form-section { flex: 1 1 30%; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }

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
        .badge-prog { background-color: #e0f2fe; color: #0369a1; }
        .badge-abrd { background-color: #fef3c7; color: #b45309; }
        .badge-vuel { background-color: #dbeafe; color: #1d4ed8; }
        .badge-ater { background-color: #dcfce7; color: #166534; }
        .badge-canc { background-color: #fee2e2; color: #b91c1c; }
        .badge-demo { background-color: #ffedd5; color: #c2410c; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-bottom: 20px;">
        <h2 style="margin-top:0; color: #0f172a;">Control de Vuelos</h2>
        <p style="color: #64748b;">Programación de vuelos, asignación de flota y control de estados operativos.</p>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="crud-container">
        <!-- Columna Izquierda: Historial -->
        <div class="grid-section">
            <h3>Vuelos Programados</h3>
            <asp:GridView ID="gvVuelos" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                          DataKeyNames="IdVuelo" OnRowCommand="gvVuelos_RowCommand">
                <Columns>
                    <asp:BoundField DataField="NumVuelo" HeaderText="Vuelo" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="RutaFormateada" HeaderText="Ruta" />
                    <asp:BoundField DataField="HoraSalida" HeaderText="Salida" DataFormatString="{0:hh\:mm}" />
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span class='<%# ObtenerClaseBadge(Eval("Estado").ToString()) %>'>
                                <%# Eval("Estado") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MatriculaAvion" HeaderText="Avión" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" CommandName="EditarVuelo" CommandArgument='<%# Eval("IdVuelo") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="EliminarVuelo" CommandArgument='<%# Eval("IdVuelo") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Está seguro de eliminar este vuelo de la programación?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Columna Derecha: Formulario -->
        <div class="form-section">
            <h3 id="lblTituloForm" runat="server">Programar Vuelo</h3>
            <asp:HiddenField ID="hdfIdVuelo" runat="server" />
            
            <div class="form-group">
                <label>Número de Vuelo (Ej: OB-100)</label>
                <asp:TextBox ID="txtNumVuelo" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Ruta (Origen - Destino)</label>
                <asp:DropDownList ID="ddlRuta" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label>Fecha del Vuelo</label>
                <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            
            <div style="display:flex; gap:10px;">
                <div class="form-group" style="flex:1;">
                    <label>Hora Salida</label>
                    <asp:TextBox ID="txtHoraSalida" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                </div>
                <div class="form-group" style="flex:1;">
                    <label>Hora Llegada</label>
                    <asp:TextBox ID="txtHoraLlegada" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <label>Asignar Avión (Opcional)</label>
                <asp:DropDownList ID="ddlAvion" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label>Estado Operativo</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                    <asp:ListItem Text="PROGRAMADO" Value="PROGRAMADO"></asp:ListItem>
                    <asp:ListItem Text="ABORDANDO" Value="ABORDANDO"></asp:ListItem>
                    <asp:ListItem Text="EN_VUELO" Value="EN_VUELO"></asp:ListItem>
                    <asp:ListItem Text="ATERRIZADO" Value="ATERRIZADO"></asp:ListItem>
                    <asp:ListItem Text="CANCELADO" Value="CANCELADO"></asp:ListItem>
                    <asp:ListItem Text="DEMORADO" Value="DEMORADO"></asp:ListItem>
                </asp:DropDownList>
            </div>
            
            <div style="margin-top: 20px; display:flex; gap:10px;">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Vuelo" CssClass="btn btn-primary" OnClick="btnGuardar_Click" style="flex:1;" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
