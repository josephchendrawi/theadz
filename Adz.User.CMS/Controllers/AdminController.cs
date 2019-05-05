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
    public class AdminController : BaseAdminController
    {
        public IAdminService adminservice = new AdminService();
        public IImageService imageservice = new ImageService();
        public ICampaignService campaignservice = new CampaignService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Admin login = new Admin();
                    login.Email = model.UserName;
                    login.Password = model.Password;

                    bool result = adminservice.CheckLogin(login).Result;
                    if (result)
                    {
                        FormsAuthentication.SetAuthCookie(login.Email, model.RememberMe);
                        return RedirectToAction("Index", "Admin");
                    }
                }
                catch (Exception ex)
                {
                    //CustomException(ex);
                    ViewBag.Error = ex.Message;
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }

        [AdminAuthorize("ADMIN", "VIEW")]
        public ActionResult Admin(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }

        public ActionResult Admin_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.Admin> listR = adminservice.GetAdminList().Result;
            List<AdminModel.Admin> newlistR = new List<AdminModel.Admin>();
            foreach (var v in listR)
            {
                newlistR.Add(new AdminModel.Admin() { AdminId = v.AdminId, FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, RoleName = v.Role.RoleName, RoleId = v.Role.RoleId, LastAction = v.LastAction });
            }
            IEnumerable<AdminModel.Admin> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize("ADMIN", "EDIT")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admin_Update([DataSourceRequest] DataSourceRequest request, AdminModel.Admin Admin, int newRoleId)
        {
            if (Admin != null)
            {
                adminservice.UpdateAdminRole(Admin.AdminId, newRoleId);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public JsonResult RoleList()
        {
            List<AdminModel.AdminRole> newlistR = new List<AdminModel.AdminRole>();
            foreach (var v in adminservice.GetAdminRoleList().Result)
            {
                newlistR.Add(new AdminModel.AdminRole() { RoleId = v.RoleId, RoleName = v.RoleName });
            }
            IEnumerable<AdminModel.AdminRole> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize("ADMIN", "VIEW")]
        public ActionResult AdminRole(string r)
        {
            ViewModels.AdminRoleVM model = new ViewModels.AdminRoleVM();

            model.AdminModule = new List<AdminModel.AdminModule>();
            foreach (var v in adminservice.GetAdminModuleList().Result)
            {
                model.AdminModule.Add(new AdminModel.AdminModule() { ModuleId = v.ModuleId, ModuleName = v.ModuleName });
            }

            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View(model);
        }
        public ActionResult AdminRole_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<AdminModel.AdminRole> newlistR = new List<AdminModel.AdminRole>();
            foreach (var v in adminservice.GetAdminRoleList().Result)
            {
                newlistR.Add(new AdminModel.AdminRole() { RoleId = v.RoleId, RoleName = v.RoleName });
            }
            IEnumerable<AdminModel.AdminRole> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AdminAuthorize("ADMIN", "EDIT")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AdminRole_Update([DataSourceRequest] DataSourceRequest request, AdminModel.AdminRole AdminRole)
        {
            if (AdminRole != null)
            {
                Adz.BLL.Lib.AdminRole adminRole = new BLL.Lib.AdminRole()
                {
                    RoleId = AdminRole.RoleId,
                    RoleName = AdminRole.RoleName
                };
                adminservice.UpdateAdminRoleAttribute(adminRole);
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [AdminAuthorize("ADMIN", "ADD")]
        public ActionResult AdminRoleAdd()
        {
            AdminModel.AdminRole model = new AdminModel.AdminRole();
            model.RoleId = 0;
            return View(model);
        }
        [AdminAuthorize("ADMIN", "ADD")]
        [HttpPost]
        public ActionResult AdminRoleAdd(FormCollection collection, AdminModel.AdminRole AdminRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.AdminRole mer = new Adz.BLL.Lib.AdminRole();
                    mer.RoleId = 0;
                    mer.RoleName = AdminRole.RoleName;

                    bool result = adminservice.CreateAdminRole(mer).Result;
                    string r;
                    if (result != false)
                    {
                        r = "a1";
                    }
                    else
                    {
                        r = "a2";
                    }
                    return RedirectToAction("AdminRole", "Admin", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(AdminRole);
        }

        public JsonResult ModuleList()
        {
            List<AdminModel.AdminModule> newlistR = new List<AdminModel.AdminModule>();
            foreach (var v in adminservice.GetAdminModuleList().Result)
            {
                newlistR.Add(new AdminModel.AdminModule() { ModuleId = v.ModuleId, ModuleName = v.ModuleName });
            }
            IEnumerable<AdminModel.AdminModule> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdminAccess_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<AdminModel.AdminAccess> newlistR = new List<AdminModel.AdminAccess>();
            int i = 0;
            foreach (var v in adminservice.GetAdminAccessList().Result)
            {
                i++;
                newlistR.Add(new AdminModel.AdminAccess() { AccessId = i, RoleId = v.RoleId, ModuleId = v.ModuleId, RoleName = v.RoleName, ModuleName = v.ModuleName, is_addable = v.is_addable, is_deleteable = v.is_deleteable, is_editable = v.is_editable, is_viewable = v.is_viewable });
            }
            IEnumerable<AdminModel.AdminAccess> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AdminAuthorize("ADMIN", "EDIT")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AdminAccess_Update([DataSourceRequest] DataSourceRequest request, AdminModel.AdminAccess AdminAccess)
        {
            if (AdminAccess != null)
            {
                Adz.BLL.Lib.AdminAccess adminaccess = new BLL.Lib.AdminAccess()
                {
                    RoleId = AdminAccess.RoleId,
                    ModuleId = AdminAccess.ModuleId,
                    is_viewable = AdminAccess.is_viewable,
                    is_editable = AdminAccess.is_editable,
                    is_addable = AdminAccess.is_addable,
                    is_deleteable = AdminAccess.is_deleteable
                };
                adminservice.UpdateAdminAccess(adminaccess);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AdminAuthorize("ADMIN", "ADD")]
        public ActionResult AdminRoleAccessAdd()
        {
            AdminModel.AdminAccess model = new AdminModel.AdminAccess();
            model.RoleId = 0;
            model.ModuleId = 0;
            return View(model);
        }
        [AdminAuthorize("ADMIN", "ADD")]
        [HttpPost]
        public ActionResult AdminRoleAccessAdd(FormCollection collection, AdminModel.AdminAccess AdminAccess)
        {
            if (collection.Get("roleDropDown") == null || collection.Get("roleDropDown") == "")
            {
                ModelState.AddModelError("RoleNull", "The Role field is required.");
            }
            else
            {
                AdminAccess.RoleId = Convert.ToInt16(collection.Get("roleDropDown").ToString());
            }

            if (collection.Get("moduleDropDown") == null || collection.Get("moduleDropDown") == "")
            {
                ModelState.AddModelError("ModuleNull", "The Module field is required.");
            }
            else
            {
                AdminAccess.ModuleId = Convert.ToInt16(collection.Get("moduleDropDown").ToString());
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.AdminAccess mer = new Adz.BLL.Lib.AdminAccess();
                    mer.RoleId = AdminAccess.RoleId;
                    mer.ModuleId = AdminAccess.ModuleId;
                    mer.is_viewable = AdminAccess.is_viewable;
                    mer.is_addable = AdminAccess.is_addable;
                    mer.is_editable = AdminAccess.is_editable;
                    mer.is_deleteable = AdminAccess.is_deleteable;

                    bool result = adminservice.CreateAdminRoleAccess(mer).Result;
                    string r;
                    if (result != false)
                    {
                        r = "aa1";
                    }
                    else
                    {
                        r = "aa2";
                    }
                    return RedirectToAction("AdminRole", "Admin", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(AdminAccess);
        }

        [AdminAuthorize("ADMIN", "VIEW")]
        public ActionResult AdminView(int AdminId)
        {
            Adz.BLL.Lib.Admin v = adminservice.GetAdminById(AdminId).Result;

            AdminModel.Admin model = new AdminModel.Admin() { AdminId = v.AdminId, FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, RoleId = v.Role.RoleId, RoleName = v.Role.RoleName, LastAction = v.LastAction  };

            return View(model);
        }
        [AdminAuthorize("ADMIN", "EDIT")]
        public ActionResult AdminEdit(int AdminId)
        {
            Adz.BLL.Lib.Admin v = adminservice.GetAdminById(AdminId).Result;

            AdminModel.Admin model = new AdminModel.Admin() { AdminId = v.AdminId, FirstName = v.FirstName, LastName = v.LastName, Email = v.Email, RoleId = v.Role.RoleId, RoleName = v.Role.RoleName, LastAction = v.LastAction };

            return View(model);
        }
        [AdminAuthorize("ADMIN", "EDIT")]
        [HttpPost]
        public ActionResult AdminEdit(FormCollection collection, AdminModel.Admin Admin)
        {
            if (collection.Get("roleDropDown") == null || collection.Get("roleDropDown") == "")
            {
                ModelState.AddModelError("RoleNull", "The Role field is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Admin mer = new Adz.BLL.Lib.Admin();
                    mer.AdminId = Admin.AdminId;
                    mer.FirstName = Admin.FirstName;
                    mer.LastName = Admin.LastName;
                    mer.Email = Admin.Email;
                    mer.Password = Admin.Password;
                    Adz.BLL.Lib.AdminRole rc = new Adz.BLL.Lib.AdminRole();
                    rc.RoleId = Convert.ToInt16(collection.Get("roleDropDown").ToString());
                    mer.Role = rc;
                    
                    bool result = adminservice.CreateEditAdmin(mer).Result;
                    string r;
                    if (result != false)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("Admin", "Admin", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Admin);
        }

        [AdminAuthorize("ADMIN", "ADD")]
        public ActionResult AdminAdd()
        {
            AdminModel.Admin model = new AdminModel.Admin();
            model.RoleId = 0;
            return View(model);
        }

        [AdminAuthorize("ADMIN", "ADD")]
        [HttpPost]
        public ActionResult AdminAdd(FormCollection collection, AdminModel.Admin Admin)
        {
            if (collection.Get("roleDropDown") == null || collection.Get("roleDropDown") == "")
            {
                ModelState.AddModelError("RoleNull", "The Role field is required.");
            }
            else
            {
                Admin.RoleId = Convert.ToInt16(collection.Get("roleDropDown").ToString());
            }

            if (Admin.Password == null || Admin.Password == "")
            {
                ModelState.AddModelError("PasswordNull", "The Password field is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Admin mer = new Adz.BLL.Lib.Admin();
                    mer.AdminId = 0;
                    mer.FirstName = Admin.FirstName;
                    mer.LastName = Admin.LastName;
                    mer.Email = Admin.Email;
                    mer.Password = Admin.Password;
                    Adz.BLL.Lib.AdminRole rc = new Adz.BLL.Lib.AdminRole();
                    rc.RoleId = Admin.RoleId;
                    mer.Role = rc;

                    bool result = adminservice.CreateEditAdmin(mer).Result;
                    string r;
                    if (result != false)
                    {
                        r = "a1";
                    }
                    else
                    {
                        r = "a2";
                    }
                    return RedirectToAction("Admin", "Admin", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Admin);
        }

        [AdminAuthorize("ADMIN", "DELETE")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admin_Destroy([DataSourceRequest] DataSourceRequest request, AdminModel.Admin Admin)
        {
            try
            {
                if (Admin != null)
                {
                    Boolean result = adminservice.DeleteAdmin(Admin.AdminId).Result;
                }
            }
            catch (Exception ex)
            {
                CustomException(ex);
                ModelState.AddModelError("", ex.Message);
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [AdminAuthorize("ADMIN", "ADD")]
        public String Admin_Duplicate(int AdminId)
        {
            string res = "";
            if (AdminId != null && AdminId > 0)
            {
                try
                {
                    Boolean result = adminservice.DuplicateAdmin(AdminId).Result;

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

        public ActionResult GoogleMap(double lat, double lng)
        {
            CountryModel.Map model = new CountryModel.Map() { Latitude = lat, Longitude = lng };
            return View(model);
        }

        public ActionResult GoogleHeatMap(string parameter)
        {
            string[] pars = parameter.Split('|');

            List<Adz.BLL.Lib.CampaignHistory> v = campaignservice.GetCampaignHistoryByCampaignId(int.Parse(pars[0]), int.Parse(pars[1])).Result;

            List<CountryModel.Map> LatLng = new List<CountryModel.Map>();
            foreach (var c in v)
            {
                LatLng.Add(new CountryModel.Map() { Latitude = (double)c.Latitude, Longitude = (double)c.Longitude });
            }

            CampaignModel.CampaignHeatMap model = new CampaignModel.CampaignHeatMap() { CampaignId = int.Parse(pars[0]), LatLng = LatLng };
            
            return View(model);
        }

	}
}