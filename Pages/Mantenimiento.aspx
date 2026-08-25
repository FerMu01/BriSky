<%@ Page Title="Gestión de Mantenimiento" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Mantenimiento.aspx.cs" Inherits="Pages_Mantenimiento" %>

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
        .btn-warning { background: #f59e0b; color: white; padding: 5px 10px; font-size: 0.85rem; }
        .btn-warning:hover { background: #d97706; }
        
        .styled-grid { width: 100%; border-collapse: collapse; margin-top: 15px; }
        .styled-grid th, .styled-grid td { padding: 10px; text-align: left; border-bottom: 1px solid #e2e8f0; font-size: 0.9rem; }
        .styled-grid th { background-color: #f8fafc; color: #1e293b; font-weight: 600; }
        .styled-grid tr:hover { background-color: #f1f5f9; }
        
        .alert { padding: 10px 15px; border-radius: 4px; margin-bottom: 15px; }
        .alert-error { background: #fef2f2; color: #b91c1c; border: 1px solid #f87171; }
        .alert-success { background: #ecfdf5; color: #047857; border: 1px solid #34d399; }
        
        .badge { padding: 4px 8px; border-radius: 12px; font-size: 0.8rem; font-weight: 600; }
        .badge-pending { background-color: #fef3c7; color: #b45309; }
        .badge-done { background-color: #dcfce7; color: #166534; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-bottom: 20px;">
        <h2 style="margin-top:0; color: #0f172a;">Control de Mantenimientos</h2>
        <p style="color: #64748b;">Programación y finalización de mantenimientos operativos de la flota.</p>
    </div>

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="crud-container">
        <!-- Columna Izquierda: Historial -->
        <div class="grid-section">
            <h3>Historial de Mantenimientos</h3>
            <asp:GridView ID="gvMantenimiento" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                          DataKeyNames="CodMantenimiento" OnRowCommand="gvMantenimiento_RowCommand">
                <Columns>
                    <asp:BoundField DataField="CodMantenimiento" HeaderText="Cod." />
                    <asp:BoundField DataField="MatriculaAvion" HeaderText="Avión" />
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha Registro" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ProximaFecha" HeaderText="Próx. Mantenimiento" DataFormatString="{0:dd/MM/yyyy}" NullDisplayText="N/A" />
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span class='<%# (bool)Eval("Finalizado") ? "badge badge-done" : "badge badge-pending" %>'>
                                <%# (bool)Eval("Finalizado") ? "Finalizado" : "En Proceso" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acción">
                        <ItemTemplate>
                            <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" 
                                        CommandName="FinalizarActividad" 
                                        CommandArgument='<%# Eval("CodMantenimiento") %>' 
                                        Visible='<%# !(bool)Eval("Finalizado") %>' 
                                        CssClass="btn btn-warning" 
                                        OnClientClick="return confirm('¿Confirma que el mantenimiento ha finalizado?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Columna Derecha: Formulario -->
        <div class="form-section">
            <h3>Programar Mantenimiento</h3>
            
            <div class="form-group">
                <label>Código del Mantenimiento</label>
                <asp:TextBox ID="txtCodMantenimiento" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Avión (Matrícula)</label>
                <asp:DropDownList ID="ddlAvion" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label>Tipo de Tarea</label>
                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                    <asp:ListItem Text="PREVENTIVO" Value="PREVENTIVO"></asp:ListItem>
                    <asp:ListItem Text="CORRECTIVO" Value="CORRECTIVO"></asp:ListItem>
                    <asp:ListItem Text="INSPECCION" Value="INSPECCION"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label>Mecánico Asignado</label>
                <asp:DropDownList ID="ddlPersonalMantenimiento" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label>Descripción del Trabajo</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Próxima Fecha (Opcional)</label>
                <asp:TextBox ID="txtProximaFecha" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            
            <div style="margin-top: 20px;">
                <asp:Button ID="btnRegistrar" runat="server" Text="Programar Mantenimiento" CssClass="btn btn-primary" style="width: 100%;" OnClick="btnRegistrar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
