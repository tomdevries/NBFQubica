using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["auth"] = "false";
            Session["uid"] = null;

            Response.Redirect("default.aspx");
        }
    }
}