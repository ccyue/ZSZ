using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZSZ.CommonMVC
{
    public class SMSSender
    {
        public string UserName { get; set; }
        public string AppKey { get; set; }

        public SMSSendResult SendSMS(string templateId, string code, string phoneNum)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            wc.Encoding = Encoding.UTF8;
            string status = wc.DownloadString("http://sms.rupeng.cn/SendSms.ashx?UserName="
                + Uri.EscapeDataString(UserName)
                + "&appKey=" + Uri.EscapeDataString(AppKey)
                + "&templateId=" + Uri.EscapeDataString(templateId)
                + "&code=" + Uri.EscapeDataString(code)
                + "&phoneNum=" + Uri.EscapeDataString(phoneNum));
             return JsonConvert.DeserializeObject<SMSSendResult>(status);
        }

    }
}
