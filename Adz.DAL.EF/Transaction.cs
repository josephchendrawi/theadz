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
    
    public partial class Transaction
    {
        public Transaction()
        {
            this.Billings = new HashSet<Billing>();
            this.GameTransactions = new HashSet<GameTransaction>();
            this.KPointTrxDs = new HashSet<KPointTrxD>();
            this.Shippings = new HashSet<Shipping>();
            this.TransactionDetails = new HashSet<TransactionDetail>();
            this.Winners = new HashSet<Winner>();
        }
    
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> promotion_id { get; set; }
        public Nullable<decimal> kpoint { get; set; }
        public Nullable<System.DateTime> transaction_date { get; set; }
        public Nullable<int> transaction_status { get; set; }
        public Nullable<System.DateTime> process_date { get; set; }
        public Nullable<bool> participate_game { get; set; }
        public string last_action { get; set; }
        public string tranID { get; set; }
        public string orderid { get; set; }
        public string status { get; set; }
        public string domain { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string currency { get; set; }
        public string appcode { get; set; }
        public Nullable<System.DateTime> paydate { get; set; }
        public string skey { get; set; }
        public string channel { get; set; }
        public string error_code { get; set; }
        public string error_desc { get; set; }
        public Nullable<int> process_status { get; set; }
    
        public virtual ICollection<Billing> Billings { get; set; }
        public virtual ICollection<GameTransaction> GameTransactions { get; set; }
        public virtual ICollection<KPointTrxD> KPointTrxDs { get; set; }
        public virtual ICollection<Shipping> Shippings { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Winner> Winners { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
