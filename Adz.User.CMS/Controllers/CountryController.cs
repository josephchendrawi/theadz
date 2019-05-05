using Adz.BLL.Lib;
using Adz.User.CMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Adz.User.CMS.Controllers
{
    public class CountryController : Controller
    {
        public ICountryService countryservice = new CountryService();

        public JsonResult CountryList()
        {
            List<Adz.BLL.Lib.Country> listR = countryservice.GetCountryListActive().Result;
            List<CountryModel.Country> newlistR = new List<CountryModel.Country>();
            foreach (var v in listR)
            {
                newlistR.Add(new CountryModel.Country() { CountryId = v.CountryId, CountryName = v.Name });
            }
            IEnumerable<CountryModel.Country> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CityList(int countryid = 0, int stateid = 0)
        {
            List<Adz.BLL.Lib.City> listR = countryservice.GetCityListActive(countryid).Result;
            List<CountryModel.City> newlistR = new List<CountryModel.City>();
            foreach (var v in listR)
            {
                newlistR.Add(new CountryModel.City() { CityId = v.CityId, CityName = v.Name });
            }
            IEnumerable<CountryModel.City> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }
	}
}