using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPartnerApi.Utils;
using MvcPartnerApi.Models;

namespace MyAppUtils
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());           
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
           routes.MapRoute(
               "Default", // 
               "{controller}/{action}"
           );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();             
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //Add Custom xmlValueProvider
            ValueProviderFactories.Factories.Add(new Utils.XmlValueProviderFactory());
            
        }

       
    }
}
