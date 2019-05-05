using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class RuleService : IRuleService
    {
        public Response<Rule> GetRuleById(int RuleId)
        {
            Response<Rule> response = null;
            Rule Rule = new Rule();
            using (var context = new TheAdzEntities())
            {
                var entityRule = from d in context.CampaignRules
                                   where d.id == RuleId
                                   select d;
                var v = entityRule.First();
                if (v != null)
                {
                    Rule.RuleId = v.id;
                    Rule.NumberOfView = (int)v.numberofviews;
                    Rule.StartTime = (DateTime)v.start_time;
                    Rule.EndTime = (DateTime)v.end_time;
                    Rule.NoEnd = (bool)v.no_end_time;
                    Rule.Gender = (int)v.gender;
                    Rule.LastAction = v.last_action;
                    Rule.Latitude = (double)v.latitude;
                    Rule.Longitude = (double)v.longitude;
                    Rule.Monday = v.monday == null ? false : (bool)v.monday;
                    Rule.Tuesday = v.tuesday == null ? false : (bool)v.tuesday;
                    Rule.Wednesday = v.wednesday == null ? false : (bool)v.wednesday;
                    Rule.Thursday = v.thursday == null ? false : (bool)v.thursday;
                    Rule.Friday = v.friday == null ? false : (bool)v.friday;
                    Rule.Saturday = v.saturday == null ? false : (bool)v.saturday;
                    Rule.Sunday = v.sunday == null ? false : (bool)v.sunday;
                    Rule.AgeGroupStart = v.agegroup_start == null ? -1 : (int)v.agegroup_start;
                    Rule.AgeGroupEnd = v.agegroup_end == null ? -1 : (int)v.agegroup_end;
                    Rule.ImageId = v.image_id == null ? 0 : (int)v.image_id;
                }
                response = Response<Rule>.Create(Rule);
            }

            return response;
        }

        public Response<Rule> CreateEditRule(Rule Rule)
        {
            Response<Rule> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Rule.RuleId != 0)
                {
                    var entityRule = from d in context.CampaignRules
                                       where d.id == Rule.RuleId
                                       select d;
                    if (entityRule.Count() > 0)
                    {
                        entityRule.First().numberofviews = Rule.NumberOfView;
                        entityRule.First().start_time = Rule.StartTime.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                        entityRule.First().end_time = Rule.EndTime.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                        entityRule.First().no_end_time = Rule.NoEnd;
                        entityRule.First().gender = Rule.Gender;
                        entityRule.First().latitude = Rule.Latitude;
                        entityRule.First().longitude = Rule.Longitude;
                        entityRule.First().last_updated = DateTime.UtcNow;
                        entityRule.First().last_action = "3";

                        entityRule.First().monday = Rule.Monday;
                        entityRule.First().tuesday = Rule.Tuesday;
                        entityRule.First().wednesday = Rule.Wednesday;
                        entityRule.First().thursday = Rule.Thursday;
                        entityRule.First().friday = Rule.Friday;
                        entityRule.First().saturday = Rule.Saturday;
                        entityRule.First().sunday = Rule.Sunday;
                        entityRule.First().agegroup_start = Rule.AgeGroupStart;
                        entityRule.First().agegroup_end = Rule.AgeGroupEnd;
                        entityRule.First().image_id = Rule.ImageId;

                        context.SaveChanges();

                        response = Response<Rule>.Create(new Rule() { Success = true });
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.RuleNotFound);
                    }
                }
                else
                {
                    Adz.DAL.EF.CampaignRule mmentity = new Adz.DAL.EF.CampaignRule();
                    mmentity.numberofviews = Rule.NumberOfView;
                    mmentity.start_time = Rule.StartTime.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                    mmentity.end_time = Rule.EndTime.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                    mmentity.no_end_time = Rule.NoEnd;
                    mmentity.gender = Rule.Gender;
                    mmentity.latitude = Rule.Latitude;
                    mmentity.longitude = Rule.Longitude;
                    mmentity.last_created = DateTime.UtcNow;
                    mmentity.last_updated = DateTime.UtcNow;
                    mmentity.last_action = "1";

                    mmentity.monday = Rule.Monday;
                    mmentity.tuesday = Rule.Tuesday;
                    mmentity.wednesday = Rule.Wednesday;
                    mmentity.thursday = Rule.Thursday;
                    mmentity.friday = Rule.Friday;
                    mmentity.saturday = Rule.Saturday;
                    mmentity.sunday = Rule.Sunday;
                    mmentity.agegroup_start = Rule.AgeGroupStart;
                    mmentity.agegroup_end = Rule.AgeGroupEnd;
                    mmentity.image_id = Rule.ImageId;

                    mmentity.campaign_id = Rule.Campaign.CampaignId;
                    context.CampaignRules.Add(mmentity);
                    context.SaveChanges();
                    int RuleId = mmentity.id;

                    response = Response<Rule>.Create(new Rule() { RuleId = RuleId, Success = true });
                }
            }

            return response;
        }

        public Response<bool> DeleteRule(int RuleId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityRule = from d in context.CampaignRules
                                   where d.id == RuleId
                                   select d;
                if (entityRule.Count() > 0)
                {
                    entityRule.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.RuleFailedDelete);
                }
            }

            return response;
        }

    }
}
