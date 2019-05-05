using Adz.DAL.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class UserService : IUserService
    {
        public IUserTrxService UserTrxService = new UserTrxService(); 

        public Response<User> CheckLogin(UserLogin login)
        {
            Response<User> response = null;

            using (var context = new TheAdzEntities())
            {
                User us = new User();
                var entityUser = from d in context.Users
                                 where d.email.ToLower() == login.Username.ToLower()
                                 && d.locked_status == false
                                 && (d.userstatus_id == 1 || d.userstatus_id == 4 || d.userstatus_id == 5)
                                 select d;
                if (entityUser.Count() > 0)
                {
                    string pass = "";
                    if (login.OAuth == false)
                    {
                        pass = Security.checkHMAC(entityUser.First().password_salt, login.Password);

                        pass = pass == "" ? "NO_PASSWORD" : pass;
                    }
                    if (login.OAuth == true || pass == entityUser.First().password)
                    {
                        entityUser.First().last_login = DateTime.UtcNow;

                        string tokensalt = Security.RandomString(60);
                        string token = Security.Encrypt(tokensalt, entityUser.First().email.ToLower());

                        var entityAPIKeys = from d in context.APIKeys
                                            where d.unique_id == login.DeviceInfo.UniqueId && d.user_id == entityUser.FirstOrDefault().id
                                            select d;
                        if (entityAPIKeys.Count() > 0)
                        {
                            entityAPIKeys.First().os_version = login.DeviceInfo.OS_Version;

                            entityAPIKeys.First().token = token;
                            entityAPIKeys.First().token_salt = tokensalt;
                            entityAPIKeys.First().token_expire = DateTime.UtcNow.AddMonths(1);
                            context.SaveChanges();

                            us.Debug = (int)entityAPIKeys.First().flag_debug;
                        }
                        else
                        {
                            Adz.DAL.EF.APIKey newentity = new Adz.DAL.EF.APIKey();
                            newentity.token = token;
                            newentity.token_salt = tokensalt;
                            newentity.token_expire = DateTime.UtcNow.AddMonths(1);
                            newentity.os = login.DeviceInfo.OS;
                            newentity.os_version = login.DeviceInfo.OS_Version;
                            newentity.model = login.DeviceInfo.Model;
                            newentity.unique_id = login.DeviceInfo.UniqueId;
                            newentity.user_id = entityUser.First().id;
                            newentity.last_created = DateTime.UtcNow;
                            newentity.flag_debug = 0;
                            
                            context.APIKeys.Add(newentity);
                            context.SaveChanges();

                            us.Debug = (int)newentity.flag_debug;
                        }
                        us.ApiKey = token;

                        us.UserId = entityUser.First().id;
                        us.FirstName = entityUser.First().first_name;
                        us.LastName = entityUser.First().last_name;
                        us.Gender = (int)entityUser.First().gender;
                        us.Email = entityUser.First().email;
                        if (entityUser.First().dateofbirth != null)
                        {
                            us.DateOfBirth = (DateTime)entityUser.First().dateofbirth;
                        }

                        us.ImageUrl = "";
                        if (entityUser.First().image_id != null)
                        {
                            us.ImageUrl = ConfigurationManager.AppSettings["uploadpath"] + entityUser.First().Image.url;
                        }
                        
                        us.Notif = (bool)entityUser.First().notif;
                        us.UserStatusId = (int)entityUser.First().userstatus_id;
                        us.PointBalance = (int)entityUser.First().point_balance;
                        us.ReferralCode = entityUser.First().referral_code;
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.UserServiceEmailPasswordWrong);
                    }
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceInvalidUsernamePassword);
                }

                response = Response<User>.Create(us);
            }

            return response;
        }

        public Response<int> UpdateReferralCode(string ReferralCode, string Email)
        {
            Response<int> response = null;

            using (var context = new TheAdzEntities())
            {
                var entityUser = from d in context.Users
                                 where d.email.ToLower() == Email.ToLower() && d.referred_by == null
                                 select d;

                if (entityUser.Count() > 0)
                {
                    var entityUser2 = from d in context.Users
                                      where d.referral_code == ReferralCode.ToUpper() && d.id != entityUser.FirstOrDefault().id
                                      select d;

                    if (entityUser2.Count() > 0)
                    {
                        var userId = entityUser.First().id;

                        entityUser.First().point_balance += AdzConstants.REFERRAL_POINT;
                        entityUser.First().referred_by = ReferralCode;
                        entityUser.First().last_updated = DateTime.UtcNow;
                        entityUser2.First().point_balance += AdzConstants.REFERRAL_POINT;
                        entityUser2.First().last_updated = DateTime.UtcNow;
                        context.SaveChanges();

                        entityUser = from d in context.Users
                                     where d.id == userId
                                     select d;

                        //create user transaction
                        UserTrxService.CreateTransaction(new UserTrx()
                        {
                            AccountFrom = "Adz",
                            AccountTo = entityUser.First().id.ToString(),
                            Balance = entityUser.First().point_balance.Value,
                            CreatedBy = 0,
                            CreditAmount = AdzConstants.REFERRAL_POINT,
                            Description = "Referred by " + entityUser2.First().first_name + " " + entityUser2.First().last_name,
                            TransactionDate = DateTime.UtcNow,
                            TransactionMonth = DateTime.UtcNow.Month,
                            TransactionYear = DateTime.UtcNow.Year,
                            UserId = entityUser.First().id,
                        });
                        UserTrxService.CreateTransaction(new UserTrx()
                        {
                            AccountFrom = "Adz",
                            AccountTo = entityUser2.First().id.ToString(),
                            Balance = entityUser2.First().point_balance.Value,
                            CreatedBy = 0,
                            CreditAmount = AdzConstants.REFERRAL_POINT,
                            Description = "Referral to " + entityUser.First().first_name + " " + entityUser.First().last_name,
                            TransactionDate = DateTime.UtcNow,
                            TransactionMonth = DateTime.UtcNow.Month,
                            TransactionYear = DateTime.UtcNow.Year,
                            UserId = entityUser2.First().id,
                        });

                        response = Response<int>.Create(AdzConstants.REFERRAL_POINT);
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.ReferralCodeNotFound);
                    }
                }
                else
                {
                    throw new CustomException(CustomErrorType.ReferralCodeAlreadyAssign);
                }
            }

            return response;
        }

        public Response<int> RedeemPromoCode(string PromoCode, string Email)
        {
            Response<int> response = null;

            using (var context = new TheAdzEntities())
            {
                var ettPromotion = from d in context.Promotions
                                   where d.code.ToLower() == PromoCode.ToLower()
                                   && d.last_action == "1"
                                   select d;

                if (ettPromotion.Count() > 0)
                {
                    var ettUser = from d in context.Users
                                  where d.email.ToLower() == Email.ToLower()
                                  select d;

                    if (ettUser.Count() > 0)
                    {
                        //check if Promotion is on schedule
                        if (ettPromotion.First().flag_on_schedule == true)
                        {
                            if (DateTime.Now.Date < ettPromotion.First().start_datetime.Value.Date)
                            {
                                throw new CustomException(CustomErrorType.PromotionHaventStarted);
                            }
                            else if (DateTime.Now.Date > ettPromotion.First().end_datetime.Value.Date)
                            {
                                throw new CustomException(CustomErrorType.PromotionAlreadyEnd);
                            }
                        }

                        var PromoId = ettPromotion.First().id;
                        var UserId = ettUser.First().id;

                        var ettPromotionTrx = from d in context.PromotionTrxes
                                              where d.promo_id == PromoId && d.user_id == UserId
                                              select d;

                        if (ettPromotionTrx.Count() > 0)
                        {
                            throw new CustomException(CustomErrorType.PromotionAlreadyAssign);
                        }
                        else
                        {
                            ettUser.First().point_balance += (int)(ettPromotion.First().value ?? (decimal)0);
                            ettUser.First().last_updated = DateTime.UtcNow;
                            context.SaveChanges();

                            //create user transaction
                            UserTrxService.CreateTransaction(new UserTrx()
                            {
                                AccountFrom = "Adz",
                                AccountTo = ettUser.First().id.ToString(),
                                Balance = ettUser.First().point_balance.Value,
                                CreatedBy = 0,
                                CreditAmount = (int)(ettPromotion.First().value ?? (decimal)0),
                                Description = "Promo Code - " + ettPromotion.First().code,
                                TransactionDate = DateTime.UtcNow,
                                TransactionMonth = DateTime.UtcNow.Month,
                                TransactionYear = DateTime.UtcNow.Year,
                                UserId = ettUser.First().id,
                            });

                            //create promotion trx
                            PromotionTrx PromotionTrx = new PromotionTrx();
                            PromotionTrx.last_created = DateTime.UtcNow;
                            PromotionTrx.promo_id = PromoId;
                            PromotionTrx.user_id = UserId;
                            context.PromotionTrxes.Add(PromotionTrx);
                            context.SaveChanges();

                            response = Response<int>.Create((int)(ettPromotion.First().value ?? (decimal)0));
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.UserServiceNotFound);
                    }
                }
                else
                {
                    throw new CustomException(CustomErrorType.PromotionNotFound);
                }
            }

            return response;
        }

        public Response<User> CreateUser(UserSignUp signup)
        {
            Response<User> response = null;

            using (var context = new TheAdzEntities())
            {

                var entityUser = from d in context.Users
                                 where d.email.ToLower() == signup.Email.ToLower()
                                 select d;
                if (entityUser.Count() > 0)
                {
                    throw new CustomException(CustomErrorType.UserServiceAlreadyAssign);
                }
                else
                {
                    Adz.DAL.EF.User mmentity = new Adz.DAL.EF.User();
                    mmentity.first_name = signup.FirstName;
                    mmentity.last_name = signup.LastName;
                    mmentity.email = signup.Email;
                    mmentity.gender = signup.Gender;
                    mmentity.contact_number = signup.ContactNumber;
                    if (signup.DateOfBirth != DateTime.MinValue)
                    {
                        mmentity.dateofbirth = signup.DateOfBirth;
                    }
                    mmentity.last_updated = DateTime.UtcNow;
                    mmentity.last_created = DateTime.UtcNow;
                    mmentity.locked_status = false;
                    mmentity.userstatus_id = 1;

                    if(signup.Facebook == true)
                    {
                        mmentity.password = "";
                        mmentity.password_salt = "";
                        mmentity.facebook_id = signup.FacebookID;
                    }
                    else if (signup.Google == true)
                    {
                        mmentity.password = "";
                        mmentity.password_salt = "";
                    }
                    else
                    {
                        string key1 = "tH34Dz";
                        string key = Security.checkHMAC(key1, signup.Email.ToLower());
                        string pass = Security.checkHMAC(key, signup.Password);
                        mmentity.password = pass;
                        mmentity.password_salt = key;
                    }

                    mmentity.notif = true;
                    mmentity.point_balance = 0;

                    string referral_code;
                    while (true)
                    {
                        referral_code = Security.GetUniqueKey(5);

                        if (context.Users.Any(u => u.referral_code == referral_code))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    mmentity.referral_code = referral_code;

                    context.Users.Add(mmentity);
                    context.SaveChanges();
                    int userid = (int)mmentity.id;
                    
                    if (signup.ReferredBy != null && signup.ReferredBy != "")
                    {
                        var entityUser2 = from d in context.Users
                                          where d.referral_code == signup.ReferredBy.ToUpper() && d.id != entityUser.FirstOrDefault().id
                                          select d;

                        if (entityUser2.Count() > 0)
                        {
                            mmentity.referred_by = signup.ReferredBy.ToUpper();
                            mmentity.point_balance = AdzConstants.REFERRAL_POINT;
                            entityUser2.First().point_balance += AdzConstants.REFERRAL_POINT;
                            entityUser2.First().last_updated = DateTime.UtcNow;
                            context.SaveChanges();

                            //create user transaction
                            UserTrxService.CreateTransaction(new UserTrx()
                            {
                                AccountFrom = "Adz",
                                AccountTo = mmentity.id.ToString(),
                                Balance = mmentity.point_balance.Value,
                                CreatedBy = 0,
                                CreditAmount = AdzConstants.REFERRAL_POINT,
                                Description = "Referred by " + entityUser2.First().first_name + " " + entityUser2.First().last_name,
                                TransactionDate = DateTime.UtcNow,
                                TransactionMonth = DateTime.UtcNow.Month,
                                TransactionYear = DateTime.UtcNow.Year,
                                UserId = mmentity.id,
                            });
                            UserTrxService.CreateTransaction(new UserTrx()
                            {
                                AccountFrom = "Adz",
                                AccountTo = entityUser2.First().id.ToString(),
                                Balance = entityUser2.First().point_balance.Value,
                                CreatedBy = 0,
                                CreditAmount = AdzConstants.REFERRAL_POINT,
                                Description = "Referral to " + mmentity.first_name + " " + mmentity.last_name,
                                TransactionDate = DateTime.UtcNow,
                                TransactionMonth = DateTime.UtcNow.Month,
                                TransactionYear = DateTime.UtcNow.Year,
                                UserId = entityUser2.First().id,
                            });
                        }
                    }
                    
                    if (signup.ImageByteString != null && signup.ImageMimeType != null)
                    {
                        UploadUserImage(signup.ImageByteString, signup.ImageMimeType, userid);
                    }

                    //this.SendVerificationEmail(mmentity.email, "http://www.google.com");

                    User us = GetUserById(userid, signup.DeviceInfo, true);
                    response = Response<User>.Create(us);
                }
            }

            return response;
        }

        public Response<int> UploadUserImage(string ImageBase64String, string ImageMimeType, int userid)
        {
            if (ImageBase64String != null && ImageMimeType != null)
            {
                using (var context = new TheAdzEntities())
                {
                    int imageid = 0;
                    try
                    {
                        string date = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                        string imagename = General.uploadImage(ImageBase64String, ImageMimeType, date, userid.ToString());
                        Adz.DAL.EF.Image entImage = new Adz.DAL.EF.Image();
                        entImage.url = imagename;
                        entImage.last_action = "1";
                        context.Images.Add(entImage);
                        context.SaveChanges();
                        imageid = entImage.id;
                    }
                    catch { }

                    if (imageid != 0)
                    {
                        var entityUserNew = from d in context.Users
                                            where d.id == userid
                                            select d;
                        entityUserNew.First().image_id = imageid;
                        context.SaveChanges();
                    }

                    return new Response<int>()
                    {
                        Result = imageid
                    };
                }
            }

            return new Response<int>()
            {
                Result = 0
            };
        }

        public User GetUserById(int id, DeviceInfo deviceinfo, bool? NewUser = false)
        {
            using (var context = new TheAdzEntities())
            {
                User us = new User();
                var entityUser = from d in context.Users
                                 where d.id == id
                                 && d.locked_status == false
                                 && (d.userstatus_id == 1 || d.userstatus_id == 4 || d.userstatus_id == 5)
                                 select d;
                if (entityUser.Count() > 0)
                {
                    if (deviceinfo != null)
                    {
                        if(NewUser == false)
                            entityUser.First().last_login = DateTime.UtcNow;

                        string tokensalt = Security.RandomString(60);
                        string token = Security.Encrypt(tokensalt, entityUser.First().email.ToLower());

                        Adz.DAL.EF.APIKey newentity = new Adz.DAL.EF.APIKey();
                        newentity.token = token;
                        newentity.token_salt = tokensalt;
                        newentity.token_expire = DateTime.UtcNow.AddMonths(1);
                        newentity.os = deviceinfo.OS;
                        newentity.os_version = deviceinfo.OS_Version;
                        newentity.model = deviceinfo.Model;
                        newentity.unique_id = deviceinfo.UniqueId;
                        newentity.user_id = entityUser.First().id;
                        newentity.last_created = DateTime.UtcNow;
                        newentity.flag_debug = 0;

                        context.APIKeys.Add(newentity);
                        context.SaveChanges();

                        us.ApiKey = token;
                        us.Debug = 0;
                    }

                    us.UserId = entityUser.First().id;
                    us.FirstName = entityUser.First().first_name;
                    us.LastName = entityUser.First().last_name;
                    us.Gender = (int)entityUser.First().gender;
                    us.ContactNumber = entityUser.First().contact_number;
                    us.Email = entityUser.First().email;
                    if (entityUser.First().dateofbirth != null)
                    {
                        us.DateOfBirth = (DateTime)entityUser.First().dateofbirth;
                    }
                    us.ImageUrl = "";
                    if (entityUser.First().image_id != null)
                    {
                        us.ImageUrl = ConfigurationManager.AppSettings["uploadpath"] + entityUser.First().Image.url;
                    }
                    us.Notif = (bool)entityUser.First().notif;
                    us.UserStatusId = (int)entityUser.First().userstatus_id;
                    us.UserStatus = new UserStatus() { UserStatusId = (int)entityUser.First().userstatus_id, UserStatusName = entityUser.First().UserStatu.name };
                    us.PointBalance = (int)entityUser.First().point_balance;
                    us.ReferralCode = entityUser.First().referral_code;
                    us.ReferredBy = entityUser.First().referred_by;

                    List<int> subimgidUser = new List<int>();
                    List<string> subimgnameUser = new List<string>();
                    List<string> subimgurlUser = new List<string>();
                    List<string> subimgurllinkUser = new List<string>();
                    if (entityUser.First().image_id != null)
                    {
                        subimgidUser.Add(entityUser.First().Image.id);
                        subimgnameUser.Add(entityUser.First().Image.url);
                        subimgurlUser.Add(ConfigurationManager.AppSettings["uploadpath"] + entityUser.First().Image.url);
                    }
                    us.SubImageId = subimgidUser;
                    us.SubImageName = subimgnameUser;
                    us.SubImageUrl = subimgurlUser;
                    us.SubImageUrlLink = subimgurllinkUser;

                    return us;
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceEmailPasswordWrong);
                }
            }
        }

        public Response<int> GetUserIdByEmail(string email)
        {
            using (var context = new TheAdzEntities())
            {
                User us = new User();
                var entityUser = from d in context.Users
                                 where d.email == email
                                 && d.locked_status == false
                                 && (d.userstatus_id == 1 || d.userstatus_id == 4 || d.userstatus_id == 5)
                                 select d;
                if (entityUser.Count() > 0)
                {
                    return new Response<int>(){
                        Result = entityUser.First().id
                    };
                }
            }

            throw new CustomException(CustomErrorType.UserServiceNotFound); 
        }


        public Response<List<User>> GetUserList()
        {
            Response<List<User>> response = null;
            List<User> UserList = new List<User>();
            using (var context = new TheAdzEntities())
            {
                var entityUser = (from u in context.Users.Include("UserStatus")
                                    join f in context.Facebooks on u.id equals f.user_id
                                    into a
                                    from b in a.DefaultIfEmpty()
                                    orderby u.last_created descending
                                    select new
                                    {
                                        u.id,
                                        u.first_name,
                                        u.last_name,
                                        u.email,
                                        u.gender,
                                        u.contact_number,
                                        u.dateofbirth,
                                        u.locked_status,
                                        u.last_lockout,
                                        u.userstatus_id,
                                        u.UserStatu.name,
                                        b.user_id,
                                        u.last_created,
                                        u.referral_code,
                                        u.referred_by,
                                        u.point_balance
                                    }).Distinct().ToList();
                foreach (var v in entityUser)
                {
                    User User = new User();
                    User.UserId = v.id;
                    User.FirstName = v.first_name;
                    User.LastName = v.last_name;
                    User.Email = v.email;
                    User.Gender = (int)v.gender;
                    User.ContactNumber = v.contact_number;
                    User.ReferralCode = v.referral_code;
                    User.ReferredBy = v.referred_by;
                    User.PointBalance = (int)v.point_balance;
                    if (v.dateofbirth != null)
                    {
                        User.DateOfBirth = (DateTime)v.dateofbirth;
                    }
                    User.LastLockout = (bool)v.locked_status;
                    if (v.last_lockout != null)
                    {
                        User.LastLockoutDate = (DateTime)v.last_lockout;
                    }
                    UserStatus st = new UserStatus();
                    st.UserStatusId = (int)v.userstatus_id;
                    st.UserStatusName = v.name;
                    User.UserStatus = st;

                    User.Create = (DateTime)v.last_created;
                    UserList.Add(User);
                }
                response = Response<List<User>>.Create(UserList);
            }

            return response;
        }
        
        public Response<List<UserStatus>> GetUserStatusList()
        {
            Response<List<UserStatus>> response = null;
            List<UserStatus> UserList = new List<UserStatus>();
            using (var context = new TheAdzEntities())
            {
                var entityUser = from d in context.UserStatus
                                   where d.last_action != "5"
                                   select d;
                foreach (var v in entityUser)
                {
                    UserStatus User = new UserStatus();
                    User.UserStatusId = v.id;
                    User.UserStatusName = v.name;
                    UserList.Add(User);
                }
                response = Response<List<UserStatus>>.Create(UserList);
            }

            return response;
        }
        
        public Response<bool> CreateEditUser(User User)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                if (User.UserId != 0)
                {
                    var entityUser = from d in context.Users
                                       where d.id == User.UserId
                                       select d;
                    if (entityUser.Count() > 0)
                    {
                        var entityUser2 = from d in context.Users
                                            where d.email.ToLower() == User.Email.ToLower()
                                              && d.id != User.UserId
                                            select d;
                        if (entityUser2.Count() > 0)
                        {
                            throw new CustomException(CustomErrorType.UserServiceAlreadyAssign);
                        }
                        else
                        {
                            entityUser.First().first_name = User.FirstName;
                            entityUser.First().last_name = User.LastName;
                            if (User.Email != null && User.Email != "")
                            {
                                entityUser.First().email = User.Email;
                            }
                            entityUser.First().gender = User.Gender;
                            entityUser.First().contact_number = User.ContactNumber;

                            if (User.Password != null && User.Password != "")
                            {
                                string key1 = "tH34Dz";
                                string key = Security.checkHMAC(key1, User.Email.ToLower());
                                string pass = Security.checkHMAC(key, User.Password);
                                entityUser.First().password = pass;
                                entityUser.First().password_salt = key;
                            }

                            //update also shipping and billing users.
                            //var entityUser3 = from d in context.ShippingUsers
                            //                    where d.user_id == User.UserId
                            //                    select d;
                            //if(entityUser3.Count()>0)
                            //    entityUser3.First().gender = User.Gender;

                            //var entityUser4 = from d in context.BillingUsers
                            //                    where d.user_id == User.UserId
                            //                    select d;
                            //if (entityUser4.Count() > 0)
                            //    entityUser4.First().gender = User.Gender;

                            if (User.DateOfBirth != DateTime.MinValue)
                            {
                                entityUser.First().dateofbirth = User.DateOfBirth;
                            }
                            entityUser.First().last_updated = DateTime.UtcNow;
                            if (User.UserStatus != null)
                            {
                                entityUser.First().userstatus_id = User.UserStatus.UserStatusId;
                            }

                            if (User.PointBalance >= 0)
                            {
                                entityUser.First().point_balance = User.PointBalance;
                            }

                            context.SaveChanges();

                            #region image
                            if (User.SubImageId != null)
                            {
                                try
                                {                                    
                                    entityUser.First().image_id = User.SubImageId.First();
                                    context.SaveChanges();
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                entityUser.First().image_id = null;
                                context.SaveChanges();
                            }
                            #endregion

                            response = Response<bool>.Create(true);
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.UserServiceNotFound);
                    }
                }
                else
                {
                    var entityUser = from d in context.Users
                                       where d.email.ToLower() == User.Email.ToLower()
                                       select d;
                    if (entityUser.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.UserServiceAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.User mmentity = new Adz.DAL.EF.User();
                        mmentity.first_name = User.FirstName;
                        mmentity.last_name = User.LastName;
                        mmentity.email = User.Email;
                        mmentity.gender = User.Gender;
                        mmentity.contact_number = User.ContactNumber;
                        mmentity.dateofbirth = User.DateOfBirth;
                        mmentity.last_created = DateTime.UtcNow;
                        mmentity.last_updated = DateTime.UtcNow;
                        mmentity.last_login = DateTime.UtcNow;
                        mmentity.locked_status = false;
                        mmentity.userstatus_id = User.UserStatus.UserStatusId;
                        string key1 = "tH34Dz";
                        string key = Security.checkHMAC(key1, User.Email.ToLower());
                        string pass = Security.checkHMAC(key, User.Password);
                        mmentity.password = pass;
                        mmentity.password_salt = key;
                        mmentity.notif = true;
                        mmentity.point_balance = User.PointBalance;

                        string referral_code;
                        while (true)
                        {
                            referral_code = Security.GetUniqueKey(5);

                            if (context.Users.Any(u => u.referral_code == referral_code))
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                        mmentity.referral_code = referral_code;

                        context.Users.Add(mmentity);
                        context.SaveChanges();

                        #region image
                        if (User.SubImageId != null)
                        {
                            try
                            {
                                entityUser.First().image_id = User.SubImageId.First();
                                context.SaveChanges();
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            entityUser.First().image_id = null;
                            context.SaveChanges();
                        }
                        #endregion

                        response = Response<bool>.Create(true);
                    }
                }
            }
            return response;
        }

        public Response<string> ResetPasswordUser(string email)
        {
            using (var context = new TheAdzEntities())
            {
                var entityUser = from d in context.Users
                                 where d.email.ToLower() == email.ToLower()
                                 && (d.userstatus_id == 1 || d.userstatus_id == 4 || d.userstatus_id == 5)
                                 select d;
                if (entityUser.Count() > 0)
                {
                    string token = Security.RandomString2(255);
                    string secret = Security.Encrypt(token, entityUser.First().id.ToString());

                    entityUser.First().resetpass = token;
                    entityUser.First().resetpass_expire = DateTime.UtcNow.AddDays(1);
                    context.SaveChanges();
                    //SendResetEmail(email, entityUser.First().first_name, usersecret, token);
                    return new Response<string> { Result = ConfigurationManager.AppSettings["domain"] + "user/resetpasswordverify" + "?usecret=" + secret + "&utoken=" + token };
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceInvalidEmail);
                }
            }
        }

        public void SendResetEmail(string email, string name, string secret, string token)
        {
            using (var context = new TheAdzEntities())
            {
                string to = email;
                string body = Email.CreateResetPasswordEmail(name, ConfigurationManager.AppSettings["domain"] + "user/resetpasswordverify" + "?usecret=" + secret + "&utoken=" + token);
                string subject = "Password Reset Request";

                General.SendEmail(subject, to, body);
            }
        }

        public Response<bool> ResetPasswordUserVerify(string secret, string token, string newpassword)
        {
            using (var context = new TheAdzEntities())
            {
                string user = Security.Decrypt(token, secret);
                int userid = int.Parse(user);

                var entityUser = from d in context.Users
                                 where d.id == userid && d.resetpass == token
                                 select d;
                if (entityUser.Count() > 0)
                {
                    if ((DateTime)entityUser.First().resetpass_expire < DateTime.UtcNow)
                    {
                        return new Response<bool> { Result = false };
                    }
                    else
                    {
                        entityUser.First().locked_status = false;
                        string key1 = "tH34Dz";
                        string key = Security.checkHMAC(key1, entityUser.First().email.ToLower());
                        string pass = Security.checkHMAC(key, newpassword);
                        entityUser.First().password = pass;
                        entityUser.First().password_salt = key;
                        entityUser.First().resetpass_expire = DateTime.UtcNow;
                        context.SaveChanges();

                        return new Response<bool> { Result = true };
                    }
                }
                else
                {
                    throw new CustomException(CustomErrorType.UserServiceNotFound);
                }
            }
        }

        int maxCount = 20;
        int actionClickCount = 5;
        int userPointIncrement = 50;

        int GMT = 8;

        public Response<List<int>> UserPointUpdate()
        {
            DateTime today_beginning = DateTime.UtcNow.AddDays(-1);
            DateTime today_end = DateTime.UtcNow;
            DateTime datetime_now = DateTime.UtcNow;

            //DateTime today_end = DateTime.ParseExact("2016-08-26 16:00:58", "yyyy-MM-dd HH:mm:ss", null);
            //DateTime today_beginning = today_end.AddDays(-1);
            //DateTime datetime_now = today_end;

            List<int> To_Be_Processed_Users = new List<int>();
            Dictionary<int, int> Insufficient_Count_Users = new Dictionary<int, int>();
            using (var context = new TheAdzEntities())
            {
                context.Database.CommandTimeout = 900;

                //get users who get point increment
                var entityCHistory = from d in context.CampaignHistories
                                     where today_beginning <= d.view_date && d.view_date < today_end
                                     group d by new { user_id = d.user_id, action = d.action } into g
                                     orderby g.Key.user_id ascending
                                     select new
                                     {
                                         user_id = g.Key.user_id,
                                         action = g.Key.action,
                                         count = g.Count()
                                     };

                if (entityCHistory.Count() > 0)
                {

                    int tempUserId = (int)entityCHistory.First().user_id;
                    int tempCount = 0;
                    foreach (var v in entityCHistory)
                    {
                        if (tempUserId != v.user_id)
                        {
                            if (tempCount >= maxCount)
                            {
                                To_Be_Processed_Users.Add(tempUserId);
                            }
                            else
                            {
                                Insufficient_Count_Users.Add(tempUserId, tempCount);
                            }

                            tempUserId = (int)v.user_id;
                            tempCount = 0;
                        }

                        tempCount += (v.count * (v.action == 1 ? 1 : actionClickCount));
                    }
                    if (tempCount >= maxCount)
                    {
                        To_Be_Processed_Users.Add(tempUserId);
                    }
                    else
                    {
                        Insufficient_Count_Users.Add(tempUserId, tempCount);
                    }

                    //set 'flag_processed' to true to all today's records
                    var entityCHistory2 = from d in context.CampaignHistories
                                          where today_beginning <= d.view_date && d.view_date < today_end
                                          select d;

                    foreach (var v in entityCHistory2)
                    {
                        v.flag_processed = true;
                        v.last_updated = datetime_now;
                        if (To_Be_Processed_Users.Contains(v.user_id ?? 0))
                        {
                            v.remarks = "Success";
                        }
                        else if(Insufficient_Count_Users.ContainsKey(v.user_id ?? 0))
                        {
                            v.remarks = Insufficient_Count_Users[v.user_id.Value] + " views is less than the required views, " + maxCount + ".";
                        }
                    }
                    context.SaveChanges();

                    #region old method
                    //set 'flag_processed' to true for requirement records only
                    /*
                    var entityCH = from d in context.CampaignHistories
                                   where today_beginning <= d.view_date && d.view_date < today_end
                                   && Users.Contains((int)d.user_id)
                                   orderby d.user_id ascending, d.view_date ascending
                                   select d;

                    if (entityCH.Count() > 0)
                    {
                        tempUserId = (int)entityCH.First().user_id;
                        tempCount = 0;
                        foreach (var v in entityCH)
                        {
                            if (tempUserId != v.user_id)
                            {
                                tempUserId = (int)v.user_id;
                                tempCount = 0;
                            }

                            if (tempCount < maxCount)
                            {
                                v.flag_processed = true;
                                tempCount += (v.action == 1 ? 1 : actionClickCount);
                            }
                        }
                        context.SaveChanges();
                    }
                    */
                    #endregion

                    //increment users 'point_balance'
                    var entityUser = from d in context.Users
                                     where To_Be_Processed_Users.Contains((int)d.id)
                                     select d;
                    foreach (var v in entityUser)
                    {
                        v.point_balance += userPointIncrement;
                        v.last_updated = datetime_now;

                        //create user transaction
                        UserTrxService.CreateTransaction(new UserTrx()
                        {
                            AccountFrom = "Adz",
                            AccountTo = v.id.ToString(),
                            Balance = v.point_balance.Value,
                            CreatedBy = 0,
                            CreditAmount = userPointIncrement,
                            Description = "Daily Point",
                            TransactionDate = datetime_now,
                            TransactionMonth = datetime_now.Month,
                            TransactionYear = datetime_now.Year,
                            CreatedDate = DateTime.UtcNow,
                            UserId = v.id,
                        });
                    }
                    context.SaveChanges();
                }
            }

            return new Response<List<int>> { Result = To_Be_Processed_Users };
        }

        public Response<List<KeyValuePair<int, DateTime>>> UserPointUpdatePastUsage()
        {
            List<KeyValuePair<int, DateTime>> result = new List<KeyValuePair<int, DateTime>>();
            using (var context = new TheAdzEntities())
            {
                context.Database.CommandTimeout = 900;

                //ToDate(in UTC) to prevent checking today's data
                var ToDate = DateTime.UtcNow.AddDays(-1);
                //var ToDate = DateTime.ParseExact(DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd") + " 16:00:00", "yyyy-MM-dd HH:mm:ss", null);

                //FromDate(in UTC) to save work from checking Older data
                var FromDate = DateTime.ParseExact("2016-05-06 16:00:00", "yyyy-MM-dd HH:mm:ss", null);

                ////for manual patching
                //var ToDate = DateTime.ParseExact("2016-09-05 16:00:00", "yyyy-MM-dd HH:mm:ss", null);
                //var FromDate = DateTime.ParseExact("2016-08-27 16:00:00", "yyyy-MM-dd HH:mm:ss", null);
                ////var user_id = 313;

                var NewlyAdded_PastUsage = from e in
                                               (from d in context.CampaignHistories
                                                where d.flag_processed == false
                                                && d.view_date != null
                                                && (FromDate <= d.view_date && d.view_date < ToDate)
                                                //&& d.user_id == user_id
                                                select new
                                                {
                                                    CampaignHistory = d,
                                                    view_date_in_LocalTime = System.Data.Entity.DbFunctions.AddHours(d.view_date, GMT).Value,
                                                })
                                           group e by new { UserId = e.CampaignHistory.user_id, Day = e.view_date_in_LocalTime.Day, Month = e.view_date_in_LocalTime.Month, Year = e.view_date_in_LocalTime.Year } into g
                                           select g.Key;

                var AlreadySuccess_PastUsage = from e in
                                                   (from d in context.CampaignHistories
                                                    where d.flag_processed == true
                                                    && d.remarks == "Success"
                                                    && d.view_date != null
                                                    && (FromDate <= d.view_date && d.view_date < ToDate)
                                                    //&& d.user_id == user_id
                                                    select new
                                                    {
                                                        CampaignHistory = d,
                                                        view_date_in_LocalTime = DbFunctions.AddHours(d.view_date, GMT).Value,
                                                    })
                                               group e by new { UserId = e.CampaignHistory.user_id, Day = e.view_date_in_LocalTime.Day, Month = e.view_date_in_LocalTime.Month, Year = e.view_date_in_LocalTime.Year } into g
                                               select g.Key;

                var ToBeProcessed_PastUsage = NewlyAdded_PastUsage.Except(AlreadySuccess_PastUsage).ToList();

                foreach (var v in ToBeProcessed_PastUsage)
                {
                    var Processing_PastUsage = from d in context.CampaignHistories
                                               where d.user_id == v.UserId
                                               && DbFunctions.TruncateTime(DbFunctions.AddHours(d.view_date, GMT)) == DbFunctions.TruncateTime(new DateTime(v.Year, v.Month, v.Day))
                                               group d by new { action = d.action } into g
                                               select new
                                               {
                                                   action = g.Key.action,
                                                   count = g.Count()
                                               };

                    //calculate usage
                    int tempCount = 0;
                    foreach (var w in Processing_PastUsage)
                    {
                        tempCount += (w.count * (w.action == 1 ? 1 : actionClickCount));
                    }

                    //check if usage is enough and process updating point if yes
                    if (tempCount >= maxCount)
                    {
                        //increment users 'point_balance'
                        var entityUser = from d in context.Users
                                         where d.id == v.UserId
                                         select d;

                        if (entityUser.Count() > 0)
                        {
                            var w = entityUser.First();

                            w.point_balance += userPointIncrement;
                            w.last_updated = DateTime.UtcNow; //in UTC

                            //create user transaction
                            UserTrxService.CreateTransaction(new UserTrx()
                            {
                                AccountFrom = "Adz",
                                AccountTo = w.id.ToString(),
                                Balance = w.point_balance.Value,
                                CreatedBy = 0,
                                CreditAmount = userPointIncrement,
                                Description = "Daily Point",
                                TransactionDate = new DateTime(v.Year, v.Month, v.Day).AddHours(-GMT).AddDays(1), //in UTC
                                TransactionMonth = v.Month,
                                TransactionYear = v.Year,
                                CreatedDate = DateTime.UtcNow,
                                UserId = w.id,
                            });
                        }
                        context.SaveChanges();
                    }

                    //set 'flag_processed' to true to all that day records
                    var Flagging_PastUsge = from d in context.CampaignHistories
                                            where d.user_id == v.UserId
                                            && DbFunctions.TruncateTime(DbFunctions.AddHours(d.view_date, GMT)) == DbFunctions.TruncateTime(new DateTime(v.Year, v.Month, v.Day))
                                            select d;

                    foreach (var w in Flagging_PastUsge)
                    {
                        w.flag_processed = true;
                        w.last_updated = DateTime.UtcNow;
                        if (tempCount >= maxCount)
                        {
                            w.remarks = "Success";
                        }
                        else
                        {
                            w.remarks = tempCount + " views is less than the required views, " + maxCount + ".";
                        }
                    }
                    context.SaveChanges();


                    if (tempCount >= maxCount)
                    {
                        result.Add(new KeyValuePair<int,DateTime>(v.UserId.Value, new DateTime(v.Year, v.Month, v.Day)));
                    }
                }

            }

            return new Response<List<KeyValuePair<int, DateTime>>> { Result = result };
        }

        public void SendVerificationEmail(string SendToEmail, string VerificationLink)
        {
            string EmailSubject = "Welcome to Adz! Please verify your account.";
            string EmailBody = Email.CreateVerificationEmail(VerificationLink);

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "mail.theadz.com";
            smtpClient.Port = 26;
            smtpClient.EnableSsl = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("report@theadz.com", "theadz12345");
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            email.From = new MailAddress("report@theadz.com", "The Adz");
            email.To.Add(SendToEmail);
            email.Subject = EmailSubject;
            email.IsBodyHtml = true;
            email.Body += EmailBody;
            try
            {
                smtpClient.Send(email);
            }
            catch
            {
                smtpClient.Port = 587;
                smtpClient.Send(email);
            }
        }

        public void SetGCMDeviceId(int UserId, string DeviceUniqueId, string GCMDeviceId)
        {
            using (var context = new TheAdzEntities())
            {
                var ettAPIKeys = from d in context.APIKeys
                                 where d.unique_id == DeviceUniqueId && d.user_id == UserId
                                 select d;

                if (ettAPIKeys.Count() > 0)
                {
                    if (string.IsNullOrWhiteSpace(ettAPIKeys.FirstOrDefault().gcm_device_id))
                    {
                        ettAPIKeys.FirstOrDefault().gcm_device_id = GCMDeviceId;
                        context.SaveChanges();
                    }
                }
            }
        }

        public void PushNotificationToAllDevice(PNobject PNobject)
        {
            using (var DBContext = new TheAdzEntities())
            {
                PushNotificationLog NewPushNotificationLog = new PushNotificationLog();
                NewPushNotificationLog.creationdate = DateTime.UtcNow;
                DBContext.PushNotificationLogs.Add(NewPushNotificationLog);
                DBContext.SaveChanges();

                PNobject.pn_log_id = NewPushNotificationLog.id;
                NewPushNotificationLog.content = JsonConvert.SerializeObject(PNobject);
                DBContext.SaveChanges();


                var UserAPIKeys = from d in DBContext.Users
                                  join e in DBContext.APIKeys on d.id equals e.user_id
                                  where d.locked_status == false
                                  && (d.userstatus_id == 1 || d.userstatus_id == 4 || d.userstatus_id == 5)
                                  select e;

                foreach (var v in UserAPIKeys)
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(v.gcm_device_id))
                        {
                            PushNotificationService.PushNotification(v.gcm_device_id, PNobject);
                        }
                    }
                    catch { }
                }

            }
        }
        
    }
}
