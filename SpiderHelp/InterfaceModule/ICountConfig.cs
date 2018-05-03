using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderHelp.InterfaceModule
{
    interface ICountConfig
    {
        #region 用于全局统计计数展示
        /// <summary>
        /// 半小时成功计数
        /// </summary>
        long IHalftrue { get; set; }
        /// <summary>
        /// 成功计数
        /// </summary>
        int Itrue { get; set; }
        /// <summary>
        /// 失败计数
        /// </summary>
        int Ifasle { get; set; }
        /// <summary>
        /// 累计计数
        /// </summary>
        int Itotal { get; set; }
        /// <summary>
        /// 全局信息变量，默认无
        /// </summary>
        string EMessage { get; set; }
        /// <summary>
        /// 请求错误代码，默认200
        /// </summary>
        string ErrorCodeStr { get; set; }
        #endregion
    }
}
