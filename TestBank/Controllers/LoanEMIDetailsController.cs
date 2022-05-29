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
    public class LoanEMIDetailsController : Controller
    {
        private BankEntities db = new BankEntities();

        // GET: LoanEMIDetails
        public ActionResult Index()
        {
            var loanEMIDetails = db.LoanEMIDetails.Include(l => l.LoanAccountDetail);
            return View(loanEMIDetails.ToList());
        }

        // GET: LoanEMIDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanEMIDetail loanEMIDetail = db.LoanEMIDetails.Find(id);
            if (loanEMIDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccNum = loanEMIDetail.AccNum;
            return View(loanEMIDetail);
        }

        // GET: LoanEMIDetails/Create
        public ActionResult Create(int? id)
        {
            LoanAccountDetail la = db.LoanAccountDetails.Find(id);
            LoanEMIDetail le = new LoanEMIDetail();
            le.AccNum = la.AccNum;
           
            le.EMIStatus = "Pending";
            le.RemainingBalance = la.BalanceAmount;

            DateTime d = (DateTime)la.LoanIssuedDate;
            if (la.BalanceAmount != 0)
            {
                var c = db.LoanEMIDetails.Where(x=>x.AccNum==la.AccNum).Count();
                if (c == 0) {
                    d = d.AddMonths(1);
                }else
                {
                    d = d.AddMonths(c+1);
                }
                le.EMIDate = d;
                le.EMIReminder = d.AddMonths(1);
              
            }

           // ViewBag.AccNum = new SelectList(db.LoanAccountDetails, "AccNum", "IFSCcode");
            return View(le);
        }

        // POST: LoanEMIDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EMIID,AccNum,EMIDate,EMIAmount,EMIStatus,RemainingBalance,EMIReminder")] LoanEMIDetail loanEMIDetail)
        {
            if (ModelState.IsValid)
            {
                LoanAccountDetail l = db.LoanAccountDetails.Find(loanEMIDetail.AccNum);
                l.BalanceAmount -= loanEMIDetail.EMIAmount;
                loanEMIDetail.RemainingBalance = l.BalanceAmount;
                db.LoanEMIDetails.Add(loanEMIDetail);
                db.SaveChanges();
                return RedirectToAction("Details",new { id = loanEMIDetail.EMIID });
            }

            ViewBag.AccNum = new SelectList(db.LoanAccountDetails, "AccNum", "IFSCcode", loanEMIDetail.AccNum);
            return View(loanEMIDetail);
        }

        // GET: LoanEMIDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanEMIDetail loanEMIDetail = db.LoanEMIDetails.Find(id);
            if (loanEMIDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccNum = new SelectList(db.LoanAccountDetails, "AccNum", "IFSCcode", loanEMIDetail.AccNum);
            return View(loanEMIDetail);
        }

        // POST: LoanEMIDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMIID,AccNum,EMIDate,EMIAmount,EMIStatus,RemainingBalance,EMIReminder")] LoanEMIDetail loanEMIDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanEMIDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccNum = new SelectList(db.LoanAccountDetails, "AccNum", "IFSCcode", loanEMIDetail.AccNum);
            return View(loanEMIDetail);
        }

        // GET: LoanEMIDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanEMIDetail loanEMIDetail = db.LoanEMIDetails.Find(id);
            if (loanEMIDetail == null)
            {
                return HttpNotFound();
            }
            return View(loanEMIDetail);
        }

        // POST: LoanEMIDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanEMIDetail loanEMIDetail = db.LoanEMIDetails.Find(id);
            db.LoanEMIDetails.Remove(loanEMIDetail);
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
