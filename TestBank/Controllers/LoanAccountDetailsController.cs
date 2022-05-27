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
    public class LoanAccountDetailsController : Controller
    {
        private TestBankDBEntities2 db = new TestBankDBEntities2();

        // GET: LoanAccountDetails
        public ActionResult Index()
        {
            var loanAccountDetails = db.LoanAccountDetails.Include(l => l.CustomerAccount);
            return View(loanAccountDetails.ToList());
        }

        // GET: LoanAccountDetails/Details/5

        public ActionResult ViewAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanAccountDetail loanAccountDetail = db.LoanAccountDetails.Find(id);
            if (loanAccountDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccNum = loanAccountDetail.AccNum;
            return View(loanAccountDetail);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanAccountDetail loanAccountDetail = db.LoanAccountDetails.Find(id);
            if (loanAccountDetail == null)
            {
                return HttpNotFound();
            }
           
            return View(loanAccountDetail);
        }

        public ActionResult Grid(int? id)
        {
            var loanEMIDetails = db.LoanEMIDetails.Where(x => x.AccNum == id).ToList();
            return PartialView(loanEMIDetails);
        }
      
     
      

        // GET: LoanAccountDetails/Create
        public ActionResult Create(int? id)
        {
            CustomerAccount obj = db.CustomerAccounts.Find(id);
            LoanAccountDetail la = new LoanAccountDetail();
            la.AccNum = obj.AccNum;
            la.LoanAccountType = obj.AccountSubType;
            ViewBag.AccNum = la.AccNum;
           
            return View(la);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccNum,IFSCcode,BalanceAmount,BranchCode,Principle,RateOfInterest,LoanDuration,TotalLoanAmount,MonthlyPayment,LoanAccountType,LoanIssuedDate,LoanPayDate")] LoanAccountDetail loanAccountDetail)
        {
            if (ModelState.IsValid)
            {
                
                    db.LoanAccountDetails.Add(loanAccountDetail);
                    db.SaveChanges();
                    return RedirectToAction("Details",new { id = loanAccountDetail.AccNum });
              
            }

            ViewBag.AccNum = new SelectList(db.CustomerAccounts, "AccNum", "AccountType", loanAccountDetail.AccNum);
            return View(loanAccountDetail);
        }

        // GET: LoanAccountDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanAccountDetail loanAccountDetail = db.LoanAccountDetails.Find(id);
            if (loanAccountDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccNum = new SelectList(db.CustomerAccounts, "AccNum", "AccountType", loanAccountDetail.AccNum);
            return View(loanAccountDetail);
        }

        // POST: LoanAccountDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccNum,IFSCcode,BalanceAmount,BranchCode,Principle,RateOfInterest,LoanDuration,TotalLoanAmount,MonthlyPayment,LoanAccountType,LoanIssuedDate,LoanPayDate")] LoanAccountDetail loanAccountDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loanAccountDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccNum = new SelectList(db.CustomerAccounts, "AccNum", "AccountType", loanAccountDetail.AccNum);
            return View(loanAccountDetail);
        }

        // GET: LoanAccountDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanAccountDetail loanAccountDetail = db.LoanAccountDetails.Find(id);
            if (loanAccountDetail == null)
            {
                return HttpNotFound();
            }
            return View(loanAccountDetail);
        }

        // POST: LoanAccountDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoanAccountDetail loanAccountDetail = db.LoanAccountDetails.Find(id);
            db.LoanAccountDetails.Remove(loanAccountDetail);
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
