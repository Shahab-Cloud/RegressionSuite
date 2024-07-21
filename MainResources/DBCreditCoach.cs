using AventStack.ExtentReports;
using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation.MainUtils;
using Newtonsoft.Json.Linq;
using SanlamAutomation.TestResources;
using System.Data.SqlClient;

namespace JMAutomation
{
    public class DBCreditCoach
    {



        static SqlConnection con;
        static SqlDataReader dr;
        static SqlCommand cmd;


        public static void getQuestionClick(String query)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            string DBCCconnectionString = getDBCCconnectionString();


            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> questionBook = new Dictionary<string, string>();
            while (dr.Read())
            {
                questionBook.Add(new KeyValuePair<string, string>(dr["Question"].ToString(), dr["CurrectAnswer0"].ToString()));
            }
            con.Close();
            int i = 0;
            while (RegistrationPage.isSecurityQuestionDisplayed())
            {
                selectAnswerOfSecurity(questionBook, RegistrationPage);
                BaseStep.wait.genericWait(5000);
                i++;
                if (i == 5)
                {
                    break;
                }
            }
            Validate.takestepFullScreenShot("5 out of 5 Questions are Selected", Status.Info);
            BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.aftersecurityquestionsubmitbtn, 15);
            BaseStep.click(RegistrationPage.AfterSecurityQuestionSubmitBtn);


        }

        private static void selectAnswerOfSecurity(IDictionary<string, string> questionBook, RegistrationPage RegistrationPage)
        {

            String question = RegistrationPage.SecurityQuestionTest();
            if (questionBook.ContainsKey(question))
            {
                string answerID = questionBook[question];
                if (answerID == "1")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("0"));
                }
                else if (answerID == "2")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("1"));
                }
                else if (answerID == "3")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("2"));
                }
                else if (answerID == "4")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("3"));
                }
                else if (answerID == "5")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("4"));
                }
                else if (answerID == "6")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("5"));
                }
                else if (answerID == "7")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("6"));
                }
                else if (answerID == "8")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("7"));
                }
                else if (answerID == "9")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("8"));
                }
                else if (answerID == "10")
                {
                    BaseStep.click(RegistrationPage.OptionSelect("9"));
                }

            }
        }
        public static void verifyDBBasicVerification(string idnumber)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.basicverificationfromDBQuery(idnumber);

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> reqQuestionBook = new Dictionary<string, string>();
            IDictionary<string, string> resQuestionBook = new Dictionary<string, string>();
            while (dr.Read())
            {
                reqQuestionBook.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["RequestParam"].ToString()));
                resQuestionBook.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["ResponseData"].ToString()));
            }

            Report.childlog.Log(Status.Info, "Request present in DB for ID " + idnumber + " is " + reqQuestionBook[idnumber]);

            Report.childlog.Log(Status.Info, "ResponseData present in DB for ID " + idnumber + " is " + resQuestionBook[idnumber]);

            con.Close();



        }

        public static void getPostValidationAfterReg(String Idnumber)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.afterRegPostValidationQuery(Idnumber);

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();

            //Is Active

            IDictionary<string, string> isActiveQuestionBook = new Dictionary<string, string>();
            IDictionary<string, string> securityQuestionConfirmed = new Dictionary<string, string>();
            IDictionary<string, string> basicInfoVerifyConfirmed = new Dictionary<string, string>();
            IDictionary<string, string> phoneNumberConfirmed = new Dictionary<string, string>();

            while (dr.Read())
            {
                // Is Active Status
                isActiveQuestionBook.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["IsActive"].ToString()));
                Report.childlog.Log(Status.Info, "IsActive Status present in DB for ID " + Idnumber + " is " + isActiveQuestionBook[Idnumber]);
                Assert.IsTrue(isActiveQuestionBook[Idnumber].Equals("True"));

                // Security Question Confirmed
                securityQuestionConfirmed.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["SecurityQuestionConfirmed"].ToString()));
                Report.childlog.Log(Status.Info, "SecurityQuestionConfirmed Status present in DB for ID " + Idnumber + " is " + securityQuestionConfirmed[Idnumber]);

                // Basic 
                basicInfoVerifyConfirmed.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["BasicInfoVerifyConfirmed"].ToString()));
                Report.childlog.Log(Status.Info, "BasicInfoVerifyConfirmed Status present in DB for ID " + Idnumber + " is " + basicInfoVerifyConfirmed[Idnumber]);
                if (securityQuestionConfirmed[Idnumber].Equals("True"))
                {
                    Assert.IsTrue(basicInfoVerifyConfirmed[Idnumber].Equals("False"));
                }
                else
                {
                    Assert.IsTrue(basicInfoVerifyConfirmed[Idnumber].Equals("True"));
                }

                // phone number active              

                phoneNumberConfirmed.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["PhoneNumberConfirmed"].ToString()));
                Report.childlog.Log(Status.Info, "PhoneNumberConfirmed Status present in DB for ID " + Idnumber + " is " + phoneNumberConfirmed[Idnumber]);
                Assert.IsTrue(phoneNumberConfirmed[Idnumber].Equals("True"));
            }
            con.Close();

        }

        public static string getCreditHistoryId(String Idnumber)
        {

            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.getCreditHistoryIDfromDB(Idnumber);

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> CreditHistoryIDBook = new Dictionary<string, string>();

            while (dr.Read())
            {
                CreditHistoryIDBook.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["Id"].ToString()));

            }

            con.Close();

            return CreditHistoryIDBook[Idnumber];
        }

        public static void updateRepaymentFromDB(string Idnumber, string expectedRepaymentValue)
        {
            string DBId = getCreditHistoryId(Idnumber);

            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.updateRepaymentfromDB(expectedRepaymentValue, DBId);

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();

            Report.childlog.Log(Status.Info, "Updated Repayment value in DB for ID " + Idnumber + " is " + expectedRepaymentValue);

            con.Close();


        }

        public static int sumOfAccountFromDB(string Idnumber, string loanType, string acc_status, string sumtype)
        {
            string sumstring = null;
            int sum = 0;
            string DBId = getCreditHistoryId(Idnumber);
            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.getSumOfAccFromDB(DBId, acc_status, sumtype);
            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            IDictionary<string, string> sumofOBCLForAccStatusOpen = new Dictionary<string, string>();
            while (dr.Read())
            {
                sumofOBCLForAccStatusOpen.Add(new KeyValuePair<string, string>(dr["AccountTypeSrn"].ToString(), dr["total"].ToString()));

            }
            con.Close();
            foreach (KeyValuePair<string, string> entry in sumofOBCLForAccStatusOpen)
            {


                if (entry.Key.ToLower() == loanType.ToLower())
                {
                    Report.childlog.Log(Status.Info, "Total amount of " + sumtype + " for " + entry.Key + " are " + entry.Value);
                    string loansum = GenericUtils.splitString(entry.Value, ".", 0);
                    int openingBalanceCC = int.Parse(loansum);
                    sum = sum + openingBalanceCC;

                }
            }
            return sum;
        }

        public static void updateScorefromDB(string IdNumber, string CreditScore)
        {
            string DBId = getCreditHistoryId(IdNumber);
            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.updateScorefromDB(CreditScore, DBId);
            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            con.Close();
            Report.childlog.Log(Status.Info, "Score is updated with value " + CreditScore);
        }
        public static string getCreditScoreFromDB(string IdNumber)
        {


            string DBId = getCreditHistoryId(IdNumber);
            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.getScorefromDB(DBId);
            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> ScoreBook = new Dictionary<string, string>();

            while (dr.Read())
            {
                ScoreBook.Add(new KeyValuePair<string, string>(dr["CreditHistoryId"].ToString(), dr["Score"].ToString()));

            }

            con.Close();

            return ScoreBook[DBId];

        }

        public static void deleteCreditHistory(string IdNumber)
        {

            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.deleteCreditHistory(IdNumber);
            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            con.Close();
            Report.childlog.Log(Status.Info, "Credit History deleted for ID " + IdNumber);
        }
        public static string getTotalIncome(string IdNumber)
        {

            string DBId = getCreditHistoryId(IdNumber);
            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.getTotalIncome_Balance_RepaymentQyery(IdNumber, DBId, "Income");

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> TotalIncomeBook = new Dictionary<string, string>();

            while (dr.Read())
            {
                TotalIncomeBook.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["GrossIncome"].ToString()));

            }

            con.Close();


            return TotalIncomeBook[IdNumber];
        }

        public static string getTotalMonthlyRepayment(string IdNumber)
        {

            string DBId = getCreditHistoryId(IdNumber);
            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.getTotalIncome_Balance_RepaymentQyery(IdNumber, DBId, "monthly repayment");

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> TotalRepaymentBook = new Dictionary<string, string>();

            while (dr.Read())
            {
                TotalRepaymentBook.Add(new KeyValuePair<string, string>(dr["CreditHistoryId"].ToString(), dr["total"].ToString()));

            }

            con.Close();


            return TotalRepaymentBook[DBId];
        }

        public static string getTotalCurrentBalance(string IdNumber)
        {

            string DBId = getCreditHistoryId(IdNumber);
            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.getTotalIncome_Balance_RepaymentQyery(IdNumber, DBId, "current balance");

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> TotalCurrentBalanceBook = new Dictionary<string, string>();

            while (dr.Read())
            {
                TotalCurrentBalanceBook.Add(new KeyValuePair<string, string>(dr["CreditHistoryId"].ToString(), dr["total"].ToString()));

            }
            con.Close();
            return TotalCurrentBalanceBook[DBId];
        }
        public static bool verifiedEmail_SMS_Tel(string IdNumber, string requestType)
        {
            string DBCCconnectionString = getDBCCconnectionString();
            string query = DBQueries.checkRequestParam(IdNumber);

            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            IDictionary<string, string> RequestBook = new Dictionary<string, string>();

            while (dr.Read())
            {
                RequestBook.Add(new KeyValuePair<string, string>(dr["IdNumber"].ToString(), dr["RequestParam"].ToString()));

            }
            con.Close();
            
            string jsonString = RequestBook[IdNumber];
            JObject json = JObject.Parse(jsonString);
            bool consent = false ;
            if (requestType.ToLower().Equals ("email")) {
                // Extract the value of the "ConsentEmail" field
                consent = (bool)json["PopiaRequestModel"]["ConsentEmail"];
            }
            else if  (requestType.ToLower().Equals("sms"))
                {
                // Extract the value of the "ConsentSMS" field
                consent = (bool)json["PopiaRequestModel"]["ConsentSMS"];
                }
            else if (requestType.ToLower().Equals("tel"))
            {
                // Extract the value of the "ConsentSMS" field
                consent = (bool)json["PopiaRequestModel"]["ConsentTel"];
            }

            return consent;
        }

        public static void verifyPhoneNumberfromDB(String query)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            string DBCCconnectionString = getDBCCconnectionString();


            con = new SqlConnection(DBCCconnectionString);
            con.Open();
            cmd = new SqlCommand(query, con);

            dr = cmd.ExecuteReader();
            string isVerified = null;
            while (dr.Read())
            {
                 isVerified = dr["IsActive"].ToString();              
                
            }
            con.Close();
            Validate.assertEquals("true", isVerified.ToLower(), "Phone number is not registered", true);
            Report.childlog.Log(Status.Info, "Phone number is registered with application DB");


        }

        private static string getDBCCconnectionString()
        {
            string path = GenericUtils.getDataPath("TestResources");
            JObject json = GenericUtils.GetJson(path + "\\Database.json");
            string DBCCconnectionString = json[properties.environment]["db_connectionstring"].ToString();
            return DBCCconnectionString;
        }

    }
}
