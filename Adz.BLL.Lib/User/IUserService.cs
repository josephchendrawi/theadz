using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        Response<User> CheckLogin(UserLogin login);
        [OperationContract]
        Response<User> CreateUser(UserSignUp signup);
        [OperationContract]
        Response<List<User>> GetUserList();
        [OperationContract]
        User GetUserById(int id, DeviceInfo deviceinfo, bool? NewUser = false);
        [OperationContract]
        Response<List<UserStatus>> GetUserStatusList();
        [OperationContract]
        Response<bool> CreateEditUser(User User);
        [OperationContract]
        Response<string> ResetPasswordUser(string email);
        [OperationContract]
        Response<bool> ResetPasswordUserVerify(string secret, string token, string newpassword);
        [OperationContract]
        Response<int> GetUserIdByEmail(string email);
        [OperationContract]
        Response<int> UploadUserImage(string ImageBase64String, string ImageMimeType, int userid);
        Response<List<int>> UserPointUpdate();
        Response<List<KeyValuePair<int, DateTime>>> UserPointUpdatePastUsage();
        Response<int> UpdateReferralCode(string ReferralCode, string Email);
        Response<int> RedeemPromoCode(string PromoCode, string Email);

        void SendVerificationEmail(string SendToEmail, string VerificationLink);
        void SetGCMDeviceId(int UserId, string DeviceUniqueId, string GCMDeviceId);
        void PushNotificationToAllDevice(PNobject PNobject);
    }

    [DataContract]
    public class UserLogin
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public DeviceInfo DeviceInfo { get; set; }
        public bool OAuth { get; set; }
    }

    [DataContract]
    public class UserSignUp
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string ContactNumber { get; set; }
        [DataMember]
        public DeviceInfo DeviceInfo { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }
        [DataMember]
        public string ImageByteString { get; set; }
        [DataMember]
        public string ImageMimeType { get; set; }
        public string ReferredBy { get; set; }
        public string FacebookID { get; set; }

        public bool Facebook { get; set; }
        public bool Google { get; set; }
    }

    [DataContract]
    public class DeviceInfo
    {
        [DataMember]
        public string OS { get; set; }
        [DataMember]
        public string OS_Version { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string UniqueId { get; set; }
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }
        [DataMember]
        public int UserStatusId { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FacebookId { get; set; }
        [DataMember]
        public bool Notif { get; set; }
        [DataMember]
        public Boolean Facebook { get; set; }
        [DataMember]
        public Boolean Twitter { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string ApiKey { get; set; }
        [DataMember]
        public bool LastLockout { get; set; }
        [DataMember]
        public DateTime? LastLockoutDate { get; set; }
        [DataMember]
        public UserStatus UserStatus { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        [DataMember]
        public List<int> SubImageId { get; set; }
        [DataMember]
        public List<string> SubImageName { get; set; }
        [DataMember]
        public List<string> SubImageUrl { get; set; }
        [DataMember]
        public List<string> SubImageUrlLink { get; set; }
        public int PointBalance { get; set; }
        public string ReferralCode { get; set; }
        public string ReferredBy { get; set; }
        public int Debug { get; set; }
        [DataMember]
        public string ContactNumber { get; set; }
    }

    [DataContract]
    public class UserStatus
    {
        [DataMember]
        public int UserStatusId { get; set; }
        [DataMember]
        public string UserStatusName { get; set; }
    }
}
