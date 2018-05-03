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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using SpiderDemo.Bll;
using SpiderDemo.DAL;
using SpiderDemo.Interfaces;
using SpiderDemo.Model;
using SpiderHelp.ConfigModule;
using SpiderHelp.MonitorModule;
using SpiderHelp.SaveModule;

namespace SpiderDemo.Spiders.TestSpider
{
    /// <summary>
    /// 爬虫全维度抓取
    /// </summary>
    public class GrabAllInfo:ISpider
    {
        /// <summary>
        /// 任务源表名
        /// </summary>
        private const string ActionTable = "XXX";
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private static readonly IndexBll spideBll = new IndexBll(System.Configuration.ConfigurationManager.AppSettings["Spider_Mysql_Ali"]);
        /// <summary>
        /// 爬虫静态辅助配置类
        /// </summary>
        public static MySpiderConfig StaticConfig = new MySpiderConfig();

        /// <summary>
        /// 遍历任务列表抓取
        /// </summary>
        public void Start()
        {          
            //TODO：爬虫配置信息初始化，检测是否异常
            InitSpider();
            Console.WriteLine(@"当前并发数量>>>【{0}】>>>休息间隔【{1}】>>>{2}", StaticConfig.SpConfig.ActionLssNum, StaticConfig.SpConfig.ActionSleepTime, DateTime.Now);
            int idy = 0;
            int tryMax = 0;
            do
            {
                try
                {
                    //TODO：获取爬虫任务源
                    List<TaskUrlConfig> allInfoUrls =
                        GetTask(StaticConfig.SpConfig.StartNum, StaticConfig.SpConfig.EndNum);
                    Console.WriteLine(@"共计任务：【{0}】 >>>{1}", allInfoUrls.Count, DateTime.Now);
                    if (allInfoUrls.Count <= 0)
                    {
                        StaticConfig.Spiderinfo.Flg = false;
                        MessageEail("无任务,每月4号开启");
                        SpiderHelp.SaveModule.CLog.DiaryLog("无任务,每月4号开启",
                            $"\\{StaticConfig.Spiderinfo.TaskName}(启动记录)\\{StaticConfig.Spiderinfo.TaskName}(启动记录)_{DateTime.Now:yyyyMMdd}.txt");
                        do
                        {
                            //控制每个月4号开启
                            if (DateTime.Now.Day == 4)
                            {
                                if (idy == 0)
                                {
                                    idy++;

                                    #region 更新任务

                                    string sqlUpdate =
                                        $"UPDATE {ActionTable} SET IState=0,Queue_time=now() WHERE Tab='XXX'";
                                    int itryMax = 3;
                                    do
                                    {
                                        int iflgx = SpiderHelp.SaveModule.MySqlHelp.Update(
                                            StaticConfig.SpConfig.AliConstr, sqlUpdate);
                                        if (iflgx >= 0)
                                        {
                                            itryMax = 0;
                                        }
                                        else
                                        {
                                            itryMax--;
                                        }

                                        Console.WriteLine("更新：【{0}】>>>{1}", iflgx, DateTime.Now);
                                    } while (itryMax > 0);

                                    #endregion

                                    StaticConfig.SpConfig.DateKs = DateTime.Now;
                                    StaticConfig.CountConfig.Itrue = 0;
                                    StaticConfig.CountConfig.Ifasle = 0;
                                    StaticConfig.CountConfig.Itotal = 0;
                                    StaticConfig.Spiderinfo.Flg = true;
                                    break;
                                }
                            }
                            else
                            {
                                idy = 0;
                                Console.Write("休息30分钟......");
                                System.Threading.Thread.Sleep(new TimeSpan(0, 30, 0));
                                Console.WriteLine("结束休息");
                            }
                        } while (true);
                    }
                    //TODO：执行相应的任务，包括任务回收和异常处理
                    DoTask(allInfoUrls);
                }
                catch (Exception ex)
                {
                    SpiderHelp.SaveModule.CLog.DiaryLog(ex.Message,
                        $"\\{StaticConfig.Spiderinfo.TaskName}(启动出错)\\{StaticConfig.Spiderinfo.TaskName}(启动出错)_{DateTime.Now:yyyyMMdd}.txt");

                    Console.WriteLine("启动出错：{0}>>>{1}", ex.Message, DateTime.Now);
                    Console.Write("{0} 休息10分钟......", DateTime.Now);
                    System.Threading.Thread.Sleep(new TimeSpan(0, 10, 0));
                    Console.WriteLine("结束休息......{0}", DateTime.Now);
                    tryMax++;
                    if (tryMax > 144)
                    {
                        SpiderHelp.SaveModule.CLog.DiaryLog(ex.Message,
                            $"\\{StaticConfig.Spiderinfo.TaskName}(启动出错)\\{StaticConfig.Spiderinfo.TaskName}(启动异常超144次)_{DateTime.Now:yyyyMMdd}.txt");
                        MessageEail("启动异常超144次", 1);
                        System.Environment.Exit(0);
                    }
                }
            } while (true);
        }

        /// <summary>
        /// 初始化爬虫
        /// </summary>
        public void InitSpider()
        {
            try
            {
                #region 爬虫配置初始化
                StaticConfig.SpConfig.PcName = System.Configuration.ConfigurationManager.AppSettings["Spider_Action_Ip"];
                StaticConfig.SpConfig.ActionEmail = System.Configuration.ConfigurationManager.AppSettings["Spider_Action_Email"];
                StaticConfig.SpConfig.ActionIp = System.Configuration.ConfigurationManager.AppSettings["Spider_Action_Ip"];
                StaticConfig.SpConfig.ActionLssNum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Spider_Action_LssNum"]);
                StaticConfig.SpConfig.PathSign = System.Configuration.ConfigurationManager.AppSettings["Spider_Action_PathSign"];
                StaticConfig.SpConfig.ResourcesPathSign = System.Configuration.ConfigurationManager.AppSettings["Spider_Action_ResourcesPathSign"];
                StaticConfig.SpConfig.ActionSleepTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Spider_Action_SleepTime"]);
                StaticConfig.SpConfig.StartNum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Spider_Action_StartNum"]);
                StaticConfig.SpConfig.EndNum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Spider_Action_EndNum"]);
                StaticConfig.SpConfig.StartCk = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Spider_Action_StartCk"]);
                StaticConfig.SpConfig.EndCk = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Spider_Action_EndCk"]);
                StaticConfig.SpConfig.AliConstr = System.Configuration.ConfigurationManager.AppSettings["Spider_Mysql_Ali"];
                StaticConfig.SpConfig.TakskDataConstr = System.Configuration.ConfigurationManager.AppSettings["Spider_Mysql_TaskData"];
                StaticConfig.SpConfig.ActionTab = System.Configuration.ConfigurationManager.AppSettings["Spider_Action_Tab"];
                StaticConfig.SpConfig.ActionTabFlg = System.Configuration.ConfigurationManager.AppSettings["Spider_Action_TabFlg"];
                StaticConfig.SpConfig.DateKs = DateTime.Now;
                #endregion

                #region 爬虫监控初始化
                StaticConfig.Spiderinfo.SpiderProgramName = string.Format("XXX{0}-{1})-lxb@jiuweiwang.com", StaticConfig.SpConfig.StartNum, StaticConfig.SpConfig.EndNum);
                StaticConfig.Spiderinfo.TaskName = string.Format("XXX({0}-{1})", StaticConfig.SpConfig.StartNum, StaticConfig.SpConfig.EndNum);
                StaticConfig.Spiderinfo.SiteName = "XXX";
                StaticConfig.Spiderinfo.TemplateName = "XXX";
                StaticConfig.Spiderinfo.IPAddress = StaticConfig.SpConfig.ActionIp;
                StaticConfig.Spiderinfo.Ext = "XXX";
                StaticConfig.Spiderinfo.Flg = true;
                //执行爬虫自动统计到数据监控系统
                StaticConfig.Spiderinfo.AutomaticCount();
                #endregion

                #region 检测存储路径是否存在
                if (!System.IO.Directory.Exists(StaticConfig.SpConfig.PathSign.Split('\\').First()))
                {
                    Console.WriteLine($@"不存在路径：{StaticConfig.SpConfig.PathSign}\\{StaticConfig.Spiderinfo.SiteName}");
                    SpiderHelp.MessageModule.Email.SendSimpleEmail(StaticConfig.SpConfig.ActionEmail, StaticConfig.Spiderinfo.TaskName + "（路径不存在结束）",
                        StaticConfig.SpConfig.PathSign + "\\" + StaticConfig.Spiderinfo.SiteName + ">>>当前计算机：" + StaticConfig.SpConfig.PcName);
                    System.Environment.Exit(0);
                }
                #endregion

                SpiderHelp.SaveModule.CLog.DiaryLog("爬虫启动初始化成功",
                    $"\\{StaticConfig.Spiderinfo.TaskName}(启动记录)\\{StaticConfig.Spiderinfo.TaskName}(启动记录)_{DateTime.Now:yyyyMMdd}.txt");
            }
            catch (Exception ex)
            {
                SpiderHelp.SaveModule.CLog.DiaryLog($"{ex.Message}>>>爬虫初始化异常，程序终止",
                    $"\\{StaticConfig.Spiderinfo.TaskName}(启动记录)\\{StaticConfig.Spiderinfo.TaskName}(启动记录)_{DateTime.Now:yyyyMMdd}.txt");
                MessageEail("爬虫初始化异常");
                System.Environment.Exit(0);
            }              
        }

        /// <summary>
        /// 获取任务类
        /// </summary>
        /// <param name="startNum">起始id</param>
        /// <param name="endNum">结束id</param>
        /// <returns>任务类集合</returns>
        public List<TaskUrlConfig> GetTask(int startNum, int endNum)
        {
            List<TaskUrlConfig> allInfoUrls = new List<TaskUrlConfig>();
            int itryMax = 3;
            do
            {
                //这个可以根据需求自己调整
                string sql;
                if (StaticConfig.SpConfig.ActionTab == "allinfo")
                {
                    sql = $"SELECT * FROM {ActionTable} WHERE IState=0 LIMIT {startNum},{endNum}";
                }
                else
                {
                    sql = $"SELECT * FROM {ActionTable} WHERE Tab='{StaticConfig.SpConfig.ActionTab}' and IState=0 LIMIT {startNum},{endNum}";
                }
                System.Data.DataTable dt = spideBll.Select(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TaskUrlConfig company = new TaskUrlConfig
                        {
                            Id = int.Parse(dt.Rows[i]["Id"].ToString()),
                            Uid = dt.Rows[i]["Uid"].ToString(),
                            CompanyName = dt.Rows[i]["CompanyName"].ToString(),
                            Tab = dt.Rows[i]["Tab"].ToString(),
                            Method = dt.Rows[i]["Method"].ToString(),
                            Url = dt.Rows[i]["Url"].ToString(),
                            Md5 = dt.Rows[i]["Md5"].ToString(),
                            ICount = int.Parse(dt.Rows[i]["ICount"].ToString()),
                            IState = int.Parse(dt.Rows[i]["IState"].ToString())
                        };
                        try
                        {
                            company.Queue_time = Convert.ToDateTime(dt.Rows[i]["Queue_time"].ToString());
                            company.Done_time = Convert.ToDateTime(dt.Rows[i]["Done_time"].ToString());
                        }
                        catch
                        {
                            company.Queue_time = DateTime.Now;
                            company.Done_time = DateTime.Now;
                        }
                        allInfoUrls.Add(company);
                    }
                    itryMax = 0;
                    StaticConfig.ErrorCode.IdbError = 0;
                }
                else if (dt == null)
                {
                    itryMax--;
                    StaticConfig.ErrorCode.IdbError++;
                }
                else
                {
                    itryMax = 0;
                    StaticConfig.ErrorCode.IdbError = 0;
                }
            } while (itryMax > 0);
            return allInfoUrls;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="allInfoUrls">任务源集合</param>
        public void DoTask(List<TaskUrlConfig> allInfoUrls)
        {
            List<Action> lss = new List<Action>();
            for (int i = 0; i < allInfoUrls.Count; i++)
            {
                //爬虫请求次数计数
                StaticConfig.Spiderinfo.RequestMount++;
                StaticConfig.CountConfig.Itotal++;
                MyParallel grabAllInfo = new MyParallel
                {
                    //接收任务对象
                    TaskUrl = allInfoUrls[i],
                    //代理初始化，默认使用本地代理
                    //需要用到阿布云代理的，可以开启fiddler代理或者把本地代理改成阿布云的代理
                    ProxyConfig =
                    {
                        // [-or-] 本地 fiddler代理阿布云                               
                        Proxy = new WebProxy("192.168.1.1"),
                        // [-or-] 阿布云
                        //Proxy = new WebProxy("http://http-dyn.abuyun.com:9020"),
                        ProxyUser = "HZ783119Z786X1UD",
                        ProxyPass = "BE35B11C1932B6CC"
                    },
                    //初始化账号Cookie信息，下面为不需要用到账号的情况
                    CkConfig=
                    {
                        Cookie = ""
                    }
                };
                lss.Add(new MyParallel(grabAllInfo).MyRequest);
                //此处控制并发数量，并发数量可以在配置文件设置
                if (i % StaticConfig.SpConfig.ActionLssNum == StaticConfig.SpConfig.ActionLssNum - 1 ||
                    i == allInfoUrls.Count - 1)
                {
                    try
                    {
                        //开始并发
                        System.Threading.Tasks.Parallel.Invoke(lss.ToArray());
                        Console.WriteLine("***********************************************************");
                        //TODO：回收任务
                        GcTask(StaticConfig.TaskUrls, StaticConfig.AddTaskUrls);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("并发出错：{0}>>>{1}", ex.Message, DateTime.Now);
                        CLog.DiaryLog(ex.Message,
                            $"\\{StaticConfig.Spiderinfo.TaskName}并发出错\\{StaticConfig.Spiderinfo.TaskName}并发出错_{DateTime.Now:yyyyMMdd}.txt");
                        System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
                    }
                    //初始化变量
                    lss = new List<Action>();
                    StaticConfig.TaskUrls = new List<TaskUrlConfig>();
                    StaticConfig.AddTaskUrls = new List<TaskUrlConfig>();
                    System.Threading.Thread.Sleep(new TimeSpan(0, 0, StaticConfig.SpConfig.ActionSleepTime));
                    System.GC.Collect();

                    //TODO：发送请求异常处理的邮件通知
                    FixException(StaticConfig.SpConfig.ActionEmail,
                        StaticConfig.Spiderinfo.TaskName, StaticConfig.CountConfig.ErrorCodeStr);
                    Console.WriteLine("***********************************************************");
                }
                //程序界面头部显示的信息，方便查看数据情况
                StaticConfig.CountConfig.EMessage = string.Format("{0}[{1:MMddHHmm}]【{2}/{3}>{4:0.000}-{5}】[{6}/{7}]",
                    StaticConfig.Spiderinfo.TaskName, StaticConfig.SpConfig.DateKs, StaticConfig.CountConfig.Itrue,
                    StaticConfig.CountConfig.Itotal,
                    ((double)StaticConfig.CountConfig.Itrue / StaticConfig.CountConfig.Itotal),
                    StaticConfig.CountConfig.Ifasle, i, allInfoUrls.Count);
                Console.Title = StaticConfig.CountConfig.EMessage;
                #region 通知邮件（每8小时统计一次）
                //可以根据实际情况选择性给自己发邮件的，不强制
                if (DateTime.Now.Hour % 8 == 0 && DateTime.Now.Minute == 0)
                {
                    MessageEail("每8个小时反馈");
                }
                #endregion
            }
        }

        /// <summary>
        /// 回收任务
        /// </summary>
        /// <param name="taskUrls">任务源类集合</param>
        /// <param name="addTaskUrls">新增任务集合</param>
        public void GcTask(List<TaskUrlConfig> taskUrls, List<TaskUrlConfig> addTaskUrls)
        {
            #region 任务增加
            if (addTaskUrls.Count > 0)
            {
                Console.WriteLine("任务增加:【{0}】>>>{1}", addTaskUrls.Count, DateTime.Now);
                string insertSql = "";
                foreach (var item in addTaskUrls)
                {
                    insertSql += string.Format(
                        "INSERT IGNORE INTO {0}(CompanyName,Uid,Tab,Url,Md5,Method,ICount,IState,Queue_time) values('{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},'{9}') ON DUPLICATE KEY UPDATE CompanyName='{1}',Uid='{2}',Tab='{3}',Url='{4}',Md5='{5}',Method='{6}',ICount={7},IState={8},Queue_time='{9}';",
                        ActionTable, item.CompanyName,
                        item.Uid, item.Tab, item.Url, item.Md5,
                        item.Method, item.ICount, item.IState, item.Queue_time);
                }

                if (!string.IsNullOrWhiteSpace(insertSql))
                {
                    int itryMax = 3;
                    while (itryMax > 0)
                    {
                        int iflg = spideBll.Insert(insertSql);
                        if (iflg >= 0)
                        {
                            itryMax = 0;
                        }
                        else
                        {
                            itryMax--;
                        }

                        Console.WriteLine("增加任务:【{0}】>>>{1}", iflg, DateTime.Now);
                    }
                }
            }
            #endregion

            #region 任务状态反馈
            if (taskUrls.Count > 0)
            {
                Console.WriteLine("任务回收:【{0}】>>>{1}", taskUrls.Count, DateTime.Now);
                string updateSql = string.Empty;
                foreach (var item in taskUrls)
                {
                    updateSql +=
                        string.Format(
                            "UPDATE {3} SET IState={0},Done_time='{1}' WHERE Md5='{2}';", item.IState,
                            item.Done_time, item.Md5, ActionTable);
                }

                if (!string.IsNullOrWhiteSpace(updateSql))
                {
                    int itryMax = 3;
                    while (itryMax > 0)
                    {
                        int iflg = spideBll.Update(updateSql);
                        if (iflg >= 0)
                        {
                            itryMax = 0;
                        }
                        else
                        {
                            itryMax--;
                        }

                        Console.WriteLine("更新任务:【{0}】>>>{1}", iflg, DateTime.Now);
                    }
                }
            }
            #endregion           
        }

        /// <summary>
        /// 程序错误代码处理
        /// </summary>
        /// <param name="actionEmail">邮件收件人</param>
        /// <param name="spiderTaskName">爬虫任务名</param>
        /// <param name="errorCode">错误代码</param>
        public void FixException(string actionEmail, string spiderTaskName, string errorCode)
        {
            //TODO：发送请求异常处理的邮件通知
            StaticConfig.ErrorCode.ErrorHandleEmail(actionEmail, spiderTaskName, errorCode);
        }

        /// <summary>
        /// 发送通知邮件
        /// </summary>
        /// <param name="infoStr">标志性的字符串</param>
        /// <param name="hourNum">休息时间（小时）,为负数时指定第2天该时间点启动</param>
        private void MessageEail(string infoStr, int hourNum)
        {
            ///TODO:长时间休息的地方记得将数据监控设置成False
            StaticConfig.Spiderinfo.Flg = false;
            SpiderHelp.MessageModule.Email.SendSimpleEmail(StaticConfig.SpConfig.ActionEmail,
                $"{StaticConfig.Spiderinfo.TaskName}({infoStr})",
                StaticConfig.CountConfig.EMessage + ">>>" + StaticConfig.SpConfig.ActionIp);
            Console.Write("{0}...开始休息...", infoStr);
            if (hourNum == 0)
            {
                do
                {
                    System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
                    if (DateTime.Now.Hour == hourNum)
                    {
                        break;
                    }
                } while (true);
            }
            else if (hourNum > 0)
            {
                System.Threading.Thread.Sleep(new TimeSpan(hourNum, 0, 0));
            }
            else
            {
                do
                {
                    System.Threading.Thread.Sleep(new TimeSpan(0, 5, 0));
                    if (DateTime.Now.Hour == -hourNum)
                    {
                        break;
                    }
                } while (true);
            }
            Console.WriteLine("结束休息...{0}", DateTime.Now);
            ///TODO:结束休息记得将数据监控设置成True
            StaticConfig.Spiderinfo.Flg = true;
        }

        /// <summary>
        /// 发送通知邮件
        /// </summary>
        /// <param name="infoStr">标志性的字符串</param>
        private void MessageEail(string infoStr)
        {
            SpiderHelp.MessageModule.Email.SendSimpleEmail(StaticConfig.SpConfig.ActionEmail,
                $"{StaticConfig.Spiderinfo.TaskName}({infoStr})",
                $"{StaticConfig.CountConfig.EMessage}>>>{StaticConfig.SpConfig.ActionIp}");
        }
    }
}
