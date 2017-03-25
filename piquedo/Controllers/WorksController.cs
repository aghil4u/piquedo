using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using piquedo.Models;
using Microsoft.AspNet.Identity;


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
        public ActionResult Create([Bind(Include = "Title,Description,Location,Latitude,Longitude,FromDate,Expiry,WorkType,SkillLevel,WorkCategory,AlternateContact,RenumerationType,RenumerationAmount,ImgUrl,Tags")] Work work)
        {
       


            work.Id = (int.Parse(db.Works.Max(w => w.Id))+1).ToString();
            work.PostingUserID =User.Identity.GetUserId();
            work.PostingDate=DateTime.Now;
            
            if (work.Expiry==null)
            {
                work.Expiry = DateTime.Now.AddDays(180);
            }
            if (ModelState.IsValid)
            {
                db.Works.Add(work);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostingUserID = new SelectList(db.AspNetUsers, "Id", "Email", work.PostingUserID);

    
            return View(work);
        }

        [HttpPost]
        public ActionResult uploadImage(string image)
        {
            return null;
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


        [HttpPost]
        public ActionResult UploadFiles()
        {
            string fname="";
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];


                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = DateTime.Now.ToString("yyyyMMddHHmmss")+file.FileName.Substring(file.FileName.Length-4);
                        }

                        // Get the complete folder path and store the file inside it.  
                        
                        file.SaveAs(Path.Combine(Server.MapPath("~/images/Userimages/"), fname));
                    }
                    // Returns message that successfully uploaded  
                    return Json("/images/Userimages/"+fname);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}
