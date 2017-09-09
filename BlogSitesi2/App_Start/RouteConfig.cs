using System.Web.Mvc;
using System.Web.Routing;

namespace BlogSitesi2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Blog",                                           // Route name
                "Icerik/{id}/{title}",                           // URL with parameters
                new { controller = "Home", action = "Icerik" }  // Parameter defaults
                );

            //routes.MapRoute(
            //    "UserProfileRoute",
            //    "Profile/{id}/{UserName}",
            //    new { controller = "Profile", action = "UserProfile" }
            //    );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}