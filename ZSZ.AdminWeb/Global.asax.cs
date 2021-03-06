﻿using Autofac;
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
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Jobs;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //log4net
            log4net.Config.XmlConfigurator.Configure();
            //filter
            FilterConfig.RegisterFilters(GlobalFilters.Filters);
            //GlobalFilters.Filters.Add(new ZSZExceptionFilter());
            //GlobalFilters.Filters.Add(new ZSZAuthorizationFilter());
            //builer
            ModelBinders.Binders.Add(typeof(string),new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(int), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(long), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(double), new TrimToDBCModelBinder());
            //AutoFac
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            Assembly[] assembies = new Assembly[] { Assembly.Load("ZSZ.Service") };
            builder.RegisterAssemblyTypes(assembies)
                .Where(type=>!type.IsAbstract 
                    && typeof(IServiceSupport).IsAssignableFrom(type)).AsImplementedInterfaces().PropertiesAutowired();
            //type1.IsAssignableFrom(type2)  type2是否实现type1
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //job
            StartQuartz();
        }
        private void StartQuartz()
        {
            IScheduler sched = new StdSchedulerFactory().GetScheduler();
            JobDetailImpl jdBossReport = new JobDetailImpl("bossJob", typeof(BossReportJob));
            IMutableTrigger triggerBossReport = CronScheduleBuilder.DailyAtHourAndMinute(11, 09).Build();//每天 23:45 执行一次 
            triggerBossReport.Key = new TriggerKey("triggerTest");
            sched.ScheduleJob(jdBossReport, triggerBossReport);

            sched.Start();
        }
    }
}
