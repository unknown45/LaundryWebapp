using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaundryWebapp.DataSource;
using LaundryWebapp.Enum;

namespace LaundryWebapp.Controllers
{
    public class TransactionHeadersController : Controller
    {
        private LaundryEntities db = new LaundryEntities();

        // GET: TransactionHeaders
        public ActionResult Index(string Status)
        {
            Status = Status ?? "";
            var transactionHeaders = db.TransactionHeaders.Include(t => t.MasterCustomer).Where(x => x.Status.Contains(Status));
            return View(transactionHeaders.ToList());
        }

        // GET: TransactionHeaders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionHeader transactionHeader = db.TransactionHeaders.Find(id);
            if (transactionHeader == null)
            {
                return HttpNotFound();
            }
            return View(transactionHeader);
        }

        // GET: TransactionHeaders/Create
        public ActionResult Create()
        {
            var CustomerList = new SelectList(db.MasterCustomers, "Id", "Name");
            var PaymentList = new SelectList(db.MasterPayments, "Id", "Description");
            ViewBag.CustomerId = CustomerList;
            return View();
        }

        // POST: TransactionHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TransactionNumber,TransactionDate,CustomerId,DiscountPrice,TotalPrice,EstimationClear,Status,PaymentStatus,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] TransactionHeader transactionHeader)
        {
            if (ModelState.IsValid)
            {
                db.TransactionHeaders.Add(transactionHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.MasterCustomers, "Id", "Name", transactionHeader.CustomerId);
            return View(transactionHeader);
        }

        // GET: TransactionHeaders/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionHeader transactionHeader = db.TransactionHeaders.Find(id);
            if (transactionHeader == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.MasterCustomers, "Id", "Name", transactionHeader.CustomerId);
            return View(transactionHeader);
        }

        // POST: TransactionHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TransactionNumber,TransactionDate,CustomerId,DiscountPrice,TotalPrice,EstimationClear,Status,PaymentStatus,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] TransactionHeader transactionHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.MasterCustomers, "Id", "Name", transactionHeader.CustomerId);
            return View(transactionHeader);
        }

        // GET: TransactionHeaders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionHeader transactionHeader = db.TransactionHeaders.Find(id);
            if (transactionHeader == null)
            {
                return HttpNotFound();
            }
            return View(transactionHeader);
        }

        // POST: TransactionHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TransactionHeader transactionHeader = db.TransactionHeaders.Find(id);
            db.TransactionHeaders.Remove(transactionHeader);
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

        private SelectList ToSelectList(List<MasterCustomer> customers)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in customers)
            {
                list.Add(new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            }
            return new SelectList(list, "Value", "Text");
        }
    }
}
