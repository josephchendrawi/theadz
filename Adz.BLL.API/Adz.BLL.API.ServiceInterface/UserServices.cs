using Adz.BLL.API.ServiceModel;
using Adz.BLL.API.ServiceModel.Types;
using Adz.BLL.Lib;
using ServiceStack;
using ServiceStack.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceInterface
{
    public class UserServices : Service
    {
        public IUserService userservice = new UserService();
        public IUserTrxService usertrxservice = new UserTrxService();

        public UserResponse Post(Login request)
        {
            try
            {
                UserLogin login = new UserLogin();
                login.Username = request.Username;
                login.Password = request.Password;
                login.DeviceInfo = new Lib.DeviceInfo()
                {
                    UniqueId = request.DeviceInfo.UniqueId,
                    OS = request.DeviceInfo.OS,
                    OS_Version = request.DeviceInfo.OS_Version,
                    Model = request.DeviceInfo.Model
                };

                var result_ = userservice.CheckLogin(login).Result;
                if (result_ != null)
                {
                    //update gcm device id
                    if (!string.IsNullOrWhiteSpace(request.GCMDeviceId))
                    {
                        userservice.SetGCMDeviceId(result_.UserId, request.DeviceInfo.UniqueId, request.GCMDeviceId);
                    }

                    Adz.BLL.API.ServiceModel.Types.User result = new Adz.BLL.API.ServiceModel.Types.User();

                    result.UserId = result_.UserId;
                    result.FirstName = result_.FirstName;
                    result.LastName = result_.LastName;
                    result.Email = result_.Email;
                    result.Gender = result_.Gender;
                    if (result_.DateOfBirth == DateTime.MinValue)
                        result.DateOfBirth = null;
                    else
                        result.DateOfBirth = result_.DateOfBirth;
                    result.UserStatus = result_.UserStatusId;
                    result.Password = result_.Password;
                    result.ImageUrl = result_.ImageUrl;
                    result.PointBalance = result_.PointBalance;
                    result.ReferralCode = result_.ReferralCode;
                    result.Debug = result_.Debug;
                    //result.ApiKey = result_.ApiKey;
                    //result.ApiKey = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId });

                    /*
                    //AuthUserSession session = (AuthUserSession)this.GetSession();
                    session.IsAuthenticated = true;
                    Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId = login.DeviceInfo.UniqueId;

                    session.FirstName = result.FirstName;
                    session.LastName = result.LastName;
                    Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email = result.Email;
                    session.Gender = result.Gender.ToString();
                    session.BirthDate = result.DateOfBirth;

                    //Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey = result_.ApiKey;

                    this.SaveSession(session, new TimeSpan(TimeSpan.TicksPerHour * int.Parse(ConfigurationManager.AppSettings["sessiontimeout_hour"])));
                    */

                    return new UserResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        },
                        Result = result,
                        Key = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId }),
                        Debug = result.Debug
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new UserResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public CustomResponse Any(LogOut request)
        {
            try
            {
                ///log out process

                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    }
                };
            }
            catch (CustomException ex)
            {
                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    }
                };
            }
        }

        public UserResponse Post(SignUp request)
        {
            try
            {
                UserSignUp signup = new UserSignUp();
                signup.Email = request.Email;
                signup.FirstName = request.FirstName;
                signup.LastName = request.LastName;
                signup.Password = request.Password;
                signup.DeviceInfo = new Lib.DeviceInfo()
                {
                    UniqueId = request.DeviceInfo.UniqueId,
                    OS = request.DeviceInfo.OS,
                    OS_Version = request.DeviceInfo.OS_Version,
                    Model = request.DeviceInfo.Model
                };
                signup.Gender = request.Gender;
                signup.ContactNumber = request.ContactNumber;
                signup.DateOfBirth = request.DateOfBirth;
                signup.ImageByteString = request.ImageByteString;
                signup.ImageMimeType = request.ImageMimeType;
                signup.ReferredBy = request.ReferralCode;

                var result_ = userservice.CreateUser(signup).Result;
                if (result_ != null)
                {
                    //update gcm device id
                    if (!string.IsNullOrWhiteSpace(request.GCMDeviceId))
                    {
                        userservice.SetGCMDeviceId(result_.UserId, request.DeviceInfo.UniqueId, request.GCMDeviceId);
                    }

                    Adz.BLL.API.ServiceModel.Types.User result = new Adz.BLL.API.ServiceModel.Types.User();

                    result.UserId = result_.UserId;
                    result.FirstName = result_.FirstName;
                    result.LastName = result_.LastName;
                    result.Email = result_.Email;
                    result.Gender = result_.Gender;
                    if (result_.DateOfBirth == DateTime.MinValue)
                        result.DateOfBirth = null;
                    else
                        result.DateOfBirth = result_.DateOfBirth;
                    result.UserStatus = result_.UserStatusId;
                    result.Password = result_.Password;
                    result.ImageUrl = result_.ImageUrl;
                    result.PointBalance = result_.PointBalance;
                    result.ReferralCode = result_.ReferralCode;
                    result.Debug = result_.Debug;
                    //result.ApiKey = result_.ApiKey;
                    //result.ApiKey = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId });

                    string msg = null;
                    if (request.ReferralCode != null && request.ReferralCode != "" && result_.ReferredBy == null)
                    {
                        msg = "Referral Code not found!";
                    }

                    return new UserResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = msg
                        },
                        Result = result,
                        Key = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId }),
                        Debug = result.Debug
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new UserResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public UserResponse Post(LoginFacebook request)
        {
            try
            {
                //get accesstoken (testing) https://www.facebook.com/dialog/oauth?client_id=903215866406532&redirect_uri=http://localhost:24217/&response_type=token&scope=email,public_profile,user_birthday

                Newtonsoft.Json.Linq.JObject UserInfo = Helper.FacebookUserInfo(request.AccessToken);
                Adz.BLL.Lib.DeviceInfo deviceinfo = new Adz.BLL.Lib.DeviceInfo()
                {
                    UniqueId = request.DeviceInfo.UniqueId,
                    OS = request.DeviceInfo.OS,
                    OS_Version = request.DeviceInfo.OS_Version,
                    Model = request.DeviceInfo.Model
                };

                string FacebookId = UserInfo.Value<string>("id") != null ? UserInfo.Value<string>("id") : String.Empty;
                string FirstName = UserInfo.Value<string>("first_name") != null ? UserInfo.Value<string>("first_name") : String.Empty;
                string LastName = UserInfo.Value<string>("last_name") != null ? UserInfo.Value<string>("last_name") : String.Empty;
                string Email = UserInfo.Value<string>("email");
                int Gender = UserInfo.Value<string>("gender") != null ? (UserInfo.Value<string>("gender") == "male" ? 1 : 2) : 0;
                DateTime DateOfBirth = UserInfo.Value<DateTime>("birthday");

                if (Email == null)
                {
                    throw new Exception("Your Facebook account has not been verified, kindly verify it and sign up again.");
                }

                UserLogin login = new UserLogin();
                login.Username = Email;
                login.Password = "";
                login.DeviceInfo = deviceinfo;
                login.OAuth = true;

                bool FirstTime = false;

                Adz.BLL.Lib.User result_ = null;

                try
                {
                    result_ = userservice.CheckLogin(login).Result;
                    //request.ReferralCode = null;
                }
                catch (CustomException e)
                {
                    if (e.ErrorType == CustomErrorType.UserServiceInvalidUsernamePassword)
                    {
                        UserSignUp signup = new UserSignUp();
                        signup.Email = Email;
                        signup.FirstName = FirstName;
                        signup.LastName = LastName;
                        signup.Password = "";
                        signup.DeviceInfo = deviceinfo;
                        signup.Gender = Gender;
                        signup.DateOfBirth = DateOfBirth;
                        signup.ContactNumber = "";
                        //signup.ImageByteString = ImageByteString;
                        //signup.ImageMimeType = ImageMimeType;
                        signup.Facebook = true;
                        signup.FacebookID = FacebookId;
                        //signup.ReferredBy = request.ReferralCode;

                        result_ = userservice.CreateUser(signup).Result;
                        FirstTime = true;
                    }
                }

                if (result_ != null)
                {
                    //update gcm device id
                    if (!string.IsNullOrWhiteSpace(request.GCMDeviceId))
                    {
                        userservice.SetGCMDeviceId(result_.UserId, request.DeviceInfo.UniqueId, request.GCMDeviceId);
                    }

                    Adz.BLL.API.ServiceModel.Types.User result = new Adz.BLL.API.ServiceModel.Types.User();

                    result.UserId = result_.UserId;
                    result.FirstName = result_.FirstName;
                    result.LastName = result_.LastName;
                    result.Email = result_.Email;
                    result.Gender = result_.Gender;
                    if (result_.DateOfBirth == DateTime.MinValue)
                        result.DateOfBirth = null;
                    else
                        result.DateOfBirth = result_.DateOfBirth;
                    result.UserStatus = result_.UserStatusId;
                    result.Password = result_.Password;
                    result.ImageUrl = result_.ImageUrl;
                    result.PointBalance = result_.PointBalance;
                    result.ReferralCode = result_.ReferralCode;
                    result.Debug = result_.Debug;
                    result.FirstTime = FirstTime;
                    //result.ApiKey = result_.ApiKey;
                    //result.ApiKey = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId });

                    /*
                    //AuthUserSession session = (AuthUserSession)this.GetSession();
                    session.IsAuthenticated = true;
                    Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId = login.DeviceInfo.UniqueId;

                    session.FirstName = result.FirstName;
                    session.LastName = result.LastName;
                    Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email = result.Email;
                    session.Gender = result.Gender.ToString();
                    session.BirthDate = result.DateOfBirth;

                    //Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey = result_.ApiKey;

                    this.SaveSession(session, new TimeSpan(TimeSpan.TicksPerHour * int.Parse(ConfigurationManager.AppSettings["sessiontimeout_hour"])));
                    */

                    string msg = null;
                    //if (request.ReferralCode != null && request.ReferralCode != "" && result_.ReferredBy == null)
                    //{
                    //    msg = "Referral Code not found!";
                    //}

                    return new UserResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = msg
                        },
                        Result = result,
                        Key = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId }),
                        Debug = result.Debug
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new UserResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public UserResponse Post(LoginGoogle request)
        {
            try
            {
                //https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=
                Newtonsoft.Json.Linq.JObject UserInfo = Helper.GetGoogleUserInfo(request.AccessToken);

                string UserInfo_email = UserInfo.Value<string>("email") ?? "";
                string UserInfo_name = UserInfo.Value<string>("name") ?? "";
                string UserInfo_first_name = UserInfo.Value<string>("given_name") ?? "";
                string UserInfo_last_name = UserInfo.Value<string>("family_name") ?? "";
                string UserInfo_email_verified = UserInfo.Value<string>("email_verified") ?? "";

                if (UserInfo_email_verified != "true")
                {
                    throw new Exception("Your Google Account has not been verified, kindly verify it and sign up again.");
                }

                Adz.BLL.Lib.DeviceInfo deviceinfo = new Adz.BLL.Lib.DeviceInfo()
                {
                    UniqueId = request.DeviceInfo.UniqueId,
                    OS = request.DeviceInfo.OS,
                    OS_Version = request.DeviceInfo.OS_Version,
                    Model = request.DeviceInfo.Model
                };

                UserLogin login = new UserLogin();
                login.Username = UserInfo_email;
                login.Password = "";
                login.DeviceInfo = deviceinfo;
                login.OAuth = true;

                bool FirstTime = false;

                Adz.BLL.Lib.User result_ = null;

                try
                {
                    result_ = userservice.CheckLogin(login).Result;
                    //request.ReferralCode = null;
                }
                catch (CustomException e)
                {
                    if (e.ErrorType == CustomErrorType.UserServiceInvalidUsernamePassword)
                    {
                        UserSignUp signup = new UserSignUp();
                        signup.Email = UserInfo_email;
                        signup.FirstName = UserInfo_first_name;
                        signup.LastName = UserInfo_last_name;
                        signup.Password = "";
                        signup.DeviceInfo = deviceinfo;
                        signup.Gender = 0;
                        signup.DateOfBirth = DateTime.MinValue;
                        signup.ContactNumber = "";
                        signup.Google = true;

                        result_ = userservice.CreateUser(signup).Result;
                        FirstTime = true;
                    }
                }

                if (result_ != null)
                {
                    //update gcm device id
                    if (!string.IsNullOrWhiteSpace(request.GCMDeviceId))
                    {
                        userservice.SetGCMDeviceId(result_.UserId, request.DeviceInfo.UniqueId, request.GCMDeviceId);
                    }

                    Adz.BLL.API.ServiceModel.Types.User result = new Adz.BLL.API.ServiceModel.Types.User();

                    result.UserId = result_.UserId;
                    result.FirstName = result_.FirstName;
                    result.LastName = result_.LastName;
                    result.Email = result_.Email;
                    result.Gender = result_.Gender;
                    if (result_.DateOfBirth == DateTime.MinValue)
                        result.DateOfBirth = null;
                    else
                        result.DateOfBirth = result_.DateOfBirth;
                    result.UserStatus = result_.UserStatusId;
                    result.Password = result_.Password;
                    result.ImageUrl = result_.ImageUrl;
                    result.PointBalance = result_.PointBalance;
                    result.ReferralCode = result_.ReferralCode;
                    result.Debug = result_.Debug;
                    result.FirstTime = FirstTime;
                    //result.ApiKey = result_.ApiKey;
                    //result.ApiKey = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId });

                    /*
                    //AuthUserSession session = (AuthUserSession)this.GetSession();
                    session.IsAuthenticated = true;
                    Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId = login.DeviceInfo.UniqueId;

                    session.FirstName = result.FirstName;
                    session.LastName = result.LastName;
                    Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email = result.Email;
                    session.Gender = result.Gender.ToString();
                    session.BirthDate = result.DateOfBirth;

                    //Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey = result_.ApiKey;

                    this.SaveSession(session, new TimeSpan(TimeSpan.TicksPerHour * int.Parse(ConfigurationManager.AppSettings["sessiontimeout_hour"])));
                    */

                    string msg = null;
                    //if (request.ReferralCode != null && request.ReferralCode != "" && result_.ReferredBy == null)
                    //{
                    //    msg = "Referral Code not found!";
                    //}

                    return new UserResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = msg
                        },
                        Result = result,
                        Key = Helper.EncodeTo64(new string[] { result_.ApiKey, result_.Email, request.DeviceInfo.UniqueId }),
                        Debug = result.Debug
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new UserResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public IntResponse Post(UserReferralCode request)
        {
            try
            {
                var result = userservice.UpdateReferralCode(request.ReferralCode, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result;

                return new IntResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = "Referral Code Accepted."
                    },
                    Result = result,
                    Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                    Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug
                };
            }
            catch (CustomException ex)
            {
                var Message = EnumExtender.GetLocalizedDescription(ex.ErrorType).Split('|');
                var Remarks = Message.Count() <= 1 ? "" : Message[1];
                return new IntResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = Message[0],
                        Remarks = Remarks
                    }
                };
            }
        }

        public IntResponse Post(UserPromoCode request)
        {
            try
            {
                var result = userservice.RedeemPromoCode(request.PromoCode, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result;

                return new IntResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = "Promo Code Accepted.",
                        Remarks = "Points Added : " + result
                    },
                    Result = result,
                    Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                    Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug
                };
            }
            catch (CustomException ex)
            {
                var Message = EnumExtender.GetLocalizedDescription(ex.ErrorType).Split('|');
                var Remarks = Message.Count() <= 1 ? "" : Message[1];
                return new IntResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = Message[0],
                        Remarks = Remarks
                    }
                };
            }
        }

        public CustomResponse Post(ResetPassword request)
        {
            try
            {
                string result = userservice.ResetPasswordUser(request.Email).Result;
                if (result != "")
                {
                    return new CustomResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = result
                        }
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceFailed);
                }
            }
            catch (CustomException ex)
            {
                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    }
                };
            }
        }

        public CustomResponse Post(ResetPasswordVerify request)
        {
            try
            {
                if (userservice.ResetPasswordUserVerify(request.uSecret, request.uToken, "123456").Result == true)
                {
                    return new CustomResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        }
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceFailed);
                }
            }
            catch (CustomException ex)
            {
                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    }
                };
            }
        }

        public UserResponse Any(UserGetCurrent request)
        {
            try
            {
                var result_ = userservice.GetUserById(userservice.GetUserIdByEmail(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result, null);
                if (result_ != null)
                {
                    Adz.BLL.API.ServiceModel.Types.User result = new Adz.BLL.API.ServiceModel.Types.User();

                    result.UserId = result_.UserId;
                    result.FirstName = result_.FirstName;
                    result.LastName = result_.LastName;
                    result.Email = result_.Email;
                    result.Gender = result_.Gender;
                    result.ContactNumber = result_.ContactNumber;
                    if (result_.DateOfBirth == DateTime.MinValue)
                        result.DateOfBirth = null;
                    else
                        result.DateOfBirth = result_.DateOfBirth;
                    result.UserStatus = result_.UserStatusId;
                    result.Password = result_.Password;
                    result.ImageUrl = result_.ImageUrl;
                    result.PointBalance = result_.PointBalance;
                    result.ReferralCode = result_.ReferralCode;

                    return new UserResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        },
                        Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                        Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug,
                        Result = result
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new UserResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public IntResponse Any(UserPointBalance request)
        {
            try
            {
                var result_ = userservice.GetUserById(userservice.GetUserIdByEmail(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result, null);
                if (result_ != null)
                {
                    return new IntResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        },
                        Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                        Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug,
                        Result = result_.PointBalance
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new IntResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = -1
                };
            }
        }

        public UserResponse Post(UserUpdate request)
        {
            try
            {
                Adz.BLL.Lib.User user = new Adz.BLL.Lib.User();
                user.UserId = userservice.GetUserIdByEmail(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Gender = request.Gender;
                user.DateOfBirth = request.DateOfBirth;
                user.ContactNumber = request.ContactNumber;
                if (request.NewEmail == null || request.NewEmail == "")
                {
                    user.Email = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email;
                }
                else
                {
                    user.Email = request.NewEmail;
                }

                int imageid = userservice.UploadUserImage(request.ImageByteString, request.ImageMimeType, user.UserId).Result;
                user.SubImageId = new List<int>();
                if (imageid == 0)
                {
                    user.SubImageId.Add(imageid);
                }
                user.PointBalance = -1;

                bool result = userservice.CreateEditUser(user).Result;
                
                if(result == true)
                {
                    var result_ = userservice.GetUserById(userservice.GetUserIdByEmail(user.Email).Result, null);
                    Adz.BLL.API.ServiceModel.Types.User User = new Adz.BLL.API.ServiceModel.Types.User();
                    if (result_ != null)
                    {
                        User.UserId = result_.UserId;
                        User.FirstName = result_.FirstName;
                        User.LastName = result_.LastName;
                        User.Email = result_.Email;
                        User.Gender = result_.Gender;
                        User.ContactNumber = result_.ContactNumber;
                        if (result_.DateOfBirth == DateTime.MinValue)
                            User.DateOfBirth = null;
                        else
                            User.DateOfBirth = result_.DateOfBirth;
                        User.UserStatus = result_.UserStatusId;
                        User.Password = result_.Password;
                        User.ImageUrl = result_.ImageUrl;
                        User.PointBalance = result_.PointBalance;
                        User.ReferralCode = result_.ReferralCode;
                    }

                    return new UserResponse
                    {
                        Result = User,
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        },
                        Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, user.Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), user.Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                        Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new UserResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    }
                };
            }
        }

        public BoolResponse Any(UserSetGCMDeviceId request)
        {
            try
            {
                //update gcm device id
                if (!string.IsNullOrWhiteSpace(request.GCMDeviceId))
                {
                    var UserData = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName));
                    userservice.SetGCMDeviceId(UserData.UserId, UserData.UniqueDeviceId, request.GCMDeviceId);
                }

                return new BoolResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Result = true
                };
            }
            catch (CustomException ex)
            {
                return new BoolResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = false
                };
            }
        }

        public object Any(UserGetPointHistory request)
        {
            try
            {
                var UserId = userservice.GetUserIdByEmail(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result;
                if (UserId != 0)
                {
                    var result = new List<Adz.BLL.API.ServiceModel.Types.UserTrx>();

                    var UserTrxList = usertrxservice.GetTransactionByUser(UserId);
                    foreach (var v in UserTrxList)
                    {
                        result.Add(new ServiceModel.Types.UserTrx()
                        {
                            AccountFrom = v.AccountFrom,
                            Balance = v.Balance,
                            CreatedDate = v.CreatedDate ?? DateTime.MinValue,
                            CreditAmount = v.CreditAmount,
                            DebitAmount = v.DebitAmount,
                            Description = v.Description,
                            TransactionDate = v.TransactionDate,
                            TransactionMonth = v.TransactionMonth,
                            TransactionYear = v.TransactionYear
                        });
                    }

                    return new UserTrxResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        },
                        Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                        Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug,
                        Result = result
                    };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceUnknown);
                }
            }
            catch (CustomException ex)
            {
                return new UserTrxResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

    }
}
