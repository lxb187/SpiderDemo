#region 版权信息
/* ======================================================================== 
 * 描述信息   
 * 作者：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间：2018/3/29 18:25:38 
 * CLR：4.0.30319.42000 
 * 功能描述：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using SpiderDemo.Interfaces;
using SpiderDemo.Spiders.TestSpider.Task;

//测试爬虫信息抓取
namespace SpiderDemo.Spiders.TestSpider
{
    /// <summary>
    /// 爬虫启动入口类
    /// </summary>
    internal class MyStart:IMyStart
    {
        /// <summary>
        /// 爬虫启动入口
        /// </summary>
        public void Start()
        {
            TaskToDo taskToDo = new TaskToDo();
            taskToDo.Start();

            GrabAllInfo grabAllInfo = new GrabAllInfo();
            grabAllInfo.Start();
        }
    }
}