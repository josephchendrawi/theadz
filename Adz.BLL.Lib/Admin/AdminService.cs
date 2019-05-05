using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adz.DAL.EF;
using System.Threading;

namespace Adz.BLL.Lib
{
    public class AdminService : IAdminService
    {
        public Response<bool> CheckLogin(Admin Admin)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityAdmin = from d in context.Admins
                                  where d.email.ToLower() == Admin.Email.ToLower()
                                  && d.last_action != "5"
                                  select d;
                if (entityAdmin.Count() > 0)
                {
                    string key = entityAdmin.First().password_salt;
                    string pass = entityAdmin.First().password;
                    string passcek = Security.checkHMAC(key, Admin.Password);
                    if (pass == passcek)
                    {
                        response = Response<bool>.Create(true);
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.AdminEmailPasswordWrong);
                    }
                }
                else
                {
                    throw new CustomException(CustomErrorType.AdminEmailPasswordWrong);
                }
            }
            return response;
        }

        public Response<bool> CreateEditAdmin(Admin Admin)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Admin.AdminId != 0)
                {
                    var entityAdmin = from d in context.Admins
                                      where d.id == Admin.AdminId
                                      select d;
                    if (entityAdmin.Count() > 0)
                    {
                        var entityAdmin2 = from d in context.Admins
                                           where d.email.ToLower() == Admin.Email.ToLower()
                                           && d.id != Admin.AdminId
                                           select d;
                        if (entityAdmin2.Count() > 0)
                        {
                            throw new CustomException(CustomErrorType.AdminAlreadyAssign);
                        }
                        else
                        {
                            entityAdmin.First().first_name = Admin.FirstName;
                            entityAdmin.First().last_name = Admin.LastName;
                            entityAdmin.First().email = Admin.Email;
                            if (Admin.Password != null && Admin.Password != "")
                            {
                                string key1 = "tH34Dz";
                                string key = Security.checkHMAC(key1, Admin.Email.ToLower());
                                string pass = Security.checkHMAC(key, Admin.Password);
                                entityAdmin.First().password = pass;
                                entityAdmin.First().password_salt = key;
                            }
                            entityAdmin.First().role_id = Admin.Role.RoleId;
                            entityAdmin.First().last_updated = DateTime.UtcNow;
                            entityAdmin.First().last_action = "3";
                            context.SaveChanges();
                            response = Response<bool>.Create(true);
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.AdminNotFound);
                    }
                }
                else
                {
                    var entityAdmin = from d in context.Admins
                                      where d.email.ToLower() == Admin.Email.ToLower()
                                      select d;
                    if (entityAdmin.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.AdminAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.Admin mmentity = new Adz.DAL.EF.Admin();
                        mmentity.first_name = Admin.FirstName;
                        mmentity.last_name = Admin.LastName;
                        mmentity.role_id = Admin.Role.RoleId;
                        mmentity.email = Admin.Email;
                        mmentity.last_updated = DateTime.UtcNow;
                        mmentity.last_created = DateTime.UtcNow;
                        string key1 = "tH34Dz";
                        string key = Security.checkHMAC(key1, Admin.Email.ToLower());
                        string pass = Security.checkHMAC(key, Admin.Password);
                        mmentity.password = pass;
                        mmentity.password_salt = key;
                        mmentity.last_action = "1";
                        context.Admins.Add(mmentity);
                        context.SaveChanges();
                        response = Response<bool>.Create(true);
                    }
                }
            }
            return response;
        }

        public Response<List<Admin>> GetAdminList()
        {
            Response<List<Admin>> response = null;
            List<Admin> AdminList = new List<Admin>();
            using (var context = new TheAdzEntities())
            {
                var entityAdmin = from d in context.Admins
                                   orderby d.last_created descending
                                   select d;
                foreach (var v in entityAdmin)
                {
                    Admin Admin = new Admin();
                    Admin.AdminId = v.id;
                    Admin.FirstName = v.first_name;
                    Admin.LastName = v.last_name;
                    Admin.Email = v.email;
                    Admin.Role = new AdminRole(){ RoleId = (int)v.role_id, RoleName = v.AdminRole.name};
                    Admin.LastAction = v.last_action;

                    AdminList.Add(Admin);
                }
                response = Response<List<Admin>>.Create(AdminList);
            }

            return response;
        }
        public Response<Admin> GetAdminById(int AdminId)
        {
            Response<Admin> response = null;
            Admin Admin = new Admin();
            using (var context = new TheAdzEntities())
            {
                var entityAdmin = from d in context.Admins
                                  where d.id == AdminId
                                  select d;
                var v = entityAdmin.First();
                if (v != null)
                {
                    Admin.AdminId = v.id;
                    Admin.FirstName = v.first_name;
                    Admin.LastName = v.last_name;
                    Admin.Email = v.email;
                    Admin.Role = new AdminRole() { RoleId = (int)v.role_id, RoleName = v.AdminRole.name };
                    Admin.LastAction = v.last_action;

                    response = Response<Admin>.Create(Admin);
                }
            }

            return response;
        }

        public static Response<bool> Admin()
        {
            Response<bool> response = null;
            try
            {
                using (var context = new TheAdzEntities())
                {
                    string currentAdminName = Thread.CurrentPrincipal.Identity.Name;
                    var entityAdmin = from d in context.Admins
                                      where d.email.ToLower() == currentAdminName.ToLower()
                                      && d.last_action != "5"
                                      select d;
                    if (entityAdmin.Count() > 0)
                    {
                        response = Response<bool>.Create(true);
                    }
                    else
                    {
                        response = Response<bool>.Create(false);
                    }
                }
            }
            catch (Exception)
            {
                response = Response<bool>.Create(false);
            }
            return response;
        }
        public Response<Admin> GetCurrentAdmin()
        {
            try
            {
                Response<Admin> response = null;
                using (var context = new TheAdzEntities())
                {
                    string currentAdminName = Thread.CurrentPrincipal.Identity.Name;
                    var entityAdmin = from d in context.Admins
                                      where d.email.ToLower() == currentAdminName.ToLower()
                                      && d.last_action != "5"
                                      select d;
                    Admin us = new Admin();
                    us.AdminId = entityAdmin.First().id;
                    us.FirstName = entityAdmin.First().first_name;
                    us.LastName = entityAdmin.First().last_name;
                    us.Email = entityAdmin.First().email;
                    response = Response<Admin>.Create(us);
                }
                return response;
            }
            catch (Exception)
            {
                throw new CustomException(CustomErrorType.Unauthenticated);
            }
        }

        public static Response<List<string>> GetRoleViewAuth()
        {
            Response<List<string>> response = new Response<List<string>>();
            try
            {
                using (var context = new TheAdzEntities())
                {
                    string currentAdminName = Thread.CurrentPrincipal.Identity.Name;
                    var entityAdmin = from d in context.Admins
                                      where d.email.ToLower() == currentAdminName.ToLower()
                                      && d.last_action != "5"
                                      select d;
                    if (entityAdmin.Count() > 0)
                    {
                        var entityAdminAccess = from d in context.AdminRoleAccesses
                                                where d.adminrole_id == (int)entityAdmin.FirstOrDefault().role_id
                                                select d;

                        entityAdminAccess = entityAdminAccess.Where(p => p.is_viewable == true);

                        if (entityAdminAccess.Count() > 0)
                        {
                            List<string> ViewAuth = new List<string>();
                            foreach (var v in entityAdminAccess)
                            {
                                ViewAuth.Add(v.AdminModule.name);
                            }

                            var entityAdminModule = from d in context.AdminModules
                                                    select d;
                            List<string> result = new List<string>();
                            foreach(var v in entityAdminModule){
                                if (ViewAuth.Contains(v.name) == false)
                                {
                                    result.Add(v.name);
                                }
                            }

                            response = Response<List<string>>.Create(result);
                        }
                        else
                        {
                            response = Response<List<string>>.Create();
                        }
                    }
                    else
                    {
                        response = Response<List<string>>.Create();
                    }
                }
            }
            catch (Exception)
            {
                response = Response<List<string>>.Create();
            }
            return response;
        }

        public static Response<bool> IsAuthorized(string module, string access)
        {
            Response<bool> response = null;
            try
            {
                using (var context = new TheAdzEntities())
                {
                    string currentAdminName = Thread.CurrentPrincipal.Identity.Name;
                    var entityAdmin = from d in context.Admins
                                      where d.email.ToLower() == currentAdminName.ToLower()
                                      && d.last_action != "5"
                                      select d;
                    if (entityAdmin.Count() > 0)
                    {
                        var entityAdminAccess = from d in context.AdminRoleAccesses
                                                where d.adminrole_id == (int)entityAdmin.FirstOrDefault().role_id
                                                && d.AdminModule.name == module
                                                select d;

                        if (access == "VIEW")
                        {
                            entityAdminAccess = entityAdminAccess.Where(p => p.is_viewable == true);
                        }
                        else if (access == "EDIT")
                        {
                            entityAdminAccess = entityAdminAccess.Where(p => p.is_editable == true);
                        }
                        else if (access == "ADD")
                        {
                            entityAdminAccess = entityAdminAccess.Where(p => p.is_addable == true);
                        }
                        else if (access == "DELETE")
                        {
                            entityAdminAccess = entityAdminAccess.Where(p => p.is_deleteable == true);
                        }

                        if (entityAdminAccess.Count() > 0)
                        {
                            response = Response<bool>.Create(true);
                        }
                        else
                        {
                            response = Response<bool>.Create(false);
                        }
                    }
                    else
                    {
                        response = Response<bool>.Create(false);
                    }
                }
            }
            catch (Exception)
            {
                response = Response<bool>.Create(false);
            }
            return response;
        }

        public Response<bool> UpdateAdminRole(int AdminId, int newRoleId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityAdmin = from d in context.Admins
                                    where d.id == AdminId
                                    select d;

                if (entityAdmin.Count() > 0)
                {
                    entityAdmin.First().role_id = newRoleId;
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    response = Response<bool>.Create(false);
                    throw new CustomException(CustomErrorType.AdminNotFound);
                }
            }
            return response;
        }
        public Response<List<AdminRole>> GetAdminRoleList()
        {
            Response<List<AdminRole>> response = null;
            List<AdminRole> AdminRoleList = new List<AdminRole>();
            using (var context = new TheAdzEntities())
            {
                var entityAdminRole = from d in context.AdminRoles
                                      select d;
                foreach (var v in entityAdminRole)
                {
                    AdminRole AdminRole = new AdminRole();
                    AdminRole.RoleId = v.id;
                    AdminRole.RoleName = v.name;

                    AdminRoleList.Add(AdminRole);
                }
                response = Response<List<AdminRole>>.Create(AdminRoleList);
            }

            return response;
        }
        public Response<List<AdminModule>> GetAdminModuleList()
        {
            Response<List<AdminModule>> response = null;
            List<AdminModule> AdminModuleList = new List<AdminModule>();
            using (var context = new TheAdzEntities())
            {
                var entityAdminModule = from d in context.AdminModules
                                        select d;
                foreach (var v in entityAdminModule)
                {
                    AdminModule AdminModule = new AdminModule();
                    AdminModule.ModuleId = v.id;
                    AdminModule.ModuleName = v.name;

                    AdminModuleList.Add(AdminModule);
                }
                response = Response<List<AdminModule>>.Create(AdminModuleList);
            }

            return response;
        }

        public Response<bool> DeleteAdmin(int AdminId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityAdmin = from d in context.Admins
                                   where d.id == AdminId
                                   select d;
                if (entityAdmin.Count() > 0)
                {
                    entityAdmin.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.AdminFailedDelete);
                }
            }

            return response;
        }

        public Response<bool> DuplicateAdmin(int AdminId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                Admin item = GetAdminById(AdminId).Result;
                item.AdminId = 0;
                item.Email = item.Email.Substring(0,item.Email.IndexOf("@")) + "-copy" + item.Email.Substring(item.Email.IndexOf("@"));
                var result = CreateEditAdmin(item).Result;
                response = Response<bool>.Create(result);
            }

            return response;
        }

        public Response<List<AdminAccess>> GetAdminAccessList()
        {
            Response<List<AdminAccess>> response = null;
            List<AdminAccess> AdminAccessList = new List<AdminAccess>();
            using (var context = new TheAdzEntities())
            {
                var entityAdminRoleAccess = from d in context.AdminRoleAccesses
                                            select d;
                foreach (var v in entityAdminRoleAccess)
                {
                    AdminAccess AdminAccess = new AdminAccess();
                    AdminAccess.ModuleId = v.module_id;
                    AdminAccess.RoleId = v.adminrole_id;
                    AdminAccess.ModuleName = v.AdminModule.name;
                    AdminAccess.RoleName = v.AdminRole.name;
                    AdminAccess.is_addable = v.is_addable == null ? false : (bool)v.is_addable;
                    AdminAccess.is_editable = v.is_editable == null ? false : (bool)v.is_editable;
                    AdminAccess.is_deleteable = v.is_deleteable == null ? false : (bool)v.is_deleteable;
                    AdminAccess.is_viewable = v.is_viewable == null ? false : (bool)v.is_viewable;

                    AdminAccessList.Add(AdminAccess);
                }
                response = Response<List<AdminAccess>>.Create(AdminAccessList);
            }

            return response;
        }

        public Response<bool> UpdateAdminAccess(AdminAccess adminaccess)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityAdminAccess = from d in context.AdminRoleAccesses
                                        where d.adminrole_id == adminaccess.RoleId && d.module_id == adminaccess.ModuleId
                                        select d;

                if (entityAdminAccess.Count() > 0)
                {
                    entityAdminAccess.First().is_addable = adminaccess.is_addable;
                    entityAdminAccess.First().is_deleteable = adminaccess.is_deleteable;
                    entityAdminAccess.First().is_editable = adminaccess.is_editable;
                    entityAdminAccess.First().is_viewable = adminaccess.is_viewable;
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    response = Response<bool>.Create(false);
                    throw new CustomException(CustomErrorType.AdminRoleAccessNotFound);
                }
            }
            return response;
        }

        public Response<bool> UpdateAdminRoleAttribute(AdminRole adminRole)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityAdminRole = from d in context.AdminRoles
                                        where d.id == adminRole.RoleId
                                        select d;

                if (entityAdminRole.Count() > 0)
                {
                    entityAdminRole.First().name = adminRole.RoleName;
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    response = Response<bool>.Create(false);
                    throw new CustomException(CustomErrorType.AdminRoleNotFound);
                }
            }
            return response;
        }

        public Response<bool> CreateAdminRole(AdminRole adminRole)
        {
            if (adminRole.RoleId == 0)
            {
                Response<bool> response = null;
                using (var context = new TheAdzEntities())
                {
                    var entityAdminRole = from d in context.AdminRoles
                                          where d.id == adminRole.RoleId
                                          select d;

                    if (entityAdminRole.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.AdminRoleAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.AdminRole mmentity = new Adz.DAL.EF.AdminRole();
                        mmentity.name = adminRole.RoleName;
                        context.AdminRoles.Add(mmentity);
                        context.SaveChanges();

                        response = Response<bool>.Create(true);
                    }
                }
                return response;
            }
            else
            {
                throw new CustomException(CustomErrorType.AdminRoleFailed);
            }
        }

        public Response<bool> CreateAdminRoleAccess(AdminAccess AdminAccess)
        {
            if (AdminAccess.RoleId != 0 && AdminAccess.RoleId != null && AdminAccess.ModuleId != 0 && AdminAccess.ModuleId != null)
            {
                Response<bool> response = null;
                using (var context = new TheAdzEntities())
                {
                    var entityAdminAccessRole = from d in context.AdminRoleAccesses
                                                where d.adminrole_id == AdminAccess.RoleId && d.module_id == AdminAccess.ModuleId
                                                select d;

                    if (entityAdminAccessRole.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.AdminRoleAccessAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.AdminRoleAccess mmentity = new Adz.DAL.EF.AdminRoleAccess();
                        mmentity.module_id = AdminAccess.ModuleId;
                        mmentity.adminrole_id = AdminAccess.RoleId;
                        mmentity.is_viewable = AdminAccess.is_viewable;
                        mmentity.is_editable = AdminAccess.is_editable;
                        mmentity.is_addable = AdminAccess.is_addable;
                        mmentity.is_deleteable = AdminAccess.is_deleteable;
                        context.AdminRoleAccesses.Add(mmentity);
                        context.SaveChanges();

                        response = Response<bool>.Create(true);
                    }
                }
                return response;
            }
            else
            {
                throw new CustomException(CustomErrorType.AdminRoleAccessFailed);
            }
        }


    }
}
