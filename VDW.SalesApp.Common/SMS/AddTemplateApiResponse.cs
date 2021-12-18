
namespace VDW.SalesApp.Common.SMS
{
    public class AddTemplateApiResponse
    {
        public AddTemplateStatus AddTemplateStatus { get; set; }
        public string RequestId { get; set; }
    }
    public class AddTemplateStatus
    {
        public string TemplateId { get; set; }
    }
}
