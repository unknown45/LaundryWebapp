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
    public class MasterItemsController : Controller
    {
        private LaundryEntities db = new LaundryEntities();

        // GET: MasterItems
        public ActionResult Index()
        {
            return View(db.MasterItems.OrderBy(x => x.Price).ToList());
        }

        // GET: MasterItems/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterItem masterItem = db.MasterItems.Find(id);
            if (masterItem == null)
            {
                return HttpNotFound();
            }
            return View(masterItem);
        }

        // GET: MasterItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MasterItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Duration,Price,IsSubscribe")] MasterItem masterItem)
        {
            if (ModelState.IsValid)
            {
                int counter = db.MasterItems.Count();
                masterItem.Id = Guid.NewGuid().ToString();
                masterItem.Code = $"ITM-{counter + 1}";
                masterItem.CreatedDate = DateTime.Now;
                masterItem.CreatedBy = User.Identity.GetUserName();
                masterItem.ModifiedDate = DateTime.Now;
                masterItem.ModifiedBy = User.Identity.GetUserName();
                db.MasterItems.Add(masterItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(masterItem);
        }

        // GET: MasterItems/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterItem masterItem = db.MasterItems.Find(id);
            if (masterItem == null)
            {
                return HttpNotFound();
            }
            return View(masterItem);
        }

        // POST: MasterItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Duration,Price,IsSubscribe")] MasterItem masterItem)
        {
            if (ModelState.IsValid)
            {
                var currentData = db.MasterItems.FirstOrDefault(x => x.Id == masterItem.Id);
                currentData.Name = masterItem.Name;
                currentData.Description = masterItem.Description;
                currentData.Duration = masterItem.Duration;
                currentData.Price = masterItem.Price;
                currentData.IsSubscribe = masterItem.IsSubscribe;
                currentData.ModifiedDate = DateTime.Now;
                currentData.ModifiedBy = User.Identity.GetUserName();
                db.Entry(currentData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(masterItem);
        }

        // GET: MasterItems/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterItem masterItem = db.MasterItems.Find(id);
            if (masterItem == null)
            {
                return HttpNotFound();
            }
            return View(masterItem);
        }

        // POST: MasterItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MasterItem masterItem = db.MasterItems.Find(id);
            db.MasterItems.Remove(masterItem);
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
