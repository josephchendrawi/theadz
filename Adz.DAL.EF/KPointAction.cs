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
    
    public partial class KPointAction
    {
        public int id { get; set; }
        public Nullable<int> code { get; set; }
        public string description { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<System.DateTime> last_updated { get; set; }
        public string last_action { get; set; }
    }
}
