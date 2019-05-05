using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel.Types
{
    public class Reward
    {
        public int RewardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<int> SubImageId { get; set; }
        public List<string> SubImageName { get; set; }
        public List<string> SubImageUrl { get; set; }
        public List<string> SubImageUrlLink { get; set; }

        public int RewardTypeId { get; set; }
        public string RewardTypeName { get; set; }
        public bool RewardTypeDelivery { get; set; }
        public bool RewardTypeMoneyTransfer { get; set; }
        public bool RewardTypeMobile { get; set; }
        public bool RewardTypeReceiver { get; set; }

        public int PointRequired { get; set; }
        public string SubTitle { get; set; }
        public string SponsorName { get; set; }

        public bool OneTimeFlag { get; set; }
    }

    public class RewardType
    {
        public int RewardTypeId { get; set; }
        public string RewardTypeName { get; set; }
        public bool RewardTypeDelivery { get; set; }
        public bool RewardTypeMoneyTransfer { get; set; }
        public bool RewardTypeMobile { get; set; }
        public bool RewardTypeReceiver { get; set; }
    }

    public class MobileOperator
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Redemption
    {
        public int RedemptionId { get; set; }
        public string RewardName { get; set; }
        public string RewardDescription { get; set; }
        public List<string> RewardImageUrl { get; set; }
        public int RewardId { get; set; }
        public int RedemptionTypeId { get; set; }
        public string RedemptionTypeName { get; set; }
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
        public string MobileOperatorName { get; set; }
        public string MobileAccNum { get; set; }
        public int RedemptionStatusId { get; set; }
        public string RedemptionStatusName { get; set; }
        public DateTime RedemptionDate { get; set; }
        public bool Reviewed { get; set; }
    }

    public class Review
    {
        public int RedemptionId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public string ByEmail { get; set; }
    }
}
