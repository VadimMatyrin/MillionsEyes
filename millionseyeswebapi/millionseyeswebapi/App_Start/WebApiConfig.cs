using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace MillionsEyesWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");
            config.EnableCors(cors);
        }
    }
}
