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
    public class MasterCustomersController : Controller
    {
        private LaundryEntities db = new LaundryEntities();

        // GET: MasterCustomers
        public ActionResult Index()
        {
            return View(db.MasterCustomers.ToList());
        }

        // GET: MasterCustomers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterCustomer masterCustomer = db.MasterCustomers.Find(id);
            if (masterCustomer == null)
            {
                return HttpNotFound();
            }
            return View(masterCustomer);
        }

        // GET: MasterCustomers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MasterCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Address,Phone,SubscribedQty,IsSubscribe,")] MasterCustomer masterCustomer)
        {
            if (ModelState.IsValid)
            {
                masterCustomer.Id = Guid.NewGuid().ToString();
                masterCustomer.TotalTransaction = 0;
                if (masterCustomer.IsSubscribe && masterCustomer.SubscribedQty > 0)
                {
                    masterCustomer.Quota = masterCustomer.SubscribedQty * 50;
                    masterCustomer.SubscribeFrom = DateTime.Now;
                    masterCustomer.SubscribeTo = DateTime.Now.AddMonths(masterCustomer.SubscribedQty.Value);
                }
                masterCustomer.CreatedDate = DateTime.Now;
                masterCustomer.CreatedBy = User.Identity.GetUserName();
                masterCustomer.ModifiedDate = DateTime.Now;
                masterCustomer.ModifiedBy = User.Identity.GetUserName();
                masterCustomer.IsActive = true;
                db.MasterCustomers.Add(masterCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(masterCustomer);
        }

        // GET: MasterCustomers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterCustomer masterCustomer = db.MasterCustomers.Find(id);
            if (masterCustomer == null)
            {
                return HttpNotFound();
            }
            return View(masterCustomer);
        }

        // POST: MasterCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,Phone,SubscribedQty,IsSubscribe")] MasterCustomer masterCustomer)
        {
            if (ModelState.IsValid)
            {
                var currentData = db.MasterCustomers.FirstOrDefault(x => x.Id == masterCustomer.Id);
                currentData.Address = masterCustomer.Address;
                currentData.Phone = masterCustomer.Phone;
                currentData.SubscribedQty += masterCustomer.SubscribedQty;
                currentData.SubscribedQty = !masterCustomer.IsSubscribe ? null : currentData.SubscribedQty;
                currentData.SubscribeFrom = !masterCustomer.IsSubscribe ? null : currentData.SubscribeFrom;
                currentData.SubscribeTo = !masterCustomer.IsSubscribe ? null : currentData.SubscribeTo;
                currentData.Quota = !masterCustomer.IsSubscribe ? null : currentData.Quota;
                if (masterCustomer.IsSubscribe)
                {
                    currentData.SubscribeFrom = currentData.IsSubscribe != masterCustomer.IsSubscribe ? DateTime.Now : currentData.SubscribeFrom;
                    currentData.SubscribeTo = currentData.IsSubscribe != masterCustomer.IsSubscribe ? DateTime.Now.AddMonths(masterCustomer.SubscribedQty.Value) : currentData.SubscribeFrom.Value.AddMonths(currentData.SubscribedQty.Value);
                    currentData.Quota = currentData.IsSubscribe != masterCustomer.IsSubscribe ? 50 : currentData.Quota + (50 * currentData.SubscribedQty);
                }
                currentData.IsSubscribe = masterCustomer.IsSubscribe;
                currentData.ModifiedDate = DateTime.Now;
                currentData.ModifiedBy = User.Identity.GetUserName();
                db.Entry(currentData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(masterCustomer);
        }

        // GET: MasterCustomers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterCustomer masterCustomer = db.MasterCustomers.Find(id);
            if (masterCustomer == null)
            {
                return HttpNotFound();
            }
            return View(masterCustomer);
        }

        // POST: MasterCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MasterCustomer masterCustomer = db.MasterCustomers.Find(id);
            db.MasterCustomers.Remove(masterCustomer);
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
