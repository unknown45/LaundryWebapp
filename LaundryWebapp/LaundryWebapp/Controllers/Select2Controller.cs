using LaundryWebapp.DataSource;
using LaundryWebapp.Enum;
using LaundryWebapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaundryWebapp.Controllers
{
    public class Select2Controller : Controller
    {
        private LaundryEntities db = new LaundryEntities();
        // GET: Select2
        public JsonResult GetSelectSubscribeItem()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.MasterItems.Where(x => x.IsSubscribe).Select(x => new Select2ViewModel()
            {
                id = x.Id,
                text = x.Name
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSelectItem()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.MasterItems.Select(x => new Select2ViewModel()
            {
                id = x.Id,
                text = x.Name
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSelectCustomer()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.MasterCustomers.Select(x => new Select2ViewModel()
            {
                id = x.Id,
                text = x.Name
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSelectPayment()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.MasterPayments.Select(x => new Select2ViewModel()
            {
                id = x.Id,
                text = x.Description
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectStatus()
        {
            var data = new List<Select2ViewModel>() {
                new Select2ViewModel(){ id = Enum.Enum.Status.Process.ToString(), text = "Sedang di Cuci" },
                new Select2ViewModel(){ id = Enum.Enum.Status.Done.ToString(), text = "Selesai Cuci" },
                new Select2ViewModel(){ id = Enum.Enum.Status.Complete.ToString(), text = "Selesai Diambil" }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectPaymentStatus()
        {
            var data = new List<Select2ViewModel>() {
                new Select2ViewModel(){ id = Enum.Enum.PaymentStatus.Paid.ToString(), text = "Sudah Lunas" },
                new Select2ViewModel(){ id = Enum.Enum.PaymentStatus.NotPaid.ToString(), text = "Belum Lunas" }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}