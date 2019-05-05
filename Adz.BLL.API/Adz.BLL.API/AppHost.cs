using Funq;
using ServiceStack;
using Adz.BLL.API.ServiceInterface;
using ServiceStack.Auth;
using Adz.BLL.Lib;
using System;
using System.Configuration;
using Adz.BLL.API.ServiceModel.Types;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Adz.BLL.API
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("Adz.BLL.API", typeof(MyServices).Assembly)
        {

        }

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());

            Plugins.Add(new SessionFeature());

            this.PreRequestFilters.Add((req, res) =>
            {
                //if (!noAuth.Contains(req.GetAbsolutePath().ToLower()))
                if (!req.GetAbsolutePath().ToLower().Contains("/user/login") && !req.GetAbsolutePath().ToLower().Contains("/user/signup") && !req.GetAbsolutePath().ToLower().Contains("/user/resetpassword") && !req.GetAbsolutePath().ToLower().Contains("/user/loginfb"))
                {
                    //AuthUserSession session = (AuthUserSession)req.GetSession();
                    //if (session.IsAuthenticated == false || session.Email == null || session.Email == "" || session.RequestTokenSecret == null || session.RequestTokenSecret == "" || session.UserAuthId == null || session.UserAuthId == "")
                    if (req.GetHeader(Helper.APIKeyName) == null || req.GetHeader(Helper.APIKeyName) == "")
                    {
                        if (!req.GetAbsolutePath().ToLower().Contains("/campaign/list"))
                        {
                            throw new CustomException(CustomErrorType.Unauthenticated);
                        }
                    }
                    else
                    {
                        try
                        {
                            var decoded = Helper.DecodeFrom64(req.GetHeader(Helper.APIKeyName));
                            //                                       (---api-key---,--email--,--device-uniqueid--)
                            Adz.BLL.Lib.User user = General.checkAuth(decoded[0], decoded[1], decoded[2]);

                            //new version store in temp object
                            /*Helper.TempUserData.FirstName = user.FirstName;
                            Helper.TempUserData.LastName = user.LastName;
                            Helper.TempUserData.Email = user.Email;
                            Helper.TempUserData.Gender = user.Gender;
                            Helper.TempUserData.DateOfBirth = user.DateOfBirth;
                            Helper.TempUserData.APIKey = decoded[0];
                            Helper.TempUserData.UniqueDeviceId = decoded[2];
                            Helper.TempUserData.Debug = user.Debug;*/

                            //old version store session
                            /*
                            session.FirstName = user.FirstName;
                            session.LastName = user.LastName;
                            session.Email = user.Email;
                            session.Gender = user.Gender.ToString();
                            session.BirthDate = user.DateOfBirth;

                            session.RequestTokenSecret = decoded[0];    //api-key
                            session.UserAuthId = decoded[2];    //unique-id
                            session.Tag = user.Debug;   //debug

                            req.SaveSession(session);//, new TimeSpan(TimeSpan.TicksPerHour * int.Parse(ConfigurationManager.AppSettings["sessiontimeout_hour"]))); //extend timeout
                            */
                        }
                        catch
                        {
                            throw new CustomException(CustomErrorType.Unauthenticated);
                        }
                    }
                }
            });

            this.GlobalResponseFilters.Add((req, res, dto) =>
            {
                /*try
                {
                    ILoggerService loggerservice = new LoggerService();
                    IUserService userservice = new UserService();

                    var UserId = -1;
                    try{
                        UserId = userservice.GetUserIdByEmail(Helper.GetUserDataByKey(req.GetHeader(Helper.APIKeyName)).Email ?? "").Result;
                    }
                    catch{}
                    loggerservice.APILogging(new APILog()
                    {
                        UniqueId = Helper.GetUserDataByKey(req.GetHeader(Helper.APIKeyName)).UniqueDeviceId ?? "",
                        UserId = UserId,
                        APIKey = Helper.GetUserDataByKey(req.GetHeader(Helper.APIKeyName)).APIKey ?? "",
                        RequestHeader = req.Headers.ToString(),                          
                        Request = new JavaScriptSerializer().Serialize(req.Dto),
                        Response = new JavaScriptSerializer().Serialize(dto),
                        URL = req.AbsoluteUri,
                        IP = req.RemoteIp,                        
                    });
                }
                catch { }*/

                //req.RemoveSession();
            });

            this.ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                //database logging
                //General.LoggingException(httpReq.RemoteIp, httpReq.AbsoluteUri, exception.ToString(), exception.Message, httpReq.GetHeader("username"));
                General.LoggingException(httpReq.RemoteIp, httpReq.AbsoluteUri, exception.ToString(), exception.Message, (Helper.GetUserDataByKey(httpReq.GetHeader(Helper.APIKeyName)).Email ?? ""));

                //file-based loggin
                //logger.Error(httpReq.RemoteIp + "\t" + httpReq.AbsoluteUri + "\t" + exception.GetType() + "\t" + exception.Message + "\t" + httpReq.GetHeader("username") + Environment.NewLine + exception.StackTrace);
                logger.Error(httpReq.RemoteIp + "\t" + httpReq.AbsoluteUri + "\t" + exception.GetType() + "\t" + exception.Message + "\t" + (Helper.GetUserDataByKey(httpReq.GetHeader(Helper.APIKeyName)).Email ?? "") + Environment.NewLine + exception.StackTrace);

                return DtoUtils.CreateErrorResponse(request, exception, new CustomResponseStatus() { Success = "0", ErrorCode = exception.ToErrorCode(), Message = exception.Message });
            });

        }

    }
}