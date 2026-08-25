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
            height: calc(100vh - 110px); /* Adjust based on navbar and padding */
            width: 100%;
            overflow: hidden;
            border-radius: 12px;
            background-color: #0b1120;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.15);
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
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="home-container">
        <img src="~/Content/Images/brisky-main.png" alt="BriSky System" class="home-image" runat="server" />
    </div>
</asp:Content>
