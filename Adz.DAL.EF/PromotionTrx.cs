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
    
    public partial class PromotionTrx
    {
        public int promo_id { get; set; }
        public int user_id { get; set; }
        public Nullable<System.DateTime> last_created { get; set; }
        public string remarks { get; set; }
    
        public virtual User User { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
