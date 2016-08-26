<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="apertura.aspx.cs" EnableEventValidation="false" Inherits="aperturAutomatica.prueba" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="CSS/styles.css" rel="stylesheet" type="text/css" />
    <link href="CSS/conciliacion.css" rel="stylesheet" type="text/css" />
    
    <title>Apertura Automática</title>
    <style type="text/css">
        .auto-style1 {
            text-align: right;
        }

        .auto-style2 {
            text-align: left;
        }

        .auto-style3 {
            width: 654px;
        }

        .auto-style4 {
            text-align: center;
        }

        #lblrepor {
            margin: auto;
        }

        body {
            margin: 0px;
            padding: 0px;
        }

        .hiddencol {
            display: none;
        }
    </style>
   
</head>
<body style="font-family: tahoma">
  <form id="form1" runat="server">

      <div id="background">
        <div id="head" >
            <div id="logo">
                &nbsp;<img src="images/Capufe.gif" alt="CAPUFE" />
            </div>
            <div id="barra_titulo">
                <label id="titulo">CAPUFE</label><br />
                
            </div>
        </div>
        <div id="green_background_ap">
            <br />
                    <asp:Button Text="Apertura" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                              OnClick="Tab1_Click" Width="70px" Height="20px"  Style="text-align:left; -webkit-linear-gradient(#909090 ,white); background: -o-linear-gradient(#909090  , white); background: -moz-linear-gradient(#909090  , white);background: linear-gradient(#909090  , white); "  />
                          <asp:Button Text="Reportes" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
                              OnClick="Tab2_Click" Width="70px" Height="20px" Style="text-align:left; background: -webkit-linear-gradient(#909090 ,white); background: -o-linear-gradient(#909090  , white); background: -moz-linear-gradient(#909090  , white);background: linear-gradient(#909090  , white); "/>
                    <asp:Button Text="TILH" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
                              OnClick="Tab3_Click" Width="70px" Height="20px" Style="text-align:left; background: -webkit-linear-gradient(#909090 ,white); background: -o-linear-gradient(#909090  , white); background: -moz-linear-gradient(#909090  , white);background: linear-gradient(#909090  , white); "/>
                    <asp:Button Text="Descuentos" BorderStyle="None" ID="Tab4" CssClass="Initial" runat="server"
                              OnClick="Tab4_Click" Width="70px" Height="20px" Style="text-align:left; background: -webkit-linear-gradient(#909090 ,white); background: -o-linear-gradient(#909090  , white); background: -moz-linear-gradient(#909090  , white);background: linear-gradient(#909090  , white); "/>
                    <asp:Button Text="Exentos" BorderStyle="None" ID="Tab5" CssClass="Initial" runat="server"
                              OnClick="Tab5_Click" Width="70px" Height="20px" Style="text-align:left; background: -webkit-linear-gradient(#909090 ,white); background: -o-linear-gradient(#909090  , white); background: -moz-linear-gradient(#909090  , white);background: linear-gradient(#909090  , white); "/>
                     <asp:Button ID="Button1" runat="server" OnClick="btnCerrar_Click"  Text="Salir" />
                                                
                                               
            <asp:Label style="padding-left: 450px; color:white;"  ID="lblSesion" runat="server"></asp:Label>

                        
            <br />
            <div id="white_background_ap">
                <div id="head_form">
                        
                          
                </div>
                <div id="multiviw">
                    <table width:80%, align="center" style="border:0;">
                      <tr>
                        <td class="auto-style3">
                            <asp:Panel runat="server" BorderWidth="0" BorderColor="White">
                          <asp:MultiView ID="MainView" runat="server">
                            <asp:View ID="View1" runat="server">
                              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; border:0">
                                <tr>
                                  <td style="text-align: center">
                                      <h3>APERTURA AUTOMÁTICA</h3>
                                      
                                    <table style="margin: 0 auto; width:350px" class="auto-style4" >
                                        <tr>
                                            <td class="Initial">Plaza de Cobro:</td>
                                            <td>
                                                <asp:DropDownList ID="drpPlaza" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="wrapper-dropdown" DataSourceID="SqlDataSource4" DataTextField="DESCRIPCION" DataValueField="CVE_TRAM_CAM" OnSelectedIndexChanged="drpPlaza_SelectedIndexChanged"  BackColor="White" ForeColor="Black"  Height="25px" Width="180px">
                                                    <asp:ListItem Value="0">-SELECCIONA PLAZA-</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPlaza" runat="server" Font-Size="Smaller" ForeColor="#990000"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Initial">Fecha Base:</td>
                                            <td class="auto-style2">
                                                <asp:TextBox ID="txtFecha" runat="server" ReadOnly="True" ></asp:TextBox>
                                                <asp:Button ID="btnFecha" runat="server" OnClick="btnFecha_Click" Text="Fecha" />
                                                <asp:Label ID="lblFechaA" runat="server" Font-Size="Smaller" ForeColor="#990000"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                              <br />
                                            </td>
                                            <td>
                                                <div style=" margin: 0 auto;">
                                                    <asp:Calendar ID="Calendar" runat="server" CaptionAlign="Bottom" OnSelectionChanged="Calendar_SelectionChanged" Visible="False"></asp:Calendar>
                                                </div>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                     <table style="margin: 0 auto;">
                                         <tr>
                                            <asp:Label ID="lblGreat" runat="server" Text="" BackColor="Yellow" ForeColor="Black" Visible="False"></asp:Label>
                                        </tr>
                                       
                                         <tr style="width:auto">
                                             <td >
                                                 <asp:Button ID="btnEnviar" runat="server" OnClick="btnEnviar_Click" Text="Enviar" Style="float:left" />
                                            </td>
                                             <td>
                                                 <asp:Button ID="btnLimpiar" runat="server" OnClick="txtLimpiar_Click" Text="Limpiar" />
                                              </td>
                                             <td>
                                                 <asp:Button ID="btnsalirAperturas" runat="server" OnClick="btnCerrar_Click"  Text="Salir" />
                                                
                                             </td>

                                            
                                             
                                         </tr>
                                         <tr>
                                             <td >
                                                  <asp:Label ID="lblExc" runat="server"></asp:Label>
                                             </td>

                                         </tr>
                                     </table>
                                  </td>
                                </tr>
                              </table >
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                              <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid; border:0" class="auto-style4">
                                <tr>
                                  <td class="auto-style4">
                                      <H3>
                                          <asp:Label ID="lblReporte" runat="server"></asp:Label>
                                      </H3>
                                        
                                      <table style="margin: 0 auto; width:350px">
                                           <tr>
                                              <td class="Initial">
                                                  Reporte:
                                              </td>
                                              <td >
                                                
                                                  
                                                  <asp:DropDownList ID="drpDes" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="wrapper-dropdown" OnSelectedIndexChanged="drpDes_SelectedIndexChanged" BackColor="White" ForeColor="Black" Height="25px" Width="200px">                                                      
                                                      <asp:ListItem Value="0">-SELECCIONA UNA OPCION-</asp:ListItem>

                                                      <asp:ListItem Value="1">Desagregado de Aforo Diario de Medios Electrónicos de Pago por Plaza de Cobro.</asp:ListItem>

                                                      <asp:ListItem Value="2">Desagregado de Ingreso Diario de Medios Electrónicos de Pago por Plaza de Cobro.</asp:ListItem>
                                                      
                                                      <asp:ListItem Value="3">Desagregado de Aforo Mensual  de Medios Electrónicos de Pago por Plaza de Cobro</asp:ListItem>
                                                      
                                                      <asp:ListItem Value="4">Desagregado de Ingreso Mensual de Medios Electrónicos de Pago por Plaza de Cobro</asp:ListItem>
                                                      
                                                      <asp:ListItem Value="5">DETALLE DE DICTAMEN FINAL MENSUAL</asp:ListItem>
                                                  </asp:DropDownList>
                                                  <asp:Label ID="lblReport" runat="server" ForeColor="#CC0000"></asp:Label>
                                              </td>
                                          </tr>

                                          <tr>
                                              <td class="Initial">
                                                <asp:Label ID="Label1" runat="server">Tipo de red:</asp:Label>
                                              </td>
                                              <td >
                                                  <asp:DropDownList ID="drpTipoRed" runat="server"  AppendDataBoundItems="True" AutoPostBack="True" CssClass="wrapper-dropdown" BackColor="White" ForeColor="Black" Height="25px" Width="200px" OnSelectedIndexChanged="drpTipoRed_SelectedIndexChanged">
                                                      <asp:ListItem Value="0">-SELECCIONA UN TIPO DE RED-</asp:ListItem>
                                                      <asp:ListItem Value="1">Red propia</asp:ListItem>
                                                      <asp:ListItem Value="3">Red Fonadin</asp:ListItem>
                                                      <asp:ListItem Value="2">Red Contratada</asp:ListItem>
                                                  </asp:DropDownList>
                                                  <asp:Label ID="lblTipoRed" runat="server" ForeColor="#CC0000"></asp:Label>
                                              </td>
                                          </tr>
                                          <tr>
                                              <td class="Initial">
                                                  &nbsp;<asp:Label ID="lblPlazaC" runat="server" Text="Plaza de cobro:"></asp:Label>
                                              </td>
                                              
                                              <td >
                                                  <asp:DropDownList ID="drpPlazaR" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="wrapper-dropdown" DataSourceID="SqlDataSource1" DataTextField="DESCRIPCION" DataValueField="CVE_TRAM_CAM" OnSelectedIndexChanged="drpPlazaR_SelectedIndexChanged"  BackColor="White" ForeColor="Black" Height="25px" Width="200px" Visible="False">
                                                      <asp:ListItem Value="0">-SELECCIONA PLAZA-</asp:ListItem>
                                                  </asp:DropDownList>

                                                  <asp:DropDownList ID="drpPlazaR1" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="wrapper-dropdown" DataSourceID="SqlDataSource2" DataTextField="DESCRIPCION" DataValueField="CVE_TRAM_CAM" OnSelectedIndexChanged="drpPlazaR_SelectedIndexChanged"  BackColor="White" ForeColor="Black" Height="25px" Width="200px" Visible="False">
                                                      <asp:ListItem Value="0">-SELECCIONA PLAZA-</asp:ListItem>
                                                  </asp:DropDownList>

                                                  <asp:DropDownList ID="drpPlazaR2" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="wrapper-dropdown" DataSourceID="SqlDataSource3" DataTextField="DESCRIPCION" DataValueField="CVE_TRAM_CAM" OnSelectedIndexChanged="drpPlazaR_SelectedIndexChanged"  BackColor="White" ForeColor="Black" Height="25px" Width="200px" Visible="False">
                                                      <asp:ListItem Value="0">-SELECCIONA PLAZA-</asp:ListItem>
                                                  </asp:DropDownList>

                                                   <asp:DropDownList ID="drpPlazaR3" runat="server" AppendDataBoundItems="True" AutoPostBack="True" CssClass="wrapper-dropdown" DataSourceID="SqlDataSource4" DataTextField="DESCRIPCION" DataValueField="CVE_TRAM_CAM" OnSelectedIndexChanged="drpPlazaR_SelectedIndexChanged"  BackColor="White" ForeColor="Black" Height="25px" Width="200px">
                                                      <asp:ListItem Value="0">-SELECCIONA PLAZA-</asp:ListItem>
                                                  </asp:DropDownList>
                                                  <br />
                                                  <asp:Label ID="lblPlazaR" runat="server" ForeColor="Red"></asp:Label>
                                              </td>
                                          </tr>
                                          
                                          <tr>
                                              <td class="Initial">
                                                  Fecha inicio:
                                              </td>
                                              <td class="auto-style2" >
                                                <asp:TextBox ID="txtFechaBase0" runat="server" ReadOnly="True"></asp:TextBox>
                                                <asp:Button ID="btnInicio" runat="server" OnClick="btnInicio_Click" Text="Fecha" />
                                              </td>
                                          </tr>
                                          <tr>
                                              <td>

                                              </td>
                                              <td class="auto-style2">
                                                  <div style="margin: 0 auto;">
                                                      <asp:Calendar ID="CalendarInicio" runat="server" CaptionAlign="Bottom" OnSelectionChanged="CalendarInicio_SelectionChanged" Visible="False"></asp:Calendar>
                                                      <asp:Label ID="lblFechaini" runat="server" ForeColor="Red"></asp:Label>
                                                  </div>
                                              </td>
                                          </tr>
                                          <tr>
                                              <td class="Initial">
                                                  Fecha fin:
                                              </td>
                                              <td class="auto-style2">
                                                  <asp:TextBox ID="txtFechaBase1" runat="server" ReadOnly="True"></asp:TextBox>
                                                  <asp:Button ID="btnFin" runat="server" OnClick="btnFin_Click" Text="Fecha" Enabled="False" />
                                              </td>
                                          </tr>
                                          <tr>
                                              <td>

                                              </td>
                                              <td class="auto-style2">
                                                  <div style="margin: 0 auto;">
                                                      <asp:Calendar ID="CalendarFin" runat="server" CaptionAlign="Bottom" OnSelectionChanged="CalendarFin_SelectionChanged" Visible="False"></asp:Calendar>
                                                      <asp:Label ID="lblErrorFecha" runat="server" ForeColor="Red"></asp:Label>
                                                  </div>
                                              </td>
                                          </tr>
                                         
                                      </table>
                                      <table style="margin: 0 auto;">
                                         <tr>
                                             <td>
                                                 <asp:Button ID="btnEnviarR" runat="server" Text="Aceptar" OnClick="btnEnviarR_Click1" Width="58px" />
                                             </td>
                                             <td>
                                                 <asp:Button ID="btnLimpiarR" runat="server" Text="Limpiar" OnClick="btnLimpiarR_Click" />
                                             </td>
                                             <td>
                                                 <asp:Button ID="btnsalirReportes" runat="server" Text="Salir" OnClick="btnsalirReportes_Click" />
                                             </td>
                                         </tr>
                                     </table>
                                          
                                         
                                          <br />
                                          <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT dd.cve_pzcobro ||'-'|| dc.descripcion DESCRIPCION, dd.CVE_PZCOBRO CVE_TRAM_CAM FROM DDTADPZC dd INNER JOIN dcpzacbr dcp ON dd.cve_pzcobro = dcp.cve_pzcobro INNER JOIN dcelmorg dc ON dc.cve_eorg=dcp.cve_eorg WHERE dd.cve_tadm =1">
                                          </asp:SqlDataSource>
                                          <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="select a.cve_pzcobro ||'-'|| b.descripcion   DESCRIPCION, a.CVE_PZCOBRO CVE_TRAM_CAM
                                            from admdcm.dcpzacbr a, admdcm.DCELMORG b 
                                            where a.cve_eorg = b.cve_eorg
                                            order by lpad(cve_pzcobro,4,0)"></asp:SqlDataSource>
                                          <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT dd.cve_pzcobro ||'-'|| dc.descripcion DESCRIPCION, dd.CVE_PZCOBRO CVE_TRAM_CAM FROM DDTADPZC dd INNER JOIN dcpzacbr dcp ON dd.cve_pzcobro = dcp.cve_pzcobro INNER JOIN dcelmorg dc ON dc.cve_eorg= dcp.cve_eorg WHERE dd.cve_tadm =2"></asp:SqlDataSource>
                                      <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT dd.cve_pzcobro ||'-'|| dc.descripcion DESCRIPCION, dd.CVE_PZCOBRO CVE_TRAM_CAM FROM DDTADPZC dd INNER JOIN dcpzacbr dcp ON dd.cve_pzcobro = dcp.cve_pzcobro INNER JOIN dcelmorg dc ON dc.cve_eorg= dcp.cve_eorg WHERE dd.cve_tadm =3"></asp:SqlDataSource>
                                          <asp:Label ID="LBLPRUEBA" runat="server" Visible="False"></asp:Label>
                                          <br />
                                  </td>
                                </tr>
                              </table>
                            </asp:View>

                              <asp:View ID="View3" runat="server" >
                              <table id="tabla" style="border:0;">
                                <tr>
                                  <td style="text-align: center">
                                      <h3>TILH</h3>
                                      
                                     
                                      <asp:Label ID="lblApertura" runat="server"></asp:Label>
                                      <br />
                                      <asp:FileUpload ID="FileUpload1" runat="server" />
                                     <asp:Button ID="btnSubir" runat="server" Text="Subir Archivo" OnClick="btnSubir_Click" />
                                    <br />
                                    <br />
                                    <br />
                                      <asp:Label ID="UploadStatusLabel" runat="server"></asp:Label>
                                    <br />
                                        <asp:Label ID="ContentsLabel" runat="server"></asp:Label>
                <br />
                <br />
                <br />
                                        <asp:TextBox ID="LengthLabel" runat="server" Height="64px" TextMode="MultiLine" Width="660px" Visible="false"></asp:TextBox>

    
                                  </td>
                                </tr>
                              </table >
                            </asp:View>

                               <asp:View ID="View4" runat="server">
                              <table id="tablas" style="margin:auto">
                                <tr>
                                  <td style="text-align: center">
                                      <h3>
                                          <asp:Label ID="lblrepor" runat="server" ></asp:Label></h3>
                                      
                                      <p>
                                          <asp:Label ID="lblPlazaCobro" runat="server" Visible="False"></asp:Label>
                                          <asp:Label ID="lblNoPlazaCobro" runat="server" Visible="False"></asp:Label>
                                      </p>
                                      <asp:Label ID="lblBan" runat="server" Visible="False"></asp:Label>
                                      <div class="hiddencol">
                                      <asp:GridView ID="GridView1" runat="server" EnableModelValidation="True" ItemStyle-CssClass="hiddencol" CellPadding="4" ForeColor="#333333" GridLines="None"   >
                                      </asp:GridView>
                                          </div>
                                      <br />

                                      <asp:Label ID="lblapr" runat="server"></asp:Label>
                                      <br />
                                     <table style="margin: 0 auto;">
                                         <tr>
                                             <td>
                                                 <asp:Button ID="btnPdf" runat="server" OnClick="btnPdf_Click" Text="Exportar a PDF" Visible="False" />
                                                 &nbsp;
                                                 <asp:Button ID="btnCsv" runat="server" OnClick="btnXml_Click" Text="Exportar a CSV" Visible="False" />
                                                 &nbsp;
                                                 <asp:Button ID="btnReturn" runat="server" Text="Regresar" OnClick="bntReturn_Click" />
                                             </td>
                                         </tr>
                                     </table>
    
                                  </td>
                                </tr>
                              </table >
                            </asp:View>

                              <asp:View ID="View5" runat="server">
                                  <asp:Label ID="lblDescuentos" runat="server" Font-Size="20px">Descuentos</asp:Label> <br /><br /><br />
                                  <asp:Button ID="btnEjecutarDescuentos" runat="server" Text="Ejecutar" OnClick="btnEjecutarDescuentos_Click"/>
                                  <br />
                                  <asp:Label ID="lblDescuento" runat="server" ForeColor="Red"></asp:Label>
                                  <br />
                                  <asp:GridView ID="GridVDescuentos" runat="server"></asp:GridView>
                                  <br />
                                  <asp:TextBox id="TextArea1" TextMode="multiline" Columns="50" Rows="5" runat="server" />                                  
                              </asp:View>
                              <asp:View ID="View6" runat="server">
                                   
                                  <asp:Label ID="lblExentos" runat="server" Font-Size="20px">Exentos</asp:Label>
                                  <br /><br /><br />
                                   <asp:Button ID="btnEjecutarExentos" runat="server" Text="Ejecutar" OnClick="btnEjecutarExentos_Click"/>
                                  <br />
                                  
                                  <asp:Label ID="lblExento" runat="server" ForeColor="Red"></asp:Label>
                                 
                                  <br />
                                  <asp:GridView ID="GridView2" runat="server"></asp:GridView>
                                  <br />

                                  <asp:TextBox id="TextArea2" TextMode="multiline" Columns="50" Rows="5" runat="server" />
                              </asp:View>

                          </asp:MultiView>
                        </asp:Panel>
                        </td>
                      </tr>
                    </table>
                    <br />
                </div>
            </div>
        </div>
    </div>
</form>
</body>
</html>
