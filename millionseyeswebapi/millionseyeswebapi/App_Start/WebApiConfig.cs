using System.Web.Http;
using System.Web.Mvc;

namespace MillionsEyesWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    "ServiceBusMetricRoute",
            //    "api/{controller}/{action}/{hoursCount}/{interval}"
            //);

            //config.Routes.MapHttpRoute(
            //    "ServiceBusTimeIntervaleMetricRoute",
            //    "api/{controller}/{action}/timespan/{startTime}/{finishTime}/{interval}",
            //    new { interval = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    "ServiceBusSingleMetricRoute",
            //    "api/{controller}/{action}/{metricName}/{hour}/",
            //    new { hour = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    "ServiceBusSingleMetricRouteWithInterval",
            //    "api/{controller}/{action}/{metricName}/{hour}/{interval}",
            //    new { hour = RouteParameter.Optional, interval = RouteParameter.Optional }
            //);

            config.EnableCors();
        }
    }
}
