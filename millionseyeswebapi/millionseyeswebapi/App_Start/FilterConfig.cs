using System.Web;
using System.Web.Mvc;

namespace MillionsEyesWebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(filter: new HandleErrorAttribute());
        }
    }
}
