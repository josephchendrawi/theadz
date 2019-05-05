using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel.Types
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string LastAction { get; set; }
    }

    public class State
    {
        public int StateId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string LastAction { get; set; }
    }

    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string LastAction { get; set; }
    }

    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
