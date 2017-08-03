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
    public partial class _default : System.Web.UI.Page
    {
        protected string _content;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true; 

            if (!this.IsPostBack)
            {
                List<S_Text> texts = TextManager.GetTexts();

                foreach (S_Text st in texts)
                {
                    switch (st.label)
                    {
                        case "Web Site Titel":
                            web_site_title.Text = st.text;
                            break;
                        case "Home":
                            home.Text = st.text;
                            break;
                    }
                }

                List<S_Competition> competitions = CompetitionManager.GetRunningCompetitions(true);

                int cntr = 0;
                foreach (S_Competition competition in competitions)
                {
                    S_Challenge challenge = ChallengeManager.GetChallenge(competition.challengeid);

                    string style = "";
                    if (cntr % 2 == 0)
                        style = "class='timeline-inverted'";

                    _content += "<li " + style + " > " +
                                "<div class='timeline-image'> " +
                                "     <img class='img-circle img-responsive' src='img/competities/1.png' alt=''> " +
                                " </div> " +
                                " <div class='timeline-panel'> " +
                                "     <div class='timeline-heading'> " +
                                "         <h4>" + challenge.name + "</h4> " +
                                "         <h4 class='subheading'>" + Conversion.DateToTitle(competition.startdate, competition.enddate) + "</h4> " +
                                "     </div> " +
                                "     <div class='timeline-body'> " +
                                "         <p class='text-muted'>" + competition.description + " <a href='./Competitie.aspx' class='portfolio-link' data-toggle='modal'>Doe mee!</a></p> " +
                                "     </div> " +
                                " </div> " +
                                "</li> ";

                    cntr++;
                }
            }
        }

        protected void buttonSubmitForm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Page.MaintainScrollPositionOnPostBack = true; 

                bool validUser = true;
                string result = string.Empty;
                try
                {
                    S_User user = new S_User();

                    if (meldwachtwoord.Text.CompareTo(meldcontrole.Text) != 0) { result = "Het wachtwoord is niet twee keer hetzelfde ingevuld!"; validUser = false; }
                    if (string.IsNullOrEmpty(meldfrequentbowlernummmer.Text))
                    {
                        result = result = "Vul een Frequent Bowler Nummer in!";
                        validUser = false;
                    }
                    else
                    {
                        long n;
                        bool isNumeric = long.TryParse(meldfrequentbowlernummmer.Text, out n);
                        if (!isNumeric)
                        {
                            result = "Vul een Frequent Bowler Nummer in!";
                            validUser = false;
                        }
                        else
                        {
                            if (UserManager.UserExistByFrequentBowlerNumber(n))
                            {
                                result = "Kies een ander Frequent Bowler Nummer, deze bestaat reeds!";
                                validUser = false;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(meldemail.Text))
                    {
                        result = result = "Vul een e-mail adres in!";
                        validUser = false;
                    }
                    else
                    {
                        if (UserManager.UserExistByEmail(meldemail.Text))
                        {
                            result = "Vul een ander e-mail adres in, dit e-mail adres is reeds ingebruik!";
                            validUser = false;
                        }
                    }


                    if (validUser)
                    {
                        user.address = meldadres.Text;
                        user.city = meldwoonplaats.Text;
                        user.email = meldemail.Text;
                        user.isMember = false;
                        user.isRegistrationConfirmed = true;
                        user.name = meldnaam.Text;
                        user.password = meldwachtwoord.Text;
                        user.roleid = Role.USER;
                        user.username = meldfrequentbowlernaam.Text;
                        long n;
                        long.TryParse(meldfrequentbowlernummmer.Text, out n);
                        user.frequentbowlernumber = n;

                        UserManager.Insert(user);
                        result = "Het aanmelden is geslaagd, ga bowlen of schrijf je eerst in voor een competitie";
                    }
                }
                catch (Exception ex)
                {
                    result = "Er is een fout opgetreden, probeer het nogmaals";
                }

                if (!validUser)
                    meldsuccess.Attributes.Add("style", "color:red");
                else
                    meldsuccess.Attributes.Add("style", "color:white");

                meldsuccess.InnerHtml= result;
            }
        }
    }
}