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
    
    public partial class Variance
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<int> inventoryitem_id { get; set; }
        public Nullable<decimal> margin { get; set; }
        public Nullable<int> available_limit { get; set; }
        public string sku { get; set; }
    
        public virtual InventoryItem InventoryItem { get; set; }
    }
}