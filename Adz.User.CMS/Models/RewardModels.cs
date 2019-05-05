using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adz.User.CMS.Models
{
    public class RewardModel
    {
        [Bind(Exclude = "")]
        public class Reward
        {
            [ScaffoldColumn(false)]
            public int RewardId { get; set; }

            //change to Sponsor Name
            [Display(Name = "Reward Name")]
            [StringLength(40, ErrorMessage = "Reward Name cannot be longer than 40 characters.")]
            public string Name { get; set; }

            public string Description { get; set; }
            public DateTime Create { get; set; }
            public DateTime Update { get; set; }
            public string LastAction { get; set; }

            public string SubImageId { get; set; }
            public string SubImageName { get; set; }
            [Display(Name = "Image")]
            public string SubImageUrl { get; set; }
            public string SubImageUrlLink { get; set; }

            [Display(Name = "Criteria")]
            public RewardCriteria RewardCriteria { get; set; }

            [Display(Name = "Type")]
            public RewardType RewardType { get; set; }

            [Display(Name = "Point Requirement")]
            [Range(0, int.MaxValue, ErrorMessage = "Point Requirement must be a positive!")]
            public int PointRequirement { get; set; }

            [Display(Name = "Sub Title")]
            public string SubTitle { get; set; }

            [Required]
            [Display(Name = "Reward Name")]
            [StringLength(40, ErrorMessage = "Reward Name cannot be longer than 40 characters.")]
            public string SponsorName { get; set; }

            [Display(Name = "One Time")]
            public bool OneTimeFlag { get; set; }

            [Display(Name = "Number Of Stock")]
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
        }

        public class Redemption
        {
            public int RedemptionId { get; set; }
            [Display(Name = "Email")]
            public string UserEmail { get; set; }
            public int RewardId { get; set; }
            [Display(Name = "Reward")]
            public string RewardName { get; set; }
            [Display(Name = "Point")]
            public int RewardPointRequirement { get; set; }
            public string Name { get; set; }
            [Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; }
            [Display(Name = "Address Line 2")]
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string PostCode { get; set; }
            [Display(Name = "Bank")]
            public string BankName { get; set; }
            [Display(Name = "Bank Account Number")]
            public string BankAccountNum { get; set; }
            [Display(Name = "Bank Account Name")]
            public string BankAccountName { get; set; }
            public int MobileOperatorId { get; set; }
            [Display(Name = "Mobile Operator")]
            public string MobileOperatorName { get; set; }
            [Display(Name = "Mobile Number")]
            public string MobileAccNum { get; set; }
            public int RedemptionStatusId { get; set; }
            [UIHint("_StatusDropDown")]
            [Display(Name = "Status")]
            public string RedemptionStatusName { get; set; }
            [Display(Name = "Redemption Date")]
            public DateTime RedemptionDate { get; set; }
        }

        public class RedemptionStatus
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class MobileOperator
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Review
        {
            public int RedemptionId { get; set; }
            [Display(Name = "Review Date")]
            public DateTime ReviewDate { get; set; }
            public int Rating { get; set; }
            public string Message { get; set; }
            [Display(Name = "Email")]
            public string ByEmail { get; set; }
            public string LastAction { get; set; }
        }

        public class RedemptionVM
        {
            public Redemption Redemption { get; set; }
            public Review Review { get; set; }
        }

    }
}