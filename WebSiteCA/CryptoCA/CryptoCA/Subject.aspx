<%@ Page Title="Create Signed Certificate" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Subject.aspx.cs" Inherits="CryptoCA.Subject" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Certificat signé
    </h2>
    
    <asp:Label ID="lbl_nom" runat="server" Text="Nom : " Width="80px"></asp:Label>
    <asp:TextBox ID="tb_nom" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="lbl_prenom" runat="server" Text="Prénom: " Width="80px"></asp:Label>
    <asp:TextBox ID="tb_prenom" runat="server"></asp:TextBox>
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
