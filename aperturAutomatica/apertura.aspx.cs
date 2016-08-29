using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Text;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


//using System.Data;
using System.Data.OracleClient;
using System.Net.Mail;
using ExcelLibrary.SpreadSheet;
using System.Data.OleDb;

namespace aperturAutomatica
{

    public partial class prueba : System.Web.UI.Page
    {
        operacionesBD conexion = new operacionesBD();
        //Thread XMLFileThread;
        DataSet ds;
        String campo = "Campo obligatorio";
        //variables de TILH                                  
        string a;
        string nom;
        string b;
        string todo;
        string index;
        string pzcobro;
        string fecha;
        string numregistro;
        string variable;
        string variable2;
        string fec_base;
        string hora;
        string cve_carril;
        string ind_cuerpo;
        string id_carril;
        string num_tarjeta;
        string evento;
        string folio;
        string cve_turno;
        string cve_tram_cam;
        string cve_clsvehiculo;
        string cve_tvhiculo;
        string cve_edotarjeta;
        string ind_empresa_admon_tarj;
        string cve_fpago;
        string ind_exento;
        string cve_fpago2;
        string pc_descuento;
        string cve_tpocl;
        string importe_dispersado;
        int con = 0;
        int con2 = 0;
        int ban;
        string sec;
        string sec2;
        string sec3;
        string nomarch;
        string plaza;
        string año;
        string mes;
        string nomarch2;
        //Termina variables de TILH
        string eh;
        string flagHeaderPdf;
        string flagHeaderCsv;
        string titulo_archivos;


        protected void Page_Load(object sender, EventArgs e)
        {
            //aperturAutomatica.EnableEventValidation = false; 
            if (Session["usuario"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                if (conexion.validaAdmin(Session["usuario"].ToString()) == 1)
                {
                    if (!IsPostBack)
                    {
                        Tab1.CssClass = "Clicked";
                        MainView.ActiveViewIndex = 0;
                        lblSesion.Text = Session["usuario"].ToString();
                    }
                }
                else
                {
                    lblReporte.Text = "REPORTE";
                    Tab1.Enabled = false;
                    Tab1.Visible = false;
                    MainView.ActiveViewIndex = 1;
                    limpiar(0);
                    LBLPRUEBA.Text = "0";
                    lblSesion.Text = Session["usuario"].ToString();

                }



            }

            Tab1.CssClass = " background: -webkit-linear-gradient(red, yellow); background: -o-linear-gradient(red, yellow); background: -moz-linear-gradient(red, yellow);background: linear-gradient(red, yellow); ";

        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;

            limpiar(1);

        }



      
        protected void Tab2_Click(object sender, EventArgs e)
        {
            lblReporte.Text = "REPORTE";
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;
            limpiar(0);
            LBLPRUEBA.Text = "0";

        }
        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Clicked";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 2;
           
        }
        protected void Tab4_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Clicked";
            Tab5.CssClass = "Initial";
            MainView.ActiveViewIndex = 4;
        }
        protected void Tab5_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            Tab4.CssClass = "Initial";
            Tab5.CssClass = "Clicked";
            MainView.ActiveViewIndex = 5;
        }
        //PESTAÑA APERTURA AUTOMATICA

        protected void btnFecha_Click(object sender, EventArgs e)
        {
            Calendar.Visible = true;
        }

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            if (DateTime.Now >= Calendar.SelectedDate)
            {
                txtFecha.Text = Calendar.SelectedDate.ToString("dd/MM/yyyy");
                lblFechaA.Text = "";
            }
            else
            {
                lblFechaA.Text = "No se pueden elegir fechas posteriores al día de hoy";
            }
            Calendar.Visible = false;
        }


        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            //se necesita hacer una validacion habilitar el boton

            if (drpPlaza.SelectedIndex != 0 && txtFecha.Text != "")
            {

                btnEnviar.Enabled = true;
                string plaza;
                bool nuev;

                plaza = drpPlaza.SelectedValue.ToString();
                nuev = conexion.apertura(txtFecha.Text, plaza, "Apertura Automatica", Session["usuario"].ToString());
                if (conexion.consultaEdoConciliacion(txtFecha.Text, plaza))
                {
                    if (nuev)
                    {
                        try
                        {
                            enviarCorreo();
                            lblGreat.Visible = true;
                            lblGreat.Text = "Apertura realizada con exito";
                        }
                        catch (Exception ex)
                        {
                            lblGreat.Visible = true;
                            lblGreat.Text = "Ups! ocurrio un problema";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                        }

                        lblFechaA.Visible = false;
                        lblPlaza.Visible = false;
                        limpiar(0);
                    }
                    else
                    {
                        lblExc.Text = "Error,Intentelo de nuevo";
                    }
                }
                else {
                    lblGreat.Visible = true;
                    lblGreat.Text = "La apertura no se realizo correctamente";
                }
            }

            lblPlaza.Text = campo;
            lblFechaA.Text = campo;
        }

        
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            salir();

        }

        //Este metodo limpia campos
        protected void txtLimpiar_Click(object sender, EventArgs e)
        {
            limpiar(0);
        }

        //PESTAÑA REPORTES

        protected void drpPlazaR_SelectedIndexChanged(object sender, EventArgs e)
        {
            cerrar_calendarios();
            if (drpPlazaR.SelectedIndex != 0)
            {
                lblPlazaR.Text = "";
            }
            if (drpDes.SelectedIndex == 3)
            {
                Visible = false;
                drpPlazaR.Enabled = false;
            }

        }

        protected void btnInicio_Click(object sender, EventArgs e)
        {
            cerrar_calendarios();
            CalendarInicio.Visible = true;
            btnFin.Enabled = true;
            CalendarInicio.SelectedDate = DateTime.Now;

        }

        protected void CalendarInicio_SelectionChanged(object sender, EventArgs e)
        {
            if (DateTime.Now > CalendarInicio.SelectedDate)
            {
                txtFechaBase0.Text = CalendarInicio.SelectedDate.ToString("dd/MM/yyyy");
                lblFechaini.Text = "";
            }
            else
            {
                lblFechaini.Text = "No se pueden elegir fechas posteriores al día de hoy";
            }
            CalendarInicio.Visible = false;
        }

        protected void btnFin_Click(object sender, EventArgs e)
        {
            cerrar_calendarios();
            CalendarFin.Visible = true;
            CalendarFin.SelectedDate = DateTime.Now;
        }

        protected void CalendarFin_SelectionChanged(object sender, EventArgs e)
        {

            if (CalendarInicio.SelectedDate <= CalendarFin.SelectedDate)
            {
                if (DateTime.Now > CalendarFin.SelectedDate)
                {
                    txtFechaBase1.Text = CalendarFin.SelectedDate.ToString("dd/MM/yyyy");
                    lblErrorFecha.Text = "";
                }
                else
                {
                    lblErrorFecha.Text = "No se pueden elegir fechas posteriores al día de hoy";
                }
            }
            else
            {
                lblErrorFecha.Text = "La fecha de fin no puede ser anterior a la fecha de inicio.";
            }
            CalendarFin.Visible = false;
        }
        /// <summary>
        /// Boton enviar en reportes  
        /// </summary>
        
        protected void btnEnviarR_Click1(object sender, EventArgs e)
        {
            string campo = "Campo Obligatorio";



            //boton enviar de reportes
            DateTime ini, fin;
            string plaza;

            cerrar_calendarios();



            // empieza la seleccion de reportes
            if (drpDes.SelectedIndex == 1)
            {

                //proceso para  el reporte desagregado aforo diario Corregido


                ini = Convert.ToDateTime(txtFechaBase0.Text);
                fin = Convert.ToDateTime(txtFechaBase1.Text);
                plaza = "";

                plaza = drpPlazaR3.SelectedValue;



                lblPlazaCobro.Text = Convert.ToString(drpPlazaR3.SelectedItem);
                lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR3.SelectedValue);

                //metodo  para rellenar con 0 la tabla 
                DataTable dt = new DataTable();
                dt.Columns.Add("FECHA");
                dt.Columns.Add("Tag6");
                dt.Columns.Add("Tag0");
                dt.Columns.Add("TagTDCRespaldo");
                dt.Columns.Add("TagTDDRespaldo");
                dt.Columns.Add("Residentes");
                dt.Columns.Add("Pago_por_recorrido");
                dt.Columns.Add("ResidentesTDC");
                dt.Columns.Add("pprTDC");
                dt.Columns.Add("ResTDD");
                dt.Columns.Add("pprTDD");
                dt.Columns.Add("prepago");
                dt.Columns.Add("exentos");
                dt.Columns.Add("af_total");
                DataRow row;
                while (ini <= fin)
                {
                    ds = conexion.reporteDID(ini, ini, plaza, drpTipoRed.SelectedIndex, LBLPRUEBA.Text);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        row = dt.NewRow();
                        row["FECHA"] = ini.ToString("dd/MM/yyyy");
                        row["Tag6"] = "0";
                        row["Tag0"] = "0";
                        row["TagTDCRespaldo"] = "0";
                        row["TagTDDRespaldo"] = "0";
                        row["Residentes"] = "0";
                        row["Pago_por_recorrido"] = "0";
                        row["ResidentesTDC"] = "0";
                        row["pprTDC"] = "0";
                        row["ResTDD"] = "0";
                        row["pprTDD"] = "0";
                        row["prepago"] = "0";
                        row["exentos"] = "0";
                        row["af_total"] = "0";
                        dt.Rows.Add(row);
                    }
                    else
                    {
                        row = dt.NewRow();
                        row["FECHA"] = ds.Tables[0].Rows[0]["FECHA"].ToString();
                        row["Tag6"] = ds.Tables[0].Rows[0]["Tag6"].ToString();
                        row["Tag0"] = ds.Tables[0].Rows[0]["Tag0"].ToString();
                        row["TagTDCRespaldo"] = ds.Tables[0].Rows[0]["TagTDCRespaldo"].ToString();
                        row["TagTDDRespaldo"] = ds.Tables[0].Rows[0]["TagTDDRespaldo"].ToString();
                        row["Residentes"] = ds.Tables[0].Rows[0]["Residentes"].ToString();
                        row["Pago_por_recorrido"] = ds.Tables[0].Rows[0]["Pago_por_recorrido"].ToString();
                        row["ResidentesTDC"] = ds.Tables[0].Rows[0]["ResidentesTDC"].ToString();
                        row["pprTDC"] = ds.Tables[0].Rows[0]["pprTDC"].ToString();
                        row["ResTDD"] = ds.Tables[0].Rows[0]["ResTDD"].ToString();
                        row["pprTDD"] = ds.Tables[0].Rows[0]["pprTDD"].ToString();
                        row["prepago"] = ds.Tables[0].Rows[0]["prepago"].ToString();
                        row["exentos"] = ds.Tables[0].Rows[0]["exentos"].ToString();
                        row["af_total"] = ds.Tables[0].Rows[0]["af_total"].ToString();
                        dt.Rows.Add(row);
                    }
                    ini = ini.AddDays(1);

                }

                //termina metodo 0


                GridView1.DataSource = ds.Tables[0];

                GridView1.DataBind();


                lblrepor.Text = "Aforo Diario de Medios Electrónicos de Pago por Plaza de Cobro.";
                titulo_archivos = "Aforo_diario";
                lblBan.Text = "FECHA" + ',' + "TAG6" + ',' + "TAG0" + ',' + "TAGTDCRESPALDO" + ',' + "TAGTDDRESPALDO" + ',' + "RESIDENTES" + ',' + "PAGO_POR_RECORRIDO" + ',' + "RESIDENTESTDC" + ',' + "PPRTDC" + ',' + "RESTDD" + ',' + "PPRTDD" + ',' + "PREPAGO" + ',' + "EXENTOS" + ',' + "AF_TOTAL";

                MainView.ActiveViewIndex = 3;

                if (drpPlazaR.SelectedIndex == 0)
                {
                    lblPlazaR.Text = campo;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    MainView.ActiveViewIndex = 3;
                }
            }
            else if (drpDes.SelectedIndex == 2)
            {
                // proceso para el reporte desagregado ingreso diario

                lblBan.Text = "FECHA" + ',' + "TAG6" + ',' + "TAG0" + ',' + "TCR" + ',' + "TDR" + ',' + "RES" + ',' + "RESTDC" + ',' + "RESTDD" + ',' + "PPR" + ',' + "PPRTDC" + ',' + "PPRTDD" + ',' + "PREP" + ',' + "INGNETO" + ',' + "DESCTO6" + ',' + "DESCRES" + ',' + "DESCPPR" + ',' + "INGBRUTO" + ',' + "VALEXENTOS";

                ini = Convert.ToDateTime(txtFechaBase0.Text);
                fin = Convert.ToDateTime(txtFechaBase1.Text);
                plaza = "";
                if (drpTipoRed.SelectedValue == "1")
                {
                    plaza = drpPlazaR.SelectedValue;
                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR.SelectedValue);
                }
                if (drpTipoRed.SelectedValue == "3")
                {
                    plaza = drpPlazaR2.SelectedValue;

                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR2.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR2.SelectedValue);
                }
                if (drpTipoRed.SelectedValue == "2")
                {
                    plaza = drpPlazaR1.SelectedValue;

                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR1.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR1.SelectedValue);
                }


                // metodo para rellenar con 0

                DataTable dt = new DataTable();
                dt.Columns.Add("FECHA");
                dt.Columns.Add("TAG6");
                dt.Columns.Add("TAG0");
                dt.Columns.Add("Tcr");
                dt.Columns.Add("Tdr");
                dt.Columns.Add("Res");
                dt.Columns.Add("ResTdc");
                dt.Columns.Add("ResTdd");
                dt.Columns.Add("ppr");
                dt.Columns.Add("pprTdc");
                dt.Columns.Add("pprTdd");
                dt.Columns.Add("prep");
                dt.Columns.Add("IngNeto");
                dt.Columns.Add("Descto6");
                dt.Columns.Add("DescRes");
                dt.Columns.Add("Descppr");
                dt.Columns.Add("IngBruto");
                dt.Columns.Add("ValExentos");

                DataRow row;
                while (ini <= fin)
                {


                    ds = conexion.reporteDID(ini, ini, plaza, drpTipoRed.SelectedIndex, LBLPRUEBA.Text);

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        row = dt.NewRow();
                        row["FECHA"] = ini.ToString("dd/MM/yyyy");
                        row["TAG6"] = "0";
                        row["TAG0"] = "0";
                        row["Tcr"] = "0";
                        row["Tdr"] = "0";
                        row["Res"] = "0";
                        row["ResTdc"] = "0";
                        row["ResTdd"] = "0";
                        row["ppr"] = "0";
                        row["pprTdc"] = "0";
                        row["pprTdd"] = "0";
                        row["prep"] = "0";
                        row["IngNeto"] = "0";
                        row["Descto6"] = "0";
                        row["DescRes"] = "0";
                        row["Descppr"] = "0";
                        row["IngBruto"] = "0";
                        row["ValExentos"] = "0";
                        dt.Rows.Add(row);
                    }
                    else {
                        row = dt.NewRow();
                        row["FECHA"] = ds.Tables[0].Rows[0]["FECHA"].ToString();
                        row["TAG6"] = ds.Tables[0].Rows[0]["TAG6"].ToString();
                        row["TAG0"] = ds.Tables[0].Rows[0]["TAG0"].ToString();
                        row["Tcr"] = ds.Tables[0].Rows[0]["Tcr"].ToString();
                        row["Tdr"] = ds.Tables[0].Rows[0]["Tdr"].ToString();
                        row["Res"] = ds.Tables[0].Rows[0]["Res"].ToString();
                        row["ResTdc"] = ds.Tables[0].Rows[0]["ResTdc"].ToString();
                        row["ResTdd"] = ds.Tables[0].Rows[0]["ResTdd"].ToString();
                        row["ppr"] = ds.Tables[0].Rows[0]["ppr"].ToString();
                        row["pprTdc"] = ds.Tables[0].Rows[0]["pprTdc"].ToString();
                        row["pprTdd"] = ds.Tables[0].Rows[0]["pprTdd"].ToString();
                        row["prep"] = ds.Tables[0].Rows[0]["prep"].ToString();
                        row["IngNeto"] = ds.Tables[0].Rows[0]["IngNeto"].ToString();
                        row["Descto6"] = ds.Tables[0].Rows[0]["Descto6"].ToString();
                        row["DescRes"] = ds.Tables[0].Rows[0]["DescRes"].ToString();
                        row["Descppr"] = ds.Tables[0].Rows[0]["Descppr"].ToString();
                        row["IngBruto"] = ds.Tables[0].Rows[0]["IngBruto"].ToString();
                        row["ValExentos"] = ds.Tables[0].Rows[0]["ValExentos"].ToString();

                        dt.Rows.Add(row);
                    }
                    ini = ini.AddDays(1);
                }
                GridView1.DataSource = dt;
                // termina para rellenar con 0

                GridView1.DataBind();
                lblrepor.Text = "Ingreso Diario de Medios Electronicos de Medios Electrónicos de Pago por Plaza de Cobro";
                titulo_archivos = "Ing_diario";
                MainView.ActiveViewIndex = 3;
                if (drpPlazaR.SelectedIndex == 0)
                {
                    lblPlazaR.Text = campo;
                    GridView1.DataSource = ds.Tables[0];
                    GridView1.DataBind();
                    MainView.ActiveViewIndex = 3;
                }
            }
            else if (drpDes.SelectedIndex == 3)
            {

                //Reporteo de aforo acumulado mensual corregido
                lblBan.Text = "DELEGACION" +
                    ',' + "PLAZA_COBRO" + ',' + "DESCRIPCION" + ',' + "TAG6" +
                    ',' + "TAG0" + ',' + "TAG_TDC_RESPALDO" + ',' + "TAG_TDD_RESPALDO" +
                    ',' + "RESIDENTES" + ',' + "PAGO_POR_RECORRIDO" + ',' + "RESIDENTESTDC" +
                    ',' + "PPRTDC" + ',' + "RESIDENTESTDD" + ',' + "PPRTDD" + ',' + "PREPAGO" +
                    ',' + "ING_NETO" + ',' + "DESCTO" + ',' + "DESCTO_RESIDENTES" + ',' +
                    "DESCTO_PR" + ',' + "VALUACION_DE_EXENTOS" + ',' + "INGRESO_BRUTO";
                drpPlazaR.SelectedIndex = 0;
                ini = Convert.ToDateTime(txtFechaBase0.Text);
                fin = Convert.ToDateTime(txtFechaBase1.Text);
                plaza = "";
                if (drpTipoRed.SelectedValue == "1")
                {
                    plaza = drpPlazaR.SelectedValue;
                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR.SelectedValue);
                }
                if (drpTipoRed.SelectedValue == "3")
                {
                    plaza = drpPlazaR2.SelectedValue;

                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR2.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR2.SelectedValue);
                }
                if (drpTipoRed.SelectedValue == "2")
                {
                    plaza = drpPlazaR1.SelectedValue;

                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR1.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR1.SelectedValue);
                }

                ds = conexion.reporteDID2(ini, fin, drpTipoRed.SelectedIndex, LBLPRUEBA.Text);
                GridView1.DataSource = ds.Tables[0];

                GridView1.DataBind();
                lblrepor.Text = "Desagregado de Aforo Mensual de Medios Electrónicos de Pago por Plaza de Cobro";

                titulo_archivos = "Aforo_mensual";
                MainView.ActiveViewIndex = 3;

            }
            else if (drpDes.SelectedIndex == 4)
            {

                lblBan.Text = "DELEGACION" +
                    ',' + "PLAZA_COBRO" + ',' + "DESCRIPCION" + ',' + "TAG6" +
                    ',' + "TAG0" + ',' + "TAG_TDC_RESPALDO" + ',' + "TAG_TDD_RESPALDO" +
                    ',' + "RESIDENTES" + ',' + "PAGO_POR_RECORRIDO" + ',' + "RESIDENTESTDC" +
                    ',' + "PPRTDC" + ',' + "RESIDENTESTDD" + ',' + "PPRTDD" + ',' + "PREPAGO" +
                    ',' + "ING_NETO" + ',' + "DESCTO" + ',' + "DESCTO_RESIDENTES" + ',' +
                    "DESCTO_PR" + ',' + "VALUACION_DE_EXENTOS" + ',' + "INGRESO_BRUTO";

                ini = Convert.ToDateTime(txtFechaBase0.Text);
                fin = Convert.ToDateTime(txtFechaBase1.Text);


                lblPlazaCobro.Text = drpPlazaR.SelectedValue;

                ds = conexion.reporteDID2(ini, fin, drpTipoRed.SelectedIndex, LBLPRUEBA.Text);
                GridView1.DataSource = ds.Tables[0];

                GridView1.DataBind();
                lblrepor.Text = "Desagregado de Ingreso Mensual de Medios Electrónicos de Pago por Plaza de Cobro";
                titulo_archivos = "Des_ing_m";

                MainView.ActiveViewIndex = 3;

            }
            else if (drpDes.SelectedIndex == 5)
            {


                lblBan.Text = "Fecha " + ',' + "Plaza de Cobro" + ',' + "Fecha Transaccion" + ',' + "Hora Transaccion" + ',' + "Número de Tarjeta" + ',' + "Estado Tarjeta" + ',' + "Carril" + ',' + "Evento" + ',' + "Clase" + ',' + "Tipo" + ',' + "Tramo" + ',' + "ExentoVig" + ',' + "DESCUENTO" + ',' + "Forma de Pago" + ',' + "Descuento TILH" + ',' + "Importe total con ejes e IVA";
                ini = Convert.ToDateTime(txtFechaBase0.Text);
                fin = Convert.ToDateTime(txtFechaBase1.Text);
                plaza = "";
                if (drpTipoRed.SelectedValue == "1")
                {
                    plaza = drpPlazaR.SelectedValue;
                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR.SelectedValue);
                }
                if (drpTipoRed.SelectedValue == "3")
                {
                    plaza = drpPlazaR2.SelectedValue;

                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR2.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR2.SelectedValue);
                }
                if (drpTipoRed.SelectedValue == "2")
                {
                    plaza = drpPlazaR1.SelectedValue;

                    lblPlazaCobro.Text = Convert.ToString(drpPlazaR1.SelectedItem);
                    lblNoPlazaCobro.Text = Convert.ToString(drpPlazaR1.SelectedValue);
                }

                ds = conexion.reporteDID(ini, fin, plaza, drpTipoRed.SelectedIndex, LBLPRUEBA.Text);
                GridView1.DataSource = ds.Tables[0];

                GridView1.DataBind();
                lblrepor.Text = "Detalle de Dictamen Final Mensual";
                titulo_archivos = "Dict_mensual";
                //  flagHeaderCsv = "TAG6" + ',' + "TAG0" + ',' + "TCR" + ',' + "TDR" + ',' + "RES" + ',' + "RESTDC" + ',' + "RESTDD" + ',' + "PPR" + ',' + "PPRTDC" + ',' + "PPRTDD" + ',' + "INGNETO" + ',' + "DESCTO6" + ',' + "DESCRES" + ',' + "DESCPPR" + ',' + "INGBRUTO" + ',' + "VALEXENTOS";

                MainView.ActiveViewIndex = 3;
                if (drpPlazaR.SelectedIndex == 0)
                {
                    lblPlazaR.Text = campo;
                    GridView1.DataSource = ds.Tables[0];
                    GridView1.DataBind();
                    MainView.ActiveViewIndex = 3;
                }
            }


            //Termina seleccion de reportes




            if (GridView1.Rows.Count == 0)
            {
                lblapr.Visible = true;
                lblapr.Text = "No se encontraron resultados para tu consulta";
                btnPdf.Visible = false;
                btnCsv.Visible = false;

            }

            else if (GridView1.Rows.Count > 0)
            {
                lblApertura.Text = "";
                btnPdf.Visible = true;
                btnCsv.Visible = true;
            }


        }

        protected void btnLimpiarR_Click(object sender, EventArgs e)
        {

            limpiar(1);


        }

        protected void btnsalirReportes_Click(object sender, EventArgs e)
        {
            salir();
        }

        


        private void limpiar(int flag)
        {
            if (flag == 0)
            {
                drpPlaza.SelectedValue = "0";
                Calendar.Visible = false;
                txtFecha.Text = "";
                lblFechaA.Text = "";
            }
            else if (flag == 1)
            {
                cerrar_calendarios();
                txtFechaBase0.Text = "";
                txtFechaBase1.Text = "";
                drpPlazaR.SelectedIndex = 0;
                drpPlazaR1.SelectedIndex = 0;
                drpPlazaR2.SelectedIndex = 0;
                drpPlazaR3.SelectedIndex = 0;
                drpTipoRed.SelectedIndex = 0;
                drpDes.SelectedIndex = 0;
                drpPlazaR3.Visible = true;
                drpPlazaR2.Visible = false;
                drpPlazaR1.Visible = false;
                drpPlazaR.Visible = false;
                lblPlazaC.Visible = true;
                btnFin.Enabled = false;
                lblErrorFecha.Text = "";
                lblFechaini.Text = "";
                lblPlazaR.Text = "";
            }
            else { }
        }


        private void salir()
        {
            Response.Redirect("login.aspx");
            Session["usuario"] = null;
        }

        private void cerrar_calendarios()
        {
            CalendarInicio.Visible = false;
            CalendarFin.Visible = false;
            Calendar.Visible = false;
        }

        protected void drpPlaza_SelectedIndexChanged(object sender, EventArgs e)
        {
            cerrar_calendarios();
            Calendar.Visible = false;


        }

        protected void bntReturn_Click(object sender, EventArgs e)
        {
            MainView.ActiveViewIndex = 1;
            limpiar(0);
            limpiar(1);
            drpPlazaR.Enabled = true;
            CalendarInicio.SelectedDate = DateTime.Now;
            CalendarFin.SelectedDate = DateTime.Now;
            lblapr.Text = "";
            lblapr.Visible = false;
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            return;
        }

        protected void btnXml_Click(object sender, EventArgs e)
        {
            string nombreDelReporte = "";

            if (drpDes.SelectedIndex == 1)
            {
                nombreDelReporte = "RT2A_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }
            else if (drpDes.SelectedIndex == 2)
            {
                nombreDelReporte = "RT1A_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }
            else if (drpDes.SelectedIndex == 3)
            {
                nombreDelReporte = "RT7B_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }
            else if (drpDes.SelectedIndex == 4)
            {
                nombreDelReporte = "RT7A_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }
            string respuesta = "";

            if (drpTipoRed.SelectedIndex == 1)
            {
                respuesta = "Red Propia";
            }
            else if (drpTipoRed.SelectedIndex == 2)
            {
                respuesta = "Red Fonadin";
            }
            else if (drpTipoRed.SelectedIndex == 3)
            {
                respuesta = "Red Contratada";
            }


            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + nombreDelReporte + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder sBuilder = new System.Text.StringBuilder();

            int help = Convert.ToInt32(lblNoPlazaCobro.Text);

            sBuilder.Append("CAMINOS Y PUENTES FEDERALES DE INGRESO Y SERVICIOS CONEXOS \r\n");
            sBuilder.Append("DELEGACION REGIONAL IX OCCIDENTE-GDL\r\n");
            if (help > 0)
                sBuilder.Append("PLAZA DE COBRO:" + lblPlazaCobro.Text + "\r\n");
            sBuilder.Append(lblrepor.Text + "\r\n");
            sBuilder.Append("Del " + txtFechaBase0.Text + " al " + txtFechaBase1.Text + "\r\n");
            sBuilder.Append(respuesta + "\r\n");
            sBuilder.Append(lblBan.Text + "\r\n");



            for (int index = 0; index < GridView1.Columns.Count; index++)
            {
                sBuilder.Append(GridView1.Columns[index].HeaderText + ',');
            }
            sBuilder.Append("\r\n");
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                for (int k = 0; k < GridView1.HeaderRow.Cells.Count; k++)
                {
                    sBuilder.Append(GridView1.Rows[i].Cells[k].Text.Replace(",", "") + ",");
                }
                sBuilder.Append("\r\n");
            }
            Response.Output.Write(sBuilder.ToString());
            Response.Flush();
            Response.End();

        }

        //Se crea documento pdf 
        protected void btnPdf_Click(object sender, EventArgs e)
        {

            string nombreDelReporte = "";
            if (drpDes.SelectedIndex == 1)
            {
                nombreDelReporte = "RT2A_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }
            else if (drpDes.SelectedIndex == 2)
            {
                nombreDelReporte = "RT1A_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }
            else if (drpDes.SelectedIndex == 3)
            {
                nombreDelReporte = "RT7B_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }
            else if (drpDes.SelectedIndex == 4)
            {
                nombreDelReporte = "RT7A_" + DateTime.Now.ToString("ddMMyyyyhhmm");
            }

            String fecha = DateTime.Now.ToString("dd/MM/yyy");
            String hra = DateTime.Now.ToString("HH:mm");
            string aux = "Plaza :";
            string respuesta = "";
            string imageURL = Server.MapPath(".") + "/images/2402a86a829694622ed63ee98eb9e836.jpg";

            if (drpTipoRed.SelectedIndex == 1)
            {
                respuesta = "Red Propia";
            }
            else if (drpTipoRed.SelectedIndex == 2)
            {
                respuesta = "Red Fonadin";
            }
            else if (drpTipoRed.SelectedIndex == 3)
            {
                respuesta = "Red Contratada";
            }
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
            aux += lblPlazaCobro.Text;

            jpg.ScaleToFit(140f, 120f);
            jpg.SpacingBefore = 10f;
            jpg.SpacingAfter = 1f;
            jpg.Alignment = Element.ALIGN_LEFT;
            Paragraph fechaEj = new Paragraph(string.Format("Fecha ejec:") + fecha);

            Paragraph hora = new Paragraph(string.Format("Hora:" + hra));

            Paragraph segundo = new Paragraph(string.Format("CAMINOS Y PUENTES FEDERALES DE INGRESOS Y SERVICIOS CONEXOS"));

            Paragraph tercero = new Paragraph(string.Format(lblrepor.Text));

            Paragraph plazaCobro = new Paragraph(string.Format("PLAZA DE COBRO:" + lblPlazaCobro.Text));

            Paragraph cuarto = new Paragraph(flagHeaderPdf);

            Paragraph quinto = new Paragraph(respuesta);

            fechaEj.Alignment = Element.ALIGN_RIGHT;
            hora.Alignment = Element.ALIGN_RIGHT;
            segundo.Alignment = Element.ALIGN_CENTER;
            tercero.Alignment = Element.ALIGN_CENTER;
            cuarto.Alignment = Element.ALIGN_CENTER;
            quinto.Alignment = Element.ALIGN_CENTER;
            plazaCobro.Alignment = Element.ALIGN_CENTER;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + nombreDelReporte + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.AllowPaging = false;
            GridView1.RenderControl(hw);
            GridView1.HeaderRow.Style.Add("width", "15%");
            GridView1.HeaderRow.Style.Add("font-size", "10px");
            GridView1.Style.Add("text-decoration", "none");
            GridView1.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            GridView1.Style.Add("font-size", "8px");
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A2, 7f, 7f, 7f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            int help = Convert.ToInt32(lblNoPlazaCobro.Text);

            pdfDoc.Add(jpg);
            pdfDoc.Add(segundo);
            pdfDoc.Add(tercero);
            if (Convert.ToInt32(lblNoPlazaCobro.Text) > 0)
                pdfDoc.Add(plazaCobro);
            pdfDoc.Add(cuarto);
            pdfDoc.Add(quinto);
            pdfDoc.Add(fechaEj);
            pdfDoc.Add(hora);



            htmlparser.Parse(sr);
            pdfDoc.Add(new Paragraph("\n\n\n\n\n\n\n\n                     ______________________________" + "                                                                                       ______________________________"));
            pdfDoc.Add(new Paragraph("\n                     Alejandro Roberto Espinosa Organista" + "                                                                                               Cesar Hernandez Contez" + "\n" +
                "                    Subgerente de Control de Operaciones de MEP" + "                                                                                             Proveedor\n" +
                "                                                                                                                                                                                  Gerente de Conciliaciones \n" +
                "                                                                                                                                                                                               TEDISA"));
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();


        }


        protected void pdbittrf()
        {
            string regDate;
            DateTime dt = DateTime.Now;

            regDate = dt.ToString("dd/MM/yyyy hh:mm:ss");
            secpdbittrf();

            try
            {
                string oradb = " Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = OFC522.CAPUFE.GOB.MX)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = MCMEPDEV))); PASSWORD = DARWINDES; USER ID = DARWINDES";
                OracleConnection conn = new OracleConnection(oradb);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Insert into pdbittrf (CVE_PZCOBRO,FCH_BASE,NOM_ARCH_COMPAC,CVE_EORG,FCHHR_TRANSFER,SEC_TRANSFER,USR_UMOD,PRC_UMOD,FCH_UMOD,USR_APP) VALUES('191', '" + fecha + "' ,'" + nomarch2 + "','3',to_date('" + regDate + "' ,'dd/mm/yyyy hh24:mi:ss'),'" + sec + "','" + Session["USUARIO"] + "','STILH',to_date('" + regDate + "' ,'dd/mm/yyyy hh24:mi:ss'),'" + Session["USUARIO"] + "')";
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();

            }
            catch (Exception e)
            {
                ContentsLabel.Text = "Error al insertar en PDBITTRF: " + e.Message;

            }
        }
        protected void pdbitimp()
        {
            string regDate;
            DateTime dt = DateTime.Now;
            regDate = dt.ToString("dd/MM/yyyy hh:mm:ss");

            secpdbitimp();
            try
            {
                string oradb = " Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = OFC522.CAPUFE.GOB.MX)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = MCMEPDEV))); PASSWORD = DARWINDES; USER ID = DARWINDES";
                OracleConnection conn = new OracleConnection(oradb);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Insert into pdbitimp (NOM_ARCH_COMPAC,LST_TIPO_ARCH,NOM_ARCHIVO,CVE_PZCOBRO,FCH_BASE,NUM_REGISTROS,FCHHR_INCORPORA,SEC_INCORPORA,USR_UMOD,PRC_UMOD,FCH_UMOD,USR_APP,CIFRA_CTRL) VALUES('" + nomarch2 + "', 'H' ,'" + nomarch2 + "','191','" + fecha + "','" + numregistro + "',to_date('" + regDate + "' ,'dd/mm/yyyy hh24:mi:ss'),'" + sec2 + "','" + Session["USUARIO"] + "','STILH',to_date('" + regDate + "' ,'dd/mm/yyyy hh24:mi:ss'),'" + Session["USUARIO"] + "','0')";
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                ContentsLabel.Text = "Error al insertar en PDBITIMP: " + e.Message;

            }

        }
        protected void pdprvtil()
        {
            string regDate;
            DateTime dt = DateTime.Now;
            regDate = dt.ToString("dd/MM/yyyy hh:mm:ss");

            try
            {
                string oradb = " Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = OFC522.CAPUFE.GOB.MX)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = MCMEPDEV))); PASSWORD = DARWINDES; USER ID = DARWINDES";
                OracleConnection conn = new OracleConnection(oradb);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Insert into pdprvtil (FCH_TRANSACCION,HR_TRANSACCION,CVE_CARRIL,NUM_TARJETA,FCH_BASE,CVE_PZCOBRO,IND_CUERPO,ID_CARRIL,NUM_EVENTO,NUM_FOLIO,CVE_TURNO,CVE_TRAM_CAM,CVE_CLSVEHICULO,CVE_TVEHICULO,CVE_EDOTARJETA,IND_EMPRESA_ADMON_TARJ,CVE_FPAGO,IND_EXENTO,CVE_FPAGO2,PC_DESCUENTO,CVE_TPOCL,IMPORTE_DISPERSADO,CVE_EDOCONCILIACION_MEP,NUM_TRANSVAL_PROV,NUM_TRANSVAL_CAPUFE,NUM_TRANSVAL_TILH,IND_DSCTO_TILH,PC_DESCUENTO_TILH,DURACION,NUM_EJEXL,NUM_EJEXP,NOM_ARCHIVO,FCHHR_INCORPORA,SEC_INCORPORA,FCH_UMOD,USR_UMOD,USR_APP,PRC_UMOD) VALUES('" + fec_base + "','" + hora + "','" + cve_carril + "','" + num_tarjeta + "','" + fec_base + "','191','" + ind_cuerpo + "','" + id_carril + "','" + evento + "','" + folio + "','" + cve_turno + "','" + cve_tram_cam + "','" + cve_clsvehiculo + "','" + cve_tvhiculo + "','" + cve_edotarjeta + "','" + ind_empresa_admon_tarj + "','" + cve_fpago + "','" + ind_exento + "','" + cve_fpago2 + "','" + pc_descuento + "','" + cve_tpocl + "','" + importe_dispersado.TrimEnd() + "','IM',null,null,'" + sec3 + "','N',null,null,null,null,'" + nomarch2 + "',to_date('" + regDate + "' ,'dd/mm/yyyy hh24:mi:ss'),'" + sec3 + "',to_date('" + regDate + "' ,'dd/mm/yyyy hh24:mi:ss'),'" + Session["USUARIO"] + "','" + Session["USUARIO"] + "','STILH')";
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                variable = "";
                variable2 = "";
                fec_base = "";
                hora = "";
                cve_carril = "";
                ind_cuerpo = "";
                id_carril = "";
                num_tarjeta = "";
                evento = "";
                folio = "";
                cve_turno = "";
                cve_tram_cam = "";
                cve_clsvehiculo = "";
                cve_tvhiculo = "";
                cve_edotarjeta = "";
                ind_empresa_admon_tarj = "";
                cve_fpago = "";
                ind_exento = "";
                cve_fpago2 = "";
                pc_descuento = "";
                cve_tpocl = "";
                importe_dispersado = "";
                sec = "";
                sec2 = "";
                sec3 = "";

            }
            catch (Exception e)
            {
                ContentsLabel.Text = "Error al insertar en pdprvtil: " + e.Message;
            }
        }


        protected void secpdbitimp()
        {
            string dato = "";

            try
            {

                string oradb = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = OFC522.CAPUFE.GOB.MX)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = MCMEPDEV)));PASSWORD=DARWINDES;USER ID=DARWINDES";
                string sql = "SELECT PSBITIMP.NEXTVAL FROM DUAL ";
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                OracleDataAdapter cper = new OracleDataAdapter(cmd);
                cper.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataRow rec = ds.Tables[0].Rows[0];
                    dato = rec["max(SEC_INCORPORA)"].ToString();

                    if (dato == "")
                    {
                        sec2 = "0";
                    }
                    else
                    {
                        sec2 = dato + 1;
                    }
                }

            }
            catch (Exception e)
            {
                ContentsLabel.Text = "Error al obtener secuencia de PDBITIMP: " + e.Message;

            }

        }


        protected void secpdbittrf()
        {
            string dato = "";

            try
            {

                string oradb = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = OFC522.CAPUFE.GOB.MX)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = MCMEPDEV)));PASSWORD=DARWINDES;USER ID=DARWINDES";
                string sql = "select max(sec_transfer) from pdbittrf where cve_pzcobro = 191";
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                OracleDataAdapter cper = new OracleDataAdapter(cmd);
                cper.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataRow rec = ds.Tables[0].Rows[0];
                    dato = rec["max(sec_transfer)"].ToString();

                    if (dato == "")
                    {
                        sec = "0";
                    }
                    else
                    {
                        sec = dato + 1;
                    }
                }

            }
            catch (Exception e)
            {
                ContentsLabel.Text = "Error al obtener secuencia de PDBITTRF: " + e.Message;

            }
        }
        protected void secpdprvtil()
        {
            string dato = "";

            try
            {

                string oradb = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = OFC522.CAPUFE.GOB.MX)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = MCMEPDEV)));PASSWORD=DARWINDES;USER ID=DARWINDES";
                string sql = "select max(num_transval_tilh) from pdprvtil where cve_pzcobro = 191";
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                OracleDataAdapter cper = new OracleDataAdapter(cmd);
                cper.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataRow rec = ds.Tables[0].Rows[0];
                    dato = rec["max(num_transval_tilh)"].ToString();

                    if (dato == "")
                    {
                        sec3 = "0";
                    }
                    else
                    {
                        sec3 = dato + 1;
                    }
                }

            }
            catch (Exception e)
            {
                ContentsLabel.Text = "Error al obtener secuencia de PDPRVTIL: " + e.Message;

            }
        }







        protected void drpDes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpDes.SelectedIndex == 1)
            {
                LBLPRUEBA.Text = "1";
                lblPlazaR.Enabled = true;
                lblPlazaC.Enabled = true;
                drpPlazaR3.Enabled = true;
                drpTipoRed.Enabled = false;

                lblPlazaR.Visible = true;
                lblPlazaC.Visible = true;
                drpPlazaR3.Visible = true;
                drpPlazaR2.Visible = false;
                drpPlazaR1.Visible = false;
                drpPlazaR.Visible = false;
                drpTipoRed.Visible = false;
                Label1.Visible = false;
            }
            if (drpDes.SelectedIndex == 2)
            {
                LBLPRUEBA.Text = "2";
                lblPlazaR.Enabled = true;
                lblPlazaC.Enabled = true;
                drpPlazaR3.Enabled = true;
                drpTipoRed.Enabled = true;

                lblPlazaR.Visible = true;
                lblPlazaC.Visible = true;
                drpPlazaR3.Visible = true;
                drpTipoRed.Visible = true;
                Label1.Visible = true;
            }

            if (drpDes.SelectedIndex == 3)
            {
                LBLPRUEBA.Text = "3";
                lblPlazaR.Enabled = false;
                lblPlazaC.Enabled = false;
                drpPlazaR3.Enabled = false;
                drpTipoRed.Enabled = true;

                lblPlazaR.Visible = false;
                lblPlazaC.Visible = false;
                drpPlazaR3.Visible = false;
                drpTipoRed.Visible = true;
                Label1.Visible = true;
            }
            if (drpDes.SelectedIndex == 4)
            {
                LBLPRUEBA.Text = "4";
                lblPlazaR.Enabled = false;
                lblPlazaC.Enabled = false;
                drpPlazaR3.Enabled = false;
                drpTipoRed.Enabled = true;

                lblPlazaR.Visible = false;
                lblPlazaC.Visible = false;
                drpPlazaR3.Visible = false;
                drpTipoRed.Visible = true;
            }
            if (drpDes.SelectedIndex == 5)
            {
                LBLPRUEBA.Text = "5";
                lblPlazaR.Enabled = true;
                lblPlazaC.Enabled = true;
                drpPlazaR3.Enabled = true;
                drpTipoRed.Enabled = true;

                lblPlazaR.Visible = true;
                lblPlazaC.Visible = true;
                drpPlazaR3.Visible = true;
                drpTipoRed.Visible = true;
                Label1.Visible = true;
            }
        }


        protected static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a, b);
            return temp;
        }

        protected void sacar()
        {
            con2 = 0;
            variable = "";
            ban = 0;
            todo = LengthLabel.Text;
            for (int i = 0; i < todo.Length; i++)
            {
                a = Mid(todo, i, 1);

                if (con2 == 4)
                {
                    pzcobro = variable; 
                    variable = "";
                }
                if (con2 == 12)
                {
                    fecha = variable; 
                    variable = "";
                    pdbittrf();
                }
                if (con2 == 17)
                {
                    numregistro = variable;
                    if (numregistro == "00000")
                    {
                        ContentsLabel.Text = "Archivo sin registros";
                    }
                    else
                    {

                        for (int y = 0; y < 5; y++)
                        {

                            b = Mid(numregistro, y, 1);

                            if (b == "0")
                            {

                            }
                            else
                            {

                                variable2 = variable2 + b;
                            }
                        }
                        numregistro = variable2;

                    }

                }
                
                if (a == "10")
                {
                    if (ban == 0)
                    {
                        index = pzcobro + fecha + numregistro;
                        pdbitimp();
                        con = 1;
                        variable = "";
                        ban = 1;
                    }
                    if (con == 20)
                    {
                        importe_dispersado = variable;
                        con = 1;
                        variable = "";
                        ban = 1;
                        pdprvtil();
                    }
                    if (con == 20)
                    {
                        importe_dispersado = variable;

                        con = 1;
                        variable = "";

                        pdprvtil();
                    }

                }

                variable = variable + a;
                con2 = con2 + 1;
                if (a == "44")
                {
                    if (con == 1)
                    {
                        fec_base = variable;
                        variable2 = "";
                        for (int s = 0; s < fec_base.Length; s++)
                        {
                            b = Mid(fec_base, s, 1);
                            if (b == ",")
                            {
                            }
                            else
                            {
                                variable2 = variable2 + b;
                            }
                        }
                        fec_base = variable2;
                        con = 2;
                        variable = "";
                    }
                    else
                    {
                        if (con == 2)
                        {
                            hora = variable;
                            variable2 = "";
                            for (int g = 0; g < hora.Length; g++)
                            {
                                b = Mid(hora, g, 1);
                                if (b == ",")
                                {

                                }
                                else
                                {
                                    variable2 = variable2 + b;
                                }
                            }
                            hora = variable2;
                            con = 3;
                            variable = "";
                        }
                        else
                        {
                            if (con == 3)
                            {
                                cve_carril = variable;
                                variable2 = "";
                                for (int y = 0; y < cve_carril.Length; y++)
                                {
                                    b = Mid(cve_carril, y, 1);
                                    if (b == ",")
                                    {
                                    }
                                    else
                                    {
                                        variable2 = variable2 + b;
                                    }
                                }
                                cve_carril = variable2;
                                con = 4;
                                variable = "";
                            }
                            else
                            {
                                if (con == 4)
                                {
                                    ind_cuerpo = variable;
                                    variable2 = "";
                                    for (int y = 0; y < ind_cuerpo.Length; y++)
                                    {
                                        b = Mid(ind_cuerpo, y, 1);
                                        if (b == ",")
                                        {
                                        }
                                        else
                                        {
                                            variable2 += b;
                                        }
                                    }
                                    ind_cuerpo = variable2;
                                    con = 5;
                                    variable = "";
                                }
                                else
                                {
                                    if (con == 5)
                                    {
                                        id_carril = variable;
                                        variable2 = "";
                                        for (int y = 0; y < id_carril.Length; y++)
                                        {
                                            b = Mid(id_carril, y, 1);
                                            if (b == ",")
                                            {
                                            }
                                            else
                                            {
                                                variable2 = variable2 + b;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (con == 6)
                                        {
                                            num_tarjeta = variable;
                                            variable2 = "";
                                            for (int y = 0; y < num_tarjeta.Length; y++)
                                            {
                                                b = Mid(num_tarjeta, y, 1);
                                                if (b == ",")
                                                {
                                                }
                                                else
                                                {
                                                    variable2 += b;
                                                }
                                            }
                                            num_tarjeta = variable2;

                                            con = 7;
                                            variable = "";
                                        }
                                        else
                                        {
                                            if (con == 7)
                                            {
                                                evento = variable;
                                                variable2 = "";
                                                for (int y = 0; y < evento.Length; y++)
                                                {
                                                    b = Mid(evento, y, 1);
                                                    if (b == ",")
                                                    {
                                                    }
                                                    else
                                                    {
                                                        variable2 += b;
                                                    }
                                                }
                                                evento = variable2;
                                                con = 8;
                                                variable = "";
                                            }
                                            else
                                            {
                                                if (con == 8)
                                                {
                                                    folio = variable;
                                                    variable2 = "";
                                                    for (int y = 0; y < folio.Length; y++)
                                                    {
                                                        b = Mid(folio, y, 1);
                                                        if (b == ",")
                                                        {
                                                        }
                                                        else
                                                        {
                                                            variable2 += b;
                                                        }
                                                    }
                                                    folio = variable2;
                                                    con = 9;
                                                    variable = "";
                                                }
                                                else
                                                {
                                                    if (con == 9)
                                                    {
                                                        cve_turno = variable;
                                                        variable2 = "";
                                                        for (int y = 0; y < cve_turno.Length; y++)
                                                        {
                                                            b = Mid(cve_turno, y, 1);
                                                            if (b == ",")
                                                            {
                                                            }
                                                            else
                                                            {
                                                                variable2 += b;
                                                            }
                                                        }
                                                        cve_turno = variable2;

                                                        con = 10;
                                                        variable = "";

                                                    }
                                                    else
                                                    {
                                                        if (con == 10)
                                                        {
                                                            cve_tram_cam = variable;
                                                            variable2 = "";
                                                            for (int y = 0; y < cve_tram_cam.Length; y++)
                                                            {
                                                                b = Mid(cve_tram_cam, y, 1);
                                                                if (b == ",")
                                                                {
                                                                }
                                                                else
                                                                {
                                                                    variable2 += b;
                                                                }
                                                            }
                                                            cve_tram_cam = variable2;

                                                            con = 11;
                                                            variable = "";
                                                        }
                                                        else
                                                        {
                                                            if (con == 11)
                                                            {
                                                                cve_clsvehiculo = variable;
                                                                variable2 = "";
                                                                for (int y = 0; y < cve_clsvehiculo.Length; y++)
                                                                {
                                                                    b = Mid(cve_clsvehiculo, y, 1);
                                                                    if (b == ",")
                                                                    {
                                                                    }
                                                                    else
                                                                    {
                                                                        variable2 += b;
                                                                    }
                                                                }
                                                                cve_clsvehiculo = variable2;

                                                                con = 12;
                                                                variable = "";
                                                            }
                                                            else
                                                            {
                                                                if (con == 12)
                                                                {
                                                                    cve_tvhiculo = variable;
                                                                    variable2 = "";
                                                                    for (int y = 0; y < cve_tvhiculo.Length; y++)
                                                                    {
                                                                        b = Mid(cve_tvhiculo, y, 1);
                                                                        if (b == ",")
                                                                        {
                                                                        }
                                                                        else
                                                                        {
                                                                            variable2 += b;
                                                                        }
                                                                    }
                                                                    cve_tvhiculo = variable2;

                                                                    con = 13;
                                                                    variable = "";
                                                                }
                                                                else
                                                                {
                                                                    if (con == 13)
                                                                    {
                                                                        cve_edotarjeta = variable;
                                                                        variable2 = "";
                                                                        for (int y = 0; y < cve_edotarjeta.Length; y++)
                                                                        {
                                                                            b = Mid(cve_edotarjeta, y, 1);
                                                                            if (b == ",")
                                                                            {
                                                                            }
                                                                            else
                                                                            {
                                                                                variable2 += b;
                                                                            }
                                                                        }
                                                                        cve_edotarjeta = variable2;

                                                                        con = 14;
                                                                        variable = "";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (con == 14)
                                                                        {
                                                                            ind_empresa_admon_tarj = variable;
                                                                            variable2 = "";
                                                                            for (int y = 0; y < ind_empresa_admon_tarj.Length; y++)
                                                                            {
                                                                                b = Mid(ind_empresa_admon_tarj, y, 1);
                                                                                if (b == ",")
                                                                                {
                                                                                }
                                                                                else
                                                                                {
                                                                                    variable2 += b;
                                                                                }
                                                                            }
                                                                            ind_empresa_admon_tarj = variable2;

                                                                            con = 15;
                                                                            variable = "";
                                                                        }
                                                                        else
                                                                        {
                                                                            if (con == 15)
                                                                            {
                                                                                cve_fpago = variable;
                                                                                variable2 = "";
                                                                                for (int y = 0; y < cve_fpago.Length; y++)
                                                                                {
                                                                                    b = Mid(cve_fpago, y, 1);
                                                                                    if (b == ",")
                                                                                    {
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        variable2 += b;
                                                                                    }
                                                                                }
                                                                                cve_fpago = variable2;

                                                                                con = 16;
                                                                                variable = "";
                                                                            }
                                                                            else
                                                                            {
                                                                                if (con == 16)
                                                                                {
                                                                                    ind_exento = variable;
                                                                                    variable2 = "";
                                                                                    for (int y = 0; y < ind_exento.Length; y++)
                                                                                    {
                                                                                        b = Mid(ind_exento, y, 1);
                                                                                        if (b == ",")
                                                                                        {
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            variable2 += b;
                                                                                        }
                                                                                    }
                                                                                    ind_exento = variable2;

                                                                                    con = 17;
                                                                                    variable = "";
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (con == 17)
                                                                                    {
                                                                                        cve_fpago2 = variable;
                                                                                        variable2 = "";
                                                                                        for (int y = 0; y < cve_fpago2.Length; y++)
                                                                                        {
                                                                                            b = Mid(cve_fpago2, y, 1);
                                                                                            if (b == ",")
                                                                                            {
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                variable2 += b;
                                                                                            }
                                                                                        }
                                                                                        cve_fpago2 = variable2;

                                                                                        con = 18;
                                                                                        variable = "";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (con == 18)
                                                                                        {
                                                                                            pc_descuento = variable;
                                                                                            variable2 = "";
                                                                                            for (int y = 0; y < pc_descuento.Length; y++)
                                                                                            {
                                                                                                b = Mid(pc_descuento, y, 1);
                                                                                                if (b == ",")
                                                                                                {
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    variable2 += b;
                                                                                                }
                                                                                            }
                                                                                            pc_descuento = variable2;

                                                                                            con = 19;
                                                                                            variable = "";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (con == 19)
                                                                                            {
                                                                                                cve_tpocl = variable;
                                                                                                variable2 = "";
                                                                                                for (int y = 0; y < cve_tpocl.Length; y++)
                                                                                                {
                                                                                                    b = Mid(cve_tpocl, y, 1);
                                                                                                    if (b == ",")
                                                                                                    {
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        variable2 += b;
                                                                                                    }
                                                                                                }
                                                                                                cve_tpocl = variable2;

                                                                                                con = 20;
                                                                                                variable = "";
                                                                                            }
                                                                                        }

                                                                                    }
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }


                        }
                    }

                }

            }
            nom = "";
            b = "";
            todo = "";
            index = "";
            pzcobro = "";
            fecha = "";
            numregistro = "";
            variable = "";
            variable2 = "";
            fec_base = "";
            hora = "";
            cve_carril = "";
            ind_cuerpo = "";
            id_carril = "";
            num_tarjeta = "";
            evento = "";
            folio = "";
            cve_turno = "";
            cve_tram_cam = "";
            cve_clsvehiculo = "";
            cve_tvhiculo = "";
            cve_edotarjeta = "";
            ind_empresa_admon_tarj = "";
            cve_fpago = "";
            ind_exento = "";
            cve_fpago2 = "";
            pc_descuento = "";
            cve_tpocl = "";
            importe_dispersado = "";
            sec = "";
            sec2 = "";
            sec3 = "";
            nom = "";
            con = 0;
            con2 = 0;
            ban = 0;
            LengthLabel.Text = "";

        }


        protected void btnSubir_Click(object sender, EventArgs e)
        {
            nomarch2 = FileUpload1.FileName;
            con2 = 1;
            variable = "";
            ContentsLabel.Text = "";

            for (int i = 0; i < nomarch2.Length; i++)
            {
                a = Mid(nomarch2, i, 1);
                variable = variable + a;
                plaza = variable;
                variable = "";
                if (con2 == 4)
                {
                }
                else if (con2 == 8)
                {
                    año = variable;
                    variable = "";
                }
                else if (con2 == 10)
                {
                    mes = variable;
                    variable = "";
                    break;
                }
            }

            if (Directory.Exists("c:/archivosTilh/Procesados 191/" + año))
            {
                Directory.CreateDirectory("c:/archivosTilh/Procesados 191/" + año);
            }
            if (Directory.Exists("c:/archivosTilh/Procesados 191/" + año + "/" + mes))
            {
                Directory.CreateDirectory("c:/archivosTilh/Procesados 191/" + año + "/" + mes);
            }
            if (FileUpload1.HasFile)
            {

                string savePath = "c:/archivosTilh/Procesados 191/" + año + "/" + mes + "/";
                savePath += FileUpload1.FileName;

                if (File.Exists(savePath))
                {
                    UploadStatusLabel.Text = "El archivo " + FileUpload1.FileName.ToString() + " ya fue cargado anteriormente, intente con otro archivo";
                }
                else
                {
                    FileUpload1.SaveAs(savePath);
                    nomarch = savePath;

                    using (StreamReader reader = new StreamReader(nomarch, Encoding.Default))
                    {
                        LengthLabel.Text = reader.ReadToEnd();
                    }

                    UploadStatusLabel.Text = "El archivo " + FileUpload1.FileName.ToString() + " se ha guardado exitosamente en el directorio";
                    
                }
            }
            else
            {
                UploadStatusLabel.Text = "Es necesario seleccionar un archivo para TILH.";
            }

        }
        /// <summary>
        /// Este metodo simplemente envia un correo cada vez que se invoque 
        /// </summary>
        protected void enviarCorreo()
        {

            string cve_plaza = drpPlaza.SelectedValue;
            string fechaBase = txtFecha.Text;
            SmtpClient smtpClient = new SmtpClient();

            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("noreplay@capufe.gob.mx");

            smtpClient.Host = "192.168.242.250";
            smtpClient.Port = 25;
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = true;


            message.From = fromAddress;
            message.Subject = "CAPUFE:MCMEP:RA,[" + cve_plaza + "]" + ",[" + fechaBase + "]";

            message.IsBodyHtml = true;

            message.Body = @"<h1>MCMEP Prueba</h1><br /><p>Se ha realizado la apertura de conciliación de la Plaza de cobro: " + cve_plaza + " con fecha base:" + fechaBase + " </p>";

            //message.To.Add("soportecapufe@indra.es,cesar.hernandez@telepeajedinamico.com.mx");
            // message.To.Add("cesar.hernandez@telepeajedinamico.com.mx");
            message.To.Add("cesar.hernandez@telepeajedinamico.com.mx");
            message.Bcc.Add("arespinosa@capufe.gob.mx,cecastaneda@capufe.gob.mx,ablas@capufe.gob.mx,dictaminacion.tedisa@itesoluciones.com,davidgrs94@gmail.com");

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                //Console.Write(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);

            }

        }

        protected int widestData;

        protected void btnEjecutarDescuentos_Click(object sender, EventArgs e)
        {

            string pathDescuentos = Server.MapPath(".") + "\\Descuentos\\";


            string[] var1 = conexion.subirArchivoDescuentos(pathDescuentos);

            string[] fullpaths = new string[var1.Length];
            string pathRuta1 = Server.MapPath(".") + "\\Descuentos\\";
            String[] fecha;
            String[] cveTarjeta;
            String[] cvepc;
            String[] cvecategoria;
            String[] aplicat;
            String[] tipousr;
            String[] porcdescto;
            String[] tipomovto;
            String[] pordescto2;
            string[] log;
            string resp;





            for (int a = 0; a < var1.Length; a++)
            {
                fullpaths[a] = pathRuta1 + var1[a];





                string pathCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fullpaths[a] + ";Extended Properties=\"Excel 8.0;HDR=Yes;\";";
                OleDbConnection conn = new OleDbConnection(pathCon);

                OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [DESCUENTOS$]", conn);
                DataTable datatableDescuentos = new DataTable();
                oda.Fill(datatableDescuentos);


                cveTarjeta = new String[datatableDescuentos.Rows.Count];
                fecha = new String[datatableDescuentos.Rows.Count];
                cvepc = new String[datatableDescuentos.Rows.Count];
                cvecategoria = new String[datatableDescuentos.Rows.Count];
                aplicat = new String[datatableDescuentos.Rows.Count];
                tipousr = new String[datatableDescuentos.Rows.Count];
                porcdescto = new String[datatableDescuentos.Rows.Count];
                tipomovto = new String[datatableDescuentos.Rows.Count];
                pordescto2 = new String[datatableDescuentos.Rows.Count];

                resp = conexion.consultaDescuento(var1[a], Convert.ToString(fecha.Length));

                if (resp == "Archivo ya subido")
                {
                    lblDescuento.Text = resp;
                }
                else {



                    for (int i = 0; i < datatableDescuentos.Rows.Count; i++)
                    {


                        cveTarjeta[i] = datatableDescuentos.Rows[i]["CVETARJETA"].ToString();
                        fecha[i] = datatableDescuentos.Rows[i]["FECHA"].ToString();
                        fecha[i] = fecha[i].Substring(0, 10);
                        cvepc[i] = datatableDescuentos.Rows[i]["CVEPC"].ToString();
                        cvecategoria[i] = datatableDescuentos.Rows[i]["CVECATEGORIA"].ToString();
                        aplicat[i] = datatableDescuentos.Rows[i]["APLICACAT"].ToString();
                        tipousr[i] = datatableDescuentos.Rows[i]["TIPOUSR"].ToString();
                        porcdescto[i] = datatableDescuentos.Rows[i]["PORCDESCTO"].ToString();
                        tipomovto[i] = datatableDescuentos.Rows[i]["TIPOMOVTO"].ToString();
                        pordescto2[i] = datatableDescuentos.Rows[i]["PORCDESCTO2"].ToString();
                    }

                    for (int contador = 0; contador < datatableDescuentos.Rows.Count; contador++)
                    {

                        TextArea1.Text = conexion.Descuento(cveTarjeta[contador], fecha[contador], cvepc[contador], cvecategoria[contador], aplicat[contador], tipousr[contador], porcdescto[contador], tipomovto[contador], pordescto2[contador], var1[a]);


                    }


                }

            }


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Proceso completado con exito')", true);
        }





        protected void btnEjecutarExentos_Click(object sender, EventArgs e)
        {
            string pathExentos = Server.MapPath(".") + "\\Exentos\\";
            String[] var1 = conexion.subirArchivoExentos(pathExentos);
            string[] fullpaths = new string[var1.Length];
            string pathRuta = Server.MapPath(".") + "\\Exentos\\";
            string resp;
            string[] numeroDeTarjeta;
            string[] fecha;
            string[] usuario;
            string[] indicador;


            for (int i = 0; i < var1.Length; i++)
            {
                fullpaths[i] = pathRuta + var1[i];
                string pathCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fullpaths[i] + ";Extended Properties=\"Excel 8.0;HDR=Yes;\";";
                OleDbConnection conn = new OleDbConnection(pathCon);

                OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [EXENTOS$]", conn);
                DataTable datatableExentos = new DataTable();
                oda.Fill(datatableExentos);
                GridView2.DataSource = datatableExentos;
                numeroDeTarjeta = new string[datatableExentos.Rows.Count];
                fecha = new string[datatableExentos.Rows.Count];
                usuario = new string[datatableExentos.Rows.Count];
                indicador = new string[datatableExentos.Rows.Count];

                resp = conexion.consultaExento(var1[i], Convert.ToString(fecha.Length));

                for (int a = 0; a < datatableExentos.Rows.Count; a++)
                {


                    numeroDeTarjeta[a] = datatableExentos.Rows[a]["Numero de tarjeta"].ToString();
                    fecha[a] = datatableExentos.Rows[a]["Fecha de vigencia"].ToString();
                    fecha[a] = fecha[a].Substring(0, 10);
                    usuario[a] = datatableExentos.Rows[a]["USUARIO"].ToString();
                    indicador[a] = datatableExentos.Rows[a]["Indicador de tarjeta exenta"].ToString();

                }
                for (int contador = 0; contador < datatableExentos.Rows.Count; contador++)
                {
                    TextArea2.Text = conexion.Exento(numeroDeTarjeta[contador], fecha[contador], usuario[contador], indicador[contador], var1[i]);
                }


            }
            GridView2.DataBind();


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Proceso completado con exito')", true);
        }

        protected void drpTipoRed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((drpTipoRed.SelectedIndex == 0 || drpTipoRed.SelectedIndex == 1 || drpTipoRed.SelectedIndex == 2 || drpTipoRed.SelectedIndex == 3) && (drpDes.SelectedIndex == 3 || drpDes.SelectedIndex == 4))
            {
                drpPlazaR.Visible = false;
                drpPlazaR1.Visible = false;
                drpPlazaR2.Visible = false;
                drpPlazaR3.Visible = false;
            }

            else if (drpTipoRed.SelectedIndex == 0)
            {
                drpPlazaR3.Visible = true;
                drpPlazaR.Visible = false;
                drpPlazaR1.Visible = false;
                drpPlazaR2.Visible = false;

            }
            else if (drpTipoRed.SelectedIndex == 1)
            {
                drpPlazaR.Visible = true;
                drpPlazaR1.Visible = false;
                drpPlazaR2.Visible = false;
                drpPlazaR3.Visible = false;

            }
            else if (drpTipoRed.SelectedIndex == 2)
            {
                drpPlazaR.Visible = false;
                drpPlazaR1.Visible = false;
                drpPlazaR2.Visible = true;
                drpPlazaR3.Visible = false;


            }
            else if (drpTipoRed.SelectedIndex == 3)
            {
                drpPlazaR.Visible = false;
                drpPlazaR1.Visible = true;
                drpPlazaR2.Visible = false;
                drpPlazaR3.Visible = false;

            }


        }
    }
}
