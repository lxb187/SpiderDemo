using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SpiderHelp.MonitorModule;

namespace SpiderHelp.InterfaceModule
{
    interface ISpiderConfig
    {
        #region 用于读取配置文件
        /// <summary>
        /// 邮箱信息
        /// </summary>
        string ActionEmail { get; set; }
        /// <summary>
        /// 并发数量
        /// </summary>
        int ActionLssNum { get; set; }
        /// <summary>
        /// 文件存储盘符
        /// </summary>
        string PathSign { get; set; }
        /// <summary>
        /// 资源文件盘符，例如图片下载
        /// </summary>
        string ResourcesPathSign { get; set; }
        /// <summary>
        /// 并发间隔时间
        /// </summary>
        int ActionSleepTime { get; set; }
        /// <summary>
        /// 爬虫运行ip
        /// </summary>
        string ActionIp { get; set; }
        /// <summary>
        /// 任务起始id
        /// </summary>
        int StartNum { get; set; }
        /// <summary>
        /// 任务结束id
        /// </summary>
        int EndNum { get; set; }
        /// <summary>
        /// 账号起始id
        /// </summary>
        int StartCk { get; set; }
        /// <summary>
        /// 账号结束id
        /// </summary>
        int EndCk{ get; set; }
        /// <summary>
        /// 阿里云espider数据库连接
        /// </summary>
        string AliConstr { get; set; }
        /// <summary>
        /// 总控taskdata数据库连接
        /// </summary>
        string TakskDataConstr { get; set; }
        /// <summary>
        /// 模板名
        /// </summary>
        string ActionTab { get; set; }
        /// <summary>
        /// 模板版本号
        /// </summary>
        string ActionTabFlg { get; set; }
        /// <summary>
        /// 程序启动时间
        /// </summary>
        DateTime DateKs { get; set; }
        #endregion           
    }
}
