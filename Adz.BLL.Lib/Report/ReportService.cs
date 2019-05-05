using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib.Report
{
    public class ReportService : IReportService
    {
        public string GetDailyReportTargetEmail()
        {
            using (var context = new TheAdzEntities())
            {
                var Email = from d in context.Emails
                            select d;

                if (Email.Count() > 0)
                {
                    return Email.FirstOrDefault().email1;
                }
                else
                {
                    return "";
                }
            }
        }

        public void CreateEditDailyReportTargetEmail(string Email)
        {
            using (var context = new TheAdzEntities())
            {
                var Emailett = from d in context.Emails
                               select d;

                if (Emailett.Count() > 0)
                {
                    Emailett.First().email1 = Email;
                    context.SaveChanges();
                }
                else
                {
                    Adz.DAL.EF.Email ett = new DAL.EF.Email();
                    ett.email1 = Email;
                    context.Emails.Add(ett);
                    context.SaveChanges();
                }
            }
        }

        public void SendEmailDailyReport(string SendToEmail)
        {
            string EmailSubject = "Daily Report - " + DateTime.Now.ToString("dd MMM yyyy");
            string EmailBody = this.GenerateDailyReportHtml(this.GetDailyReport());

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "mail.theadz.com";
            smtpClient.Port = 26;
            smtpClient.EnableSsl = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("report@theadz.com", "theadz12345");
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            email.From = new MailAddress("report@theadz.com", "Adz Report");
            email.To.Add(SendToEmail);
            email.Subject = EmailSubject;
            email.IsBodyHtml = true;
            email.Body += EmailBody;
            try
            {
                smtpClient.Send(email);
            }
            catch
            {
                smtpClient.Port = 587;
                smtpClient.Send(email);
            }
        }

        public string GenerateDailyReportHtml(DailyReport DailyReport)
        {
            string html = "<html><body style=\"font-family: Tahoma, Geneva, sans-serif;\">" +
            "<h2>" + DateTime.Now.ToString("dd MMM yyyy") + "</h2>" +
            "<h3>DB Unallocated Space : " + DailyReport.DB_Unallocated_Space + "</h3>" +
            "<br/>" +
            "<h3>Redemption Request</h3>" +
            "<table style=\"border-collapse: collapse; border: 1px solid #e0e0e0;\">" +
            "	<thead style=\"background-color: #4CAF50; color: white;\">" +
            "		<tr>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">User</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Reward Name</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Status</td>" +
            "		</tr>" +
            "	</thead>" +
            "	<tbody>";

            foreach (var v in DailyReport.RedemptionRequestList)
            {
                html += "" +
                "	<tr>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.User + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Reward + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Status + "</td>" +
                "	</tr>";
            }

            html += "" +
            "	</tbody>" +
            "</table>";

            html += "" +
            "<br/>" +
            "<h3>New Sign Up User</h3>" +
            "<table style=\"border-collapse: collapse; border: 1px solid #e0e0e0;\">" +
            "	<thead style=\"background-color: #4CAF50; color: white;\">" +
            "		<tr>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Name</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Email</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Reffered By</td>" +
            "		</tr>" +
            "	</thead>" +
            "	<tbody>";

            foreach (var v in DailyReport.NewSignUpUserList)
            {
                html += "" +
                "	<tr>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Name + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Email + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.RefferedBy + "</td>" +
                "	</tr>";
            }

            html += "" +
            "	</tbody>" +
            "</table>";

            html += "" +
            "<br/>" +
            "<h3>User Point Allocation</h3>" +
            "<table style=\"border-collapse: collapse; border: 1px solid #e0e0e0;\">" +
            "	<thead style=\"background-color: #4CAF50; color: white;\">" +
            "		<tr>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Name</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Debit</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Credit</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Balance</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">From</td>" +
            "			<td style=\"border-collapse: collapse; font-weight: bold; padding: 8px 12px; border: 1px solid #e0e0e0;\">Transaction Date</td>" +
            "		</tr>" +
            "	</thead>" +
            "	<tbody>";

            foreach (var v in DailyReport.UserPointAllocationList)
            {
                html += "" +
                "	<tr>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Name + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Debit + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Credit + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.Balance + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.From + "</td>" +
                "		<td style=\"border-collapse: collapse; font-size: 14px; padding: 6px 10px; border: 1px solid #e0e0e0;\">" + v.TransactionDate + "</td>" +
                "	</tr>";
            }

            html += "" +
            "	</tbody>" +
            "</table>";

            html += "</body></html>";

            return html;
        }

        public DailyReport GetDailyReport()
        {
            DailyReport DailyReport = new DailyReport();

            var ToDate = DateTime.UtcNow;
            var FromDate = ToDate.AddDays(-1);

            using (var context = new TheAdzEntities())
            {
                var RedemptionRequests = from d in context.Redemptions
                                         where d.redemption_date < ToDate && d.redemption_date >= FromDate
                                         && (d.redemption_status_id == (int)RedemptionStatusCode.Submitted || d.redemption_status_id == (int)RedemptionStatusCode.Completed)
                                         select d;

                foreach (var v in RedemptionRequests)
                {
                    DailyReport.RedemptionRequestList.Add(new RedemptionRequest() { Reward = v.reward_name, User = v.name, Status = v.RedemptionStatu.name });
                }


                var NewSignUpUsers = from d in context.Users
                                    where d.last_created < ToDate && d.last_created >= FromDate
                                    select d;

                foreach (var v in NewSignUpUsers)
                {
                    DailyReport.NewSignUpUserList.Add(new NewSignUpUser() { Email = v.email, Name = v.first_name + " " + v.last_name, RefferedBy = v.referred_by });
                }


                var UserPointAllocation = from d in context.UserTrxes
                                          join e in context.Users on d.user_id equals e.id
                                          where d.created_date < ToDate && d.created_date >= FromDate
                                          select new
                                          {
                                              UserTrx = d,
                                              User = e,
                                          };

                foreach (var v in UserPointAllocation)
                {
                    DailyReport.UserPointAllocationList.Add(new UserPointAllocation() { Balance = v.UserTrx.balance ?? 0, Credit = v.UserTrx.credit_amount ?? 0, Debit = v.UserTrx.debit_amount ?? 0, From = v.UserTrx.account_from, Name = v.User.first_name + " " + v.User.last_name, TransactionDate = (v.UserTrx.transaction_date ?? DateTime.MinValue).AddHours(8).ToString("dd MMM yyyy") });
                }

                //populate DB_Unallocated_Space
                var sqlConn = context.Database.Connection as SqlConnection;
                var cmd = new SqlCommand("sp_spaceused")
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                    Connection = sqlConn as SqlConnection
                };
                var adp = new SqlDataAdapter(cmd);
                var dataset = new DataSet();
                sqlConn.Open();
                adp.Fill(dataset);
                sqlConn.Close();

                DailyReport.DB_Unallocated_Space = dataset.Tables[0].Rows[0][2].ToString();
            }

            return DailyReport;
        }
    }
}
