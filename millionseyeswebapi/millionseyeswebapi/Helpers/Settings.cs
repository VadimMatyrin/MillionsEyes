using System.Web.Configuration;

namespace MillionsEyesWebApi.Helpers
{
    public static class Settings
    {
        public static string BaseUrl => WebConfigurationManager.AppSettings["BaseUrl"];
        public static string TenantId => WebConfigurationManager.AppSettings["TenantId"];
        public static string ClientId => WebConfigurationManager.AppSettings["ClientId"];
        public static string Secret => WebConfigurationManager.AppSettings["Secret"];
        public static string SubscriptionId => WebConfigurationManager.AppSettings["SubscriptionId"];
        public static string ApiVersion => WebConfigurationManager.AppSettings["ApiVersion"];
        public static string Aggregation => WebConfigurationManager.AppSettings["Aggregation"];
    }
}