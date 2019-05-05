using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel.Types
{
    public class Merchant
    {
        public int MerchantId { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public string PostCode { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }

        public List<int> SubImageId { get; set; }
        public List<string> SubImageName { get; set; }
        public List<string> SubImageUrl { get; set; }
        public List<string> SubImageUrlLink { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public string LastAction { get; set; }
        public List<Campaign> Campaigns { get; set; }
        public List<Branch> Branches { get; set; }
    }

    public class Branch
    {
        public int BranchId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public string PostCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public string LastAction { get; set; }
        public int MerchantId { get; set; }
        public Boolean Success { get; set; }
    }
}
