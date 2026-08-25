<%@ Page Title="Gestión de Tripulación" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Tripulacion.aspx.cs" Inherits="Pages_Tripulacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .filter-section { background: #f8fafc; padding: 20px; border-radius: 8px; border: 1px solid #e2e8f0; margin-bottom: 20px; }
        
        .crud-container { display: flex; gap: 20px; flex-wrap: wrap; }
        .grid-section { flex: 1 1 60%; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); overflow-x: auto; }
        .form-section { flex: 1 1 35%; background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }

        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; margin-bottom: 5px; font-weight: 500; color: #1e293b; }
        .form-control { width: 100%; padding: 8px 12px; border: 1px solid #cbd5e1; border-radius: 4px; box-sizing: border-box; font-family: inherit; }
        
        .btn { padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; font-weight: 500; }
        .btn-primary { background: #3b82f6; color: white; }
        .btn-primary:hover { background: #2563eb; }
        
        .styled-grid { width: 100%; border-collapse: collapse; margin-top: 15px; }
        .styled-grid th, .styled-grid td { padding: 10px; text-align: left; border-bottom: 1px solid #e2e8f0; font-size: 0.9rem; }
        .styled-grid th { background-color: #f8fafc; color: #1e293b; font-weight: 600; }
        .styled-grid tr:hover { background-color: #f1f5f9; }
        
        .action-link { color: #ef4444; text-decoration: none; cursor: pointer; }
        .action-link:hover { color: #dc2626; }
        
        .alert { padding: 10px 15px; border-radius: 4px; margin-bottom: 15px; }
        .alert-error { background: #fef2f2; color: #b91c1c; border: 1px solid #f87171; }
        .alert-success { background: #ecfdf5; color: #047857; border: 1px solid #34d399; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-bottom: 20px;">
        <h2 style="margin-top:0; color: #0f172a;">Asignación de Tripulación</h2>
        <p style="color: #64748b;">Asigna pilotos y tripulantes de cabina a vuelos específicos.</p>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- Filtro Maestro -->
    <div class="filter-section">
        <div class="form-group" style="margin-bottom: 0;">
            <label style="font-size: 1.1rem; color: #334155;">Seleccione un Vuelo para gestionar su tripulación:</label>
            <asp:DropDownList ID="ddlVueloFiltro" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlVueloFiltro_SelectedIndexChanged" style="max-width: 500px; font-size: 1rem;"></asp:DropDownList>
        </div>
    </div>

    <!-- Si no hay vuelo seleccionado, ocultamos el área de trabajo -->
    <asp:Panel ID="pnlAreaTrabajo" runat="server" Visible="false">
        <div class="crud-container">
            <!-- Columna Izquierda: Detalle -->
            <div class="grid-section">
                <h3>Tripulación Asignada</h3>
                <asp:GridView ID="gvTripulacion" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                              OnRowCommand="gvTripulacion_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Rol" HeaderText="Rol en Vuelo" />
                        <asp:BoundField DataField="NombreEmpleado" HeaderText="Empleado" />
                        <asp:TemplateField HeaderText="Acción">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnRemover" runat="server" CommandName="Remover" CommandArgument='<%# Eval("IdVuelo") + "|" + Eval("CodEmpleado") %>' CssClass="action-link" OnClientClick="return confirm('¿Remover a este empleado del vuelo?');"><i class="fa-solid fa-xmark"></i> Remover</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="padding: 20px; text-align: center; color: #94a3b8;">
                            No hay tripulación asignada a este vuelo todavía.
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <!-- Columna Derecha: Formulario -->
            <div class="form-section">
                <h3>Agregar a Vuelo</h3>
                
                <div class="form-group">
                    <label>Empleado (Solo autorizados)</label>
                    <asp:DropDownList ID="ddlEmpleado" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                
                <div class="form-group">
                    <label>Rol Asignado</label>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control">
                        <asp:ListItem Text="CAPITAN" Value="CAPITAN"></asp:ListItem>
                        <asp:ListItem Text="COPILOTO" Value="COPILOTO"></asp:ListItem>
                        <asp:ListItem Text="JEFE_CABINA" Value="JEFE_CABINA"></asp:ListItem>
                        <asp:ListItem Text="TRIPULANTE_CABINA" Value="TRIPULANTE_CABINA"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div style="margin-top: 20px;">
                    <asp:Button ID="btnAsignar" runat="server" Text="Asignar a Vuelo" CssClass="btn btn-primary" style="width: 100%;" OnClick="btnAsignar_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>

</asp:Content>
