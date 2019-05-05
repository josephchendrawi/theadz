using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adz.User.CMS.Models
{
    public class PushNotificationModel
    {
        [Required]
        public string Message { get; set; }
    }
}