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
    public class SavingAccountTxnHistoriesController : Controller
    {
        private TestBankDBEntities2 db = new TestBankDBEntities2();

        // GET: SavingAccountTxnHistories
        public ActionResult Index()
        {
            var savingAccountTxnHistories = db.SavingAccountTxnHistories.Include(s => s.SavingAccountDetail);
            return View(savingAccountTxnHistories.ToList());
        }

        // GET: SavingAccountTxnHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingAccountTxnHistory savingAccountTxnHistory = db.SavingAccountTxnHistories.Find(id);
            if (savingAccountTxnHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccNum = savingAccountTxnHistory.AccNum;
            return View(savingAccountTxnHistory);
        }
        public ActionResult Withdraw(int? id)
        {
            SavingAccountDetail c = db.SavingAccountDetails.Find(id);
            
            SavingAccountTxnHistory o = new SavingAccountTxnHistory();
            o.AccNum = c.AccNum;
            o.Balance = c.Balance;
            ViewBag.AccNum = o.AccNum;
           // ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType");
            return View(o);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw([Bind(Include = "TxnID,TxnDate,AccNum,Balance,SourceType,TransType,Amount")] SavingAccountTxnHistory savingAccountTxnHistory)
        {
            SavingAccountDetail s = db.SavingAccountDetails.Find(savingAccountTxnHistory.AccNum);
            if (s.Balance < savingAccountTxnHistory.Amount)
            {
                ModelState.AddModelError("Balance", "Insufficient Balance");
            }
            else
            {
                var transactions = db.SavingAccountTxnHistories.Where(x => x.AccNum == savingAccountTxnHistory.AccNum ).Where( x => x.TxnDate == savingAccountTxnHistory.TxnDate).Where(x => x.TransType == savingAccountTxnHistory.TransType).ToList();
                decimal limit = 0;
                foreach (var trans in transactions)
                { 
                     limit+=trans.Amount;
                }
                if (limit > s.TrasferLimit)
                {
                    ModelState.AddModelError("Amount", "Transfer Limit Exceeded");
                }
                else
                {
                    s.Balance = s.Balance - savingAccountTxnHistory.Amount;
                    savingAccountTxnHistory.Balance = s.Balance;
                    db.SaveChanges();
                }
            }
            if (ModelState.IsValid)
            {
                db.SavingAccountTxnHistories.Add(savingAccountTxnHistory);
                db.SaveChanges();
                return RedirectToAction("Details", "SavingAccountTxnHistories", new { id = savingAccountTxnHistory.TxnID });
            }

            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType", savingAccountTxnHistory.AccNum);
            return View(savingAccountTxnHistory);
        }
        // GET: SavingAccountTxnHistories/Create
        public ActionResult Create(int? id)
        {
           SavingAccountDetail c = db.SavingAccountDetails.Find(id);
            //ViewBag.c = c;
            SavingAccountTxnHistory o = new SavingAccountTxnHistory();
            o.AccNum = c.AccNum;
            o.Balance = c.Balance;
            ViewBag.AccNum = o.AccNum;
           // ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType");
            return View(o);
        }

        // POST: SavingAccountTxnHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TxnID,TxnDate,AccNum,Balance,SourceType,TransType,Amount")] SavingAccountTxnHistory savingAccountTxnHistory)
        {
            if (ModelState.IsValid)
            {
                SavingAccountDetail s = db.SavingAccountDetails.Find(savingAccountTxnHistory.AccNum);
                s.Balance = s.Balance + savingAccountTxnHistory.Amount;
                savingAccountTxnHistory.Balance = s.Balance;
                db.SaveChanges();
                db.SavingAccountTxnHistories.Add(savingAccountTxnHistory);
                db.SaveChanges();
                return RedirectToAction("Details", "SavingAccountTxnHistories", new { id = savingAccountTxnHistory.TxnID });
            }

            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType", savingAccountTxnHistory.AccNum);
            return View(savingAccountTxnHistory);
        }

        // GET: SavingAccountTxnHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingAccountTxnHistory savingAccountTxnHistory = db.SavingAccountTxnHistories.Find(id);
            if (savingAccountTxnHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType", savingAccountTxnHistory.AccNum);
            return View(savingAccountTxnHistory);
        }

        // POST: SavingAccountTxnHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TxnID,TxnDate,AccNum,Balance,SourceType,TransType,Amount")] SavingAccountTxnHistory savingAccountTxnHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(savingAccountTxnHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType", savingAccountTxnHistory.AccNum);
            return View(savingAccountTxnHistory);
        }

        // GET: SavingAccountTxnHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavingAccountTxnHistory savingAccountTxnHistory = db.SavingAccountTxnHistories.Find(id);
            if (savingAccountTxnHistory == null)
            {
                return HttpNotFound();
            }
            return View(savingAccountTxnHistory);
        }

        // POST: SavingAccountTxnHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SavingAccountTxnHistory savingAccountTxnHistory = db.SavingAccountTxnHistories.Find(id);
            db.SavingAccountTxnHistories.Remove(savingAccountTxnHistory);
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
