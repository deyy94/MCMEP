using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.IO;
using System.Collections;


namespace aperturAutomatica
{
    public class operacionesBD
    {
        //string _user;
        /*Variable para obtener la cadena de conexion a ORACLE*/
        static string myconn = "Data Source=(DESCRIPTION="
        + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=OFC520.CAPUFE.GOB.MX)(PORT=1521)))"
        + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=DCAPUFE1)));"
        + "User Id=MCMEPADM;Password=desamcmepadm;";

        /// <summary>
        /// Metodo para abrir conexion
        /// </summary>
        /// <returns></returns>
        public static OracleConnection conexion()
        {
            try
            {
                //Abre la conexion con oracle
                OracleConnection conO = new OracleConnection(myconn);

                conO.Open();
                return conO;
            }
            catch
            {
                throw;
            }
        }

        //procedimiento para  los reportes de desagregado de ingreso diario, aforo diario y dictamen final mensual
        public DataSet reporteDID(DateTime inicio, DateTime fin, string plaza, int red, string ban_reporte)
        {
            OracleConnection myconnection = conexion();

            DataTable dt = new DataTable();
            OracleDataAdapter oda = new OracleDataAdapter();
            DataSet ds = new DataSet();

            string ino, fi, pla;

            ino = inicio.ToString("dd/MM/yyyy");
            fi = fin.ToString("dd/MM/yyyy");
            pla = plaza.ToString();

            int ban;

            ban = Convert.ToInt32(ban_reporte);
            try
            {
                OracleCommand consultaDID = new OracleCommand();
                consultaDID.Connection = myconnection;
                if (red == 0)
                {
                    consultaDID.CommandText = "SP_DESAGREGADO_AFORO_DIARIO";
                    consultaDID.CommandType = CommandType.StoredProcedure;
                    consultaDID.Parameters.Add("vp_fecha_ini", OracleType.VarChar).Value = ino;
                    consultaDID.Parameters.Add("vp_fecha_fin", OracleType.VarChar).Value = fi;
                    consultaDID.Parameters.Add("vp_pzcobro", OracleType.VarChar).Value = pla;
                    consultaDID.Parameters.Add("p_recordset", OracleType.Cursor).Direction = ParameterDirection.Output;


                }
                if (ban == 2)
                {
                    consultaDID.CommandText = "SP_DESAGREGADO_INGRESO_DIARIO";
                    consultaDID.CommandType = CommandType.StoredProcedure;
                    consultaDID.Parameters.Add("vp_fecha_ini", OracleType.VarChar).Value = ino;
                    consultaDID.Parameters.Add("vp_fecha_fin", OracleType.VarChar).Value = fi;
                    consultaDID.Parameters.Add("vp_pzcobro", OracleType.VarChar).Value = pla;
                    consultaDID.Parameters.Add("vp_tadm", OracleType.VarChar).Value = red;
                    consultaDID.Parameters.Add("p_recordset", OracleType.Cursor).Direction = ParameterDirection.Output;


                }
                OracleDataAdapter odr = new OracleDataAdapter(consultaDID);
                odr = new OracleDataAdapter(consultaDID);
                odr.Fill(ds);
                myconnection.Close();
                return ds;
            }
            catch (Exception e)
            {
                return ds;
            }


            finally
            {
                myconnection.Close();
            }
        }

        // Procedimiento para realizar aforo acumulado mensual y ingreso acumulado mensual
        public DataSet reporteDID2(DateTime fechabase, DateTime fechabase2, int red, string ban_reporte)
        {
            OracleConnection myconnection = conexion();

            DataTable dt = new DataTable();
            OracleDataAdapter oda = new OracleDataAdapter();
            DataSet ds = new DataSet();

            string base1, base2;

            base1 = fechabase.ToString("dd/MM/yyyy");
            base2 = fechabase2.ToString("dd/MM/yyyy");

            int ban;

            ban = Convert.ToInt32(ban_reporte);
            try
            {
                OracleCommand consultaDID = new OracleCommand();
                consultaDID.Connection = myconnection;

                if (ban == 3)
                {
                    consultaDID.CommandText = "SP_AFORO_ACUM_MENSUAL";
                }
                if (ban == 4)
                {
                    consultaDID.CommandText = "SP_INGRESO_ACUM_MENSUAL";
                }

                consultaDID.CommandType = CommandType.StoredProcedure;
                consultaDID.Parameters.Add("vp_fechabase", OracleType.VarChar).Value = base1;
                consultaDID.Parameters.Add("vp_fechabase2", OracleType.VarChar).Value = base2;
                consultaDID.Parameters.Add("vp_tadm", OracleType.VarChar).Value = red;
                consultaDID.Parameters.Add("p_recordset", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter odr = new OracleDataAdapter(consultaDID);
                odr.Fill(ds);
                myconnection.Close();
                return ds;
            }
            catch (Exception e)
            {
                return ds;
            }

            finally
            {
                myconnection.Close();
            }
        }

        public string consultaExento(string nameFile, string tarjetas)
        {
            string archivos = "";
            string nombArchivo = "";

            OracleConnection myconnection = conexion();

            string sql = "SELECT * FROM pdbimcat WHERE NOM_ARCHIVO_C='" + nameFile + "' AND NUM_REGISTROS=" + tarjetas;
            OracleCommand cmd = new OracleCommand(sql, myconnection);
            cmd.CommandType = CommandType.Text;

            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            oda.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow campo = ds.Tables[0].Rows[i];
                    nombArchivo = campo["NOM_ARCHIVO_C"].ToString();
                    archivos = campo["NUM_REGISTROS"].ToString();
                }
            }

            if (nombArchivo == nameFile && archivos == tarjetas)
            {
                return "Archivo ya subido";
            }
            else if (nombArchivo == nameFile && archivos != tarjetas)
            {
                return "Archivo actualizado";
            }
            else {
                sql = "insert into pdbimcat values('" + nameFile + "','" + tarjetas + "',SYSDATE,PSBIMCAT.NEXTVAL,SYSDATE,PSBIMCAT.NEXTVAL,'ABLAS','ABLAS','ADP',SYSDATE)";
                cmd = new OracleCommand(sql, myconnection);
                cmd.CommandType = CommandType.Text;
                oda = new OracleDataAdapter(cmd);
                ds = new DataSet();
                oda.Fill(ds);
                myconnection.Close();
                return "Archivo insertado";
            }
        }

        public string[] subirArchivoExentos(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xls"); //Getting Text files
            string[] nam = new string[Files.Length];
            try
            {

                for (int x = 0; x < Files.Length; x++)
                {
                    nam[x] = Files[x].Name;

                }
                return nam;
            }
            catch
            {

                nam[0] = "Archivos no encontrados";
                return nam;
            }
        }

        public String Exento(string cveTarjeta, string fecha, string usuario, string indicador, string archivo)
        {
            string NUM_TARJETA = "", FCH_INIVEXC = "", FCH_FINVEXC = "";
            string msj = "";

            OracleConnection myconnection = conexion();
            string sql = "select NUM_TARJETA,FCH_INIV_EXC,FCH_FINV_EXC from PCEXNTOS WHERE NUM_TARJETA='" + cveTarjeta + "'";
            OracleCommand cmd = new OracleCommand(sql, myconnection);
            cmd.CommandType = CommandType.Text;
            OracleDataAdapter oda = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow campo = ds.Tables[0].Rows[i];
                    NUM_TARJETA = campo["NUM_TARJETA"].ToString();
                    FCH_INIVEXC = campo["FCH_INIV_EXC"].ToString();
                    FCH_FINVEXC = campo["FCH_FINV_EXC"].ToString();

                }
            }


            //Este if es en caso de que no exista la tarjeta 
            if (indicador == "I" && NUM_TARJETA!="")
            {
                sql = "update PCEXNTOS set fch_finv_exc='" + fecha + "' where num_tarjeta='" + cveTarjeta + "' and fch_iniv_exc='" + FCH_INIVEXC + "'";
                cmd = new OracleCommand(sql, myconnection);
                cmd.CommandType = CommandType.Text;
                oda = new OracleDataAdapter(cmd);

                myconnection.Close();
            }
            else if (indicador == "A" && NUM_TARJETA=="")
            {
                sql = "INSERT into PCEXNTOS values('" + cveTarjeta + "','" + fecha + "','" + usuario + "','','" + archivo + "',SYSDATE,PSEXNTOS.NEXTVAL,'ABLAS','ABLAS','TOAD_Ticket-122419',SYSDATE)";
                cmd = new OracleCommand(sql, myconnection);
                cmd.CommandType = CommandType.Text;
                oda = new OracleDataAdapter(cmd);


                myconnection.Close();
            }
            else {
                msj = "Error verifica la tarjeta " + cveTarjeta + " probablemente tus datos son incorrectos";
            }
            myconnection.Close();
            return msj;



        }

        //Estoy obteniendo los nombres de todo el directorio
        public string[] subirArchivoDescuentos(string path)
        {

            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.xls"); //Getting Text files

            string[] nam = new string[Files.Length];
            try
            {
                for (int x = 0; x < Files.Length; x++)
                {
                    nam[x] = Files[x].Name;
                }
                return nam;
            }
            catch
            {

                nam[0] = "Archivos no encontrados";
                return nam;
            }
        }

        public string consultaDescuento(string nameFile, string num_registros)
        {
            string archivos = "";
            string nombArchivo = "";
            string msj = "";

            try
            {
                OracleConnection myconnection = conexion();

                string sql = "SELECT * FROM pdbimcat WHERE NOM_ARCHIVO_C='" + nameFile + "' AND NUM_REGISTROS='" + num_registros + "'";

                OracleCommand cmd = new OracleCommand(sql, myconnection);
                cmd.CommandType = CommandType.Text;

                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                oda.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow campo = ds.Tables[0].Rows[i];
                        nombArchivo = campo["NOM_ARCHIVO_C"].ToString();
                        archivos = campo["NUM_REGISTROS"].ToString();
                    }
                    myconnection.Close();
                    if (nombArchivo == nameFile && archivos == num_registros)
                    {
                        msj = "Archivo ya subido";
                    }
                    else if (nombArchivo == nameFile && Convert.ToInt32(archivos) != Convert.ToInt32(num_registros))
                    {
                        sql = "UPDATE pdbimcat SET NUM_REGISTROS='" + num_registros + "', FCH_UMOD=SYSDATE  WHERE NOM_ARCHIVO_C='" + nameFile + "'";
                        cmd = new OracleCommand(sql, myconnection);
                        cmd.CommandType = CommandType.Text;
                        oda = new OracleDataAdapter(cmd);
                        ds = new DataSet();
                        oda.Fill(ds);
                        myconnection.Close();
                        msj = "Archivo actualizado";
                    }

                }

                else
                {
                    sql = "insert into pdbimcat values('" + nameFile + "','" + num_registros + "',SYSDATE,PSBIMCAT.NEXTVAL,SYSDATE,PSBIMCAT.NEXTVAL,'ABLAS','ABLAS','ADP',SYSDATE)";
                    cmd = new OracleCommand(sql, myconnection);
                    cmd.CommandType = CommandType.Text;
                    oda = new OracleDataAdapter(cmd);
                    ds = new DataSet();
                    oda.Fill(ds);
                    myconnection.Close();
                }
                return msj;

            }
            catch (Exception e)
            {
                return "fallo";
            }

        }

        public String Descuento(string cveTarjeta, string fecha, string cvepc, string cvecatego, string aplicacat, string tipousr, string porcdescto, string tipomovto, string porcdescto2, string archivo)
        {
            string NUM_TARJETA = "", FCH_INIVDSC = "", FCH_FINVDSC = "", IND_TMOV = "";
            string msj = "";
            try
            {
                OracleConnection myconnection = conexion();
                string sql = "select NUM_TARJETA,FCH_INIVDSC,FCH_FINVDSC,IND_TMOV from PDDSCTOS WHERE NUM_TARJETA='" + cveTarjeta + "'";
                OracleCommand cmd = new OracleCommand(sql, myconnection);
                cmd.CommandType = CommandType.Text;
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                oda.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow campo = ds.Tables[0].Rows[i];
                        NUM_TARJETA = campo["NUM_TARJETA"].ToString();
                        FCH_INIVDSC = campo["FCH_INIVDSC"].ToString();
                        FCH_FINVDSC = campo["FCH_FINVDSC"].ToString();
                        IND_TMOV = campo["IND_TMOV"].ToString();
                    }

                }
                if (NUM_TARJETA == "" && tipomovto == "A" && FCH_FINVDSC == "")
                {
                    sql = "INSERT INTO PDDSCTOS VALUES('" + cveTarjeta + "','" + fecha + "','','" + cvepc + "','" + cvecatego + "','" + aplicacat + "','" + tipousr + "','" + porcdescto + "','" + tipomovto + "','" + archivo + "','ABLAS','ABLAS','ADP',SYSDATE,'" + porcdescto2 + "')";
                    cmd = new OracleCommand(sql, myconnection);
                    cmd.CommandType = CommandType.Text;
                    oda = new OracleDataAdapter(cmd);
                    ds = new DataSet();
                    oda.Fill(ds);
                    myconnection.Close();
                }
                else if (NUM_TARJETA != "" && FCH_INIVDSC != "" && tipomovto == "B" && FCH_FINVDSC == "")
                {
                    sql = "UPDATE PDDSCTOS SET FCH_FINVDSC = '" + fecha + "', IND_TMOV = '" + tipomovto + "', USR_UMOD = 'ABLAS', USR_APP = 'ABLAS', PRC_UMOD = 'ADP', FCH_UMOD = SYSDATE WHERE NUM_TARJETA IN('" + cveTarjeta + "') AND FCH_FINVDSC IS NULL";
                    cmd = new OracleCommand(sql, myconnection);
                    cmd.CommandType = CommandType.Text;
                    oda = new OracleDataAdapter(cmd);
                    ds = new DataSet();
                    oda.Fill(ds);
                    myconnection.Close();
                }
                else {
                    msj = "Error verifica la tarjeta " + cveTarjeta + " probablemente tus datos son incorrectos";
                }
                myconnection.Close();
                return msj;
            }
            catch (Exception e)
            {
                msj = "Error";
                return msj;
            }

        }

        public int validaLogin(string user, string pass)
        {

            OracleConnection myconnection = conexion();
            string dato = "null";
            int valor = 0;

            //myconnection.Open();
            string sql = "select UPPER(PASSWORD) AS PASS from SCUSRAPP WHERE CVE_USUARIO='" + user + "'";

            OracleCommand consultaContraseña = new OracleCommand(sql, myconnection);
            consultaContraseña.CommandType = CommandType.Text;
            OracleDataAdapter oda = new OracleDataAdapter(consultaContraseña);


            DataSet ds = new DataSet();
            oda.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow cveComp = ds.Tables[0].Rows[0];
                dato = cveComp["PASS"].ToString();
                if (dato == pass)
                {
                    valor = 1;
                }
            }
            myconnection.Close();
            return valor;
        }

        //Validamos El usuario con su perfil para que en caso de que sea ADMIN_GRAL aparesca en view1 Apertura automatica 
        public int validaAdmin(string userRol)
        {
            OracleConnection myconnection = conexion();
            string dato = "null";
            int valor = 0;

            string sqlAd = "select cve_perfil from SCUSRAPP where cve_usuario='" + userRol + "'";

            OracleCommand consultaPerfil = new OracleCommand(sqlAd, myconnection);
            consultaPerfil.CommandType = CommandType.Text;
            OracleDataAdapter cper = new OracleDataAdapter(consultaPerfil);
            DataSet ds = new DataSet();
            cper.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow cveComp = ds.Tables[0].Rows[0];
                dato = cveComp["cve_perfil"].ToString();
                if (dato == "ADMIN_GRAL")
                {
                    valor = 1;
                }
                else
                {
                    valor = 2;
                }
            }
            myconnection.Close();
            return valor;

        }

        public bool apertura(String fecha, String plaza, String motivo, string usuario)
        {
            OracleConnection myconnection = conexion();
            try
            {
                //myconnection.Open();
                OracleCommand apertura = new OracleCommand();
                apertura.Connection = myconnection;
                apertura.CommandText = "SP_APERTURA_CONCILIACION";
                apertura.CommandType = CommandType.StoredProcedure;
                apertura.Parameters.Add("vp_fecha_base", OracleType.VarChar).Value = fecha;
                apertura.Parameters.Add("vp_pzcobro", OracleType.VarChar).Value = plaza;
                apertura.Parameters.Add("vp_usuario", OracleType.VarChar).Value = usuario;
                apertura.Parameters.Add("vp_motivo", OracleType.VarChar).Value = motivo;

                apertura.ExecuteNonQuery();
                myconnection.Close();
                //Console.WriteLine("Se inserto con exito");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio el siguiente error: {0} al realizar Apertura Automática, consulte al administrador del sistema", e.Message.ToString());
                return false;
            }
            finally
            {
                myconnection.Close();
            }
        }

        public bool consultaEdoConciliacion(String fecha, String plaza)
        {
            OracleConnection con = conexion();
            bool flag = false;

            try
            {
                string sqlCons = "select cve_edoconciliacion_mep from pdconcil where cve_pzcobro='" + plaza + "' and fch_base='" + fecha + "'";
                string cveEdo = String.Empty;

                OracleCommand consultaEdoConc = new OracleCommand(sqlCons, con);
                consultaEdoConc.CommandType = CommandType.Text;
                OracleDataAdapter cper = new OracleDataAdapter(consultaEdoConc);
                DataSet ds = new DataSet();
                cper.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow cveEdoConc = ds.Tables[0].Rows[0];
                    cveEdo = cveEdoConc["cve_edoconciliacion_mep"].ToString();
                    if (cveEdo == "CC")
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                con.Close();

                return flag;
            }
            catch { return false; }
            finally { con.Close(); }
        }


    }
}