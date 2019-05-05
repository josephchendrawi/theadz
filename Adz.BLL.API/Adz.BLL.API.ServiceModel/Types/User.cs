using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel.Types
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int UserStatus { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        //public string ApiKey { get; set; }
        public int PointBalance { get; set; }
        public string ReferralCode { get; set; }
        public int Debug { get; set; }
        public bool? FirstTime { get; set; }
    }

    public class UserData
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Debug { get; set; }
        public string APIKey { get; set; }
        public string UniqueDeviceId { get; set; }

    }
}
