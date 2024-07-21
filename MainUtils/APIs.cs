using AventStack.ExtentReports;
using JM.MainUtils;
using RestSharp;


namespace JMAutomation.MainUtils
{
    public class APIs
    {
        public static void AutoReg(string idnumber, string fname, string surname, string pnumber, string websource) {

            RestRequest request = new RestRequest("https://app-func-jm-preprod-001.azurewebsites.net/api/CreateUserFromLead", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("x-functions-key", "lOiZv4LE9emxP4NxzcTY2NXVPQ6k82/aF7Fu3GZKLLTbGb6imvyXvw==");
            // Set the request body as a JSON string
            request.AddJsonBody(new {
                IdNumber= idnumber,
                FirstName= fname,
                SurName= surname,
                PhoneNumber= "0"+ pnumber,
                Email= "Test12"+GenericUtils.getRandomString(4)+"@test.com",
                AcceptTerms= true,
                Source= "AutoRegister",
                websource= websource,
                LeadId="12345"
            });

            
           
            // Create a new RestClient instance and execute the request
            RestClient client = new RestClient();
            RestResponse response = client.Execute(request);
            String content = response.Content;
            Report.childlog.Log(Status.Info, content);
            bool noErrorinResponse = content.Contains("Registration Successful");
            Validate.assertEquals(true,noErrorinResponse, "Response is not succesful", true);
                      
            Report.childlog.Log(Status.Info,"User is succesfully Auto Registered with content - "+ content);

        }

        public static void AutoReg_prod(string idnumber, string fname, string surname, string pnumber, string websource)
        {

            RestRequest request = new RestRequest("https://app-func-jm-prod-001.azurewebsites.net/api/CreateUserFromLead", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("x-functions-key", "7vMD4M7VklTN9zNmgLbVh5G6tk4BDMdPe1aA9qcSEH19KgpgyAT2aA==");
            // Set the request body as a JSON string
            request.AddJsonBody(new
            {
                IdNumber = idnumber,
                FirstName = fname,
                SurName = surname,
                PhoneNumber = "0" + pnumber,
                Email = "Test12" + GenericUtils.getRandomString(4) + "@test.com",
                AcceptTerms = true,
                Source = "AutoRegister",
                websource = websource,
                LeadId = "12345"
            });



            // Create a new RestClient instance and execute the request
            RestClient client = new RestClient();
            RestResponse response = client.Execute(request);
            String content = response.Content;
            Report.childlog.Log(Status.Info, content);
            bool noErrorinResponse = content.Contains("Registration Successful");
            Validate.assertEquals(true, noErrorinResponse, "Response is not succesful", true);

            Report.childlog.Log(Status.Info, "User is succesfully Auto Registered with content - " + content);

        }
        public static void AutoRegistration(string idnumber, string fname, string surname, string pnumber, string websource) {

            
            string url = "https://app-func-jm-preprod-001.azurewebsites.net/api/CreateUserFromLead";
           
                        

        }
    }
}
