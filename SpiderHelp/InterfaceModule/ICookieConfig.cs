using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpiderHelp.InterfaceModule
{
    interface ICookieConfig
    {
        /// <summary>
        /// 自增长Id
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 账号昵称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// 账号的Cookie
        /// </summary>
        string Cookie { get; set; }
        /// <summary>
        /// 账号状态
        /// </summary>
        int IState { get; set; }
        /// <summary>
        /// 爬虫检查账号状态
        /// </summary>
        int CheckType { get; set; }
        /// <summary>
        /// 爬虫检测当前账号时间
        /// </summary>
        DateTime CheckDate { get; set; }
        /// <summary>
        /// 当前账号累计请求成功次数
        /// </summary>
        int ItrueNum { get; set; }
        /// <summary>
        /// 当前账号累计请求为空次数
        /// </summary>
        int InullNum { get; set; }
        /// <summary>
        /// 当前账号累计请求被打码次数
        /// </summary>
        int IvfyNum { get; set; }
        /// <summary>
        /// 账号登录时间
        /// </summary>
        DateTime Queue_time { get; set; }
    }
}
