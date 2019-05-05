using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class UserTrxService : IUserTrxService
    {
        public List<UserTrx> GetTransactionByUser(int UserId)
        {
            List<UserTrx> result = new List<UserTrx>();
            using (var context = new TheAdzEntities())
            {
                var UserTRXs = from d in context.UserTrxes
                               where d.user_id == UserId
                               orderby d.created_date descending
                               select d;

                foreach(var v in UserTRXs)
                {
                    UserTrx UserTrx = new UserTrx()
                    {
                        AccountFrom = v.account_from,
                        AccountTo = v.account_to,
                        Balance = v.balance ?? 0,
                        CreatedBy = v.created_by ?? 0,
                        CreatedDate = v.created_date ?? DateTime.MinValue,
                        CreditAmount = v.credit_amount ?? 0,
                        DebitAmount = v.debit_amount ?? 0,
                        Description = v.desc,
                        TransactionDate = v.transaction_date ?? DateTime.MinValue,
                        TransactionMonth = v.transaction_month ?? 0,
                        TransactionYear = v.transaction_year ?? 0,
                        UserId = v.user_id ?? 0,
                        UserTrxId = v.id,
                    };

                    result.Add(UserTrx);
                }
            }

            return result;
        }

        public Response<int> CreateTransaction(UserTrx Trx)
        {
            using (var context = new TheAdzEntities())
            {
                Adz.DAL.EF.UserTrx mmentity = new Adz.DAL.EF.UserTrx();
                mmentity.account_from = Trx.AccountFrom;
                mmentity.account_to = Trx.AccountTo;
                mmentity.balance = Trx.Balance;
                mmentity.created_by = Trx.CreatedBy;
                mmentity.created_date = Trx.CreatedDate != null ? Trx.CreatedDate.Value : DateTime.UtcNow;
                mmentity.credit_amount = Trx.CreditAmount;
                mmentity.debit_amount = Trx.DebitAmount;
                mmentity.desc = Trx.Description;
                mmentity.transaction_date = Trx.TransactionDate;
                mmentity.transaction_month = Trx.TransactionMonth;
                mmentity.transaction_year = Trx.TransactionYear;
                mmentity.user_id = Trx.UserId;

                context.UserTrxes.Add(mmentity);
                context.SaveChanges();

                int UserTrxId = mmentity.id;

                return new Response<int>
                {
                    Result = UserTrxId
                };
            }
        }
    }
}
