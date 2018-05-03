using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SpiderHelp.InterfaceModule;

namespace SpiderHelp.ConfigModule
{
    /// <summary>
    /// 代理配置
    /// </summary>
    public class ProxyConfig:IProxyConfig
    {
        /// <summary>
        /// 代理初始化
        /// </summary>
        public ProxyConfig()
        {
            Proxy = new WebProxy("192.168.1.1");
            ProxyUser = "0";
            ProxyPass = "0";
        }
        /// <summary>
        /// 代理
        /// </summary>
        public WebProxy Proxy { get; set; }
        /// <summary>
        /// 代理用户账号
        /// </summary>
        public string ProxyUser { get; set; }
        /// <summary>
        /// 代理用户密码
        /// </summary>
        public string ProxyPass { get; set; }
    }
}
