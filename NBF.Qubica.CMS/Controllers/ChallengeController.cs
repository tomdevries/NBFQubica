using NBF.Qubica.Classes;
using NBF.Qubica.CMS.Models;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBF.Qubica.CMS.Controllers
{
    public class ChallengeController : Controller
    {
        //
        // GET: /Competition/

        [Authorize]
        public ActionResult Index(string name)
        {
            ObservableCollection<ChallengeGridModel> challengeModelList = new ObservableCollection<ChallengeGridModel>();

            List<S_Challenge> challengeList;

            challengeList = ChallengeManager.GetChallenges();

            foreach (S_Challenge challenge in challengeList)
            {
                ChallengeGridModel cgm = new ChallengeGridModel();
                cgm.Id = challenge.id;
                cgm.Name = challenge.name;
                challengeModelList.Add(cgm);
            }

            return View(challengeModelList);
        }
    }
}
