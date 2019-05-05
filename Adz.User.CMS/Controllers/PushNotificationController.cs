using Adz.BLL.Lib;
using Adz.User.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adz.User.CMS.Controllers
{
    public class PushNotificationController : BaseAdminController
    {
        IUserService UserService = new UserService();

        public ActionResult Add(string r)
        {
            ViewBag.msg = r;

            PushNotificationModel model = new PushNotificationModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(FormCollection collection, PushNotificationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PNobject PNobject = new BLL.Lib.PNobject();
                    PNobject.alert = model.Message;

                    UserService.PushNotificationToAllDevice(PNobject);
                    string r = "a1";

                    return RedirectToAction("Add", "PushNotification", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(model);
        }

    }
}