using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;

namespace ZSZ.FrontWeb.App_Start
{
    public class BinderConfig
    {
        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(string), new TrimToDBCModelBinder());
            binders.Add(typeof(int), new TrimToDBCModelBinder());
            binders.Add(typeof(double), new TrimToDBCModelBinder());
            binders.Add(typeof(long), new TrimToDBCModelBinder());
        }
    }
}