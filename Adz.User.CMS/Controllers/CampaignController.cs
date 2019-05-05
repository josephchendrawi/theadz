using Adz.BLL.Lib;
using Adz.User.CMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Adz.User.CMS.Controllers
{
    public class CampaignController : BaseAdminController
    {
        public ICampaignService campaignservice = new CampaignService();
        public IRuleService ruleservice = new RuleService();
        public ITagService tagservice = new TagService();
        public IImageService imageservice = new ImageService();

        [AdminAuthorize("CAMPAIGN", "VIEW")]
        public ActionResult Campaign(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }
        [AdminAuthorize("CAMPAIGN", "VIEW")]
        public ActionResult CampaignArchive()
        {
            return View();
        }
        public ActionResult Campaign_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.Campaign> listR = campaignservice.GetCampaignList().Result;
            List<CampaignModel.Campaign> newlistR = new List<CampaignModel.Campaign>();
            foreach (var v in listR)
            {
                newlistR.Add(new CampaignModel.Campaign() { CampaignId = v.CampaignId, Name = v.Name, Merchant = new MerchantModel.Merchant() { MerchantId = v.Merchant.MerchantId, Name = v.Merchant.Name }, Description = v.Description, LastAction = v.LastAction, Start = v.Start.AddHours(Helper.defaultGMT), End = v.End.AddHours(Helper.defaultGMT), Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT), SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), LinkURL = v.LinkURL });
            }
            IEnumerable<CampaignModel.Campaign> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AdminAuthorize("CAMPAIGN", "DELETE")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Campaign_Destroy([DataSourceRequest] DataSourceRequest request, CampaignModel.Campaign Campaign)
        {
            try
            {
                if (Campaign != null)
                {
                    Boolean result = campaignservice.DeleteCampaign(Campaign.CampaignId).Result;
                }
            }
            catch (Exception ex)
            {
                CustomException(ex);
                ModelState.AddModelError("", ex.Message);
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [AdminAuthorize("CAMPAIGN", "ADD")]
        public String Campaign_Duplicate(int CampaignId)
        {
            string res = "";
            if (CampaignId != null && CampaignId > 0)
            {
                try
                {
                    Boolean result = campaignservice.DuplicateCampaign(CampaignId).Result;

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
        [AdminAuthorize("CAMPAIGN", "VIEW")]
        public ActionResult CampaignView(int CampaignId, string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            Adz.BLL.Lib.Campaign v = campaignservice.GetCampaignById(CampaignId).Result;

            List<CampaignModel.Rule> rules = new List<CampaignModel.Rule>();
            foreach (var c in v.Rules)
            {
                Image Image = new Image() { ImageId = 0, Name = "", Url = "" };
                if (c.ImageId != 0)
                    Image = imageservice.GetImageByImageId(c.ImageId).Result;

                rules.Add(new CampaignModel.Rule()
                {
                    RuleId = c.RuleId,
                    NumberOfView = (int)c.NumberOfView,
                    StartTime = (DateTime)c.StartTime.AddHours(Helper.defaultGMT),
                    EndTime = (DateTime)c.EndTime.AddHours(Helper.defaultGMT),
                    NoEnd = (bool)c.NoEnd,
                    Gender = (int)c.Gender,
                    LastAction = c.LastAction,
                    Latitude = (double)c.Latitude,
                    Longitude = (double)c.Longitude,
                    Monday = (bool)c.Monday,
                    Tuesday = (bool)c.Tuesday,
                    Wednesday = (bool)c.Wednesday,
                    Thursday = (bool)c.Thursday,
                    Friday = (bool)c.Friday,
                    Saturday = (bool)c.Saturday,
                    Sunday = (bool)c.Sunday,
                    AgeGroupStart = (int)c.AgeGroupStart,
                    AgeGroupEnd = (int)c.AgeGroupEnd,
                    Image = new ImageModel.Image(){
                        ImageId = (int)Image.ImageId,
                        Name = Image.Name,
                        Url = Image.Url
                    }
                });
            }

            List<Adz.BLL.Lib.Tag> primtaglist = tagservice.GetSelectedTagList(CampaignId, true).Result;
            List<CampaignModel.Tag> primtags = new List<CampaignModel.Tag>();
            foreach (var t in primtaglist)
            {
                if (t.Selected == true)
                    primtags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
            }

            List<Adz.BLL.Lib.Tag> sectaglist = tagservice.GetSelectedTagList(CampaignId, false).Result;
            List<CampaignModel.Tag> sectags = new List<CampaignModel.Tag>();
            foreach (var t in sectaglist)
            {
                if(t.Selected == true)
                    sectags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
            }

            Adz.BLL.Lib.CampaignHistoryGroup CampaignHistoryGroup = campaignservice.GetCampaignHistoryGroupById(CampaignId).Result;
            int Views = CampaignHistoryGroup.Views;
            int Clicks = CampaignHistoryGroup.Clicks;
            DateTime LastView = CampaignHistoryGroup.LastViewTime;
            DateTime LastClick = CampaignHistoryGroup.LastClickTime;

            CampaignModel.Campaign model = new CampaignModel.Campaign() { CampaignId = v.CampaignId, Name = v.Name, Merchant = new MerchantModel.Merchant() { MerchantId = v.Merchant.MerchantId, Name = v.Merchant.Name }, Description = v.Description, LastAction = v.LastAction, Start = v.Start.AddHours(Helper.defaultGMT), End = v.End.AddHours(Helper.defaultGMT), Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT), SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), LinkURL = v.LinkURL, Rules = rules, PrimaryTags = primtags, SecondaryTags = sectags, Views = Views, LastView = LastView, Clicks = Clicks, LastClick = LastClick };

            return View(model);
        }

        [AdminAuthorize("CAMPAIGN", "EDIT")]
        public ActionResult CampaignEdit(int CampaignId)
        {
            Adz.BLL.Lib.Campaign v = campaignservice.GetCampaignById(CampaignId).Result;

            List<CampaignModel.Rule> rules = new List<CampaignModel.Rule>();
            foreach (var c in v.Rules)
            {
                Image Image = new Image() { ImageId = 0, Name = "", Url = "" };
                if (c.ImageId != 0)
                    Image = imageservice.GetImageByImageId(c.ImageId).Result;

                rules.Add(new CampaignModel.Rule()
                {
                    RuleId = c.RuleId,
                    NumberOfView = (int)c.NumberOfView,
                    StartTime = (DateTime)c.StartTime.AddHours(Helper.defaultGMT),
                    EndTime = (DateTime)c.EndTime.AddHours(Helper.defaultGMT),
                    NoEnd = (bool)c.NoEnd,
                    Gender = (int)c.Gender,
                    LastAction = c.LastAction,
                    Latitude = (double)c.Latitude,
                    Longitude = (double)c.Longitude,
                    Monday = (bool)c.Monday,
                    Tuesday = (bool)c.Tuesday,
                    Wednesday = (bool)c.Wednesday,
                    Thursday = (bool)c.Thursday,
                    Friday = (bool)c.Friday,
                    Saturday = (bool)c.Saturday,
                    Sunday = (bool)c.Sunday,
                    AgeGroupStart = (int)c.AgeGroupStart,
                    AgeGroupEnd = (int)c.AgeGroupEnd,
                    Image = new ImageModel.Image()
                    {
                        ImageId = (int)Image.ImageId,
                        Name = Image.Name,
                        Url = Image.Url
                    }
                });
            }
            
            List<Adz.BLL.Lib.Tag> primtaglist = tagservice.GetSelectedTagList(CampaignId, true).Result;
            List<CampaignModel.Tag> primtags = new List<CampaignModel.Tag>();
            foreach (var t in primtaglist)
            {
                primtags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
            }

            List<Adz.BLL.Lib.Tag> sectaglist = tagservice.GetSelectedTagList(CampaignId, false).Result;
            List<CampaignModel.Tag> sectags = new List<CampaignModel.Tag>();
            foreach (var t in sectaglist)
            {
                sectags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
            }

            CampaignModel.Campaign model = new CampaignModel.Campaign() { CampaignId = v.CampaignId, Name = v.Name, Merchant = new MerchantModel.Merchant() { MerchantId = v.Merchant.MerchantId, Name = v.Merchant.Name }, Description = v.Description, LastAction = v.LastAction, Start = v.Start.AddHours(Helper.defaultGMT), End = v.End.AddHours(Helper.defaultGMT), Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT), SubImageId = String.Join(",", v.SubImageId), SubImageName = String.Join("|", v.SubImageName), SubImageUrl = String.Join("|", v.SubImageUrl), SubImageUrlLink = String.Join("|", v.SubImageUrlLink), LinkURL = v.LinkURL, Rules = rules, PrimaryTags = primtags, SecondaryTags = sectags };

            return View(model);
        }
        [AdminAuthorize("CAMPAIGN", "EDIT")]
        [HttpPost]
        public ActionResult CampaignEdit(FormCollection collection, CampaignModel.Campaign Campaign)
        {
            if (collection.Get("merchantDropDown") == null || collection.Get("merchantDropDown") == "")
            {
                Campaign.Merchant = new MerchantModel.Merchant();
                ModelState.AddModelError("MerchantNull", "The Merchant field is required.");
            }
            else
            {
                Campaign.Merchant = new MerchantModel.Merchant() { MerchantId = Convert.ToInt16(collection.Get("merchantDropDown").ToString()) };
            }
            if (Campaign.Rules == null)
            {
                Campaign.Rules = new List<CampaignModel.Rule>();
            }

            if (Campaign.SubImageId == null || Campaign.SubImageId == "" || Regex.IsMatch(Campaign.SubImageId, @"^,+$") == true)
            {
                ModelState.AddModelError("ImageNull", "Image is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Campaign mer = new Adz.BLL.Lib.Campaign();
                    mer.CampaignId = Campaign.CampaignId;
                    mer.Name = Campaign.Name;
                    mer.Start = Campaign.Start;
                    mer.End = Campaign.End;
                    mer.Description = Campaign.Description;
                    mer.LinkURL = Campaign.LinkURL;
                    Adz.BLL.Lib.Merchant mrcnt = new Adz.BLL.Lib.Merchant();
                    mrcnt.MerchantId = Convert.ToInt16(collection.Get("merchantDropDown").ToString());
                    mer.Merchant = mrcnt;

                    if (Campaign.SubImageId != null && Campaign.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = Campaign.SubImageId.Split(',');
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
                            ruleservice.DeleteRule(int.Parse(v));
                        }
                    }
                    if (iEdit != null && iEdit != "")
                    {
                        foreach (var v in iEdit.Split('|'))
                        {
                            var rule = v.Split('$');
                            Adz.BLL.Lib.Rule rl = new Adz.BLL.Lib.Rule();
                            rl.RuleId = int.Parse(rule[0]);
                            rl.Gender = int.Parse(rule[1]);
                            rl.NumberOfView = int.Parse(rule[2]);
                            rl.StartTime = DateTime.Parse(rule[3]);
                            rl.EndTime = DateTime.Parse(rule[4]);
                            rl.NoEnd = Boolean.Parse(rule[5]);
                            rl.Latitude = double.Parse(rule[6]);
                            rl.Longitude = double.Parse(rule[7]);
                            rl.Monday = bool.Parse(rule[8]);
                            rl.Tuesday = bool.Parse(rule[9]);
                            rl.Wednesday = bool.Parse(rule[10]);
                            rl.Thursday = bool.Parse(rule[11]);
                            rl.Friday = bool.Parse(rule[12]);
                            rl.Saturday = bool.Parse(rule[13]);
                            rl.Sunday = bool.Parse(rule[14]);
                            rl.AgeGroupStart = int.Parse(rule[15]);
                            rl.AgeGroupEnd = int.Parse(rule[16]);
                            rl.ImageId = int.Parse(rule[17]);

                            ruleservice.CreateEditRule(rl);
                        }
                    }
                    if (iAdd != null && iAdd != "")
                    {
                        foreach (var v in iAdd.Split('|'))
                        {
                            var rule = v.Split('$');
                            Adz.BLL.Lib.Rule rl = new Adz.BLL.Lib.Rule();
                            rl.RuleId = 0;
                            rl.Gender = int.Parse(rule[0]);
                            rl.NumberOfView = int.Parse(rule[1]);
                            rl.StartTime = DateTime.Parse(rule[2]);
                            rl.EndTime = DateTime.Parse(rule[3]);
                            rl.NoEnd = Boolean.Parse(rule[4]);
                            rl.Latitude = double.Parse(rule[5]);
                            rl.Longitude = double.Parse(rule[6]);
                            rl.Campaign = new Adz.BLL.Lib.Campaign();
                            rl.Campaign.CampaignId = Campaign.CampaignId;
                            rl.Monday = bool.Parse(rule[7]);
                            rl.Tuesday = bool.Parse(rule[8]);
                            rl.Wednesday = bool.Parse(rule[9]);
                            rl.Thursday = bool.Parse(rule[10]);
                            rl.Friday = bool.Parse(rule[11]);
                            rl.Saturday = bool.Parse(rule[12]);
                            rl.Sunday = bool.Parse(rule[13]);
                            rl.AgeGroupStart = int.Parse(rule[14]);
                            rl.AgeGroupEnd = int.Parse(rule[15]);
                            rl.ImageId = int.Parse(rule[16]);

                            ruleservice.CreateEditRule(rl);
                        }
                    }

                    string primtagAdd = collection.Get("primtagAdd");
                    string primtagDelete = collection.Get("primtagDelete");

                    if (primtagAdd != null && primtagAdd != "")
                    {
                        foreach (var v in primtagAdd.Split('|'))
                        {
                            tagservice.AddCampaignTag(Campaign.CampaignId, int.Parse(v), true);
                        }
                    }
                    if (primtagDelete != null && primtagDelete != "")
                    {
                        foreach (var v in primtagDelete.Split('|'))
                        {
                            tagservice.DeleteCampaignTag(Campaign.CampaignId, int.Parse(v), true);
                        }
                    }

                    string sectagAdd = collection.Get("sectagAdd");
                    string sectagDelete = collection.Get("sectagDelete");

                    if (sectagAdd != null && sectagAdd != "")
                    {
                        foreach (var v in sectagAdd.Split('|'))
                        {
                            tagservice.AddCampaignTag(Campaign.CampaignId, int.Parse(v), false);
                        }
                    }
                    if (sectagDelete != null && sectagDelete != "")
                    {
                        foreach (var v in sectagDelete.Split('|'))
                        {
                            tagservice.DeleteCampaignTag(Campaign.CampaignId, int.Parse(v), false);
                        }
                    }
                    
                    int result = campaignservice.CreateEditCampaign(mer).Result;
                    string r;
                    if (result != 0)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("CampaignView", "Campaign", new { CampaignId = Campaign.CampaignId, r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            else
            {
                List<Adz.BLL.Lib.Tag> primtaglist = tagservice.GetSelectedTagList(Campaign.CampaignId, true).Result;
                List<CampaignModel.Tag> primtags = new List<CampaignModel.Tag>();
                foreach (var t in primtaglist)
                {
                    primtags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
                }

                List<Adz.BLL.Lib.Tag> sectaglist = tagservice.GetSelectedTagList(Campaign.CampaignId, false).Result;
                List<CampaignModel.Tag> sectags = new List<CampaignModel.Tag>();
                foreach (var t in sectaglist)
                {
                    sectags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
                }

                Campaign.PrimaryTags = primtags;
                Campaign.SecondaryTags = sectags;
            }
            return View(Campaign);
        }

        [AdminAuthorize("CAMPAIGN", "ADD")]
        public ActionResult CampaignAdd()
        {
            CampaignModel.Campaign model = new CampaignModel.Campaign()
            {
                Start = DateTime.Now,
                End = DateTime.Now
            };
            model.Rules = new List<CampaignModel.Rule>();
            model.Merchant = new MerchantModel.Merchant();

            List<Adz.BLL.Lib.Tag> primtaglist = tagservice.GetSelectedTagList(0, true).Result;
            List<CampaignModel.Tag> primtags = new List<CampaignModel.Tag>();
            foreach (var t in primtaglist)
            {
                primtags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
            }

            List<Adz.BLL.Lib.Tag> sectaglist = tagservice.GetSelectedTagList(0, false).Result;
            List<CampaignModel.Tag> sectags = new List<CampaignModel.Tag>();
            foreach (var t in sectaglist)
            {
                sectags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
            }

            model.PrimaryTags = primtags;
            model.SecondaryTags = sectags;

            return View(model);
        }
        [AdminAuthorize("CAMPAIGN", "ADD")]
        [HttpPost]
        public ActionResult CampaignAdd(FormCollection collection, CampaignModel.Campaign Campaign)
        {
            Campaign.Merchant = new MerchantModel.Merchant();
            if (collection.Get("merchantDropDown") == null || collection.Get("merchantDropDown") == "")
            {
                ModelState.AddModelError("MerchantNull", "The Merchant field is required.");
            }
            else
            {
                Campaign.Merchant.MerchantId = Convert.ToInt16(collection.Get("merchantDropDown").ToString());
            }
            Campaign.Rules = new List<CampaignModel.Rule>();

            if (Campaign.SubImageId == null || Campaign.SubImageId == "" || Regex.IsMatch(Campaign.SubImageId, @"^,+$") == true)
            {
                ModelState.AddModelError("ImageNull", "Image is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Campaign mer = new Adz.BLL.Lib.Campaign();
                    mer.CampaignId = 0;
                    mer.Name = Campaign.Name;
                    mer.Start = Campaign.Start;
                    mer.End = Campaign.End;
                    mer.Description = Campaign.Description;
                    mer.LinkURL = Campaign.LinkURL;
                    Adz.BLL.Lib.Merchant mrcnt = new Adz.BLL.Lib.Merchant();
                    mrcnt.MerchantId = Convert.ToInt16(collection.Get("merchantDropDown").ToString());
                    mer.Merchant = mrcnt;

                    if (Campaign.SubImageId != null && Campaign.SubImageId != "")
                    {
                        List<int> subimg = new List<int>();
                        List<string> subimglink = new List<string>();
                        String[] subimagelist = Campaign.SubImageId.Split(',');
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

                    int result = campaignservice.CreateEditCampaign(mer).Result;

                    string iAdd = collection.Get("itemAdd");
                    if (iAdd != null && iAdd != "")
                    {
                        foreach (var v in iAdd.Split('|'))
                        {
                            var rule = v.Split('$');
                            Adz.BLL.Lib.Rule rl = new Adz.BLL.Lib.Rule();
                            rl.RuleId = 0;
                            rl.Gender = int.Parse(rule[0]);
                            rl.NumberOfView = int.Parse(rule[1]);
                            rl.StartTime = DateTime.Parse(rule[2]);
                            rl.EndTime = DateTime.Parse(rule[3]);
                            rl.NoEnd = Boolean.Parse(rule[4]);
                            rl.Latitude = double.Parse(rule[5]);
                            rl.Longitude = double.Parse(rule[6]);
                            rl.Campaign = new Adz.BLL.Lib.Campaign();
                            rl.Monday = bool.Parse(rule[7]);
                            rl.Tuesday = bool.Parse(rule[8]);
                            rl.Wednesday = bool.Parse(rule[9]);
                            rl.Thursday  = bool.Parse(rule[10]);
                            rl.Friday = bool.Parse(rule[11]);
                            rl.Saturday = bool.Parse(rule[12]);
                            rl.Sunday = bool.Parse(rule[13]);
                            rl.AgeGroupStart = int.Parse(rule[14]);
                            rl.AgeGroupEnd = int.Parse(rule[15]);
                            rl.ImageId = int.Parse(rule[16]);

                            rl.Campaign.CampaignId = result;

                            ruleservice.CreateEditRule(rl);
                        }
                    }
                    
                    string primtagAdd = collection.Get("primtagAdd");

                    if (primtagAdd != null && primtagAdd != "")
                    {
                        foreach (var v in primtagAdd.Split('|'))
                        {
                            tagservice.AddCampaignTag(result, int.Parse(v), true);
                        }
                    }

                    string sectagAdd = collection.Get("sectagAdd");

                    if (sectagAdd != null && sectagAdd != "")
                    {
                        foreach (var v in sectagAdd.Split('|'))
                        {
                            tagservice.AddCampaignTag(result, int.Parse(v), false);
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
                    return RedirectToAction("Campaign", "Campaign", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            else
            {
                List<Adz.BLL.Lib.Tag> primtaglist = tagservice.GetSelectedTagList(Campaign.CampaignId, true).Result;
                List<CampaignModel.Tag> primtags = new List<CampaignModel.Tag>();
                foreach (var t in primtaglist)
                {
                    primtags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
                }

                List<Adz.BLL.Lib.Tag> sectaglist = tagservice.GetSelectedTagList(Campaign.CampaignId, false).Result;
                List<CampaignModel.Tag> sectags = new List<CampaignModel.Tag>();
                foreach (var t in sectaglist)
                {
                    sectags.Add(new CampaignModel.Tag() { TagId = t.TagId, Name = t.Name, Create = t.Create.AddHours(Helper.defaultGMT), Update = t.Update.AddHours(Helper.defaultGMT), LastAction = t.LastAction, Selected = t.Selected });
                }

                Campaign.PrimaryTags = primtags;
                Campaign.SecondaryTags = sectags;
            }
            return View(Campaign);
        }

        public ActionResult RuleEdit(string rid, int gender = 0, int nov = 0, string start = "", string end = "", string noend = "", double latitude = 0, double longitude = 0, bool monday = false, bool tuesday = false, bool wednesday = false, bool thursday = false, bool friday = false, bool saturday = false, bool sunday = false, int agegroupstart = -1, int agegroupend = -1, int imageid = 0, string subimageids = "", string subimageurls = "")
        {
            int selectedImage = 0;
            List<ImageModel.Image> subimgs = new List<ImageModel.Image>();
            String[] subimagelist1 = subimageids.Split(',');
            String[] subimagelist2 = subimageurls.Split('|');
            for (int i = 0; i < subimagelist1.Length; i++ )
            {
                if (subimagelist1[i] != "")
                {
                    subimgs.Add(new ImageModel.Image()
                    {
                        ImageId = int.Parse(subimagelist1[i]),
                        Url = subimagelist2[i]
                    });

                    if(subimagelist1[i] == imageid.ToString())
                    {
                        selectedImage = i;
                    }
                }
            }
            
            CampaignModel.Rule model = new CampaignModel.Rule()
            {
                RuleId = rid.IndexOf("temp") == -1 ? int.Parse(rid) : (int.Parse(rid.Substring(4)) * -1), //if temp then Ruleid = negative 
                Gender = gender,
                NumberOfView = nov,
                StartTime = DateTime.Parse(start),
                EndTime = DateTime.Parse(end),
                NoEnd = Boolean.Parse(noend),
                Latitude = latitude,
                Longitude = longitude,
                Monday = monday,
                Tuesday = tuesday,
                Wednesday = wednesday,
                Thursday = thursday,
                Friday = friday,
                Saturday = saturday,
                Sunday = sunday,
                AgeGroupStart = agegroupstart,
                AgeGroupEnd = agegroupend,
                ImageId = imageid,
                ImageUrl = subimagelist2[selectedImage],
                ExistingImages = subimgs,
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult RuleEdit(FormCollection collection, CampaignModel.Rule Rule)
        {
            try
            {
                string coord = collection.Get("TempLatLong");
                char[] delimChars = { ',' };
                string[] seperated = coord.Split(delimChars);

                int length1 = seperated[0].Length;
                int length2 = seperated[1].Length;
                Rule.Latitude = float.Parse(seperated[0].Substring(1, length1 - 1));
                Rule.Longitude = float.Parse(seperated[1].Substring(0, length2 - 2));
            }
            catch
            {
                Rule.Latitude = 0;
                Rule.Longitude = 0;
            }

            if (ModelState.IsValid && collection.Get("ruleimage") != null && collection.Get("ruleimage") != "")
            {
                string chosenimage = collection.Get("ruleimage");
                Rule.ImageId = int.Parse(chosenimage.Split('|')[0]);
                Rule.ImageUrl = chosenimage.Split('|')[1];

                ViewBag.msg = "e1";
            }

            return View(Rule);
        }

        public ActionResult RuleAdd(int campaignId, string subimageids = "", string subimageurls = "")
        {
            List<ImageModel.Image> subimgs = new List<ImageModel.Image>();
            String[] subimagelist1 = subimageids.Split(',');
            String[] subimagelist2 = subimageurls.Split('|');
            for (int i = 0; i < subimagelist1.Length; i++)
            {
                if (subimagelist1[i] != "")
                {
                    subimgs.Add(new ImageModel.Image()
                    {
                        ImageId = int.Parse(subimagelist1[i]),
                        Url = subimagelist2[i]
                    });
                }
            }

            CampaignModel.Rule model = new CampaignModel.Rule()
            {
                Campaign = new CampaignModel.Campaign() { CampaignId = campaignId },
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                ExistingImages = subimgs
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult RuleAdd(FormCollection collection, CampaignModel.Rule Rule)
        {
            try
            {
                string coord = collection.Get("TempLatLong");
                char[] delimChars = { ',' };
                string[] seperated = coord.Split(delimChars);

                int length1 = seperated[0].Length;
                int length2 = seperated[1].Length;
                Rule.Latitude = float.Parse(seperated[0].Substring(1, length1 - 1));
                Rule.Longitude = float.Parse(seperated[1].Substring(0, length2 - 2));
            }
            catch
            {
                Rule.Latitude = 0;
                Rule.Longitude = 0;
            }

            if (ModelState.IsValid && collection.Get("ruleimage") != null && collection.Get("ruleimage") != "")
            {
                string chosenimage = collection.Get("ruleimage");
                Rule.ImageId = int.Parse(chosenimage.Split('|')[0]);
                Rule.ImageUrl = chosenimage.Split('|')[1];

                ViewBag.msg = "a1";
            }

            return View(Rule);
        }

        [AdminAuthorize("CAMPAIGN", "VIEW")]
        public ActionResult Tag(string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            return View();
        }

        [AdminAuthorize("CAMPAIGN", "VIEW")]
        public ActionResult Tag_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Adz.BLL.Lib.Tag> listR = tagservice.GetTagList().Result;
            List<CampaignModel.Tag> newlistR = new List<CampaignModel.Tag>();
            foreach (var v in listR)
            {
                newlistR.Add(new CampaignModel.Tag() { TagId = v.TagId, Name = v.Name, Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT), LastAction = v.LastAction });
            }
            IEnumerable<CampaignModel.Tag> ienuList = newlistR;
            var result = ienuList.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize("CAMPAIGN", "VIEW")]
        public ActionResult TagView(int TagId, string r)
        {
            if (r != null || r != "")
            {
                ViewBag.msg = r;
            }

            Adz.BLL.Lib.Tag v = tagservice.GetTagById(TagId).Result;

            CampaignModel.Tag model = new CampaignModel.Tag() { TagId = v.TagId, Name = v.Name, LastAction = v.LastAction, Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT) };

            return View(model);
        }

        [AdminAuthorize("CAMPAIGN", "ADD")]
        public ActionResult TagAdd()
        {
            CampaignModel.Tag model = new CampaignModel.Tag();

            return View(model);
        }
        [AdminAuthorize("CAMPAIGN", "ADD")]
        [HttpPost]
        public ActionResult TagAdd(FormCollection collection, CampaignModel.Tag Tag)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Tag mer = new Adz.BLL.Lib.Tag();
                    mer.TagId = 0;
                    mer.Name = Tag.Name;

                    int result = tagservice.CreateEditTag(mer).Result;
                    
                    string r;
                    if (result != 0)
                    {
                        r = "a1";
                    }
                    else
                    {
                        r = "a2";
                    }
                    return RedirectToAction("Tag", "Campaign", new { r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Tag);
        }

        [AdminAuthorize("CAMPAIGN", "EDIT")]
        public ActionResult TagEdit(int TagId)
        {
            Adz.BLL.Lib.Tag v = tagservice.GetTagById(TagId).Result;

            CampaignModel.Tag model = new CampaignModel.Tag() { TagId = v.TagId, Name = v.Name, LastAction = v.LastAction, Create = v.Create.AddHours(Helper.defaultGMT), Update = v.Update.AddHours(Helper.defaultGMT) };

            return View(model);
        }
        [AdminAuthorize("CAMPAIGN", "EDIT")]
        [HttpPost]
        public ActionResult TagEdit(FormCollection collection, CampaignModel.Tag Tag)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Adz.BLL.Lib.Tag mer = new Adz.BLL.Lib.Tag();
                    mer.TagId = Tag.TagId;
                    mer.Name = Tag.Name;

                    int result = tagservice.CreateEditTag(mer).Result;
                    string r;
                    if (result != 0)
                    {
                        r = "e1";
                    }
                    else
                    {
                        r = "e2";
                    }
                    return RedirectToAction("TagView", "Campaign", new { TagId = Tag.TagId, r = r });
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    ViewBag.msg = ex.Message;
                }
            }
            return View(Tag);
        }
        [AdminAuthorize("CAMPAIGN", "VIEW")]
        public ActionResult TagArchive()
        {
            return View();
        }
        [AdminAuthorize("CAMPAIGN", "DELETE")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Tag_Destroy([DataSourceRequest] DataSourceRequest request, CampaignModel.Tag Tag)
        {
            try
            {
                if (Tag != null)
                {
                    Boolean result = tagservice.DeleteTag(Tag.TagId).Result;
                }
            }
            catch (Exception ex)
            {
                CustomException(ex);
                ModelState.AddModelError("", ex.Message);
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [AdminAuthorize("CAMPAIGN", "ADD")]
        public String Tag_Duplicate(int TagId)
        {
            string res = "";
            if (TagId != null && TagId > 0)
            {
                try
                {
                    Boolean result = tagservice.DuplicateTag(TagId).Result;

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

	}
}