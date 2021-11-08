using System;
using System.Threading.Tasks;

namespace VDW.SalesApp.Common.Salesforce.Interface
{
    public interface ISalesforceProvider
    {
        Task<SaleforceAuthorization> GetAccessToken(string iss, string aud, string sub, Uri authUrl, string privateKeyFilePath);
    }
}
