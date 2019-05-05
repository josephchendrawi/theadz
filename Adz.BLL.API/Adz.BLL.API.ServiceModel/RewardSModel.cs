using Adz.BLL.API.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel
{
    [Route("/reward/", "GET")]
    [Route("/reward/{ID}", "GET")]
    public class RewardGet : IReturn<RewardResponse>
    {
        public int id { get; set; }
    }

    [Route("/reward/list")]
    public class RewardList : IReturn<RewardListResponse>
    {
    }

    [Route("/mobileoperator/list")]
    public class MobileOperatorList : IReturn<MobileOperatorListResponse>
    {
    }

    [Route("/redemption/request", "POST")]
    public class RedemptionRequest : IReturn<CustomResponse>
    {
        public int RewardId { get; set; }
        //public string ReceiverName { get; set; }
        //public string AddressLine1 { get; set; }
        //public string AddressLine2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Country { get; set; }
        //public string PostCode { get; set; }
        //public string BankName { get; set; }
        //public string BankAccountNum { get; set; }
        //public string BankAccountName { get; set; }
        //public int MobileOperatorId { get; set; }
        public string MobileAccNum { get; set; }
    }

    [Route("/redemption/list")]
    public class RedemptionList : IReturn<RedemptionListResponse>
    {
    }

    [Route("/redemption/review", "POST")]
    public class ReviewRedemption : IReturn<CustomResponse>
    {
        public int RedemptionId { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
    }

    [Route("/review/", "GET")]
    [Route("/review/redemption/{RedemptionId}", "GET")]
    public class ReviewGetByRedemption : IReturn<ReviewResponse>
    {
        public int RedemptionId { get; set; }
    }

    [Route("/review/list/", "GET")]
    [Route("/review/list/reward/{RewardId}", "GET")]
    public class ReviewListOfReward : IReturn<ReviewListResponse>
    {
        public int RewardId { get; set; }
    }

    [Route("/review/list/user", "GET")]
    public class ReviewListOfUser : IReturn<ReviewListResponse>
    {
    }

    [Route("/reward/type/list")]
    public class RewardTypeList : IReturn<RewardTypeListResponse>
    {
    }

    public class RewardListResponse : RewardResponse
    {
        public List<Reward> Result { get; set; }
    }
    public class RedemptionListResponse : RedemptionResponse
    {
        public List<Redemption> Result { get; set; }
    }
    public class ReviewListResponse : ReviewResponse
    {
        public List<Review> Result { get; set; }
    }

    public class RewardResponse
    {
        public Reward Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

    public class MobileOperatorListResponse
    {
        public List<MobileOperator> Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

    public class RedemptionResponse
    {
        public Redemption Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

    public class ReviewResponse
    {
        public Review Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }

    public class RewardTypeListResponse
    {
        public List<RewardType> Result { get; set; }
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }
}
