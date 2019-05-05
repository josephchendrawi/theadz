using Adz.BLL.Lib.Report;
using Adz.User.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adz.User.CMS.Controllers
{
    public class SettingController : BaseAdminController
    {
        public IReportService ReportService = new ReportService();

        public ActionResult DailyReport()
        {
            DailyReportEmail model = new DailyReportEmail();
            model.Email = ReportService.GetDailyReportTargetEmail();

            return View(model);
        }
        [HttpPost]
        public ActionResult DailyReport(FormCollection collection, DailyReportEmail Model)
        {
            if (ModelState.IsValid)
            {
                ReportService.CreateEditDailyReportTargetEmail(Model.Email);

                ViewBag.msg = "a1";
            }
            else
            {
                ViewBag.msg = string.Join(" ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
            }

            return View(Model);
        }
    }
}