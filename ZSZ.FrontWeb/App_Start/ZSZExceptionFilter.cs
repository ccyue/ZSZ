using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.FrontWeb.App_Start
{
    public class ZSZExceptionFilter : IExceptionFilter
    {
        ILog log = LogManager.GetLogger(nameof(ZSZExceptionFilter));
        public void OnException(ExceptionContext filterContext)
        {
            log.Error("Exception", filterContext.Exception);
        }
    }
}