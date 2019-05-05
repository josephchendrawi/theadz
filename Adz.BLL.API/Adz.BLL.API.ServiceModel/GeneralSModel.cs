using Adz.BLL.API.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel
{
    [Route("/country/list")]
    public class CountryList : IReturn<CountryListResponse>
    {
    }

    [Route("/city/list/")]
    [Route("/city/list/{CountryId}", "GET")]
    public class CityList : IReturn<CityListResponse>
    {
        public int CountryId { get; set; }
    }

    [Route("/logging", "POST")]
    public class MobileLogging : IReturn<CustomResponse>
    {
        public string Response { get; set; }
        public string Request { get; set; }
        public string URL { get; set; }
        public string Activity { get; set; }
    }

    public class CityListResponse : CustomResponse
    {
        public List<City> Result { get; set; }
    }

    public class CountryListResponse : CustomResponse
    {
        public List<Country> Result { get; set; }
    }
}
