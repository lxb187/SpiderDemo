using System;
using System.Collections.Generic;
using System.Net;

namespace SpiderHelp.ConfigModule
{
    /// <summary>
    /// 爬虫初始化配置类
    /// </summary>
    public class SpiderConfig:SpiderHelp.InterfaceModule.ISpiderConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SpiderConfig()
        {
            PcName = "192.168.1.1";
            ActionEmail = "lxb@jiuweiwang.com";
            ActionIp = "192.168.1.1";
            ActionLssNum = 1;
            PathSign = "测试盘符";
            ResourcesPathSign = "测试存放资源性文件盘符，例如图片下载";
            ActionSleepTime = 0;
            StartNum = 1;
            EndNum = 2;
            StartCk = 1;
            EndCk = 2;
            AliConstr = "测试阿里云链接";
            TakskDataConstr = "测试总控链接";
            ActionTab = "测试模板";
            ActionTabFlg = "测试模板版本标志";
            DateKs = DateTime.Now;          
        } 
        /// <summary>
        /// 通知邮件
        /// </summary>
        public string ActionEmail { get; set; }
        /// <summary>
        /// 计算机名（以虚拟机IP为准）
        /// </summary>
        public string PcName { get; set; }
        /// <summary>
        /// 并发次数
        /// </summary>
        public int ActionLssNum { get; set; }
        /// <summary>
        /// 文件系统盘符
        /// </summary>
        public string PathSign { get; set; }
        /// <summary>
        /// 资源文件盘符，例如图片下载
        /// </summary>
        public string ResourcesPathSign { get; set; }

        /// <summary>
        /// 休息时间
        /// </summary>
        public int ActionSleepTime { get; set; }
        /// <summary>
        /// 虚拟机IP
        /// </summary>
        public string ActionIp { get; set; }
        /// <summary>
        /// 任务起始ID
        /// </summary>
        public int StartNum { get; set; }
        /// <summary>
        /// 任务结束ID
        /// </summary>
        public int EndNum { get; set; }
        /// <summary>
        /// 账号起始id
        /// </summary>
        public int StartCk { get; set; }
        /// <summary>
        /// 账号结束id
        /// </summary>
        public int EndCk { get; set; }

        /// <summary>
        /// 阿里云数据库连接
        /// </summary>
        public string AliConstr { get; set; }
        /// <summary>
        /// 任务总库数据库连接
        /// </summary>
        public string TakskDataConstr { get; set; }
        /// <summary>
        /// 模板名
        /// </summary>
       public string ActionTab { get; set; }
        /// <summary>
        /// 模板版本号
        /// </summary>
       public string ActionTabFlg { get; set; }

        /// <summary>
        /// 爬虫开始时间
        /// </summary>
        public DateTime DateKs { get; set; }      
    }
}
