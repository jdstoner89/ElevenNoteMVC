using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        // GET: Note
        public ActionResult Index()
        {
            var UserId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(UserId);
            var model = service.GetNotes();
            return View(model);
 
        }
        //GET
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (ModelState.IsValid) return View(model);
 
            var service = CreateNotService();

            if (service.CreateNote(model))
            {
                TempData["SaveResult"] = "Your note was created.";
                return RedirectToAction("Index");
            };
            ModelState.AddModelError("", "Note could not be created.");

            return View(model);
        }
        public ActionResult Details(int id)
        {
            var svc = CreateNotService();
            var model = svc.GetNoteById(id);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var service = CreateNotService();
            var detail = service.GetNoteById(id);
            var model =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content
                };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid) return View(model);
            if(model.NoteId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }
            var service = CreateNotService();
            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Your note could not be updated.");
            return View();
        }
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateNotService();
            var model = svc.GetNoteById(id);

            return View(model);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateNotService();
            service.DeleteNote(id);
            TempData["SaveResult"] = "Your note was deleted.";
            return RedirectToAction("Index");
        }
        private NoteService CreateNotService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}