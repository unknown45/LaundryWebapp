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
    
    public partial class MasterCustomer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MasterCustomer()
        {
            this.TransactionHeaders = new HashSet<TransactionHeader>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int TotalTransaction { get; set; }
        public Nullable<decimal> Quota { get; set; }
        public bool IsSubscribe { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> SubscribedQty { get; set; }
        public Nullable<System.DateTime> SubscribeFrom { get; set; }
        public Nullable<System.DateTime> SubscribeTo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionHeader> TransactionHeaders { get; set; }
    }
}