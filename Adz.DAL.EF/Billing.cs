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
    
    public partial class Billing
    {
        public int id { get; set; }
        public Nullable<int> transaction_id { get; set; }
        public Nullable<int> payment_method { get; set; }
        public string payment_cc { get; set; }
        public string payment_ccv { get; set; }
        public Nullable<int> payment_expm { get; set; }
        public Nullable<int> payment_expy { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public Nullable<int> gender { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string last_action { get; set; }
    
        public virtual Transaction Transaction { get; set; }
    }
}
