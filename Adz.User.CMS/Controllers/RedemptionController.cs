using Adz.BLL.Lib;
using Adz.User.CMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Adz.User.CMS.Controllers
{
    public class RedemptionController : BaseAdminController
    {
        public IRewardService rewardservice = new RewardService();

        public ActionResult Redemption(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }

        public ActionResult Redemption_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.Redemption> listR = rewardservice.GetRedemptionList().Result;
            List<RewardModel.Redemption> newlistR = new List<RewardModel.Redemption>();
            foreach (var v in listR)
            {
                newlistR.Add(new RewardModel.Redemption()
                {
                    RedemptionId = v.RedemptionId,
                    UserEmail = v.UserEmail,
                    RewardName = v.Reward.Name,
                    RewardPointRequirement = v.Reward.PointRequirement,
                    RedemptionDate = v.RedemptionDate,
                    RedemptionStatusId = v.RedemptionStatus.Id,
                    RedemptionStatusName = v.RedemptionStatus.Name,
                    RewardId = v.Reward.RewardId
                });
            }
            IEnumerable<RewardModel.Redemption> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Redemption_Update([DataSourceRequest] DataSourceRequest request, RewardModel.Redemption Redemption, int NewStatusId)
        {
            if (Redemption != null && NewStatusId != 0)
            {
                rewardservice.UpdateRedemptionStatus(Redemption.RedemptionId, NewStatusId);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public JsonResult RedemptionStatusList()
        {
            List<RewardModel.RedemptionStatus> newlistR = new List<RewardModel.RedemptionStatus>();
            foreach (var v in rewardservice.GetRedemptionStatusList().Result)
            {
                newlistR.Add(new RewardModel.RedemptionStatus() { Id = v.Id, Name = v.Name });
            }
            IEnumerable<RewardModel.RedemptionStatus> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RedemptionView(int RedemptionId, string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            Adz.BLL.Lib.Redemption v = rewardservice.GetRedemptionById(RedemptionId).Result;

            RewardModel.RedemptionVM model = new RewardModel.RedemptionVM();

            model.Redemption = new RewardModel.Redemption()
            {
                RedemptionId = v.RedemptionId,
                RewardName = v.Reward.Name,
                RewardPointRequirement = v.Reward.PointRequirement,
                Name = v.Name,
                RedemptionDate = v.RedemptionDate,
                RedemptionStatusId = v.RedemptionStatus.Id,
                RedemptionStatusName = v.RedemptionStatus.Name,
                AddressLine1 = v.AddressLine1,
                AddressLine2 = v.AddressLine2,
                City = v.City,
                State = v.State,
                Country = v.Country,
                PostCode = v.PostCode,
                BankName = v.BankName,
                BankAccountNum = v.BankAccountNum,
                BankAccountName = v.BankAccountName,
                MobileAccNum = v.MobileAccNum,
                MobileOperatorName = v.MobileOperator.Name,
                UserEmail = v.UserEmail
            };

            if (v.RedemptionStatus.Id == (int)RedemptionStatusCode.Completed)
            {
                try
                {
                    Adz.BLL.Lib.Review v2 = rewardservice.GetReviewByRedemptionId(RedemptionId).Result;

                    model.Review = new RewardModel.Review()
                    {
                        RedemptionId = v2.RedemptionId,
                        ByEmail = v2.ByEmail,
                        Message = v2.Message,
                        Rating = v2.Rating,
                        ReviewDate = v2.ReviewDate,
                        LastAction = v2.LastAction
                    };
                }
                catch (NullReferenceException e)
                {
                    model.Review = null;
                }
            }

            return View(model);
        }

        public JsonResult StatusList()
        {
            List<Adz.BLL.Lib.RedemptionStatus> listR = rewardservice.GetRedemptionStatusList().Result;
            List<RewardModel.RedemptionStatus> newlistR = new List<RewardModel.RedemptionStatus>();
            foreach (var v in listR)
            {
                newlistR.Add(new RewardModel.RedemptionStatus() { Id = v.Id, Name = v.Name });
            }
            IEnumerable<RewardModel.RedemptionStatus> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MobileOperatorList()
        {
            List<Adz.BLL.Lib.MobileOperator> listR = rewardservice.GetMobileOperatorList().Result;
            List<RewardModel.MobileOperator> newlistR = new List<RewardModel.MobileOperator>();
            foreach (var v in listR)
            {
                newlistR.Add(new RewardModel.MobileOperator() { Id = v.Id, Name = v.Name });
            }
            IEnumerable<RewardModel.MobileOperator> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RedemptionEdit(int RedemptionId)
        {

            Adz.BLL.Lib.Redemption v = rewardservice.GetRedemptionById(RedemptionId).Result;

            RewardModel.RedemptionVM model = new RewardModel.RedemptionVM();

            model.Redemption = new RewardModel.Redemption()
            {
                RedemptionId = v.RedemptionId,
                RewardName = v.Reward.Name,
                RewardPointRequirement = v.Reward.PointRequirement,
                Name = v.Name,
                RedemptionDate = v.RedemptionDate,
                RedemptionStatusId = v.RedemptionStatus.Id,
                RedemptionStatusName = v.RedemptionStatus.Name,
                AddressLine1 = v.AddressLine1,
                AddressLine2 = v.AddressLine2,
                City = v.City,
                State = v.State,
                Country = v.Country,
                PostCode = v.PostCode,
                BankName = v.BankName,
                BankAccountNum = v.BankAccountNum,
                BankAccountName = v.BankAccountName,
                MobileAccNum = v.MobileAccNum,
                MobileOperatorName = v.MobileOperator.Name,
                MobileOperatorId = v.MobileOperator.Id,
                UserEmail = v.UserEmail
            };

            ViewBag.BoolDelivery = "false";
            ViewBag.BoolMoney = "false";
            ViewBag.BoolMobile = "false";
            ViewBag.BoolReview = "false";

            /*if (model.Redemption.AddressLine1 != null && model.Redemption.AddressLine1 != "")
            {
                ViewBag.BoolDelivery = "true";
            }
            if (model.Redemption.BankName != null && model.Redemption.BankName != "")
            {
                ViewBag.BoolMoney = "true";
            }*/
            if (model.Redemption.MobileOperatorId != 0 && model.Redemption.MobileOperatorId != null)
            {
                ViewBag.BoolMobile = "true";
            }
            
            try
            {
                Adz.BLL.Lib.Review v2 = rewardservice.GetReviewByRedemptionId(RedemptionId).Result;

                model.Review = new RewardModel.Review()
                {
                    RedemptionId = v2.RedemptionId,
                    ByEmail = v2.ByEmail,
                    Message = v2.Message,
                    Rating = v2.Rating,
                    ReviewDate = v2.ReviewDate,
                    LastAction = v2.LastAction
                };
                ViewBag.BoolReview = "true";
            }
            catch (NullReferenceException e)
            {
                model.Review = null;
            }

            ViewBag.RedemptionStatusCompleted = (int)RedemptionStatusCode.Completed;

            return View(model);
        }

        [HttpPost]
        public ActionResult RedemptionEdit(FormCollection collection, RewardModel.RedemptionVM RedemptionVM)
        {
            //if (collection.Get("BoolMobile") == "true" && (collection.Get("MobileOperatorDropDown") == null || collection.Get("MobileOperatorDropDown") == ""))
            //{
            //    ModelState.AddModelError("MobileOperatorNull", "The MobileOperator field is required.");
            //}

            //if (collection.Get("StatusDropDown") == null || collection.Get("StatusDropDown") == "")
            //{
            //    ModelState.AddModelError("StatusNull", "The Status field is required.");
            //}

            if (collection.Get("BoolReview") != "true")
            {
                ModelState.Where(m => m.Key.Contains("Review")).FirstOrDefault().Value.Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Redemption mer = new Adz.BLL.Lib.Redemption();
                    mer.RedemptionId = RedemptionVM.Redemption.RedemptionId;
                    mer.UserEmail = RedemptionVM.Redemption.UserEmail;
                    mer.Name = RedemptionVM.Redemption.Name;

                    //Adz.BLL.Lib.RedemptionStatus rs = new Adz.BLL.Lib.RedemptionStatus();
                    //rs.Id = Convert.ToInt16(collection.Get("StatusDropDown").ToString());
                    //mer.RedemptionStatus = rs;


                    mer.MobileAccNum = RedemptionVM.Redemption.MobileAccNum;
                    Adz.BLL.Lib.MobileOperator mo = new Adz.BLL.Lib.MobileOperator();
                    mo.Id = 1; // Convert.ToInt16(collection.Get("MobileOperatorDropDown").ToString());
                    mer.MobileOperator = mo;
                    /*if (collection.Get("BoolDelivery") == "true")
                    {
                        mer.AddressLine1 = RedemptionVM.Redemption.AddressLine1;
                        mer.AddressLine2 = RedemptionVM.Redemption.AddressLine2;
                        mer.City = RedemptionVM.Redemption.City;
                        mer.State = RedemptionVM.Redemption.State;
                        mer.Country = RedemptionVM.Redemption.Country;
                        mer.PostCode = RedemptionVM.Redemption.PostCode;
                    }

                    if (collection.Get("BoolMoney") == "true")
                    {
                        mer.BankName = RedemptionVM.Redemption.BankName;
                        mer.BankAccountName = RedemptionVM.Redemption.BankAccountName;
                        mer.BankAccountNum = RedemptionVM.Redemption.BankAccountNum;
                    }

                    if (collection.Get("BoolMobile") == "true")
                    {
                        mer.MobileAccNum = RedemptionVM.Redemption.MobileAccNum;
                        Adz.BLL.Lib.MobileOperator mo = new Adz.BLL.Lib.MobileOperator();
                        mo.Id = Convert.ToInt16(collection.Get("MobileOperatorDropDown").ToString());
                        mer.MobileOperator = mo;
                    }*/

                    int result = rewardservice.CreateEditRedemption(mer).Result;

                    int result2 = 0;
                    if (/*mer.RedemptionStatus.Id == (int)RedemptionStatusCode.Completed &&*/ RedemptionVM.Redemption.RedemptionStatusId == (int)RedemptionStatusCode.Completed && collection.Get("BoolReview") == "true")
                    {
                        Adz.BLL.Lib.Review it = new Adz.BLL.Lib.Review();
                        it.RedemptionId = RedemptionVM.Redemption.RedemptionId;
                        it.Message = RedemptionVM.Review.Message;
                        it.Rating = RedemptionVM.Review.Rating;

                        result2 = rewardservice.CreateEditReview(it, true).Result;
                    }
                    else
                    {
                        result2 = 1;
                    }

                    string r;
                    if (result != 0 && result2 != 0)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }

                    return RedirectToAction("RedemptionView", "Redemption", new { RedemptionId = RedemptionVM.Redemption.RedemptionId, r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            else
            {
                var t = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            ViewBag.BoolDelivery = collection.Get("BoolDelivery");
            ViewBag.BoolMoney = collection.Get("BoolMoney");
            ViewBag.BoolMobile = collection.Get("BoolMobile");
            ViewBag.BoolReview = collection.Get("BoolReview");

            ViewBag.RedemptionStatusCompleted = (int)RedemptionStatusCode.Completed;

            return View(RedemptionVM);
        }
        
        ///////
        public ActionResult Review_Read([DataSourceRequest] DataSourceRequest request, int RewardId)
        {
            List<Adz.BLL.Lib.Review> listR = rewardservice.GetReviewListByRewardId(RewardId).Result;
            List<RewardModel.Review> newlistR = new List<RewardModel.Review>();
            foreach (var v in listR)
            {
                newlistR.Add(new RewardModel.Review()
                {
                    RedemptionId = v.RedemptionId,
                    ByEmail = v.ByEmail,
                    Message = v.Message,
                    Rating = v.Rating,
                    ReviewDate = v.ReviewDate,
                    LastAction = v.LastAction
                });
            }
            IEnumerable<RewardModel.Review> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

	}
}