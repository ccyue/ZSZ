using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.CommonMVC;
using ZSZ.FrontWeb.App_Start;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //log4net
            log4net.Config.XmlConfigurator.Configure();
            //Autofac
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            //register assembly
            var assemblies = new Assembly[] { Assembly.Load("ZSZ.Service") };
            builder.RegisterAssemblyTypes(assemblies).Where(type => !type.IsAbstract 
                       && typeof(IServiceSupport).IsAssignableFrom(type))
                .AsImplementedInterfaces().PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //model builder
            BinderConfig.RegisterBinders(ModelBinders.Binders);
            //Filter
            FilterConfig.RegisterFilters(GlobalFilters.Filters);

        }
    }
}
