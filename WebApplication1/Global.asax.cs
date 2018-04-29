using Autofac;
using Autofac.Integration.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new JsonNetActionFilter());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly)
                .Where(type => typeof(ActionFilterAttribute).IsAssignableFrom(type) && !type.IsAbstract)
                .PropertiesAutowired();

            Assembly[] assemblies = new Assembly[] { Assembly.Load("TestService") };
            builder.RegisterAssemblyTypes(assemblies).Where(type => !type.IsAbstract).AsImplementedInterfaces().PropertiesAutowired();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            #region Quartz
            //IScheduler sched = new StdSchedulerFactory().GetScheduler();
            //JobDetailImpl jdBossReport = new JobDetailImpl("jdTest", typeof(TestJob));
            //CalendarIntervalScheduleBuilder b = CalendarIntervalScheduleBuilder.Create();
            //b.WithInterval(3, IntervalUnit.Second);
            //IMutableTrigger triggerBossReport = b.Build();
            //triggerBossReport.Key = new TriggerKey("triggerTest");
            //sched.ScheduleJob(jdBossReport, triggerBossReport);
            //sched.Start(); 
            #endregion

        }
    }
}
