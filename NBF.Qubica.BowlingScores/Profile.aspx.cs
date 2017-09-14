using NBF.Qubica.Classes;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class Profile : System.Web.UI.Page
    {
        protected string _scores;
        protected string _competitions;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool auth = Convert.ToBoolean(Session["auth"]);

#if DEBUG
                if(!auth)
                {
                    auth = true;
                    Session["uid"] = 1;
                }
#endif

                if (!auth)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    long id = Convert.ToInt64(Session["uid"]);
                    S_User user = UserManager.GetUserById(id);

                    if (!string.IsNullOrEmpty(user.name)) profilenaam.Text = user.name;
                    if (!string.IsNullOrEmpty(user.address)) profileadres.Text = user.address;
                    if (!string.IsNullOrEmpty(user.city)) profileplaats.Text = user.city;
                    if (!string.IsNullOrEmpty(user.email)) profileemail.Text = user.email;
                    profilefrequentbowlernaam.Text = user.username;
                    profilefrequentbowlernummmer.Text = user.frequentbowlernumber.ToString();

                    buildCompetitions(user);
                    buildScores(user);
                }
            }
        }

        private void buildCompetitions(S_User user)
        {
            List<S_Competition> competitions = CompetitionManager.GetCompetitionsByPlayer(user.id);


            _competitions = "<div class='col-lg-8 col-lg-offset-2'>";
            _competitions += "  <h2>Jouw Competities</h2>";
            if (competitions.Count() > 0)
            {
                foreach (S_Competition competition in competitions)
                {
                    S_Challenge challenge = ChallengeManager.GetChallenge(competition.challengeid);
                    List<S_CompetitonBowlingcenter> competitonBowlingcenters = CompetitionManager.GetBowlingcentersByCompetition(competition.id);

                    _competitions += "  <h3>" + challenge.name + "</h3>";
                    _competitions += " <p> Van " + competition.startdate.ToString("dd-MM-yyyy") + " tot " + competition.enddate.ToString("dd-MM-yyyy") + " bij de volgende bowlingcentra: ";

                    foreach (S_CompetitonBowlingcenter competitonBowlingcenter in competitonBowlingcenters)
                    {
                        S_BowlingCenter bowlingCenter = BowlingCenterManager.GetBowlingCenterById(competitonBowlingcenter.bowlingcenterid);
                        _competitions += "<br/><br/>" + bowlingCenter.name;
                    }

                    _competitions += "</p>";
                }
            }
            else
            {
                _competitions += "  <p>Je doet nog niet mee aan een competitie</p>";
            }
            _competitions += "</div>";
        }

        private void buildScores(S_User user)
        {

            List<KeyValuePair<long, string>> bowlingcenters = ProfileManager.getBowlingCentersByUser(user.username, user.frequentbowlernumber);

            _scores  = "<div class='col-lg-8 col-lg-offset-2'>";
            _scores += "  <h2>Jouw resultaten</h2>";
            foreach (KeyValuePair<long, string> bowlingcenter in bowlingcenters)
            {
                _scores += "  <h3>"+bowlingcenter.Value+"</h3>";

                bool first = true;

                List<KeyValuePair<DateTime, KeyValuePair<int,int>>> scroresPerLanePerDateList = ProfileManager.getScroresPerLanePerDate(bowlingcenter.Key, user.username, user.frequentbowlernumber);
                foreach (KeyValuePair<DateTime, KeyValuePair<int, int>> scroresPerLanePerDate in scroresPerLanePerDateList)
                {
                    if (!first)
                    {
                        _scores += "   <div class='row'>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "       <div class='col-lg-1' style='border-width: 0 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                        _scores += "   </div>";
                    }
                    else
                        first = false;

                    _scores += "   <div class='row'>";
                    _scores += "     <div class='col-lg-2'>Datum</div>";
                    _scores += "     <div class='col-lg-6'></div>";
                    _scores += "     <div class='col-lg-2'>Hi-Score</div>";
                    _scores += "     <div class='col-lg-1'>&nbsp;</div>";
                    _scores += "     <div class='col-lg-1'>&nbsp;</div>";
                    _scores += "   </div>";
                    _scores += "   <div class='row'>";
                    _scores += "     <div class='col-lg-2'>" + scroresPerLanePerDate.Key.ToString("dd-MM-yyyy") + "</div>";
                    _scores += "     <div class='col-lg-6'>Baan " + scroresPerLanePerDate.Value.Key + "</div>";
                    _scores += "     <div class='col-lg-2'>" + scroresPerLanePerDate.Value.Value + "</div>";
                    _scores += "     <div class='col-lg-1'>&nbsp;</div>";
                    _scores += "     <div class='col-lg-1'>&nbsp;</div>";
                    _scores += "   </div>";


                    _scores += "   <div class='row'>";
                    _scores += "   <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>1</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>2</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>3</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>4</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>5</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>6</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>7</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>8</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>9</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 1px; border-style: solid; border-color: black;'>10</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 1px 0 1px; border-style: solid; border-color: black;'>Total</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 0 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "   </div>";

                    List<S_BowlScore> scores = ProfileManager.getGamesScores(bowlingcenter.Key, user.username, user.frequentbowlernumber, scroresPerLanePerDate.Key, scroresPerLanePerDate.Value.Key);


                    string bowl1 = "&nbsp;";
                    int bowl1Pins = 0;
                    string bowl2 = "&nbsp;";
                    int bowl2Pins = 0;
                    string bowl3 = "&nbsp;";
                    string progressiveTotal = "";

                    int gameCode = -1;

                    int recordNr=0;
                    for (recordNr = 0; recordNr < scores.Count; recordNr++)
                    {
                        if (gameCode != scores[recordNr].gamecode)
                        {
                            if (gameCode != -1)
                            {
                                _scores += "   <div class='col-lg-1' style='border-width: 0 0 0 1px; border-style: solid; border-color: black;'>";
                                _scores += "       <div class='row'>";
                                _scores += "           <div style='float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                                _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl1 + "</div>";
                                _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl2 + "</div>";
                                _scores += "       </div>";
                                _scores += "       <div class='row'>";
                                _scores += "           <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>" + progressiveTotal + "</div>";
                                _scores += "       </div>";
                                _scores += "   </div>";
                                if (recordNr > 0)
                                {
                                    for (int i = scores[recordNr - 1].framenumber; i < 10; i++)
                                    {
                                        _scores += "   <div class='col-lg-1' style='border-width: 0 0 0 1px; border-style: solid; border-color: black;'>";
                                        _scores += "       <div class='row'>";
                                        _scores += "           <div style='float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                                        _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>&nbsp;</div>";
                                        _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>&nbsp;</div>";
                                        _scores += "       </div>";
                                        _scores += "       <div class='row'>";
                                        _scores += "           <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>&nbsp;</div>";
                                        _scores += "       </div>";
                                        _scores += "   </div>";
                                    }
                                } 
                                _scores += "   <div class='col-lg-1' style='border-width: 0 1px 0 1px; border-style: solid; border-color: black;'>";
                                _scores += "   <div class='row'>";
                                _scores += "       <div style='float:left;width:34%;border-width: 1px 1px 1px 0; border-style: solid; border-color: black;'>" + bowl3 + "</div>";
                                _scores += "       <div style='float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                                _scores += "       <div style='float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                                _scores += "   </div>";
                                _scores += "   <div class='row'>";
                                _scores += "       <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>" + progressiveTotal + "</div>";
                                _scores += "   </div>";
                                _scores += "</div>";
                                _scores += "<div class='col-lg-1' style='border-width: 0 0 0 0; border-style: solid; border-color: black;'>";
                                _scores += "   <div class='row'>";
                                _scores += "      <img src='img/Facebook-Share.jpg' width='50px'/>";
                                _scores += "   </div>";
                                _scores += "</div>"; 
                                _scores += "</div>";
                            }
                            gameCode = scores[recordNr].gamecode;
                            _scores += "   <div class='row'>";

                            bowl1 = "&nbsp;";
                            bowl1Pins = 0;
                            bowl2 = "&nbsp;";
                            bowl2Pins = 0;
                            bowl3 = "&nbsp;";
                            progressiveTotal = "";
                        }

                        switch (scores[recordNr].framenumber)
                        {
                            case 1:
                                if (scores[recordNr].bowlnumber == 1)
                                {
                                    progressiveTotal = scores[recordNr].progressivetotal.ToString();
                                    if (scores[recordNr].isStrike)
                                        bowl1 = "X";
                                    else
                                        bowl1 = scores[recordNr].total.ToString();

                                    bowl1Pins = scores[recordNr].total.Value;
                                }
                                if (scores[recordNr].bowlnumber == 2)
                                {
                                    if (scores[recordNr].isSpare)
                                        bowl2 = "/";
                                    else
                                        bowl2 = (scores[recordNr].total - bowl1Pins).ToString();

                                    bowl2Pins = scores[recordNr].total.Value- bowl1Pins;
                                }
                                break;
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                                if (scores[recordNr].bowlnumber == 1)
                                {
                                    _scores += "   <div class='col-lg-1' style='border-width: 0 0 0 1px; border-style: solid; border-color: black;'>";
                                    _scores += "       <div class='row'>";
                                    _scores += "           <div style='float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                                    _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl1 + "</div>";
                                    _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl2 + "</div>";
                                    _scores += "       </div>";
                                    _scores += "       <div class='row'>";
                                    _scores += "           <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>" + progressiveTotal + "</div>";
                                    _scores += "       </div>";
                                    _scores += "   </div>";

                                    bowl2 = "&nbsp;";
                                    bowl3 = "&nbsp;";

                                    progressiveTotal = scores[recordNr].progressivetotal.ToString();
                                    if (scores[recordNr].isStrike)
                                        bowl1 = "X";
                                    else
                                        bowl1 = scores[recordNr].total.ToString();

                                    bowl1Pins = scores[recordNr].total.Value;
                                }
                                if (scores[recordNr].bowlnumber == 2)
                                {
                                    if (scores[recordNr].isSpare)
                                        bowl2 = "/";
                                    else
                                        bowl2 = (scores[recordNr].total - bowl1Pins).ToString();

                                    bowl2Pins = scores[recordNr].total.Value - bowl1Pins;
                                }
                                break;
                            case 10: 
                                if (scores[recordNr].bowlnumber == 1)
                                {
                                    _scores += "   <div class='col-lg-1' style='border-width: 0 0 0 1px; border-style: solid; border-color: black;'>";
                                    _scores += "       <div class='row'>";
                                    _scores += "           <div style='float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                                    _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl1 + "</div>";
                                    _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl2 + "</div>";
                                    _scores += "       </div>";
                                    _scores += "       <div class='row'>";
                                    _scores += "           <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>" + progressiveTotal + "</div>";
                                    _scores += "       </div>";
                                    _scores += "   </div>";

                                    bowl2 = "&nbsp;";
                                    bowl3 = "&nbsp;";

                                    progressiveTotal = scores[recordNr].progressivetotal.ToString();
                                    if (scores[recordNr].isStrike)
                                        bowl1 = "X";
                                    else
                                        bowl1 = scores[recordNr].total.ToString();

                                    bowl1Pins = scores[recordNr].total.Value;
                                }
                                if (scores[recordNr].bowlnumber == 2)
                                {
                                    if (scores[recordNr].isStrike)
                                        bowl2 = "X";
                                    else
                                    if (scores[recordNr].isSpare)
                                        bowl2 = "/";
                                    else
                                        if (bowl1Pins == 10)
                                            bowl2 = scores[recordNr].total.ToString();
                                        else
                                            bowl2 = (scores[recordNr].total - bowl1Pins).ToString();

                                    if (bowl1Pins == 10)
                                        bowl2Pins = scores[recordNr].total.Value;
                                    else
                                        bowl2Pins = scores[recordNr].total.Value - bowl1Pins;
                                }
                                if (scores[recordNr].bowlnumber == 3)
                                {
                                    if (scores[recordNr].isStrike)
                                        bowl3 = "X";
                                    else
                                        if (scores[recordNr].isSpare)
                                            bowl3 = "/";
                                        else
                                            if (bowl2Pins == 10 || bowl1Pins+bowl2Pins == 10 )
                                                bowl3 = scores[recordNr].total.ToString();
                                            else
                                                bowl3 = (scores[recordNr].total-bowl2Pins).ToString();
                                }
                                break;
                        }
                    }

                    _scores += "   <div class='col-lg-1' style='border-width: 0 0 0 1px; border-style: solid; border-color: black;'>";
                    _scores += "       <div class='row'>";
                    _scores += "           <div style='float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl1 + "</div>";
                    _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>" + bowl2 + "</div>";
                    _scores += "       </div>";
                    _scores += "       <div class='row'>";
                    _scores += "           <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>" + progressiveTotal + "</div>";
                    _scores += "       </div>";
                    _scores += "   </div>";
                    
                    if (recordNr > 0)
                    {
                        for (int i = scores[recordNr - 1].framenumber; i < 10; i++)
                        {
                            _scores += "   <div class='col-lg-1' style='border-width: 0 0 0 1px; border-style: solid; border-color: black;'>";
                            _scores += "       <div class='row'>";
                            _scores += "           <div style='float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                            _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>&nbsp;</div>";
                            _scores += "           <div style='float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;'>&nbsp;</div>";
                            _scores += "       </div>";
                            _scores += "       <div class='row'>";
                            _scores += "           <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>&nbsp;</div>";
                            _scores += "       </div>";
                            _scores += "   </div>";
                        }
                    }

                    _scores += "   <div class='col-lg-1' style='border-width: 0 1px 0 1px; border-style: solid; border-color: black;'>";
                    _scores += "   <div class='row'>";
                    _scores += "       <div style='float:left;width:34%;border-width: 1px 1px 1px 0; border-style: solid; border-color: black;'>" + bowl3 + "</div>";
                    _scores += "       <div style='float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div style='float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "   </div>";
                    _scores += "   <div class='row'>";
                    _scores += "       <div style='border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px'>" + progressiveTotal + "</div>";
                    _scores += "   </div>";
                    _scores += "</div>";
                    _scores += "<div class='col-lg-1' style='border-width: 0 0 0 0; border-style: solid; border-color: black;'>";
                    _scores += "   <div class='row'>";
                    _scores += "      <img src='img/Facebook-Share.jpg' width='50px'/>";
                    _scores += "   </div>";
                    _scores += "</div>";
                    _scores += "</div>";
                }

                if (!first)
                {
                    _scores += "   <div class='row'>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 1px 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "       <div class='col-lg-1' style='border-width: 0 0 0 0; border-style: solid; border-color: black;'>&nbsp;</div>";
                    _scores += "   </div>";
                }
            }
            _scores += "</div>";
        }

        protected void buttonSubmitForm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool auth = Convert.ToBoolean(Session["auth"]);

                if (!auth)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    bool validUser = true;
                    string result = string.Empty;

                    if (profilewachtwoord.Text.CompareTo(profilecontrole.Text) != 0) { result = "Het wachtwoord is niet twee keer hetzelfde ingevuld!"; validUser = false; }

                    if (validUser)
                    {
                        long id = Convert.ToInt64(Session["uid"]);
                        S_User user = UserManager.GetUserById(id);

                        user.name = profilenaam.Text;
                        user.address = profileadres.Text;
                        user.city = profileplaats.Text;

                        if (!string.IsNullOrEmpty(profilewachtwoord.Text))
                            user.password = profilewachtwoord.Text;

                        UserManager.Update(user);

                        result = "De gegevens zijn opgeslagen!";

                        buildCompetitions(user);
                        buildScores(user);
                    }
                    if (!validUser)
                        profileSuccess.Attributes.Add("style", "color:red");
                    else
                        profileSuccess.Attributes.Add("style", "color:black");

                    profileSuccess.InnerHtml = result;

                }
            }
        }

        protected void btnRemoveAccount_Click(object sender, EventArgs e)
        {
            bool auth = Convert.ToBoolean(Session["auth"]);

            if (!auth)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                // find the user
                long id = Convert.ToInt64(Session["uid"]);
                S_User user = UserManager.GetUserById(id);

                // remove favorits
                FavoritManager.DeleteFavoritsByUserId(user.id);

                // remove competitionplayers
                CompetitionManager.DeleteCompetitionPlayer(user.id);

                // remove user
                UserManager.Delete(user.id);

                Response.Redirect("~/Logout.aspx");
            }
        }
    }
}