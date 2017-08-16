using NBF.Qubica.Classes;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void buttonSubmitForm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool validUserData = true;
                string result = string.Empty;
                long frequentBowlerNumber=0;

                validUserData = !string.IsNullOrEmpty(loginFrequentBowlerNaam.Text);
                validUserData = !string.IsNullOrEmpty(loginFrequentBowlerNummer.Text);
                validUserData = !string.IsNullOrEmpty(loginWachtwoord.Text);

                try
                {
                    long.TryParse(loginFrequentBowlerNummer.Text, out frequentBowlerNumber);
                }
                catch (Exception ex)
                {
                    validUserData = false;
                }

                if (!validUserData)
                    result = "Vul Frequent Bowler Naam, ID en wachtwoord in!";
                else
                {
                    S_User user = UserManager.GetUserByNamePasswordAndFrequentbowlernumber(loginFrequentBowlerNaam.Text, loginWachtwoord.Text, frequentBowlerNumber);
                    if (user == null)
                    {
                        result = "De gegevens zijn incorrect";
                        loginWachtwoord.Text = "";
                    }
                    else
                    {
                        Session["auth"] = "true";
                        Session["uid"] = user.id;

                        Response.Redirect("default.aspx");
                    }
                }

                meldSuccess.InnerHtml = result;
            }
        }
    }
}