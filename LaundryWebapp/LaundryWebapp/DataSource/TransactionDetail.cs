//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LaundryWebapp.DataSource
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransactionDetail
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public string ItemId { get; set; }
        public decimal Weight { get; set; }
        public int Price { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual MasterItem MasterItem { get; set; }
        public virtual TransactionHeader TransactionHeader { get; set; }
    }
}