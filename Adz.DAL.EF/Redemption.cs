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
    
    public partial class Redemption
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> reward_id { get; set; }
        public string name { get; set; }
        public string bank_name { get; set; }
        public string bank_account_num { get; set; }
        public string bank_account_name { get; set; }
        public Nullable<int> mobile_operator_id { get; set; }
        public string mobile_acc_num { get; set; }
        public Nullable<int> redemption_status_id { get; set; }
        public Nullable<System.DateTime> redemption_date { get; set; }
        public Nullable<int> reward_type_id { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string postcode { get; set; }
        public Nullable<int> reward_point_requirement { get; set; }
        public string reward_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    
        public virtual MobileOperator MobileOperator { get; set; }
        public virtual RedemptionStatu RedemptionStatu { get; set; }
        public virtual Reward Reward { get; set; }
        public virtual User User { get; set; }
        public virtual Review Review { get; set; }
    }
}
