using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel.Types
{
    public class CustomResponseStatus : ResponseStatus
    {
        public string Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string Remarks { get; set; }
    }

    public class CustomResponse
    {
        public CustomResponseStatus ResponseStatus { get; set; }
        public string Key { get; set; }
        public int Debug { get; set; }
    }
}
