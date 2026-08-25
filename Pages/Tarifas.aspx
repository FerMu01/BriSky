<%@ Page Title="Gestión de Tarifas" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Tarifas.aspx.cs" Inherits="Pages_Tarifas" %>

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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-bottom: 20px;">
        <h2 style="margin-top:0; color: #0f172a;">Catálogo de Tarifas Comerciales</h2>
        <p style="color: #64748b;">Administra las familias tarifarias, precios base y franquicias de equipaje.</p>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="crud-container">
        <!-- Columna Izquierda: Historial -->
        <div class="grid-section">
            <h3>Tarifas Disponibles</h3>
            <asp:GridView ID="gvTarifas" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                          DataKeyNames="CodTarifa" OnRowCommand="gvTarifas_RowCommand">
                <Columns>
                    <asp:BoundField DataField="CodTarifa" HeaderText="Código" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre Comercial" />
                    <asp:BoundField DataField="PrecioBase" HeaderText="Precio Base ($)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="EquipajeIncluido" HeaderText="Equipaje (kg)" DataFormatString="{0:N1}" />
                    <asp:BoundField DataField="Condiciones" HeaderText="Condiciones" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server" CommandName="EditarTarifa" CommandArgument='<%# Eval("CodTarifa") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnEliminar" runat="server" CommandName="EliminarTarifa" CommandArgument='<%# Eval("CodTarifa") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Está seguro de eliminar esta tarifa?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Columna Derecha: Formulario -->
        <div class="form-section">
            <h3 id="lblTituloForm" runat="server">Nueva Tarifa</h3>
            
            <div class="form-group">
                <label>Código de Tarifa</label>
                <asp:TextBox ID="txtCodTarifa" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Nombre de la Tarifa (Ej: Económica Básica)</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
            </div>
            
            <div style="display:flex; gap:10px;">
                <div class="form-group" style="flex:1;">
                    <label>Precio Base ($)</label>
                    <asp:TextBox ID="txtPrecioBase" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                </div>
                <div class="form-group" style="flex:1;">
                    <label>Equipaje (Kg)</label>
                    <asp:TextBox ID="txtEquipajeIncluido" runat="server" CssClass="form-control" TextMode="Number" step="0.1"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <label>Condiciones Comerciales (Opcional)</label>
                <asp:TextBox ID="txtCondiciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>
            
            <div style="margin-top: 20px; display:flex; gap:10px;">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Tarifa" CssClass="btn btn-primary" OnClick="btnGuardar_Click" style="flex:1;" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
