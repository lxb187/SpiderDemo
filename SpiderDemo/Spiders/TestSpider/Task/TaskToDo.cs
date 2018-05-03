using System;
using System.Collections.Generic;
using SpiderDemo.Bll;
using SpiderDemo.Interfaces;
using SpiderHelp.ConfigModule;
using SpiderHelp.ExtStaticModule;
using SpiderHelp.SaveModule;

namespace SpiderDemo.Spiders.TestSpider.Task
{
    /// <summary>
    /// 用于分配任务
    /// </summary>
    internal class TaskToDo:IMyStart
    {
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private static readonly IndexBll spideBll = new IndexBll(System.Configuration.ConfigurationManager.AppSettings["Spider_Mysql_Ali"]);
        /// <summary>
        /// 任务名
        /// </summary>
        private const string taskName = "XXX";
        /// <summary>
        /// 任务数据库表名
        /// </summary>
        private const string actionTable = "XXX";
        /// <summary>
        /// 任务启动入口
        /// </summary>
        public void Start()
        {
            TaskAdd();
        }

        /// <summary>
        /// 初始任务源入库
        /// </summary>
        private void TaskAdd()
        {
            try
            {
                List<TaskUrlConfig> allInfoUrls = new List<TaskUrlConfig>();
                int maxPage = 214;
                for (int i = 1; i < maxPage + 1; i++)
                {
                    string urlInfo = $"XXX";
                    TaskUrlConfig allInfoUrl = new TaskUrlConfig
                    {
                        CompanyName = "无",
                        Uid = "无",
                        Tab = "list",
                        Url = urlInfo,
                        Md5 = MyConvert.ToUserMd5(urlInfo),
                        Method = "get",
                        ICount = 0,
                        IState = 0,
                        Queue_time = DateTime.Now,
                        Done_time = DateTime.Now
                    };
                    allInfoUrls.Add(allInfoUrl);
                }
                Console.WriteLine($@"共计任务:【{allInfoUrls.Count}】>>>{DateTime.Now}");
                int lssNum = 100;
                DateTime date = DateTime.Now;
                string sqll = $"INSERT IGNORE INTO {actionTable}(CompanyName,Uid,Tab,Url,Md5,Method,ICount,IState,Queue_time,Done_time) VALUES";
                string sqlStr = string.Empty;
                for (int i = 0; i < allInfoUrls.Count; i++)
                {
                    sqlStr +=
                        $"('{allInfoUrls[i].CompanyName}','{allInfoUrls[i].Uid}','{allInfoUrls[i].Tab}','{allInfoUrls[i].Url}','{allInfoUrls[i].Md5}','{allInfoUrls[i].Method}',{allInfoUrls[i].ICount},{allInfoUrls[i].IState},now(),now()),";
                    if (i % lssNum == lssNum - 1 || i == allInfoUrls.Count - 1)
                    {
                        int itryMax = 3;
                        while (itryMax > 0)
                        {
                            int iflg = spideBll.Insert(sqll + sqlStr.TrimEnd(','), $"{taskName}任务入库异常");
                            if (iflg >= 0)
                            {
                                itryMax = 0;
                            }
                            else
                            {
                                itryMax--;
                            }
                            Console.WriteLine("入库【{0}】>>>{1}", iflg, DateTime.Now);
                        }
                        Console.WriteLine(@"**************************************************");
                        sqlStr = string.Empty;
                    }
                    Console.Title = $@"{taskName}任务入库[{date:MMddHHmm}]【{i}/{allInfoUrls.Count}】";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"{ex.Message}>>>{DateTime.Now}");
                CLog.DiaryLog(ex.Message, $"\\{taskName}任务源入库异常\\{actionTable}任务源入库异常_{DateTime.Now:yyyyMMdd}.txt");
            }
        }
    }
}
