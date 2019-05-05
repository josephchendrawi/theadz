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
    public interface ILoggerService
    {
        Response<int> MobileLogging(Log Log, bool checkFlagDebug = true);
        Response<int> APILogging(APILog Log);
    }

    public class Log
    {
        public string UniqueId { get; set; }
        public int UserId { get; set; }
        public string IP { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }
        public string URL { get; set; }
        public string Activity { get; set; }
    }

    public class APILog
    {
        public string UniqueId { get; set; }
        public int UserId { get; set; }
        public string APIKey { get; set; }
        public string IP { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }
        public string RequestHeader { get; set; }
        public string URL { get; set; }
    }

}
