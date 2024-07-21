using JMAutomation.MainUtils;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace JMAutomation
{
    public class DBCreditCoachOTP
    {
        static SqlConnection con;
        static SqlDataReader dr;
        static SqlCommand cmd;
        static string pin;
       
        public static string getOTP(String OTPquery) {

            string DBCCOconnectionString = getDBCCOconnectionString();
           

            try
            {
                con = new SqlConnection(DBCCOconnectionString);
                con.Open();
                cmd = new SqlCommand(OTPquery, con);

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string isVerified = dr["IsVerified"].ToString();
                    Console.WriteLine(isVerified);
                     pin = dr["Pin"].ToString();
                    Console.WriteLine(pin);
                    return pin;
                }
                con.Close();
              

            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return pin;

        }

        private static string getDBCCOconnectionString()
        {
            string path = GenericUtils.getDataPath("TestResources");
            JObject json = GenericUtils.GetJson(path + "\\Database.json");
            string DBCCconnectionString = json[properties.environment]["dbotp_connectionstring"].ToString();
            return DBCCconnectionString;
        }

    }
}
