using System.Web;
using System.Web.Mvc;

namespace FESA.SCM.Notifications
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
