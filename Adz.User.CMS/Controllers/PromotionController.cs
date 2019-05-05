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
    public class PromotionController : BaseAdminController
    {
        public IPromotionService PromotionService = new PromotionService();

        public ActionResult Promotion(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }

        public ActionResult Promotion_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.Promotion> listR = PromotionService.GetPromotionList().Result;
            List<PromotionModel.Promotion> newlistR = new List<PromotionModel.Promotion>();
            foreach (var v in listR)
            {
                newlistR.Add(new PromotionModel.Promotion() { Create = v.Create.AddHours(Helper.defaultGMT), PromotionId = v.PromotionId, PromoCode = v.PromoCode, Value = v.Value, LastAction = v.LastAction = v.LastAction, Description = v.Description, OnScheduleFlag = v.OnScheduleFlag, StartAt = v.StartAt, EndAt = v.EndAt });
            }
            IEnumerable<PromotionModel.Promotion> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PromotionEdit(int PromotionId)
        {
            Adz.BLL.Lib.Promotion v = PromotionService.GetPromotionById(PromotionId).Result;

            PromotionModel.Promotion model = new PromotionModel.Promotion() { Create = v.Create.AddHours(Helper.defaultGMT), PromotionId = v.PromotionId, PromoCode = v.PromoCode, Value = v.Value, LastAction = v.LastAction = v.LastAction, Description = v.Description, OnScheduleFlag = v.OnScheduleFlag, StartAt = v.StartAt, EndAt = v.EndAt };

            return View(model);
        }

        [HttpPost]
        public ActionResult PromotionEdit(FormCollection collection, PromotionModel.Promotion Promotion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Promotion.StartAt > Promotion.EndAt)
                    {
                        throw new Exception("Start At cannot be greater that End At");
                    }

                    Adz.BLL.Lib.Promotion mer = new Adz.BLL.Lib.Promotion();
                    mer.PromotionId = Promotion.PromotionId;
                    mer.Description = Promotion.Description;
                    mer.Value = Promotion.Value;
                    mer.PromoCode = Promotion.PromoCode;

                    mer.OnScheduleFlag = Promotion.OnScheduleFlag;
                    mer.StartAt = Promotion.StartAt.Value.Date;
                    mer.EndAt = Promotion.EndAt.Value.Date;

                    int result = PromotionService.CreateEditPromotion(mer).Result;
                    string r;
                    if (result != 0)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("Promotion", "Promotion", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Promotion);
        }

        public ActionResult PromotionAdd()
        {
            PromotionModel.Promotion model = new PromotionModel.Promotion();
            model.StartAt = DateTime.Now;
            model.EndAt = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult PromotionAdd(FormCollection collection, PromotionModel.Promotion Promotion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Promotion.StartAt > Promotion.EndAt)
                    {
                        throw new Exception("Start At cannot be greater that End At");
                    }

                    Adz.BLL.Lib.Promotion mer = new Adz.BLL.Lib.Promotion();
                    mer.PromotionId = 0;
                    mer.Description = Promotion.Description;
                    mer.Value = Promotion.Value;
                    mer.PromoCode = Promotion.PromoCode;

                    mer.OnScheduleFlag = Promotion.OnScheduleFlag;
                    mer.StartAt = Promotion.StartAt.Value.Date;
                    mer.EndAt = Promotion.EndAt.Value.Date;
                    
                    int result = PromotionService.CreateEditPromotion(mer).Result;

                    string r;
                    if (result != 0)
                    {
                        r = "a1";
                    }
                    else
                    {
                        r = "a2";
                    }
                    return RedirectToAction("Promotion", "Promotion", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Promotion);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Promotion_Destroy([DataSourceRequest] DataSourceRequest request, PromotionModel.Promotion Promotion)
        {
            try
            {
                if (Promotion != null)
                {
                    Boolean result = PromotionService.DeletePromotion(Promotion.PromotionId).Result;
                }
            }
            catch (Exception ex)
            {
                CustomException(ex);
                ModelState.AddModelError("", ex.Message);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult PromotionRun(int PromotionId)
        {
            try
            {
                var result = PromotionService.RunPromotion(PromotionId).Result;
                string r;
                if (result == true)
                {
                    r = "e1";
                }
                else
                {
                    r = "e2";
                }
                return RedirectToAction("Promotion", "Promotion", new { r = r });
            }
            catch (Exception ex)
            {
                string r = ex.Message;
                return RedirectToAction("Promotion", "Promotion", new { r = r });
            }
        }

        public ActionResult PromotionStop(int PromotionId)
        {
            try
            {
                var result = PromotionService.StopPromotion(PromotionId).Result;
                string r;
                if (result == true)
                {
                    r = "e1";
                }
                else
                {
                    r = "e2";
                }
                return RedirectToAction("Promotion", "Promotion", new { r = r });
            }
            catch (Exception ex)
            {
                string r = ex.Message;
                return RedirectToAction("Promotion", "Promotion", new { r = r });
            }
        }

	}
}