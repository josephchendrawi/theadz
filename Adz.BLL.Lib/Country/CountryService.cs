using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class CountryService : ICountryService
    {
        public Response<List<Country>> GetCountryListActive()
        {
            Response<List<Country>> response = null;
            List<Country> CountryList = new List<Country>();
            using (var context = new TheAdzEntities())
            {
                var entityCountry = from d in context.Countries
                                    where d.last_action != "5"
                                    select d;
                foreach (var v in entityCountry)
                {
                    Country Country = new Country();
                    Country.CountryId = v.id;
                    Country.Name = v.name;
                    CountryList.Add(Country);
                }
                response = Response<List<Country>>.Create(CountryList);
            }

            return response;
        }

        public Response<List<City>> GetCityListActive(int CountryId)
        {
            Response<List<City>> response = null;
            List<City> CityList = new List<City>();
            using (var context = new TheAdzEntities())
            {
                var entityCountry = from d in context.Cities.Include("Currency")
                                    where d.country_id == CountryId
                                    && d.last_action != "5"
                                    select d;
                foreach (var v in entityCountry)
                {
                    City City = new City();
                    City.CityId = v.id;
                    City.Name = v.name;
                    //Currency currency = new Currency()
                    //{
                    //    CurrencyId = (int)v.currency_id,
                    //    Name = v.Currency.name,
                    //    Code = v.Currency.cosde
                    //};
                    //City.Currency = currency;
                    CityList.Add(City);
                }
                response = Response<List<City>>.Create(CityList);
            }

            return response;
        }

        public Response<City> GetCityById(int CityId)
        {
            Response<City> response = null;
            City City = new City();
            using (var context = new TheAdzEntities())
            {
                var entityCountry = from d in context.Cities.Include("Currency")
                                    where d.id == CityId
                                    && d.last_action != "5"
                                    select d;

                var v = entityCountry.First();
                City.CityId = CityId;
                City.Name = v.name;

                City.CountryId = (int)v.country_id;
                City.CountryName = v.Country.name;
                
                response = Response<City>.Create(City);
            }

            return response;
        }
    }
}
