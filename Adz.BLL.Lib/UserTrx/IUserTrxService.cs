using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public interface IUserTrxService
    {
        Response<int> CreateTransaction(UserTrx Trx);
        List<UserTrx> GetTransactionByUser(int UserId);
    }

    [DataContract]
    public class UserTrx
    {
        [DataMember]
        public int UserTrxId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int DebitAmount { get; set; }
        [DataMember]
        public int CreditAmount { get; set; }
        [DataMember]
        public int Balance { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        public DateTime TransactionDate { get; set; }
        [DataMember]
        public int TransactionMonth { get; set; }
        [DataMember]
        public int TransactionYear { get; set; }
        [DataMember]
        public string AccountFrom { get; set; }
        [DataMember]
        public string AccountTo { get; set; }
    }
}
