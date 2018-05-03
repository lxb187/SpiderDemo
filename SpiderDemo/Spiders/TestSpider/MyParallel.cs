#region 版权信息
/* ======================================================================== 
 * 描述信息   
 * 作者：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间：2018年4月4日15:38:01
 * CLR：4.0.30319.42000 
 * 功能描述：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using SpiderDemo.Model;
using SpiderHelp.ConfigModule;
using SpiderHelp.ExtStaticModule;
using SpiderHelp.SaveModule;

namespace SpiderDemo.Spiders.TestSpider
{  
    /// <summary>
    /// 我的并发类
    /// </summary>
    public class MyParallel
    {
        /// <summary>
        /// 接收GrabAllInfo声明的静态爬虫配置类对象
        /// </summary>
        private static MySpiderConfig StaticConfig = GrabAllInfo.StaticConfig;
        /// <summary>
        /// 爬虫任务类
        /// </summary>
        public TaskUrlConfig TaskUrl = new TaskUrlConfig();
        /// <summary>
        /// 爬虫代理类
        /// </summary>
        public ProxyConfig ProxyConfig = new ProxyConfig();
        /// <summary>
        /// 爬虫账号Cookie类
        /// </summary>
        public CookieConfig CkConfig = new CookieConfig();
        /// <summary>
        /// 构造函数
        /// </summary>
        public MyParallel()
        {
            
        }
        /// <summary>
        /// 带参类构造函数
        /// </summary>
        /// <param name="myParallel">我的并发类对象</param>
        public MyParallel(MyParallel myParallel)
        {
            TaskUrl = myParallel.TaskUrl;
            ProxyConfig = myParallel.ProxyConfig;
            CkConfig = myParallel.CkConfig;
        }
        /// <summary>
        /// 请求方法
        /// </summary>
        public void MyRequest()
        {
            lock (TaskUrl)
            {
                switch (TaskUrl.Tab)
                {
                    case "list":
                        Request_List();
                        break;
                    case "base":
                        Request_Base();
                        break;
                    default:
                        Request_Extra();
                        break;
                }
            }
        }
        /// <summary>
        /// 获取未知名模板数据
        /// </summary>
        private void Request_Extra()
        {
            lock (TaskUrl)
            {
                WriteRequestResult("未知名模板");
            }
        }

        /// <summary>
        /// 获取详情数据
        /// </summary>
        private void Request_Base()
        {
            lock (TaskUrl)
            {
                HttpWebResponse response = null;
                try
                {
                    #region 请求
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TaskUrl.Url);
                    request.Timeout = 10000;
                    request.ReadWriteTimeout = 12000;
                    request.KeepAlive = false;
                    request.AllowAutoRedirect = false;
                    request.ServicePoint.Expect100Continue = false;//加快载入速度
                    request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                    request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                    request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                    request.UserAgent =
                        "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.91 Safari/537.36";
                    request.Accept =
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                    request.Headers.Add("DNT", @"1");
                    request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                    request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
                    request.Headers.Set(HttpRequestHeader.Cookie, CkConfig.Cookie);
                    if (ProxyConfig.Proxy != null && ProxyConfig.Proxy.Address.Authority != "192.168.1.1")
                    {
                        request.Proxy = ProxyConfig.Proxy;
                    }
                    ///TODO:添加代理身份验证（2018年4月4日14:58:47）
                    request.Proxy.Credentials = new NetworkCredential(ProxyConfig.ProxyUser, ProxyConfig.ProxyPass);
                    response = (HttpWebResponse)request.GetResponse();
                    #endregion

                    ///TODO:标准化处理200代码
                    StaticConfig.CountConfig.ErrorCodeStr = StaticConfig.ErrorCode.HandleMethod(response);
                    string html = response.ResponseHtml();
                    //判断是否含有该特征标志
                    HtmlNode pageNode =
                        MyConvert.ToHtmlNode(html).SelectSingleNode("//*[@class='hanweidasha cl zk']//*[@class='right-hanweidasha fr cl']");
                    if (pageNode != null)
                    {
                        StaticConfig.CountConfig.Itrue++;
                        string fileName = $"{StaticConfig.Spiderinfo.SiteName}#{TaskUrl.Tab}#{TaskUrl.Md5}.html";
                        string path =
                            $"{StaticConfig.SpConfig.PathSign}\\{DateTime.Now:yyyyMMdd}\\{StaticConfig.Spiderinfo.SiteName}\\{TaskUrl.Tab}\\{DateTime.Now:HH}";
                        //该方法可以改用 FileIoHelp.FileDown();                           
                        if (ReadToFile(html, fileName, path))
                        {
                            //写入错误计数清零
                            StaticConfig.ErrorCode.IwriteError = 0;
                            ///TODO：同一页面的多个模板分开存储数量切换为入库量计算
                            StaticConfig.Spiderinfo.StoreMount++;
                        }
                        else
                        {
                            //写入错误计数
                            StaticConfig.ErrorCode.IwriteError++;
                        }

                        #region 记录任务状态 和写入爬虫监控

                        TaskUrl.IState = 1;
                        TaskUrl.Done_time = DateTime.Now;
                        lock (StaticConfig.TaskUrls)
                        {
                            StaticConfig.TaskUrls.Add(TaskUrl);
                        }
                        //转换编码
                        Encoding gb = System.Text.Encoding.GetEncoding("utf-8");
                        //获取字节数组
                        byte[] bytes = gb.GetBytes(html);
                        ///TODO:统计成功请求数据的字节长度
                        StaticConfig.Spiderinfo.TotalLength += bytes.Length;
                        ///TODO:统计成功抓取量
                        StaticConfig.Spiderinfo.CatchMount++;

                        #endregion

                        //解析出错次数清零
                        StaticConfig.ErrorCode.IanalysisError = 0;
                        //请求无数据次数清零
                        StaticConfig.ErrorCode.IzeroError = 0;
                        //请求为空次数清零
                        StaticConfig.ErrorCode.InullError = 0;
                        //请求模板异常次数清零
                        StaticConfig.ErrorCode.ItabError = 0;
                    }
                    else if (string.IsNullOrWhiteSpace(html))
                    {
                        StaticConfig.ErrorCode.InullError++;
                        WriteRequestResult("请求为空");
                    }
                    else if (html.Contains("alert(\'温馨提醒,该信息不存在,可能已经被删除\');"))
                    {
                        TaskUrl.IState = 2;
                        TaskUrl.Done_time = DateTime.Now;
                        lock (StaticConfig.TaskUrls)
                        {
                            StaticConfig.TaskUrls.Add(TaskUrl);
                        }
                        WriteRequestResult("无数据");
                    }
                    else
                    {
                        StaticConfig.ErrorCode.ItabError++;
                        TaskUrl.IState = -1;
                        TaskUrl.Done_time = DateTime.Now;
                        lock (StaticConfig.TaskUrls)
                        {
                            StaticConfig.TaskUrls.Add(TaskUrl);
                        }
                        #region 特殊异常存储
                        string fileNameError = $"{StaticConfig.Spiderinfo.SiteName}#{TaskUrl.Tab}#{TaskUrl.Md5}.html";
                        string pathError = $"{StaticConfig.SpConfig.PathSign}\\Others\\{StaticConfig.Spiderinfo.SiteName}异常\\{DateTime.Now:yyyyMMdd}\\{TaskUrl.Tab}";
                        if (ReadToFile(html, fileNameError, pathError))
                        {
                            StaticConfig.ErrorCode.IwriteError = 0;
                        }
                        else
                        {
                            StaticConfig.ErrorCode.IwriteError++;
                        }
                        #endregion
                        WriteRequestResult("特殊异常");
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        response = (HttpWebResponse)ex.Response;
                        #region 标准化处理XXX代码

                        StaticConfig.CountConfig.ErrorCodeStr = StaticConfig.ErrorCode.HandleMethod(response);
                        if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            StaticConfig.CountConfig.Ifasle++;
                            TaskUrl.IState = 500;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            StaticConfig.CountConfig.Ifasle++;
                            TaskUrl.IState = 403;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            StaticConfig.CountConfig.Ifasle++;
                            TaskUrl.IState = 404;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                        }

                        #endregion
                    }
                    WriteRequestResult(ex.Message);
                }
                catch (Exception ex)
                {
                    #region 非服务器反馈异常
                    StaticConfig.CountConfig.Ifasle++;
                    StaticConfig.ErrorCode.IanalysisError++;
                    TaskUrl.IState = 3;
                    TaskUrl.Done_time = DateTime.Now;
                    lock (StaticConfig.TaskUrls)
                    {
                        StaticConfig.TaskUrls.Add(TaskUrl);
                    }
                    #endregion
                    WriteRequestResult(ex.Message);
                }
                finally
                {
                    response?.Close();
                }
            }
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        private void Request_List()
        {
            lock (TaskUrl)
            {
                HttpWebResponse response = null;
                try
                {
                    #region 请求
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TaskUrl.Url);
                    request.Timeout = 10000;
                    request.ReadWriteTimeout = 12000;
                    request.KeepAlive = false;
                    request.AllowAutoRedirect = false;
                    request.ServicePoint.Expect100Continue = false;//加快载入速度
                    request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                    request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                    request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                    request.UserAgent =
                        "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.91 Safari/537.36";
                    request.Accept =
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                    request.Headers.Add("DNT", @"1");
                    request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                    request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
                    request.Headers.Set(HttpRequestHeader.Cookie, CkConfig.Cookie);
                    if (ProxyConfig.Proxy != null && ProxyConfig.Proxy.Address.Authority != "192.168.1.1")
                    {
                        request.Proxy = ProxyConfig.Proxy;
                    }
                    ///TODO:添加代理身份验证（2018年4月4日14:58:47）
                    request.Proxy.Credentials = new NetworkCredential(ProxyConfig.ProxyUser, ProxyConfig.ProxyPass);
                    response = (HttpWebResponse)request.GetResponse();
                    #endregion

                    ///TODO:标准化处理200代码
                    StaticConfig.CountConfig.ErrorCodeStr = StaticConfig.ErrorCode.HandleMethod(response);
                    string html = response.ResponseHtml();
                    //判断是否含有该特征标志
                    HtmlNode pageNode =
                        MyConvert.ToHtmlNode(html).SelectSingleNode("//*[@class='chushou-shangpu cl zk']//*[@class='toubu-chushou-shangpu cl']");
                    if (pageNode != null)
                    {
                        StaticConfig.CountConfig.Itrue++;
                        HtmlNodeCollection pageNodes =
                            MyConvert.ToHtmlNode(html).SelectNodes("//*[@class='wen fr']//a");
                        if (pageNodes != null)
                        {
                            foreach (var item in pageNodes)
                            {
                                //加锁防止资源争夺
                                lock (StaticConfig.AddTaskUrls)
                                {
                                    string urlInfo = item.Attributes["href"].Value;
                                    TaskUrlConfig taskUrl = new TaskUrlConfig
                                    {
                                        Uid = TaskUrl.Uid,
                                        CompanyName = item.InnerText,
                                        Tab = "base",
                                        Url = urlInfo,
                                        Md5 = MyConvert.ToUserMd5(urlInfo),
                                        Method = "get",
                                        Queue_time = DateTime.Now,
                                        Done_time = DateTime.Now,
                                        ICount = 0,
                                        IState = 0
                                    };
                                    StaticConfig.AddTaskUrls.Add(taskUrl);
                                }
                            }
                            #region list不存储
                            //string fileName = $"{StaticConfig.Spiderinfo.SiteName}#{TaskUrl.Tab}#{TaskUrl.Md5}.html";
                            //string path =
                            //    $"{StaticConfig.SpConfig.PathSign}\\{DateTime.Now.ToString("yyyyMMdd")}\\{StaticConfig.Spiderinfo.SiteName}\\{TaskUrl.Tab}\\{DateTime.Now.ToString("HH")}";
                            ////该方法可以改用 FileIoHelp.FileDown();                           
                            //if (ReadToFile(html, fileName, path))
                            //{
                            //    //写入错误计数清零
                            //    StaticConfig.ErrorCode.IwriteError = 0;
                            //    ///TODO：同一页面的多个模板分开存储数量切换为入库量计算
                            //    StaticConfig.Spiderinfo.StoreMount++;
                            //}
                            //else
                            //{
                            //    //写入错误计数
                            //    StaticConfig.ErrorCode.IwriteError++;
                            //}
                            #endregion

                            #region 记录任务状态 和写入爬虫监控
                            TaskUrl.IState = 1;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                            #endregion
                        }
                        else
                        {
                            TaskUrl.IState = 3;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                            //解析出错次数清零
                            StaticConfig.ErrorCode.IanalysisError++;                         
                            WriteRequestResult("解析出错");
                        }
                        //转换编码
                        Encoding gb = System.Text.Encoding.GetEncoding("utf-8");
                        //获取字节数组
                        byte[] bytes = gb.GetBytes(html); 
                        ///TODO:统计成功请求数据的字节长度
                        StaticConfig.Spiderinfo.TotalLength += bytes.Length;
                        ///TODO:统计成功抓取量
                        StaticConfig.Spiderinfo.CatchMount++;

                        //解析出错次数清零
                        StaticConfig.ErrorCode.IanalysisError = 0;
                        //请求无数据次数清零
                        StaticConfig.ErrorCode.IzeroError = 0;
                        //请求为空次数清零
                        StaticConfig.ErrorCode.InullError = 0;
                        //请求模板异常次数清零
                        StaticConfig.ErrorCode.ItabError = 0;
                    }
                    else if (string.IsNullOrWhiteSpace(html))
                    {
                        StaticConfig.ErrorCode.InullError++;
                        WriteRequestResult("请求为空");
                    }
                    else if (html.Contains("alert(\'温馨提醒,该信息不存在,可能已经被删除\');"))
                    {
                        TaskUrl.IState = 2;
                        TaskUrl.Done_time = DateTime.Now;
                        lock (StaticConfig.TaskUrls)
                        {
                            StaticConfig.TaskUrls.Add(TaskUrl);
                        }
                        WriteRequestResult("无数据");
                    }
                    else
                    {
                        StaticConfig.ErrorCode.ItabError++;
                        TaskUrl.IState = -1;
                        TaskUrl.Done_time = DateTime.Now;
                        lock (StaticConfig.TaskUrls)
                        {
                            StaticConfig.TaskUrls.Add(TaskUrl);
                        }
                        #region 特殊异常存储
                        string fileNameError = $"{StaticConfig.Spiderinfo.SiteName}#{TaskUrl.Tab}#{TaskUrl.Md5}.html";
                        string pathError = $"{StaticConfig.SpConfig.PathSign}\\Others\\{StaticConfig.Spiderinfo.SiteName}异常\\{DateTime.Now:yyyyMMdd}\\{TaskUrl.Tab}";
                        if (ReadToFile(html, fileNameError, pathError))
                        {
                            StaticConfig.ErrorCode.IwriteError = 0;
                        }
                        else
                        {
                            StaticConfig.ErrorCode.IwriteError++;
                        }
                        #endregion
                        WriteRequestResult("特殊异常");
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        response = (HttpWebResponse)ex.Response;
                        #region 标准化处理XXX代码

                        StaticConfig.CountConfig.ErrorCodeStr = StaticConfig.ErrorCode.HandleMethod(response);
                        if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            StaticConfig.CountConfig.Ifasle++;
                            TaskUrl.IState = 500;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            StaticConfig.CountConfig.Ifasle++;
                            TaskUrl.IState = 403;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            StaticConfig.CountConfig.Ifasle++;
                            TaskUrl.IState = 404;
                            TaskUrl.Done_time = DateTime.Now;
                            lock (StaticConfig.TaskUrls)
                            {
                                StaticConfig.TaskUrls.Add(TaskUrl);
                            }
                        }

                        #endregion
                    }
                    WriteRequestResult(ex.Message);
                }
                catch (Exception ex)
                {
                    #region 非服务器反馈异常
                    StaticConfig.CountConfig.Ifasle++;
                    StaticConfig.ErrorCode.IanalysisError++;
                    TaskUrl.IState = 3;
                    TaskUrl.Done_time = DateTime.Now;
                    lock (StaticConfig.TaskUrls)
                    {
                        StaticConfig.TaskUrls.Add(TaskUrl);
                    }
                    #endregion
                    WriteRequestResult(ex.Message);
                }
                finally
                {
                    response?.Close();
                }
            }
        }

        /// <summary>
        /// 文件写入，尝试三次
        /// </summary>
        /// <param name="html">HTML内容</param>
        /// <param name="fileName">文件名带后缀</param>
        /// <param name="path">文件路径</param>
        /// <returns>是否成功写入</returns>
        private bool ReadToFile(string html, string fileName, string path)
        {
            bool flg = false;
            #region 保存文件
            int tryNum = 3;
            while(tryNum > 0)
            {
                try
                {
                    FileIoHelp.WriteFile(path, fileName, html);
                    flg = true;
                    Console.WriteLine($@"【成功保存】>>>【{path}\\{fileName}>>>{DateTime.Now}】");
                    tryNum = 0;
                }
                catch(Exception ex)
                {
                    tryNum--;
                    Console.WriteLine($@"存储出错：{ex.Message}>>>{DateTime.Now}");
                }
            }
            #endregion
            return flg;
        }

        /// <summary>
        /// 输入请求结果信息
        /// </summary>
        /// <param name="meInfo">消息</param>
        private void WriteRequestResult(string meInfo)
        {
            lock (TaskUrl)
            {
                Console.WriteLine(@"【{0}】>>>【{1}】>>>【{2}】>>>【{3}】>>>{4}", TaskUrl.Id, TaskUrl.Tab,
                    TaskUrl.IState, meInfo, DateTime.Now);
            }
        }
    }
}
