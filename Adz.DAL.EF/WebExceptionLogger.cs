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
    
    public partial class WebExceptionLogger
    {
        public int id { get; set; }
        public Nullable<System.DateTime> last_created { get; set; }
        public string url { get; set; }
        public string ip_address { get; set; }
        public string logger { get; set; }
        public string err_exception { get; set; }
        public string err_message { get; set; }
    }
}
