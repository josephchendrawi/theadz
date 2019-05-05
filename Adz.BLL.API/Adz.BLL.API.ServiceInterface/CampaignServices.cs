using Adz.BLL.API.ServiceModel;
using Adz.BLL.API.ServiceModel.Types;
using Adz.BLL.Lib;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Adz.BLL.API.ServiceInterface
{
    public class CampaignServices : Service
    {
        public ICampaignService campaignservice = new CampaignService();
        public ILoggerService loggerservice = new LoggerService();
        public IUserService userservice = new UserService();

        public CampaignResponse Get(CampaignGet request)
        {
            try
            {
                Adz.BLL.Lib.Campaign v = campaignservice.GetCampaignById(request.id).Result;
                ServiceModel.Types.Campaign r = new ServiceModel.Types.Campaign() { CampaignId = v.CampaignId, Name = v.Name, MerchantId = v.Merchant.MerchantId, MerchantName = v.Merchant.Name, Description = v.Description, /*LastAction = v.LastAction,*/ Start = v.Start.AddHours(Helper.defaultGMT), End = v.End.AddHours(Helper.defaultGMT), /*Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT),*/ SubImageId = v.SubImageId, SubImageName = v.SubImageName, SubImageUrl = v.SubImageUrl, SubImageUrlLink = v.SubImageUrlLink, SubImageHeight = v.SubImageHeight, SubImageWidth = v.SubImageWidth, SubImageCheckSum = v.SubImageCheckSum, LinkURL = v.LinkURL };

                return new CampaignResponse
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
                return new CampaignResponse
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

        public CampaignListResponse Any(CampaignList request)
        {
            try
            {
                List<Adz.BLL.Lib.Campaign> listR;
                if (Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey != null)
                {                    
                    int Age = -1;
                    if (Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).DateOfBirth > DateTime.MinValue)
                    {
                        Age = DateTime.Today.Year - Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).DateOfBirth.Value.Year;
                        if (Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).DateOfBirth > DateTime.Today.AddYears(-Age)) Age--;
                    }
                    listR = campaignservice.GetNowCampaignList(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Gender, request.latitude, request.longitude, Age).Result;
                }
                else
                {
                    //listR = campaignservice.GetAllCampaignList().Result;
                    listR = new List<Lib.Campaign>();
                }

                List<ServiceModel.Types.Campaign> newlistR = new List<ServiceModel.Types.Campaign>();
                foreach (var v in listR)
                {
                    newlistR.Add(new ServiceModel.Types.Campaign() { CampaignId = v.CampaignId, Name = v.Name, MerchantId = v.Merchant.MerchantId, MerchantName = v.Merchant.Name, /*Description = v.Description, LastAction = v.LastAction, Start = v.Start.AddHours(Helper.defaultGMT), End = v.End.AddHours(Helper.defaultGMT), Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT),*/ ImageId = v.SubImageId.First(), ImageName = v.SubImageName.FirstOrDefault(), ImageUrl = v.SubImageUrl.FirstOrDefault(), ImageUrlLink = v.SubImageUrlLink.FirstOrDefault(), ImageWidth = v.SubImageWidth.FirstOrDefault(), ImageHeight = v.SubImageHeight.FirstOrDefault(), ImageCheckSum = v.SubImageCheckSum.FirstOrDefault(), LinkURL = v.LinkURL, SubImageId = v.SubImageId, SubImageName = v.SubImageName, SubImageUrl = v.SubImageUrl, SubImageUrlLink = v.SubImageUrlLink });
                }

                return new CampaignListResponse
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
                return new CampaignListResponse
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

        public CampaignResponse Post(CampaignView request)
        {
            try
            {
                Adz.BLL.Lib.Campaign v = campaignservice.ViewThisCampaign(request.id, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Gender, request.latitude, request.longitude, null, null, -1).Result;
                ServiceModel.Types.Campaign r = new ServiceModel.Types.Campaign() { CampaignId = v.CampaignId, Name = v.Name, MerchantId = v.Merchant.MerchantId, MerchantName = v.Merchant.Name, Description = v.Description, /*LastAction = v.LastAction,*/ Start = v.Start.AddHours(Helper.defaultGMT), End = v.End.AddHours(Helper.defaultGMT), /*Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT),*/ SubRuleId = v.SubRuleId, SubImageId = v.SubImageId, SubImageName = v.SubImageName, SubImageUrl = v.SubImageUrl, SubImageUrlLink = v.SubImageUrlLink, SubImageHeight = v.SubImageHeight, SubImageWidth = v.SubImageWidth, SubImageCheckSum = v.SubImageCheckSum, LinkURL = v.LinkURL };

                return new CampaignResponse
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
                return new CampaignResponse
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

        public CampaignIncrementResponse Post(CampaignViewIncrement request)
        {
            try
            {
                int v = campaignservice.IncrementCampaignView(request.id, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Gender, request.latitude, request.longitude).Result;
                Views r = new Views();

                if (v < 0)
                {
                    r.StillValid = false;
                    v = v * -1;
                }
                else
                {
                    r.StillValid = true;
                }

                r.NumberOfView = v;

                return new CampaignIncrementResponse
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
                return new CampaignIncrementResponse
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

        public CustomResponse Post(CampaignUsageSync request)
        {
            try
            {
                string msg = "";
                foreach (var v in request.Usage)
                {
                    try
                    {
                        Adz.BLL.Lib.Campaign r = campaignservice.ViewThisCampaign(v.CampaignId, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Gender, v.Latitude, v.Longitude, v.ViewTime, v.TimeZone, v.Action).Result;
                    }
                    catch (Exception e)
                    {
                        if (msg == "")
                            msg = "Failed : " + v.CampaignId;
                        else
                            msg += ", " + v.CampaignId;
                    }
                }

                try
                {
                    loggerservice.MobileLogging(new Log()
                    {
                        UniqueId = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId,
                        UserId = userservice.GetUserIdByEmail(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result,
                        Request = new JavaScriptSerializer().Serialize(request),
                        Response = msg,
                        URL = "/campaign/sync",
                        Activity = "CampaignUsageSync",
                        IP = base.Request.RemoteIp
                    }, false);
                }
                catch { }

                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = msg
                    },
                    Key = Helper.EncodeTo64(new string[] { General.RegenerateAPIKey(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).APIKey, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId), Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email, Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId }),
                    Debug = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Debug
                };
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
                    }
                };
            }
        }

    }
}
