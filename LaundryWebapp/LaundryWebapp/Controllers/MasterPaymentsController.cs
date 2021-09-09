using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaundryWebapp.DataSource;
using Microsoft.AspNet.Identity;

namespace LaundryWebapp.Controllers
{
    [Authorize]
    public class MasterPaymentsController : Controller
    {
        private LaundryEntities db = new LaundryEntities();

        // GET: MasterPayments
        public ActionResult Index()
        {
            return View(db.MasterPayments.ToList());
        }

        // GET: MasterPayments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterPayment masterPayment = db.MasterPayments.Find(id);
            if (masterPayment == null)
            {
                return HttpNotFound();
            }
            return View(masterPayment);
        }

        // GET: MasterPayments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MasterPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description")] MasterPayment masterPayment)
        {
            if (ModelState.IsValid)
            {
                int counter = db.MasterPayments.Count();
                masterPayment.Id = Guid.NewGuid().ToString();
                masterPayment.Code = $"PYM-{(counter + 1)}";
                masterPayment.CreatedDate = DateTime.Now;
                masterPayment.CreaetedBy = User.Identity.GetUserName();
                masterPayment.ModifiedDate = DateTime.Now;
                masterPayment.ModifiedBy = User.Identity.GetUserName();
                masterPayment.IsActive = true;
                db.MasterPayments.Add(masterPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(masterPayment);
        }

        // GET: MasterPayments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterPayment masterPayment = db.MasterPayments.Find(id);
            if (masterPayment == null)
            {
                return HttpNotFound();
            }
            return View(masterPayment);
        }

        // POST: MasterPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsActive")] MasterPayment masterPayment)
        {
            if (ModelState.IsValid)
            {
                masterPayment.ModifiedDate = DateTime.Now;
                masterPayment.ModifiedBy = User.Identity.GetUserName();
                db.Entry(masterPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(masterPayment);
        }

        // GET: MasterPayments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterPayment masterPayment = db.MasterPayments.Find(id);
            if (masterPayment == null)
            {
                return HttpNotFound();
            }
            return View(masterPayment);
        }

        // POST: MasterPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MasterPayment masterPayment = db.MasterPayments.Find(id);
            db.MasterPayments.Remove(masterPayment);
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
