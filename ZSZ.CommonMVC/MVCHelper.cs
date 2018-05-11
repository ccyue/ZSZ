using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.Mvc;
using System.Linq;

namespace ZSZ.CommonMVC
{
    public class MVCHelper
    {
        public static string GetValidMsg(ModelStateDictionary modelState)//有两个ModelStateDictionary类，别弄混乱了。要使用System.Web.Mvc下的
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in modelState.Keys)
            {
                if (modelState[key].Errors.Count <= 0)
                {
                    continue;
                }
                sb.Append("属性【").Append(key).Append("】错误：");
                foreach (var modelError in modelState[key].Errors)
                {
                    sb.AppendLine(modelError.ErrorMessage);
                }
            }
            return sb.ToString();
        }
        public static string ToQueryString(NameValueCollection nvc)
        {
            StringBuilder strb = new StringBuilder();
            foreach(var key in nvc.AllKeys)
            {
                strb.Append(key).Append("=").Append(nvc[key]).Append("&");
            }
            return strb.ToString().Trim('&');
        }
        public static string UpdateQueryString(NameValueCollection nvc,string name,string value)
        {
            NameValueCollection newNVC = new NameValueCollection(nvc);
            if(newNVC.AllKeys.Contains(name))
            {
                newNVC[name] = value;
            }
            else
            {
                newNVC.Add(name, value);
            }
            return ToQueryString(newNVC);
        }
        public static string RemoveQueryString(NameValueCollection nvc, string name)
        {
            NameValueCollection newNVC = new NameValueCollection(nvc);
            if (newNVC.AllKeys.Contains(name))
            {
                newNVC.Remove(name);
            }

            return ToQueryString(newNVC);
        }
    }
}
