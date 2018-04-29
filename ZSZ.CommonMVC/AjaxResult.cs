using System;

namespace ZSZ.CommonMVC
{
    public class AjaxResult
    {
        public string Status { get;set;}
        public string ErrorMsg { get; set; }
        public object Data { get; set; }
    }
}
