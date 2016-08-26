using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace aperturAutomatica
{
    public partial class _Default : System.Web.UI.Page
    {
        operacionesBD nuevo = new operacionesBD();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Session["usuario"] = null;
        }

        internal class Helper
        {
            public static string EncodePassword(string originalPassword)
            {

                MD5 md5 = MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(originalPassword);
                byte[] hash = md5.ComputeHash(inputBytes);

                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {


            
            string now = "";
            if (txtUsuario.Text == "" && txtContrasena.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('Llena todos los campos');", true);
            }
            else if (txtUsuario.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('llena el campo Usuario');", true);
            }
            else if ( txtContrasena.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", "alert('llena el campo Contraseña');", true);
            }
            else
            {

                now = Helper.EncodePassword(txtContrasena.Text);
                if (nuevo.validaLogin(txtUsuario.Text, now) == 1)
                {
                    lblError.Text = "Usuario o contraseña";
                    Session["usuario"] = txtUsuario.Text;
                    Response.Redirect("apertura.aspx");
                }
                else
                {
                    lblError.Text = "Usuario o contraseña incorrectas";
                }
            }
            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "";
            txtContrasena.Text = "";
            lblError.Text = "";
        }

        protected void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            txtUsuario.Text = txtUsuario.Text.ToUpper();
        }

        protected void txtContrasena_TextChanged(object sender, EventArgs e)
        {
            txtContrasena.Text = txtContrasena.Text.ToUpper();
        }
    }
}