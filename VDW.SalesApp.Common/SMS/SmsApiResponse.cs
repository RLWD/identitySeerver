
using System.Collections.Generic;

namespace VDW.SalesApp.Common.SMS
{
    public class SmsApiResponse
    {
        public List<SmsReport> SendStatusSet { get; set; }
        public string RequestId { get; set; }
    }
    public class SmsReport
    {
        public string SerialNo { get; set; }
        public string PhoneNumber { get; set; }
        public int Fee { get; set; }
        public string SessionContext { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string IsoCode { get; set; }
    }
}
