using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class TagService : ITagService
    {
        public Response<List<Tag>> GetTagList()
        {
            Response<List<Tag>> response = null;
            List<Tag> TagList = new List<Tag>();
            using (var context = new TheAdzEntities())
            {
                var entityTag = from d in context.Tags
                                select d;
                foreach (var v in entityTag)
                {
                    Tag Tag = new Tag();
                    Tag.TagId = v.id;
                    Tag.Name = v.name;
                    Tag.Create = (DateTime)v.last_created;
                    Tag.Update = (DateTime)v.last_updated;
                    Tag.LastAction = v.last_action;

                    TagList.Add(Tag);
                }
                response = Response<List<Tag>>.Create(TagList);
            }

            return response;
        }

        public Response<List<Tag>> GetSelectedTagList(int CampaignId, bool Primary)
        {
            Response<List<Tag>> response = null;
            List<Tag> TagList = new List<Tag>();
            using (var context = new TheAdzEntities())
            {
                if (CampaignId != null && Primary != null)
                {
                    var entityTag = from d in context.Tags
                                    where d.last_action != "5"
                                    select d;

                    
                    if(Primary == true){
                        var entityCampaignTag = from d in context.CampaignPrimaryTags
                                                where d.campaign_id == CampaignId && d.last_action != "5"
                                                select d.tag_id;

                        var entityTagSelected = entityTag.Where(x => entityCampaignTag.Contains(x.id));
                        foreach (var v in entityTagSelected)
                        {
                            Tag Tag = new Tag();
                            Tag.TagId = v.id;
                            Tag.Name = v.name;
                            Tag.Create = (DateTime)v.last_created;
                            Tag.Update = (DateTime)v.last_updated;
                            Tag.LastAction = v.last_action;
                            Tag.Selected = true;

                            TagList.Add(Tag);
                        }

                        entityTag = entityTag.Where(x => !entityCampaignTag.Contains(x.id));
                    }
                    else if (Primary == false)
                    {
                        var entityCampaignTag = from d in context.CampaignSecondaryTags
                                                where d.campaign_id == CampaignId && d.last_action != "5"
                                                select d.tag_id;

                        var entityTagSelected = entityTag.Where(x => entityCampaignTag.Contains(x.id));
                        foreach (var v in entityTagSelected)
                        {
                            Tag Tag = new Tag();
                            Tag.TagId = v.id;
                            Tag.Name = v.name;
                            Tag.Create = (DateTime)v.last_created;
                            Tag.Update = (DateTime)v.last_updated;
                            Tag.LastAction = v.last_action;
                            Tag.Selected = true;

                            TagList.Add(Tag);
                        }

                        entityTag = entityTag.Where(x => !entityCampaignTag.Contains(x.id));
                    }

                    foreach (var v in entityTag)
                    {
                        Tag Tag = new Tag();
                        Tag.TagId = v.id;
                        Tag.Name = v.name;
                        Tag.Create = (DateTime)v.last_created;
                        Tag.Update = (DateTime)v.last_updated;
                        Tag.LastAction = v.last_action;
                        Tag.Selected = false;

                        TagList.Add(Tag);
                    }
                }
                response = Response<List<Tag>>.Create(TagList);
            }

            return response;
        }
        
        public Response<bool> AddCampaignTag(int CampaignId, int TagId, bool Primary)
        {
            Response<bool> response = null;

            using (var context = new TheAdzEntities())
            {
                if (Primary == true)
                {
                    var entityCampaignPrimaryTag = from d in context.CampaignPrimaryTags
                                                   where d.campaign_id == CampaignId && d.tag_id == TagId
                                                   select d;
                    if (entityCampaignPrimaryTag.Count() > 0)
                    {
                        entityCampaignPrimaryTag.First().last_action = "3";
                        context.SaveChanges();
                    }
                    else
                    {
                        CampaignPrimaryTag newentity = new CampaignPrimaryTag();
                        newentity.campaign_id = CampaignId;
                        newentity.tag_id = TagId;
                        newentity.last_action = "1";

                        context.CampaignPrimaryTags.Add(newentity);
                        context.SaveChanges();
                    }
                }
                else
                {
                    var entityCampaignSecondaryTag = from d in context.CampaignSecondaryTags
                                                     where d.campaign_id == CampaignId && d.tag_id == TagId
                                                     select d;
                    if (entityCampaignSecondaryTag.Count() > 0)
                    {
                        entityCampaignSecondaryTag.First().last_action = "3";
                        context.SaveChanges();
                    }
                    else
                    {
                        CampaignSecondaryTag newentity = new CampaignSecondaryTag();
                        newentity.campaign_id = CampaignId;
                        newentity.tag_id = TagId;
                        newentity.last_action = "1";

                        context.CampaignSecondaryTags.Add(newentity);
                        context.SaveChanges();
                    }
                }
            }

            response = Response<bool>.Create(true);
            return response;
        }

        public Response<bool> DeleteCampaignTag(int CampaignId, int TagId, bool Primary)
        {
            Response<bool> response = null;

            using (var context = new TheAdzEntities())
            {
                if (Primary == true)
                {
                    var entityCampaignPrimaryTag = from d in context.CampaignPrimaryTags
                                                   where d.campaign_id == CampaignId && d.tag_id == TagId
                                                   select d;
                    if (entityCampaignPrimaryTag.Count() > 0)
                    {
                        entityCampaignPrimaryTag.First().last_action = "5";
                        context.SaveChanges();
                        response = Response<bool>.Create(true);
                    }
                    else
                    {
                        response = Response<bool>.Create(false);
                    }
                }
                else
                {
                    var entityCampaignSecondaryTag = from d in context.CampaignSecondaryTags
                                                     where d.campaign_id == CampaignId && d.tag_id == TagId
                                                     select d;
                    if (entityCampaignSecondaryTag.Count() > 0)
                    {
                        entityCampaignSecondaryTag.First().last_action = "5";
                        context.SaveChanges();
                        response = Response<bool>.Create(true);
                    }
                    else
                    {
                        response = Response<bool>.Create(true);
                    }
                }
            }

            return response;
        }

        public Response<Tag> GetTagById(int TagId)
        {
            Response<Tag> response = null;
            Tag Tag = new Tag();
            using (var context = new TheAdzEntities())
            {
                var entityTag = from d in context.Tags
                                where d.id == TagId
                                select d;
                var v = entityTag.First();
                if (v != null)
                {
                    Tag.TagId = v.id;
                    Tag.Name = v.name;
                    Tag.Create = (DateTime)v.last_created;
                    Tag.Update = (DateTime)v.last_updated;
                    Tag.LastAction = v.last_action;
                }
                response = Response<Tag>.Create(Tag);
            }

            return response;
        }

        public Response<int> CreateEditTag(Tag Tag)
        {
            Response<int> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Tag.TagId != 0)
                {
                    var entityTag = from d in context.Tags
                                    where d.id == Tag.TagId
                                    select d;
                    if (entityTag.Count() > 0)
                    {
                        entityTag.First().name = Tag.Name;
                        entityTag.First().last_updated = DateTime.UtcNow;
                        entityTag.First().last_action = "3";
                        context.SaveChanges();

                        response = Response<int>.Create(Tag.TagId);
                        
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.TagNotFound);
                    }
                }
                else
                {
                    var entityTag = from d in context.Tags
                                         where d.name.ToLower() == Tag.Name.ToLower()
                                         select d;
                    if (entityTag.Count() > 0)
                    {
                        throw new CustomException(CustomErrorType.TagAlreadyAssign);
                    }
                    else
                    {
                        Adz.DAL.EF.Tag mmentity = new Adz.DAL.EF.Tag();
                        mmentity.name = Tag.Name;
                        mmentity.last_created = DateTime.UtcNow;
                        mmentity.last_updated = DateTime.UtcNow;
                        mmentity.last_action = "1";
                        context.Tags.Add(mmentity);
                        context.SaveChanges();
                        int TagId = mmentity.id;

                        response = Response<int>.Create(TagId);
                    }
                }
            }
            return response;
        }

        public Response<bool> DeleteTag(int TagId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityTag = from d in context.Tags
                                     where d.id == TagId
                                     select d;
                if (entityTag.Count() > 0)
                {
                    entityTag.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.TagFailedDelete);
                }
            }

            return response;
        }

        public Response<bool> DuplicateTag(int TagId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                Tag item = GetTagById(TagId).Result;
                item.TagId = 0;
                item.Name = item.Name + " - Copy";
                var result = CreateEditTag(item).Result != 0 ? true : false;
                response = Response<bool>.Create(result);
            }

            return response;
        }

    }
}