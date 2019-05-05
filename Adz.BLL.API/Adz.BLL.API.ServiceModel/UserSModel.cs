using Adz.BLL.API.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel
{
    [Route("/user/login", "POST")]
    public class Login : IReturn<UserResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public string GCMDeviceId { get; set; }
    }

    [Route("/user/loginfb", "POST")]
    public class LoginFacebook : IReturn<UserResponse>
    {
        public string AccessToken { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public string GCMDeviceId { get; set; }
        //public string ReferralCode { get; set; }
    }

    [Route("/user/login/google", "POST")]
    public class LoginGoogle : IReturn<UserResponse>
    {
        public string AccessToken { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public string GCMDeviceId { get; set; }
    }

    [Route("/user/referral", "POST")]
    public class UserReferralCode : IReturn<IntResponse>
    {
        public string ReferralCode { get; set; }
    }

    [Route("/user/promocode", "POST")]
    public class UserPromoCode : IReturn<IntResponse>
    {
        public string PromoCode { get; set; }
    }

    [Route("/user/logout")]
    public class LogOut : IReturn<CustomResponse>
    {
    }

    [Route("/user/signup", "POST")]
    public class SignUp : IReturn<UserResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public string GCMDeviceId { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageByteString { get; set; }
        public string ImageMimeType { get; set; }
        public string ReferralCode { get; set; }
        public string ContactNumber { get; set; }
    }

    [Route("/user/resetpassword", "POST")]
    public class ResetPassword : IReturn<CustomResponse>
    {
        public string Email { get; set; }
    }

    [Route("/user/resetpasswordverify", "POST")]
    public class ResetPasswordVerify : IReturn<CustomResponse>
    {
        public string uSecret { get; set; }
        public string uToken { get; set; }
    }

    [Route("/user/get")]
    public class UserGetCurrent : IReturn<UserResponse>
    {
    }
    
    [Route("/user/point")]
    public class UserPointBalance : IReturn<UserResponse>
    {
    }

    [Route("/user/update", "POST")]
    public class UserUpdate : IReturn<UserResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageByteString { get; set; }
        public string ImageMimeType { get; set; }
        public string NewEmail { get; set; }
        public string ContactNumber { get; set; }
    }

    [Route("/user/update/gcm_deviceid", "POST")]
    public class UserSetGCMDeviceId : IReturn<BoolResponse>
    {
        public string GCMDeviceId { get; set; }
    }

    [Route("/user/pointhistory")]
    public class UserGetPointHistory : IReturn<UserTrxResponse>
    {
    }

    public class IntResponse
    {
        public int Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

    public class BoolResponse
    {
        public bool Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

    public class UserResponse
    {
        public User Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

    public class UserTrxResponse
    {
        public List<UserTrx> Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

}
