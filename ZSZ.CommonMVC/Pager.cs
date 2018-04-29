using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.CommonMVC
{
    public class Pager
    {
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int MaxPageCount { get; set; }

        public int PageIndex { get; set; }

        public string UrlPattern { get; set; }

        public string CurrentPageClassName { get; set; }

        public string GetPagerHtml()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<ul>");

            int pageCount = (int)Math.Ceiling(TotalCount * 1.0f / PageSize);
            int startPageIndex = Math.Max(1, PageIndex - MaxPageCount / 2);
            int endPageIndex = Math.Min(pageCount, startPageIndex + MaxPageCount);
            for(int i = startPageIndex;i<=endPageIndex;i++)
            {
                if(i==PageIndex)
                {
                    html.Append("<li class = '")
                        .Append(CurrentPageClassName+"'>")
                        .Append(i)
                        .Append("</li>");
                }
                else
                {
                    html.Append("<li> <a = href = '")
                        .Append(UrlPattern.Replace("{pn}",i.ToString()))
                        .Append("'>")
                        .Append(i)
                        .Append("</a></li>");
                }
            }
            html.Append("</ul>");
            return html.ToString();
        }
    }
}
