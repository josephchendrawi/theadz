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
    public class ReportingController : BaseAdminController
    {
        public ICampaignService campaignservice = new CampaignService();

        public ActionResult AdsPerformanceUsage()
        {
            ViewModels.AdsPerformanceUsageVM model = new ViewModels.AdsPerformanceUsageVM();

            //model.CampaignHistories = new List<CampaignModel.CampaignHistory>();
            model.CampaignHistoryGroup = new List<CampaignModel.CampaignHistoryGroup>();

            //foreach (var v in campaignservice.GetCampaignHistory().Result)
            //{
            //    model.CampaignHistories.Add(new CampaignModel.CampaignHistory() { Campaign = new CampaignModel.Campaign() { CampaignId = v.Campaign.CampaignId, Name = v.Campaign.Name }, User = new UserModel.User() { UserId = v.User.UserId, Email = v.User.Email }, ViewTime = v.ViewTime, Gender = v.Gender, Latitude = v.Latitude, Longitude = v.Longitude });
            //}
            foreach (var v in campaignservice.GetCampaignHistoryGroup().Result)
            {
                model.CampaignHistoryGroup.Add(new CampaignModel.CampaignHistoryGroup() { Campaign = new CampaignModel.Campaign() { CampaignId = v.Campaign.CampaignId, Name = v.Campaign.Name }, Views = v.Views, LastViewTime = v.LastViewTime, Clicks = v.Clicks, LastClickTime = v.LastClickTime });
            }

            return View(model);
        }
        
        public ActionResult AdsViewPerformanceUsage_Read([DataSourceRequest] DataSourceRequest request)
        {
            var newlistR = new List<CampaignModel.CampaignHistory>();
            foreach (var v in campaignservice.GetCampaignHistory((int)CampaignHistoryAction.View).Result)
            {
                newlistR.Add(new CampaignModel.CampaignHistory() { Campaign = new CampaignModel.Campaign() { CampaignId = v.Campaign.CampaignId, Name = v.Campaign.Name }, User = new UserModel.User() { UserId = v.User.UserId, Email = v.User.Email }, DateTime = v.DateTime, Gender = v.Gender == 1 ? "Male" : v.Gender == 2 ? "Female" : "Undisclosed", Latitude = v.Latitude, Longitude = v.Longitude });
            }
            IEnumerable<CampaignModel.CampaignHistory> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdsClickPerformanceUsage_Read([DataSourceRequest] DataSourceRequest request)
        {
            var newlistR = new List<CampaignModel.CampaignHistory>();
            foreach (var v in campaignservice.GetCampaignHistory((int)CampaignHistoryAction.Click).Result)
            {
                newlistR.Add(new CampaignModel.CampaignHistory() { Campaign = new CampaignModel.Campaign() { CampaignId = v.Campaign.CampaignId, Name = v.Campaign.Name }, User = new UserModel.User() { UserId = v.User.UserId, Email = v.User.Email }, DateTime = v.DateTime, Gender = v.Gender == 1 ? "Male" : v.Gender == 2 ? "Female" : "Undisclosed", Latitude = v.Latitude, Longitude = v.Longitude });
            }
            IEnumerable<CampaignModel.CampaignHistory> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdsPerfUsageByCampaign(int campaignId)
        {
            ViewModels.AdsPerformanceUsageVM model = new ViewModels.AdsPerformanceUsageVM();
            model.CampaignHistories = new List<CampaignModel.CampaignHistory>();
            foreach (var v in campaignservice.GetCampaignHistoryByCampaignId(campaignId, 0).Result)
            {
                model.CampaignHistories.Add(new CampaignModel.CampaignHistory() { Campaign = new CampaignModel.Campaign() { CampaignId = v.Campaign.CampaignId, Name = v.Campaign.Name }, User = new UserModel.User() { UserId = v.User.UserId, Email = v.User.Email }, DateTime = v.DateTime, Gender = v.Gender == 1 ? "Male" : v.Gender == 2 ? "Female" : "Undisclosed", Latitude = v.Latitude, Longitude = v.Longitude, Action = v.Action == 1 ? "View" : "Click" });
            }
            return View(model);
        }

        public ActionResult AdsPerformanceDetail(int CampaignId)
        {
            ViewBag.CampaignId = CampaignId;
            return View();
        }

        public ActionResult AdsPerformanceUsageDetail_Read([DataSourceRequest] DataSourceRequest request, int CampaignId)
        {
            var newlistR = new List<CampaignModel.CampaignHistory>();
            foreach (var v in campaignservice.GetCampaignHistoryByCampaignId(CampaignId, 0).Result)
            {
                int Age = -1;
                if (v.User.DateOfBirth > DateTime.MinValue)
                {
                    Age = DateTime.Today.Year - v.User.DateOfBirth.Year;
                    if (v.User.DateOfBirth > DateTime.Today.AddYears(-Age)) Age--;
                }
                newlistR.Add(new CampaignModel.CampaignHistory() { Campaign = new CampaignModel.Campaign() { CampaignId = v.Campaign.CampaignId, Name = v.Campaign.Name }, User = new UserModel.User() { UserId = v.User.UserId, Email = v.User.Email, Age = Age }, DateTime = v.DateTime, Gender = v.Gender == 1 ? "Male" : v.Gender == 2 ? "Female" : "Undisclosed", Latitude = v.Latitude, Longitude = v.Longitude, Action = v.Action == 1 ? "View" : ( v.Action == 2 ? "Click" : "N/A") });
            }
            IEnumerable<CampaignModel.CampaignHistory> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdsPerformanceUsageDetailSummarize_Read([DataSourceRequest] DataSourceRequest request, int CampaignId)
        {
            var newlistR = new List<CampaignModel.CampaignHistoryGroupSummarize>();
            var listR = campaignservice.GetCampaignHistoryByCampaignId(CampaignId, 0).Result.Select(
                            x => new
                            {
                                Action = x.Action,
                                Date = x.DateTime.Date,
                                Hour = x.DateTime.AddMinutes(-1).Hour % 2 != 0 ? x.DateTime.AddMinutes(-1).Hour + 1 : x.DateTime.AddMinutes(-1).Hour
                            }
                        ).GroupBy(x => new { x.Date, x.Hour });
            foreach (var v in listR)
            {
                newlistR.Add(new CampaignModel.CampaignHistoryGroupSummarize() { Date = v.Key.Date, HourGroup = (v.Key.Hour - 2) + ":01 - " + v.Key.Hour + ":00", Views = v.Where(x => x.Action == 1).Count(), Clicks = v.Where(x => x.Action == 2).Count() });
            }
            IEnumerable<CampaignModel.CampaignHistoryGroupSummarize> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterMenuCustomization_Genders()
        {
            List<String> Genders = new List<string>();
            Genders.Add("Male");
            Genders.Add("Female");
            Genders.Add("Undisclosed");
            return Json(Genders, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterMenuCustomization_AgeGroup()
        {
            List<String> AgeGroup = new List<string>();
            AgeGroup.Add("0");
            AgeGroup.Add("10");
            AgeGroup.Add("20");
            AgeGroup.Add("30");
            AgeGroup.Add("40");
            AgeGroup.Add("50");
            return Json(AgeGroup, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdsPerformanceUsageDetailGraph_Read([DataSourceRequest] DataSourceRequest request, int CampaignId)
        {
            List<CampaignModel.CampaignChart> model = new List<CampaignModel.CampaignChart>();

            var data = campaignservice.GetCampaignHistoryByCampaignId(CampaignId, 0).Result;

            for (int i = 7; i >= 1; i--)
            {
                DateTime iDate = DateTime.Now.AddDays(-i);
                int iViews = data.Where(d => d.DateTime.Day == iDate.Day && d.DateTime.Month == iDate.Month && d.DateTime.Year == iDate.Year && d.Action == 1).Count();
                int iClicks = data.Where(d => d.DateTime.Day == iDate.Day && d.DateTime.Month == iDate.Month && d.DateTime.Year == iDate.Year && d.Action == 2).Count();

                model.Add(new CampaignModel.CampaignChart() { Clicks = iClicks, Views = iViews, Date = iDate });
            }
            
            IEnumerable<CampaignModel.CampaignChart> chartData = model;
            return Json(chartData);
        }

        public ActionResult AdsPerformanceUsageDetailAgePie_Read([DataSourceRequest] DataSourceRequest request, int CampaignId)
        {
            List<CampaignModel.CampaignPieChart> model = new List<CampaignModel.CampaignPieChart>();
            int Year = DateTime.UtcNow.Year;

            int[] Count = {0,0,0,0,0,0,0};

            foreach(var v in campaignservice.GetCampaignHistoryByCampaignId(CampaignId, 0).Result)
            {
                if(Year - v.User.DateOfBirth.Year < 10)
                {
                    Count[1]++;
                }
                else if (Year - v.User.DateOfBirth.Year < 20)
                {
                    Count[2]++;
                }
                else if (Year - v.User.DateOfBirth.Year < 30)
                {
                    Count[3]++;
                }
                else if (Year - v.User.DateOfBirth.Year < 40)
                {
                    Count[4]++;
                }
                else if (Year - v.User.DateOfBirth.Year < 50)
                {
                    Count[5]++;
                }
                else if (Year - v.User.DateOfBirth.Year < 60)
                {
                    Count[6]++;
                }
                else
                {
                    Count[0]++;
                }
            }

            model.Add(new CampaignModel.CampaignPieChart() { Category = "0 - 10", Color = "#0AAF9F", Value = Count[1] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "11 - 20", Color = "#ABD825", Value = Count[2] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "21 - 30", Color = "#29AAE3", Value = Count[3] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "31 - 40", Color = "#DD3745", Value = Count[4] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "41 - 50", Color = "#F58344", Value = Count[5] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "51 - 60", Color = "#FAE25E", Value = Count[6] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "Unknown", Color = "#888888", Value = Count[0] });

            IEnumerable<CampaignModel.CampaignPieChart> chartData = model;
            return Json(model);
        }

        public ActionResult AdsPerformanceUsageDetailGenderPie_Read([DataSourceRequest] DataSourceRequest request, int CampaignId)
        {
            List<CampaignModel.CampaignPieChart> model = new List<CampaignModel.CampaignPieChart>();

            int[] Count = { 0, 0, 0 };

            foreach (var v in campaignservice.GetCampaignHistoryByCampaignId(CampaignId, 0).Result)
            {
                if (v.Gender == 1)
                {
                    Count[1]++;
                }
                else if (v.Gender == 2)
                {
                    Count[2]++;
                }
                else
                {
                    Count[0]++;
                }
            }

            model.Add(new CampaignModel.CampaignPieChart() { Category = "Male", Color = "#29AAE3", Value = Count[1] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "Female", Color = "#DD3745", Value = Count[2] });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "Unknown", Color = "#888888", Value = Count[0] });

            IEnumerable<CampaignModel.CampaignPieChart> chartData = model;
            return Json(model);
        }

        public ActionResult OverallUserStatus()
        {
            return View();
        }

        public ActionResult DaysActiveGraph_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<CampaignModel.OverallChart> model = new List<CampaignModel.OverallChart>();

            List<int> XDaysActiveList = new List<int>();
            XDaysActiveList.Add(1);
            XDaysActiveList.Add(3);
            XDaysActiveList.Add(5);
            XDaysActiveList.Add(7);
            XDaysActiveList.Add(10);

            var data = campaignservice.GetSensiteGraphValueByActiveDays(XDaysActiveList);

            for (int i = 0; i < XDaysActiveList.Count(); i++)
            {
                model.Add(new CampaignModel.OverallChart()
                {
                    XDaysActive = XDaysActiveList[i],
                    SensiteTotalUser = (decimal)Math.Round(data.SensiteTotalUser[i], 2),
                    AverageDaysActive = (decimal)Math.Round(data.AverageDaysActive[i], 2),
                    AverageDailyViews = (decimal)Math.Round(data.AverageDaysView[i], 2),
                    AverageCTRinPercent = (decimal)Math.Round(data.AverageCTR[i] * 100, 2),
                });
            }

            IEnumerable<CampaignModel.OverallChart> chartData = model;
            return Json(chartData);
        }

        public ActionResult DailyViewsGraph_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<CampaignModel.OverallChart> model = new List<CampaignModel.OverallChart>();

            List<int> XDailyViews = new List<int>();
            XDailyViews.Add(1);
            XDailyViews.Add(10);
            XDailyViews.Add(20);
            XDailyViews.Add(30);
            XDailyViews.Add(50);

            var data = campaignservice.GetSensiteGraphValueByDailyView(XDailyViews);

            for (int i = 0; i < XDailyViews.Count(); i++)
            {
                model.Add(new CampaignModel.OverallChart()
                {
                    XDaysActive = XDailyViews[i],
                    SensiteTotalUser = (decimal)Math.Round(data.SensiteTotalUser[i], 2),
                    AverageDaysActive = (decimal)Math.Round(data.AverageDaysActive[i], 2),
                    AverageDailyViews = (decimal)Math.Round(data.AverageDaysView[i], 2),
                    AverageCTRinPercent = (decimal)Math.Round(data.AverageCTR[i] * 100, 2),
                });
            }

            IEnumerable<CampaignModel.OverallChart> chartData = model;
            return Json(chartData);
        }

        public ActionResult GenderOverallView_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<CampaignModel.CampaignPieChart> model = new List<CampaignModel.CampaignPieChart>();

            var result = campaignservice.GetGenderOverallViews();

            model.Add(new CampaignModel.CampaignPieChart() { Category = "Male", Color = "#29AAE3", Value = (int)result.Male });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "Female", Color = "#DD3745", Value = (int)result.Female });
            model.Add(new CampaignModel.CampaignPieChart() { Category = "Unknown", Color = "#888888", Value = (int)result.Unknown });

            IEnumerable<CampaignModel.CampaignPieChart> chartData = model;
            return Json(model);
        }

        public ActionResult GenderOverallCTR_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<CampaignModel.CampaignBarChart> model = new List<CampaignModel.CampaignBarChart>();

            var result = campaignservice.GetGenderOverallCTR();

            model.Add(new CampaignModel.CampaignBarChart() { Category = "Male", Color = "#29AAE3", Value = Math.Round(result.Male * 100, 2) });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Female", Color = "#DD3745", Value = Math.Round(result.Female * 100, 2) });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Unknown", Color = "#888888", Value = Math.Round(result.Unknown * 100, 2) });

            IEnumerable<CampaignModel.CampaignBarChart> chartData = model;
            return Json(model);
        }
        
        public ActionResult GenderTotalViewAndCTRByCampaign_Read([DataSourceRequest] DataSourceRequest request, int CampaignId)
        {
            List<CampaignModel.CampaignBarChart> model = new List<CampaignModel.CampaignBarChart>();

            var result = campaignservice.GetGenderTotalViewAndCTRByCampaign(CampaignId);

            model.Add(new CampaignModel.CampaignBarChart() { Category = "Male", Color = "#29AAE3", Value = result.CTR.Male == null ? 0 : Math.Round(result.CTR.Male * 100, 2), Value2 = result.TotalView.Male == null ? 0 : result.TotalView.Male });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Female", Color = "#DD3745", Value = result.CTR.Female == null ? 0 : Math.Round(result.CTR.Female * 100, 2), Value2 = result.TotalView.Female == null ? 0 : result.TotalView.Female });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Unknown", Color = "#888888", Value = result.CTR.Unknown == null ? 0 : Math.Round(result.CTR.Unknown * 100, 2), Value2 = result.TotalView.Unknown == null ? 0 : result.TotalView.Unknown });

            IEnumerable<CampaignModel.CampaignBarChart> chartData = model;
            return Json(model);
        }

        public ActionResult WeekdaysTotalViewAndCTRByCampaign_Read([DataSourceRequest] DataSourceRequest request, int CampaignId)
        {
            List<CampaignModel.CampaignBarChart> model = new List<CampaignModel.CampaignBarChart>();

            var result = campaignservice.GetWeekdaysTotalViewAndCTRByCampaign(CampaignId);

            model.Add(new CampaignModel.CampaignBarChart() { Category = "Sunday", Color = "#e67e22", Value = Math.Round(result.CTR.Sunday * 100, 2), Value2 = result.TotalView.Sunday });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Monday", Color = "#e67e22", Value = Math.Round(result.CTR.Monday * 100, 2), Value2 = result.TotalView.Monday });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Tuesday", Color = "#e67e22", Value = Math.Round(result.CTR.Tuesday * 100, 2), Value2 = result.TotalView.Tuesday });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Wednesday", Color = "#e67e22", Value = Math.Round(result.CTR.Wednesday * 100, 2), Value2 = result.TotalView.Wednesday });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Thursday", Color = "#e67e22", Value = Math.Round(result.CTR.Thursday * 100, 2), Value2 = result.TotalView.Thursday });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Friday", Color = "#e67e22", Value = Math.Round(result.CTR.Friday * 100, 2), Value2 = result.TotalView.Friday });
            model.Add(new CampaignModel.CampaignBarChart() { Category = "Saturday", Color = "#e67e22", Value = Math.Round(result.CTR.Saturday * 100, 2), Value2 = result.TotalView.Saturday });

            IEnumerable<CampaignModel.CampaignBarChart> chartData = model;
            return Json(model);
        }

	}
}