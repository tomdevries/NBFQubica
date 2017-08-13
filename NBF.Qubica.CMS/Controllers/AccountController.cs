using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using NBF.Qubica.CMS.Models;
using NBF.Qubica.Managers;
using NBF.Qubica.Classes;
using System.Collections.ObjectModel;

namespace NBF.Qubica.CMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [Authorize]
        public ActionResult Index(string name)
        {
            ObservableCollection<AccountGridModel> accountList = new ObservableCollection<AccountGridModel>();

            List<S_User> userList;

            if (String.IsNullOrEmpty(name))
                userList = UserManager.GetUsers();
            else
            {
                userList = UserManager.GetUsersByName(name);
            
                if (userList.Count() == 0)
                    TempData["error"] = "Er zijn geen accounts gevonden.";
                else
                    if (userList.Count() == 1)
                        TempData["message"] = "Er is 1 account gevonden.";
                    else
                        TempData["message"] = "Er zijn " + userList.Count().ToString() + " accounts gevonden.";
            }

            foreach (S_User user in userList)
            {
                AccountGridModel accountGridModel = new AccountGridModel();
                accountGridModel.Id = user.id;
                accountGridModel.Name = user.name;
                accountGridModel.Address = user.address;
                accountGridModel.City = user.city;
                accountGridModel.Email = user.email;
                accountGridModel.FrequentBowlerNumber = user.frequentbowlernumber;
                accountList.Add(accountGridModel);
            }

            return View(accountList);
        } 
        
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                S_User user = UserManager.GetUserByNameAndPassword(model.UserName, model.Password);
                if (user != null && user.roleid == 0 && user.isRegistrationConfirmed)
                {
                    FormsAuthentication.SetAuthCookie(user.name, false);
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "De gebruikersnaam of het wachtwoord is niet correct.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the contact
                try
                {
                    S_User user = new S_User();
                    user.name = model.Name;
                    user.email = model.Email;
                    user.address = model.Address;
                    user.city = model.City;
                    user.username = model.UserName;
                    user.password = model.Password;
                    user.roleid = Role.USER;
                    user.isMember = model.IsMember;
                    user.memberNumber = model.MemberNumber;
                    user.isRegistrationConfirmed = false;
                    user.frequentbowlernumber = model.FrequentBowlerNumber;

                    UserManager.Insert(user);

                    //return RedirectToAction("Index", "Home", new { name = user.name });
                    return RedirectToAction("RegisterResult", "Account", new { name = user.name} );
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [Authorize]
        public ActionResult Insert()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the contact
                try
                {
                    S_User user = new S_User();
                    user.name = model.Name;
                    user.email = model.Email;
                    user.address = model.Address;
                    user.city = model.City;
                    user.username = model.UserName;
                    user.password = model.Password;
                    user.roleid = Role.ADMINISTRATOR;
                    user.isMember = model.IsMember;
                    user.memberNumber = model.MemberNumber;
                    user.isRegistrationConfirmed = true;
                    user.frequentbowlernumber = model.FrequentBowlerNumber;

                    UserManager.Insert(user);

                    return RedirectToAction("Index", "Account", new { name = user.name });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // Get: /Account/RegisterResult/<name>
        [AllowAnonymous]
        public ActionResult RegisterResult(string name)
        {
            ViewBag.Name = name;

            return View();
        }

        [Authorize]
        public ActionResult Edit(long id)
        {
            S_User user = UserManager.GetUserById(id);
            AccountModel accountModel = new AccountModel();

            accountModel.Id = user.id;
            accountModel.Name = user.name;
            accountModel.Address = user.address;
            accountModel.City = user.city;
            accountModel.Email = user.email;
            accountModel.UserName = user.username;
            accountModel.IsAdmin = Role.ADMINISTRATOR == user.roleid;
            accountModel.Password = "";
            accountModel.ConfirmPassword = "";
            accountModel.IsMember = user.isMember;
            accountModel.MemberNumber = user.memberNumber;
            accountModel.IsRegistrationConfirmed = user.isRegistrationConfirmed;
            accountModel.FrequentBowlerNumber = user.frequentbowlernumber;
                
            return View(accountModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the bowlinghuis
                try
                {
                    S_User user = UserManager.GetUserById(model.Id);

                    user.name = model.Name;
                    user.address = model.Address;
                    user.city = model.City;
                    user.email = model.Email;
                    user.username = model.UserName;
                    if (model.IsAdmin)
                        user.roleid = Role.ADMINISTRATOR;
                    else
                        user.roleid = Role.USER;

                    if (!String.IsNullOrEmpty(model.Password))
                        user.password = model.Password;

                    user.isMember = model.IsMember;
                    user.memberNumber = model.MemberNumber;
                    user.isRegistrationConfirmed = model.IsRegistrationConfirmed;
                    user.frequentbowlernumber = model.FrequentBowlerNumber;

                    UserManager.Update(user);
                    TempData["message"] = "Het account " + user.name + " is aangepast.";

                    return RedirectToAction("index", "Account", new { name = "" });
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
            try
            {
                UserManager.Delete(id);
                TempData["message"] = "Het account is verwijderd.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("index", "Account", new { name = "" });
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Gebruikersnaam bestaat reeds. Kies een andere gebruikersnaam.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Een gebruikersnaam voor het gekozen e-mailadres bestaat reeds. Kies een ander e-mailadres";

                case MembershipCreateStatus.InvalidPassword:
                    return "Het wachtwoord is incorrect, kies een correct wachtwoord";

                case MembershipCreateStatus.InvalidEmail:
                    return "Het e-mailadres is niet correct. Controleer het e-mailadres en probeer het opnieuw.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Het wachtwoord herstel antwoord is niet correct. Controleer de waarde en probeer het opnieuw.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "De wachtwoord herstel vraag is niet correct. Controleer de waarde en probeer het opnieuw.";

                case MembershipCreateStatus.InvalidUserName:
                    return "De gebruikersnaam is niet correct. Controleer de waarde en probeer het opnieuw.";

                case MembershipCreateStatus.ProviderError:
                    return "De authenticatie provider levert een fout op. Controleer de waarden en probeer het opnieuw. Indien het probleem blijft bestaan, neem dan contact op met de beheerder.";

                case MembershipCreateStatus.UserRejected:
                    return "Het registeren van de gebruiker is afgebroken. Controleer de waarden en probeer het opnieuw. Indien het probleem blijft bestaan, neem dan contact op met de beheerder.";

                default:
                    return "Een onbekende fout is opgetreden. Controleer de waarden en probeer het opnieuw. Indien het probleem blijft bestaan, neem dan contact op met de beheerder.";
            }
        }
        #endregion
    }
}
