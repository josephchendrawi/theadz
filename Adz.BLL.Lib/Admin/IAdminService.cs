using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    [ServiceContract]
    public interface IAdminService
    {
        [OperationContract]
        Response<bool> CheckLogin(Admin Admin);
        [OperationContract]
        Response<bool> CreateEditAdmin(Admin Admin);
        [OperationContract]
        Response<List<Admin>> GetAdminList();
        [OperationContract]
        Response<bool> UpdateAdminRole(int AdminId, int newRoleId);
        [OperationContract]
        Response<List<AdminRole>> GetAdminRoleList();
        [OperationContract]
        Response<List<AdminModule>> GetAdminModuleList();
        [OperationContract]
        Response<Admin> GetAdminById(int AdminId);
        [OperationContract]
        Response<bool> DuplicateAdmin(int AdminId);
        [OperationContract]
        Response<bool> DeleteAdmin(int AdminId);
        [OperationContract]
        Response<List<AdminAccess>> GetAdminAccessList();
        [OperationContract]
        Response<bool> UpdateAdminAccess(AdminAccess adminaccess);
        [OperationContract]
        Response<bool> UpdateAdminRoleAttribute(AdminRole adminRole);
        [OperationContract]
        Response<bool> CreateAdminRole(AdminRole adminRole);
        [OperationContract]
        Response<bool> CreateAdminRoleAccess(AdminAccess AdminAccess);
    }

    [DataContract]
    public class Admin
    {
        [DataMember]
        public int AdminId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        [DataMember]
        public DateTime Update { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string PasswordSalt { get; set; }
        public string LastAction { get; set; }
        public AdminRole Role { get; set; }
    }

    [DataContract]
    public class AdminRole
    {
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string RoleName { get; set; }
    }

    [DataContract]
    public class AdminModule
    {
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public string ModuleName { get; set; }
    }

    [DataContract]
    public class AdminAccess
    {
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string ModuleName { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public bool is_viewable { get; set; }
        [DataMember]
        public bool is_editable { get; set; }
        [DataMember]
        public bool is_addable { get; set; }
        [DataMember]
        public bool is_deleteable { get; set; }
    }
}
