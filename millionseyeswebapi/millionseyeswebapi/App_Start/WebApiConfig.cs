using Autofac;
using Autofac.Integration.WebApi;
using MillionsEyesWebApi.Helpers;
using MillionsEyesWebApi.Interfaces;
using MillionsEyesWebApi.Models;
using MillionsEyesWebApi.Repositories;
using MillionsEyesWebApi.Repository;
using System.Web.Http;

namespace MillionsEyesWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            var builder = new ContainerBuilder();
            builder.RegisterInstance<IServiceBusMetricsRepository>(new ServiceBusMetricsRepository());
            builder.RegisterInstance<IHttpClientHelper>(new HttpClientHelper());
            builder.RegisterInstance<IQueuesMetricRepository>(new QueuesMetricRepository());

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ServiceBusMetricRoute",
                routeTemplate: "api/{controller}/{action}/{hoursCount}/{interval}"
            );

            config.Routes.MapHttpRoute(
                name: "ServiceBusTimeIntervaleMetricRoute",
                routeTemplate: "api/{controller}/{action}/timespan/{startTime}/{finishTime}/{interval}",
                defaults: new { interval = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ServiceBusSingleMetricRoute",
                routeTemplate: "api/{controller}/{action}/{metricName}/{hoursCount}/",
                defaults: new { hoursCount = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ServiceBusSingleMetricRouteWithInterval",
                routeTemplate: "api/{controller}/{action}/{metricName}/{hoursCount}/{interval}",
                defaults: new { hoursCount = RouteParameter.Optional, interval = RouteParameter.Optional }
            );

            config.EnableCors();
        }
    }
}
