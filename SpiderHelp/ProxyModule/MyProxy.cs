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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SpiderHelp.ExtStaticModule;

namespace SpiderHelp.ProxyModule
{
    /// <summary>
    /// 常用代理方法类
    /// </summary>
    public class MyProxy
    {
        /// <summary>
        /// 获取阿布云代理
        /// </summary>
        /// <param name="proxyUser">账号</param>
        /// <param name="proxyPass">密码</param>
        /// <returns></returns>
        public WebProxy GetAbyProxy(string proxyUser, string proxyPass)
        {
            // 代理服务器
            string proxyHost = "http://http-dyn.abuyun.com";
            string proxyPort = "9020";

            // 代理隧道验证信息
            var proxy = new WebProxy
            {
                Address = new Uri(string.Format("{0}:{1}", proxyHost, proxyPort)),
                Credentials = new NetworkCredential(proxyUser, proxyPass)
            };
            ServicePointManager.Expect100Continue = false;
            return proxy;
        }

        // 当站大爷该API今日请求HTTP次数已经超限时
        // 返回一个特殊的Ip,http://vip.zdaye.com
        /// <summary>
        /// 在线获取站大爷私密IP
        /// </summary>
        /// <param name="num">获取代理IP数量</param>
        /// <returns></returns>
        public static Stack<WebProxy> ZdyPrivateIp(int num = 50)
        {
            Stack<WebProxy> sip = new Stack<WebProxy>();
            int itryMax = 3;

            while(itryMax > 0)
            {
                try
                {
                    string html = "";
                    if(Request_zdayeApi(out var response, num))
                    {
                        html = response.ResponseHtml(Encoding.GetEncoding("gb2312"));
                    }
                    if(!string.IsNullOrEmpty(html))
                    {
                        if(html.Contains("该API今日请求HTTP次数已经超限"))
                        {
                            sip = new Stack<WebProxy>();
                            sip.Push(new WebProxy("http://vip.zdaye.com"));
                        }
                        else
                        {
                            List<string> IPs =
                            html.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            for(int i = 0; i < IPs.Count(); i++)
                            {
                                sip.Push(new WebProxy(IPs[i]));
                            }
                        }
                    }
                    itryMax = 0;
                }
                catch
                {
                    itryMax--;
                }
                finally
                {
                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, 3));
                }
            }
            return sip;
        }
        private static bool Request_zdayeApi(out HttpWebResponse response, int num)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://vip.zdaye.com/?api=201801030958349137&count={0}&gb=0&fitter=2&px=2", num));
                request.Timeout = 6000;
                request.ReadWriteTimeout = 8000;
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.108 Safari/537.36";
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9");

                //request.Headers.Set(HttpRequestHeader.Cookie, @"ASPSESSIONIDSQTCQSDT=IFNAOJNBMFFKMCJMPIFMDJDO");             
                //TODO:添加代理身份验证（2018年1月3日10:10:05）
                request.Proxy.Credentials = new NetworkCredential("201801030958349137", "35855996");
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(WebException e)
            {
                if(e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch(Exception)
            {
                if(response != null) response.Close();
                return false;
            }
            return true;
        }
    }
}
