﻿<%@ Page Title="Page d'accueil" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="CryptoCA._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Autorité de certification
    </h2>
    

    <asp:Label ID="lbl_ca" runat="server" Text="Nom CA: " Width="80px"></asp:Label>
    <asp:TextBox ID="tb_ca" runat="server"></asp:TextBox>
    <br />
    
    <asp:Label ID="lbl_ville" runat="server" Text="Ville" Width="80px"></asp:Label>
    <asp:TextBox ID="tb_ville" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lbl_pays" runat="server" Text="Pays : " Width="80px"></asp:Label>
    <asp:DropDownList ID="ddl_pays" runat="server"></asp:DropDownList>
    <br />
    <asp:Label ID="lbl_entreprise" runat="server" Text="Entreprise : " Width="80px"></asp:Label>
    <asp:TextBox ID="tb_entreprise" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lbl_service" runat="server" Text="Service : " Width="80px"></asp:Label>
    <asp:TextBox ID="tb_service" runat="server"></asp:TextBox>

    <br />
    <br />
    <asp:Button ID="btn_valider" runat="server" OnClick="btn_valider_Click" Text="Valider" />

</asp:Content>
