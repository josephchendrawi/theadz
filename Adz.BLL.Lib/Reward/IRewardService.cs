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
    public interface IRewardService
    {
        [OperationContract]
        Response<List<Reward>> GetRewardList();
        [OperationContract]
        Response<Reward> GetRewardById(int RewardId);
        [OperationContract]
        Response<int> CreateEditReward(Reward Reward);
        [OperationContract]
        Response<bool> DeleteReward(int RewardId);
        [OperationContract]
        Response<bool> DuplicateReward(int RewardId);
        [OperationContract]
        Response<List<RewardCriteria>> GetRewardCriteriaList();
        Response<List<MobileOperator>> GetMobileOperatorList();
        Response<int> CreateEditRedemption(Redemption Redemption);
        Response<List<Redemption>> GetRedemptionList();
        Response<List<Redemption>> GetRedemptionListByUser(string Email);
        Response<Redemption> GetRedemptionById(int RedemptionId);
        Response<int> CreateEditReview(Review Review, bool Editable);
        Response<List<RewardType>> GetRewardTypeList();
        Response<List<Review>> GetReviewListByRewardId(int RewardId);
        Response<List<Review>> GetReviewListByUserId(int UserId);
        Response<Review> GetReviewByRedemptionId(int RedemptionId);
        Response<bool> UpdateRedemptionStatus(int RedemptionId, int NewStatusId);
        Response<List<RedemptionStatus>> GetRedemptionStatusList();
        Response<bool> DeleteReview(int RedemptionId);
    }

    public class Reward
    {
        [DataMember]
        public int RewardId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SponsorName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        [DataMember]
        public DateTime Update { get; set; }
        [DataMember]
        public string LastAction { get; set; }

        [DataMember]
        public List<int> SubImageId { get; set; }
        [DataMember]
        public List<string> SubImageName { get; set; }
        [DataMember]
        public List<string> SubImageUrl { get; set; }
        [DataMember]
        public List<string> SubImageUrlLink { get; set; }

        public RewardCriteria RewardCriteria { get; set; }

        public RewardType RewardType { get; set; }

        public int PointRequirement { get; set; }
        [DataMember]
        public string SubTitle { get; set; }

        public bool OneTimeFlag { get; set; }

        public int NumberOfStock { get; set; }
    }

    public class RewardCriteria
    {
        public int CriteriaId { get; set; }
        public string Name { get; set; }
    }

    public class RewardType
    {
        public int RewardTypeId { get; set; }
        public string RewardTypeName { get; set; }
        public bool Delivery { get; set; }
        public bool MoneyTransfer { get; set; }
        public bool Mobile { get; set; }
        public bool Receiver { get; set; }
        public string RewardTypeSubName { get; set; }
    }

    public class MobileOperator
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class Redemption
    {
        public int RedemptionId { get; set; }
        public string UserEmail { get; set; }
        public int RewardId { get; set; }
        public Reward Reward { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string BankName { get; set; }
        public string BankAccountNum { get; set; }
        public string BankAccountName { get; set; }
        public int MobileOperatorId { get; set; }
        public MobileOperator MobileOperator { get; set; }
        public string MobileAccNum { get; set; }
        public RedemptionStatus RedemptionStatus { get; set; }
        public DateTime RedemptionDate { get; set; }
        public bool Reviewed { get; set; }
    }
    public class RedemptionStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Review
    {
        public int RedemptionId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public string ByEmail { get; set; }
        public string LastAction { get; set; }
    }

    public enum RedemptionStatusCode
    {
        Submitted = 1,
        Processing = 2,
        Completed = 3,
        Cancelled = 4
    }

}
