using Adz.BLL.Lib;
using Adz.User.CMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Adz.User.CMS.Controllers
{
    public class UserController : BaseAdminController
    {
        public IUserService userservice = new UserService();

        [AdminAuthorize("USER", "VIEW")]
        public ActionResult User(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }

        public ActionResult User_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.User> listR = userservice.GetUserList().Result;
            List<UserModel.User> newlistR = new List<UserModel.User>();
            string gender = "";
            foreach (var v in listR)
            {
                if (v.Gender == 1) gender = "Male";
                else if (v.Gender == 2) gender = "Female";
                newlistR.Add(new UserModel.User() { Create = v.Create.AddHours(Helper.defaultGMT), UserId = v.UserId, FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, Gender = v.Gender, GenderStr = gender, DateOfBirth = v.DateOfBirth, LastLockout = v.LastLockout, LastLockoutDate = v.LastLockoutDate, UserStatus = new UserModel.UserStatus() { UserStatusId = v.UserStatus.UserStatusId, UserStatusName = v.UserStatus.UserStatusName }, ContactNumber = v.ContactNumber });
            }
            IEnumerable<UserModel.User> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize("USER", "VIEW")]
        public ActionResult UserView(int UserId, string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            Adz.BLL.Lib.User v = userservice.GetUserById(UserId, null);
            UserModel.User model;
            string gender = "";
            if (v.Gender == 1) gender = "Male";
            else if (v.Gender == 2) gender = "Female";

            model = new UserModel.User() { UserId = v.UserId, FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, Gender = v.Gender, GenderStr = gender, DateOfBirth = v.DateOfBirth, UserStatus = new UserModel.UserStatus() { UserStatusId = v.UserStatus.UserStatusId, UserStatusName = v.UserStatus.UserStatusName }, SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), PointBalance = v.PointBalance, ReferralCode = v.ReferralCode, ReferredBy = v.ReferredBy == null ? "-" : v.ReferredBy, ContactNumber = v.ContactNumber };

            return View(model);
        }

        [AdminAuthorize("USER", "EDIT")]
        public ActionResult UserEdit(int UserId)
        {
            Adz.BLL.Lib.User v = userservice.GetUserById(UserId, null);
            UserModel.User model;
            string gender = "";
            if (v.Gender == 1) gender = "Male";
            else if (v.Gender == 2) gender = "Female";

            model = new UserModel.User() { UserId = v.UserId, FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, Gender = v.Gender, GenderStr = gender, DateOfBirth = v.DateOfBirth, UserStatus = new UserModel.UserStatus() { UserStatusId = v.UserStatus.UserStatusId, UserStatusName = v.UserStatus.UserStatusName }, SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), PointBalance = v.PointBalance, ReferralCode = v.ReferralCode, ReferredBy = v.ReferredBy == null ? "-" : v.ReferredBy, ContactNumber = v.ContactNumber };

            return View(model);
        }

        [AdminAuthorize("USER", "EDIT")]
        [HttpPost]
        public ActionResult UserEdit(FormCollection collection, UserModel.User User)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.User mer = new Adz.BLL.Lib.User();
                    mer.UserId = User.UserId;
                    mer.FirstName = User.FirstName;
                    mer.LastName = User.LastName;
                    mer.Email = User.Email;
                    mer.Password = User.Password;
                    mer.Gender = User.Gender;
                    mer.ContactNumber = User.ContactNumber;
                    mer.DateOfBirth = User.DateOfBirth;
                    Adz.BLL.Lib.UserStatus status = new Adz.BLL.Lib.UserStatus();
                    status.UserStatusId = User.UserStatus.UserStatusId;
                    mer.UserStatus = status;

                    if (User.SubImageId != null && User.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = User.SubImageId.Split(',');
                        foreach (var v in subimagelist)
                        {
                            if (v != "")
                            {
                                subimg.Add(int.Parse(v));
                                subimglink.Add(collection.Get("txtSubImg" + v));
                            }
                        }
                        mer.SubImageId = subimg;
                        mer.SubImageUrlLink = subimglink;
                    }

                    mer.PointBalance = User.PointBalance;

                    bool result = userservice.CreateEditUser(mer).Result;
                    string r;
                    if (result == true)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("UserView", "User", new { UserId = User.UserId, r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(User);
        }

        [AdminAuthorize("USER", "ADD")]
        public ActionResult UserAdd()
        {
            UserModel.User model = new UserModel.User();
            model.Password = "";
            return View(model);
        }

        [AdminAuthorize("USER", "ADD")]
        [HttpPost]
        public ActionResult UserAdd(FormCollection collection, UserModel.User User)
        {
            if (User.Password == null || User.Password == "")
            {
                ModelState.AddModelError("PasswordNull", "The Password field is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.User mer = new Adz.BLL.Lib.User();
                    mer.UserId = User.UserId;
                    mer.FirstName = User.FirstName;
                    mer.LastName = User.LastName;
                    mer.Email = User.Email;
                    mer.Password = User.Password;
                    mer.Gender = User.Gender;
                    mer.DateOfBirth = User.DateOfBirth;
                    mer.ContactNumber = User.ContactNumber;
                    Adz.BLL.Lib.UserStatus status = new Adz.BLL.Lib.UserStatus();
                    status.UserStatusId = User.UserStatus.UserStatusId;
                    mer.UserStatus = status;
                    
                    if (User.SubImageId != null && User.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = User.SubImageId.Split(',');
                        foreach (var v in subimagelist)
                        {
                            if (v != "")
                            {
                                subimg.Add(int.Parse(v));
                                subimglink.Add(collection.Get("txtSubImg" + v));
                            }
                        }
                        mer.SubImageId = subimg;
                        mer.SubImageUrlLink = subimglink;
                    }

                    mer.PointBalance = User.PointBalance;

                    bool result = userservice.CreateEditUser(mer).Result;
                    string r;
                    if (result == true)
                    {
                        r = "a1";
                    }
                    else
                    {
                        r = "a2";
                    }
                    return RedirectToAction("User", "User", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(User);
        }

        public JsonResult UserStatusList()
        {
            List<Adz.BLL.Lib.UserStatus> listR = userservice.GetUserStatusList().Result;
            List<UserModel.UserStatus> newlistR = new List<UserModel.UserStatus>();
            foreach (var v in listR)
            {
                newlistR.Add(new UserModel.UserStatus() { UserStatusId = v.UserStatusId, UserStatusName = v.UserStatusName });
            }
            IEnumerable<UserModel.UserStatus> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult UserDownloadCSV()
        {
            List<Adz.BLL.Lib.User> listR = userservice.GetUserList().Result;
            List<UserModel.UserReport> newlistR = new List<UserModel.UserReport>();
            string gender = "";
            foreach (var v in listR)
            {
                if (v.Gender == 1) gender = "Male";
                else if (v.Gender == 2) gender = "Female";
                newlistR.Add(new UserModel.UserReport() { Create = v.Create.AddHours(Helper.defaultGMT), UserId = v.UserId, FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, Gender = gender, ContactNumber = v.ContactNumber, DateOfBirth = v.DateOfBirth != DateTime.MinValue ? v.DateOfBirth.ToString("dd MMMM yyyy") : "", Status = v.UserStatus.UserStatusName, PointBalance = v.PointBalance, ReferralCode = v.ReferralCode, ReferredBy = v.ReferredBy });
            }

            string csv = Helper.GetCSV(newlistR);
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "UserReport-" + DateTime.Now.ToString("yyyyMMdd") + ".csv");
        }

        public ActionResult Campaign_Statistic_By_User_Read([DataSourceRequest] DataSourceRequest request, int UserId)
        {
            //var listR = new CampaignService().GetCampaignHistoryGroupByDateAndUser(UserId).Result;
            var listR = new UserTrxService().GetTransactionByUser(UserId);
            List<CampaignModel.CampaignHistoryGroupByDate> newlistR = new List<CampaignModel.CampaignHistoryGroupByDate>();
            foreach (var v in listR)
            {
                newlistR.Add(new CampaignModel.CampaignHistoryGroupByDate()
                {
                    //Date = v.Date,
                    //Views = v.Views,
                    //Clicks = v.Clicks,
                    //isGetPoint = v.isGetPoint,

                    //Description = v.Views + " views and " + v.Clicks + " clicks",
                    //Points = v.isGetPoint == true ? "50 points" : "0 point"

                    Date = v.TransactionDate,
                    Views = 0,
                    Clicks = 0,
                    isGetPoint = v.Description == "Success",

                    Description = v.Description,
                    Points = v.CreditAmount > 0 ? v.CreditAmount.ToString() : "-" + v.DebitAmount
                });
            }
            IEnumerable<CampaignModel.CampaignHistoryGroupByDate> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

	}
}