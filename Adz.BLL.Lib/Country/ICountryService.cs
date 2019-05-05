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
    public interface ICountryService
    {
        [OperationContract]
        Response<List<City>> GetCityListActive(int CountryId);
        [OperationContract]
        Response<List<Country>> GetCountryListActive();
        [OperationContract]
        Response<City> GetCityById(int CityId);
    }

    [DataContract]
    public class Country
    {
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public string Name { get; set; }
        public string LastAction { get; set; }
    }
    [DataContract]
    public class State
    {
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        public string LastAction { get; set; }
    }
    [DataContract]
    public class City
    {
        [DataMember]
        public int CityId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        public string LastAction { get; set; }
    }
    [DataContract]
    public class Currency
    {
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
}
