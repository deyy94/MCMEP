<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="confirmar.aspx.cs" Inherits="aperturAutomatica.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="CSS/styles.css" rel="stylesheet" type="text/css" />
    <link href="CSS/conciliacion.css" rel="stylesheet" type="text/css" />
    <title>Apertura Automática</title>
    <style type="text/css">
        
        .auto-style2 {
            text-align: left;
        }
        
    </style>

</head>
<body style="text-align: center">
    <form id="form1" runat="server">
    <div id="background">
          <div id="head" >
            <div id="logo">
                <img src="images/Capufe.gif" alt="CAPUFE" />
            </div>
            <div id="barra_titulo">
                <label id="titulo">CAPUFE</label><br />
                <label id="subtitulo">APERTURA DE CONCILIACIÓN</label>
            </div>
        </div>
        <div id="green_background_ap">
         <br />
            <div id="white_background_ap">
                <br />
                <br />

        <asp:Label ID="Label1" runat="server" Text="Procedimiento realizado con exito"></asp:Label>
    
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Regresar" OnClick="Button1_Click" />
                </div>
   </div> 
    </div>
    </form>
</body>
</html>

