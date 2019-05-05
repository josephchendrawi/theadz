using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    [ServiceContract]
    public interface IBranchService
    {
        [OperationContract]
        Response<Branch> GetBranchById(int BranchId);
        [OperationContract]
        Response<Branch> CreateEditBranch(Branch Branch);
        [OperationContract]
        Response<bool> DeleteBranch(int BranchId);
    }

    [DataContract]
    public class Branch
    {
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public string AddressLine2 { get; set; }
        [DataMember]
        public Country Country { get; set; }
        [DataMember]
        public City City { get; set; }
        [DataMember]
        public string PostCode { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        [DataMember]
        public DateTime Update { get; set; }
        [DataMember]
        public string LastAction { get; set; }
        [DataMember]
        public Merchant Merchant { get; set; }
        [DataMember]
        public Boolean Success { get; set; }
    }
}
