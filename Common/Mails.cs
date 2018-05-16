using System.Net.Mail;
using System;

namespace Pool.Common
{
    public class Mails
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="isattfile">是否上传附件</param>
        /// <param name="attfile">附件地址</param>
        /// <param name="mailarr">发送对象集合</param>
        /// <param name="mailtitle">邮件标题</param>
        /// <param name="maildesc">邮件内容</param>
        /// <returns></returns>
        /*public static bool SendMail(bool isattfile,string attfile,string[] mailarr,string mailtitle,string maildesc)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(ConfigurationManager.AppSettings["MailAccUser"]);
            foreach(string s in mailarr)
            {
                if(Strings.IsEmail(s))mm.To.Add(s);
            }
            
            mm.Subject = mailtitle;//标题
            mm.Body = maildesc;//内容
            if(isattfile) mm.Attachments.Add(new Attachment(attfile));
            mm.IsBodyHtml = true;
            //编码
            mm.BodyEncoding = System.Text.Encoding.GetEncoding("gb2312");//内容
            mm.SubjectEncoding = System.Text.Encoding.GetEncoding("gb2312");//主题
            SmtpClient smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["MailServer"];
            smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["MailAccUser"], ConfigurationManager.AppSettings["MailAccPw"]);
            smtp.Timeout = 300000;
            try
            {
                smtp.Send(mm);
                mm.Dispose();
                return true;
            }
            catch(Exception e)
            {
                ILog log = LogManager.GetLogger("Logger");
                log.Error(e.ToString());
                return false;
            }
            finally
            {
                mm.Dispose();
            }
        }*/

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="isattfile">是否上传附件</param>
        /// <param name="attfile">附件地址</param>
        /// <param name="mailarr">发送对象集合</param>
        /// <param name="mailtitle">邮件标题</param>
        /// <param name="maildesc">邮件内容</param>
        /// <param name="configs">邮件配置</param>
        /// <returns></returns>
        public static bool SendMail(bool isattfile, string attfile, string[] mailarr, string mailtitle, string maildesc,MailConfig configs)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(configs.MailFrom);
            foreach (string s in mailarr)
            {
                if (Strings.IsEmail(s)) mm.To.Add(s);
            }
            if (configs.MailCCs != null && configs.MailCCs.Length > 0)
            {
                foreach (string s in configs.MailCCs)
                {
                    if (Strings.IsEmail(s)) mm.CC.Add(s);
                }
            }
            mm.Subject = mailtitle;//标题
            mm.Body = maildesc;//内容
            
            if (isattfile) mm.Attachments.Add(new Attachment(attfile));
            mm.IsBodyHtml = true;
            //编码
            mm.BodyEncoding = System.Text.Encoding.GetEncoding("gb2312");//内容
            mm.SubjectEncoding = System.Text.Encoding.GetEncoding("gb2312");//主题
            SmtpClient smtp = new SmtpClient();
            smtp.Port = configs.MailPoint;
            smtp.Host = configs.MailServer;
            smtp.Credentials = new System.Net.NetworkCredential(configs.MailUser, configs.MailPassWord);
            smtp.Timeout = 300000;
            try
            {
                smtp.Send(mm);
                mm.Dispose();
                return true;
            }
            catch (Exception e)
            {
                mm.Dispose();
                return false;
            }
        }

        public class MailConfig
        {
            /// <summary>
            /// 邮箱服务器
            /// </summary>
            public string MailServer { set; get; }

            /// <summary>
            /// 邮箱服务器端口
            /// </summary>
            public int MailPoint { set; get; }

            /// <summary>
            /// 发件箱
            /// </summary>
            public string MailFrom { set; get; }

            /// <summary>
            /// 账号
            /// </summary>
            public string MailUser { set; get; }

            /// <summary>
            /// 密码
            /// </summary>
            public string MailPassWord { set; get; }

            /// <summary>
            /// 抄送邮箱列表
            /// </summary>
            public string[] MailCCs { set; get; }
        }
    }
}
