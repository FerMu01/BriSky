<%@ Page Title="Gestión de Ubicaciones" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Ubicaciones.aspx.cs" Inherits="Pages_Ubicaciones" %>

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

        /* GridView Styles */
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-bottom: 20px;">
        <h2 style="margin-top:0; color: #0f172a;">Gestión de Ubicaciones</h2>
        <p style="color: #64748b;">Administra las ciudades, aeropuertos y oficinas de la aerolínea.</p>
    </div>

    <!-- Pestañas manuales controlando MultiView -->
    <div class="tabs-container">
        <asp:LinkButton ID="btnTabCiudades" runat="server" CssClass="tab-button active-tab" OnClick="CambiarPestaña_Click" CommandArgument="0">Ciudades</asp:LinkButton>
        <asp:LinkButton ID="btnTabAeropuertos" runat="server" CssClass="tab-button" OnClick="CambiarPestaña_Click" CommandArgument="1">Aeropuertos</asp:LinkButton>
        <asp:LinkButton ID="btnTabOficinas" runat="server" CssClass="tab-button" OnClick="CambiarPestaña_Click" CommandArgument="2">Oficinas</asp:LinkButton>
    </div>

    <asp:MultiView ID="mvUbicaciones" runat="server" ActiveViewIndex="0">
        
        <!-- VISTA 1: CIUDADES -->
        <asp:View ID="vwCiudades" runat="server">
            
            <asp:Panel ID="pnlMensajeCiudad" runat="server" Visible="false">
                <asp:Label ID="lblMensajeCiudad" runat="server" Text=""></asp:Label>
            </asp:Panel>

            <div class="crud-container">
                <!-- Listado (GridView) -->
                <div class="grid-section">
                    <h3>Listado de Ciudades</h3>
                    <asp:GridView ID="gvCiudades" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                  DataKeyNames="CodCiudad" OnRowCommand="gvCiudades_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CodCiudad" HeaderText="Código" />
                            <asp:BoundField DataField="Nombre" HeaderText="Ciudad" />
                            <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" CommandName="EditarCiudad" CommandArgument='<%# Eval("CodCiudad") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminar" runat="server" CommandName="EliminarCiudad" CommandArgument='<%# Eval("CodCiudad") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Está seguro de eliminar esta ciudad?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Formulario -->
                <div class="form-section">
                    <h3 id="lblTituloFormCiudad" runat="server">Registrar Ciudad</h3>
                    <asp:HiddenField ID="hfModoEdicionCiudad" runat="server" Value="false" />
                    
                    <div class="form-group">
                        <label>Código (ej: BOG)</label>
                        <asp:TextBox ID="txtCodCiudad" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Nombre de Ciudad</label>
                        <asp:TextBox ID="txtNombreCiudad" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Departamento / Región</label>
                        <asp:TextBox ID="txtDepartamentoCiudad" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnGuardarCiudad" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarCiudad_Click" />
                        <asp:Button ID="btnCancelarCiudad" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarCiudad_Click" />
                    </div>
                </div>
            </div>
        </asp:View>

        <!-- VISTA 2: AEROPUERTOS (Fase 2) -->
        <asp:View ID="vwAeropuertos" runat="server">
            <asp:Panel ID="pnlMensajeAeropuerto" runat="server" Visible="false">
                <asp:Label ID="lblMensajeAeropuerto" runat="server" Text=""></asp:Label>
            </asp:Panel>

            <div class="crud-container">
                <!-- Listado (GridView) -->
                <div class="grid-section">
                    <h3>Listado de Aeropuertos</h3>
                    <asp:GridView ID="gvAeropuertos" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                  DataKeyNames="CodAeropuerto" OnRowCommand="gvAeropuertos_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CodAeropuerto" HeaderText="Código" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre del Aeropuerto" />
                            <asp:BoundField DataField="Pais" HeaderText="País" />
                            <asp:BoundField DataField="Caracteristicas" HeaderText="Características" />
                            <asp:BoundField DataField="CodCiudad" Visible="false" />
                            <asp:BoundField DataField="NombreCiudad" HeaderText="Ciudad" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditarApto" runat="server" CommandName="EditarAeropuerto" CommandArgument='<%# Eval("CodAeropuerto") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminarApto" runat="server" CommandName="EliminarAeropuerto" CommandArgument='<%# Eval("CodAeropuerto") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Está seguro de eliminar este aeropuerto?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Formulario -->
                <div class="form-section">
                    <h3 id="lblTituloFormAeropuerto" runat="server">Registrar Aeropuerto</h3>
                    <asp:HiddenField ID="hfModoEdicionAeropuerto" runat="server" Value="false" />
                    
                    <div class="form-group">
                        <label>Código IATA (ej: VVI)</label>
                        <asp:TextBox ID="txtCodAeropuerto" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Nombre</label>
                        <asp:TextBox ID="txtNombreAeropuerto" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>País</label>
                        <asp:TextBox ID="txtPaisAeropuerto" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Ciudad Asociada</label>
                        <asp:DropDownList ID="ddlCiudad" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Características (Opcional)</label>
                        <asp:TextBox ID="txtCaracteristicas" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnGuardarAeropuerto" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarAeropuerto_Click" />
                        <asp:Button ID="btnCancelarAeropuerto" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarAeropuerto_Click" />
                    </div>
                </div>
            </div>
        </asp:View>

        <!-- VISTA 3: OFICINAS (Fase 3) -->
        <asp:View ID="vwOficinas" runat="server">
            <asp:Panel ID="pnlMensajeOficina" runat="server" Visible="false">
                <asp:Label ID="lblMensajeOficina" runat="server" Text=""></asp:Label>
            </asp:Panel>

            <div class="crud-container">
                <!-- Listado (GridView) -->
                <div class="grid-section">
                    <h3>Listado de Oficinas</h3>
                    <asp:GridView ID="gvOficinas" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                  DataKeyNames="CodOficina" OnRowCommand="gvOficinas_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CodOficina" HeaderText="Código" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre Oficina" />
                            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                            <asp:BoundField DataField="Correo" HeaderText="Correo" />
                            <asp:BoundField DataField="CodCiudad" Visible="false" />
                            <asp:BoundField DataField="NombreCiudad" HeaderText="Ciudad" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditarOfi" runat="server" CommandName="EditarOficina" CommandArgument='<%# Eval("CodOficina") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminarOfi" runat="server" CommandName="EliminarOficina" CommandArgument='<%# Eval("CodOficina") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Está seguro de eliminar esta oficina?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <!-- Formulario -->
                <div class="form-section">
                    <h3 id="lblTituloFormOficina" runat="server">Registrar Oficina</h3>
                    <asp:HiddenField ID="hfModoEdicionOficina" runat="server" Value="false" />
                    
                    <div class="form-group">
                        <label>Código (ej: OFI-01)</label>
                        <asp:TextBox ID="txtCodOficina" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Nombre</label>
                        <asp:TextBox ID="txtNombreOficina" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Dirección</label>
                        <asp:TextBox ID="txtDireccionOficina" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Ciudad Asociada</label>
                        <asp:DropDownList ID="ddlCiudadOficina" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Teléfono (Opcional)</label>
                        <asp:TextBox ID="txtTelefonoOficina" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Correo (Opcional)</label>
                        <asp:TextBox ID="txtCorreoOficina" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnGuardarOficina" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarOficina_Click" />
                        <asp:Button ID="btnCancelarOficina" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarOficina_Click" />
                    </div>
                </div>
            </div>
        </asp:View>

    </asp:MultiView>
</asp:Content>
