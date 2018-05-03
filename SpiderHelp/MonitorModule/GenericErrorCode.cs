#region 版权信息
/* ======================================================================== 
 * 描述信息 
 *  
 * 作者  ：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间  ：2018年3月12日14:45:04
 * 功能  ：爬虫常见错误代码处理
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Net;
using SpiderHelp.MessageModule;

namespace SpiderHelp.MonitorModule
{
    /// <summary>
    /// 程序错误代码类
    /// </summary>
	public class GenericErrorCode
	{
		#region 服务器异常响应变量
		/// <summary>
		/// 504错误
		/// </summary>
		private int GatewayTimeout { get; set; }
        /// <summary>
        /// 502错误
        /// </summary>
	    private int BadGateway { get; set; }
        /// <summary>
        /// 5xx错误
        /// </summary>
        private int InternalServerError { get; set; }
		/// <summary>
		/// 429错误 暂不作处理
		/// </summary>
		private int ToManyRequest { get; set; }
        /// <summary>
        /// 404错误
        /// </summary>
        private int NotFound { get; set; }
		/// <summary>
		/// 403错误
		/// </summary>
		private int Forbidden { get; set; }
		/// <summary>
		/// 401错误
		/// </summary>
		private int Unauthorized { get; set; }
		/// <summary>
		/// 4xx错误
		/// </summary>
		private int BadRequest { get; set; }
	    /// <summary>
	    /// 402
	    /// </summary>
	    private int PaymentRequired { get; set; }
		/// <summary>
		/// 3xx错误
		/// </summary>
		private int MultipleChoices { get; set; }
		/// <summary>
		/// 1xx错误
		/// </summary>
		private int Continue { get; set; }
	    /// <summary>
	    /// xxx错误
	    /// </summary>
	    private int ExtraErrorRequest { get; set; }
        #endregion

        #region 数据库和文件操作异常变量
        /// <summary>
        /// 数据库操作连接失败次数
        /// </summary>
        public int IdbError { get; set; }
        /// <summary>
        /// 文件写入连续错误次数
        /// </summary>
        public int IwriteError { get; set; }
        /// <summary>
        /// 请求连续为空的次数
        /// </summary>
        public int InullError { get; set; }
        /// <summary>
        /// 请求连续无数据的次数
        /// </summary>
        public int IzeroError { get; set; }
        /// <summary>
        /// 连续被打码的次数
        /// </summary>
        public int IvfError { get; set; }
        /// <summary>
        /// 模板异常计数
        /// </summary>
        public int ItabError { get; set; }
        //连续解析出错的次数
        /// <summary>
        /// 连续解析出错的次数
        /// </summary>
        public int IanalysisError { get; set; }
        #endregion
        /// <summary>
        /// 连续触发异常的次数
        /// </summary>
        private int ItryMax { get; set; }
		/// <summary>
		/// 连续触发请求异常的次数
		/// </summary>
		private int IrequestTryMax { get; set; }
		/// <summary>
		/// 异常处理的邮件机制
		/// </summary>
		/// <param name="email">邮箱</param>
		/// <param name="title">标题</param>
		/// <param name="errorCode">错误代码</param>
		public void ErrorHandleEmail(string email, string title, string errorCode)
		{
			if (errorCode != "200")
			{
				IrequestTryMax++;
				bool flgx = Email.SendErrorCodeEmail(email, title, errorCode);
				Console.Write("{0}>>>{1}请求数据触发异常报警, 发送邮件【{2}】休息5分钟.....", DateTime.Now, errorCode, flgx);
				System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
				Console.WriteLine("结束休息...{0}", DateTime.Now);
			}
			else if(errorCode == "200")
			{
				if(ItabError >= 30)
				{
					ItryMax++;
					bool flg = Email.SendErrorCodeEmail(email, title, "600");
					Console.Write("{0}>>>600模板异常连续超过30次, 发送邮件【{1}】休息24小时.....", DateTime.Now, flg);
					System.Threading.Thread.Sleep(new TimeSpan(24, 0, 0));
					Console.WriteLine("结束休息...{0}", DateTime.Now);
					ItabError = 0;
				}				
				else if(InullError >= 50)
				{
					ItryMax++;
					bool flg = Email.SendErrorCodeEmail(email, title, "601");
					Console.Write("{0}601模板请求为空连续超过50个, 发送邮件【{1}】休息5分钟.....", DateTime.Now, flg);
					System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
					Console.WriteLine("结束休息...{0}", DateTime.Now);
					InullError = 0;
				}
				else if(IzeroError >= 50)
				{
					ItryMax++;
					bool flg = Email.SendErrorCodeEmail(email, title, "602");
					Console.Write("{0}602模板请求无数据连续超过50个, 发送邮件【{1}】休息5分钟.....", DateTime.Now, flg);
					System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
					Console.WriteLine("结束休息...{0}", DateTime.Now);
					IzeroError = 0;
				}
				else if(IvfError >= 50)
				{
					ItryMax++;
					bool flg = Email.SendErrorCodeEmail(email, title, "603");
					Console.Write("{0}603模板请求被打码连续超过50个, 发送邮件【{1}】休息5分钟.....", DateTime.Now, flg);
					System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
					Console.WriteLine("结束休息...{0}", DateTime.Now);
					IvfError = 0;
				}
				else if(IanalysisError >= 50)
				{
					ItryMax++;
					bool flg = Email.SendErrorCodeEmail(email, title, "604");
					Console.Write("{0}604模板解析出错连续超过50个, 发送邮件【{1}】休息5分钟.....", DateTime.Now, flg);
					System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
					Console.WriteLine("结束休息...{0}", DateTime.Now);
					IanalysisError = 0;
				}
				else if (IwriteError >= 30)
				{
				    ItryMax++;
				    bool flg = Email.SendErrorCodeEmail(email, title, "700");
				    Console.Write("{0}700磁盘写入失败连续超过30次, 发送邮件【{1}】休息30分钟.....", DateTime.Now, flg);
				    System.Threading.Thread.Sleep(new TimeSpan(0, 30, 0));
				    Console.WriteLine("结束休息...{0}", DateTime.Now);
				    IwriteError = 0;
				}
				else if (IdbError >= 30)
				{
				    ItryMax++;
				    bool flg = Email.SendErrorCodeEmail(email, title, "800");
				    Console.Write("{0}800数据库操作失败连续超过30次, 发送邮件【{1}】休息10分钟.....", DateTime.Now, flg);
				    System.Threading.Thread.Sleep(new TimeSpan(0, 10, 0));
				    Console.WriteLine("结束休息...{0}", DateTime.Now);
				    IdbError = 0;
				}
                else
				{
					ItryMax = 0;
				}
			}
			else
			{
				IrequestTryMax = 0;
			}			
			if (ItryMax >= 6)
			{
				bool flg = Email.SendErrorCodeEmail(email, title, "000");
				Console.Write("{0}连续数据处理异常报警超过6次, 发送邮件【{1}】休息5分钟后终止程序.....", DateTime.Now, flg);
				System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
				Console.WriteLine("结束休息...{0}", DateTime.Now);
				ItryMax = 0;
				System.Environment.Exit(0);
			}
			if(IrequestTryMax >= 144)
			{
				bool flg = Email.SendErrorCodeEmail(email, title, "001");
				Console.Write("{0}连续数据请求异常报警超过144次, 发送邮件【{1}】休息5分钟后终止程序.....", DateTime.Now, flg);
				System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
				Console.WriteLine("结束休息...{0}", DateTime.Now);
				IrequestTryMax = 0;
				System.Environment.Exit(0);
			}
		}

		/// <summary>
		/// 请求异常的处理方法
		/// </summary>
		/// <param name="rsp">请求之后的HttpWebResponse</param>
		/// <returns>错误代码：504、5xx、404、403、401、4xx、3xx、1xx</returns>
		public string HandleMethod(HttpWebResponse rsp)
		{
			string str;	     
			if(rsp == null)
			{
				str = "200";
				GatewayTimeout = 0;
			    BadGateway = 0;
                InternalServerError = 0;
				ToManyRequest = 0;
				NotFound = 0;
				Forbidden = 0;
			    PaymentRequired = 0;
				Unauthorized = 0;
			    BadRequest = 0;
				MultipleChoices = 0;
				Continue = 0;
			    ExtraErrorRequest = 0;
			}
			else
			{
				#region 错误代码标识
				if(rsp.StatusCode == HttpStatusCode.OK)
				{				   
				    GatewayTimeout = 0;
				    BadGateway = 0;
                    InternalServerError = 0;
				    ToManyRequest = 0;
				    NotFound = 0;
				    Forbidden = 0;
				    Unauthorized = 0;
				    BadRequest = 0;
				    PaymentRequired = 0;
				    MultipleChoices = 0;
				    Continue = 0;
				    ExtraErrorRequest = 0;
                } 
				//504错误
				else if(rsp.StatusCode == HttpStatusCode.GatewayTimeout)
				{
					GatewayTimeout++;
				}
				//502错误
				else if (rsp.StatusCode == HttpStatusCode.BadGateway)
				{
				    BadGateway++;
				}
                //5xx错误
                else if (rsp.StatusCode == HttpStatusCode.HttpVersionNotSupported ||
                         rsp.StatusCode == HttpStatusCode.ServiceUnavailable ||
                         rsp.StatusCode == HttpStatusCode.NotImplemented ||
                         rsp.StatusCode == HttpStatusCode.InternalServerError)
				{
				    InternalServerError++;
				}
				//404错误
				else if (rsp.StatusCode == HttpStatusCode.NotFound)
				{
					NotFound++;
				}
                //403错误
                else if (rsp.StatusCode == HttpStatusCode.Forbidden)
				{
					Forbidden++;
				}
				//402错误
				else if (rsp.StatusCode == HttpStatusCode.PaymentRequired)
				{
				    PaymentRequired++;
				}
                //401错误
                else if (rsp.StatusCode == HttpStatusCode.Unauthorized)
				{
					Unauthorized++;
				} 
                //4xx错误
				else if (rsp.StatusCode == HttpStatusCode.ExpectationFailed ||
				         rsp.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable ||
				         rsp.StatusCode == HttpStatusCode.UnsupportedMediaType ||
				         rsp.StatusCode == HttpStatusCode.RequestUriTooLong ||
				         rsp.StatusCode == HttpStatusCode.RequestEntityTooLarge ||
				         rsp.StatusCode == HttpStatusCode.PreconditionFailed ||
				         rsp.StatusCode == HttpStatusCode.LengthRequired || rsp.StatusCode == HttpStatusCode.Gone ||
				         rsp.StatusCode == HttpStatusCode.Conflict || rsp.StatusCode == HttpStatusCode.RequestTimeout ||
				         rsp.StatusCode == HttpStatusCode.ProxyAuthenticationRequired ||
				         rsp.StatusCode == HttpStatusCode.NotAcceptable ||
				         rsp.StatusCode == HttpStatusCode.MethodNotAllowed ||
				         rsp.StatusCode == HttpStatusCode.BadRequest)
				{
				    BadRequest++;
				}
				//3xx错误
				else if (rsp.StatusCode == HttpStatusCode.RedirectKeepVerb || rsp.StatusCode == HttpStatusCode.TemporaryRedirect ||
				         rsp.StatusCode == HttpStatusCode.Unused || rsp.StatusCode == HttpStatusCode.UseProxy ||
				         rsp.StatusCode == HttpStatusCode.NotModified || rsp.StatusCode == HttpStatusCode.RedirectMethod ||
				         rsp.StatusCode == HttpStatusCode.SeeOther || rsp.StatusCode == HttpStatusCode.Redirect ||
				         rsp.StatusCode == HttpStatusCode.Found || rsp.StatusCode == HttpStatusCode.Moved ||
				         rsp.StatusCode == HttpStatusCode.MovedPermanently || rsp.StatusCode == HttpStatusCode.Ambiguous ||
				         rsp.StatusCode == HttpStatusCode.MultipleChoices)
				{
				    MultipleChoices++;
				}
				//1xx错误
				else if (rsp.StatusCode == HttpStatusCode.SwitchingProtocols || rsp.StatusCode == HttpStatusCode.Continue)
				{
					Continue++;
				}
				else
				{
				    ExtraErrorRequest++;
				}
                #endregion

			    #region 关于错误的返回
			    //504错误处理方式
                if (GatewayTimeout >= 30)
				{
					str = "504";
				    GatewayTimeout = 0;				    
                }
                //502错误处理方式
                else if (BadGateway >= 30)
                {
                    str = "502";
                    BadGateway = 0;                  
                }
                //5xx错误处理方式
                else if (InternalServerError >= 30)
				{
					str = "5xx";
					InternalServerError = 0;					
				}
                //404错误处理方式
                else if (NotFound >= 5)
				{
					str = "404";
					NotFound = 0;
				}
                //402错误处理方式
                else if (PaymentRequired >= 30)
                {
                    str = "402";
                    PaymentRequired = 0;
                }
                //403错误处理方式
                else if (Forbidden >= 30)
				{
					str = "403";
					Forbidden = 0;
				}
                //401错误处理方式
                else if (Unauthorized >= 1)
				{
					str = "401";
					Unauthorized = 0;
				}               
                //4xx错误处理方式
                else if (BadRequest >= 10)
				{
					str = "4xx";
					BadRequest = 0;
				}
                //3xx错误处理方式
                else if (MultipleChoices >= 10)
				{
					str = "3xx";
					MultipleChoices = 0;
				}
                //1xx错误处理方式
                else if (Continue >= 1)
				{
				    str = "1xx";
				    Continue = 0;
				}
                //xxx错误处理方式
                else if (ExtraErrorRequest >= 1)
                {
                    str = "x0x";
                    ExtraErrorRequest = 0;
                }
                else
				{
				    str = "200";
				}
				#endregion
			}
			return str;
		}
		private string ErrorCodeReslut(HttpWebResponse rsp)
		{
			string str = string.Empty;
			if(rsp == null)
			{
				str = "200";
			}
			else
			{
				#region 错误代码标识

				if(rsp.StatusCode == HttpStatusCode.OK)
				{
					str = "200";
				} //504错误处理方式
				else if(rsp.StatusCode == HttpStatusCode.GatewayTimeout)
				{
					GatewayTimeout++;
					//Thread.Sleep(3000);
				} //504错误
				else if(rsp.StatusCode == HttpStatusCode.HttpVersionNotSupported || rsp.StatusCode == HttpStatusCode.ServiceUnavailable ||
						 rsp.StatusCode == HttpStatusCode.BadGateway || rsp.StatusCode == HttpStatusCode.NotImplemented ||
						 rsp.StatusCode == HttpStatusCode.InternalServerError)
				{
					InternalServerError++;
					//Thread.Sleep(3000);
				} //5xx错误
				else if(rsp.StatusCode == HttpStatusCode.NotFound)
				{
					NotFound++;
				} //404错误
				else if(rsp.StatusCode == HttpStatusCode.Forbidden)
				{
					Forbidden++;
					//Thread.Sleep(3000);
				} //403错误
				else if(rsp.StatusCode == HttpStatusCode.Unauthorized)
				{
					Unauthorized++;
				} //401错误
				else if(rsp.StatusCode == HttpStatusCode.ExpectationFailed ||
						 rsp.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable ||
						 rsp.StatusCode == HttpStatusCode.UnsupportedMediaType || rsp.StatusCode == HttpStatusCode.RequestUriTooLong ||
						 rsp.StatusCode == HttpStatusCode.RequestEntityTooLarge || rsp.StatusCode == HttpStatusCode.PreconditionFailed ||
						 rsp.StatusCode == HttpStatusCode.LengthRequired || rsp.StatusCode == HttpStatusCode.Gone ||
						 rsp.StatusCode == HttpStatusCode.Conflict || rsp.StatusCode == HttpStatusCode.RequestTimeout ||
						 rsp.StatusCode == HttpStatusCode.ProxyAuthenticationRequired || rsp.StatusCode == HttpStatusCode.NotAcceptable ||
						 rsp.StatusCode == HttpStatusCode.MethodNotAllowed || rsp.StatusCode == HttpStatusCode.PaymentRequired ||
						 rsp.StatusCode == HttpStatusCode.BadRequest)
				{
					BadRequest++;
					//Thread.Sleep(3000);
				} //4xx错误
				else if(rsp.StatusCode == HttpStatusCode.RedirectKeepVerb || rsp.StatusCode == HttpStatusCode.TemporaryRedirect ||
						 rsp.StatusCode == HttpStatusCode.Unused || rsp.StatusCode == HttpStatusCode.UseProxy ||
						 rsp.StatusCode == HttpStatusCode.NotModified || rsp.StatusCode == HttpStatusCode.RedirectMethod ||
						 rsp.StatusCode == HttpStatusCode.SeeOther || rsp.StatusCode == HttpStatusCode.Redirect ||
						 rsp.StatusCode == HttpStatusCode.Found || rsp.StatusCode == HttpStatusCode.Moved ||
						 rsp.StatusCode == HttpStatusCode.MovedPermanently || rsp.StatusCode == HttpStatusCode.Ambiguous ||
						 rsp.StatusCode == HttpStatusCode.MultipleChoices)
				{
					MultipleChoices++;
				} //3xx错误
				else if(rsp.StatusCode == HttpStatusCode.SwitchingProtocols || rsp.StatusCode == HttpStatusCode.Continue)
				{
					Continue++;
				} //1xx错误

				#endregion

				#region 关于错误的返回

				if(GatewayTimeout >= 1)
				{
					str = "504";
				} //504错误处理方式
				else if(InternalServerError >= 1)
				{
					str = "5xx";
				} //5xx错误处理方式
				else if(NotFound >= 1)
				{
					str = "404";
				} //404错误处理方式
				else if(Forbidden >= 1)
				{
					str = "403";
				} //403错误处理方式
				else if(Unauthorized >= 1)
				{
					str = "401";
				} //401错误处理方式
				else if(PaymentRequired >= 1)
				{
					str = "4x";
					GatewayTimeout = 0;
					InternalServerError = 0;
					NotFound = 0;
					Forbidden = 0;
					Unauthorized = 0;
					BadRequest++;
					MultipleChoices = 0;
					Continue = 0;
				} //4xx错误第一规则处理方式
				else if(BadRequest >= 1)
				{
					str = "4xx";
				} //4xx错误处理方式
				else if(MultipleChoices >= 1)
				{
					str = "3xx";
				} //3xx错误处理方式
				else if(Continue >= 1)
				{
					str = "1xx";
				} //1xx错误处理方式

				#endregion
			}
			return str;
		}
	}
}
