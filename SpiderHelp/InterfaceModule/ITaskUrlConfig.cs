using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderHelp.InterfaceModule
{
    interface ITaskUrlConfig
    {
        int Id { get; set; }
        string Uid { get; set; }
        string CompanyName { get; set; }
        string Tab { get; set; }
        string Url { get; set; }
        string Md5 { get; set; }
        string Method { get; set; }
        int ICount { get; set; }
        int IState { get; set; }
        DateTime Queue_time { get; set; }
        DateTime Done_time { get; set; }
    }
}
