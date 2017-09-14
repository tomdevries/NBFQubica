using NBF.Qubica.Classes;
using NBF.Qubica.Common;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class CompetitieStanden : System.Web.UI.Page
    {
        protected string _content = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;

            if (!this.IsPostBack)
            {
                List<S_Competition> competitions = CompetitionManager.GetCompetitions(true);

                //ListItem item = new ListItem();
                //item.Value = "0";
                //item.Text = "-- Selecteer een competitie --";
                //compCompetitie.Items.Add(item);

                foreach (S_Competition competition in competitions)
                {
                    S_Challenge challenge = ChallengeManager.GetChallenge(competition.challengeid);

                    //item = new ListItem();
                    //item.Value = competition.id.ToString();
                    //item.Text = Conversion.DateToTitle(competition.startdate, competition.enddate) + " : " + challenge.email;
                    //compCompetitie.Items.Add(item);

                    List<S_CompetitionPlayers> playerList;
                    playerList = CompetitionManager.GetPlayersByCompetition(competition.id);

                    if (playerList != null && playerList.Count() > 0)
                    {
                        challenge = ChallengeManager.GetChallengeByCompetition(playerList[0].competitionid);
                        List<S_CompetitionPlayersRanking> cprl = CompetitionManager.GetCompetitionPlayersRanking(challenge.id, playerList[0].competitionid, playerList, competition.startdate, competition.enddate);

                        List<S_CompetitionPlayersRanking> playerRankingModelList = new List<S_CompetitionPlayersRanking>();
                        foreach (S_CompetitionPlayersRanking c in cprl)
                            playerRankingModelList.Add(c);


                        int rank = playerRankingModelList.Count();
                        if (challenge.id != 6)
                        {
                            foreach (S_CompetitionPlayers cp in playerList)
                            {
                                bool playerInRanking = false;
                                foreach (S_CompetitionPlayersRanking c in cprl)
                                    if (c.UserId == cp.userid)
                                        playerInRanking = true;
                                if (!playerInRanking)
                                {
                                    S_User u = UserManager.GetUserById(cp.userid);
                                    playerRankingModelList.Add(new S_CompetitionPlayersRanking { Name = u.name, FrequentBowlernumber = u.frequentbowlernumber, Rank = ++rank });
                                }
                            }
                        }

                        _content += "<br/><br/><h4>" + Conversion.DateToTitle(competition.startdate, competition.enddate) + " : " + challenge.name + "</h4>";
                        _content += "<table width='100%'>";
                        int row = 0;
                        foreach (S_CompetitionPlayersRanking s in playerRankingModelList)
                        {
                            string background = "lightgrey";
                            if (row++ % 2 == 0)
                                background = "white";
                            _content += "<tr><td width='20px' style='background-color:" + background + ";text-align:left;border-bottom: 1px solid black'>" + s.Rank + "</td><td width='120px' style='background-color:" + background + ";text-align:left;border-bottom: 1px solid black'>" + s.Name + "</td><td width='30px' style='background-color:" + background + ";text-align:left;border-bottom: 1px solid black'>" + s.Score + "</td></tr>";
                        }
                        _content += "</table>";
                    }
                }
            }
        }
    }
}