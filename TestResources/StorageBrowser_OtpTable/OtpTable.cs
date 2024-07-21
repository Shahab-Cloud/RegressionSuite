using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMAutomation.TestResources.StorageBrowser_OtpTable
{
    public class OtpTable : TableEntity
    {
        public int AttemptCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredTime { get; set; }
        public int HitCount { get; set; }
        public bool IsVerified { get; set; }
        public String ObjectId { get; set; }
        public String OTPObjectiveId { get; set; }
        public String OTPTypeId { get; set; }
        public int Pin { get; set; }
        public String SessionCloseReason { get; set; }
        public String Provider { get; set; }

        public String VerifiedDate { get; set; }

        public Guid SessionId { get; set; }
    }
}
