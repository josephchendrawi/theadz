using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class CampaignService : ICampaignService
    {
        public Response<List<Campaign>> GetCampaignList()
        {
            Response<List<Campaign>> response = null;
            List<Campaign> CampaignList = new List<Campaign>();
            using (var context = new TheAdzEntities())
            {
                var entityCampaign = from d in context.Campaigns.Include("Merchant")
                                     orderby d.last_created descending
                                     select d;
                foreach (var v in entityCampaign)
                {
                    Campaign Campaign = new Campaign();
                    Campaign.CampaignId = v.id;
                    Campaign.Name = v.name;
                    Campaign.Start = (DateTime)v.start_date;
                    Campaign.End = (DateTime)v.end_date;
                    Merchant merchant = new Merchant()
                    {
                        MerchantId = (int)v.merchant_id,
                        Name = v.Merchant.name,
                    };
                    Campaign.Merchant = merchant;

                    Campaign.Description = v.description;
                    Campaign.Create = (DateTime)v.last_created;
                    Campaign.Update = (DateTime)v.last_updated;
                    Campaign.LastAction = v.last_action;
                    Campaign.LinkURL = v.LINK_URL;
                    
                    List<int> subimgidcampaign = new List<int>();
                    List<string> subimgnamecampaign = new List<string>();
                    List<string> subimgurlcampaign = new List<string>();
                    List<string> subimgurllinkcampaign = new List<string>();

                    List<int> subimgwidth = new List<int>();
                    List<int> subimgheight = new List<int>();
                    var entitySubImgCampaign = from d in context.CampaignImages
                                               from e in context.Images
                                               where d.image_id == e.id && d.campaign_id == v.id
                                               select new { d.image_id, e.url, d.image_url, e.width, e.height };
                    foreach (var v2 in entitySubImgCampaign)
                    {
                        subimgidcampaign.Add(v2.image_id);
                        subimgnamecampaign.Add(v2.url);
                        subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                        subimgurllinkcampaign.Add(v2.image_url);

                        subimgwidth.Add((int)v2.width);
                        subimgheight.Add((int)v2.height);
                    }
                    Campaign.SubImageId = subimgidcampaign;
                    Campaign.SubImageName = subimgnamecampaign;
                    Campaign.SubImageUrl = subimgurlcampaign;
                    Campaign.SubImageUrlLink = subimgurllinkcampaign;

                    Campaign.SubImageWidth = subimgwidth;
                    Campaign.SubImageHeight = subimgheight;

                    CampaignList.Add(Campaign);
                }
                response = Response<List<Campaign>>.Create(CampaignList);
            }

            return response;
        }

        public Response<Campaign> GetCampaignById(int CampaignId)
        {
            Response<Campaign> response = null;
            Campaign Campaign = new Campaign();
            using (var context = new TheAdzEntities())
            {
                var entityCampaign = from d in context.Campaigns.Include("Merchant")
                                     where d.id == CampaignId
                                     select d;
                var v = entityCampaign.First();
                if (v != null)
                {
                    Campaign.CampaignId = v.id;
                    Campaign.Name = v.name;
                    Campaign.Start = (DateTime)v.start_date;
                    Campaign.End = (DateTime)v.end_date;
                    Merchant merchant = new Merchant()
                    {
                        MerchantId = (int)v.merchant_id,
                        Name = v.Merchant.name,
                    };
                    Campaign.Merchant = merchant;

                    Campaign.Description = v.description;
                    Campaign.Create = (DateTime)v.last_created;
                    Campaign.Update = (DateTime)v.last_updated;
                    Campaign.LastAction = v.last_action;
                    Campaign.LinkURL = v.LINK_URL;

                    List<int> subimgidcampaign = new List<int>();
                    List<string> subimgnamecampaign = new List<string>();
                    List<string> subimgurlcampaign = new List<string>();
                    List<string> subimgurllinkcampaign = new List<string>();
                    List<int> subimgwidth = new List<int>();
                    List<int> subimgheight = new List<int>();
                    List<string> subimgchecksum = new List<string>();
                    var entitySubImgCampaign = from d in context.CampaignImages
                                               from e in context.Images
                                               where d.image_id == e.id && d.campaign_id == v.id
                                               select new { d.image_id, e.url, d.image_url, e.height, e.width, e.checksum };
                    foreach (var v2 in entitySubImgCampaign)
                    {
                        subimgidcampaign.Add(v2.image_id);
                        subimgnamecampaign.Add(v2.url);
                        subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                        subimgurllinkcampaign.Add(v2.image_url);
                        subimgwidth.Add((int)v2.width);
                        subimgheight.Add((int)v2.height);
                        subimgchecksum.Add(v2.checksum);
                    }
                    Campaign.SubImageId = subimgidcampaign;
                    Campaign.SubImageName = subimgnamecampaign;
                    Campaign.SubImageUrl = subimgurlcampaign;
                    Campaign.SubImageUrlLink = subimgurllinkcampaign;
                    Campaign.SubImageWidth = subimgwidth;
                    Campaign.SubImageHeight = subimgheight;
                    Campaign.SubImageCheckSum = subimgchecksum;

                    List<Rule> Rules = new List<Rule>();
                    foreach (var c in v.CampaignRules)
                    {
                        if (c.last_action != "5")
                        {
                            Rules.Add(new Rule()
                            {
                                RuleId = c.id,
                                NumberOfView = (int)c.numberofviews,
                                StartTime = (DateTime)c.start_time,
                                EndTime = (DateTime)c.end_time,
                                NoEnd = (bool)c.no_end_time,
                                Gender = (int)c.gender,
                                LastAction = c.last_action,
                                Latitude = (double)c.latitude,
                                Longitude = (double)c.longitude,
                                Monday = c.monday == null ? false : (bool)c.monday,
                                Tuesday = c.tuesday == null ? false : (bool)c.tuesday,
                                Wednesday = c.wednesday == null ? false : (bool)c.wednesday,
                                Thursday = c.thursday == null ? false : (bool)c.thursday,
                                Friday = c.friday == null ? false : (bool)c.friday,
                                Saturday = c.saturday == null ? false : (bool)c.saturday,
                                Sunday = c.sunday == null ? false : (bool)c.sunday,
                                AgeGroupStart = c.agegroup_start == null ? -1 : (int)c.agegroup_start,
                                AgeGroupEnd = c.agegroup_end == null ? -1 : (int)c.agegroup_end,
                                ImageId = c.image_id == null ? 0 : (int)c.image_id,
                            });
                        }
                    }
                    Campaign.Rules = Rules;
                }
                response = Response<Campaign>.Create(Campaign);
            }

            return response;
        }

        public Response<int> CreateEditCampaign(Campaign Campaign)
        {
            Response<int> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Campaign.CampaignId != 0)
                {
                    var entityCampaign = from d in context.Campaigns
                                         where d.id == Campaign.CampaignId
                                         select d;
                    if (entityCampaign.Count() > 0)
                    {
                        var entityCampaign2 = from d in context.Campaigns
                                              where d.name.ToLower() == Campaign.Name.ToLower()
                                              && d.id != Campaign.CampaignId
                                              select d;
                        if (entityCampaign2.Count() > 0)
                        {
                            throw new CustomException(CustomErrorType.CampaignAlreadyAssign);
                        }
                        else
                        {
                            entityCampaign.First().name = Campaign.Name;
                            entityCampaign.First().start_date = Campaign.Start.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                            entityCampaign.First().end_date = Campaign.End.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                            entityCampaign.First().description = Campaign.Description;
                            entityCampaign.First().last_updated = DateTime.UtcNow;
                            entityCampaign.First().last_action = "3";
                            entityCampaign.First().LINK_URL = Campaign.LinkURL;
                            entityCampaign.First().merchant_id = Campaign.Merchant.MerchantId;
                            context.SaveChanges();
                                                        
                            #region image
                            if (Campaign.SubImageId != null)
                            {
                                try
                                {
                                    var entityImage = from d in context.CampaignImages
                                                      where d.campaign_id == Campaign.CampaignId
                                                      select d;
                                    foreach (var v in entityImage)
                                    {
                                        context.CampaignImages.Remove(v);
                                    }
                                    context.SaveChanges();
                                    int i = 0;
                                    foreach (var v in Campaign.SubImageId)
                                    {
                                        CampaignImage mmimage = new CampaignImage();
                                        mmimage.campaign_id = Campaign.CampaignId;
                                        mmimage.image_id = v;
                                        mmimage.image_url = Campaign.SubImageUrlLink[i];
                                        context.CampaignImages.Add(mmimage);
                                        context.SaveChanges();
                                        i++;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                var entityImage = from d in context.CampaignImages
                                                  where d.campaign_id == Campaign.CampaignId
                                                  select d;
                                foreach (var v in entityImage)
                                {
                                    context.CampaignImages.Remove(v);
                                }
                                context.SaveChanges();
                            }
                            #endregion

                            response = Response<int>.Create(Campaign.CampaignId);
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.CampaignNotFound);
                    }
                }
                else
                {
                    var entityCampaign = from d in context.Campaigns
                                         where d.name.ToLower() == Campaign.Name.ToLower()
                                         select d;
                    if (entityCampaign.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.CampaignAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.Campaign mmentity = new Adz.DAL.EF.Campaign();
                        mmentity.name = Campaign.Name;
                        mmentity.start_date = Campaign.Start.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                        mmentity.end_date = Campaign.End.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                        mmentity.description = Campaign.Description;
                        mmentity.last_created = DateTime.UtcNow;
                        mmentity.last_updated = DateTime.UtcNow;
                        mmentity.last_action = "1";
                        mmentity.LINK_URL = Campaign.LinkURL;
                        mmentity.merchant_id = Campaign.Merchant.MerchantId;
                        context.Campaigns.Add(mmentity);
                        context.SaveChanges();
                        int CampaignId = mmentity.id;

                        #region image
                        if (Campaign.SubImageId != null)
                        {
                            try
                            {
                                int i = 0;
                                foreach (var v in Campaign.SubImageId)
                                {
                                    CampaignImage mmimage = new CampaignImage();
                                    mmimage.campaign_id = CampaignId;
                                    mmimage.image_id = v;
                                    mmimage.image_url = Campaign.SubImageUrlLink[i];
                                    context.CampaignImages.Add(mmimage);
                                    context.SaveChanges();
                                    i++;
                                }
                            }
                            catch
                            {

                            }
                        }
                        #endregion

                        response = Response<int>.Create(CampaignId);
                    }
                }
            }
            return response;
        }

        public Response<bool> DeleteCampaign(int CampaignId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityCampaign = from d in context.Campaigns
                                     where d.id == CampaignId
                                     select d;
                if (entityCampaign.Count() > 0)
                {
                    entityCampaign.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.CampaignFailedDelete);
                }
            }

            return response;
        }

        public Response<bool> DuplicateCampaign(int CampaignId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                Campaign item = GetCampaignById(CampaignId).Result;
                item.CampaignId = 0;
                item.Name = item.Name + " - Copy";
                var result = CreateEditCampaign(item).Result != 0 ? true : false;
                response = Response<bool>.Create(result);
            }

            return response;
        }

        public Response<Campaign> ViewThisCampaign(int CampaignId, string username, int gender, double lat, double lng, DateTime? time, string timezone, int action)
        {
            Response<Campaign> response = null;
            Campaign Campaign = new Campaign();
            using (var context = new TheAdzEntities())
            {
                var entityCampaign = from d in context.Campaigns.Include("Merchant")
                                     where d.id == CampaignId && d.last_action != "5"
                                     select d;
                var v = entityCampaign.First();
                if (v != null)
                {
                    var entityUser = from d in context.Users
                                     where d.email.ToLower() == username.ToLower()
                                     && d.locked_status == false
                                     && (d.userstatus_id == 1 || d.userstatus_id == 4)
                                     select d;
                    if (entityUser.Count() <= 0)
                    {
                        throw new CustomException(CustomErrorType.UserServiceNotFound);
                    }

                    int Age = -1;
                    if (entityUser.First().dateofbirth > DateTime.MinValue)
                    {
                        Age = DateTime.Today.Year - entityUser.First().dateofbirth.Value.Year;
                        if (entityUser.First().dateofbirth > DateTime.Today.AddYears(-Age)) Age--;
                    }

                    #region old-method
                    /*
                    var entityRule = from d in context.CampaignRules
                                     where d.start_time <= DateTime.UtcNow && d.gender == gender && d.last_action != "5"
                                     && (from ch in context.CampaignHistories where ch.campaign_id == d.campaign_id select d).Count() < d.numberofviews
                                     && d.campaign_id == CampaignId
                                     select d;
                    if (entityRule.Count() <= 0)
                    {
                        throw new CustomException(CustomErrorType.RuleNotFound);

                        ////
                    }
                    else
                    {
                        bool flag = false;
                        foreach (var v2 in entityRule)
                        {
                            //if (Adz.BLL.Lib.Distance.getdistance(lat, lng, (double)v2.latitude, (double)v2.longitude, 'K') < 500)
                            //{
                                if (v2.no_end_time == true)
                                {
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    if (DateTime.UtcNow <= v2.end_time)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                            //}
                        }
                        if (flag == false)
                        {
                            throw new CustomException(CustomErrorType.RuleNotFound);
                        }
                    }

                    Adz.DAL.EF.CampaignHistory mmentity = new Adz.DAL.EF.CampaignHistory();
                    mmentity.campaign_id = CampaignId;
                    mmentity.user_id = entityUser.First();
                    mmentity.view_date = DateTime.UtcNow;
                    mmentity.latitude = lat;
                    mmentity.longitude = lng;
                    mmentity.gender = gender;
                    context.CampaignHistories.Add(mmentity);
                    context.SaveChanges();
                    */
                    #endregion

                    bool flag = false;
                    List<int> RuleIdList = new List<int>();

                    var entityRule = from d in context.CampaignRules
                                        where d.last_action != "5"
                                        && d.campaign_id == CampaignId
                                        select d;
                    if (entityRule.Count() > 0)
                    {
                        entityRule = from d in context.CampaignRules
                                     where d.last_action != "5"
                                     && d.campaign_id == CampaignId
                                     && d.start_time <= DateTime.UtcNow && d.gender == gender && (from ch in context.CampaignHistories where ch.campaign_id == d.campaign_id select d).Count() < d.numberofviews
                                     //&& d.agegroup_start < Age && Age <= d.agegroup_end
                                     select d;
                        //temp
                        if (Age != -1)
                        {
                            entityRule = entityRule.Where(d => d.agegroup_start < Age && Age <= d.agegroup_end);
                        }

                        if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Monday)
                            entityRule = entityRule.Where(d => d.monday == true);
                        else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Tuesday)
                            entityRule = entityRule.Where(d => d.tuesday == true);
                        else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Wednesday)
                            entityRule = entityRule.Where(d => d.wednesday == true);
                        else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Thursday)
                            entityRule = entityRule.Where(d => d.thursday == true);
                        else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Friday)
                            entityRule = entityRule.Where(d => d.friday == true);
                        else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday)
                            entityRule = entityRule.Where(d => d.saturday == true);
                        else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                            entityRule = entityRule.Where(d => d.sunday == true);

                        if (entityRule.Count() > 0)
                        {
                            foreach (var v2 in entityRule)
                            {
                                //if (Adz.BLL.Lib.Distance.getdistance(lat, lng, (double)v2.latitude, (double)v2.longitude, 'K') < 500)
                                //{
                                if (v2.no_end_time == true)
                                {
                                    RuleIdList.Add((int)v2.id);
                                    flag = true;
                                }
                                else
                                {
                                    if (DateTime.UtcNow <= v2.end_time)
                                    {
                                        RuleIdList.Add((int)v2.id);
                                        flag = true;
                                    }
                                }
                                //}
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                    }

                    if (action != -1) //add to CampaignHistory
                    {
                        if (flag == true)
                        {
                            Adz.DAL.EF.CampaignHistory mmentity = new Adz.DAL.EF.CampaignHistory();
                            mmentity.campaign_id = CampaignId;
                            mmentity.user_id = entityUser.First().id;
                            mmentity.view_date = time == null ? DateTime.UtcNow : time;
                            mmentity.timezone = timezone;
                            mmentity.created_date = DateTime.UtcNow;
                            mmentity.latitude = lat;
                            mmentity.longitude = lng;
                            mmentity.gender = gender;
                            mmentity.action = action;
                            mmentity.flag_processed = false;
                            context.CampaignHistories.Add(mmentity);
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new CustomException(CustomErrorType.CampaignExpired);
                        }
                    }

                    if (flag == true)
                    {
                        Campaign.CampaignId = v.id;
                        Campaign.Name = v.name;
                        Campaign.Start = (DateTime)v.start_date;
                        Campaign.End = (DateTime)v.end_date;
                        Merchant merchant = new Merchant()
                        {
                            MerchantId = (int)v.merchant_id,
                            Name = v.Merchant.name,
                        };
                        Campaign.Merchant = merchant;

                        Campaign.Description = v.description;
                        Campaign.Create = (DateTime)v.last_created;
                        Campaign.Update = (DateTime)v.last_updated;
                        Campaign.LastAction = v.last_action;
                        Campaign.LinkURL = v.LINK_URL;

                        List<int> subimgidcampaign = new List<int>();
                        List<string> subimgnamecampaign = new List<string>();
                        List<string> subimgurlcampaign = new List<string>();
                        List<string> subimgurllinkcampaign = new List<string>();
                        List<int> subimgwidth = new List<int>();
                        List<int> subimgheight = new List<int>();
                        List<string> subimgchecksum = new List<string>();

                        if (RuleIdList.Count() > 0)
                        {
                            var entityCampaignRules = from d in context.CampaignRules
                                                      where RuleIdList.Contains((int)d.id)
                                                      select d;
                            foreach (var v2 in entityCampaignRules)
                            {
                                subimgidcampaign.Add(v2.Image.id);
                                subimgnamecampaign.Add(v2.Image.url);
                                subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.Image.url);
                                subimgurllinkcampaign.Add(null);
                                subimgwidth.Add((int)v2.Image.width);
                                subimgheight.Add((int)v2.Image.height);
                                subimgchecksum.Add(v2.Image.checksum);
                            }
                            Campaign.SubRuleId = RuleIdList;
                        }
                        else
                        {
                            var entitySubImgCampaign = from d in context.CampaignImages
                                                       from e in context.Images
                                                       where d.image_id == e.id && d.campaign_id == v.id && e.last_action != "5"
                                                       select new { d.image_id, e.url, d.image_url, e.height, e.width, e.checksum };
                            foreach (var v2 in entitySubImgCampaign)
                            {
                                subimgidcampaign.Add(v2.image_id);
                                subimgnamecampaign.Add(v2.url);
                                subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                                subimgurllinkcampaign.Add(v2.image_url);
                                subimgwidth.Add((int)v2.width);
                                subimgheight.Add((int)v2.height);
                                subimgchecksum.Add(v2.checksum);
                            }
                        }
                        Campaign.SubImageId = subimgidcampaign;
                        Campaign.SubImageName = subimgnamecampaign;
                        Campaign.SubImageUrl = subimgurlcampaign;
                        Campaign.SubImageUrlLink = subimgurllinkcampaign;
                        Campaign.SubImageWidth = subimgwidth;
                        Campaign.SubImageHeight = subimgheight;
                        Campaign.SubImageCheckSum = subimgchecksum;

                        List<Rule> Rules = new List<Rule>();
                        foreach (var c in v.CampaignRules)
                        {
                            if (c.last_action != "5")
                            {
                                Rules.Add(new Rule()
                                {
                                    RuleId = c.id,
                                    NumberOfView = (int)c.numberofviews,
                                    StartTime = (DateTime)c.start_time,
                                    EndTime = (DateTime)c.end_time,
                                    NoEnd = (bool)c.no_end_time,
                                    Gender = (int)c.gender,
                                    LastAction = c.last_action,
                                    Latitude = (double)c.latitude,
                                    Longitude = (double)c.longitude
                                });
                            }
                        }
                        Campaign.Rules = Rules;
                    }
                }
                response = Response<Campaign>.Create(Campaign);
            }

            return response;
        }

        public Response<List<Campaign>> GetNowCampaignList(int gender, double lat, double lng, int age)
        {
            Response<List<Campaign>> response = null;
            List<Campaign> CampaignList = new List<Campaign>();
            using (var context = new TheAdzEntities())
            {
                List<int> NowCampaignIdList = new List<int>();
                var entityRule = from d in context.CampaignRules
                                 where d.start_time <= DateTime.UtcNow && ( d.gender == gender || d.gender == 3 ) && d.last_action != "5"
                                 && ((from ch in context.CampaignHistories where ch.campaign_id == d.campaign_id select d).Count() < d.numberofviews || d.numberofviews == 0)
                                 //&& d.agegroup_start < age && age <= d.agegroup_end
                                 select d;
                //temp
                if (age != -1)
                {
                    entityRule = entityRule.Where(d => d.agegroup_start < age && age <= d.agegroup_end);
                }

                if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Monday)
                    entityRule = entityRule.Where(d => d.monday == true);
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Tuesday)
                    entityRule = entityRule.Where(d => d.tuesday == true);
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Wednesday)
                    entityRule = entityRule.Where(d => d.wednesday == true);
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Thursday)
                    entityRule = entityRule.Where(d => d.thursday == true);
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Friday)
                    entityRule = entityRule.Where(d => d.friday == true);
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday)
                    entityRule = entityRule.Where(d => d.saturday == true);
                else if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                    entityRule = entityRule.Where(d => d.sunday == true);

                entityRule = entityRule.OrderBy(d => d.campaign_id);

                #region old-method
                /*
                foreach (var v in entityRule)
                {
                    //if (Adz.BLL.Lib.Distance.getdistance(lat, lng, (double)v.latitude, (double)v.longitude, 'K') < 500)
                    //{
                    if (v.no_end_time == true)
                    {
                        if (!NowCampaignIdList.Contains((int)v.campaign_id))
                            NowCampaignIdList.Add((int)v.campaign_id);
                    }
                    else
                    {
                        if (DateTime.UtcNow <= v.end_time)
                        {
                            if (!NowCampaignIdList.Contains((int)v.campaign_id))
                                NowCampaignIdList.Add((int)v.campaign_id);
                        }
                    }
                    //}
                }

                var entityCampaign = from d in context.Campaigns.Include("Merchant")
                                     where NowCampaignIdList.Any(nc => nc == d.id) && d.last_action != "5" && d.start_date <= DateTime.Today && d.end_date >= DateTime.Today
                                     orderby d.last_created descending
                                     select d;
                foreach (var v in entityCampaign)
                {
                    Campaign Campaign = new Campaign();
                    Campaign.CampaignId = v.id;
                    Campaign.Name = v.name;
                    Campaign.Start = (DateTime)v.start_date;
                    Campaign.End = (DateTime)v.end_date;
                    Merchant merchant = new Merchant()
                    {
                        MerchantId = (int)v.merchant_id,
                        Name = v.Merchant.name,
                    };
                    Campaign.Merchant = merchant;

                    Campaign.Description = v.description;
                    Campaign.Create = (DateTime)v.last_created;
                    Campaign.Update = (DateTime)v.last_updated;
                    Campaign.LastAction = v.last_action;
                    Campaign.LinkURL = v.LINK_URL;

                    List<int> subimgidcampaign = new List<int>();
                    List<string> subimgnamecampaign = new List<string>();
                    List<string> subimgurlcampaign = new List<string>();
                    List<string> subimgurllinkcampaign = new List<string>();
                    List<int> subimgwidth = new List<int>();
                    List<int> subimgheight = new List<int>();
                    List<string> subimgchecksum = new List<string>();
                    var entitySubImgCampaign = from d in context.CampaignImages
                                               from e in context.Images
                                               where d.image_id == e.id && d.campaign_id == v.id && e.last_action != "5"
                                               select new { d.image_id, e.url, d.image_url, e.height, e.width, e.checksum };
                    foreach (var v2 in entitySubImgCampaign)
                    {
                        subimgidcampaign.Add(v2.image_id);
                        subimgnamecampaign.Add(v2.url);
                        subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                        subimgurllinkcampaign.Add(v2.image_url);
                        subimgwidth.Add((int)v2.width);
                        subimgheight.Add((int)v2.height);
                        subimgchecksum.Add(v2.checksum);
                    }
                    Campaign.SubImageId = subimgidcampaign;
                    Campaign.SubImageName = subimgnamecampaign;
                    Campaign.SubImageUrl = subimgurlcampaign;
                    Campaign.SubImageUrlLink = subimgurllinkcampaign;
                    Campaign.SubImageWidth = subimgwidth;
                    Campaign.SubImageHeight = subimgheight;
                    Campaign.SubImageCheckSum = subimgchecksum;

                    CampaignList.Add(Campaign);
                }
                */
                #endregion

                foreach (var v in entityRule)
                {
                    //if (Adz.BLL.Lib.Distance.getdistance(lat, lng, (double)v.latitude, (double)v.longitude, 'K') < 500)
                    //{

                    int CurrCampaignId = 0;
                    if (v.no_end_time == true)
                    {
                        if (!NowCampaignIdList.Contains((int)v.campaign_id))
                        {
                            NowCampaignIdList.Add((int)v.campaign_id);
                            CurrCampaignId = (int)v.campaign_id;
                        }
                        else
                        {
                            CurrCampaignId = -(int)v.campaign_id;
                        }
                    }
                    else
                    {
                        if (DateTime.UtcNow <= v.end_time)
                        {
                            if (!NowCampaignIdList.Contains((int)v.campaign_id))
                            {
                                NowCampaignIdList.Add((int)v.campaign_id);
                                CurrCampaignId = (int)v.campaign_id;
                            }
                            else
                            {
                                CurrCampaignId = -(int)v.campaign_id;
                            }
                        }
                    }

                    if (CurrCampaignId < 0)
                    {
                        var entityImage = from d in context.Images
                                          where d.id == v.image_id && d.last_action != "5"
                                          select d;
                        if(entityImage.Count() > 0)
                        {
                            if (CampaignList.Last().CampaignId == -CurrCampaignId)
                            {
                                var v2 = entityImage.First();
                                CampaignList.Last().SubImageId.Add(v2.id);
                                CampaignList.Last().SubImageName.Add(v2.url);
                                CampaignList.Last().SubImageUrl.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                                CampaignList.Last().SubImageUrlLink.Add(null); ///
                                CampaignList.Last().SubImageWidth.Add((int)v2.width);
                                CampaignList.Last().SubImageHeight.Add((int)v2.height);
                                CampaignList.Last().SubImageCheckSum.Add(v2.checksum);
                            }
                        }                 
                    }
                    else if (CurrCampaignId > 0)
                    {
                        var entityCampaign = from d in context.Campaigns.Include("Merchant")
                                             where d.id == CurrCampaignId && d.last_action != "5" && d.start_date <= DateTime.Today && d.end_date >= DateTime.Today
                                             select d;

                        if (entityCampaign.Count() > 0)
                        {
                            var c = entityCampaign.First();
                            Campaign Campaign = new Campaign();
                            Campaign.CampaignId = c.id;
                            Campaign.Name = c.name;
                            Campaign.Start = (DateTime)c.start_date;
                            Campaign.End = (DateTime)c.end_date;
                            Merchant merchant = new Merchant()
                            {
                                MerchantId = (int)c.merchant_id,
                                Name = c.Merchant.name,
                            };
                            Campaign.Merchant = merchant;

                            Campaign.Description = c.description;
                            Campaign.Create = (DateTime)c.last_created;
                            Campaign.Update = (DateTime)c.last_updated;
                            Campaign.LastAction = c.last_action;
                            Campaign.LinkURL = c.LINK_URL;

                            List<int> subimgidcampaign = new List<int>();
                            List<string> subimgnamecampaign = new List<string>();
                            List<string> subimgurlcampaign = new List<string>();
                            List<string> subimgurllinkcampaign = new List<string>();
                            List<int> subimgwidth = new List<int>();
                            List<int> subimgheight = new List<int>();
                            List<string> subimgchecksum = new List<string>();
                            var entityImage = from d in context.Images
                                              where d.id == v.image_id && d.last_action != "5"
                                              select d;
                            foreach (var v2 in entityImage)
                            {
                                subimgidcampaign.Add(v2.id);
                                subimgnamecampaign.Add(v2.url);
                                subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                                subimgurllinkcampaign.Add(null); ///
                                subimgwidth.Add((int)v2.width);
                                subimgheight.Add((int)v2.height);
                                subimgchecksum.Add(v2.checksum);
                            }
                            Campaign.SubImageId = subimgidcampaign;
                            Campaign.SubImageName = subimgnamecampaign;
                            Campaign.SubImageUrl = subimgurlcampaign;
                            Campaign.SubImageUrlLink = subimgurllinkcampaign;
                            Campaign.SubImageWidth = subimgwidth;
                            Campaign.SubImageHeight = subimgheight;
                            Campaign.SubImageCheckSum = subimgchecksum;

                            CampaignList.Add(Campaign);
                        }
                    }

                    //}
                }

                //no rules campaign - just directly get into List
                List<int> NowCampaignIdList2 = new List<int>();
                var entityRule2 = from d in context.CampaignRules
                                  where d.last_action != "5"
                                  group d by d.campaign_id into g
                                  select g;
                foreach (var v in entityRule2)
                {
                    NowCampaignIdList2.Add((int)v.Key);
                }

                var entityCampaign2 = from d in context.Campaigns.Include("Merchant")
                                      where !NowCampaignIdList2.Contains(d.id) && d.last_action != "5" && d.start_date <= DateTime.Today && d.end_date >= DateTime.Today
                                      orderby d.last_created descending
                                      select d;

                foreach (var v in entityCampaign2)
                {
                    Campaign Campaign = new Campaign();
                    Campaign.CampaignId = v.id;
                    Campaign.Name = v.name;
                    Campaign.Start = (DateTime)v.start_date;
                    Campaign.End = (DateTime)v.end_date;
                    Merchant merchant = new Merchant()
                    {
                        MerchantId = (int)v.merchant_id,
                        Name = v.Merchant.name,
                    };
                    Campaign.Merchant = merchant;

                    Campaign.Description = v.description;
                    Campaign.Create = (DateTime)v.last_created;
                    Campaign.Update = (DateTime)v.last_updated;
                    Campaign.LastAction = v.last_action;
                    Campaign.LinkURL = v.LINK_URL;

                    List<int> subimgidcampaign = new List<int>();
                    List<string> subimgnamecampaign = new List<string>();
                    List<string> subimgurlcampaign = new List<string>();
                    List<string> subimgurllinkcampaign = new List<string>();
                    List<int> subimgwidth = new List<int>();
                    List<int> subimgheight = new List<int>();
                    List<string> subimgchecksum = new List<string>();
                    var entitySubImgCampaign = from d in context.CampaignImages
                                               from e in context.Images
                                               where d.image_id == e.id && d.campaign_id == v.id
                                               select new { d.image_id, e.url, d.image_url, e.height, e.width, e.checksum };
                    foreach (var v2 in entitySubImgCampaign)
                    {
                        subimgidcampaign.Add(v2.image_id);
                        subimgnamecampaign.Add(v2.url);
                        subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                        subimgurllinkcampaign.Add(v2.image_url);
                        subimgwidth.Add((int)v2.width);
                        subimgheight.Add((int)v2.height);
                        subimgchecksum.Add(v2.checksum);
                    }
                    Campaign.SubImageId = subimgidcampaign;
                    Campaign.SubImageName = subimgnamecampaign;
                    Campaign.SubImageUrl = subimgurlcampaign;
                    Campaign.SubImageUrlLink = subimgurllinkcampaign;
                    Campaign.SubImageWidth = subimgwidth;
                    Campaign.SubImageHeight = subimgheight;
                    Campaign.SubImageCheckSum = subimgchecksum;

                    CampaignList.Add(Campaign);
                }

                response = Response<List<Campaign>>.Create(CampaignList);
            }

            return response;
        }

        public Response<List<Campaign>> GetAllCampaignList()
        {
            Response<List<Campaign>> response = null;
            List<Campaign> CampaignList = new List<Campaign>();
            using (var context = new TheAdzEntities())
            {
                var entityCampaign = from d in context.Campaigns
                                     where d.last_action != "5"
                                     orderby d.last_created descending
                                     select d;
                foreach (var v in entityCampaign)
                {
                    Campaign Campaign = new Campaign();
                    Campaign.CampaignId = v.id;
                    Campaign.Name = v.name;
                    Campaign.Start = (DateTime)v.start_date;
                    Campaign.End = (DateTime)v.end_date;
                    Merchant merchant = new Merchant()
                    {
                        MerchantId = (int)v.merchant_id,
                        Name = v.Merchant.name,
                    };
                    Campaign.Merchant = merchant;

                    Campaign.Description = v.description;
                    Campaign.Create = (DateTime)v.last_created;
                    Campaign.Update = (DateTime)v.last_updated;
                    Campaign.LastAction = v.last_action;
                    Campaign.LinkURL = v.LINK_URL;

                    List<int> subimgidcampaign = new List<int>();
                    List<string> subimgnamecampaign = new List<string>();
                    List<string> subimgurlcampaign = new List<string>();
                    List<string> subimgurllinkcampaign = new List<string>();
                    var entitySubImgCampaign = from d in context.CampaignImages
                                               from e in context.Images
                                               where d.image_id == e.id && d.campaign_id == v.id
                                               select new { d.image_id, e.url, d.image_url };
                    foreach (var v2 in entitySubImgCampaign)
                    {
                        subimgidcampaign.Add(v2.image_id);
                        subimgnamecampaign.Add(v2.url);
                        subimgurlcampaign.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                        subimgurllinkcampaign.Add(v2.image_url);
                    }
                    Campaign.SubImageId = subimgidcampaign;
                    Campaign.SubImageName = subimgnamecampaign;
                    Campaign.SubImageUrl = subimgurlcampaign;
                    Campaign.SubImageUrlLink = subimgurllinkcampaign;

                    CampaignList.Add(Campaign);
                }
                response = Response<List<Campaign>>.Create(CampaignList);
            }

            return response;
        }
        
        public Response<List<CampaignHistory>> GetCampaignHistory(int action)
        {
            Response<List<CampaignHistory>> response = null;
            List<CampaignHistory> CampaignHistoryList = new List<CampaignHistory>();
            using (var context = new TheAdzEntities())
            {
                var entityCampaignHistory = from d in context.CampaignHistories
                                            where d.action == action
                                            orderby d.view_date descending
                                            select d;

                foreach (var v in entityCampaignHistory)
                {
                    CampaignHistoryList.Add(new CampaignHistory()
                    {
                        User = new User() { UserId = (int)v.user_id, Email = v.User.email },
                        Campaign = new Campaign() { CampaignId = (int)v.campaign_id, Name = v.Campaign.name },
                        Latitude = (double)v.latitude, Longitude = (double)v.longitude, Gender = (int)v.gender,
                        DateTime = (DateTime)v.view_date
                    });
                }

                response = Response<List<CampaignHistory>>.Create(CampaignHistoryList);
            }

            return response;
        }

        public Response<List<CampaignHistory>> GetCampaignHistoryByCampaignId(int CampaignId, int Action)
        {
            Response<List<CampaignHistory>> response = null;
            List<CampaignHistory> CampaignHistoryList = new List<CampaignHistory>();
            using (var context = new TheAdzEntities())
            {
                var entityCampaignHistory = from d in context.CampaignHistories
                                            where d.campaign_id == CampaignId
                                            orderby d.view_date descending
                                            select d;

                if (Action != 0)
                {
                    entityCampaignHistory = entityCampaignHistory.Where(p => p.action == Action).OrderByDescending(p => p.view_date);
                }

                foreach (var v in entityCampaignHistory)
                {
                    CampaignHistoryList.Add(new CampaignHistory()
                    {
                        User = new User() { UserId = (int)v.user_id, Email = v.User.email, DateOfBirth = v.User.dateofbirth == null ? DateTime.MinValue : (DateTime)v.User.dateofbirth },
                        Campaign = new Campaign() { CampaignId = (int)v.campaign_id, Name = v.Campaign.name },
                        Latitude = (double)v.latitude,
                        Longitude = (double)v.longitude,
                        Gender = (int)v.gender,
                        DateTime = (DateTime)v.view_date,
                        Action = (int)v.action
                    });
                }

                response = Response<List<CampaignHistory>>.Create(CampaignHistoryList);
            }

            return response;
        }

        public Response<List<CampaignHistoryGroup>> GetCampaignHistoryGroup()
        {
            Response<List<CampaignHistoryGroup>> response = null;
            List<CampaignHistoryGroup> CampaignHistoryGroupList = new List<CampaignHistoryGroup>();
            using (var context = new TheAdzEntities())
            {
                var entityCampaignHistory = from d in context.CampaignHistories
                                            where d.campaign_id != null
                                            group d by new { d.Campaign.id, d.Campaign.name } into g
                                            select new
                                            {
                                                id = g.Key.id,
                                                name = g.Key.name,
                                                count = g.Count(),
                                                view_count = (from x in context.CampaignHistories
                                                              where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.View
                                                              select x.campaign_id
                                                              ).Count(),
                                                click_count = (from x in context.CampaignHistories
                                                               where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.Click
                                                               select x.campaign_id
                                                               ).Count(),
                                                view_lastview = (from x in context.CampaignHistories
                                                                 where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.View
                                                                 orderby x.view_date descending
                                                                 select x.view_date
                                                                 ).FirstOrDefault(),
                                                click_lastview = (from x in context.CampaignHistories
                                                                  where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.Click
                                                                  orderby x.view_date descending
                                                                  select x.view_date
                                                                  ).FirstOrDefault()
                                            };

                foreach (var v in entityCampaignHistory)
                {
                    CampaignHistoryGroupList.Add(new CampaignHistoryGroup()
                    {
                        Campaign = new Campaign() { CampaignId = (int)v.id, Name = v.name },
                        Views = (int)v.view_count,
                        LastViewTime = v.view_lastview == null ? DateTime.MinValue : (DateTime)v.view_lastview,
                        Clicks = (int)v.click_count,
                        LastClickTime = v.click_lastview == null ? DateTime.MinValue : (DateTime)v.click_lastview
                    });
                }

                response = Response<List<CampaignHistoryGroup>>.Create(CampaignHistoryGroupList);
            }

            return response;
        }

        public Response<CampaignHistoryGroup> GetCampaignHistoryGroupById(int CampaignId)
        {
            Response<CampaignHistoryGroup> response = null;
            CampaignHistoryGroup CampaignHistoryGroup = new CampaignHistoryGroup();
            using (var context = new TheAdzEntities())
            {
                var entityCampaignHistory = from d in context.CampaignHistories
                                            where d.campaign_id == CampaignId
                                            group d by new { d.Campaign.id, d.Campaign.name } into g
                                            select new
                                            {
                                                id = g.Key.id,
                                                name = g.Key.name,
                                                count = g.Count(),
                                                view_count = (from x in context.CampaignHistories
                                                              where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.View
                                                              select x.campaign_id
                                                              ).Count(),
                                                click_count = (from x in context.CampaignHistories
                                                               where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.Click
                                                               select x.campaign_id
                                                               ).Count(),
                                                view_lastview = (from x in context.CampaignHistories
                                                                 where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.View
                                                                 orderby x.view_date descending
                                                                 select x.view_date
                                                                 ).FirstOrDefault(),
                                                click_lastview = (from x in context.CampaignHistories
                                                                  where x.campaign_id == g.Key.id && x.action == (int)CampaignHistoryAction.Click
                                                                  orderby x.view_date descending
                                                                  select x.view_date
                                                                  ).FirstOrDefault()
                                            };

                if (entityCampaignHistory.Count()>0)
                {
                    var v = entityCampaignHistory.First();
                    CampaignHistoryGroup.Campaign = new Campaign() { CampaignId = (int)v.id, Name = v.name };
                    CampaignHistoryGroup.Views = (int)v.view_count;
                    CampaignHistoryGroup.LastViewTime = v.view_lastview == null ? DateTime.MinValue : (DateTime)v.view_lastview;
                    CampaignHistoryGroup.Clicks = (int)v.click_count;
                    CampaignHistoryGroup.LastClickTime = v.click_lastview == null ? DateTime.MinValue : (DateTime)v.click_lastview;
                }

                response = Response<CampaignHistoryGroup>.Create(CampaignHistoryGroup);
            }

            return response;
        }

        public Response<List<CampaignHistoryGroupByDate>> GetCampaignHistoryGroupByDateAndUser(int UserId)
        {
            Response<List<CampaignHistoryGroupByDate>> response = null;
            List<CampaignHistoryGroupByDate> CampaignHistoryGroupByDateList = new List<CampaignHistoryGroupByDate>();
            using (var context = new TheAdzEntities())
            {
                var ett = from d in context.CampaignHistories
                          where d.user_id == UserId
                          && d.view_date != null
                          group d by new { Year = d.view_date.Value.Year, Month = d.view_date.Value.Month, Day = d.view_date.Value.Day } into g
                          orderby new { g.Key.Year, g.Key.Month, g.Key.Day }
                          select new
                          {
                              Date = g.Key,
                              count = g.Count(),
                              view_count = g.Where(m => m.action == (int)CampaignHistoryAction.View).Count(),
                              click_count = g.Where(m => m.action == (int)CampaignHistoryAction.Click).Count(),
                              isGetPoint = g.FirstOrDefault().remarks == "Success"
                          };

                foreach (var v in ett)
                {
                    CampaignHistoryGroupByDateList.Add(new CampaignHistoryGroupByDate()
                    {
                        Date = new DateTime(v.Date.Year, v.Date.Month, v.Date.Day),
                        Views = (int)v.view_count,
                        Clicks = (int)v.click_count,
                        isGetPoint = v.isGetPoint
                    });
                }

                response = Response<List<CampaignHistoryGroupByDate>>.Create(CampaignHistoryGroupByDateList);
            }

            return response;
        }

        public Response<int> IncrementCampaignView(int CampaignId, string username, int gender=0, double lat=0, double lng=0)
        {
            Response<int> response = null;

            using (var context = new TheAdzEntities())
            {
                var entityCampaign = from d in context.Campaigns.Include("Merchant")
                                     where d.id == CampaignId
                                     select d;
                var v = entityCampaign.First();
                if (v != null)
                {
                    var entityUser = from d in context.Users
                                     where d.email.ToLower() == username.ToLower()
                                     && d.locked_status == false
                                     && (d.userstatus_id == 1 || d.userstatus_id == 4)
                                     select d.id;
                    if (entityUser.Count() <= 0)
                    {
                        throw new CustomException(CustomErrorType.UserServiceNotFound);
                    }

                    bool flag = false;

                    var entityRule = from d in context.CampaignRules
                                     where d.last_action != "5"
                                     && d.campaign_id == CampaignId
                                     select d;
                    if (entityRule.Count() > 0)
                    {
                        entityRule = from d in context.CampaignRules
                                     where d.last_action != "5"
                                     && d.campaign_id == CampaignId
                                     && d.start_time <= DateTime.UtcNow && d.gender == gender && (from ch in context.CampaignHistories where ch.campaign_id == d.campaign_id select d).Count() < d.numberofviews
                                     select d;

                        if (entityRule.Count() > 0)
                        {
                            foreach (var v2 in entityRule)
                            {
                                //if (Adz.BLL.Lib.Distance.getdistance(lat, lng, (double)v2.latitude, (double)v2.longitude, 'K') < 500)
                                //{
                                if (v2.no_end_time == true)
                                {
                                    flag = true;
                                    break;
                                }
                                else
                                {
                                    if (DateTime.UtcNow <= v2.end_time)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                //}
                            }
                        }
                    }
                    else
                    {
                        flag = true;
                    }

                    if (flag == true)
                    {
                        Adz.DAL.EF.CampaignHistory mmentity = new Adz.DAL.EF.CampaignHistory();
                        mmentity.campaign_id = CampaignId;
                        mmentity.user_id = entityUser.First();
                        mmentity.view_date = DateTime.UtcNow;
                        mmentity.created_date = DateTime.UtcNow;
                        mmentity.latitude = lat;
                        mmentity.longitude = lng;
                        mmentity.gender = gender;
                        mmentity.action = 1;
                        mmentity.flag_processed = false;
                        context.CampaignHistories.Add(mmentity);
                        context.SaveChanges();
                    }

                    var entityCampaignHistory = from d in context.CampaignHistories
                                                where d.campaign_id == CampaignId
                                                select d.campaign_id;

                    int NumberOfViews = entityCampaignHistory.Count();

                    NumberOfViews = flag == false ? (NumberOfViews * -1) : NumberOfViews;

                    response = Response<int>.Create(NumberOfViews);
                }
            }

            return response;
        }

        public SensiteGraph GetSensiteGraphValueByActiveDays(List<int> XDaysActive)
        {
            SensiteGraph result = new SensiteGraph()
            {
                SensiteTotalUser = new List<double>(),
                AverageDaysActive = new List<double>(),
                AverageDaysView = new List<double>(),
                AverageCTR = new List<double>()
            };

            using (var context = new TheAdzEntities())
            {
                foreach (var v in XDaysActive)
                {
                    var ett = from j in
                                  (from e in
                                       (from d in context.CampaignHistories
                                        where d.action == (int)CampaignHistoryAction.View
                                        group d by new
                                        {
                                            d.user_id,
                                            date = EntityFunctions.TruncateTime(d.view_date.Value)
                                        } into g
                                        select new
                                        {
                                            user_id = g.Key.user_id,
                                            view_date = g.Key.date,
                                            count = g.Count()
                                        })
                                   group e by e.user_id into h
                                   select new
                                   {
                                       user_id = h.Key,
                                       daycount = h.Count()
                                   })
                              join k in
                                  (from c in context.CampaignHistories
                                   where c.action == (int)CampaignHistoryAction.View
                                   group c by new
                                   {
                                       c.user_id
                                   } into i
                                   select new
                                   {
                                       user_id = i.Key.user_id,
                                       viewcount = i.Count()
                                   }) on j.user_id equals k.user_id into k
                              from y in k.DefaultIfEmpty()
                              join l in
                                  (from b in context.CampaignHistories
                                   where b.action == (int)CampaignHistoryAction.Click
                                   group b by new
                                   {
                                       b.user_id
                                   } into a
                                   select new
                                   {
                                       user_id = a.Key.user_id,
                                       clickcount = a.Count()
                                   }) on j.user_id equals l.user_id into l
                              from x in l.DefaultIfEmpty()
                              where j.daycount >= v
                              select new
                              {
                                  user_id = j.user_id,
                                  daycount = j.daycount,
                                  viewcount = y.viewcount == null ? 0 : y.viewcount,
                                  averagedailyviews = (y.viewcount == null ? 0 : ((double)y.viewcount / (double)j.daycount)),
                                  clickcount = x.clickcount == null ? 0 : x.clickcount,
                                  ctr = (x.clickcount == null ? 0 : ((double)x.clickcount) / (double)y.viewcount),
                              };

                    var UserActiveCount = ett.Count();
                    var AverageDaysActive = ett.Count() == 0 ? 0 : ett.Average(m => m.daycount);
                    var AverageDaysView = ett.Count() == 0 ? 0 : ett.Average(m => m.averagedailyviews);
                    var AverageCTR = ett.Count() == 0 ? 0 : ett.Average(m => m.ctr);

                    result.SensiteTotalUser.Add(UserActiveCount);
                    result.AverageDaysActive.Add(AverageDaysActive);
                    result.AverageDaysView.Add(AverageDaysView);
                    result.AverageCTR.Add(AverageCTR);
                }
            }
            return result;
        }

        public SensiteGraph GetSensiteGraphValueByDailyView(List<int> XDailyViews)
        {
            SensiteGraph result = new SensiteGraph()
            {
                SensiteTotalUser = new List<double>(),
                AverageDaysActive = new List<double>(),
                AverageDaysView = new List<double>(),
                AverageCTR = new List<double>()
            };

            using (var context = new TheAdzEntities())
            {
                foreach (var v in XDailyViews)
                {
                    var ett = from j in
                                  (from e in
                                       (from d in context.CampaignHistories
                                        where d.action == (int)CampaignHistoryAction.View
                                        group d by new
                                        {
                                            d.user_id,
                                            date = EntityFunctions.TruncateTime(d.view_date.Value)
                                        } into g
                                        select new
                                        {
                                            user_id = g.Key.user_id,
                                            view_date = g.Key.date,
                                            count = g.Count()
                                        })
                                   group e by e.user_id into h
                                   select new
                                   {
                                       user_id = h.Key,
                                       daycount = h.Count()
                                   })
                              join k in
                                  (from c in context.CampaignHistories
                                   where c.action == (int)CampaignHistoryAction.View
                                   group c by new
                                   {
                                       c.user_id
                                   } into i
                                   select new
                                   {
                                       user_id = i.Key.user_id,
                                       viewcount = i.Count()
                                   }) on j.user_id equals k.user_id into k
                              from y in k.DefaultIfEmpty()
                              join l in
                                  (from b in context.CampaignHistories
                                   where b.action == (int)CampaignHistoryAction.Click
                                   group b by new
                                   {
                                       b.user_id
                                   } into a
                                   select new
                                   {
                                       user_id = a.Key.user_id,
                                       clickcount = a.Count()
                                   }) on j.user_id equals l.user_id into l
                              from x in l.DefaultIfEmpty()
                              where (y.viewcount == null ? 0 : ((double)y.viewcount / (double)j.daycount)) >= v
                              select new
                              {
                                  user_id = j.user_id,
                                  daycount = j.daycount,
                                  viewcount = y.viewcount == null ? 0 : y.viewcount,
                                  averagedailyviews = (y.viewcount == null ? 0 : ((double)y.viewcount / (double)j.daycount)),
                                  clickcount = x.clickcount == null ? 0 : x.clickcount,
                                  ctr = (x.clickcount == null ? 0 : ((double)x.clickcount) / (double)y.viewcount),
                              };

                    var UserActiveCount = ett.Count();
                    var AverageDaysActive = ett.Count() == 0 ? 0 : ett.Average(m => m.daycount);
                    var AverageDaysView = ett.Count() == 0 ? 0 : ett.Average(m => m.averagedailyviews);
                    var AverageCTR = ett.Count() == 0 ? 0 : ett.Average(m => m.ctr);

                    result.SensiteTotalUser.Add(UserActiveCount);
                    result.AverageDaysActive.Add(AverageDaysActive);
                    result.AverageDaysView.Add(AverageDaysView);
                    result.AverageCTR.Add(AverageCTR);
                }
            }
            return result;
        }

        public GenderGraph GetGenderOverallViews()
        {
            GenderGraph result = new GenderGraph();
            using (var context = new TheAdzEntities())
            {
                var ett = from x in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.View
                               group d by d.gender into g
                               select new
                               {
                                   gender = g.Key == null ? 0 : g.Key,
                                   view_count = g.Count()
                               })
                          join y in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.Click
                               group d by d.gender into g
                               select new
                               {
                                   gender = g.Key == null ? 0 : g.Key,
                                   click_count = g.Count()
                               }) on x.gender equals y.gender into y
                          from z in y.DefaultIfEmpty()
                          select new
                          {
                              gender = x.gender,
                              view_count = x.view_count,
                              click_count = z.click_count,
                              ctr = (decimal)z.click_count / (decimal)x.view_count
                          };

                var Male = ett.Where(m => m.gender == 1);
                if (Male.Count() > 0)
                {
                    result.Male = Male.First().view_count;
                }
                else
                {
                    result.Male = 0;
                }

                var Female = ett.Where(m => m.gender == 2);
                if (Female.Count() > 0)
                {
                    result.Female = Female.First().view_count;
                }
                else
                {
                    result.Female = 0;
                }

                var Unknown = ett.Where(m => m.gender == 0 || m.gender == null);
                if (Unknown.Count() > 0)
                {
                    result.Unknown = Unknown.First().view_count;
                }
                else
                {
                    result.Unknown = 0;
                }
            }

            return result;
        }

        public GenderGraph GetGenderOverallCTR()
        {
            GenderGraph result = new GenderGraph();
            using (var context = new TheAdzEntities())
            {
                var ett = from x in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.View
                               group d by d.gender into g
                               select new
                               {
                                   gender = g.Key == null ? 0 : g.Key,
                                   view_count = g.Count()
                               })
                          join y in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.Click
                               group d by d.gender into g
                               select new
                               {
                                   gender = g.Key == null ? 0 : g.Key,
                                   click_count = g.Count()
                               }) on x.gender equals y.gender into y
                          from z in y.DefaultIfEmpty()
                          select new
                          {
                              gender = x.gender,
                              view_count = x.view_count,
                              click_count = z.click_count,
                              ctr = (decimal)z.click_count / (decimal)x.view_count
                          };

                var Male = ett.Where(m => m.gender == 1);
                if (Male.Count() > 0)
                {
                    result.Male = Male.First().ctr;
                }
                else
                {
                    result.Male = 0;
                }

                var Female = ett.Where(m => m.gender == 2);
                if (Female.Count() > 0)
                {
                    result.Female = Female.First().ctr;
                }
                else
                {
                    result.Female = 0;
                }

                var Unknown = ett.Where(m => m.gender == 0 || m.gender == null);
                if (Unknown.Count() > 0)
                {
                    result.Unknown = Unknown.First().ctr;
                }
                else
                {
                    result.Unknown = 0;
                }
            }

            return result;
        }

        public GenderGraphCombine GetGenderTotalViewAndCTRByCampaign(int CampaignId)
        {
            GenderGraphCombine result = new GenderGraphCombine();
            result.TotalView = new GenderGraph();
            result.CTR = new GenderGraph();
            using (var context = new TheAdzEntities())
            {
                var ett = from x in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.View
                               && d.campaign_id == CampaignId
                               group d by d.gender into g
                               select new
                               {
                                   gender = g.Key == null ? 0 : g.Key,
                                   view_count = g.Count()
                               })
                          join y in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.Click
                               && d.campaign_id == CampaignId
                               group d by d.gender into g
                               select new
                               {
                                   gender = g.Key == null ? 0 : g.Key,
                                   click_count = g.Count()
                               }) on x.gender equals y.gender into y
                          from z in y.DefaultIfEmpty()
                          select new
                          {
                              gender = x.gender == null ? 0 : x.gender,
                              view_count = x.view_count,
                              click_count = z.click_count,
                              ctr = (decimal)z.click_count / (decimal)x.view_count
                          };

                foreach (var v in ett)
                {
                    if (v.gender == 1)
                    {
                        result.TotalView.Male = v.view_count;
                        result.CTR.Male = v.ctr;
                    }
                    else if (v.gender == 2)
                    {
                        result.TotalView.Female = v.view_count;
                        result.CTR.Female = v.ctr;
                    }
                    else if (v.gender == 0 || v.gender == null)
                    {
                        result.TotalView.Unknown = v.view_count;
                        result.CTR.Unknown = v.ctr;
                    }
                }

                //var Male = ett.Where(m => m.gender == 1);
                //if (Male.Count() > 0)
                //{
                //    result.TotalView.Male = Male.First().view_count;
                //    result.CTR.Male = Male.First().ctr;
                //}
                //else
                //{
                //    result.TotalView.Male = 0;
                //    result.CTR.Male = 0;
                //}

                //var Female = ett.Where(m => m.gender == 2);
                //if (Female.Count() > 0)
                //{
                //    result.TotalView.Female = Female.First().view_count;
                //    result.CTR.Female = Female.First().ctr;
                //}
                //else
                //{
                //    result.TotalView.Female = 0;
                //    result.CTR.Female = 0;
                //}

                //var Unknown = ett.Where(m => m.gender == 0 || m.gender == null);
                //if (Unknown.Count() > 0)
                //{
                //    result.TotalView.Unknown = Unknown.First().view_count;
                //    result.CTR.Unknown = Unknown.First().ctr;
                //}
                //else
                //{
                //    result.TotalView.Unknown = 0;
                //    result.CTR.Unknown = 0;
                //}
            }

            return result;
        }



        public WeekdaysGraphCombine GetWeekdaysTotalViewAndCTRByCampaign(int CampaignId)
        {
            WeekdaysGraphCombine result = new WeekdaysGraphCombine();
            result.TotalView = new WeekdaysGraph();
            result.CTR = new WeekdaysGraph();
            using (var context = new TheAdzEntities())
            {
                List<int> weekdays = new List<int>();
                for (int i = 1; i <= 7; i++)
                {
                    weekdays.Add(i);
                }
                var ett = from v in
                              (from d in weekdays
                               select new
                               {
                                   weekday = d
                               })
                          join x in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.View
                               && d.campaign_id == CampaignId
                               group d by SqlFunctions.DatePart("weekday", d.view_date) into g
                               select new
                               {
                                   weekday = g.Key == null ? 0 : g.Key,
                                   view_count = g.Count()
                               }) on v.weekday equals x.weekday into x
                          from w in x.DefaultIfEmpty()
                          join y in
                              (from d in context.CampaignHistories
                               where d.action == (int)CampaignHistoryAction.Click
                               && d.campaign_id == CampaignId
                               group d by SqlFunctions.DatePart("weekday", d.view_date) into g
                               select new
                               {
                                   weekday = g.Key == null ? 0 : g.Key,
                                   click_count = g.Count()
                               }) on v.weekday equals y.weekday into y
                          from z in y.DefaultIfEmpty()
                          select new
                          {
                              weekday = v.weekday,
                              view_count = w == null ? 0 : w.view_count,
                              click_count = z == null ? 0 : z.click_count,
                              ctr = w == null || z == null ? 0 : (decimal)z.click_count / (decimal)w.view_count
                          };

                foreach (var v in ett)
                {
                    if (v.weekday == 1)
                    {
                        result.TotalView.Sunday = v.view_count;
                        result.CTR.Sunday = v.ctr;
                    }
                    else if (v.weekday == 2)
                    {
                        result.TotalView.Monday = v.view_count;
                        result.CTR.Monday = v.ctr;
                    }
                    else if (v.weekday == 3)
                    {
                        result.TotalView.Tuesday = v.view_count;
                        result.CTR.Tuesday = v.ctr;
                    }
                    else if (v.weekday == 4)
                    {
                        result.TotalView.Wednesday = v.view_count;
                        result.CTR.Wednesday = v.ctr;
                    }
                    else if (v.weekday == 5)
                    {
                        result.TotalView.Thursday = v.view_count;
                        result.CTR.Thursday = v.ctr;
                    }
                    else if (v.weekday == 6)
                    {
                        result.TotalView.Friday = v.view_count;
                        result.CTR.Friday = v.ctr;
                    }
                    else if (v.weekday == 7)
                    {
                        result.TotalView.Saturday = v.view_count;
                        result.CTR.Saturday = v.ctr;
                    }
                }
            }

            return result;
        }

    }
}
