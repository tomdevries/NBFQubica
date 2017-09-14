using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class _default : System.Web.UI.Page
    {
        protected string _content;
        protected string _login_out;
        protected string _account;
        protected string _meld_je_aan;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true; 

            bool auth = Convert.ToBoolean(Session["auth"]);

            if (!auth)
            {
                _login_out = "<a class='portfolio-link' href='./Login.aspx' id='login'>Login</a>";
                _account = "";
            }
            else
            {
                _login_out = "<a class='portfolio-link' href='./Logout.aspx' id='login'>Logout</a>";
                _account = "<a class='portfolio-link' href='./Profile.aspx' id='profile'>Profiel</a>";
            }

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
                    case "Meld je aan":
                        meldjeaan.Text = st.text;
                        break;
                    case "Installeer de app":
                        installeerdeapp.Text = st.text;
                        break;
                    case "Ga bowlen":
                        gabowlen.Text = st.text;
                        break;
                }
            }

            if (!this.IsPostBack)
            {
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
                                "         <p class='text-muted'>" + competition.description + " <a href='./Competitie.aspx' class='portfolio-link'>Doe mee!</a></p> " +
                                "     </div> " +
                                " </div> " +
                                "</li> ";

                    cntr++;
                }

                long fbn = generateFrequentBowlerNumber();
                int cnt = 0;
                while (UserManager.UserExistByFrequentBowlerNumber(fbn) && cnt++ < 10)
                    fbn = generateFrequentBowlerNumber();
                meldfrequentbowlernummmer.Text = fbn.ToString();
            }
        }

        private long generateFrequentBowlerNumber()
        {
            Random rnd = new Random();
            int fbn = rnd.Next(100000, 999999);

            return fbn;
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
                        result = result = "Vul een ID in!";
                        validUser = false;
                    }
                    else
                    {
                        long n;
                        bool isNumeric = long.TryParse(meldfrequentbowlernummmer.Text, out n);
                        if (!isNumeric)
                        {
                            result = "Vul een ID in!";
                            validUser = false;
                        }
                        else
                        {
                            if (UserManager.UserExistByFrequentBowlerNumber(n))
                            {
                                result = "Kies een ander ID, deze bestaat reeds!";
                                validUser = false;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(meldfrequentbowlernaam.Text))
                    {
                        result = "Vul een Frequent Bowler Naam in!";
                        validUser = false;
                    }
                    else
                    {
                        if (UserManager.UserExistByUsername(meldfrequentbowlernaam.Text.ToUpper()))
                        {
                            result = "Vul een andere Frequent Bowler Naam in, deze is reeds ingebruik!";
                            validUser = false;
                        }
                    }

                    if (string.IsNullOrEmpty(meldemail.Text))
                    {
                        result = "Vul een e-mail adres in!";
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
                        user.address = "";
                        user.city = "";
                        user.email = meldemail.Text;
                        user.isMember = false;
                        user.isRegistrationConfirmed = false;
                        user.name = string.Concat(meldvoornaam.Text, " ", meldachternaam.Text).Trim();
                        user.password = meldwachtwoord.Text;
                        user.roleid = Role.USER;
                        user.username = meldfrequentbowlernaam.Text.ToUpper();
                        long n;
                        long.TryParse(meldfrequentbowlernummmer.Text, out n);
                        user.frequentbowlernumber = n;

                        UserManager.Insert(user);
                        result = "Het aanmelden is geslaagd, check je e-mail, ga bowlen of schrijf je eerst in voor een competitie";
                        Mail.SendRegistrationMessage(user.email, user.name, user.username, user.frequentbowlernumber);
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

        protected void meldvoornaam_TextChanged(object sender, EventArgs e)
        {
            string fbn = meldfrequentbowlernaam.Text;
            if (string.IsNullOrEmpty(fbn))
            {
                string voornaam = meldvoornaam.Text;
                string nospacevoornaam = new string(voornaam.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
                string nohyphennospacevoornaam = new string(voornaam.ToCharArray().Where(c => c != '\x002D').ToArray());
                meldfrequentbowlernaam.Text = RemoveDiacritics(nohyphennospacevoornaam);
            }
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}