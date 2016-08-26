<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="aperturAutomatica._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="CSS/styles.css" rel="stylesheet" type="text/css" />
    <link href="CSS/conciliacion.css" rel="stylesheet" type="text/css" />
     
    <title>Login Apertura</title>
   
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
        body {
        margin:0;
        padding:0;
        }
    </style>
   
</head>
<body>
    <div id="background">
        <div id="head" >
            <div id="logo">
                <img src="images/Capufe.gif" alt="CAPUFE" />
            </div>
            <div id="barra_titulo">
                <label id="titulo">CAPUFE</label><br />
                
            </div>
        </div>
        <form id="form2" runat="server">
        <div id="green_background">
            <br />
            <div id="white_background">
                <div id="head_form">
                    <label>Inicio de Sesi&oacute;n</label>
                </div>
                <div id= "formulario">
                         <table>
                             <tr>
                                 <td align="right">
                                    <label >Usuario: </label>
                                </td>
                                 <td>
                                    <asp:TextBox ID="txtUsuario" runat="server" OnTextChanged="txtUsuario_TextChanged" Style="text-transform:uppercase;"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                 <td align="right">
                                     <label>Contraseña</label>
                                 </td>
                                 <td>
                                    <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" OnTextChanged="txtContrasena_TextChanged" Style="text-transform:uppercase;" ></asp:TextBox>
                                 </td>
                             </tr>
                             <tr>
                                 <td colspan="2">
                                     <asp:Label id="lblError" runat="server"></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td></td>
                                 <td >
                                     <asp:Button ID="btnEntrar" runat="server" Text="Aceptar" OnClick="btnEntrar_Click" />
                                 
                                     <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                                 </td>
                             </tr>
                        </table>
                </div>
            </div>
        </div>
        </form>
        </div>
    
</body>
</html>