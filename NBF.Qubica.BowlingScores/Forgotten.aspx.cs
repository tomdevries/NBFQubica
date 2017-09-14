using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Managers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class Forgotten : System.Web.UI.Page
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void buttonSubmitForm_Click(object sender, EventArgs e)
        {
            logger.Debug("We hebben op de knop gedrukt...");

            try
            {
                buttonSubmitForm.Style["visibility"] = "hidden";

                S_User user = UserManager.GetUserByEmail(forgottenEmail.Text);


                if (!string.IsNullOrEmpty(forgottenEmail.Text) && user != null)
                {
                    logger.Debug("Gebruikler gevonden...");
                    Mail.SendForgottenMessage(user.email, user.name, user.username, user.frequentbowlernumber);
                    meldSuccess.InnerHtml = "Er is een mail gestuurd naar het e-mail adres " + forgottenEmail.Text + "<br/>Klik in de e-mail op de link om je wachtwoord opnieuw in te stellen.<br/>(E-mail niet ontvangen? controleer je spam folder).<br/><br/>";
                    forgottenEmail.Text = string.Empty;
                    forgottenEmail.Visible = false;
                }
                else
                {
                    logger.Debug("Gebruiker NIET gevonden op basis van email ades ..." + forgottenEmail.Text);
                    meldSuccess.InnerHtml = "Er is iets fout gegaan bij het versturen van de e-mail, klopt het e-mailadres?";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                meldSuccess.InnerHtml = "Er is iets fout gegaan bij het versturen van de e-mail, klopt het e-mailadres?";
            }
        }
    }
}