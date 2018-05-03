using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiderHelp.InterfaceModule;

namespace SpiderHelp.ConfigModule
{
    /// <summary>
    /// 通用任务模板类
    /// </summary>
    public class TaskUrlConfig: ITaskUrlConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskUrlConfig()
        {
            Id = 0;
            Uid = "测试Uid";
            CompanyName = "测试CompanyName";
            Tab = "测试Tab";
            Url = "测试Url";
            Md5 = "测试Md5";
            Method = "测试Method";
            ICount = 0;
            IState = 0;
            Queue_time = DateTime.Now;
            Done_time = DateTime.Now;
        }
        /// <summary>
        /// 自增长Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公司Uid
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 模板名
        /// </summary>
        public string Tab { get; set; }
        /// <summary>
        /// 请求链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求链接压缩后的MD5值
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// Get或者Post
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 请求链接展示的结果数量
        /// </summary>
        public int ICount { get; set; }
        /// <summary>
        /// 获取状态
        /// </summary>
        public int IState { get; set; }
        /// <summary>
        /// 入队时间
        /// </summary>
        public DateTime Queue_time { get; set; }
        /// <summary>
        /// 出队时间
        /// </summary>
        public DateTime Done_time { get; set; }
    }
}
