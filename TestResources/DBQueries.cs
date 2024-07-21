using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SanlamAutomation.TestResources
{
    public class DBQueries
    {

        public static string OTPquery = "select Top 1 * from [dbo].[Otp] order by createddate desc";
       
      
        // verifyPhoneNumberfromDB

        public static string verifyPhoneNumberfromDB(string phonenumber)
        {
            string qQuery = @"select * from [dbo].[User] where IsActive=1 and PhoneNumber = '"+ phonenumber + "'";
            return qQuery;
        }

        public static string getTotalIncome_Balance_RepaymentQyery(string IdNumber, string DBId, string totaltype)
        {
            string qQuery = null;

            if (totaltype.ToLower().Equals("current balance"))
            {
                qQuery = @"SELECT CreditHistoryId, SUM(Current_Balance) as total
FROM [credithistory].[AccountInformation] WHERE CreditHistoryId = '" + DBId + "'GROUP BY CreditHistoryId";
            }
            else if (totaltype.ToLower().Equals("monthly repayment"))
            {
                qQuery = @"SELECT CreditHistoryId, SUM(Installment_Amount) as total
FROM [credithistory].[AccountInformation] WHERE CreditHistoryId = '" + DBId + "'GROUP BY CreditHistoryId";

            }
            else
            {
                qQuery = @"select * from [dbo].[user] where idnumber = '" + IdNumber + "'";
            }
            return qQuery;
        }

        public static string updateRepaymentfromDB(string expectedRepaymentValue, string DBId)
        {
            string qQuery = @"update [credithistory].[credithealthinfo] set TotalMonthlyInstalments = '" + expectedRepaymentValue + "'where credithistoryid ='" + DBId + "'";
            return qQuery;
        }

        public static string getSumOfAccFromDB(string DBId, string loantype, string sum)
        {
            string qQuery = @"SELECT AccountTypeSrn, SUM(" + sum + ") as total FROM [credithistory].[AccountInformation] WHERE Acc_Status = '" + loantype + "' and CreditHistoryId = '" + DBId + "'GROUP BY AccountTypeSrn";
            return qQuery;
        }

        public static string updateScorefromDB(string CreditScore, string DBId)
        {
            string qQuery = @"update [credithistory].[scoreInformation] set Score = '" + CreditScore + "'where credithistoryid ='" + DBId + "'";
            return qQuery;
        }
        public static string deleteCreditHistory(string IdNumber)
        {
            string qQuery = @"delete  from [dbo].[credithistory] where idnumber ='" + IdNumber + "'";
            return qQuery;
        }

        public static string getCreditHistoryIDfromDB(string IdNumber)
        {
            string qQuery = @"select Top 1 * from [dbo].[credithistory] where idnumber = '" + IdNumber + "'order by createddate desc";
            return qQuery;
        }

        public static string getScorefromDB(string DBId)
        {
            string qQuery = @"SELECT Top 1 *
  FROM [credithistory].[scoreInformation]
  where CreditHistoryId ='" + DBId + "' order by createddate desc";
            return qQuery;
        }

        public static string questionQuery(string IdNumber)
        {
            string qQuery = @"DECLARE @json NVARCHAR(MAX) SET @json = (SELECT [SecurityQuestionJson] from [SecurityQuestionAnswer] where IdNumber='" + IdNumber + "')\r\nSELECT * FROM OPENJSON ( @json ) WITH (Question varchar(2000) '$.Question', CurrectAnswer0 varchar(20) '$.SecurityAnswerIds[0]', CurrectAnswer1 varchar(20) '$.SecurityAnswerIds[1]', CurrectAnswer2 varchar(20) '$.SecurityAnswerIds[2]', CurrectAnswer3 varchar(20) '$.SecurityAnswerIds[3]')";
            return qQuery;

        }
        public static string basicverificationfromDBQuery(string IdNumber)
        {
            string qQuery = @"select * from [dbo].[ExternalCommLog] where ExternalCommLogtypeid = 3 and idnumber = '" + IdNumber + "'";
            return qQuery;
        }

        public static string afterRegPostValidationQuery(string IdNumber)
        {
            string qQuery = @"select * from [dbo].[user] where idnumber = '" + IdNumber + "'";
            return qQuery;
        }

        public static string checkRequestParam(string IdNumber)
        {
            string query = "select Top 1 * from[dbo].[ExternalCommLog] where Endpoint = 'http://10.0.2.198:8049/POPIA/Lead' and IdNumber = '" + IdNumber + "' order by RequestTime desc";
            return query;
        }
    }
}
