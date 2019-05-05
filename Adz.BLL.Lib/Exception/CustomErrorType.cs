using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class LocalizedEnumAttribute : DescriptionAttribute
    {
        private PropertyInfo _nameProperty;
        private Type _resourceType;

        public LocalizedEnumAttribute(string displayNameKey)
            : base(displayNameKey)
        {

        }

        public Type NameResourceType
        {
            get
            {
                return _resourceType;
            }
            set
            {
                _resourceType = value;

                _nameProperty = _resourceType.GetProperty(this.Description, BindingFlags.Static | BindingFlags.Public);
            }
        }

        public override string Description
        {
            get
            {
                //check if nameProperty is null and return original display name value
                if (_nameProperty == null)
                {
                    return base.Description;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }

    public static class EnumExtender
    {
        public static string GetLocalizedDescription(this Enum @enum)
        {
            if (@enum == null)
                return null;

            string description = @enum.ToString();

            FieldInfo fieldInfo = @enum.GetType().GetField(description);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any())
                return attributes[0].Description;

            return description;
        }
    }

    ////////-----------------------------------------------------
    /// <summary>
    /// Type of errors.
    /// </summary>
    public enum CustomErrorType
    {
        [Description("Unknown error.")]
        Unknown = 0,
        [Description("Database or entity error.")]
        Data = 1,
        [Description("Unauthorized access.")]
        Unauthorized = 2,
        [Description("Unauthenticated.")]
        Unauthenticated = 3,
        [Description("Image file error.")]
        ImageError = 4,
        [Description("Key Generating error.")]
        KeyGeneratingError = 5,
        
        [Description("Admin service error.")]
        AdminUnknown = 100,
        [Description("Admin token invalid.")]
        AdminNotFound = 101,
        [Description("Admin already assigned.")]
        AdminAlreadyAssign = 102,
        [Description("Admin failed.")]
        AdminFailed = 103,
        [Description("Admin failed delete.")]
        AdminFailedDelete = 104,
        [Description("Email or password invalid.")]
        AdminEmailPasswordWrong = 105,
        [Description("Password invalid.")]
        AdminPasswordWrong = 106,

        [Description("General user service error.")]
        UserServiceUnknown = 1000,
        [Description("Duplicate username.")]
        UserServiceDuplicateUserName = 1001,
        [Description("Invalid password.")]
        UserServiceInvalidPassword = 1002,
        [Description("Duplicate email.")]
        UserServiceDuplicateEmail = 1003,
        [Description("Invalid email.")]
        UserServiceInvalidEmail = 1004,
        [Description("Invalid answer.")]
        UserServiceInvalidAnswer = 1005,
        [Description("Invalid question.")]
        UserServiceInvalidQuestion = 1006,
        [Description("Invalid Username.")]
        UserServiceInvalidUserName = 1007,
        [Description("Provider error.")]
        UserServiceProviderError = 1008,
        [Description("Invalid username or password.")]
        UserServiceInvalidUsernamePassword = 1009,
        [Description("Email address is already taken. Please sign in.")]
        UserServiceAlreadyAssign = 1010,
        [Description("User failed.")]
        UserServiceFailed = 1011,
        [Description("User failed delete.")]
        UserServiceFailedDelete = 1012,
        [Description("User not found.")]
        UserServiceNotFound = 1013,
        [Description("Email or password wrong.")]
        UserServiceEmailPasswordWrong = 1014,
        [Description("Facebook ID exists. Try login instead.")]
        UserServiceFacebookIdExists = 1015,
        [Description("Facebook ID not found")]
        UserServiceFacebookIdNotFound = 1016,
        [Description("The Kuazoo account already have a linked Facebook account")]
        UserServiceLinkFacebookAccFailed = 1017,
        [Description("The facebook friend is already added into friends list")]
        UserServiceAddSameFbFriend = 1018,
        [Description("Linking encountered error")]
        UserServiceLinkError = 1019,
        [Description("Kuazoo only link to Facebook account with same email as registered email")]
        UserServiceFBLinkDifferentEmail = 1025,
        [Description("Password wrong.")]
        UserServicePasswordWrong = 1026,
        [Description("Facebook error.")]
        UserServiceFacebookError = 1027,
        [Description("Facebook Email Problem.")]
        UserServiceFacebookEmailProblem = 1028,
        [Description("You do not have enough points for this.")]
        UserServiceInsufficientPointBalance = 1029,
        [Description("User can't do logging.")]
        UserServiceDebugFalse = 1030,
        [Description("Google not found")]
        UserServiceGoogleNotFound = 1031,

        [Description("Merchant service error.")]
        MerchantUnknown = 2000,
        [Description("Merchant token invalid.")]
        MerchantNotFound = 2001,
        [Description("Merchant already assign.")]
        MerchantAlreadyAssign = 2002,
        [Description("Merchant failed.")]
        MerchantFailed = 2003,
        [Description("Merchant failed delete.")]
        MerchantFailedDelete = 2004,

        [Description("Campaign service error.")]
        CampaignUnknown = 2000,
        [Description("Campaign token invalid.")]
        CampaignNotFound = 2001,
        [Description("Campaign already assign.")]
        CampaignAlreadyAssign = 2002,
        [Description("Campaign failed.")]
        CampaignFailed = 2003,
        [Description("Campaign failed delete.")]
        CampaignFailedDelete = 2004,
        [Description("Campaign expired.")]
        CampaignExpired = 2005,

        [Description("Branch service error.")]
        BranchUnknown = 3000,
        [Description("Branch token invalid.")]
        BranchNotFound = 3001,
        [Description("Branch already assign.")]
        BranchAlreadyAssign = 3002,
        [Description("Branch failed.")]
        BranchFailed = 3003,
        [Description("Branch failed delete.")]
        BranchFailedDelete = 3004,

        [Description("Rule service error.")]
        RuleUnknown = 4000,
        [Description("Rule token invalid.")]
        RuleNotFound = 4001,
        [Description("Rule already assign.")]
        RuleAlreadyAssign = 4002,
        [Description("Rule failed.")]
        RuleFailed = 4003,
        [Description("Rule failed delete.")]
        RuleFailedDelete = 4004,

        [Description("Reward service error.")]
        RewardUnknown = 5000,
        [Description("Reward token invalid.")]
        RewardNotFound = 5001,
        [Description("Reward already assign.")]
        RewardAlreadyAssign = 5002,
        [Description("Reward failed.")]
        RewardFailed = 5003,
        [Description("Reward failed delete.")]
        RewardFailedDelete = 5004,

        [Description("Admin Role service error.")]
        AdminRoleUnknown = 6000,
        [Description("Admin Role token invalid.")]
        AdminRoleNotFound = 6001,
        [Description("Admin Role already assign.")]
        AdminRoleAlreadyAssign = 6002,
        [Description("Admin Role failed.")]
        AdminRoleFailed = 6003,
        [Description("Admin Role failed delete.")]
        AdminRoleFailedDelete = 6004,

        [Description("Admin Role Access service error.")]
        AdminRoleAccessUnknown = 7000,
        [Description("Admin Role Access token invalid.")]
        AdminRoleAccessNotFound = 7001,
        [Description("Admin Role Access already assign.")]
        AdminRoleAccessAlreadyAssign = 7002,
        [Description("Admin Role Access failed.")]
        AdminRoleAccessFailed = 7003,
        [Description("Admin Role Access failed delete.")]
        AdminRoleAccessFailedDelete = 7004,

        [Description("Tag service error.")]
        TagUnknown = 8000,
        [Description("Tag token invalid.")]
        TagNotFound = 8001,
        [Description("Tag already assign.")]
        TagAlreadyAssign = 8002,
        [Description("Tag failed.")]
        TagFailed = 8003,
        [Description("Tag failed delete.")]
        TagFailedDelete = 8004,
        [Description("Tag expired.")]
        TagExpired = 8005,

        [Description("Redemption service error.")]
        RedemptionUnknown = 9000,
        [Description("Redemption token invalid.")]
        RedemptionNotFound = 9001,
        [Description("This reward can only be redeemed once.")]
        RedemptionAlreadyAssign = 9002,
        [Description("Redemption failed.")]
        RedemptionFailed = 9003,
        [Description("Redemption failed delete.")]
        RedemptionFailedDelete = 9004,
        [Description("Redemption is not enough.")]
        RedemptionNotEnough = 9005,
        [Description("This reward is no longer available.")]
        RedemptionOutOfStock = 9006,

        [Description("Review service error.")]
        ReviewUnknown = 10000,
        [Description("Review token invalid.")]
        ReviewNotFound = 10001,
        [Description("Review already assign.")]
        ReviewAlreadyAssign = 10002,
        [Description("Review failed.")]
        ReviewFailed = 10003,
        [Description("Review failed delete.")]
        ReviewFailedDelete = 10004,
        [Description("Review can't be edited.")]
        ReviewUneditable = 10005,

        [Description("Promotion service error.")]
        PromotionUnknown = 11000,
        [Description("Invalid promo code.|Please enter a different promo code.")]
        PromotionNotFound = 11001,
        [Description("Promo code has been redeemed.")]
        PromotionAlreadyAssign = 11002,
        [Description("Promotion failed.")]
        PromotionFailed = 11003,
        [Description("Promotion failed delete.")]
        PromotionFailedDelete = 11004,
        [Description("Promotion can't be edited.")]
        PromotionUneditable = 11005,
        [Description("Stop Promotion failed.")]
        PromotionFailedStop = 11006,
        [Description("Run Promotion failed.")]
        PromotionFailedRun = 11007,
        [Description("Promo code will start shortly. Please try again later.")]
        PromotionHaventStarted = 11008,
        [Description("Promo code has expired.")]
        PromotionAlreadyEnd = 11009,

        [Description("Invalid referral code.|Please enter a different referral code.")]
        ReferralCodeNotFound = 12000,
        [Description("Referral code has been redeemed.")]
        ReferralCodeAlreadyAssign = 12001,
    }
}
