using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PemUtils;
using RestSharp;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VDW.SalesApp.Common.Salesforce
{
    public class SalesforceProvider
    {
        public SaleforceAuthorization GetAccessToken(string iss, string aud, string sub, Uri authUrl, string privateKeyFilePath)
        {
            var header = new { alg = "RS256", typ = "JWT" };
            var expr = GetExpiryDate();

            var claimset = new
            {
                iss = iss,
                sub = sub,
                aud = aud,
                exp = expr
            };

            var headerSerialized = JsonConvert.SerializeObject(header);
            var headerBytes = Encoding.UTF8.GetBytes(headerSerialized);
            var claimsetSerialized = JsonConvert.SerializeObject(claimset);
            var claimsetBytes = Encoding.UTF8.GetBytes(claimsetSerialized);

            var encodedHeader = Base64UrlEncoder.Encode(headerBytes);
            var encodedClaimSet = Base64UrlEncoder.Encode(claimsetBytes);
            var data = string.Concat(encodedHeader, ".", encodedClaimSet);
            var signature = SignSHA256RSAAsync(data, privateKeyFilePath);
            var bearerToken = string.Concat(data, ".", signature);

            var client = new RestClient(authUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer");
            request.AddParameter("assertion", bearerToken);

            IRestResponse response = client.Execute(request);
            var tokenResponse = JsonConvert.DeserializeObject<SaleforceAuthorization>(response.Content);
            tokenResponse.StatusCode = response.StatusCode;
            return tokenResponse;
        }

        private string SignSHA256RSAAsync(string itemToSign, string pemfileValue)
        {
            var bytes = Encoding.UTF8.GetBytes(itemToSign);
            var privateKeyBytes = Encoding.UTF8.GetBytes(pemfileValue);
            var keystream = new MemoryStream(privateKeyBytes);
            using (var reader = new PemReader(keystream))
            {
                var rsaParameters = reader.ReadRsaKey();
                byte[] hv = SHA256.Create().ComputeHash(bytes);
                RSACryptoServiceProvider prov = new RSACryptoServiceProvider();
                prov.ImportParameters(rsaParameters);
                RSAPKCS1SignatureFormatter rf = new RSAPKCS1SignatureFormatter(prov);
                rf.SetHashAlgorithm("SHA256");
                byte[] signature = rf.CreateSignature(hv);
                return Base64UrlEncoder.Encode(signature);
            }
        }
        private int GetExpiryDate()
        {
            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var currentUtcTime = DateTime.UtcNow;
            var exp = (int)currentUtcTime.AddMinutes(4).Subtract(utc0).TotalSeconds;
            return exp;
        }
    }
}
