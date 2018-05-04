using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;

namespace MillionsEyesWebApi.Request
{
    public class Request
    {
        public static string ServiceBusNameSpace
        {
            get
            {
                return WebConfigurationManager.AppSettings["ServiceBusNamespace"];
            }
            set
            {
            }
        }

        public static string ResourceGroup
        {
            get
            {
                return WebConfigurationManager.AppSettings["ResourceGroup"];
            }
            set
            {
                ResourceGroup = value;
            }
        }

        public static string Timestamp
        {
            get;
            set;
        }

        public static string Provider
        {
            get
            {
                return WebConfigurationManager.AppSettings["Provider"];
            }
            set
            {
            }
        }

        public static string InsightProvider
        {
            get
            {
                return WebConfigurationManager.AppSettings["InsightProvider"];
            }
            set
            {
            }
        }

        public static string Interval
        {
            get;
            set;
        }

        public static string EntityName
        {
            get;
            set;
        }
        public static string MetricName
        {
            get;
            set;
        }

        private static string ConvertStringArrayToString(List<string> metrics)
        {
            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in metrics)
            {
                builder.Append(value);
                if (value != metrics[metrics.Count - 1])
                {
                    builder.Append(",");
                }
            }
            return builder.ToString();
        }
    }
}