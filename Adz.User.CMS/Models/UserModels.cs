using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adz.User.CMS.Models
{
    public class UserModel
    {
        public class User
        {
            [ScaffoldColumn(false)]
            public int UserId { get; set; }
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Email")]
            [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid Email")]
            public string Email { get; set; }
            [Required]
            [Display(Name = "Gender")]
            public int Gender { get; set; }
            [Display(Name = "Gender")]
            public string GenderStr { get; set; }
            [Display(Name = "Date Of Birth")]
            public DateTime DateOfBirth { get; set; }
            public bool LastLockout { get; set; }
            public DateTime? LastLockoutDate { get; set; }
            [Required(ErrorMessage = "*")]
            [Display(Name = "Status")]
            public UserStatus UserStatus { get; set; }
            //public Facebook Facebook { get; set; }
            public DateTime Create { get; set; }
            public string Password { get; set; }
            public string SubImageId { get; set; }
            public string SubImageName { get; set; }
            [Display(Name = "Image")]
            public string SubImageUrl { get; set; }
            public string SubImageUrlLink { get; set; }
            [Display(Name = "Point Balance")]
            [Range(0, int.MaxValue, ErrorMessage = "Point Balance must be a positive!")]
            public int PointBalance { get; set; }
            [Display(Name = "Referral Code")]
            public string ReferralCode { get; set; }
            [Display(Name = "Referred By")]
            public string ReferredBy { get; set; }

            [Display(Name = "Age")]
            public int Age { get; set; }
            [Display(Name = "Phone Number")]
            public string ContactNumber { get; set; }
        }
        public sealed class UserStatus
        {
            public int UserStatusId { get; set; }
            public string UserStatusName { get; set; }
        }
        
        public class UserReport
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public string ContactNumber { get; set; }
            public string DateOfBirth { get; set; }
            public string Status { get; set; }
            public DateTime Create { get; set; }
            public int PointBalance { get; set; }
            public string ReferralCode { get; set; }
            public string ReferredBy { get; set; }
        }
    }
}