using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using LaundryWebapp.DataSource;
using LaundryWebapp.Enum;
using Microsoft.AspNet.Identity;

namespace LaundryWebapp.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private LaundryEntities db = new LaundryEntities();

        // GET: Transactions
        public ActionResult Index(string Status)
        {
            Status = Status ?? "";
            var transactions = db.Transactions.Include(t => t.MasterCustomer).Include(t => t.MasterPayment).Include(t => t.MasterItem).Where(x => x.Status.Contains(Status));
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        [HttpPost]
        public ActionResult PrintAction()
        {
            Print();
            return RedirectToAction("Details");
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,PaymentId,ItemId,DiscountPrice,Weight,Price,PaymentStatus")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                int currDay = DateTime.Now.Day;
                int count = db.Transactions.Count(x => x.TransactionDate.Day == currDay);
                transaction.Id = Guid.NewGuid().ToString();
                transaction.TransactionNumber = $"TR-{DateTime.Now.ToString("ddMMyyyy")}-{count}";
                transaction.TransactionDate = DateTime.Now;
                var item = db.MasterItems.FirstOrDefault(x => x.Id == transaction.ItemId);

                if (item.Name.ToUpper().Contains("kilat"))
                    transaction.EstimationClear = DateTime.Now.AddDays(1);
                else
                    transaction.EstimationClear = DateTime.Now.AddDays(3);

                transaction.Status = Enum.Enum.Status.Process.ToString();
                transaction.CreatedDate = DateTime.Now;
                transaction.CreatedBy = User.Identity.GetUserName();
                transaction.ModifiedDate = DateTime.Now;
                transaction.ModifiedBy = User.Identity.GetUserName();
                transaction.IsActive = true;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.MasterCustomers, "Id", "Name", transaction.CustomerId);
            ViewBag.PaymentId = new SelectList(db.MasterPayments, "Id", "Code", transaction.PaymentId);
            ViewBag.ItemId = new SelectList(db.MasterItems, "Id", "Code", transaction.ItemId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TransactionNumber,TransactionDate,CustomerId,PaymentId,ItemId,DiscountPrice,Weight,Price,EstimationClear,Status,PaymentStatus,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void Print()
        {
            var printer = new SerialPrinter("COM3", 57600);
            var e = new EPSON();
            printer.Write(
                ByteSplicer.Combine(
                    e.CenterAlign(),
                    e.PrintLine("Nira Laundry")
                )
            );
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
