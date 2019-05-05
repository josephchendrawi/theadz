using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class PromotionService : IPromotionService
    {
        public Response<List<Promotion>> GetPromotionList()
        {
            Response<List<Promotion>> response = null;
            List<Promotion> PromotionList = new List<Promotion>();
            using (var context = new TheAdzEntities())
            {
                var entityPromotion = from d in context.Promotions
                                      where d.last_action != "5"
                                      orderby d.last_created descending
                                      select d;
                foreach (var v in entityPromotion)
                {
                    Promotion Promotion = new Promotion();
                    Promotion.PromotionId = v.id;
                    Promotion.PromoCode = v.code;
                    Promotion.Description = v.description;
                    Promotion.Value = v.value ?? 0;
                    Promotion.Create = v.last_created ?? DateTime.MinValue;
                    Promotion.LastAction = v.last_action;

                    Promotion.StartAt = v.start_datetime ?? DateTime.MinValue;
                    Promotion.EndAt = v.end_datetime ?? DateTime.MinValue;
                    Promotion.OnScheduleFlag = v.flag_on_schedule ?? false;

                    PromotionList.Add(Promotion);
                }
                response = Response<List<Promotion>>.Create(PromotionList);
            }

            return response;
        }

        public Response<Promotion> GetPromotionById(int PromotionId)
        {
            Response<Promotion> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityPromotion = from d in context.Promotions
                                      where d.last_action != "5"
                                      && d.id == PromotionId
                                      select d;

                if (entityPromotion.Count() > 0)
                {
                    var v = entityPromotion.First();

                    Promotion Promotion = new Promotion();
                    Promotion.PromotionId = v.id;
                    Promotion.PromoCode = v.code;
                    Promotion.Description = v.description;
                    Promotion.Value = v.value ?? 0;
                    Promotion.Create = v.last_created ?? DateTime.MinValue;
                    Promotion.LastAction = v.last_action;

                    Promotion.StartAt = v.start_datetime ?? DateTime.MinValue;
                    Promotion.EndAt = v.end_datetime ?? DateTime.MinValue;
                    Promotion.OnScheduleFlag = v.flag_on_schedule ?? false;

                    response = Response<Promotion>.Create(Promotion);
                }
            }

            return response;
        }

        public Response<int> CreateEditPromotion(Promotion Promotion)
        {
            Response<int> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Promotion.PromotionId != 0)
                {
                    var entityPromotion = from d in context.Promotions
                                          where d.id == Promotion.PromotionId
                                          select d;
                    if (entityPromotion.Count() > 0)
                    {
                        var entityPromotion2 = from d in context.Promotions
                                               where d.code.ToLower() == Promotion.PromoCode.ToLower()
                                               && d.id != Promotion.PromotionId
                                               select d;
                        if (entityPromotion2.Count() > 0)
                        {
                            throw new CustomException(CustomErrorType.PromotionAlreadyAssign);
                        }
                        else
                        {
                            entityPromotion.First().code = Promotion.PromoCode;
                            entityPromotion.First().description = Promotion.Description;
                            entityPromotion.First().value = Promotion.Value;

                            entityPromotion.First().start_datetime = Promotion.StartAt;
                            entityPromotion.First().end_datetime = Promotion.EndAt;
                            entityPromotion.First().flag_on_schedule = Promotion.OnScheduleFlag;

                            entityPromotion.First().last_updated = DateTime.UtcNow;
                            context.SaveChanges();

                            response = Response<int>.Create(Promotion.PromotionId);
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.PromotionNotFound);
                    }
                }
                else
                {
                    var entityPromotion = from d in context.Promotions
                                         where d.code.ToLower() == Promotion.PromoCode.ToLower()
                                         select d;
                    if (entityPromotion.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.PromotionAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.Promotion mmentity = new Adz.DAL.EF.Promotion();
                        mmentity.code = Promotion.PromoCode;
                        mmentity.description = Promotion.Description;
                        mmentity.value = Promotion.Value;

                        mmentity.start_datetime = Promotion.StartAt.Value.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                        mmentity.end_datetime = Promotion.EndAt.Value.AddHours(-1 * int.Parse(ConfigurationManager.AppSettings["GMT"]));
                        mmentity.flag_on_schedule = Promotion.OnScheduleFlag;

                        mmentity.last_created = DateTime.UtcNow;
                        mmentity.last_updated = DateTime.UtcNow;
                        mmentity.last_action = "1";
                        context.Promotions.Add(mmentity);
                        context.SaveChanges();
                        int PromotionId = mmentity.id;
                        
                        response = Response<int>.Create(PromotionId);
                    }
                }
            }
            return response;
        }

        public Response<bool> DeletePromotion(int PromotionId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityPromotion = from d in context.Promotions
                                 where d.id == PromotionId
                                 select d;
                if (entityPromotion.Count() > 0)
                {
                    entityPromotion.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.PromotionFailedDelete);
                }
            }

            return response;
        }

        public Response<bool> RunPromotion(int PromotionId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityPromotion = from d in context.Promotions
                                      where d.id == PromotionId
                                      select d;
                if (entityPromotion.Count() > 0)
                {
                    entityPromotion.First().last_action = ((int)PromotionStatus.OnGoing).ToString();
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.PromotionFailedRun);
                }
            }

            return response;
        }

        public Response<bool> StopPromotion(int PromotionId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityPromotion = from d in context.Promotions
                                      where d.id == PromotionId
                                      select d;
                if (entityPromotion.Count() > 0)
                {
                    entityPromotion.First().last_action = ((int)PromotionStatus.Stopped).ToString();
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.PromotionFailedStop);
                }
            }

            return response;
        }

    }
}
