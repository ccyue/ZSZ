using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

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
    }
}
