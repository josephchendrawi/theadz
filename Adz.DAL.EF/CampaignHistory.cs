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
    
    public partial class CampaignHistory
    {
        public int id { get; set; }
        public Nullable<int> campaign_id { get; set; }
        public Nullable<System.DateTime> view_date { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<double> longitude { get; set; }
        public Nullable<double> latitude { get; set; }
        public Nullable<int> gender { get; set; }
        public Nullable<bool> flag_processed { get; set; }
        public Nullable<int> action { get; set; }
        public Nullable<System.DateTime> last_updated { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public string remarks { get; set; }
        public string timezone { get; set; }
    
        public virtual Campaign Campaign { get; set; }
        public virtual User User { get; set; }
    }
}