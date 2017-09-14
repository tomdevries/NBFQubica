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
    public partial class Competitie : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true; 

            if (!this.IsPostBack)
            {
                List<S_Competition> competitions = CompetitionManager.GetRunningCompetitions(true);

                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = "-- Selecteer een competitie --";
                compCompetitie.Items.Add(item);

                foreach (S_Competition competition in competitions)
                {
                    S_Challenge challenge = ChallengeManager.GetChallenge(competition.challengeid);

                    item = new ListItem();
                    item.Value = competition.id.ToString();
                    item.Text = Conversion.DateToTitle(competition.startdate, competition.enddate) + " : " + challenge.name;
                    compCompetitie.Items.Add(item);
                }

                bool auth = Convert.ToBoolean(Session["auth"]);

                if (auth)
                {
                    long id = Convert.ToInt64(Session["uid"]);
                    S_User user = UserManager.GetUserById(id);
                    compFrequentBowlerNaam.Text = user.username;
                    compFrequentBowlerNummer.Text = user.frequentbowlernumber.ToString();
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
                    S_User user = null;
                    long frequentBowlerNumber = 0;

                    if (string.IsNullOrEmpty(compFrequentBowlerNummer.Text))
                    {
                        result = result = "Vul een Frequent Bowler Nummer in!";
                        validUser = false;
                    }
                    else
                    {
                        bool isNumeric = long.TryParse(compFrequentBowlerNummer.Text, out frequentBowlerNumber);
                        if (!isNumeric)
                        {
                            result = "Vul je ID in!";
                            validUser = false;
                        }
                        else
                        {
                            if (!UserManager.UserExistByFrequentBowlerNumber(frequentBowlerNumber))
                            {
                                result = "Kies je eigen ID, deze bestaat niet!";
                                validUser = false;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(compWachtwoord.Text))
                    {
                        result = result = "Vul je wachtwoord in!";
                        validUser = false;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(compFrequentBowlerNaam.Text))
                        {
                            result = result = "Vul je Frequent Bowler Naam in!";
                            validUser = false;
                        }
                        else
                        {
                            if (!UserManager.UserExistByUsername(compFrequentBowlerNaam.Text))
                            {
                                result = result = "Er bestaat geen registratie met deze Frequent Bowler Naam!";
                                validUser = false;
                            }
                            else if (!UserManager.UserExistByFrequentBowlerNumber(frequentBowlerNumber))
                            {
                                result = result = "Er bestaat geen registratie met dit ID!";
                                validUser = false;
                            }
                            else
                            {
                                user = UserManager.GetUserByNamePasswordAndFrequentbowlernumber(compFrequentBowlerNaam.Text, compWachtwoord.Text, frequentBowlerNumber);
                                if (user == null)
                                {
                                    result = result = "Het wachtwoord is niet correct voor deze Frequent Bowler Naam en ID!";
                                    validUser = false;
                                }
                            }
                        }
                    }


                    if (validUser)
                    {
                        int competitionId = Convert.ToInt16(compCompetitie.SelectedValue);
                        S_Competition competition = CompetitionManager.GetCompetition(competitionId);

                        if (competition == null)
                        {
                            result = result = "Deze competitie bestaat niet meer, selecteer een andere!";
                            validUser = false;
                        }
                        else
                        {
                            if (CompetitionManager.ExistPlayerInCompetition(competitionId, user.id))
                            {
                                result = result = "Je doet al mee aan deze competitie, selecteer een andere!";
                                validUser = false;
                            }
                            else
                            {
                                CompetitionManager.AddPlayer(user.id, competitionId);
                                result = result = "Je bent ingeschreven voor deze competitie!";
                                compCompetitie.Visible = false;
                                compFrequentBowlerNaam.Visible = false;
                                compFrequentBowlerNummer.Visible = false;
                                compWachtwoord.Visible = false;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    result = "Er is een fout opgetreden, probeer het nogmaals";
                }

                if (!validUser)
                    meldSuccess.Attributes.Add("style", "color:red");
                else
                    meldSuccess.Attributes.Add("style", "color:black");

                meldSuccess.InnerHtml = result;
            }
        }
    }
}