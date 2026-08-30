<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .main-container {
            padding: 20px;
            display: flex;
            flex-direction: column;
            flex-grow: 1;
        }

        .home-container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: calc(100vh - 40px);
            width: 100%;
            overflow: hidden;
            border-radius: 16px;
            background: rgba(11, 17, 32, 0.65);
            backdrop-filter: blur(16px);
            -webkit-backdrop-filter: blur(16px);
            border: 1px solid rgba(255,255,255,0.25);
            box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.3);
        }
        
        .home-image {
            width: 100%;
            height: 100%;
            object-fit: cover;
            border-radius: 12px;
            opacity: 0.95;
            transition: opacity 0.5s ease-in-out;
        }
        
        .home-image:hover {
            opacity: 1;
        }
        
        .roles-container {
            position: absolute;
            display: flex;
            gap: 20px;
            z-index: 10;
        }
        
        .btn-role {
            padding: 15px 30px;
            font-size: 1.2rem;
            font-weight: 600;
            color: #fff;
            background: rgba(11, 17, 32, 0.8);
            border: 2px solid #00d2ff;
            border-radius: 8px;
            cursor: pointer;
            transition: all 0.3s ease;
            text-transform: uppercase;
        }
        
        .btn-role:hover {
            background: #00d2ff;
            color: #0b1120;
            box-shadow: 0 0 15px rgba(0, 210, 255, 0.5);
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="home-container">
        <img src="~/Content/Images/brisky-main.png" alt="BriSky System" class="home-image" runat="server" />
        <div class="roles-container">
            <asp:Button ID="btnUser" runat="server" Text="Ingresar como Usuario" CssClass="btn-role" OnClick="btnUser_Click" />
            <asp:Button ID="btnAdmin" runat="server" Text="Ingresar como Administrativo" CssClass="btn-role" OnClick="btnAdmin_Click" />
        </div>
    </div>
</asp:Content>
