<%@ Page Title="Gestión de Flota" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Flota.aspx.cs" Inherits="Pages_Flota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .tabs-container { margin-bottom: 20px; border-bottom: 2px solid #e2e8f0; display: flex; gap: 10px; }
        .tab-button { background: none; border: none; padding: 10px 20px; font-size: 1rem; color: #64748b; cursor: pointer; border-bottom: 3px solid transparent; font-weight: 500; transition: all 0.2s; }
        .tab-button:hover { color: #3b82f6; }
        .tab-button.active-tab { color: #3b82f6; border-bottom-color: #3b82f6; }
        
        .crud-container { display: flex; gap: 20px; flex-wrap: wrap; }
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
        <h2 style="margin-top:0; color: #0f172a;">Gestión de Flota</h2>
        <p style="color: #64748b;">Administra los modelos de aviones, el inventario físico y la compatibilidad con aeropuertos.</p>
    </div>

    <!-- Pestañas -->
    <div class="tabs-container">
        <asp:LinkButton ID="btnTabModelos" runat="server" CssClass="tab-button active-tab" OnClick="CambiarPestaña_Click" CommandArgument="0">Modelos de Avión</asp:LinkButton>
        <asp:LinkButton ID="btnTabAviones" runat="server" CssClass="tab-button" OnClick="CambiarPestaña_Click" CommandArgument="1">Inventario de Aviones</asp:LinkButton>
        <asp:LinkButton ID="btnTabCompat" runat="server" CssClass="tab-button" OnClick="CambiarPestaña_Click" CommandArgument="2">Matriz de Compatibilidad</asp:LinkButton>
    </div>

    <asp:MultiView ID="mvFlota" runat="server" ActiveViewIndex="0">
        
        <!-- ============================================== -->
        <!-- VISTA 1: MODELOS DE AVIÓN -->
        <!-- ============================================== -->
        <asp:View ID="vwModelos" runat="server">
            <asp:Panel ID="pnlMensajeModelo" runat="server" Visible="false">
                <asp:Label ID="lblMensajeModelo" runat="server"></asp:Label>
            </asp:Panel>

            <div class="crud-container">
                <div class="grid-section">
                    <h3>Catálogo de Modelos</h3>
                    <asp:GridView ID="gvModelos" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                  DataKeyNames="CodModelo" OnRowCommand="gvModelos_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CodModelo" HeaderText="Código" />
                            <asp:BoundField DataField="Fabricante" HeaderText="Fabricante" />
                            <asp:BoundField DataField="Nombre" HeaderText="Modelo" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                            <asp:BoundField DataField="CapacidadPasajeros" HeaderText="Pasajeros" />
                            <asp:BoundField DataField="CapacidadEquipaje" HeaderText="Equipaje (kg)" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditarModelo" runat="server" CommandName="EditarModelo" CommandArgument='<%# Eval("CodModelo") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminarModelo" runat="server" CommandName="EliminarModelo" CommandArgument='<%# Eval("CodModelo") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Eliminar modelo de avión?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="form-section">
                    <h3 id="lblTituloFormModelo" runat="server">Registrar Modelo</h3>
                    <asp:HiddenField ID="hfModoEdicionModelo" runat="server" Value="false" />
                    
                    <div class="form-group">
                        <label>Código del Modelo</label>
                        <asp:TextBox ID="txtCodModelo" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Fabricante (ej: Boeing, Airbus)</label>
                        <asp:TextBox ID="txtFabricante" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Nombre (ej: 737 MAX, A320neo)</label>
                        <asp:TextBox ID="txtNombreModelo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Tipo (ej: Comercial, Carga)</label>
                        <asp:TextBox ID="txtTipoModelo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Categoría (Opcional)</label>
                        <asp:TextBox ID="txtCategoria" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Capacidad Pasajeros</label>
                        <asp:TextBox ID="txtPasajeros" runat="server" CssClass="form-control" TextMode="Number" Text="0"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Capacidad Equipaje (kg)</label>
                        <asp:TextBox ID="txtEquipaje" runat="server" CssClass="form-control" Text="0.00"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnGuardarModelo" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarModelo_Click" />
                        <asp:Button ID="btnCancelarModelo" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarModelo_Click" />
                    </div>
                </div>
            </div>
        </asp:View>

        <!-- ============================================== -->
        <!-- VISTA 2: INVENTARIO DE AVIONES (FÍSICOS) -->
        <!-- ============================================== -->
        <asp:View ID="vwAviones" runat="server">
            <asp:Panel ID="pnlMensajeAvion" runat="server" Visible="false">
                <asp:Label ID="lblMensajeAvion" runat="server"></asp:Label>
            </asp:Panel>

            <div class="crud-container">
                <div class="grid-section">
                    <h3>Inventario Operativo</h3>
                    <asp:GridView ID="gvAviones" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                  DataKeyNames="CodInterno" OnRowCommand="gvAviones_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="CodInterno" HeaderText="Cód. Interno" />
                            <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
                            <asp:BoundField DataField="NombreModelo" HeaderText="Modelo Físico" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                            <asp:BoundField DataField="FechaIncorporacion" HeaderText="Incorporación" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditarAvion" runat="server" CommandName="EditarAvion" CommandArgument='<%# Eval("CodInterno") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminarAvion" runat="server" CommandName="EliminarAvion" CommandArgument='<%# Eval("CodInterno") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Eliminar este avión del inventario?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="form-section">
                    <h3 id="lblTituloFormAvion" runat="server">Registrar Avión</h3>
                    <asp:HiddenField ID="hfModoEdicionAvion" runat="server" Value="false" />
                    
                    <div class="form-group">
                        <label>Código Interno de Flota</label>
                        <asp:TextBox ID="txtCodInterno" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Matrícula</label>
                        <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Modelo Asignado</label>
                        <asp:DropDownList ID="ddlModeloAvion" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Estado Laboral / Operativo</label>
                        <asp:TextBox ID="txtEstadoAvion" runat="server" CssClass="form-control" placeholder="Ej: DISPONIBLE"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Fecha de Incorporación</label>
                        <asp:TextBox ID="txtFechaInc" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Último Mantenimiento (Opcional)</label>
                        <asp:TextBox ID="txtUltimoMant" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Próximo Mantenimiento (Opcional)</label>
                        <asp:TextBox ID="txtProxMant" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnGuardarAvion" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarAvion_Click" />
                        <asp:Button ID="btnCancelarAvion" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarAvion_Click" />
                    </div>
                </div>
            </div>
        </asp:View>

        <!-- ============================================== -->
        <!-- VISTA 3: COMPATIBILIDAD AEROPUERTO-MODELO -->
        <!-- ============================================== -->
        <asp:View ID="vwCompatibilidad" runat="server">
            <asp:Panel ID="pnlMensajeCompat" runat="server" Visible="false">
                <asp:Label ID="lblMensajeCompat" runat="server"></asp:Label>
            </asp:Panel>

            <div class="crud-container">
                <div class="grid-section">
                    <h3>Matriz de Restricciones N:M</h3>
                    <asp:GridView ID="gvCompat" runat="server" AutoGenerateColumns="False" CssClass="styled-grid" 
                                  DataKeyNames="CodAeropuerto,CodModelo" OnRowCommand="gvCompat_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="NombreAeropuerto" HeaderText="Aeropuerto" />
                            <asp:BoundField DataField="NombreModelo" HeaderText="Modelo" />
                            <asp:BoundField DataField="Restricciones" HeaderText="Restricciones Operativas" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditarCompat" runat="server" CommandName="EditarCompat" CommandArgument='<%# Eval("CodAeropuerto") + "|" + Eval("CodModelo") %>' CssClass="action-link"><i class="fa-solid fa-pen"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnEliminarCompat" runat="server" CommandName="EliminarCompat" CommandArgument='<%# Eval("CodAeropuerto") + "|" + Eval("CodModelo") %>' CssClass="action-link action-delete" OnClientClick="return confirm('¿Desvincular esta compatibilidad?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="form-section">
                    <h3 id="lblTituloFormCompat" runat="server">Configurar Compatibilidad</h3>
                    <asp:HiddenField ID="hfModoEdicionCompat" runat="server" Value="false" />
                    
                    <div class="form-group">
                        <label>Aeropuerto Físico</label>
                        <asp:DropDownList ID="ddlAeropuertoCompat" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Modelo de Avión</label>
                        <asp:DropDownList ID="ddlModeloCompat" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Restricciones (Ej: "Solo pista 2, horario diurno")</label>
                        <asp:TextBox ID="txtRestricciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    
                    <div style="margin-top: 20px;">
                        <asp:Button ID="btnGuardarCompat" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarCompat_Click" />
                        <asp:Button ID="btnCancelarCompat" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelarCompat_Click" />
                    </div>
                </div>
            </div>
        </asp:View>

    </asp:MultiView>
</asp:Content>
