using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class LoggerService : ILoggerService
    {
        public Response<int> MobileLogging(Log Log, bool checkFlagDebug = true)
        {
            using (var context = new TheAdzEntities())
            {
                var entityAPIKey = from d in context.APIKeys
                                   where d.user_id == Log.UserId
                                   && d.unique_id == Log.UniqueId
                                   select d;

                if (checkFlagDebug == true)
                {
                    entityAPIKey = entityAPIKey.Where(d => d.flag_debug != 0);
                }

                if (entityAPIKey.Count() != 0)
                {
                    Adz.DAL.EF.MobileLog mmentity = new Adz.DAL.EF.MobileLog();
                    mmentity.unique_id = Log.UniqueId;
                    mmentity.user_id = Log.UserId;
                    mmentity.date_created = DateTime.UtcNow;
                    mmentity.request = Log.Request;
                    mmentity.response = Log.Response;
                    mmentity.url = Log.URL;
                    mmentity.activity = Log.Activity;
                    mmentity.ip_address = Log.IP;

                    context.MobileLogs.Add(mmentity);
                    context.SaveChanges();
                    int MobileLogId = mmentity.id;

                    return new Response<int>
                    {
                        Result = MobileLogId
                    };
                }
            }

            return new Response<int>
            {
                Result = 0
            };
        }

        public Response<int> APILogging(APILog Log)
        {
            using (var context = new TheAdzEntities())
            {
                Adz.DAL.EF.APILog mmentity = new Adz.DAL.EF.APILog();
                mmentity.unique_id = Log.UniqueId;
                mmentity.user_id = Log.UserId;
                mmentity.api_key = Log.APIKey;
                mmentity.date_created = DateTime.UtcNow;
                mmentity.request_header = Log.RequestHeader;
                mmentity.request = Log.Request;
                mmentity.response = Log.Response;
                mmentity.url = Log.URL;
                mmentity.ip_address = Log.IP;

                context.APILogs.Add(mmentity);
                context.SaveChanges();
                int APILogId = mmentity.id;

                return new Response<int>
                {
                    Result = APILogId
                };
            }
        }

    }
}
