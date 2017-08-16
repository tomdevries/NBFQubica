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
    public class TextController : Controller
    {
        //
        // GET: /Text/

        [Authorize]
        public ActionResult Index(string name)
        {
            ObservableCollection<TextGridModel> textModelList = new ObservableCollection<TextGridModel>();

            List<S_Text> textList;

            textList = TextManager.GetTexts();

            foreach (S_Text text in textList)
            {
                TextGridModel tgm = new TextGridModel();
                tgm.Id = text.id;
                tgm.Label = text.label;
                textModelList.Add(tgm);
            }

            return View(textModelList);
        }

        [Authorize]
        public ActionResult Edit(long id)
        {
            S_Text text = TextManager.GetTextById(id);
            TextModel textModel = new TextModel();

            textModel.Id = text.id;
            textModel.label = text.label;
            textModel.text = text.text;

            return View(textModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(TextModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to save the text
                try
                {
                    S_Text text = TextManager.GetTextById(model.Id);

                    text.text = model.text;
                    text.id = model.Id;

                    TextManager.Update(text);

                    return RedirectToAction("index", "text", null);
                }
                catch (Exception e)
                {
                    TempData["error"] = "Er is een fout opgetreden";
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
