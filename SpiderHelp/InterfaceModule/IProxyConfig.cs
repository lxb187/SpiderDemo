using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpiderHelp.InterfaceModule
{
    interface IProxyConfig
    {
        /// <summary>
        /// 代理ip，默认192.168.1.1
        /// </summary>
        WebProxy Proxy { get; set; }
        /// <summary>
        /// 代理IP用户名
        /// </summary>
        string ProxyUser { get; set; }
        /// <summary>
        /// 代理IP密码
        /// </summary>
        string ProxyPass { get; set; }
    }
}
