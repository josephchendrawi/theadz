using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adz.User.CMS.Models
{
    public class AdminLoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class AdminModel
    {
        public class Admin
        {
            public int AdminId { get; set; }
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Display(Name = "LastName")]
            public string LastName { get; set; }
            [Required]
            [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid Email.")]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [UIHint("_RoleDropDown")]
            [Display(Name = "Role")]
            public string RoleName { get; set; }
            public int RoleId { get; set; }
            public string LastAction { get; set; }
        }

        public class AdminRole
        {
            public int RoleId { get; set; }
            [Required]
            [Display(Name = "Role Name")]
            public string RoleName { get; set; }
        }

        public class AdminModule
        {
            public int ModuleId { get; set; }
            [Display(Name = "Module Name")]
            public string ModuleName { get; set; }
        }

        public class AdminAccess
        {
            public int AccessId { get; set; }
            public int ModuleId { get; set; }
            public int RoleId { get; set; }
            [Display(Name = "Module")]
            public string ModuleName { get; set; }
            [Display(Name = "Role")]
            public string RoleName { get; set; }
            [Display(Name = "Viewable")]
            public bool is_viewable { get; set; }
            [Display(Name = "Editable")]
            public bool is_editable { get; set; }
            [Display(Name = "Addable")]
            public bool is_addable { get; set; }
            [Display(Name = "Deleteable")]
            public bool is_deleteable { get; set; }
        }
    }
}