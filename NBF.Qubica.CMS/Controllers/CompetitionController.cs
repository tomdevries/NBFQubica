using NBF.Qubica.Classes;
using NBF.Qubica.CMS.Models;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NBF.Qubica.CMS.Controllers
{
    public class CompetitionController : Controller
    {
        [Authorize]
        public ActionResult Competition(long id)
        {
            S_Challenge challenge = ChallengeManager.GetChallenge(id);

            ObservableCollection<CompetitionGridModel> competitionModelList = new ObservableCollection<CompetitionGridModel>();

            List<S_Competition> competitionList;

            competitionList = CompetitionManager.GetCompetitionsByChallengeId(id);

            foreach (S_Competition competition in competitionList)
            {
                CompetitionGridModel cgm = new CompetitionGridModel();
                cgm.Id = competition.id;
                cgm.challenge = challenge.name;
                cgm.challengeId = id;
                cgm.description = competition.description;
                cgm.EndDate = competition.enddate;
                cgm.Id = competition.id;
                cgm.price = competition.price;
                cgm.StartDate = competition.startdate;

                competitionModelList.Add(cgm);
            }

            ViewBag.challengeid = id;

            return View(competitionModelList);
        }

        [Authorize]
        public ActionResult Insert(long id)
        {
            S_Challenge s = ChallengeManager.GetChallenge(id);
            CompetitionModel m = new CompetitionModel();
            m.challengeId = id;
            m.challenge = s.name;

            return View(m);
        }

        [Authorize]
        public ActionResult competitionplayers(long id, long challengeid)
        {
            ObservableCollection<PlayerGridModel> playerModelList = new ObservableCollection<PlayerGridModel>();

            List<S_CompetitionPlayers> playerList;

            playerList = CompetitionManager.GetPlayersByCompetition(id);

            foreach (S_CompetitionPlayers player in playerList)
            {
                S_User user = UserManager.GetUserById(player.userid);

                PlayerGridModel pgm = new PlayerGridModel();
                pgm.Id = player.id;
                pgm.Name = user.name;
                pgm.FrequentBowlernumber = user.frequentbowlernumber;
                pgm.competitionId = id;

                playerModelList.Add(pgm);
            }

            ViewBag.competitionid = id;
            ViewBag.challengeid = challengeid;

            return View(playerModelList);
        }

        [Authorize]
        public ActionResult competitionplayersinsert(long id, string name, long challengeid)
        {
            ObservableCollection<PlayerModel> playerCompetitionList = new ObservableCollection<PlayerModel>();

            List<S_Player> playerList;

            if (String.IsNullOrEmpty(name))
                playerList = CompetitionManager.GetPlayersNotInCompetition(id);
            else
            {
                playerList = CompetitionManager.GetPlayersNotInCompetitionByName(id, name);

                if (playerList.Count() == 0)
                    TempData["error"] = "Er zijn geen spelers gevonden.";
                else
                    if (playerList.Count() == 1)
                        TempData["message"] = "Er is 1 speler gevonden.";
                    else
                        TempData["message"] = "Er zijn " + playerList.Count().ToString() + " spelers gevonden.";
            }

            foreach (S_Player player in playerList)
            {
                S_User user = UserManager.GetUserById(player.userid);

                PlayerModel pm = new PlayerModel();
                pm.Id = player.userid;
                pm.Name = user.name;
                pm.FrequentBowlernumber = user.frequentbowlernumber;

                playerCompetitionList.Add(pm);
            }
            
            ViewBag.competitionid = id;
            ViewBag.challengeid = challengeid;

            return View(playerCompetitionList);
        }

        [Authorize]
        public ActionResult competitionranking(long id, string name, long challengeid)
        {
            ObservableCollection<PlayerRankingGridModel> playerRankingModelList = new ObservableCollection<PlayerRankingGridModel>();
            ObservableCollection<PlayerRankingGridModel> filteredPlayerRankingModelList = new ObservableCollection<PlayerRankingGridModel>();

            List<S_CompetitionPlayers> playerList;
            playerList = CompetitionManager.GetPlayersByCompetition(id);

            if (playerList != null && playerList.Count() > 0)
            {
                S_Competition competition = CompetitionManager.GetCompetition(playerList[0].competitionid);
                S_Challenge challenge = ChallengeManager.GetChallengeByCompetition(playerList[0].competitionid);
                List<S_CompetitionPlayersRanking> cprl = CompetitionManager.GetCompetitionPlayersRanking(challenge.id, playerList[0].competitionid, playerList, competition.startdate, competition.enddate);

                foreach (S_CompetitionPlayersRanking cpr in cprl)
                {
                    PlayerRankingGridModel prgm = new PlayerRankingGridModel();
                    prgm.Rank = cpr.Rank;
                    prgm.UserId = cpr.UserId;
                    prgm.Name = cpr.Name;
                    prgm.FrequentBowlernumber = cpr.FrequentBowlernumber;
                    prgm.Score = cpr.Score;

                    playerRankingModelList.Add(prgm);
                }

                int rank = cprl.Count();
                if (challengeid != 6)
                {
                    foreach (S_CompetitionPlayers cp in playerList)
                    {
                        bool playerInRanking = false;
                        foreach (PlayerRankingGridModel prgm in playerRankingModelList)
                            if (prgm.UserId == cp.userid)
                                playerInRanking = true;
                        if (!playerInRanking)
                        {
                            S_User u = UserManager.GetUserById(cp.userid);
                            playerRankingModelList.Add(new PlayerRankingGridModel { Name = u.name, FrequentBowlernumber = u.frequentbowlernumber, Rank = ++rank });
                        }
                    }
                }

                foreach (PlayerRankingGridModel prgm in playerRankingModelList)
                {
                    if (name == null || prgm.Name.ToUpper().Contains(name.ToUpper()))
                        filteredPlayerRankingModelList.Add(prgm);
                }
            }

            ViewBag.competitionid = id;
            ViewBag.challengeid = challengeid;

            return View(filteredPlayerRankingModelList);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(CompetitionModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the contact
                try
                {
                    S_Competition competition = new S_Competition();
                    competition.challengeid = model.challengeId;
                    competition.description = model.description;
                    competition.enddate = model.EndDate;
                    competition.price = model.price;
                    competition.startdate = model.StartDate;

                    CompetitionManager.Insert(competition);

                    return RedirectToAction("Competition", "Competition", new { id = competition.challengeid });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", "Fout");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(long id)
        {
            S_Competition competition = CompetitionManager.GetCompetition(id);
            CompetitionModel competitionModel = new CompetitionModel();

            competitionModel.Id = competition.id;
            competitionModel.challengeId = competition.challengeid;
            competitionModel.challenge = ChallengeManager.GetChallenge(competition.challengeid).name;
            competitionModel.description = competition.description;
            competitionModel.StartDate = competition.startdate;
            competitionModel.EndDate = competition.enddate;
            competitionModel.price = competition.price;

            List<S_BowlingCenter> bcl = BowlingCenterManager.GetBowlingCenters();
            List<S_CompetitonBowlingcenter> cbcl = CompetitionManager.GetBowlingcentersByCompetition(competition.id);
            competitionModel.AllBowlingCentersChecked = bcl.Count == cbcl.Count;

            //var selectedBowlingCenters = CheckboxManager.GetAll()
            //   .Where(x => cbcl.Any(s => x.Id.ToString().Equals(s.bowlingcenterid)))
            //   .ToList();

            List<C_Checkbox> selectedBowlingCenters = new List<C_Checkbox>();
            foreach (S_CompetitonBowlingcenter cbc in cbcl)
            {
                S_BowlingCenter bc = BowlingCenterManager.GetBowlingCenterById(cbc.bowlingcenterid);
                selectedBowlingCenters.Add(new C_Checkbox { Id = cbc.bowlingcenterid, Name = bc.name });
            }
            //setup a view model
            competitionModel.AvailableBowlingCenters = CheckboxManager.GetAll().ToList();
            competitionModel.SelectedBowlingCenters = selectedBowlingCenters;

            return View(competitionModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(CompetitionModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the competition
                try
                {
                    S_Competition competition = CompetitionManager.GetCompetition(model.Id);

                    competition.challengeid = model.challengeId;
                    competition.description = model.description;
                    competition.enddate = model.EndDate;
                    competition.startdate = model.StartDate;
                    competition.price = model.price;
                    competition.id = model.Id;

                    CompetitionManager.Update(competition);
                    CompetitionManager.DeleteAllBowlingCentersByCompetition(competition.id);

                    // setup properties
                    var selectedBowlingCenters = new List<C_Checkbox>();
                    var postedBowlingCenterIds = new string[0];
                    if (model.PostedBowlingCenters == null) model.PostedBowlingCenters = new C_PostedCheckbox();

                    // if a view model array of posted ids exists
                    // and is not empty, save selected ids
                    if (model.PostedBowlingCenters.CheckboxIds != null && model.PostedBowlingCenters.CheckboxIds.Any())
                    {
                        postedBowlingCenterIds = model.PostedBowlingCenters.CheckboxIds;
                        foreach (var id in postedBowlingCenterIds)
                            CompetitionManager.InsertBowlingcenterForCompetition(competition.id, long.Parse(id));
                    }

                    TempData["message"] = "De competitie met nummer " + competition.id + " is aangepast.";

                    return RedirectToAction("competition", "Competition", new { id = model.challengeId });
                }
                catch (Exception e)
                {
                    TempData["error"] = "Er is een fout opgetreden";
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult Delete(long id)
        {
            S_Competition c = CompetitionManager.GetCompetition(id);

            try
            {
                CompetitionManager.Delete(id);
                TempData["message"] = "De competitie is verwijderd.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("competition", "Competition", new { id = c.challengeid });
        }

        [Authorize]
        public ActionResult DeletePlayer(long id, long challengeid)
        {
            S_CompetitionPlayers c = CompetitionManager.GetCompetitionPlayer(id);

            try
            {
                CompetitionManager.DeletePlayerByID(c.id);
                TempData["message"] = "De deelnemer is verwijderd.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("CompetitionPlayers", "competition", new { id = c.competitionid, challengeid = challengeid });
        }

        [Authorize]
        public ActionResult AddCompetionPlayer(long id, long cid, long challengeid)
        {
            try
            {
                CompetitionManager.AddPlayer(id, cid);
                TempData["message"] = "De deelnemer is toegevoegd.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("CompetitionPlayers", "competition", new { id = cid, challengeid = challengeid });
        }
    }
}
