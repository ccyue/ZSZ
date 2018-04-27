using CaptchaGen;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;

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
            log4net.Config.XmlConfigurator.Configure();

            ILog log = LogManager.GetLogger(typeof(Program));
            log.Debug("debug...");
            log.Warn("Warn...");
            log.Error("Log..");
            #endregion

            Console.ReadKey();
        }
    }
}
