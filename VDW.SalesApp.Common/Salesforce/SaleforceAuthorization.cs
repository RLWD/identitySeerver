using Newtonsoft.Json;
using System.Net;

namespace VDW.SalesApp.Common.Salesforce
{
    public class SaleforceAuthorization
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "instance_url")]
        public string InstanceUrl { get; set; }
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }


    }
}
