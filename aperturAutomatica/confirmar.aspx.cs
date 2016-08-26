using System;

using System.Web.UI.WebControls;
using System.Net.Mail;


namespace aperturAutomatica
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

         
        }
       
	

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("apertura.aspx");
           
        }
    }
}