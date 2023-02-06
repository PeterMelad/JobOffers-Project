using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult GetUserjobs()
        {
            var userid = User.Identity.GetUserId();
            var jobs = db.ApplyForJobs.Where(a => a.userid == userid).ToList();
            return View(jobs);
        }
        public ActionResult JobDetails(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job != null) return View(job);
            
           return RedirectToAction("GetUserjobs");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetUserjobs");
            }
            return View(job);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            db.ApplyForJobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("GetUserjobs");
        }
        [Authorize]
        public ActionResult getpublisherjobs()
        {
            var userid = User.Identity.GetUserId();
            var jobs = db.Jobs.Where(a => a.publisherId == userid).ToList();
           return View(jobs);
        }
        [Authorize]
        public ActionResult getusersapplied()
        {
            var userid = User.Identity.GetUserId();

            var jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.job.id equals job.id
                       where job.publisherId == userid
                       select app;
            
            return View(jobs.ToList());
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult searchfield(string Search)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(Search) ||
              a.JobContent.Contains(Search) || a.category.CategoryName.Contains(Search) || a.category.CategoryDescription.Contains(Search)).ToList();

            return View(result);
        }

    }
}