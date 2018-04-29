using Autofac;
using Autofac.Integration.Mvc;
using ITestService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1
{
    public class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                //IUserService svc = DependencyResolver.Current.GetService<IUserService>();
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                using (container.BeginLifetimeScope())
                {
                    IUserService us = container.Resolve<IUserService>();
                    us.CheckUserExist("123");
                }                   
          
            }
            catch(Exception ex)
            {
                string ss = ex.Message;
            }
        }

    }
}