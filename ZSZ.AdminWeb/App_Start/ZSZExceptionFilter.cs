using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace ZSZ.AdminWeb.App_Start
{
    public class ZSZExceptionFilter:IExceptionFilter
    {
        ILog log = LogManager.GetLogger(typeof(ZSZExceptionFilter));
        public void OnException(ExceptionContext filterContext)
        {
            log.Error("Exception", filterContext.Exception);
        }
    }
}