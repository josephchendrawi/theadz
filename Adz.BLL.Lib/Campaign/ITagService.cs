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
    public interface ITagService
    {
        [OperationContract]
        Response<List<Tag>> GetTagList();
        [OperationContract]
        Response<List<Tag>> GetSelectedTagList(int CampaignId, bool Primary);
        [OperationContract]
        Response<Tag> GetTagById(int TagId);
        [OperationContract]
        Response<int> CreateEditTag(Tag Tag);
        [OperationContract]
        Response<bool> DeleteTag(int TagId);
        [OperationContract]
        Response<bool> DuplicateTag(int TagId);
        [OperationContract]
        Response<bool> AddCampaignTag(int CampaignId, int TagId, bool Primary);
        [OperationContract]
        Response<bool> DeleteCampaignTag(int CampaignId, int TagId, bool Primary);
    }

    [DataContract]
    public class Tag
    {
        [DataMember]
        public int TagId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        [DataMember]
        public DateTime Update { get; set; }
        [DataMember]
        public string LastAction { get; set; }
        [DataMember]
        public bool Selected { get; set; }
    }
}