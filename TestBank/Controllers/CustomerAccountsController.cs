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
    public class CustomerAccountsController : Controller
    {
        private TestBankDBEntities2 db = new TestBankDBEntities2();

        // GET: CustomerAccounts
        public ActionResult Index()
        {
            var customerAccounts = db.CustomerAccounts.Include(c => c.SavingAccountDetail).Include(c => c.CustomerDetail);
            return View(customerAccounts.ToList());
        }

        // GET: CustomerAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAccount customerAccount = db.CustomerAccounts.Find(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }
            return View(customerAccount);
        }

        // GET: CustomerAccounts/Create
        public ActionResult Create(int? id)
        {
            CustomerDetail customerdetail = db.CustomerDetails.Find(id);
             ViewBag.customer = customerdetail;
            CustomerAccount c = new CustomerAccount();
            c.CustID = customerdetail.CustID;
            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType");
            ViewBag.CustID = new SelectList(db.CustomerDetails, "CustID", "FirstName");
            ViewBag.Acctypes = new SelectList(new List<object> { "Savings", "Loan" });
            ViewBag.Accsubtypes = new SelectList(new List<object> { "Regular", "Salary" });
            return View(c);
        }

        // POST: CustomerAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustID,AccNum,AccountType,AccountSubType")] CustomerAccount customerAccount)
        {
            if (ModelState.IsValid)
            {
                db.CustomerAccounts.Add(customerAccount);
                 db.SaveChanges();
                if (customerAccount.AccountType == "Savings")
                {
                    return RedirectToAction("Create", "SavingAccountDetails",new { id = customerAccount.AccNum });
                }
                else
                {
                    return RedirectToAction("Create", "LoanAccountDetails", new { id = customerAccount.AccNum });
                }
            }

            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType", customerAccount.AccNum);
           ViewBag.CustID = new SelectList(db.CustomerDetails, "CustID", "FirstName", customerAccount.CustID);
            return View(customerAccount);
        }

        // GET: CustomerAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAccount customerAccount = db.CustomerAccounts.Find(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType", customerAccount.AccNum);
            ViewBag.CustID = new SelectList(db.CustomerDetails, "CustID", "FirstName", customerAccount.CustID);
            return View(customerAccount);
        }

        // POST: CustomerAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustID,AccNum,AccountType,AccountSubType")] CustomerAccount customerAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccNum = new SelectList(db.SavingAccountDetails, "AccNum", "SavingAccType", customerAccount.AccNum);
            ViewBag.CustID = new SelectList(db.CustomerDetails, "CustID", "FirstName", customerAccount.CustID);
            return View(customerAccount);
        }

        // GET: CustomerAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAccount customerAccount = db.CustomerAccounts.Find(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }
            return View(customerAccount);
        }

        // POST: CustomerAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerAccount customerAccount = db.CustomerAccounts.Find(id);
            db.CustomerAccounts.Remove(customerAccount);
            db.SaveChanges();
            return RedirectToAction("Search","CustomerDetails");
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
