#region 版权信息
/* ======================================================================== 
 * 描述信息 
 *  
 * 作者  ：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间  ：2018年3月12日14:45:04
 * 功能  ：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Net;
using System.Net.Mail;
using SpiderHelp.ConfigModule;
using SpiderHelp.SaveModule;

namespace SpiderHelp.MessageModule
{
    /// <summary>
    /// 邮件通知类
    /// </summary>
	public class Email
    {
        #region 常量
        /// <summary>
        /// 发送者邮箱
        /// </summary>
        private const string SFrom = "Messager2@jiuweiwang.com";
        /// <summary>
        /// 发送人
        /// </summary>
        private const string SFromer = "Messager";
        /// <summary>
        /// smtp服务器
        /// </summary>
        private const string SmtpHost = "smtp.exmail.qq.com";
        /// <summary>
        /// smtp邮箱
        /// </summary>
        private const string SmtPuser = "Messager2@jiuweiwang.com";
        /// <summary>
        /// smtp邮箱密码
        /// </summary>
        private const string SmtPpass = "Jww123";
        #endregion

        #region 邮箱发送
        /// <summary>
        /// 重要消息邮件通知
        /// </summary>
        /// <param name="bodyInfo">邮件内容</param>
        private bool SendEmailMessage(string bodyInfo)
        {
			try
			{
				return SendEerrorToMail(bodyInfo, MyConfig.ToEmail);
			}
			catch (Exception ex)
			{
                CLog.EmailLog($"Email.SendEmailMessage>>>{ex.Message}", "EmailSendError");
				return false;
			}			
		}

		/// <summary>
		/// 给主账号发送邮件
		/// </summary>
		/// <param name="title">邮件标题</param>
		/// <param name="bodyInfo">邮件内容</param>
		/// <returns>是否发送成功</returns>
		private bool SendLogMessage(string title, string bodyInfo)
		{
			try
			{
				return SendMail(SFrom, SFromer, MyConfig.ToEmail, "", title, bodyInfo, "", SmtpHost, SmtPuser, SmtPpass);
			}
			catch (Exception ex)
			{
                CLog.EmailLog($"Email.SendLogMessage>>>{ex.Message}", "EmailSendError");
				return false;
			}			
		}

		/// <summary>
		/// 简单邮件发送
		/// </summary>
		/// <param name="toEmail">邮箱</param>
		/// <param name="title">标题</param>
		/// <param name="bodyInfo">邮件内容</param>
		/// <returns>是否发送成功</returns>
		public static bool SendSimpleEmail(string toEmail, string title, string bodyInfo)
		{
			try
			{
				return SendMail(SFrom, SFromer, toEmail, "", title, bodyInfo, "", SmtpHost, SmtPuser, SmtPpass);
			}
			catch (Exception ex)
			{
                CLog.EmailLog($"Email.SendSimpleEmail>>>{ex.Message}", "EmailSendError");
				return false;
			}			
		}

		/// <summary>
		/// 大数据平台发送重要错误
		/// </summary>
		/// <param name="Error">错误信息</param>
		/// <param name="ToMail">邮箱</param>
		/// <returns>是否发送成功</returns>
		public static bool SendEerrorToMail(string Error, string ToMail)
		{
			return SendMail(SFrom, SFromer, ToMail, "", "大数据平台重要消息错误", Error, "", SmtpHost, SmtPuser, SmtPpass);
		}

		/// <summary>
		/// C#发送邮件函数
		/// </summary>
		/// <param name="sfrom">发送者邮箱</param>
		/// <param name="sfromer">发送人</param>
		/// <param name="sto">接受者邮箱</param>
		/// <param name="stoer">收件人</param>
		/// <param name="sSubject">主题</param>
		/// <param name="sBody">内容</param>
		/// <param name="sfile">附件</param>
		/// <param name="sSmtpHost">smtp服务器</param>
		/// <param name="sSmtPuser">邮箱</param>
		/// <param name="sSmtPpass">密码</param>
		/// <returns>是否发送成功</returns>
		public static bool SendMail(string sfrom, string sfromer, string sto, string stoer, string sSubject, string sBody, string sfile, string sSmtpHost, string sSmtPuser, string sSmtPpass)
		{
		    try
		    {
		        ////设置from和to地址
		        MailAddress from = new MailAddress(sfrom, sfromer);
		        MailAddress to = new MailAddress(sto, stoer);
		        ////创建一个MailMessage对象
		        MailMessage oMail = new MailMessage(from, to);
		        //// 添加附件
		        if (sfile != "")
		        {
		            oMail.Attachments.Add(new Attachment(sfile));
		        }
		        ////邮件标题
		        oMail.Subject = sSubject;
		        ////邮件内容
		        oMail.Body = sBody;
		        ////邮件格式
		        oMail.IsBodyHtml = true;
		        ////邮件采用的编码
		        oMail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");
		        ////设置邮件的优先级为高
		        oMail.Priority = MailPriority.High;
		        ////发送邮件
		        SmtpClient client = new SmtpClient
		        {
		            Host = sSmtpHost,
		            Credentials = new NetworkCredential(sSmtPuser, sSmtPpass),
		            DeliveryMethod = SmtpDeliveryMethod.Network
		        };
		        ////client.UseDefaultCredentials = false; 
		        client.Send(oMail);
		        return true;
		    }
		    catch (Exception ex)
		    {
		        //失败，错误信息：ex.Message;  
		        CLog.EmailLog($"Email.SendMail>>>{ex.Message}", "EmailSendError");
		        return false;
		    }
		}

		/// <summary>
        /// C#发送邮件函数（用于阿里云服务器）
        /// </summary>
        /// <param name="sfrom">发送者邮箱</param>
        /// <param name="sfromer">发送人</param>
        /// <param name="sto">接受者邮箱</param>
        /// <param name="stoer">收件人</param>
        /// <param name="sSubject">主题</param>
        /// <param name="sBody">内容</param>
        /// <param name="sfile">附件</param>
        /// <param name="sSmtpHost">smtp服务器</param>
        /// <param name="sSmtPuser">邮箱</param>
        /// <param name="sSmtPpass">密码</param>
        /// <returns>是否发送成功</returns>
        public static bool SendMailUseGmail(string sfrom, string sfromer, string sto, string stoer, string sSubject, string sBody, string sfile, string sSmtpHost, string sSmtPuser, string sSmtPpass)
        {
#pragma warning disable 618
            System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
#pragma warning restore 618
            try
            {
                mail.To = sto;
                mail.From = sfrom;
                mail.Subject = sSubject;
#pragma warning disable 618
                mail.BodyFormat = System.Web.Mail.MailFormat.Html;
#pragma warning restore 618
                mail.Body = sBody;
				//身份验证  
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
				//邮箱登录账号，这里跟前面的发送账号一样就行
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mail.From);
				//这个密码要注意：如果是一般账号，要用授权码；企业账号用登录密码
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", sSmtPpass);
				//端口
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);
				//SSL加密
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
				//企业账号用smtp.exmail.qq.com
#pragma warning disable 618
                System.Web.Mail.SmtpMail.SmtpServer = sSmtpHost; 
#pragma warning restore 618
#pragma warning disable 618
                System.Web.Mail.SmtpMail.Send(mail);
#pragma warning restore 618
				//邮件发送成功  
                return true;             
            }
            catch (Exception ex)
            {
                //失败，错误信息：ex.Message;  
                CLog.EmailLog($"Email.SendMailUseGmail>>>{ex.Message}", "EmailSendError");
				return false;                
            }
        }

		/// <summary>
		///	异常代码邮件发送
		/// </summary>
		/// <param name="toEmail">收件人</param>
		/// <param name="title">邮件标题</param>
		/// <param name="code">错误代码</param>
		/// <returns>是否发送成功</returns>
		public static bool SendErrorCodeEmail(string toEmail, string title, string code)
		{
		    try
		    {
		        string error = "";
		        string method = "";
		        string errordetial = "";
		        string infot = "";
		        string titles = "";
		        switch (code)
		        {
		            #region 需要终止程序的异常情况
		            case "000":
		                titles = title + "-程序处理数据连续触发异常【000】";
		                error = titles;
		                method = "连续数据处理异常报警超过6次，程序终止";
		                errordetial = "需要终止程序的异常情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "001":
		                titles = title + "-程序请求数据连续触发异常【001】";
		                error = titles;
		                method = "连续数据请求异常报警超过144次，程序终止";
		                errordetial = "需要终止程序的异常情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;		          
		            #endregion

		            #region 数据处理（非服务器响应的异常）              

		            case "600":
		                titles = title + "-模板标识检测特殊异常【600】";
		                error = titles;
		                method = "异常模版检测标识总共超过50个（不包含空页面），程序休息24小时，累计出现指定次数，停止抓取，发送邮件给负责人";
		                errordetial = "拿到数据内容过不了特征标识，或者正则规则，Xpath检测等情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "601":
		                titles = title + "-模板请求为空【601】";
		                error = titles;
		                method = "模版请求为空连续超过50个，程序休息5分钟，累计出现指定次数，停止抓取，发送邮件给负责人";
		                errordetial = "拿到数据内容过不了特征标识，或者正则规则，Xpath检测等情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "602":
		                titles = title + "-模板请求无数据【602】";
		                error = titles;
		                method = "模板请求无数据连续超过50个（不包含空页面），程序休息5分钟，累计出现指定次数，停止抓取，发送邮件给负责人";
		                errordetial = "拿到数据内容过不了特征标识，或者正则规则，Xpath检测等情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "603":
		                titles = title + "-模板请求打码【603】";
		                error = titles;
		                method = "模板请求被打码连续超过50个（不包含空页面），程序休息5分钟，累计出现指定次数，停止抓取，发送邮件给负责人";
		                errordetial = "拿到数据内容过不了特征标识，或者正则规则，Xpath检测等情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "604":
		                titles = title + "-模板解析出错【604】";
		                error = titles;
		                method = "模板解析出错连续超过50个（不包含空页面），程序休息5分钟，累计出现指定次数，停止抓取,发送邮件给负责人";
		                errordetial = "解析数据提取过不了特征标识，或者正则规则，Xpath检测等情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;

                    #endregion

                    #region 文件写入异常    
		            case "700":
		                titles = title + "-文件写入异常【700】";
		                error = titles;
		                method = "文件写入失败连续超过30次，阻塞写入线程，爬虫休息30分钟，继续尝试写入。由监控程序监控";
		                errordetial = "文件写入异常";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
                    case "701":
		                titles = title + "-磁盘空间不足【701】";
		                error = titles;
		                method = "阻塞写入线程，爬虫休息30分钟，继续尝试写入。由监控程序监控";
		                errordetial = "磁盘空间不足";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "702":
		                titles = title + "-文件被占用【702】";
		                error = titles;
		                method = "阻塞写入线程，爬虫休息30分钟，继续尝试写入。由监控程序监控";
		                errordetial = "文件被占用";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "703":
		                titles = title + "-无文件写入权限【703】";
		                error = titles;
		                method = "阻塞写入线程，爬虫休息30分钟，继续尝试写入。由监控程序监控";
		                errordetial = "当前文件系统无写入权限";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "704":
		                titles = title + "-无法定位文件写入路径【704】";
		                error = titles;
		                method = "阻塞写入线程，爬虫休息30分钟，继续尝试写入。由监控程序监控";
		                errordetial = "获取不到该文件系统的写入路径";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;

                    #endregion

                    #region 数据库操作异常

		            case "800":
		                titles = title + "-数据库操作异常【800】";
		                error = titles;
		                method = "数据库操作失败连续30次以上，阻塞数据库处理线程，将错误代码发送邮件给负责人，休息10分钟，继续尝试打开";
		                errordetial = "数据库操作异常";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
                    case "801":
		                titles = title + "-数据库空间已满【801】";
		                error = titles;
		                method = "阻塞数据库处理线程，连续30次以上，将错误代码发送邮件给负责人，休息10分钟，继续尝试打开";
		                errordetial = "数据库空间满了";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "802":
		                titles = title + "-数据库占用过高【802】";
		                error = titles;
		                method = "阻塞数据库处理线程，连续30次以上，将错误代码发送邮件给负责人，休息10分钟，继续尝试打开";
		                errordetial = "打不开数据库";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "803":
		                titles = title + "-数据库写入超时【803】";
		                error = titles;
		                method = "阻塞数据库处理线程，连续30次以上，将错误代码发送邮件给负责人，休息10分钟，继续尝试打开";
		                errordetial = "操作数据库超时";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "804":
		                titles = title + "-数据库操作失败【804】";
		                error = titles;
		                method = "阻塞数据库处理线程，连续30次以上，将错误代码发送邮件给负责人，休息10分钟，继续尝试打开";
		                errordetial = "数据库连接字符串等格式不规范造成";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;

		            #endregion

		            #region 程序内存溢出

		            case "900":
		                titles = title + "-程序内存溢出【900】";
		                error = titles;
		                method = "监控系统对无数据的时候进行反馈，由监控端通知给爬虫设计者，更新优化爬虫程序";
		                errordetial = "内存溢出，一般由于爬虫占用的系统内存资源不释放造成的";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;

		            #endregion

		            #region 数据请求异常（服务器响应异常的情况）

		            case "504":
		                titles = title + "-网络错误【504】";
		                error = titles;
		                method =
		                    "1.休息3秒，再次发送请求，连续30次以上，停止爬虫，将错误代码发送邮件给负责人<br>2.若某些站点是类似ID遍历这样的任务（部分请求本来就是无效，而不是站点有问题），则不采用第一条的办法";
		                errordetial = "请求超时";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "502":
		                titles = title + "-网络错误【502】";
		                error = titles;
		                method =
		                    "1.休息3秒，再次发送请求，连续30次以上，停止爬虫，将错误代码发送邮件给负责人<br>2.若某些站点是类似ID遍历这样的任务（部分请求本来就是无效，而不是站点有问题），则不采用第一条的办法";
		                errordetial = "服务器接收错误响应";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
                    case "5xx":
		                titles = title + "-网络错误【5xx】";
		                error = titles;
		                method = "休息3秒，再次发送请求，连续30次以上，停止爬虫，将错误代码发送邮件给负责人";
		                errordetial = "服务器返回了非法的应答，对方服务器内部不可用，服务停止等原因造成的";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "429":
		                titles = title + "-网络错误【429】";
		                error = titles;
		                method = "将错误代码发给邮件负责人";
		                errordetial = "阿布云的请求过快出现的反馈信号";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "404":
		                titles = title + "-网络错误【404】";
		                error = titles;
		                method = "连续出现5次。1.跳过该页面，爬虫继续<br>2.若某些站点偶尔会出现请求不到内容，但请求多几次内容就会出现的情况，则不采用第一条的办法";
		                errordetial = "没有该页面";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "402":
		                titles = title + "-网络错误【402】";
		                error = titles;
		                method = "休息3秒，再次发送请求，连续30次以上，爬虫休息5分钟，将错误代码发送邮件给负责人";
		                errordetial = "主要是代理需要续费的情况";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "403":
		                titles = title + "-网络错误【403】";
		                error = titles;
		                method = "休息3秒，再次发送请求，连续30次以上，爬虫休息5分钟，将错误代码发送邮件给负责人";
		                errordetial = "此页面为前端禁止访问页面";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
                    case "401":
		                titles = title + "-网络错误【401】";
		                error = titles;
		                method = "连续出现1次。需要重新更新Cookies，继续该页面抓取。若还是出现错误，爬虫休息5分钟，将错误代码发送邮件给负责人";
		                errordetial = "未授权页面";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "4xx":
		                titles = title + "-网络错误【4xx】";
		                error = titles;
		                method = "休息3秒，再次发送请求，连续5次出现错误，则跳过该页面，爬虫继续。若出现连续10次跳过页面的情况，爬虫休息5分钟，将错误代码发送邮件给负责人";
		                errordetial = "服务器不能处理爬虫请求的格式内容，URL过长等问题";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "3xx":
		                titles = title + "-网络错误【3xx】";
		                error = titles;
		                method = "拿到服务器给出的调整地址，重新访问，如要登录请更新Cookies,进行访问，连续超过10次，放弃该页面请求，爬虫继续，将错误代码发送邮件给负责人";
		                errordetial = "服务器超链转移，一般是服务器有登录要求的网站";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "1xx":
		                titles = title + "-网络错误【1xx】";
		                error = titles;
		                method = "连续出现1次。有数据反馈就进行接收处理，无则放弃，爬虫继续";
		                errordetial = "该情况少见";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		            case "x0x":
		                titles = title + "-数据请求未知明异常【x0x】";
		                error = titles;
		                method = "数据请求出现的服务器特殊异常，爬虫休息5分钟，将错误代码发送邮件给负责人，自行排查错误原因";
		                errordetial = "不在既定规则下产生的特殊异常";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
                    #endregion
                    default:
		                titles = title + "-程序未知明异常【xxx】";
		                error = titles;
		                method = "程序出现的特殊异常，爬虫休息5分钟，将错误代码发送邮件给负责人，自行排查错误原因";
		                errordetial = "不在既定规则下产生的特殊异常";
		                infot = string.Format(
		                    "<strong>错误说明：</strong>{0}<br><strong>处理办法：</strong>{1}<br><strong>错误描述：</strong>{2}", error,
		                    method, errordetial);
		                break;
		        }
		        return SendMail(SFrom, SFromer, toEmail, "", titles, infot, "", SmtpHost, SmtPuser, SmtPpass);
		    }
		    catch (Exception ex)
		    {
		        CLog.EmailLog($"Email.SendErrorCodeEmail>>>{ex.Message}", "EmailSendError");
		        return false;
		    }
		}		
		#endregion
	}
}
