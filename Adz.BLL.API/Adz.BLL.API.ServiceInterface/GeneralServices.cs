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
    public class GeneralServices : Service
    {
        public ICountryService countryservice = new CountryService();
        public ILoggerService loggerservice = new LoggerService();
        public IUserService userservice = new UserService();

        public CountryListResponse Any(CountryList request)
        {
            try
            {
                List<Adz.BLL.Lib.Country> listR;
                listR = countryservice.GetCountryListActive().Result;

                List<ServiceModel.Types.Country> newlistR = new List<ServiceModel.Types.Country>();
                foreach (var v in listR)
                {
                    newlistR.Add(new ServiceModel.Types.Country() { CountryId = v.CountryId, Name = v.Name });
                }

                return new CountryListResponse
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
                return new CountryListResponse
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

        public CityListResponse Any(CityList request)
        {
            try
            {
                List<Adz.BLL.Lib.City> listR;
                listR = countryservice.GetCityListActive(request.CountryId).Result;

                List<ServiceModel.Types.City> newlistR = new List<ServiceModel.Types.City>();
                foreach (var v in listR)
                {
                    newlistR.Add(new ServiceModel.Types.City() { CityId = v.CityId, Name = v.Name });
                }

                return new CityListResponse
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
                return new CityListResponse
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

        public CustomResponse Post(MobileLogging request)
        {
            try
            {
                Log Log = new Lib.Log
                {
                    UniqueId = Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).UniqueDeviceId,
                    UserId = userservice.GetUserIdByEmail(Helper.GetUserDataByKey(Request.GetHeader(Helper.APIKeyName)).Email).Result,
                    Request = request.Request,
                    Response = request.Response,
                    URL = request.URL,
                    Activity = request.Activity,
                    IP = base.Request.RemoteIp
                };
                int LogId = loggerservice.MobileLogging(Log).Result;

                if (LogId == 0)
                {
                    throw new CustomException(CustomErrorType.UserServiceDebugFalse);
                }

                return new CustomResponse
                {
                    ResponseStatus = new CustomResponseStatus()
                    {
                        Success = "1",
                        ErrorCode = "0",
                        Message = null
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
