using System.Web.Http;
using WebActivatorEx;
using MillionsEyesWebApi;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace MillionsEyesWebApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
              .EnableSwagger(c => c.SingleApiVersion("v1", "MillionEyes"))
              .EnableSwaggerUi();
        }
    }
}
