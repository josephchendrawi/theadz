using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class RewardService : IRewardService
    {
        public IUserTrxService UserTrxService = new UserTrxService();

        public Response<List<Reward>> GetRewardList()
        {
            Response<List<Reward>> response = null;
            List<Reward> RewardList = new List<Reward>();
            using (var context = new TheAdzEntities())
            {
                var entityReward = from d in context.Rewards
                                  orderby d.last_created descending
                                  select d;
                foreach (var v in entityReward)
                {
                    Reward Reward = new Reward();
                    Reward.RewardId = v.id;
                    Reward.Name = v.name;
                    Reward.SponsorName = v.sponsor_name;

                    Reward.Description = v.description;
                    Reward.Create = (DateTime)v.last_created;
                    Reward.Update = (DateTime)v.last_updated;
                    Reward.SubTitle = v.sub_title;
                    Reward.LastAction = v.last_action;

                    Reward.RewardCriteria = new RewardCriteria() { CriteriaId = v.RewardCriteria.id, Name = v.RewardCriteria.name };
                    Reward.RewardType = new RewardType() { RewardTypeId = (int)v.reward_type_id, RewardTypeName = v.RewardType.name, Delivery = (bool)v.RewardType.delivery, MoneyTransfer = (bool)v.RewardType.money_transfer, Mobile = (bool)v.RewardType.mobile, Receiver = (bool)v.RewardType.receiver };

                    List<int> subimgidReward = new List<int>();
                    List<string> subimgnameReward = new List<string>();
                    List<string> subimgurlReward = new List<string>();
                    List<string> subimgurllinkReward = new List<string>();
                    var entitySubImgReward = from d in context.RewardImages
                                               from e in context.Images
                                               where d.image_id == e.id && d.reward_id == v.id
                                               select new { d.image_id, e.url };
                    foreach (var v2 in entitySubImgReward)
                    {
                        subimgidReward.Add(v2.image_id);
                        subimgnameReward.Add(v2.url);
                        subimgurlReward.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                    }
                    Reward.SubImageId = subimgidReward;
                    Reward.SubImageName = subimgnameReward;
                    Reward.SubImageUrl = subimgurlReward;
                    Reward.SubImageUrlLink = subimgurllinkReward;

                    Reward.PointRequirement = (int)v.point_requirement;

                    Reward.OneTimeFlag = v.flag_one_time ?? false;

                    Reward.NumberOfStock = v.number_of_stock ?? 0;

                    RewardList.Add(Reward);
                }
                response = Response<List<Reward>>.Create(RewardList);
            }

            return response;
        }

        public Response<Reward> GetRewardById(int RewardId)
        {
            Response<Reward> response = null;
            Reward Reward = new Reward();
            using (var context = new TheAdzEntities())
            {
                var entityReward = from d in context.Rewards
                                     where d.id == RewardId
                                     select d;
                var v = entityReward.First();
                if (v != null)
                {
                    Reward.RewardId = v.id;
                    Reward.Name = v.name;
                    Reward.SponsorName = v.sponsor_name;
                    Reward.Description = v.description;
                    Reward.Create = (DateTime)v.last_created;
                    Reward.Update = (DateTime)v.last_updated;
                    Reward.SubTitle = v.sub_title;
                    Reward.LastAction = v.last_action;

                    Reward.RewardCriteria = new RewardCriteria() { CriteriaId = v.RewardCriteria.id, Name = v.RewardCriteria.name };
                    Reward.RewardType = new RewardType() { RewardTypeId = (int)v.reward_type_id, RewardTypeName = v.RewardType.name, Delivery = (bool)v.RewardType.delivery, MoneyTransfer = (bool)v.RewardType.money_transfer, Mobile = (bool)v.RewardType.mobile };

                    List<int> subimgidReward = new List<int>();
                    List<string> subimgnameReward = new List<string>();
                    List<string> subimgurlReward = new List<string>();
                    List<string> subimgurllinkReward = new List<string>();
                    var entitySubImgReward = from d in context.RewardImages
                                               from e in context.Images
                                               where d.image_id == e.id && d.reward_id == v.id
                                               select new { d.image_id, e.url };
                    foreach (var v2 in entitySubImgReward)
                    {
                        subimgidReward.Add(v2.image_id);
                        subimgnameReward.Add(v2.url);
                        subimgurlReward.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                    }
                    Reward.SubImageId = subimgidReward;
                    Reward.SubImageName = subimgnameReward;
                    Reward.SubImageUrl = subimgurlReward;
                    Reward.SubImageUrlLink = subimgurllinkReward;

                    Reward.PointRequirement = (int)v.point_requirement;

                    Reward.OneTimeFlag = v.flag_one_time ?? false;

                    Reward.NumberOfStock = v.number_of_stock ?? 0;
                }
                response = Response<Reward>.Create(Reward);
            }

            return response;
        }

        public Response<int> CreateEditReward(Reward Reward)
        {
            Response<int> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Reward.RewardId != 0)
                {
                    var entityReward = from d in context.Rewards
                                         where d.id == Reward.RewardId
                                         select d;
                    if (entityReward.Count() > 0)
                    {
                        var entityReward2 = from d in context.Rewards
                                            where d.sponsor_name.ToLower() == Reward.SponsorName.ToLower()
                                            && d.id != Reward.RewardId
                                            select d;
                        if (entityReward2.Count() > 0)
                        {
                            throw new CustomException(CustomErrorType.RewardAlreadyAssign);
                        }
                        else
                        {
                            entityReward.First().name = Reward.Name;
                            entityReward.First().sponsor_name = Reward.SponsorName;
                            entityReward.First().sub_title = Reward.SubTitle;
                            entityReward.First().description = Reward.Description;
                            entityReward.First().criteria_id = Reward.RewardCriteria.CriteriaId;
                            entityReward.First().last_updated = DateTime.UtcNow;
                            entityReward.First().last_action = "3";

                            entityReward.First().reward_type_id = Reward.RewardType.RewardTypeId;
                            entityReward.First().point_requirement = Reward.PointRequirement;

                            entityReward.First().flag_one_time = Reward.OneTimeFlag;
                            entityReward.First().number_of_stock = Reward.NumberOfStock;

                            context.SaveChanges();

                            #region image
                            if (Reward.SubImageId != null)
                            {
                                try
                                {
                                    var entityImage = from d in context.RewardImages
                                                      where d.reward_id == Reward.RewardId
                                                      select d;
                                    foreach (var v in entityImage)
                                    {
                                        context.RewardImages.Remove(v);
                                    }
                                    context.SaveChanges();
                                    int i = 0;
                                    foreach (var v in Reward.SubImageId)
                                    {
                                        RewardImage mmimage = new RewardImage();
                                        mmimage.reward_id = Reward.RewardId;
                                        mmimage.image_id = v;
                                        context.RewardImages.Add(mmimage);
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
                                var entityImage = from d in context.RewardImages
                                                  where d.reward_id == Reward.RewardId
                                                  select d;
                                foreach (var v in entityImage)
                                {
                                    context.RewardImages.Remove(v);
                                }
                                context.SaveChanges();
                            }
                            #endregion

                            response = Response<int>.Create(Reward.RewardId);
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.RewardNotFound);
                    }
                }
                else
                {
                    var entityReward = from d in context.Rewards
                                       where d.sponsor_name.ToLower() == Reward.SponsorName.ToLower()
                                       select d;
                    if (entityReward.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.RewardAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.Reward mmentity = new Adz.DAL.EF.Reward();
                        mmentity.name = Reward.Name;
                        mmentity.sponsor_name = Reward.SponsorName;
                        mmentity.sub_title = Reward.SubTitle;
                        mmentity.description = Reward.Description;
                        mmentity.criteria_id = Reward.RewardCriteria.CriteriaId;
                        mmentity.last_created = DateTime.UtcNow;
                        mmentity.last_updated = DateTime.UtcNow;
                        mmentity.last_action = "1";

                        mmentity.reward_type_id = Reward.RewardType.RewardTypeId;
                        mmentity.point_requirement = Reward.PointRequirement;

                        mmentity.flag_one_time = Reward.OneTimeFlag;
                        mmentity.number_of_stock = Reward.NumberOfStock;

                        context.Rewards.Add(mmentity);
                        context.SaveChanges();
                        int RewardId = mmentity.id;

                        #region image
                        if (Reward.SubImageId != null)
                        {
                            try
                            {
                                int i = 0;
                                foreach (var v in Reward.SubImageId)
                                {
                                    RewardImage mmimage = new RewardImage();
                                    mmimage.reward_id = RewardId;
                                    mmimage.image_id = v;
                                    context.RewardImages.Add(mmimage);
                                    context.SaveChanges();
                                    i++;
                                }
                            }
                            catch
                            {

                            }
                        }
                        #endregion

                        response = Response<int>.Create(RewardId);
                    }
                }
            }
            return response;
        }

        public Response<bool> DeleteReward(int RewardId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityReward = from d in context.Rewards
                                     where d.id == RewardId
                                     select d;
                if (entityReward.Count() > 0)
                {
                    entityReward.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.RewardFailedDelete);
                }
            }

            return response;
        }

        public Response<bool> DuplicateReward(int RewardId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                Reward item = GetRewardById(RewardId).Result;
                item.RewardId = 0;
                item.Name = item.Name + " - Copy";
                var result = CreateEditReward(item).Result != 0 ? true : false;
                response = Response<bool>.Create(result);
            }

            return response;
        }
        public Response<List<RewardCriteria>> GetRewardCriteriaList()
        {
            Response<List<RewardCriteria>> response = null;
            List<RewardCriteria> RewardCriteriaList = new List<RewardCriteria>();
            using (var context = new TheAdzEntities())
            {
                var entityRewardCriteria = from d in context.RewardCriterias
                                           select d;
                foreach (var v in entityRewardCriteria)
                {
                    RewardCriteria RewardCriteria = new RewardCriteria();
                    RewardCriteria.CriteriaId = v.id;
                    RewardCriteria.Name = v.name;

                    RewardCriteriaList.Add(RewardCriteria);
                }
                response = Response<List<RewardCriteria>>.Create(RewardCriteriaList);
            }

            return response;
        }
        public Response<List<RewardType>> GetRewardTypeList()
        {
            Response<List<RewardType>> response = null;
            List<RewardType> RewardTypeList = new List<RewardType>();
            using (var context = new TheAdzEntities())
            {
                var entityRewardType = from d in context.RewardTypes
                                       orderby d.name
                                       select d;
                foreach (var v in entityRewardType)
                {
                    RewardType RewardType = new RewardType();
                    RewardType.RewardTypeId = v.id;
                    RewardType.RewardTypeName = v.name;
                    RewardType.Delivery = (bool)v.delivery;
                    RewardType.MoneyTransfer = (bool)v.money_transfer;
                    RewardType.Mobile = (bool)v.mobile;
                    RewardType.Receiver = (bool)v.receiver;
                    RewardType.RewardTypeSubName = v.sub_name;

                    RewardTypeList.Add(RewardType);
                }
                response = Response<List<RewardType>>.Create(RewardTypeList);
            }

            return response;
        }

        public Response<List<MobileOperator>> GetMobileOperatorList()
        {
            Response<List<MobileOperator>> response = null;
            List<MobileOperator> MobileOperatorList = new List<MobileOperator>();
            using (var context = new TheAdzEntities())
            {
                var entityMobileOperator = from d in context.MobileOperators
                                           select d;
                foreach (var v in entityMobileOperator)
                {
                    MobileOperatorList.Add(new MobileOperator() { Id = v.id, Name = v.name });
                }
                response = Response<List<MobileOperator>>.Create(MobileOperatorList);
            }

            return response;
        }

        public Response<int> CreateEditRedemption(Redemption Redemption)
        {
            Response<int> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityUser = from d in context.Users
                                 where d.email.ToLower() == Redemption.UserEmail.ToLower()
                                 select d;
                if (entityUser.Count() > 0)
                {

                    if (Redemption.RedemptionId != 0)
                    {
                        var entityRedemption = from d in context.Redemptions
                                               where d.id == Redemption.RedemptionId
                                               select d;
                        if (entityRedemption.Count() > 0)
                        {
                            entityRedemption.First().name = Redemption.Name;

                            var entityRewardTypes = from d in context.RewardTypes
                                                    where d.id == entityRedemption.FirstOrDefault().reward_type_id
                                                    select d;

                            //
                            if (Redemption.MobileOperatorId != null && Redemption.MobileOperatorId != 0 /*&& Redemption.MobileAccNum != null && Redemption.MobileAccNum != ""*/)
                            {
                                //entityRedemption.First().mobile_operator_id = Redemption.MobileOperatorId;
                                entityRedemption.First().mobile_acc_num = Redemption.MobileAccNum;
                            }
                            else if (Redemption.MobileOperator != null && Redemption.MobileOperator.Id != null && Redemption.MobileOperator.Id != 0)
                            {
                                //entityRedemption.First().mobile_operator_id = Redemption.MobileOperator.Id;
                                entityRedemption.First().mobile_acc_num = Redemption.MobileAccNum;
                            }
                            else
                                throw new CustomException(CustomErrorType.RedemptionNotEnough);

                            /*if (entityRewardTypes.First().delivery == true)
                            {
                                if (Redemption.AddressLine1 != null && Redemption.AddressLine1 != "" && Redemption.AddressLine2 != null && Redemption.AddressLine2 != "" && Redemption.City != null && Redemption.City != "" && Redemption.State != null && Redemption.State != "" && Redemption.Country != null && Redemption.Country != "" && Redemption.PostCode != null && Redemption.PostCode != "")
                                {
                                    entityRedemption.First().address_line1 = Redemption.AddressLine1;
                                    entityRedemption.First().address_line2 = Redemption.AddressLine2;
                                    entityRedemption.First().city = Redemption.City;
                                    entityRedemption.First().state = Redemption.State;
                                    entityRedemption.First().country = Redemption.Country;
                                    entityRedemption.First().postcode = Redemption.PostCode;
                                }
                                else
                                    throw new CustomException(CustomErrorType.RedemptionNotEnough);
                            }
                            if (entityRewardTypes.First().money_transfer == true)
                            {
                                if (Redemption.BankName != null && Redemption.BankName != "" && Redemption.BankAccountNum != null && Redemption.BankAccountNum != "" && Redemption.BankAccountName != null && Redemption.BankAccountName != "")
                                {
                                    entityRedemption.First().bank_name = Redemption.BankName;
                                    entityRedemption.First().bank_account_num = Redemption.BankAccountNum;
                                    entityRedemption.First().bank_account_name = Redemption.BankAccountName;
                                }
                                else
                                    throw new CustomException(CustomErrorType.RedemptionNotEnough);
                            }
                            if (entityRewardTypes.First().mobile == true)
                            {
                                if (Redemption.MobileOperatorId != null && Redemption.MobileOperatorId != 0 && Redemption.MobileAccNum != null && Redemption.MobileAccNum != "")
                                {
                                    entityRedemption.First().mobile_operator_id = Redemption.MobileOperatorId;
                                    entityRedemption.First().mobile_acc_num = Redemption.MobileAccNum;
                                }
                                else
                                    throw new CustomException(CustomErrorType.RedemptionNotEnough);
                            }*/

                            //entityRedemption.First().redemption_status_id = Redemption.RedemptionStatus.Id;
                            context.SaveChanges();

                            response = Response<int>.Create(Redemption.RedemptionId);
                        }
                        else
                        {
                            throw new CustomException(CustomErrorType.RedemptionNotFound);
                        }
                    }
                    else
                    {
                        var entityReward = from d in context.Rewards
                                           where d.id == Redemption.RewardId
                                           select d;

                        if (entityReward.First().last_action == "5")
                        {
                            throw new CustomException(CustomErrorType.RewardNotFound);
                        }

                        //check one time flag
                        if (entityReward.First().flag_one_time == true)
                        {
                            var entityRedemptionHistory = from d in context.Redemptions
                                                          where d.reward_id == Redemption.RewardId
                                                          && d.user_id == entityUser.FirstOrDefault().id
                                                          select d;

                            if (entityRedemptionHistory.Count() > 0)
                            {
                                throw new CustomException(CustomErrorType.RedemptionAlreadyAssign);
                            }
                        }

                        //check number of stock
                        if (entityReward.First().number_of_stock != null && entityReward.First().number_of_stock != 0)
                        {
                            var entityRedemptionHistory = from d in context.Redemptions
                                                          where d.reward_id == Redemption.RewardId
                                                          select d;

                            if (entityRedemptionHistory.Count() >= entityReward.First().number_of_stock)
                            {
                                throw new CustomException(CustomErrorType.RedemptionOutOfStock);
                            }
                        }

                        try
                        {
                            if (entityUser.First().point_balance < entityReward.First().point_requirement)
                            {
                                throw new CustomException(CustomErrorType.UserServiceInsufficientPointBalance);
                            }
                        }
                        catch (Exception)
                        {
                            throw new CustomException(CustomErrorType.UserServiceInsufficientPointBalance);
                        }

                        Adz.DAL.EF.Redemption mmentity = new Adz.DAL.EF.Redemption();

                        mmentity.user_id = entityUser.First().id;
                        mmentity.reward_id = Redemption.RewardId;
                        mmentity.name = Redemption.Name;

                        mmentity.mobile_operator_id = null;

                        //
                        if (Redemption.MobileOperatorId != null && Redemption.MobileOperatorId != 0 /*&& Redemption.MobileAccNum != null && Redemption.MobileAccNum != ""*/)
                        {
                            mmentity.mobile_operator_id = Redemption.MobileOperatorId;
                            mmentity.mobile_acc_num = Redemption.MobileAccNum;
                        }
                        else
                            throw new CustomException(CustomErrorType.RedemptionNotEnough);

                        /*if (entityReward.First().RewardType.delivery == true)
                        {
                            if (Redemption.AddressLine1 != null && Redemption.AddressLine1 != "" && Redemption.AddressLine2 != null && Redemption.AddressLine2 != "" && Redemption.City != null && Redemption.City != "" && Redemption.State != null && Redemption.State != "" && Redemption.Country != null && Redemption.Country != "" && Redemption.PostCode != null && Redemption.PostCode != "")
                            {
                                mmentity.address_line1 = Redemption.AddressLine1;
                                mmentity.address_line2 = Redemption.AddressLine2;
                                mmentity.city = Redemption.City;
                                mmentity.state = Redemption.State;
                                mmentity.country = Redemption.Country;
                                mmentity.postcode = Redemption.PostCode;
                            }
                            else
                                throw new CustomException(CustomErrorType.RedemptionNotEnough);
                        }
                        if (entityReward.First().RewardType.money_transfer == true)
                        {
                            if (Redemption.BankName != null && Redemption.BankName != "" && Redemption.BankAccountNum != null && Redemption.BankAccountNum != "" && Redemption.BankAccountName != null && Redemption.BankAccountName != "")
                            {
                                mmentity.bank_name = Redemption.BankName;
                                mmentity.bank_account_num = Redemption.BankAccountNum;
                                mmentity.bank_account_name = Redemption.BankAccountName;
                            }
                            else
                                throw new CustomException(CustomErrorType.RedemptionNotEnough);
                        }
                        if (entityReward.First().RewardType.mobile == true)
                        {
                            if (Redemption.MobileOperatorId != null && Redemption.MobileOperatorId != 0 && Redemption.MobileAccNum != null && Redemption.MobileAccNum != "" )
                            {
                                mmentity.mobile_operator_id = Redemption.MobileOperatorId;
                                mmentity.mobile_acc_num = Redemption.MobileAccNum;
                            }
                            else
                                throw new CustomException(CustomErrorType.RedemptionNotEnough);
                        }*/

                        mmentity.reward_name = entityReward.First().sponsor_name; //name;
                        mmentity.reward_point_requirement = entityReward.First().point_requirement;
                        mmentity.reward_type_id = entityReward.First().RewardType.id;
                        mmentity.redemption_date = DateTime.UtcNow;
                        mmentity.redemption_status_id = (int)RedemptionStatusCode.Submitted;

                        context.Redemptions.Add(mmentity);
                        context.SaveChanges();
                        int RedemptionId = mmentity.id;
                        
                        //deduct user point after created Redemption
                        entityUser.First().point_balance -= entityReward.First().point_requirement;
                        context.SaveChanges();                        
                        UserTrxService.CreateTransaction(new UserTrx()
                        {
                            AccountFrom = "Adz",
                            AccountTo = entityUser.First().id.ToString(),
                            Balance = entityUser.First().point_balance.Value,
                            CreatedBy = 0,
                            DebitAmount = entityReward.First().point_requirement.Value,
                            Description = "Redemption for " + mmentity.reward_name,
                            TransactionDate = DateTime.UtcNow,
                            TransactionMonth = DateTime.UtcNow.Month,
                            TransactionYear = DateTime.UtcNow.Year,
                            UserId = entityUser.First().id,
                        });

                        response = Response<int>.Create(RedemptionId);
                    }
                }
            }
            return response;
        }

        public Response<List<Redemption>> GetRedemptionListByUser(string Email)
        {
            Response<List<Redemption>> response = null;
            List<Redemption> RedemptionList = new List<Redemption>();
            using (var context = new TheAdzEntities())
            {
                var entityUser = from d in context.Users
                                 where d.email.ToLower() == Email.ToLower()
                                 select d.id;

                var entityRedemption = from d in context.Redemptions
                                       where d.user_id == entityUser.FirstOrDefault()
                                       select d;
                foreach (var v in entityRedemption)
                {
                    var entityReview = from d in context.Reviews
                                       where d.redemption_id == v.id && d.last_action != "5"
                                       select d.redemption_id;

                    var Reviewed = false;
                    if (entityReview.Count() > 0)
                    {
                        Reviewed = true;
                    }

                    var entityReward = from d in context.Rewards
                                       where d.id == v.reward_id
                                       select d;
                    var entityRewardType = from d in context.RewardTypes
                                           where d.id == v.reward_type_id
                                           select d;

                    List<string> subimgurlReward = new List<string>();
                    var entitySubImgReward = from d in context.RewardImages
                                             from e in context.Images
                                             where d.image_id == e.id && d.reward_id == v.reward_id
                                             select new { d.image_id, e.url };
                    foreach (var v2 in entitySubImgReward)
                    {
                        subimgurlReward.Add(ConfigurationManager.AppSettings["uploadpath"] + v2.url);
                    }
                    Reward R = new Reward()
                    {
                        RewardId = (int)v.reward_id,
                        Name = v.reward_name,
                        PointRequirement = (int)v.reward_point_requirement,
                        RewardType = new RewardType()
                        {
                            RewardTypeId = (int)v.reward_type_id,
                            RewardTypeName = entityRewardType.First().name
                        },
                        Description = entityReward.First().description,
                        SubImageUrl = subimgurlReward
                    };
                                        
                    MobileOperator MO = new MobileOperator();
                    if (v.mobile_operator_id != null && v.mobile_operator_id != 0)
                    {
                        MO.Id = (int)v.mobile_operator_id;
                        MO.Name = v.MobileOperator.name;
                    }

                    RedemptionList.Add(new Redemption() { RedemptionId = v.id, Reward = R, Name = v.name, RedemptionDate = (DateTime)v.redemption_date, RedemptionStatus = new RedemptionStatus() { Id = v.RedemptionStatu.id, Name = v.RedemptionStatu.name }, AddressLine1 = v.address_line1, AddressLine2 = v.address_line2, City = v.city, State = v.state, Country = v.country, PostCode = v.postcode, BankName = v.bank_name, BankAccountNum = v.bank_account_num, BankAccountName = v.bank_account_name, MobileAccNum = v.mobile_acc_num, MobileOperator = MO, Reviewed = Reviewed });
                }
                response = Response<List<Redemption>>.Create(RedemptionList);
            }

            return response;
        }

        public Response<Redemption> GetRedemptionById(int RedemptionId)
        {
            Response<Redemption> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityRedemption = from d in context.Redemptions
                                       where d.id == RedemptionId
                                       select d;
                if(entityRedemption.Count() > 0)
                {
                    var v = entityRedemption.First();
                    Reward R = new Reward()
                    {
                        RewardId = (int)v.reward_id,
                        Name = v.reward_name,
                        PointRequirement = (int)v.reward_point_requirement
                    };

                    MobileOperator MO = new MobileOperator();
                    if (v.mobile_operator_id != null && v.mobile_operator_id != 0)
                    {
                        MO.Id = (int)v.mobile_operator_id;
                        MO.Name = v.MobileOperator.name;
                    }

                    Redemption r = new Redemption() { RedemptionId = v.id, Reward = R, Name = v.name, RedemptionDate = (DateTime)v.redemption_date, RedemptionStatus = new RedemptionStatus() { Id = v.RedemptionStatu.id, Name = v.RedemptionStatu.name }, AddressLine1 = v.address_line1, AddressLine2 = v.address_line2, City = v.city, State = v.state, Country = v.country, PostCode = v.postcode, BankName = v.bank_name, BankAccountNum = v.bank_account_num, BankAccountName = v.bank_account_name, MobileAccNum = v.mobile_acc_num, MobileOperator = MO, UserEmail = v.User.email };

                    response = Response<Redemption>.Create(r);
                }
            }

            return response;
        }

        public Response<int> CreateEditReview(Review Review, bool Editable)
        {
            Response<int> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Review.RedemptionId != 0)
                {
                    var entityRedemption = from d in context.Redemptions
                                           where d.id == Review.RedemptionId && d.redemption_status_id == (int)RedemptionStatusCode.Completed
                                           select d;

                    if (entityRedemption.Count() > 0)
                    {
                        var entityReview = from d in context.Reviews
                                           where d.redemption_id == Review.RedemptionId
                                           select d;
                        if (entityReview.Count() > 0)
                        {
                            if (Editable == true)
                            {
                                entityReview.First().message = Review.Message;
                                entityReview.First().rating = Review.Rating;
                                entityReview.First().last_action = "3";
                                context.SaveChanges();

                                response = Response<int>.Create(Review.RedemptionId);
                            }
                            else
                            {
                                throw new CustomException(CustomErrorType.ReviewUneditable);
                            }
                        }
                        else
                        {
                            Adz.DAL.EF.Review mmentity = new Adz.DAL.EF.Review();

                            mmentity.redemption_id = Review.RedemptionId;
                            mmentity.message = Review.Message;
                            mmentity.rating = Review.Rating;
                            mmentity.review_date = DateTime.UtcNow;
                            mmentity.last_action = "1";

                            context.Reviews.Add(mmentity);
                            context.SaveChanges();

                            response = Response<int>.Create(Review.RedemptionId);
                        }
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.RedemptionNotFound);
                    }
                }
            }
            
            return response;
        }
        
        public Response<bool> DeleteReview(int RedemptionId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityReview = from d in context.Reviews
                                   where d.redemption_id == RedemptionId
                                   select d;
                if (entityReview.Count() > 0)
                {
                    entityReview.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.ReviewFailedDelete);
                }
            }

            return response;
        }

        public Response<List<Review>> GetReviewListByRewardId(int RewardId)
        {
            Response<List<Review>> response = null;

            List<Review> result = new List<Review>();

            using (var context = new TheAdzEntities())
            {
                var entityReview = from d in context.Reviews
                                   where d.Redemption.reward_id == RewardId && d.last_action != "5"
                                   && d.Redemption.redemption_status_id == (int)RedemptionStatusCode.Completed
                                   select d;

                foreach (var v in entityReview)
                {
                    result.Add(new Review() { RedemptionId = v.redemption_id, Message = v.message, Rating = (int)v.rating, ReviewDate = (DateTime)v.review_date, ByEmail = v.Redemption.User.email });
                }

                response = Response<List<Review>>.Create(result);
            }

            return response;
        }

        public Response<List<Review>> GetReviewListByUserId(int UserId)
        {
            Response<List<Review>> response = null;

            List<Review> result = new List<Review>();

            using (var context = new TheAdzEntities())
            {
                var entityReview = from d in context.Reviews
                                   where d.Redemption.user_id == UserId && d.last_action != "5"
                                   select d;

                foreach (var v in entityReview)
                {
                    result.Add(new Review() { RedemptionId = v.redemption_id, Message = v.message, Rating = (int)v.rating, ReviewDate = (DateTime)v.review_date, ByEmail = v.Redemption.User.email });
                }

                response = Response<List<Review>>.Create(result);
            }

            return response;
        }

        public Response<Review> GetReviewByRedemptionId(int RedemptionId)
        {
            Response<Review> response = null;

            using (var context = new TheAdzEntities())
            {
                var entityReview = from d in context.Reviews
                                   where d.redemption_id == RedemptionId
                                   && d.last_action != "5"
                                   select d;

                if (entityReview.Count() > 0)
                {
                    var v = entityReview.First();

                    response = Response<Review>.Create(new Review() { RedemptionId = v.redemption_id, Message = v.message, Rating = (int)v.rating, ReviewDate = (DateTime)v.review_date, ByEmail = v.Redemption.User.email });
                }
            }

            return response;
        }
        
        public Response<List<Redemption>> GetRedemptionList()
        {
            Response<List<Redemption>> response = null;
            List<Redemption> RedemptionList = new List<Redemption>();
            using (var context = new TheAdzEntities())
            {
                var entityRedemption = from d in context.Redemptions
                                       select d;
                foreach (var v in entityRedemption)
                {
                    Reward R = new Reward()
                    {
                        RewardId = (int)v.reward_id,
                        Name = v.reward_name,
                        PointRequirement = (int)v.reward_point_requirement
                    };

                    MobileOperator MO = new MobileOperator();
                    if (v.mobile_operator_id != null && v.mobile_operator_id != 0)
                    {
                        MO.Id = (int)v.mobile_operator_id;
                        MO.Name = v.MobileOperator.name;
                    }

                    RedemptionList.Add(new Redemption() { RedemptionId = v.id, UserEmail = v.User.email, Reward = R, Name = v.name, RedemptionDate = (DateTime)v.redemption_date, RedemptionStatus = new RedemptionStatus() { Id = v.RedemptionStatu.id, Name = v.RedemptionStatu.name }, AddressLine1 = v.address_line1, AddressLine2 = v.address_line2, City = v.city, State = v.state, Country = v.country, PostCode = v.postcode, BankName = v.bank_name, BankAccountNum = v.bank_account_num, BankAccountName = v.bank_account_name, MobileAccNum = v.mobile_acc_num, MobileOperator = MO });
                }
                response = Response<List<Redemption>>.Create(RedemptionList);
            }

            return response;
        }

        public Response<bool> UpdateRedemptionStatus(int RedemptionId, int NewStatusId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityRedemption = from d in context.Redemptions
                                       where d.id == RedemptionId
                                       select d;

                if (entityRedemption.Count() > 0)
                {
                    entityRedemption.First().redemption_status_id = NewStatusId;
                    context.SaveChanges();

                    //if NewStatus = Cancelled
                    if (NewStatusId == 4)
                    {
                        //refund user if Cancel Redemption
                        int UserId = (int)entityRedemption.First().user_id;
                        int DeductedPoint = (int)entityRedemption.First().reward_point_requirement;

                        var entityUser = from d in context.Users
                                         where d.id == UserId
                                         select d;

                        entityUser.First().point_balance += DeductedPoint;
                        context.SaveChanges();

                        UserTrxService.CreateTransaction(new UserTrx()
                        {
                            AccountFrom = "Adz",
                            AccountTo = entityUser.First().id.ToString(),
                            Balance = entityUser.First().point_balance.Value,
                            CreatedBy = 0,
                            CreditAmount = DeductedPoint,
                            DebitAmount = 0,
                            Description = "Refund for redemption of " + entityRedemption.First().reward_name,
                            TransactionDate = DateTime.UtcNow,
                            TransactionMonth = DateTime.UtcNow.Month,
                            TransactionYear = DateTime.UtcNow.Year,
                            UserId = entityUser.First().id,
                        });
                    }

                    response = Response<bool>.Create(true);
                }
                else
                {
                    response = Response<bool>.Create(false);
                    throw new CustomException(CustomErrorType.RedemptionNotFound);
                }
            }
            return response;
        }

        public Response<List<RedemptionStatus>> GetRedemptionStatusList()
        {
            Response<List<RedemptionStatus>> response = null;
            List<RedemptionStatus> RedemptionStatusList = new List<RedemptionStatus>();
            using (var context = new TheAdzEntities())
            {
                var entityRedemptionStatus = from d in context.RedemptionStatus
                                            select d;
                foreach (var v in entityRedemptionStatus)
                {
                    RedemptionStatus RedemptionStatus = new RedemptionStatus();
                    RedemptionStatus.Id = v.id;
                    RedemptionStatus.Name = v.name;

                    RedemptionStatusList.Add(RedemptionStatus);
                }
                response = Response<List<RedemptionStatus>>.Create(RedemptionStatusList);
            }

            return response;
        }

    }
}
