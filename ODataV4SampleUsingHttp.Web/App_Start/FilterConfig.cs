using System.Web;
using System.Web.Mvc;

namespace ODataV4SampleUsingHttp.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
