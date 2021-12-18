using Newtonsoft.Json;
using System;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sms.V20210111;
using TencentCloud.Sms.V20210111.Models;

namespace VDW.SalesApp.Common.SMS
{
    public class SmsProvider
    {
        public AddTemplateApiResponse AddSmsTemplate(string secretId, string secretKey, string templateFormat, string remarks, ulong isInternationalSmsTemplate)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                };
                var clientProfile = new ClientProfile
                {
                    SignMethod = ClientProfile.SIGN_TC3SHA256,
                    HttpProfile = new HttpProfile { Timeout = 30, Endpoint = "sms.tencentcloudapi.com" }
                };
                var smsClient = new SmsClient(cred, "ap-guangzhou", clientProfile);
                var smsTemplate = new AddSmsTemplateRequest { International = isInternationalSmsTemplate, Remark = remarks, SmsType = 0, TemplateContent = templateFormat, TemplateName = "SalesApps Verification Code with Expiry Timeout" };
                AddSmsTemplateResponse resp = smsClient.AddSmsTemplateSync(smsTemplate);
                return JsonConvert.DeserializeObject<AddTemplateApiResponse>(AbstractModel.ToJsonString(resp));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public SmsApiResponse SendSms(string templateId, string sdkAppId, string secretId, string secretKey, string[] smsContent, string phoneNumberwithCountryCode)
        {
            try
            {
                Credential cred = new Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                };
                var clientProfile = new ClientProfile
                {
                    SignMethod = ClientProfile.SIGN_TC3SHA256,
                    HttpProfile = new HttpProfile { Timeout = 30, Endpoint = "sms.tencentcloudapi.com" }
                };
                var smsClient = new SmsClient(cred, "ap-guangzhou", clientProfile);
                var smsReq = new SendSmsRequest { TemplateId = templateId, TemplateParamSet = smsContent, SmsSdkAppId = sdkAppId, SessionContext = "", PhoneNumberSet = new string[] { phoneNumberwithCountryCode } };
                SendSmsResponse resp = smsClient.SendSmsSync(smsReq);
                return JsonConvert.DeserializeObject<SmsApiResponse>(AbstractModel.ToJsonString(resp));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
