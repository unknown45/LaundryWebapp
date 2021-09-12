using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaundryWebapp.DataSource;
using LaundryWebapp.ViewModels;
using Microsoft.Ajax.Utilities;
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
            ViewBag.ItemId = new SelectList(db.MasterItems.Where(x => x.IsSubscribe == true), "Id", "Name");
            return View();
        }

        // POST: MasterCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Address,Phone,SubscribedQty,IsSubscribe,ItemId")] CustomerViewModel masterCustomer)
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
                MasterCustomerItem customerItem = new MasterCustomerItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ItemId = masterCustomer.ItemId,
                    CustomerId = masterCustomer.Id,
                    CreatedDate = DateTime.Now,
                    CreatedBy = User.Identity.GetUserName(),
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = User.Identity.GetUserName(),
                    IsActive = true
                };
                db.MasterCustomers.Add(masterCustomer as MasterCustomer);
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
            var ItemList = new SelectList(db.MasterItems.Where(x => x.IsSubscribe == true), "Id", "Name");
            MasterCustomerItem customerItem = db.MasterCustomerItems.FirstOrDefault(x => x.CustomerId == id);
            CustomerViewModel result = new CustomerViewModel()
            {
                Id = masterCustomer.Id,
                Name = masterCustomer.Name,
                Address = masterCustomer.Address,
                Phone = masterCustomer.Phone,
                TotalTransaction = masterCustomer.TotalTransaction,
                IsSubscribe = masterCustomer.IsSubscribe,
                Quota = masterCustomer.Quota,
                SubscribedQty = masterCustomer.SubscribedQty,
                SubscribeFrom = masterCustomer.SubscribeFrom,
                SubscribeTo = masterCustomer.SubscribeTo,
                ItemId = customerItem?.ItemId
            };
            ItemList.ForEach(x =>
            {
                if (x.Value == result.ItemId)
                    x.Selected = true;
            });
            ViewBag.ItemId = ItemList;
            return View(result);
        }

        // POST: MasterCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,Phone,SubscribedQty,IsSubscribe,ItemId")] CustomerViewModel masterCustomer)
        {
            if (ModelState.IsValid)
            {
                var currentData = db.MasterCustomers.FirstOrDefault(x => x.Id == masterCustomer.Id);
                var currentDataCustItem = db.MasterCustomerItems.FirstOrDefault(x => x.CustomerId == masterCustomer.Id);
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

                if (currentDataCustItem == null)
                {
                    currentDataCustItem = new MasterCustomerItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerId = masterCustomer.Id,
                        ItemId = masterCustomer.ItemId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = User.Identity.GetUserName(),
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = User.Identity.GetUserName(),
                        IsActive = true
                    };
                    db.MasterCustomerItems.Add(currentDataCustItem);
                }
                else
                {
                    currentDataCustItem.ItemId = masterCustomer.ItemId;
                    db.Entry(currentDataCustItem).State = EntityState.Modified;
                }

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
