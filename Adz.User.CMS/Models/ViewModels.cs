using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adz.User.CMS.Models
{
    public class ViewModels
    {
        public class AdsPerformanceUsageVM
        {
            public List<CampaignModel.CampaignHistoryGroup> CampaignHistoryGroup { get; set; }
            public List<CampaignModel.CampaignHistory> CampaignHistories { get; set; }
        }

        public class AdminRoleVM
        {
            public List<AdminModel.AdminModule> AdminModule { get; set; }
        }
    }
}