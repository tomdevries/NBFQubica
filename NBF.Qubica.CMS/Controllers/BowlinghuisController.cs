using NBF.Qubica.Classes;
using NBF.Qubica.CMS.Models;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NBF.Qubica.CMS.Controllers
{
    public class BowlinghuisController : Controller
    {
        [Authorize]
        public ActionResult Index(string name)
        {
            ObservableCollection<BowlinghuisGridModel> bowlinghuisList = new ObservableCollection<BowlinghuisGridModel>();
            
            List<S_BowlingCenter> bowlingcenterList;

            if (String.IsNullOrEmpty(name))
                bowlingcenterList = BowlingCenterManager.GetBowlingCenters();
            else { 
                bowlingcenterList = BowlingCenterManager.GetBowlingCentersByName(name);
            
                if (bowlingcenterList.Count() == 0)
                    TempData["error"] = "Er zijn geen bowlinghuizen gevonden.";
                else
                    if (bowlingcenterList.Count() == 1)
                        TempData["message"] = "Er is 1 bowlinghuis gevonden.";
                    else
                        TempData["message"] = "Er zijn " + bowlingcenterList.Count().ToString() + " bowlinghuizen gevonden.";
            }

            foreach(S_BowlingCenter bowlingcenter in bowlingcenterList)
            {
                BowlinghuisGridModel bhgm = new BowlinghuisGridModel();
                bhgm.id = bowlingcenter.id;
                bhgm.Name = bowlingcenter.name;
                bhgm.Address = bowlingcenter.address;
                bhgm.Zipcode = bowlingcenter.zipcode;
                bhgm.City = bowlingcenter.city;
                bhgm.Phonenumber = bowlingcenter.phonenumber;
                bhgm.Appname = bowlingcenter.appname;
                bhgm.SecretKey = bowlingcenter.secretkey;

                bowlinghuisList.Add(bhgm);
            }

            return View(bowlinghuisList);
        } 

        //
        // GET: /Bowlinghuis/Insert
        [Authorize]
        public ActionResult Insert()
        {
            BowlinghuisModel bowlinghuisModel = new BowlinghuisModel();

            SelectListItem selectListItem = new SelectListItem();
            selectListItem.Value = "1.00.00";
            selectListItem.Text = "1.00.00";

            bowlinghuisModel.ApiVersions = new Collection<SelectListItem>() { selectListItem };

            return View(bowlinghuisModel);
        }

        //
        // POST: /Bowlinghuis/Insert
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(BowlinghuisModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the bowlinghuis
                try
                {
                    S_BowlingCenter bowlingcenter = new S_BowlingCenter();
                    bowlingcenter.name = model.Name;
                    bowlingcenter.uri = model.Uri;
                    bowlingcenter.centerId = model.Port;
                    bowlingcenter.address = model.Address;
                    bowlingcenter.APIversion = model.ApiVersion;
                    bowlingcenter.appname = model.Appname;
                    bowlingcenter.secretkey = model.Secretkey;
                    bowlingcenter.city = model.City;
                    bowlingcenter.email = model.Email;
                    bowlingcenter.lastSyncDate = model.LastSyncDate;
                    bowlingcenter.logo = model.UrlLogo;
                    bowlingcenter.numberOfLanes = model.NumberOfLanes;
                    bowlingcenter.phonenumber = model.Phonenumber;
                    bowlingcenter.website = model.Website;
                    bowlingcenter.zipcode = model.ZipCode;

                    BowlingCenterManager.Insert(bowlingcenter);
                    TempData["message"] = "Het bowlinghuis is toegevoegd.";

                    return RedirectToAction("index", "Bowlinghuis", new { name = ""});
                }
                catch (Exception e)
                {
                    TempData["error"] = "Er is een fout opgetreden.";
                }
            }

            // If we got this far, something failed, redisplay form
            SelectListItem selectListItem = new SelectListItem();
            selectListItem.Value = "1.00.00";
            selectListItem.Text = "1.00.00";

            model.ApiVersions = new Collection<SelectListItem>() { selectListItem };

            return View(model);
        }

        [Authorize]
        public ActionResult Edit(long id)
        {
            S_BowlingCenter bowlingcenter = BowlingCenterManager.GetBowlingCenterById(id);
            BowlinghuisModel bowlinghuisModel = new BowlinghuisModel();

            bowlinghuisModel.Id = bowlingcenter.id;
            bowlinghuisModel.Name = bowlingcenter.name;
            bowlinghuisModel.Address = bowlingcenter.address;
            bowlinghuisModel.ApiVersion = bowlingcenter.APIversion;
            bowlinghuisModel.Appname = bowlingcenter.appname;
            bowlinghuisModel.Secretkey = bowlingcenter.secretkey;
            bowlinghuisModel.City = bowlingcenter.city;
            bowlinghuisModel.Email = bowlingcenter.email;
            bowlinghuisModel.LastSyncDate = bowlingcenter.lastSyncDate.Value;
            bowlinghuisModel.NumberOfLanes = bowlingcenter.numberOfLanes.Value;
            bowlinghuisModel.Phonenumber = bowlingcenter.phonenumber;
            bowlinghuisModel.Port = bowlingcenter.centerId.Value;
            bowlinghuisModel.Uri = bowlingcenter.uri;
            bowlinghuisModel.UrlLogo = bowlingcenter.logo;
            bowlinghuisModel.Website = bowlingcenter.website;
            bowlinghuisModel.ZipCode = bowlingcenter.zipcode;

            SelectListItem selectListItem = new SelectListItem();
            selectListItem.Value = "1.00.00";
            selectListItem.Text = "1.00.00";

            bowlinghuisModel.ApiVersions = new Collection<SelectListItem>() { selectListItem };
            return View(bowlinghuisModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(BowlinghuisModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the bowlinghuis
                try
                {
                    S_BowlingCenter bowlingcenter = BowlingCenterManager.GetBowlingCenterById(model.Id);

                    bowlingcenter.name = model.Name;
                    bowlingcenter.uri = model.Uri;
                    bowlingcenter.centerId = model.Port;
                    bowlingcenter.address = model.Address;
                    bowlingcenter.APIversion = model.ApiVersion;
                    bowlingcenter.appname = model.Appname;
                    bowlingcenter.secretkey = model.Secretkey;
                    bowlingcenter.city = model.City;
                    bowlingcenter.email = model.Email;
                    bowlingcenter.lastSyncDate = model.LastSyncDate;
                    bowlingcenter.logo = model.UrlLogo;
                    bowlingcenter.numberOfLanes = model.NumberOfLanes;
                    bowlingcenter.phonenumber = model.Phonenumber;
                    bowlingcenter.website = model.Website;
                    bowlingcenter.zipcode = model.ZipCode;

                    BowlingCenterManager.Update(bowlingcenter);
                    TempData["message"] = "Het bowlinghuis " + bowlingcenter.name + " is aangepast.";

                    return RedirectToAction("index", "Bowlinghuis", new { name = "" });
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
                BowlingCenterManager.Delete(id);
                TempData["message"] = "Het bowlinghuis is verwijderd.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            
            return RedirectToAction("index", "Bowlinghuis", new { name = "" });
        }

        [Authorize]
        public ActionResult Opentimes(long id)
        {
            ObservableCollection<OpentimeGridModel> opentimeModelList = new ObservableCollection<OpentimeGridModel>();

            List<S_Opentime> opentimeList;

            opentimeList = OpentimeManager.GetOpentimesByBowlingcenterId(id);

            foreach (S_Opentime opentime in opentimeList)
            {
                OpentimeGridModel opentimeGridModel = new OpentimeGridModel();
                opentimeGridModel.Id = opentime.id;
                opentimeGridModel.BowlingcenterId = opentime.bowlingCenterId;
                opentimeGridModel.Day = ConvertDay(opentime.day);
                opentimeGridModel.Opentime = opentime.openTime;
                opentimeGridModel.Closetime = opentime.closeTime;

                opentimeModelList.Add(opentimeGridModel);
            }

            ViewBag.bowlingcenterid = id;

            return View(opentimeModelList);
        }

        private string ConvertDay(Day day)
        {
            switch(day)
            {
                case Day.Friday: return "Vrijdag";
                case Day.Monday: return "Maandag";
                case Day.Saterday: return "Zaterdag";
                case Day.Sunday: return "Zondag";
                case Day.Thursday: return "Donderdag";
                case Day.Tuesday: return "Dinsdag";
                case Day.Wednesday: return "Woensdag";
            }
            return "";
        }

        //
        // GET: /Bowlinghuis/Insert
        [Authorize]
        public ActionResult InsertOpentime(long id)
        {
            OpentimeModel opentimeModel = new OpentimeModel();
            opentimeModel.BowlingcenterId = id;

            SelectListItem monday = new SelectListItem();
            monday.Value = "Monday";
            monday.Text = "Maandag";

            SelectListItem tuesday = new SelectListItem();
            tuesday.Value = "Tuesday";
            tuesday.Text = "Dinsdag";

            SelectListItem wednesday = new SelectListItem();
            wednesday.Value = "Wednesday";
            wednesday.Text = "Woensdag";

            SelectListItem thursday = new SelectListItem();
            thursday.Value = "Thursday";
            thursday.Text = "Donderdag";

            SelectListItem friday = new SelectListItem();
            friday.Value = "Friday";
            friday.Text = "Vrijdag";

            SelectListItem saterday = new SelectListItem();
            saterday.Value = "Saterday";
            saterday.Text = "Zaterdag";

            SelectListItem sunday = new SelectListItem();
            sunday.Value = "Sunday";
            sunday.Text = "Zondag";

            opentimeModel.Days = new Collection<SelectListItem>() { monday, tuesday, wednesday, thursday, friday, saterday, sunday };

            return View(opentimeModel);
        }

        //
        // POST: /Bowlinghuis/Insert
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult InsertOpentime(OpentimeModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the opentime
                try
                {
                    S_Opentime opentime = new S_Opentime();
                    opentime.day = (Day)Enum.Parse(typeof(Day), model.Day);
                    opentime.bowlingCenterId = model.BowlingcenterId;
                    opentime.openTime = model.Opentime;
                    opentime.closeTime = model.Closetime;

                    OpentimeManager.Insert(opentime);
                    TempData["message"] = "De openingstijd is toegevoegd.";

                    return RedirectToAction("opentimes", "Bowlinghuis", new { id = model.BowlingcenterId });
                }
                catch (Exception e)
                {
                    TempData["error"] = "Er is een fout opgetreden.";
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult EditOpentime(long id)
        {
            S_Opentime opentime = OpentimeManager.GetOpentimeById(id);
            OpentimeModel opentimeModel = new OpentimeModel();

            opentimeModel.Id = opentime.id;
            opentimeModel.Day = opentime.day.ToString();
            opentimeModel.Opentime = opentime.openTime;
            opentimeModel.Closetime = opentime.closeTime;

            SelectListItem monday = new SelectListItem();
            monday.Value = "Monday";
            monday.Text = "Maandag";

            SelectListItem tuesday = new SelectListItem();
            tuesday.Value = "Tuesday";
            tuesday.Text = "Dinsdag";

            SelectListItem wednesday = new SelectListItem();
            wednesday.Value = "Wednesday";
            wednesday.Text = "Woensdag";

            SelectListItem thursday = new SelectListItem();
            thursday.Value = "Thursday";
            thursday.Text = "Donderdag";

            SelectListItem friday = new SelectListItem();
            friday.Value = "Friday";
            friday.Text = "Vrijdag";

            SelectListItem saterday = new SelectListItem();
            saterday.Value = "Saterday";
            saterday.Text = "Zaterdag";

            SelectListItem sunday = new SelectListItem();
            sunday.Value = "Sunday";
            sunday.Text = "Zondag";

            opentimeModel.Days = new Collection<SelectListItem>() { monday, tuesday, wednesday, thursday, friday, saterday, sunday };
            return View(opentimeModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditOpentime(OpentimeModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the opentime
                try
                {
                    S_Opentime opentime = OpentimeManager.GetOpentimeById(model.Id);

                    opentime.day = (Day)Enum.Parse(typeof(Day), model.Day);
                    opentime.openTime = model.Opentime;
                    opentime.closeTime = model.Closetime;

                    OpentimeManager.Update(opentime);
                    TempData["message"] = "De openingstijd is aangepast.";

                    return RedirectToAction("opentimes", "Bowlinghuis", new { id = opentime.bowlingCenterId });
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
        public ActionResult DeleteOpentime(long id)
        {
            S_Opentime opentime = null;

            try
            {
                opentime = OpentimeManager.GetOpentimeById(id);

                OpentimeManager.Delete(id);
                TempData["message"] = "De openingstijd is verwijderd.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("opentimes", "Bowlinghuis", new { id = opentime.bowlingCenterId });
        }

        [Authorize]
        public ActionResult Adverts(long id)
        {
            ObservableCollection<AdvertGridModel> advertModelList = new ObservableCollection<AdvertGridModel>();

            List<S_Advert> advertList;

            advertList = AdvertManager.GetAdvertsByBowlingCenterid(id);

            foreach (S_Advert advert in advertList)
            {
                AdvertGridModel advertGridModel = new AdvertGridModel();
                advertGridModel.Id = advert.id;
                advertGridModel.BowlingcenterId = advert.bowlingcenterId;
                advertGridModel.Advertisement = advert.advertisement;
                advertGridModel.AdvertisementUrl = advert.advertisement_url;
                advertGridModel.AdvertisementWWW = advert.advertisement_www;

                advertModelList.Add(advertGridModel);
            }

            ViewBag.bowlingcenterid = id;

            return View(advertModelList);
        }

        //
        // GET: /Bowlinghuis/Insert
        [Authorize]
        public ActionResult InsertAdvert(long id)
        {
            AdvertModel advertModel = new AdvertModel();
            advertModel.BowlingcenterId = id;

            return View(advertModel);
        }

        //
        // POST: /Bowlinghuis/Insert
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult InsertAdvert(AdvertModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the advert
                try
                {
                    S_Advert advert = new S_Advert();
                    advert.advertisement = model.Advertisement;
                    advert.bowlingcenterId = model.BowlingcenterId;
                    advert.advertisement_url = model.AdvertisementUrl;
                    advert.advertisement_www = model.AdvertisementWWW;

                    HttpPostedFileBase remoteFile = Request.Files["UploadedFile"];

                    if ((remoteFile != null) && (remoteFile.ContentLength > 0) && !string.IsNullOrEmpty(remoteFile.FileName))
                    {
                        string remoteFileName = remoteFile.FileName;
                        string remoteFileContentType = remoteFile.ContentType;
                        byte[] remoteFileBytes = new byte[remoteFile.ContentLength];
                        remoteFile.InputStream.Read(remoteFileBytes, 0, Convert.ToInt32(remoteFile.ContentLength));

                        var fileName = Path.GetFileName(remoteFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Banners"), fileName);
                        remoteFile.SaveAs(path);

                        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                        advert.advertisement_url = baseUrl + "/Banners/" + fileName;
                    }

                    AdvertManager.Insert(advert);
                    TempData["message"] = "De advertentie is toegevoegd.";

                    return RedirectToAction("adverts", "Bowlinghuis", new { id = model.BowlingcenterId });
                }
                catch (Exception e)
                {
                    TempData["error"] = "Er is een fout opgetreden.";
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize]
        public ActionResult EditAdvert(long id)
        {
            S_Advert advert = AdvertManager.GetAdvertById(id);
            AdvertModel advertModel = new AdvertModel();

            advertModel.Id = advert.id;
            advertModel.Advertisement = advert.advertisement;
            advertModel.AdvertisementUrl = advert.advertisement_url;
            advertModel.AdvertisementWWW = advert.advertisement_www;

            return View(advertModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditAdvert(AdvertModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the opentime
                try
                {
                    S_Advert advert = AdvertManager.GetAdvertById(model.Id);

                    advert.advertisement = model.Advertisement;
                    advert.advertisement_url = model.AdvertisementUrl;
                    advert.advertisement_www = model.AdvertisementWWW;

                    HttpPostedFileBase remoteFile = Request.Files["UploadedFile"];

                    if ((remoteFile != null) && (remoteFile.ContentLength > 0) && !string.IsNullOrEmpty(remoteFile.FileName))
                    {
                        string remoteFileName = remoteFile.FileName;
                        string remoteFileContentType = remoteFile.ContentType;
                        byte[] remoteFileBytes = new byte[remoteFile.ContentLength];
                        remoteFile.InputStream.Read(remoteFileBytes, 0, Convert.ToInt32(remoteFile.ContentLength));

                        var fileName = Path.GetFileName(remoteFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Banners"), fileName);
                        remoteFile.SaveAs(path);

                        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                        advert.advertisement_url = baseUrl + "/Banners/" + fileName;
                    } 
                    
                    AdvertManager.Update(advert);
                    TempData["message"] = "De advertentie is aangepast.";

                    return RedirectToAction("adverts", "Bowlinghuis", new { id = advert.bowlingcenterId });
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
        public ActionResult DeleteAdvert(long id)
        {
            S_Advert advert = null;

            try
            {
                advert = AdvertManager.GetAdvertById(id);

                AdvertManager.Delete(id);
                TempData["message"] = "De advertenttie is verwijderd.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("adverts", "Bowlinghuis", new { id = advert.bowlingcenterId });
        }
    }
}
