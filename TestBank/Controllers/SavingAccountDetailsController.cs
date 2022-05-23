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
    public class SavingAccountDetailsController : Controller
    {
        private TestBankDBEntities2 db = new TestBankDBEntities2();

        // GET: SavingAccountDetails
        public ActionResult Index()
        {
            var savingAccountDetails = db.SavingAccountDetails.Include(s => s.CustomerAccount);
            return View(savingAccountDetails.ToList());
        }

        // GET: SavingAccountDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingAccountDetail savingAccountDetail = db.SavingAccountDetails.Find(id);
            if (savingAccountDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.acno = id;
            return View(savingAccountDetail);
        }

        // GET: SavingAccountDetails/Create
        public ActionResult Create(int? id)
        {
            CustomerAccount c = db.CustomerAccounts.Find(id);
            ViewBag.c = c;
           
            ViewBag.AccNum = new SelectList(db.CustomerAccounts, "AccNum", "AccountType");
            return View();
        }

        // POST: SavingAccountDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccNum,SavingAccType,IFSCcode,Balance,TrasferLimit,BranchCode")] SavingAccountDetail savingAccountDetail)
        {
            if (ModelState.IsValid)
            {
                db.SavingAccountDetails.Add(savingAccountDetail);
                db.SaveChanges();
                return RedirectToAction("Details",new { id = savingAccountDetail.AccNum });
            }

            ViewBag.AccNum = new SelectList(db.CustomerAccounts, "AccNum", "AccountType", savingAccountDetail.AccNum);
            return View(savingAccountDetail);
        }

        // GET: SavingAccountDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingAccountDetail savingAccountDetail = db.SavingAccountDetails.Find(id);
            if (savingAccountDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.SavingAccType = new SelectList(new List<object> { "Regular", "Salary" });
            ViewBag.AccNum = new SelectList(db.CustomerAccounts, "AccNum", "AccountType", savingAccountDetail.AccNum);
            return View(savingAccountDetail);
        }

        // POST: SavingAccountDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccNum,SavingAccType,IFSCcode,Balance,TrasferLimit,BranchCode")] SavingAccountDetail savingAccountDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(savingAccountDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Search","CustomerDetails");
            }
            ViewBag.AccNum = new SelectList(db.CustomerAccounts, "AccNum", "AccountType", savingAccountDetail.AccNum);
            return View(savingAccountDetail);
        }

        // GET: SavingAccountDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingAccountDetail savingAccountDetail = db.SavingAccountDetails.Find(id);
            if (savingAccountDetail == null)
            {
                return HttpNotFound();
            }
            return View(savingAccountDetail);
        }

        // POST: SavingAccountDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SavingAccountDetail savingAccountDetail = db.SavingAccountDetails.Find(id);
            db.SavingAccountDetails.Remove(savingAccountDetail);
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
