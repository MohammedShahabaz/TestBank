using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestBank.Models;

namespace TestBank.Controllers
{
    public class PostalCodesController : Controller
    {
        private BankEntities db = new BankEntities();

        // GET: PostalCodes
        public ActionResult Index()
        {
            var postalCodes = db.PostalCodes.Include(p => p.City);
            return View(postalCodes.ToList());
        }

        // GET: PostalCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostalCode postalCode = db.PostalCodes.Find(id);
            if (postalCode == null)
            {
                return HttpNotFound();
            }
            return View(postalCode);
        }

        // GET: PostalCodes/Create
        public ActionResult Create()
        {
            ViewBag.Citycode = new SelectList(db.Cities, "CityCode", "CityName");
            return View();
        }

        // POST: PostalCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ZipCode,Citycode")] PostalCode postalCode)
        {
            if (ModelState.IsValid)
            {
                db.PostalCodes.Add(postalCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Citycode = new SelectList(db.Cities, "CityCode", "CityName", postalCode.Citycode);
            return View(postalCode);
        }

        // GET: PostalCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostalCode postalCode = db.PostalCodes.Find(id);
            if (postalCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.Citycode = new SelectList(db.Cities, "CityCode", "CityName", postalCode.Citycode);
            return View(postalCode);
        }

        // POST: PostalCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ZipCode,Citycode")] PostalCode postalCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postalCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Citycode = new SelectList(db.Cities, "CityCode", "CityName", postalCode.Citycode);
            return View(postalCode);
        }

        // GET: PostalCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostalCode postalCode = db.PostalCodes.Find(id);
            if (postalCode == null)
            {
                return HttpNotFound();
            }
            return View(postalCode);
        }

        // POST: PostalCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostalCode postalCode = db.PostalCodes.Find(id);
            db.PostalCodes.Remove(postalCode);
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
