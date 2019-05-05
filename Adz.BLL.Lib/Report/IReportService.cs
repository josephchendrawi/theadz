using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib.Report
{
    public interface IReportService
    {
        void SendEmailDailyReport(string SendToEmail);
        string GetDailyReportTargetEmail();
        void CreateEditDailyReportTargetEmail(string Email);
    }

    public class DailyReport
    {
        public DailyReport()
        {
            RedemptionRequestList = new List<RedemptionRequest>();
            NewSignUpUserList = new List<NewSignUpUser>();
            UserPointAllocationList = new List<UserPointAllocation>();
        }
        public List<RedemptionRequest> RedemptionRequestList { get; set; }
        public List<NewSignUpUser> NewSignUpUserList { get; set; }
        public List<UserPointAllocation> UserPointAllocationList { get; set; }

        public string DB_Unallocated_Space { get; set; }
    }

    public class RedemptionRequest
    {
        public string User { get; set; }
        public string Reward { get; set; }
        public string Status { get; set; }
    }

    public class NewSignUpUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string RefferedBy { get; set; }
    }

    public class UserPointAllocation
    {
        public string Name { get; set; }
        public int Debit { get; set; }
        public int Credit { get; set; }
        public int Balance { get; set; }
        public string From { get; set; }
        public string TransactionDate { get; set; }
    }
}
