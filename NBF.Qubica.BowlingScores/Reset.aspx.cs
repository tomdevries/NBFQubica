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
    public partial class Reset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                try
                {
                    long idnumber;
                    if (long.TryParse(Request.QueryString["idnummer"], out idnumber))
                    {
                        S_User user = UserManager.GetUserByFrequentbowlernumber(idnumber);
                        Session["uid"] = user.id;

                        if (user != null)
                        {
                            confirmText.Text = "Vul je wachtwoord twee keer in";
                            Session["uid"] = user.id;
                        }
                        else
                        {
                            confirmText.Text = "Het account bestaat niet";
                            buttonSubmitForm.Style["visibility"] = "hidden";
                        }
                    }
                    else
                    {
                        confirmText.Text = "Het account bestaat niet";
                        buttonSubmitForm.Style["visibility"] = "hidden";
                    }
                }
                catch (Exception ex)
                {
                    confirmText.Text = "Er is iets fout gegaan.";
                    buttonSubmitForm.Style["visibility"] = "hidden";
                }
            }
        }

        protected void buttonSubmitForm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Page.MaintainScrollPositionOnPostBack = true;

                string result = string.Empty;
                try
                {
                    if (meldwachtwoord.Text.CompareTo(meldcontrole.Text) != 0)
                    {
                        result = "Het wachtwoord is niet twee keer hetzelfde ingevuld!";
                    }
                    else
                    {
                        long uid;
                        if (long.TryParse(Session["uid"].ToString(), out uid))
                        {
                            S_User user = UserManager.GetUserById(uid);

                            user.password = meldwachtwoord.Text;
                            UserManager.Update(user);
                            result = "Het wachtwoord is aangepast.";
                            Mail.SendPasswordUpdatedMessage(user.email, user.name, user.username, user.frequentbowlernumber);

                            meldwachtwoord.Text = string.Empty;
                            meldcontrole.Text = string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = "Er is een fout opgetreden, probeer het nogmaals";
                }

                meldSuccess.InnerHtml = result;
            }
        }
    }
}