using LaundryWebapp.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaundryWebapp.ViewModels
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public MasterCustomer Customer { get; set; }
        public int DiscountPrice { get; set; }
        public int TotalPrice { get; set; }
        public int EstimationClear { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public List<MasterItem> Items { get; set; }
        public decimal Weight { get; set; }
        public int Price { get; set; }
    }
}