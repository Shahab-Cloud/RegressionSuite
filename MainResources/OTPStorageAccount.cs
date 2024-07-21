using AventStack.ExtentReports;
using JM.MainUtils;
using JMAutomation.MainUtils;
using JMAutomation.TestResources.StorageBrowser_OtpTable;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace JMAutomation.MainResources
{
    public class OTPStorageAccount
    {
        private static IEnumerable<OtpTable> sortedEntities;

        public static async Task<IEnumerable<OtpTable>> connectWithStorageAccountAsync(string phonenumber)
        {

            string connectionString = getStorageBrowserconnectionString();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Otp");

            TableQuery<OtpTable> query = new TableQuery<OtpTable>()
      .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, phonenumber));


            Task.Run(async () =>
           {
               var queryResult = await table.ExecuteQuerySegmentedAsync(query, null);
               // Process the query result asynchronously if needed

               sortedEntities = queryResult.Results.OrderByDescending(entity => entity.Timestamp)
               .Take(2);

           }).GetAwaiter().GetResult();

            return sortedEntities;

        }

        private static string getStorageBrowserconnectionString()
        {
            string path = GenericUtils.getDataPath("TestResources");
            JObject json = GenericUtils.GetJson(path + "\\Database.json");
            string DBCCconnectionString = json[properties.environment]["storagebrowser_connectionstring"].ToString();
            return DBCCconnectionString;
        }



        public static async Task<int> FetchthedatafromthetableAsyncofIntType(string partitionKey, int rowNumber, string columnName)
        {

            var sortedEntities = await connectWithStorageAccountAsync(partitionKey);

            int hitCount = 0;
            int attemptcount = 0;
            int pin = 0;

            if (columnName.ToLower() == "hitcount")
            {

                var hitCounts = sortedEntities.Select(entity => entity.HitCount).ToArray();
                int index = rowNumber - 1;
                hitCount = hitCounts[index];
                Console.WriteLine("HitCount of the " + rowNumber + " row: " + hitCount);
                Report.childlog.Log(Status.Info, "HitCount of the " + rowNumber + " otp row: " + hitCount);
                return hitCount;
            }
            else if (columnName.ToLower() == "attemptcount")
            {
                var AttemptCounts = sortedEntities.Select(entity => entity.AttemptCount).ToArray();
                int index = rowNumber - 1;
                attemptcount = AttemptCounts[index];
                Console.WriteLine("AttemptCount of the " + rowNumber + " row: " + attemptcount);
                Report.childlog.Log(Status.Info, "AttemptCount of the " + rowNumber + " otp row: " + attemptcount);
                return attemptcount;
            }
            else if (columnName.ToLower() == "pin")
            {
                var Pins = sortedEntities.Select(entity => entity.Pin).ToArray();
                int index = rowNumber - 1;
                pin = Pins[index];

                Console.WriteLine("Pin of the " + rowNumber + " row: " + pin);
                Report.childlog.Log(Status.Info, "Pin of the " + rowNumber + " otp row: " + pin);
                return pin;
            }

            return hitCount;
        }






        public static async Task<bool> Fetchthedatafromthetableofbooltype(string partitionKey, int rowNumber, string columnName)
        {

            var sortedEntities = await connectWithStorageAccountAsync(partitionKey);

            var IsVerified = false;

            if (columnName.ToLower() == "IsVerified")
            {
                var IsVerifieds = sortedEntities.Select(entity => entity.IsVerified).ToArray();

                // Ensure that the number of hit counts matches the desired count
                if (IsVerifieds.Length == 2)
                {
                    int index = rowNumber - 1;
                    IsVerified = IsVerifieds[index];

                }
                Console.WriteLine("IsVerified of the row is: " + IsVerified);
                Report.childlog.Log(Status.Info, "IsVerified of the " + rowNumber + " otp row: " + IsVerified);
                return IsVerified;

            }

            return IsVerified;
        }

        public static async Task deleteOTPTableAsync(string partitionKey)
        {

            string connectionString = getStorageBrowserconnectionString();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Otp");
            TableQuery<DynamicTableEntity> query = new TableQuery<DynamicTableEntity>()
        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            TableContinuationToken continuationToken = null;

            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResult.ContinuationToken;

                foreach (DynamicTableEntity entity in queryResult.Results)
                {
                    TableOperation deleteOperation = TableOperation.Delete(entity);
                    await table.ExecuteAsync(deleteOperation);
                }

            } while (continuationToken != null);

        }

        public static async Task updateOtpExpiryTimeAsync(string partitionKey, int updateFirstorSecondOtp)
        {

            string connectionString = getStorageBrowserconnectionString();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Otp");

            int expiryTimeInMinutes = 5; // Original expiry time in minutes

            TableQuery<OtpTable> query = new TableQuery<OtpTable>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey))
                .Take(2);

            TableQuerySegment<OtpTable> queryResult = await table.ExecuteQuerySegmentedAsync(query, null);

            List<OtpTable> otpList = queryResult.ToList();

            // Sort the OTP records based on creation time in ascending order
            List<OtpTable> sortedOtpList = otpList.OrderBy(otp => otp.CreatedDate).ToList();


            if (sortedOtpList.Count >= 2)
            {
                if (updateFirstorSecondOtp == 1)
                {
                    // Update the expiry time of the second OTP
                    OtpTable secondOtp = sortedOtpList[1]; // Access the second OTP record
                    secondOtp.ExpiredTime = secondOtp.ExpiredTime.AddMinutes(-expiryTimeInMinutes -5 + 1); // Decrease expiry time by 5 minutes

                    // Update the record in the Azure Table with the modified OTP entity
                    TableOperation updateOperation = TableOperation.Replace(secondOtp);
                    await table.ExecuteAsync(updateOperation);
                }
                else
                {
                    // Update the expiry time of the first OTP
                    OtpTable firstOtp = sortedOtpList[0]; // Access the first OTP record
                    firstOtp.ExpiredTime = firstOtp.ExpiredTime.AddMinutes(-expiryTimeInMinutes + 1); // Decrease expiry time by 5 minutes

                    // Update the record in the Azure Table with the modified OTP entity
                    TableOperation updateOperation = TableOperation.Replace(firstOtp);
                    await table.ExecuteAsync(updateOperation);

                }
            }
        }
    }
}
