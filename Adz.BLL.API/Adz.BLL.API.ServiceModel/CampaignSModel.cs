using Adz.BLL.API.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel
{
    [Route("/campaign/", "GET")]
    [Route("/campaign/{ID}", "GET")]
    public class CampaignGet : IReturn<CampaignResponse>
    {
        public int id { get; set; }
    }

    [Route("/campaign/list")]
    public class CampaignList : IReturn<CampaignListResponse>
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    [Route("/campaign/view", "POST")]
    public class CampaignView : IReturn<CampaignResponse>
    {
        public int id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    [Route("/campaign/increment", "POST")]
    public class CampaignViewIncrement : IReturn<CampaignIncrementResponse>
    {
        public int id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    [Route("/campaign/sync", "POST")]
    public class CampaignUsageSync : IReturn<CustomResponse>
    {
        public List<UsageData> Usage { get; set; }
    }

    public class CampaignListResponse : CampaignResponse
    {
        public List<Campaign> Result { get; set; }
    }

    public class CampaignIncrementResponse : CampaignResponse
    {
        public Views Result { get; set; }
    }

    public class CampaignResponse
    {
        public Campaign Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

}
