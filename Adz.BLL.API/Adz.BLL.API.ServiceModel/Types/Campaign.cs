using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel.Types
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }
        //public DateTime Create { get; set; }
        //public DateTime Update { get; set; }
        //public string LastAction { get; set; }
        public int MerchantId { get; set; }
        public string MerchantName { get; set; }

        public List<int> SubRuleId { get; set; }
        public List<int> SubImageId { get; set; }
        public List<string> SubImageName { get; set; }
        public List<string> SubImageUrl { get; set; }
        public List<string> SubImageUrlLink { get; set; }

        public List<int> SubImageWidth { get; set; }
        public List<int> SubImageHeight { get; set; }

        public List<string> SubImageCheckSum { get; set; }

        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlLink { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public string ImageCheckSum { get; set; }

        public string LinkURL { get; set; }

        public List<Rule> Rules { get; set; }
    }

    public class Rule
    {
        public int RuleId { get; set; }
        public int Gender { get; set; }
        public int NumberOfView { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Boolean NoEnd { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public string LastAction { get; set; }

        public int CampaignId { get; set; }

        public Boolean Success { get; set; }
    }

    public class Views
    {
        public int NumberOfView { get; set; }
        public bool StillValid { get; set; }
    }

    public class UsageData
    {
        public int CampaignId { get; set; }
        public DateTime ViewTime { get; set; }
        public string TimeZone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Action { get; set; }
    }

}
