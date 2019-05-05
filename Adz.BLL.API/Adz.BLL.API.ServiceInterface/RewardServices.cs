using Adz.BLL.API.ServiceModel;
using Adz.BLL.API.ServiceModel.Types;
using Adz.BLL.Lib;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceInterface
{
    public class RewardServices : Service
    {
        public IRewardService rewardservice = new RewardService();
        public IUserService userservice = new UserService();

        public RewardResponse Get(RewardGet request)
        {
            try
            {
                Adz.BLL.Lib.Reward v = rewardservice.GetRewardById(request.id).Result;
                ServiceModel.Types.Reward r = new ServiceModel.Types.Reward() { RewardId = v.RewardId, Name = v.Name, SponsorName = v.SponsorName, SubTitle = v.SubTitle, RewardTypeName = v.RewardType.RewardTypeName, RewardTypeId = v.RewardType.RewardTypeId, RewardTypeDelivery = v.RewardType.Delivery, RewardTypeMoneyTransfer = v.RewardType.MoneyTransfer, RewardTypeMobile = v.RewardType.Mobile, Description = v.Description, SubImageId = v.SubImageId, SubImageName = v.SubImageName, SubImageUrl = v.SubImageUrl, SubImageUrlLink = v.SubImageUrlLink, PointRequired = v.PointRequirement };

                return new RewardResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Result = r,
                    Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                    Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug
                };
            }
            catch (CustomException ex)
            {
                return new RewardResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public RewardListResponse Any(RewardList request)
        {
            try
            {
                List<Adz.BLL.Lib.Reward> listR;
                listR = rewardservice.GetRewardList().Result;

                List<ServiceModel.Types.Reward> newlistR = new List<ServiceModel.Types.Reward>();
                foreach (var v in listR)
                {
                    if(v.LastAction != "5")
                        newlistR.Add(new ServiceModel.Types.Reward() { RewardId = v.RewardId, Name = v.Name, SubTitle = v.SubTitle, SponsorName = v.SponsorName, RewardTypeName = v.RewardType.RewardTypeName, RewardTypeId = v.RewardType.RewardTypeId, RewardTypeDelivery = v.RewardType.Delivery, RewardTypeMoneyTransfer = v.RewardType.MoneyTransfer, RewardTypeMobile = v.RewardType.Mobile, RewardTypeReceiver = v.RewardType.Receiver, Description = v.Description, SubImageId = v.SubImageId, SubImageName = v.SubImageName, SubImageUrl = v.SubImageUrl, SubImageUrlLink = v.SubImageUrlLink, PointRequired = v.PointRequirement, OneTimeFlag = v.OneTimeFlag });
                }

                return new RewardListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Result = newlistR,
                    Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                };
            }
            catch (CustomException ex)
            {
                return new RewardListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public MobileOperatorListResponse Any(MobileOperatorList request)
        {
            try
            {
                List<Adz.BLL.Lib.MobileOperator> listR;
                listR = rewardservice.GetMobileOperatorList().Result;

                List<ServiceModel.Types.MobileOperator> newlistR = new List<ServiceModel.Types.MobileOperator>();
                foreach (var v in listR)
                {
                    newlistR.Add(new ServiceModel.Types.MobileOperator() { Id = v.Id, Name = v.Name });
                }

                return new MobileOperatorListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Result = newlistR,
                    Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                };
            }
            catch (CustomException ex)
            {
                return new MobileOperatorListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public CustomResponse Post(RedemptionRequest request)
        {
            try
            {
                Adz.BLL.Lib.Redemption redemption = new Adz.BLL.Lib.Redemption()
                {
                    UserEmail = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email,
                    RewardId = request.RewardId,
                    Name = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).FirstName + " " + Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).LastName,
                    /*AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    City = request.City,
                    State = request.State,
                    Country = request.Country,
                    PostCode = request.PostCode,
                    BankName = request.BankName,
                    BankAccountNum = request.BankAccountNum,
                    BankAccountName = request.BankAccountName,*/
                    MobileOperatorId = 1,//request.MobileOperatorId,
                    MobileAccNum = request.MobileAccNum
                };
                int RedemptionId = rewardservice.CreateEditRedemption(redemption).Result;

                if (RedemptionId != null && RedemptionId != 0)
                {
                    return new CustomResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        },
                        Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                    };
                }
                else
                {
                    return new CustomResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "0",
                            ErrorCode = ((int)CustomErrorType.RedemptionFailed).ToString(),
                            Message = EnumExtender.GetLocalizedDescription(CustomErrorType.RedemptionFailed)
                        },
                    };
                }
            }
            catch (CustomException ex)
            {
                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                };
            }
        }

        public RedemptionListResponse Any(RedemptionList request)
        {
            try
            {
                var RedemptionList = rewardservice.GetRedemptionListByUser(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result;

                List<ServiceModel.Types.Redemption> result = new List<ServiceModel.Types.Redemption>();
                foreach (var v in RedemptionList)
                {
                    result.Add(new ServiceModel.Types.Redemption()
                    {
                        RedemptionId = v.RedemptionId,
                        RewardId = (int)v.Reward.RewardId,
                        RewardName = v.Reward.Name,
                        RedemptionTypeId = v.Reward.RewardType.RewardTypeId,
                        RedemptionTypeName = v.Reward.RewardType.RewardTypeName,
                        Name = v.Name,
                        RedemptionDate = v.RedemptionDate,
                        RedemptionStatusId = v.RedemptionStatus.Id,
                        RedemptionStatusName = v.RedemptionStatus.Name,
                        AddressLine1 = v.AddressLine1,
                        AddressLine2 = v.AddressLine2,
                        City = v.City,
                        State = v.State,
                        Country = v.Country,
                        PostCode = v.PostCode,
                        BankName = v.BankName,
                        BankAccountNum = v.BankAccountNum,
                        BankAccountName = v.BankAccountName,
                        MobileAccNum = v.MobileAccNum,
                        MobileOperatorId = (int)v.MobileOperator.Id,
                        MobileOperatorName = v.MobileOperator.Name,
                        Reviewed = v.Reviewed,
                        RewardDescription = v.Reward.Description,
                        RewardImageUrl = v.Reward.SubImageUrl
                    });
                }

                return new RedemptionListResponse
                {
                    Result = result,
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                };
            }
            catch (CustomException ex)
            {
                return new RedemptionListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public CustomResponse Post(ReviewRedemption request)
        {
            try
            {
                Adz.BLL.Lib.Review Review = new Adz.BLL.Lib.Review()
                {
                    RedemptionId = request.RedemptionId,
                    Message = request.Message,
                    Rating = request.Rating
                };
                int RedemptionId = rewardservice.CreateEditReview(Review, false).Result;

                if (RedemptionId != null && RedemptionId != 0)
                {
                    return new CustomResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "1",
                            ErrorCode = "0",
                            Message = null
                        },
                        Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                    };
                }
                else
                {
                    return new CustomResponse
                    {
                        ResponseStatus = new CustomResponseStatus()
                        {
                            Success = "0",
                            ErrorCode = ((int)CustomErrorType.ReviewFailed).ToString(),
                            Message = EnumExtender.GetLocalizedDescription(CustomErrorType.ReviewFailed)
                        },
                    };
                }
            }
            catch (CustomException ex)
            {
                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                };
            }
        }

        public ReviewResponse Get(ReviewGetByRedemption request)
        {
            try
            {
                Adz.BLL.Lib.Review v = rewardservice.GetReviewByRedemptionId(request.RedemptionId).Result;
                ServiceModel.Types.Review r = new ServiceModel.Types.Review() { RedemptionId = v.RedemptionId, ReviewDate = v.ReviewDate, Message = v.Message, Rating = v.Rating, ByEmail = v.ByEmail };

                return new ReviewResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Result = r,
                    Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                    Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug
                };
            }
            catch (CustomException ex)
            {
                return new ReviewResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public ReviewListResponse Any(ReviewListOfReward request)
        {
            try
            {
                var ReviewList = rewardservice.GetReviewListByRewardId(request.RewardId).Result;

                List<ServiceModel.Types.Review> result = new List<ServiceModel.Types.Review>();
                foreach (var v in ReviewList)
                {
                    result.Add(new ServiceModel.Types.Review()
                    {
                        RedemptionId = v.RedemptionId,
                        Message = v.Message,
                        Rating = v.Rating,
                        ReviewDate = v.ReviewDate,
                        ByEmail = v.ByEmail
                    });
                }

                return new ReviewListResponse
                {
                    Result = result,
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                };
            }
            catch (CustomException ex)
            {
                return new ReviewListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public ReviewListResponse Any(ReviewListOfUser request)
        {
            try
            {
                var ReviewList = rewardservice.GetReviewListByUserId(userservice.GetUserIdByEmail(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result).Result;

                List<ServiceModel.Types.Review> result = new List<ServiceModel.Types.Review>();
                foreach (var v in ReviewList)
                {
                    result.Add(new ServiceModel.Types.Review()
                    {
                        RedemptionId = v.RedemptionId,
                        Message = v.Message,
                        Rating = v.Rating,
                        ReviewDate = v.ReviewDate,
                        ByEmail = v.ByEmail
                    });
                }

                return new ReviewListResponse
                {
                    Result = result,
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                };
            }
            catch (CustomException ex)
            {
                return new ReviewListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

        public object Any(RewardTypeList request)
        {
            try
            {
                List<Adz.BLL.Lib.RewardType> listR;
                listR = rewardservice.GetRewardTypeList().Result;

                var temp = "";
                List<ServiceModel.Types.RewardType> newlistR = new List<ServiceModel.Types.RewardType>();
                foreach (var v in listR)
                {
                    if(v.RewardTypeName != temp)
                        newlistR.Add(new ServiceModel.Types.RewardType() { RewardTypeId = v.RewardTypeId, RewardTypeName = v.RewardTypeName, RewardTypeDelivery = v.Delivery, RewardTypeMoneyTransfer = v.MoneyTransfer, RewardTypeMobile = v.Mobile, RewardTypeReceiver = v.Receiver });
                    temp = v.RewardTypeName;
                }

                return new RewardTypeListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
                    },
                    Result = newlistR,
                    Key = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null ? Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }) : null
                };
            }
            catch (CustomException ex)
            {
                return new RewardTypeListResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "0",
                        ErrorCode = ((int)ex.ErrorType).ToString(),
                        Message = EnumExtender.GetLocalizedDescription(ex.ErrorType)
                    },
                    Result = null
                };
            }
        }

    }
}
