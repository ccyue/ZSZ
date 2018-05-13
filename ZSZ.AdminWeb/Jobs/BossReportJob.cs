using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Jobs
{
    public class BossReportJob : IJob
    {
        private static ILog log = LogManager.GetLogger(typeof(BossReportJob));
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                log.Info("执行report:" + DateTime.Now);
                IHouseService houseService;
                ICityService cityService;
                ISettingService settingService;
                StringBuilder sb = new StringBuilder();
                string bossEmails;
                string smtpServer, smtpUserName, smtpPassword, smtpEmail;
                int smtpServerPort;
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                using (container.BeginLifetimeScope())
                {
                    houseService = container.Resolve<IHouseService>();
                    cityService = container.Resolve<ICityService>();
                    settingService = container.Resolve<ISettingService>();
                    bossEmails = settingService.GetValue("BossEmail");//读取配置中的老板邮箱 
                    smtpServer = settingService.GetValue("SmtpServer");
                    smtpServerPort = Convert.ToInt32(settingService.GetValue("SmtpServerPort"));
                    smtpUserName = settingService.GetValue("SmtpUserName");
                    smtpPassword = settingService.GetValue("SmtpPassword");
                    smtpEmail = settingService.GetValue("SmtpEmail");
                    var allCity = cityService.GetAll();
                    
                    foreach (var city in allCity)
                    {
                        long count = houseService.GetTodayNewHouseCount(city.Id);
                        sb.AppendLine(string.Format("{0}:{1}", city.Name, count));
                    }

                }
                using (MailMessage mailMessage = new MailMessage())
                using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                {
                    foreach(var email in bossEmails.Split(','))
                    {
                        mailMessage.To.Add(email);
                    }
                    
                    mailMessage.Body = sb.ToString();
                    mailMessage.From = new MailAddress(smtpEmail);
                    mailMessage.Subject = string.Format("{0} 新增房源数量报表", DateTime.Now.ToString("yyyy-MM-dd"));
                    smtpClient.Credentials = new System.Net.NetworkCredential(smtpUserName, smtpPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }

                log.Info("report完成:" + DateTime.Now);
            }
            catch(Exception ex)
            {
                log.Error("report出错",ex);
            }
        }
    }
}