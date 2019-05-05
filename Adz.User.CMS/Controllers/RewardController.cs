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
    public class RewardController : BaseAdminController
    {
        public IRewardService rewardservice = new RewardService();

        [AdminAuthorize("REWARD", "VIEW")]
        public ActionResult Reward(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }

        public ActionResult Reward_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.Reward> listR = rewardservice.GetRewardList().Result;
            List<RewardModel.Reward> newlistR = new List<RewardModel.Reward>();
            foreach (var v in listR)
            {
                newlistR.Add(new RewardModel.Reward() { RewardId = v.RewardId, Name = v.Name, SponsorName = v.SponsorName, SubTitle = v.SubTitle, Description = v.Description, LastAction = v.LastAction, Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT), SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), RewardCriteria = new RewardModel.RewardCriteria() { CriteriaId = v.RewardCriteria.CriteriaId, Name = v.RewardCriteria.Name }, RewardType = new RewardModel.RewardType() { RewardTypeId = v.RewardType.RewardTypeId, RewardTypeName = v.RewardType.RewardTypeName }, PointRequirement = v.PointRequirement, OneTimeFlag = v.OneTimeFlag, NumberOfStock = v.NumberOfStock });
            }
            IEnumerable<RewardModel.Reward> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize("REWARD", "DELETE")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Reward_Destroy([DataSourceRequest] DataSourceRequest request, RewardModel.Reward Reward)
        {
            try
            {
                if (Reward != null)
                {
                    Boolean result = rewardservice.DeleteReward(Reward.RewardId).Result;
                }
            }
            catch (Exception ex)
            {
                CustomException(ex);
                ModelState.AddModelError("", ex.Message);
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [AdminAuthorize("REWARD", "ADD")]
        public String Reward_Duplicate(int RewardId)
        {
            string res = "";
            if (RewardId != null && RewardId > 0)
            {
                try
                {
                    Boolean result = rewardservice.DuplicateReward(RewardId).Result;

                    if (result) res = "1";
                    else res = "2";
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    res = ex.Message;
                }

            }
            return res;
        }
        [AdminAuthorize("REWARD", "VIEW")]
        public ActionResult RewardView(int RewardId, string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            Adz.BLL.Lib.Reward v = rewardservice.GetRewardById(RewardId).Result;

            RewardModel.Reward model = new RewardModel.Reward() { RewardId = v.RewardId, Name = v.Name, SponsorName = v.SponsorName, SubTitle = v.SubTitle, Description = v.Description, LastAction = v.LastAction, Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT), SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), RewardCriteria = new RewardModel.RewardCriteria() { CriteriaId = v.RewardCriteria.CriteriaId, Name = v.RewardCriteria.Name }, RewardType = new RewardModel.RewardType() { RewardTypeId = v.RewardType.RewardTypeId, RewardTypeName = v.RewardType.RewardTypeName }, PointRequirement = v.PointRequirement, OneTimeFlag = v.OneTimeFlag, NumberOfStock = v.NumberOfStock };

            return View(model);
        }

        public ActionResult Review_Read([DataSourceRequest] DataSourceRequest request, int ReviewId)
        {
            List<Adz.BLL.Lib.Review> listR = rewardservice.GetReviewListByRewardId(ReviewId).Result;
            List<RewardModel.Review> newlistR = new List<RewardModel.Review>();
            foreach (var v in listR)
            {
                newlistR.Add(new RewardModel.Review() { RedemptionId = v.RedemptionId, Message = v.Message, Rating = v.Rating, ReviewDate = v.ReviewDate, ByEmail = v.ByEmail });
            }
            IEnumerable<RewardModel.Review> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Review_Update([DataSourceRequest] DataSourceRequest request)
        {
            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Review_Destroy([DataSourceRequest] DataSourceRequest request)
        {
            return Json(ModelState.ToDataSourceResult());
        }

        [AdminAuthorize("REWARD", "EDIT")]
        public ActionResult RewardEdit(int RewardId)
        {
            Adz.BLL.Lib.Reward v = rewardservice.GetRewardById(RewardId).Result;

            RewardModel.Reward model = new RewardModel.Reward() { RewardId = v.RewardId, Name = v.Name, SponsorName = v.SponsorName, SubTitle = v.SubTitle, Description = v.Description, LastAction = v.LastAction, Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT), SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), RewardCriteria = new RewardModel.RewardCriteria() { CriteriaId = v.RewardCriteria.CriteriaId, Name = v.RewardCriteria.Name }, RewardType = new RewardModel.RewardType() { RewardTypeId = v.RewardType.RewardTypeId, RewardTypeName = v.RewardType.RewardTypeName }, PointRequirement = v.PointRequirement, OneTimeFlag = v.OneTimeFlag, NumberOfStock = v.NumberOfStock };

            return View(model);
        }
        [AdminAuthorize("REWARD", "EDIT")]
        [HttpPost]
        public ActionResult RewardEdit(FormCollection collection, RewardModel.Reward Reward)
        {
            if (collection.Get("criteriaDropDown") == null || collection.Get("criteriaDropDown") == "")
            {
                //Reward.RewardCriteria = new RewardModel.RewardCriteria();
                ModelState.AddModelError("CriteriaNull", "The Criteria field is required.");
            }

            if (collection.Get("RewardTypeDropDown") == null || collection.Get("RewardTypeDropDown") == "")
            {
                //Reward.RewardType = new RewardModel.RewardType();
                ModelState.AddModelError("RewardTypeNull", "The Type field is required.");
            }

            if (Reward.SubImageId == null || Reward.SubImageId == "" || Regex.IsMatch(Reward.SubImageId, @"^,+$") == true)
            {
                ModelState.AddModelError("ImageNull", "Image is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string tempDeleteId = collection.Get("tempDeleteId");
                    string tempEditId = collection.Get("tempEditId");
                    string tempEditRating = collection.Get("tempEditRating");
                    string tempEditMessage = collection.Get("tempEditMessage");

                    if (tempDeleteId != null && tempDeleteId != "")
                    {
                        foreach (var v in tempDeleteId.Split('|'))
                        {
                            if(v != ""){
                                rewardservice.DeleteReview(int.Parse(v));
                            }
                        }
                    }

                    if (tempEditId != null && tempEditId != "" && tempEditRating != null && tempEditRating != "" && tempEditMessage != null && tempEditMessage != "")
                    {
                        var tEditId = tempEditId.Split('|');
                        var tEditRating = tempEditRating.Split('|');
                        var tEditMessage = tempEditMessage.Split(new string[] { "#|#" }, StringSplitOptions.None);

                        for (int i = 0; i < tEditId.Length; i++)
                        {
                            if (tEditId[i] != "")
                            {
                                rewardservice.CreateEditReview(new Review() { RedemptionId = int.Parse(tEditId[i]), Message = tEditMessage[i], Rating = int.Parse(tEditRating[i]) }, true);
                            }
                        }
                    }


                    Adz.BLL.Lib.Reward mer = new Adz.BLL.Lib.Reward();
                    mer.RewardId = Reward.RewardId;
                    mer.Name = Reward.Name;
                    mer.SponsorName = Reward.SponsorName;
                    mer.SubTitle = Reward.SubTitle;
                    mer.Description = Reward.Description;
                    mer.OneTimeFlag = Reward.OneTimeFlag;
                    mer.NumberOfStock = Reward.NumberOfStock;
                    Adz.BLL.Lib.RewardCriteria rc = new Adz.BLL.Lib.RewardCriteria();
                    rc.CriteriaId = Convert.ToInt16(collection.Get("criteriaDropDown").ToString());
                    mer.RewardCriteria = rc;

                    Adz.BLL.Lib.RewardType rt = new Adz.BLL.Lib.RewardType();
                    rt.RewardTypeId = Convert.ToInt16(collection.Get("RewardTypeDropDown").ToString());
                    mer.RewardType = rt;
                    mer.PointRequirement = Reward.PointRequirement;

                    if (Reward.SubImageId != null && Reward.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = Reward.SubImageId.Split(',');
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

                    int result = rewardservice.CreateEditReward(mer).Result;
                    string r;
                    if (result != 0)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("RewardView", "Reward", new { RewardId = Reward.RewardId, r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Reward);
        }

        public JsonResult CriteriaList()
        {
            List<Adz.BLL.Lib.RewardCriteria> listR = rewardservice.GetRewardCriteriaList().Result;
            List<RewardModel.RewardCriteria> newlistR = new List<RewardModel.RewardCriteria>();
            foreach (var v in listR)
            {
                newlistR.Add(new RewardModel.RewardCriteria() { CriteriaId = v.CriteriaId, Name = v.Name });
            }
            IEnumerable<RewardModel.RewardCriteria> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RewardTypeList()
        {
            List<Adz.BLL.Lib.RewardType> listR = rewardservice.GetRewardTypeList().Result;
            List<RewardModel.RewardType> newlistR = new List<RewardModel.RewardType>();
            foreach (var v in listR)
            {
                string RewardTypeSubName = "";
                if (v.RewardTypeSubName != null && v.RewardTypeSubName != "")
                {
                    RewardTypeSubName = " (" + v.RewardTypeSubName + ")";
                }
                newlistR.Add(new RewardModel.RewardType() { RewardTypeId = v.RewardTypeId, RewardTypeName = v.RewardTypeName + RewardTypeSubName, Delivery = v.Delivery, Mobile = v.Mobile, MoneyTransfer = v.MoneyTransfer });
            }
            IEnumerable<RewardModel.RewardType> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize("REWARD", "VIEW")]
        public ActionResult RewardArchive()
        {
            return View();
        }

        [AdminAuthorize("REWARD", "ADD")]
        public ActionResult RewardAdd()
        {
            RewardModel.Reward model = new RewardModel.Reward();
            model.RewardCriteria = new RewardModel.RewardCriteria();
            model.RewardType = new RewardModel.RewardType();
            return View(model);
        }
        [AdminAuthorize("REWARD", "ADD")]
        [HttpPost]
        public ActionResult RewardAdd(FormCollection collection, RewardModel.Reward Reward)
        {
            if (collection.Get("criteriaDropDown") == null || collection.Get("criteriaDropDown") == "")
            {
                Reward.RewardCriteria = new RewardModel.RewardCriteria();
                ModelState.AddModelError("CriteriaNull", "The Criteria field is required.");
            }

            if (collection.Get("RewardTypeDropDown") == null || collection.Get("RewardTypeDropDown") == "")
            {
                Reward.RewardType = new RewardModel.RewardType();
                ModelState.AddModelError("RewardTypeNull", "The Type field is required.");
            }

            if (Reward.SubImageId == null || Reward.SubImageId == "" || Regex.IsMatch(Reward.SubImageId, @"^,+$") == true)
            {
                ModelState.AddModelError("ImageNull", "Image is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Reward mer = new Adz.BLL.Lib.Reward();
                    mer.RewardId = 0;
                    mer.Name = Reward.Name;
                    mer.SponsorName = Reward.SponsorName;
                    mer.SubTitle = Reward.SubTitle;
                    mer.Description = Reward.Description;
                    mer.OneTimeFlag = Reward.OneTimeFlag;
                    mer.NumberOfStock = Reward.NumberOfStock;
                    Adz.BLL.Lib.RewardCriteria rc = new Adz.BLL.Lib.RewardCriteria();
                    rc.CriteriaId = Convert.ToInt16(collection.Get("criteriaDropDown").ToString());
                    mer.RewardCriteria = rc;

                    Adz.BLL.Lib.RewardType rt = new Adz.BLL.Lib.RewardType();
                    rt.RewardTypeId = Convert.ToInt16(collection.Get("RewardTypeDropDown").ToString());
                    mer.RewardType = rt;
                    mer.PointRequirement = Reward.PointRequirement;

                    if (Reward.SubImageId != null && Reward.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = Reward.SubImageId.Split(',');
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

                    int result = rewardservice.CreateEditReward(mer).Result;
                    string r;
                    if (result != 0)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("Reward", "Reward", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Reward);
        }
	}
}