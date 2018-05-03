#region 版权信息
/* ======================================================================== 
 * 描述信息   
 * 作者：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间：2018/4/17 17:33:27 
 * CLR：4.0.30319.42000 
 * 功能描述：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System.Collections.Generic;
using SpiderHelp.ConfigModule;
using SpiderHelp.MonitorModule;

namespace SpiderDemo.Model
{
    /// <summary>
    /// 爬虫默认提供的初始化配置类
    /// </summary>
    public class MySpiderConfig
    {
        /// <summary>
        /// 爬虫初始化配置类
        /// </summary>
        public SpiderConfig SpConfig = new SpiderConfig();

        /// <summary>
        /// 爬虫计数统计类
        /// </summary>
        public CountConfig CountConfig = new CountConfig();

        /// <summary>
        /// 爬虫任务类集合
        /// </summary>
        public List<TaskUrlConfig> TaskUrls = new List<TaskUrlConfig>();

        /// <summary>
        /// 增加爬虫任务类集合
        /// </summary>
        public List<TaskUrlConfig> AddTaskUrls = new List<TaskUrlConfig>();

        /// <summary>
        /// 爬虫数据监控信息类
        /// </summary>
        public Spiderinfo Spiderinfo = new Spiderinfo();

        /// <summary>
        /// 爬虫程序错误代码
        /// </summary>
        public GenericErrorCode ErrorCode = new GenericErrorCode();
    }
}