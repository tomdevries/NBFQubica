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
    public class BondController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            S_Federation federation;
            FederationModel federationModel = new FederationModel();

            if (FederationManager.FederationExistById(1))
            { 
                federation = FederationManager.GetFederationById(1);
                federationModel.Id = federation.id;
                federationModel.Information = federation.information;
                federationModel.UrlLogo = federation.logo;
            }

            return View(federationModel);
        }
                
        //
        // POST: /Bond
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FederationModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the bond
                try
                {
                    S_Federation federation = new S_Federation();
                    federation.information = model.Information;
                    federation.id = model.Id;
                    federation.logo = model.UrlLogo;

                    if (!FederationManager.FederationExistById(1))
                    {
                        FederationManager.Insert(federation);
                        TempData["message"] = "De bond is toegevoegd.";
                    }
                    else
                    { 
                        FederationManager.Update(federation);
                        TempData["message"] = "De bond is bijgewerkt.";
                    }

                    return RedirectToAction("index", "Bond");
                }
                catch (Exception e)
                {
                    TempData["error"] = "Er is een fout opgetreden.";
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
