using LaundryWebapp.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaundryWebapp.ViewModels
{
    public class CustomerViewModel : MasterCustomer
    {
        public string ItemId { get; set; }
    }
}