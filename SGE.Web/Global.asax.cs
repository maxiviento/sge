using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SGE.Web.Infrastucture;

namespace SGE.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

//            routes.MapRoute(
//    "Default2", // Route name
//    "SGE.Web/{controller}/{action}/{id}", // URL with parameters
//    new { controller = "Login", action = "Login", id = UrlParameter.Optional } // Parameter defaults
//);

     
            //HttpContext.Current.RewritePath(Request.ApplicationPath, false);
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Login", id = UrlParameter.Optional } // Parameter defaults
            );

//var virtualPathData = RouteTable.Routes.GetVirtualPath(this.viewContext.RequestContext, pageLinkValueDictionary);


        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Authentication.AuthenticateRequest(Context);
        }


        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Response.Clear();

        //    HttpException httpException = exception as HttpException;

        //    if (httpException != null)
        //    {
        //        string action;

        //        switch (httpException.GetHttpCode())
        //        {
        //            case 404:
        //                // page not found
        //                action = "HttpError404";
        //                break;
        //            case 500:
        //                // server error
        //                action = "HttpError500";
        //                break;
        //            default:
        //                action = "General";
        //                break;
        //        }

        //        // clear error on server
        //        Server.ClearError();

        //        Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, exception.Message));
        //    }
        //}

//        protected void Session_OnStart(object sender, EventArgs e)
//    {
//    //'Ocurre cuando se crea una nueva session, es decir cuando un usuario ingresa
//        Context.Response.Write("<b>III. Session iniciada</b><br />");
//    }

//protected void Session_OnEnd(object sender, EventArgs e)
//{//'Ocurre cuando se abandona una session o se supera el tiempo de espera de una session
//    //Context.Response.Write("<b>IV. Session finalizada</b><br />");
//}


    }
}