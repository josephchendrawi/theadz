using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adz.User.CMS.Models
{
    public abstract class CountryModel
    {
        public class Country
        {
            public int CountryId { get; set; }
            public string CountryName { get; set; }
        }
        public class City
        {
            public int CityId { get; set; }
            public string CityName { get; set; }
        }
        public class Currency
        {
            public int CurrencyId { get; set; }
            public string CurrencyName { get; set; }
            public string CurrencyCode { get; set; }
        }
        public class Map
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}