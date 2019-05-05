using Adz.BLL.API.ServiceModel.Types;
using Adz.BLL.Lib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceInterface
{
    public static class Helper
    {
        public const string APIKeyName = "Key";

        public static int defaultGMT
        {
            get
            {
                int gmt = int.Parse(ConfigurationManager.AppSettings["GMT"]);
                return gmt;
            }
        }

        //public static UserData TempUserData = new TempUserData();
        public static UserData GetUserDataByKey(string Key)
        {
            UserData UserData = new UserData();

            var decoded = Helper.DecodeFrom64(Key);

            var user = General.GetUserData(decoded[1], decoded[2]);
            UserData.UserId = user.UserId;
            UserData.FirstName = user.FirstName;
            UserData.LastName = user.LastName;
            UserData.Email = user.Email;
            UserData.Gender = user.Gender;
            UserData.DateOfBirth = user.DateOfBirth;
            UserData.APIKey = decoded[0];
            UserData.UniqueDeviceId = decoded[2];
            UserData.Debug = user.Debug;

            return UserData;
        }

        public static JObject FacebookUserInfo(string AccessToken)
        {
            try
            {
                WebClient client = new WebClient();
                string JsonResult = client.DownloadString(string.Concat(
                       "https://graph.facebook.com/me?access_token=", AccessToken));

                JObject jsonUserInfo = JObject.Parse(JsonResult);

                return jsonUserInfo;
            }
            catch (Exception e)
            {
                throw new CustomException(CustomErrorType.UserServiceFacebookError, "Facebook exception.", e);
            }
        }

        public static JObject GetGoogleUserInfo(string AccessToken)
        {
            try
            {
                WebClient client = new WebClient();
                string JsonResult = client.DownloadString(string.Concat(
                       "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=", AccessToken));

                JObject jsonUserInfo = JObject.Parse(JsonResult);

                return jsonUserInfo;
            }
            catch (Exception e)
            {
                throw new CustomException(CustomErrorType.UserServiceGoogleNotFound);
            }
        }

        public static string EncodeTo64(string[] StringsToEncode)
        {
            string toEncode = "";
            foreach (var i in StringsToEncode)
            {
                toEncode = toEncode + i + "|";
            }
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string[] DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string decoded =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            string[] returnValue = decoded.Split('|');

            return returnValue;
        }
    }
}
