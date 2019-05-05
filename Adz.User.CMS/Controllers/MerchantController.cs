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
    public class MerchantController : BaseAdminController
    {
        public IMerchantService merchantservice = new MerchantService();
        public IBranchService branchservice = new BranchService();
        public ICountryService countryservice = new CountryService();

        [AdminAuthorize("MERCHANT", "VIEW")]
        public ActionResult Merchant(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }

        [AdminAuthorize("MERCHANT", "VIEW")]
        public ActionResult MerchantArchive()
        {
            return View();
        }

        public ActionResult Merchant_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.Merchant> listR = merchantservice.GetMerchantList().Result;
            List<MerchantModel.Merchant> newlistR = new List<MerchantModel.Merchant>();
            foreach (var v in listR)
            {
                newlistR.Add(new MerchantModel.Merchant() { Create = v.Create.AddHours(Helper.defaultGMT), MerchantId = v.MerchantId, Name = v.Name, CountryId = v.Country.CountryId, Country = new CountryModel.Country() { CountryId = v.Country.CountryId, CountryName = v.Country.Name }, AddressLine1 = v.AddressLine1, AddressLine2 = v.AddressLine2, CityId = v.City.CityId, City = new CountryModel.City() { CityId = v.City.CityId, CityName = v.City.Name }, PostCode = v.PostCode, ContactNumber = v.ContactNumber, Email = v.Email, Website = v.Website, Facebook = v.Facebook, Latitude = v.Latitude, Longitude = v.Longitude, StatusId = v.Status.StatusId, LastAction = v.LastAction = v.LastAction, Description = v.Description, SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink) });
            }
            IEnumerable<MerchantModel.Merchant> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize("MERCHANT", "VIEW")]
        public ActionResult MerchantView(int MerchantId, string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            Adz.BLL.Lib.Merchant v = merchantservice.GetMerchantById(MerchantId).Result;

            List<CampaignModel.Campaign> campaigns = new List<CampaignModel.Campaign>();
            foreach (var c in v.Campaigns)
            {
                campaigns.Add(new CampaignModel.Campaign() { CampaignId = c.CampaignId, Name = c.Name });
            }

            List<MerchantModel.Branch> branches = new List<MerchantModel.Branch>();
            foreach (var c in v.Branches)
            {
                branches.Add(new MerchantModel.Branch()
                {
                    BranchId = c.BranchId,
                    AddressLine1 = c.AddressLine1,
                    AddressLine2 = c.AddressLine2,
                    City = new CountryModel.City { CityId = (int)c.City.CityId, CityName = c.City.Name },
                    Country = new CountryModel.Country { CountryId = (int)c.Country.CountryId, CountryName = c.Country.Name },
                    LastAction = c.LastAction,
                    Latitude = (double)c.Latitude,
                    Longitude = (double)c.Longitude,
                    PostCode = c.PostCode
                });
            }

            MerchantModel.Merchant model = new MerchantModel.Merchant() { MerchantId = v.MerchantId, Name = v.Name, CountryId = v.Country.CountryId, Country = new CountryModel.Country() { CountryId = v.Country.CountryId, CountryName = v.Country.Name }, AddressLine1 = v.AddressLine1, AddressLine2 = v.AddressLine2, CityId = v.City.CityId, City = new CountryModel.City() { CityId = v.City.CityId, CityName = v.City.Name }, PostCode = v.PostCode, ContactNumber = v.ContactNumber, Email = v.Email, Website = v.Website, Facebook = v.Facebook, Latitude = v.Latitude, Longitude = v.Longitude, StatusId = v.Status.StatusId, LastAction = v.LastAction = v.LastAction, Description = v.Description, SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), Campaigns = campaigns, Branches = branches };

            return View(model);
        }

        [AdminAuthorize("MERCHANT", "EDIT")]
        public ActionResult MerchantEdit(int MerchantId)
        {
            Adz.BLL.Lib.Merchant v = merchantservice.GetMerchantById(MerchantId).Result;

            List<MerchantModel.Branch> branches = new List<MerchantModel.Branch>();
            foreach (var c in v.Branches)
            {
                branches.Add(new MerchantModel.Branch()
                {
                    BranchId = c.BranchId,
                    AddressLine1 = c.AddressLine1,
                    AddressLine2 = c.AddressLine2,
                    City = new CountryModel.City { CityId = (int)c.City.CityId, CityName = c.City.Name },
                    Country = new CountryModel.Country { CountryId = (int)c.Country.CountryId, CountryName = c.Country.Name },
                    LastAction = c.LastAction,
                    Latitude = (double)c.Latitude,
                    Longitude = (double)c.Longitude,
                    PostCode = c.PostCode
                });
            }

            MerchantModel.Merchant model = new MerchantModel.Merchant() { MerchantId = v.MerchantId, Name = v.Name, CountryId = v.Country.CountryId, Country = new CountryModel.Country() { CountryId = v.Country.CountryId, CountryName = v.Country.Name }, AddressLine1 = v.AddressLine1, AddressLine2 = v.AddressLine2, CityId = v.City.CityId, City = new CountryModel.City() { CityId = v.City.CityId, CityName = v.City.Name }, PostCode = v.PostCode, ContactNumber = v.ContactNumber, Email = v.Email, Website = v.Website, Facebook = v.Facebook, Latitude = v.Latitude, Longitude = v.Longitude, StatusId = v.Status.StatusId, LastAction = v.LastAction = v.LastAction, Description = v.Description, SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), Branches = branches };

            return View(model);
        }

        [AdminAuthorize("MERCHANT", "EDIT")]
        [HttpPost]
        public ActionResult MerchantEdit(FormCollection collection, MerchantModel.Merchant Merchant)
        {
            if (collection.Get("countryDropDown") == null || collection.Get("countryDropDown") == "")
            {
                ModelState.AddModelError("CountryNull", "The Country field is required.");
            }
            if (collection.Get("cityDropDown") == null || collection.Get("cityDropDown") == "")
            {
                ModelState.AddModelError("CityNull", "The City field is required.");
            }

            if (Merchant.Branches == null)
            {
                Merchant.Branches = new List<MerchantModel.Branch>();
            }

            if (ModelState.ContainsKey("Branches"))
                ModelState["Branches"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Merchant mer = new Adz.BLL.Lib.Merchant();
                    mer.MerchantId = Merchant.MerchantId;
                    mer.Name = Merchant.Name;
                    mer.AddressLine1 = Merchant.AddressLine1;
                    mer.AddressLine2 = Merchant.AddressLine2;
                    Adz.BLL.Lib.City city = new Adz.BLL.Lib.City();
                    city.CityId = Merchant.CityId;
                    mer.City = city;
                    mer.PostCode = Merchant.PostCode;
                    mer.ContactNumber = Merchant.ContactNumber;
                    mer.Email = Merchant.Email;
                    mer.Website = Merchant.Website;
                    mer.Facebook = Merchant.Facebook;
                    Adz.BLL.Lib.Status stat = new Adz.BLL.Lib.Status();
                    stat.StatusId = Merchant.StatusId;
                    mer.Status = stat;
                    mer.Description = Merchant.Description;
                    if (Merchant.SubImageId != null && Merchant.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = Merchant.SubImageId.Split(',');
                        foreach (var v in subimagelist)
                        {
                            if (v != "")
                            {
                                subimg.Add(int.Parse(v));
                                subimglink.Add(collection.Get("txtSubImg" + v));
                            }
                        }
                        mer.SubImageId = subimg;
                        mer.SubImageUrlLink = subimglink;
                    }

                    string tDelete = collection.Get("tempDelete");
                    string iEdit = collection.Get("itemEdit");
                    string iAdd = collection.Get("itemAdd");

                    if (tDelete != null && tDelete != "")
                    {
                        foreach (var v in tDelete.Split('#'))
                        {
                            branchservice.DeleteBranch(int.Parse(v));
                        }
                    }
                    if (iEdit != null && iEdit != "")
                    {
                        foreach (var v in iEdit.Split('|'))
                        {
                            var branch = v.Split('$');
                            Adz.BLL.Lib.Branch br = new Adz.BLL.Lib.Branch();
                            br.BranchId = int.Parse(branch[0]);
                            br.AddressLine1 = branch[1];
                            br.City = new Adz.BLL.Lib.City();
                            br.City.CityId = int.Parse(branch[2]);
                            br.PostCode = branch[3];
                            br.Latitude = double.Parse(branch[4]);
                            br.Longitude = double.Parse(branch[5]);

                            branchservice.CreateEditBranch(br);
                        }
                    }
                    if (iAdd != null && iAdd != "")
                    {
                        foreach (var v in iAdd.Split('|'))
                        {
                            var branch = v.Split('$');
                            Adz.BLL.Lib.Branch br = new Adz.BLL.Lib.Branch();
                            br.BranchId = 0;
                            br.AddressLine1 = branch[0];
                            br.City = new Adz.BLL.Lib.City();
                            br.City.CityId = int.Parse(branch[1]);
                            br.PostCode = branch[2];
                            br.Latitude = double.Parse(branch[3]);
                            br.Longitude = double.Parse(branch[4]);
                            br.Merchant = new Adz.BLL.Lib.Merchant();
                            br.Merchant.MerchantId = Merchant.MerchantId;

                            branchservice.CreateEditBranch(br);
                        }
                    }

                    int result = merchantservice.CreateEditMerchant(mer).Result;
                    string r;
                    if (result != 0)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("MerchantView", "Merchant", new { MerchantId = Merchant.MerchantId, r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Merchant);
        }

        [AdminAuthorize("MERCHANT", "ADD")]
        public ActionResult MerchantAdd()
        {
            MerchantModel.Merchant model = new MerchantModel.Merchant();
            model.Country = new CountryModel.Country();
            model.City = new CountryModel.City();
            model.Branches = new List<MerchantModel.Branch>();
            return View(model);
        }

        [AdminAuthorize("MERCHANT", "ADD")]
        [HttpPost]
        public ActionResult MerchantAdd(FormCollection collection, MerchantModel.Merchant Merchant)
        {
            if (collection.Get("countryDropDown") == null || collection.Get("countryDropDown") == "")
            {
                ModelState.AddModelError("CountryNull", "The Country field is required.");
            }

            if (collection.Get("cityDropDown") == null || collection.Get("cityDropDown") == "")
            {
                ModelState.AddModelError("CityNull", "The City field is required.");
            }

            if (ModelState.ContainsKey("Branches"))
                ModelState["Branches"].Errors.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    MerchantModel mv = new MerchantModel();

                    Adz.BLL.Lib.Merchant mer = new Adz.BLL.Lib.Merchant();
                    mer.MerchantId = 0;
                    mer.Name = collection.Get("Name");
                    mer.AddressLine1 = collection.Get("AddressLine1");
                    mer.AddressLine2 = collection.Get("AddressLine2");
                    Adz.BLL.Lib.City city = new Adz.BLL.Lib.City();
                    city.CityId = Convert.ToInt16(collection.Get("CityDropDown").ToString());
                    mer.City = city;
                    mer.PostCode = collection.Get("PostCode");
                    mer.ContactNumber = collection.Get("ContactNumber");
                    mer.Email = collection.Get("Email");
                    mer.Website = collection.Get("Website");
                    mer.Facebook = collection.Get("Facebook");

                    Adz.BLL.Lib.Status stat = new Adz.BLL.Lib.Status();
                    stat.StatusId = (int)MerchantStatus.Premium;
                    mer.Status = stat;
                    mer.Description = Merchant.Description;

                    if (Merchant.SubImageId != null && Merchant.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = Merchant.SubImageId.Split(',');
                        foreach (var v in subimagelist)
                        {
                            if (v != "")
                            {
                                subimg.Add(int.Parse(v));
                                subimglink.Add(collection.Get("txtSubImg" + v));
                            }
                        }
                        mer.SubImageId = subimg;
                        mer.SubImageUrlLink = subimglink;
                    }

                    int result = merchantservice.CreateEditMerchant(mer).Result;

                    string iAdd = collection.Get("itemAdd");

                    if (iAdd != null && iAdd != "")
                    {
                        foreach (var v in iAdd.Split('|'))
                        {
                            var branch = v.Split('$');
                            Adz.BLL.Lib.Branch br = new Adz.BLL.Lib.Branch();
                            br.BranchId = 0;
                            br.AddressLine1 = branch[0];
                            br.City = new Adz.BLL.Lib.City();
                            br.City.CityId = int.Parse(branch[1]);
                            br.PostCode = branch[2];
                            br.Latitude = double.Parse(branch[3]);
                            br.Longitude = double.Parse(branch[4]);
                            br.Merchant = new Adz.BLL.Lib.Merchant();
                            br.Merchant.MerchantId = result;

                            branchservice.CreateEditBranch(br);
                        }
                    }

                    string r;
                    if (result != 0)
                    {
                        r = "a1";
                    }
                    else
                    {
                        r = "a2";
                    }
                    return RedirectToAction("Merchant", "Merchant", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Merchant);
        }

        [AdminAuthorize("MERCHANT", "DELETE")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Merchant_Destroy([DataSourceRequest] DataSourceRequest request, MerchantModel.Merchant Merchant)
        {
            try
            {
                if (Merchant != null)
                {
                    Boolean result = merchantservice.DeleteMerchant(Merchant.MerchantId).Result;
                }
            }
            catch (Exception ex)
            {
                CustomException(ex);
                ModelState.AddModelError("", ex.Message);
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [AdminAuthorize("MERCHANT", "ADD")]
        public String Merchant_Duplicate(int MerchantId)
        {
            string res = "";
            if (MerchantId != null && MerchantId > 0)
            {
                try
                {
                    Boolean result = merchantservice.DuplicateMerchant(MerchantId).Result;

                    if (result) res = "1";
                    else res = "2";
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    res = ex.Message;
                }

            }
            return res;
        }

        public JsonResult MerchantList()
        {
            List<Adz.BLL.Lib.Merchant> listR = merchantservice.GetMerchantList().Result;
            List<MerchantModel.Merchant> newlistR = new List<MerchantModel.Merchant>();
            listR = listR.Where(x => x.LastAction != "5").ToList();
            foreach (var v in listR)
            {
                newlistR.Add(new MerchantModel.Merchant() { MerchantId = v.MerchantId, Name = v.Name });
            }
            IEnumerable<MerchantModel.Merchant> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BranchEdit(string bid, string address = "", int cityid = 0, string city = "", int countryid = 0, string country = "", string postcode = "", double latitude = 0, double longitude = 0)
        {
            MerchantModel.Branch model = new MerchantModel.Branch()
            {
                BranchId = bid.IndexOf("temp") == -1 ? int.Parse(bid) : (int.Parse(bid.Substring(4)) * -1), //if temp then branchid = negative 
                AddressLine1 = address,
                City = new CountryModel.City { CityId = cityid, CityName = city },
                Country = new CountryModel.Country { CountryName = country, CountryId = countryid },
                Latitude = latitude,
                Longitude = longitude,
                PostCode = postcode
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult BranchEdit(FormCollection collection, MerchantModel.Branch Branch)
        {
            int invalid = 0;
            if (collection.Get("countryDropDown") == null || collection.Get("countryDropDown") == "")
            {
                ModelState.AddModelError("CountryNull", "The Country field is required.");
                invalid++;
                Branch.Country = new CountryModel.Country() { CountryId = int.Parse(collection["country-val"]) };
            }
            if (collection.Get("cityDropDown") == null || collection.Get("cityDropDown") == "")
            {
                ModelState.AddModelError("CityNull", "The City field is required.");
                invalid++;
                Branch.City = new CountryModel.City() { CityId = int.Parse(collection["city-val"]) };
            }

            if (invalid == 0)
            {

                try
                {
                    string coord = collection.Get("TempLatLong");
                    char[] delimChars = { ',' };
                    string[] seperated = coord.Split(delimChars);

                    int length1 = seperated[0].Length;
                    int length2 = seperated[1].Length;
                    Branch.Latitude = float.Parse(seperated[0].Substring(1, length1 - 1));
                    Branch.Longitude = float.Parse(seperated[1].Substring(0, length2 - 2));
                }
                catch
                {
                    Branch.Latitude = 0;
                    Branch.Longitude = 0;
                }


                var city = countryservice.GetCityById(int.Parse(collection.Get("cityDropDown"))).Result;
                Branch.Country = new CountryModel.Country
                {
                    CountryId = city.CountryId,
                    CountryName = city.CountryName
                };

                Branch.City = new CountryModel.City
                {
                    CityId = city.CityId,
                    CityName = city.Name
                };

                ViewBag.msg = "e1";
            }

            return View(Branch);
        }

        public ActionResult BranchAdd(int merchantId)
        {
            MerchantModel.Branch model = new MerchantModel.Branch()
            {
                Merchant = new MerchantModel.Merchant() { MerchantId = merchantId }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult BranchAdd(FormCollection collection, MerchantModel.Branch Branch)
        {
            int invalid = 0;
            if (collection.Get("countryDropDown") == null || collection.Get("countryDropDown") == "")
            {
                ModelState.AddModelError("CountryNull", "The Country field is required.");
                invalid++;
            }
            if (collection.Get("cityDropDown") == null || collection.Get("cityDropDown") == "")
            {
                ModelState.AddModelError("CityNull", "The City field is required.");
                invalid++;
            }

            if (invalid == 0)
            {
                try
                {
                    string coord = collection.Get("TempLatLong");
                    char[] delimChars = { ',' };
                    string[] seperated = coord.Split(delimChars);

                    int length1 = seperated[0].Length;
                    int length2 = seperated[1].Length;
                    Branch.Latitude = float.Parse(seperated[0].Substring(1, length1 - 1));
                    Branch.Longitude = float.Parse(seperated[1].Substring(0, length2 - 2));
                }
                catch
                {
                    Branch.Latitude = 0;
                    Branch.Longitude = 0;
                }

                var country = countryservice.GetCityById(Branch.City.CityId).Result;
                Branch.Country = new CountryModel.Country
                {
                    CountryId = country.CountryId,
                    CountryName = country.CountryName
                };

                ViewBag.msg = "a1";
            }

            return View(Branch);
        }

        public JsonResult BranchDelete(int id)
        {
            var res = false;

            var result = branchservice.DeleteBranch(id).Result;

            if (result == true)
            {
                res = true;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

	}
}