using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool auth = Convert.ToBoolean(Session["auth"]);

            if (auth)
            {
                long id = Convert.ToInt64(Session["uid"]);
                S_User user = UserManager.GetUserById(id);

                if (!string.IsNullOrEmpty(user.name)) contactNaam.Text = user.name;
                if (!string.IsNullOrEmpty(user.email)) contactEmail.Text = user.email;
            }
        }

        protected void buttonSubmitForm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Page.MaintainScrollPositionOnPostBack = true;

                bool validContact = false;
                string result = string.Empty;

                try
                {
                    S_Contact contact = new S_Contact();
                    contact.email = contactEmail.Text;
                    contact.message = contactMessage.Text;
                    ContactManager.Insert(contact);

                    validContact = true;
                    Mail.SendContactMailToNBF(Settings.MailNBF, contactNaam.Text, contact.email, contact.message);
                    result = "Het bericht is verstuurd naar de NBF, u kunt spoedig een reactie te gemoed zien.";

                    contactNaam.Visible = false;
                    contactEmail.Visible = false;
                    contactMessage.Visible = false;
                }
                catch (Exception ex)
                {
                    result = "Er is een fout opgetreden, probeer het nogmaals";
                }

                if (!validContact)
                    meldSuccess.Attributes.Add("style", "color:red");
                else
                    meldSuccess.Attributes.Add("style", "color:black");

                meldSuccess.InnerHtml = result;
            }
        }
    }
}