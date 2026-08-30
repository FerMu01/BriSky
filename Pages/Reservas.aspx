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
                          DataKeyNames="CodReserva">
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
                </Columns>
            </asp:GridView>
        </div>

        </div>
    </div>
</asp:Content>
