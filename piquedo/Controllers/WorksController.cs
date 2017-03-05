using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using piquedo.Models;

namespace piquedo.Controllers
{
    public class WorksController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Works
        public ActionResult Index()
        {
            var works = db.Works.Include(w => w.AspNetUser);
            return View(works.ToList());
        }

        // GET: Works/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // GET: Works/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.PostingUserID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Works/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PostingUserID,Title,Description,PostingDate,Location,FromDate,Expiry,WorkType,SkillLevel,WorkCategory,AlternateContact,RenumerationType,RenumerationAmount,ImgUrl,Tags")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Works.Add(work);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostingUserID = new SelectList(db.AspNetUsers, "Id", "Email", work.PostingUserID);
            return View(work);
        }

        // GET: Works/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostingUserID = new SelectList(db.AspNetUsers, "Id", "Email", work.PostingUserID);
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostingUserID,Title,Description,PostingDate,Location,FromDate,Expiry,WorkType,SkillLevel,WorkCategory,AlternateContact,RenumerationType,RenumerationAmount,ImgUrl,Tags")] Work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostingUserID = new SelectList(db.AspNetUsers, "Id", "Email", work.PostingUserID);
            return View(work);
        }

        // GET: Works/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = db.Works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Work work = db.Works.Find(id);
            db.Works.Remove(work);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
