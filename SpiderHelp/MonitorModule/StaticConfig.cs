using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiderHelp.ConfigModule;

namespace SpiderHelp.MonitorModule
{
    /// <summary>
    /// 爬虫默认提供的初始化配置类
    /// </summary>
    public static class StaticConfig
    {
        /// <summary>
        /// 爬虫初始化配置类
        /// </summary>
        public static SpiderConfig SpConfig = new SpiderConfig();
        /// <summary>
        /// 爬虫计数统计类
        /// </summary>
        public static CountConfig CountConfig = new CountConfig();
        /// <summary>
        /// 爬虫任务类集合
        /// </summary>
        public static List<TaskUrlConfig> TaskUrls = new List<TaskUrlConfig>();
        /// <summary>
        /// 增加爬虫任务类集合
        /// </summary>
        public static List<TaskUrlConfig> AddTaskUrls = new List<TaskUrlConfig>();
        /// <summary>
        /// 爬虫数据监控信息类
        /// </summary>
        public static Spiderinfo Spiderinfo = new Spiderinfo();
        /// <summary>
        /// 爬虫程序错误代码
        /// </summary>
        public static SpiderHelp.MonitorModule.GenericErrorCode ErrorCode = new GenericErrorCode();
    }
}
