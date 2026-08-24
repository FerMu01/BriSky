<%@ Page Title="Demo Orientada a Objetos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Demo.aspx.cs" Inherits="Pages_Demo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .demo-section {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            background-color: #f9f9f9;
        }
        .demo-title {
            color: #0056b3;
            border-bottom: 2px solid #0056b3;
            padding-bottom: 5px;
            margin-bottom: 15px;
        }
        .status-message {
            font-size: 1.1em;
            font-weight: bold;
            padding: 10px;
            border-radius: 5px;
            margin-bottom: 15px;
        }
        .status-success { background-color: #d4edda; color: #155724; border: 1px solid #c3e6cb; }
        .status-error { background-color: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }
        .form-group { margin-bottom: 15px; }
        .form-group label { display: block; font-weight: bold; margin-bottom: 5px; }
        .form-control { width: 100%; padding: 8px; border: 1px solid #ccc; border-radius: 4px; box-sizing: border-box; }
        .btn-custom { padding: 10px 15px; background-color: #0056b3; color: white; border: none; border-radius: 4px; cursor: pointer; font-weight: bold; }
        .btn-custom:hover { background-color: #004494; }
        .btn-success { background-color: #28a745; }
        .btn-success:hover { background-color: #218838; }
        .grid-view { width: 100%; border-collapse: collapse; margin-top: 15px; background-color: white; }
        .grid-view th, .grid-view td { border: 1px solid #ddd; padding: 10px; text-align: left; }
        .grid-view th { background-color: #0056b3; color: white; }
    </style>

    <div class="container" style="margin-top: 30px;">
        <h2>✈️ Demostraci&oacute;n de BD Orientada a Objetos</h2>
        <p class="lead">Inserci&oacute;n y lectura polim&oacute;rfica (Empleado -> Tripulante -> Piloto).</p>

        <!-- Formulario de Inserción -->
        <div class="demo-section">
            <h3 class="demo-title">Registrar Nuevo Empleado</h3>
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="status-message"></asp:Panel>

            <div style="display: flex; gap: 15px; flex-wrap: wrap;">
                <div class="form-group" style="flex: 1; min-width: 200px;">
                    <label>Nombre:</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                <div class="form-group" style="flex: 1; min-width: 200px;">
                    <label>Apellido:</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                <div class="form-group" style="flex: 1; min-width: 200px;">
                    <label>Documento:</label>
                    <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group" style="flex: 1; min-width: 250px;">
                    <label>Tipo (Clase instanciada):</label>
                    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Empleado (Tabla base)" Value="Empleado"></asp:ListItem>
                        <asp:ListItem Text="Tripulante (Hereda de Empleado)" Value="Tripulante"></asp:ListItem>
                        <asp:ListItem Text="Piloto (Hereda de Tripulante)" Value="Piloto"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group" style="display: flex; align-items: flex-end;">
                    <asp:Button ID="btnCreateEmployee" runat="server" Text="Insertar" CssClass="btn-custom btn-success" OnClick="btnCreateEmployee_Click" />
                </div>
            </div>
        </div>

        <!-- Listado de Datos -->
        <div class="demo-section">
            <h3 class="demo-title">Personal Registrado (Lectura y JOIN)</h3>
            <p>Se muestran Tripulantes y Pilotos combinando sus propiedades de clase heredadas.</p>
            <asp:GridView ID="gvTripulantes" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                <Columns>
                    <asp:BoundField DataField="CodEmpleado" HeaderText="Cod." />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                    <asp:BoundField DataField="Documento" HeaderText="Documento" />
                    <asp:TemplateField HeaderText="Clase">
                        <ItemTemplate>
                            <strong><%# ObtenerRol(Container.DataItem) %></strong>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Horas Vuelo">
                        <ItemTemplate>
                            <%# ObtenerHorasVuelo(Container.DataItem) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No hay registros en la base de datos.
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
