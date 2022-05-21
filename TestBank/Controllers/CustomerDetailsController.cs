using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestBank.Models;


namespace TestBank.Controllers
{
    public class CustomerDetailsController : Controller
    {
        private TestBankDBEntities1 db = new TestBankDBEntities1();

        // GET: CustomerDetails
        public ActionResult Index()
        { 
        var customerDetails = db.CustomerDetails.Include(c => c.City1).Include(c => c.Country1).Include(c => c.PostalCode).Include(c => c.State1);
            return View(customerDetails.ToList());
        }

        public List<Country> GetCountryList()
        {
            List<Country> countries = db.Countries.ToList();
            return countries;
        }
        public ActionResult GetStateList(int? CountryCode)
        {
            List<State> selectList = db.States.Where(x=>x.CountryCode==CountryCode).ToList();
            ViewBag.Slist = new SelectList(selectList, "StateCode", "StateName");
            return PartialView("DisplayStates");
        }
        
            public ActionResult Grid(int? id)
        {
            var customerAccounts = db.CustomerAccounts.Where(x => x.CustID == id).ToList();
            //var customerAccounts = db.CustomerAccounts.Include(c => c.SavingAccountDetail).Include(c => c.CustomerDetail);
            return PartialView(customerAccounts);
        }
        public ActionResult Search()
        {
         
             ViewBag.City = new SelectList(db.Cities, "CityCode", "CityName");
            ViewBag.CountryList = new SelectList(db.Countries, "CountryCode", "CountryName");
            ViewBag.ZIPCode = new SelectList(db.PostalCodes, "ZipCode", "ZipCode");
            ViewBag.State = new SelectList(db.States, "StateCode", "StateName");
            ViewBag.accounts = new SelectList(db.CustomerAccounts);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDetail customerDetail = db.CustomerDetails.Find(id);
            if (customerDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = customerDetail.CustID;
            return View(customerDetail);


        }
        // get: customerdetails/details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDetail customerDetail = db.CustomerDetails.Find(id);
            if (customerDetail == null)
            {
                return HttpNotFound();
            }
            return View(customerDetail);
        }

        // GET: CustomerDetails/Create
        public ActionResult Create()
        {
            ViewBag.City = new SelectList(db.Cities, "CityCode", "CityName");
            ViewBag.Country = new SelectList(db.Countries, "CountryCode", "CountryName");
            ViewBag.ZIPCode = new SelectList(db.PostalCodes, "ZipCode", "ZipCode");
            ViewBag.State = new SelectList(db.States, "StateCode", "StateName");
            return View();
        }

        // POST: CustomerDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustID,FirstName,LastName,Address1,Address2,EmailId,Mobile,DOB,MaritalStatus,ZIPCode,City,State,Country")] CustomerDetail customerDetail)
        {
            if (ModelState.IsValid)
            {
                db.CustomerDetails.Add(customerDetail);
                db.SaveChanges();
                return RedirectToAction("Search");
            }

            ViewBag.City = new SelectList(db.Cities, "CityCode", "CityName", customerDetail.City);
            ViewBag.Country = new SelectList(db.Countries, "CountryCode", "CountryName", customerDetail.Country);
            ViewBag.ZIPCode = new SelectList(db.PostalCodes, "ZipCode", "ZipCode", customerDetail.ZIPCode);
            ViewBag.State = new SelectList(db.States, "StateCode", "StateName", customerDetail.State);
            return View(customerDetail);
        }

        // GET: CustomerDetails/Edit/5
        public ActionResult Edit(int? id)
        {
         
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDetail customerDetail = db.CustomerDetails.Find(id);
            if (customerDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.City = new SelectList(db.Cities, "CityCode", "CityName", customerDetail.City);
            ViewBag.Country = new SelectList(db.Countries, "CountryCode", "CountryName", customerDetail.Country);
            ViewBag.ZIPCode = new SelectList(db.PostalCodes, "ZipCode", "ZipCode", customerDetail.ZIPCode);
            ViewBag.State = new SelectList(db.States, "StateCode", "StateName", customerDetail.State);
            return View(customerDetail);
        }

        // POST: CustomerDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustID,FirstName,LastName,Address1,Address2,EmailId,Mobile,DOB,MaritalStatus,ZIPCode,City,State,Country")] CustomerDetail customerDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Search");
            }
            ViewBag.City = new SelectList(db.Cities, "CityCode", "CityName", customerDetail.City);
            ViewBag.Country = new SelectList(db.Countries, "CountryCode", "CountryName", customerDetail.Country);
            ViewBag.ZIPCode = new SelectList(db.PostalCodes, "ZipCode", "ZipCode", customerDetail.ZIPCode);
            ViewBag.State = new SelectList(db.States, "StateCode", "StateName", customerDetail.State);
            return View(customerDetail);
        }

        // GET: CustomerDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDetail customerDetail = db.CustomerDetails.Find(id);
            if (customerDetail == null)
            {
                return HttpNotFound();
            }
            return View(customerDetail);
        }

        // POST: CustomerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerDetail customerDetail = db.CustomerDetails.Find(id);
            db.CustomerDetails.Remove(customerDetail);
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
