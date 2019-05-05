using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.API.ServiceModel.Types
{
    public class UserTrx
    {
        public string Description { get; set; }
        public int DebitAmount { get; set; }
        public int CreditAmount { get; set; }
        public int Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionMonth { get; set; }
        public int TransactionYear { get; set; }
        public string AccountFrom { get; set; }
    }
}
