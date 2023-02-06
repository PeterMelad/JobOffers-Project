using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public ActionResult Index()
        {
            var jobs = db.Jobs.Include(j => j.category).ToList();
            return View(jobs);
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["jobid"] = id;
            return View(job);
        }
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply(string Message)
        {
            var userid = User.Identity.GetUserId();
            var jobid = (int)Session["jobid"];
            var check = db.ApplyForJobs.Where(a => a.jobid == jobid && a.userid == userid).ToList();
            if (check.Count==0)
            {
            var job = new ApplyForJob()
            {
                jobid=jobid,
                userid=userid,
                message=Message,
                dateTime=DateTime.Now
            };
            db.ApplyForJobs.Add(job);
            db.SaveChanges();
                TempData["result"] = "applied successfully";
            }
            else
            {
                TempData["result"] = "you already applied to this job";
            }
            return RedirectToAction("index");
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName");
            return View();
        }

        // POST: Jobs/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job,HttpPostedFileBase upload)
        {
            if (upload == null)
            {
                ModelState.AddModelError("", "Upload image please");
                var x = job.JobImage;
            }
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                //Server.MapPath("~/Uploads") => da hyrg3 el path bta3 el server
                upload.SaveAs(path);    // kda ana 7tet el path bta3 el image fe el server
                job.JobImage = upload.FileName;     // kda ana 7tet asm el image fe el database
                job.publisherId = User.Identity.GetUserId();
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName", job.CategoryId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName", job.CategoryId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Job job, HttpPostedFileBase upload)
        {
          
            if (ModelState.IsValid)
            {
                string oldpath = Path.Combine(Server.MapPath("~/Uploads"), job.JobImage);

                if(upload != null)
                {
                System.IO.File.Delete(oldpath);
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);    
                job.JobImage = upload.FileName; 
                }
                   
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName", job.CategoryId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
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
