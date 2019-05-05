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
    public interface IRuleService
    {
        [OperationContract]
        Response<Rule> GetRuleById(int RuleId);
        [OperationContract]
        Response<Rule> CreateEditRule(Rule Rule);
        [OperationContract]
        Response<bool> DeleteRule(int RuleId);
    }

    [DataContract]
    public class Rule
    {
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public int Gender { get; set; }
        [DataMember]
        public int NumberOfView { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public Boolean NoEnd { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public DateTime Create { get; set; }
        [DataMember]
        public DateTime Update { get; set; }
        [DataMember]
        public string LastAction { get; set; }

        [DataMember]
        public Campaign Campaign { get; set; }

        [DataMember]
        public Boolean Success { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public int AgeGroupStart { get; set; }
        public int AgeGroupEnd { get; set; }

        public int ImageId { get; set; }
    }
}
