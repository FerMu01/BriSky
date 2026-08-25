<%@ Page Title="Gestión de Personal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Empleados.aspx.cs" Inherits="Pages_Empleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .tabs-container {
            margin-bottom: 20px;
            border-bottom: 2px solid #e2e8f0;
            display: flex;
            gap: 10px;
        }
        .tab-button {
            background: none;
            border: none;
            padding: 10px 20px;
            font-size: 1rem;
            color: #64748b;
            cursor: pointer;
            border-bottom: 3px solid transparent;
            font-weight: 500;
            transition: all 0.2s;
        }
        .tab-button:hover {
            color: #3b82f6;
        }
        .tab-button.active-tab {
            color: #3b82f6;
            border-bottom-color: #3b82f6;
        }
        
        .crud-container {
            display: flex;
            gap: 20px;
            flex-wrap: wrap;
        }
        
        .grid-section {
            flex: 1 1 60%;
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        
        .form-section {
            flex: 1 1 35%;
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }

        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: 500;
            color: #1e293b;
        }
        .form-control {
            width: 100%;
            padding: 8px 12px;
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            box-sizing: border-box;
            font-family: inherit;
        }
        .btn {
            padding: 8px 16px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: 500;
        }
        .btn-primary { background: #3b82f6; color: white; }
        .btn-primary:hover { background: #2563eb; }
        .btn-secondary { background: #94a3b8; color: white; }
        .btn-secondary:hover { background: #64748b; }
        .btn-danger { background: #ef4444; color: white; }
        .btn-danger:hover { background: #dc2626; }

        .styled-grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }
        .styled-grid th, .styled-grid td {
            padding: 12px;
            text-align: left;
            border-bottom: 1px solid #e2e8f0;
        }
        .styled-grid th {
            background-color: #f8fafc;
            color: #1e293b;
            font-weight: 600;
        }
        .styled-grid tr:hover {
            background-color: #f1f5f9;
        }
        .action-link {
            color: #3b82f6;
            text-decoration: none;
            margin-right: 10px;
            cursor: pointer;
        }
        .action-link:hover {
            text-decoration: underline;
        }
        .action-delete {
            color: #ef4444;
        }
        .alert {
            padding: 10px 15px;
            border-radius: 4px;
            margin-bottom: 15px;
        }
        .alert-error { background: #fef2f2; color: #b91c1c; border: 1px solid #f87171; }
        .alert-success { background: #ecfdf5; color: #047857; border: 1px solid #34d399; }
        
        .panel-dinamico {
            padding: 15px;
            background-color: #f8fafc;
            border-left: 4px solid #3b82f6;
            margin-top: 10px;
            margin-bottom: 15px;
            border-radius: 4px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-bottom: 20px;">
        <h2 style="margin-top:0; color: #0f172a;">Gestión de Personal</h2>
        <p style="color: #64748b;">Administra las áreas y empleados de la aerolínea con sus roles específicos.</p>
    </div>

    <!-- Pestañas -->
    <div class="tabs-container">
        <asp:LinkButton ID="btnTabAreas" runat="server" CssClass="tab-button active-tab" OnClick="CambiarPestaña_Click" CommandArgument="0">Áreas</asp:LinkButton>
        <asp:LinkButton ID="btnTabEmpleados" runat="server" CssClass="tab-button" OnClick="CambiarPestaña_Click" CommandArgument="1">Empleados</asp:LinkButton>
    </div>

    <asp:MultiView ID="mvPersonal" runat="server" ActiveViewIndex="0">
        
        <!-- ============================================== -->
        <!-- VISTA 1: ÁREAS -->
        <!-- ============================================== -->
        <asp:View ID="vwAreas" runat="server">
            
            <asp:Panel ID="pnlMensajeArea" runat="server" Visible="false">
                <asp:Label ID="lblMensajeArea" runat="server" Text=""></asp:Label>
            </asp:Panel>

            <div class="crud-container">
                <div class="grid-section">
                    <h3>Listado de Áreas</h3>
                    <asp:GridView ID="gvAreas" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                  DataKeyNames="CodArea" OnRowCommand="gvAreas_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CodArea" HeaderText="Código" />
                            <asp:BoundField DataField="Nombre" HeaderText="Área" />
                            <asp:BoundField DataField="Funcion" HeaderText="Función" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditarArea" runat="server" CommandName="EditarArea" CommandArgument='<%# Eval("CodArea") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminarArea" runat="server" CommandName="EliminarArea" CommandArgument='<%# Eval("CodArea") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Está seguro de eliminar esta área?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="form-section">
                    <h3 id="lblTituloFormArea" runat="server">Registrar Área</h3>
                    <asp:HiddenField ID="hfModoEdicionArea" runat="server" Value="false" />
                    
                    <div class="form-group">
                        <label>Código (ej: A-01)</label>
                        <asp:TextBox ID="txtCodArea" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Nombre del Área</label>
                        <asp:TextBox ID="txtNombreArea" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Función / Descripción</label>
                        <asp:TextBox ID="txtFuncionArea" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnGuardarArea" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarArea_Click" />
                        <asp:Button ID="btnCancelarArea" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarArea_Click" />
                    </div>
                </div>
            </div>
        </asp:View>

        <!-- ============================================== -->
        <!-- VISTA 2: EMPLEADOS (Jerarquía Polimórfica) -->
        <!-- ============================================== -->
        <asp:View ID="vwEmpleados" runat="server">
            
            <asp:UpdatePanel ID="UpdatePanelEmpleados" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMensajeEmpleado" runat="server" Visible="false">
                        <asp:Label ID="lblMensajeEmpleado" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <div class="crud-container">
                        <div class="grid-section">
                            <h3>Listado General de Empleados</h3>
                            <asp:GridView ID="gvEmpleados" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                          DataKeyNames="CodEmpleado">
                                <Columns>
                                    <asp:BoundField DataField="CodEmpleado" HeaderText="Código" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                    <asp:BoundField DataField="Documento" HeaderText="Documento" />
                                    <asp:BoundField DataField="NombreArea" HeaderText="Área" />
                                    <asp:BoundField DataField="TipoEmpleado" HeaderText="Rol (Subclase)" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-section">
                            <h3 id="lblTituloFormEmpleado" runat="server">Registrar Empleado</h3>
                            
                            <!-- CAMPOS COMUNES (Base Empleado) -->
                            <div class="form-group">
                                <label>Código de Empleado</label>
                                <asp:TextBox ID="txtCodEmpleado" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Nombres</label>
                                <asp:TextBox ID="txtNombreEmpleado" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Apellidos</label>
                                <asp:TextBox ID="txtApellidoEmpleado" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Documento de Identidad</label>
                                <asp:TextBox ID="txtDocumentoEmpleado" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Área Asignada</label>
                                <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label>Teléfono (Opcional)</label>
                                <asp:TextBox ID="txtTelefonoEmpleado" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Correo (Opcional)</label>
                                <asp:TextBox ID="txtCorreoEmpleado" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                            </div>

                            <!-- EL SELECTOR ESTRATÉGICO -->
                            <div class="form-group" style="margin-top: 25px; border-top: 1px solid #e2e8f0; padding-top: 15px;">
                                <label style="color: #3b82f6; font-weight:bold;">Tipo de Empleado (Rol)</label>
                                <asp:DropDownList ID="ddlTipoEmpleado" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoEmpleado_SelectedIndexChanged">
                                    <asp:ListItem Text="-- Seleccione un Rol --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Oficina" Value="EMPLEADO_OFICINA"></asp:ListItem>
                                    <asp:ListItem Text="Piloto" Value="PILOTO"></asp:ListItem>
                                    <asp:ListItem Text="Tripulante de Cabina" Value="TRIPULANTE_CABINA"></asp:ListItem>
                                    <asp:ListItem Text="Personal de Mantenimiento" Value="PERSONAL_MANTENIMIENTO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <!-- PANELES DINÁMICOS -->
                            
                            <!-- Panel Oficina -->
                            <asp:Panel ID="pnlOficina" runat="server" Visible="false" CssClass="panel-dinamico">
                                <h4 style="margin-top:0;">Datos Específicos: Oficina</h4>
                                <div class="form-group">
                                    <label>Cargo</label>
                                    <asp:TextBox ID="txtCargo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Oficina Asignada</label>
                                    <asp:DropDownList ID="ddlOficina" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </asp:Panel>

                            <!-- Panel Piloto -->
                            <asp:Panel ID="pnlPiloto" runat="server" Visible="false" CssClass="panel-dinamico">
                                <h4 style="margin-top:0;">Datos Específicos: Piloto</h4>
                                <div class="form-group">
                                    <label>N° Licencia de Piloto</label>
                                    <asp:TextBox ID="txtLicenciaPiloto" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Rango de Vuelo</label>
                                    <asp:TextBox ID="txtRango" runat="server" CssClass="form-control" placeholder="Ej: Capitán, Primer Oficial"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <!-- Panel Tripulante de Cabina -->
                            <asp:Panel ID="pnlCabina" runat="server" Visible="false" CssClass="panel-dinamico">
                                <h4 style="margin-top:0;">Datos Específicos: Tripulante de Cabina</h4>
                                <div class="form-group">
                                    <label>N° Licencia de Cabina</label>
                                    <asp:TextBox ID="txtLicenciaCabina" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <!-- Panel Mantenimiento -->
                            <asp:Panel ID="pnlMantenimiento" runat="server" Visible="false" CssClass="panel-dinamico">
                                <h4 style="margin-top:0;">Datos Específicos: Mantenimiento</h4>
                                <div class="form-group">
                                    <label>Especialidad Técnica</label>
                                    <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control" placeholder="Ej: Aviónica, Motores"></asp:TextBox>
                                </div>
                            </asp:Panel>
                            
                            <div style="margin-top: 20px;">
                                <asp:Button ID="btnGuardarEmpleado" runat="server" Text="Registrar Empleado" CssClass="btn btn-primary" OnClick="btnGuardarEmpleado_Click" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:View>

    </asp:MultiView>
</asp:Content>
