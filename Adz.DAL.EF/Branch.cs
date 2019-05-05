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
    
    public partial class Branch
    {
        public int id { get; set; }
        public Nullable<int> merchant_id { get; set; }
        public Nullable<double> longitude { get; set; }
        public Nullable<double> latitude { get; set; }
        public string last_action { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public Nullable<int> city_id { get; set; }
        public string postcode { get; set; }
        public Nullable<System.DateTime> last_created { get; set; }
        public Nullable<System.DateTime> last_updated { get; set; }
    
        public virtual Merchant Merchant { get; set; }
        public virtual City City { get; set; }
    }
}
