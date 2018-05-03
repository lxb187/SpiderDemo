using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiderHelp.InterfaceModule;

namespace SpiderHelp.ConfigModule
{
    /// <summary>
    /// 爬虫常用计数统计类
    /// </summary>
    public class CountConfig : ICountConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CountConfig()
        {
            EMessage = "无";
            ErrorCodeStr = "200";
        }
        /// <summary>
        /// 每半小时成功计数
        /// </summary>
        public long IHalftrue { get; set; }
        /// <summary>
        /// 请求成功次数
        /// </summary>
        public int Itrue { get; set; }
        /// <summary>
        /// 请求失败次数
        /// </summary>
        public int Ifasle { get; set; }
        /// <summary>
        /// 请求总次数
        /// </summary>
        public int Itotal { get; set; }
        /// <summary>
        /// 全局信息变量
        /// </summary>
        public string EMessage { get; set; }
        /// <summary>
        /// 错误代码字符
        /// </summary>
        public string ErrorCodeStr { get; set; }
    }
}
