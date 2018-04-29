using Autofac;
using CaptchaGen;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using IMyBLL;
using log4net;
using MyBLL;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.Service;
using ZSZ.Service.Entities;

namespace ZSZ.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 验证码
            //string s= CommonHelper.GenerateCaptchaCode(5);
            //Console.WriteLine(s); 
            #endregion

            #region Email
            //using (MailMessage mailMessage = new MailMessage())
            //using (SmtpClient smtpClient = new SmtpClient("smtp.qq.com", 465))
            //{
            //    mailMessage.To.Add("chengcheng.yue@outlook.com");
            //    mailMessage.Body = "Good morning";
            //    mailMessage.From = new MailAddress("884167190@qq.com");
            //    mailMessage.Subject = "Test email";
            //    smtpClient.Credentials = new System.Net.NetworkCredential("884167190@qq.com", "ziighahexapzbcih");
            //    smtpClient.Send(mailMessage);
            //}
            #endregion

            #region 缩略图
            //ImageProcessingJob job = new ImageProcessingJob();
            //job.Filters.Add(new FixedResizeConstraint(200, 200));
            //job.SaveProcessedImageToFileSystem(@"D:\a\01.png", @"D:\a\01_new.jpg", new JpegFormatEncoderParams());
            #endregion

            #region 水印
            //ImageWatermark imageWatermark = new ImageWatermark(@"D:\a\wm.png");
            //imageWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;
            //imageWatermark.Alpha = 100;
            //ImageProcessingJob job = new ImageProcessingJob();
            //job.Filters.Add(imageWatermark);
            //job.Filters.Add(new FixedResizeConstraint(600, 600));
            //job.SaveProcessedImageToFileSystem(@"D:\a\01.png", @"D:\a\02_new.jpg", new JpegFormatEncoderParams());
            #endregion

            #region 生成验证码图片
            //string code = CommonHelper.GenerateCaptchaCode(5);
            //using (MemoryStream ms = ImageFactory.GenerateImage(code, 60, 100, 20, 6))
            //using (FileStream fs = File.OpenWrite(@"D:\a\vc.jpg"))
            //{
            //    ms.CopyTo(fs);
            //}
            #endregion

            #region log4net
            //log4net.Config.XmlConfigurator.Configure();

            //ILog log = LogManager.GetLogger(typeof(Program));
            //log.Debug("debug...");
            //log.Warn("Warn...");
            //log.Error("Log..");
            #endregion

            #region Quartz
            //IScheduler sched = new StdSchedulerFactory().GetScheduler();

            //{
            //    JobDetailImpl jdBossReport = new JobDetailImpl("jdTest", typeof(TestJob));
            //    var builder = CalendarIntervalScheduleBuilder.Create();
            //    builder.WithInterval(3, IntervalUnit.Second);
            //    IMutableTrigger triggerBossReport = builder.Build();
            //    //IMutableTrigger triggerBossReport =
            //    //CronScheduleBuilder.DailyAtHourAndMinute(8, 54).Build();//每天23:45执行一次
            //    triggerBossReport.Key = new TriggerKey("triggerTest");
            //    sched.ScheduleJob(jdBossReport, triggerBossReport);
            //    sched.Start();
            //}
            #endregion

            #region IOC
            //ContainerBuilder builder = new ContainerBuilder();
            ////builder.RegisterType<UserBLL>().As<IUserBLL>();
            ////builder.RegisterType<UserBLL>().AsImplementedInterfaces();
            //Assembly asm = Assembly.Load("MyBLL");
            //builder.RegisterAssemblyTypes(asm).AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            //IContainer container =  builder.Build();
            //IUserBLL userbll = container.Resolve<IUserBLL>();
            //userbll.Check("yue", "123");
            #endregion

            #region SMS
            //System.Net.WebClient wc = new System.Net.WebClient();
            //string userName = "test0428";
            //string appKey = "2248af97feb06c7e1855a7";
            //int templateId = 1054;
            //int code = 76823;
            //string phoneNum = "18945267963";
            //wc.Encoding = Encoding.UTF8;
            //string status = wc.DownloadString("http://sms.rupeng.cn/SendSms.ashx?UserName="
            //    + Uri.EscapeDataString(userName)
            //    + "&appKey="+Uri.EscapeDataString(appKey)
            //    + "&templateId="+templateId
            //    + "&code="+code
            //    + "&phoneNum="+phoneNum);

            #endregion

            //using (ZSZDbContext dbcontext = new ZSZDbContext())
            //{
            //    dbcontext.Database.Delete();
            //    dbcontext.Database.Create();
            //}

                Console.ReadKey();
        }
    }
}
