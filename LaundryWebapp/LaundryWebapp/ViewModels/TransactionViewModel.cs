using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaundryWebapp.ViewModels
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CustomerId { get; set; }
        public int DiscountPrice { get; set; }
        public int TotalPrice { get; set; }
        public DateTime EstimationClear { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentId { get; set; }
    }
}