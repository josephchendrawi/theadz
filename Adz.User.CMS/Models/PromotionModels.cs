using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adz.User.CMS.Models
{
    public class PromotionModel
    {
        public class Promotion
        {
            public int PromotionId { get; set; }
            public string PromoCode { get; set; }
            public string Description { get; set; }
            public decimal Value { get; set; }
            public DateTime Create { get; set; }
            public string LastAction { get; set; }

            [Display(Name = "Start At")]
            public DateTime? StartAt { get; set; }
            [Display(Name = "End At")]
            public DateTime? EndAt { get; set; }
            [Display(Name = "On Schedule")]
            public bool OnScheduleFlag { get; set; }
        }
    }
}