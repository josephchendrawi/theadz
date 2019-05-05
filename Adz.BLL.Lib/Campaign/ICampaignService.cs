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
    public interface ICampaignService
    {
        [OperationContract]
        Response<List<Campaign>> GetCampaignList();
        [OperationContract]
        Response<Campaign> GetCampaignById(int CampaignId);
        [OperationContract]
        Response<int> CreateEditCampaign(Campaign Campaign);
        [OperationContract]
        Response<bool> DeleteCampaign(int CampaignId);
        [OperationContract]
        Response<bool> DuplicateCampaign(int CampaignId);
        [OperationContract]
        Response<Campaign> ViewThisCampaign(int CampaignId, string username, int gender, double lat, double lng, DateTime? time, string timezone, int action);
        [OperationContract]
        Response<List<Campaign>> GetNowCampaignList(int gender, double lat, double lng, int age);
        [OperationContract]
        Response<List<Campaign>> GetAllCampaignList();
        [OperationContract]
        Response<List<CampaignHistory>> GetCampaignHistory(int action);
        [OperationContract]
        Response<List<CampaignHistoryGroup>> GetCampaignHistoryGroup();
        [OperationContract]
        Response<List<CampaignHistory>> GetCampaignHistoryByCampaignId(int CampaignId, int Action);
        [OperationContract]
        Response<List<CampaignHistoryGroupByDate>> GetCampaignHistoryGroupByDateAndUser(int UserId);
        [OperationContract]
        Response<int> IncrementCampaignView(int CampaignId, string username, int gender, double lat, double lng);
        Response<CampaignHistoryGroup> GetCampaignHistoryGroupById(int CampaignId);
        SensiteGraph GetSensiteGraphValueByActiveDays(List<int> XDaysActive);
        SensiteGraph GetSensiteGraphValueByDailyView(List<int> XDailyViews);
        GenderGraph GetGenderOverallViews();
        GenderGraph GetGenderOverallCTR();
        GenderGraphCombine GetGenderTotalViewAndCTRByCampaign(int CampaignId);
        WeekdaysGraphCombine GetWeekdaysTotalViewAndCTRByCampaign(int CampaignId);
    }

    [DataContract]
    public class Campaign
    {
        [DataMember]
        public int CampaignId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime Start { get; set; }
        [DataMember]
        public DateTime End { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        [DataMember]
        public DateTime Update { get; set; }
        [DataMember]
        public string LastAction { get; set; }
        [DataMember]
        public Merchant Merchant { get; set; }

        public List<int> SubRuleId { get; set; }
        public List<int> SubImageId { get; set; }
        public List<string> SubImageName { get; set; }
        public List<string> SubImageUrl { get; set; }
        public List<string> SubImageUrlLink { get; set; }
        public List<int> SubImageWidth { get; set; }
        public List<int> SubImageHeight { get; set; }
        public List<string> SubImageCheckSum { get; set; }

        [DataMember]
        public string LinkURL { get; set; }

        public List<Rule> Rules { get; set; }
    }

    [DataContract]
    public class CampaignHistory
    {
        [DataMember]
        public Campaign Campaign { get; set; }
        [DataMember]
        public User User { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public int Action { get; set; }
    }

    [DataContract]
    public class CampaignHistoryGroup
    {
        [DataMember]
        public Campaign Campaign { get; set; }
        [DataMember]
        public int Views { get; set; }
        [DataMember]
        public int Clicks { get; set; }
        [DataMember]
        public DateTime LastViewTime { get; set; }
        [DataMember]
        public DateTime LastClickTime { get; set; }
    }

    [DataContract]
    public class CampaignHistoryGroupByDate
    {
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Views { get; set; }
        [DataMember]
        public int Clicks { get; set; }
        [DataMember]
        public bool isGetPoint { get; set; }
    }
    
    public enum CampaignHistoryAction
    {
        View = 1,
        Click = 2
    }

    public class SensiteGraph
    {
        public List<double> SensiteTotalUser { get; set; }
        public List<double> AverageDaysActive { get; set; }
        public List<double> AverageDaysView { get; set; }
        public List<double> AverageCTR { get; set; }
    }

    public class GenderGraph
    {
        public decimal Male { get; set; }
        public decimal Female { get; set; }
        public decimal Unknown { get; set; }
    }

    public class GenderGraphCombine
    {
        public GenderGraph TotalView { get; set; }
        public GenderGraph CTR { get; set; }
    }
    
    public class WeekdaysGraph
    {
        public decimal Sunday { get; set; }
        public decimal Monday { get; set; }
        public decimal Tuesday { get; set; }
        public decimal Wednesday { get; set; }
        public decimal Thursday { get; set; }
        public decimal Friday { get; set; }
        public decimal Saturday { get; set; }
    }

    public class WeekdaysGraphCombine
    {
        public WeekdaysGraph TotalView { get; set; }
        public WeekdaysGraph CTR { get; set; }
    }

}
