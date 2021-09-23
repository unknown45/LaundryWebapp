using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaundryWebapp.ViewModels
{
    public class ItemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string CustomerId { get; set; }
    }
}