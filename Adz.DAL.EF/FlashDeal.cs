//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Adz.DAL.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class FlashDeal
    {
        public int id { get; set; }
        public Nullable<int> inventoryitem_id { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
        public Nullable<bool> flag { get; set; }
        public string last_action { get; set; }
        public Nullable<bool> email_flag { get; set; }
    
        public virtual InventoryItem InventoryItem { get; set; }
    }
}
