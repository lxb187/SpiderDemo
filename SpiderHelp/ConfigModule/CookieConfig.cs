using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiderHelp.InterfaceModule;

namespace SpiderHelp.ConfigModule
{
    /// <summary>
    /// 账号Cookie配置类
    /// </summary>
    public class CookieConfig : ICookieConfig
    {
        /// <summary>
        /// 默认初始化构造函数
        /// </summary>
        public CookieConfig()
        {
            Id = 0;
            Name = "测试";
            Account = "测试账号";
            Password = "测试密码";
            Cookie = "测试Cookie";
            IState = 0;
            CheckType = 0;
            CheckDate = DateTime.Now;
            ItrueNum = 0;
            InullNum = 0;
            IvfyNum = 0;
            Queue_time = DateTime.Now;
        }

        /// <summary>
        /// 自增长Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 账号昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 账号的Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// 账号状态
        /// </summary>
        public int IState { get; set; }
        /// <summary>
        /// 爬虫检查账号状态
        /// </summary>
        public int CheckType { get; set; }
        /// <summary>
        /// 爬虫检测当前账号时间
        /// </summary>
        public DateTime CheckDate { get; set; }
        /// <summary>
        /// 当前账号累计请求成功次数
        /// </summary>
        public int ItrueNum { get; set; }
        /// <summary>
        /// 当前账号累计请求为空次数
        /// </summary>
        public int InullNum { get; set; }
        /// <summary>
        /// 当前账号累计请求被打码次数
        /// </summary>
        public int IvfyNum { get; set; }
        /// <summary>
        /// 账号登录时间
        /// </summary>
        public DateTime Queue_time { get; set; }
    }
}
