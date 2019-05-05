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
    public interface IPromotionService
    {
        Response<List<Promotion>> GetPromotionList();
        Response<Promotion> GetPromotionById(int PromotionId);
        Response<int> CreateEditPromotion(Promotion Promotion);
        Response<bool> DeletePromotion(int PromotionId);
        Response<bool> RunPromotion(int PromotionId);
        Response<bool> StopPromotion(int PromotionId);
    }

    [DataContract]
    public class Promotion
    {
        [DataMember]
        public int PromotionId { get; set; }
        [DataMember]
        public string PromoCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Value { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        public string LastAction { get; set; }

        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public bool OnScheduleFlag { get; set; }
    }

    [DataContract]
    public enum PromotionStatus
    {
        [EnumMember]
        OnGoing = 1,
        [EnumMember]
        Stopped = 2,
    }

}
