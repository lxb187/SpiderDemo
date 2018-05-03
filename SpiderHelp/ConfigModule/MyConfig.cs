#region 版权信息
/* ======================================================================== 
 * 描述信息 
 *  
 * 作者  ：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间  ：2018年3月12日14:45:04
 * 功能  ：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;

namespace SpiderHelp.ConfigModule
{
	internal class MyConfig
	{
		/// <summary>
		/// 启动时间
		/// </summary>
		public static DateTime StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);
		/// <summary>
		/// 周期时间
		/// </summary>
		public static int Period = 1 * 60 * 1000;// 24 * 60 * 60 * 1000;//
		/// <summary>
		/// 日志路径
		/// </summary>
		public static string FullPath = "C:\\ServiceWHlog.html";
		public static string TaskList = "C:\\TaskList.txt";
		/// <summary>
		/// 传入的参数
		/// </summary>
		public static object obj = new object();

		public static string ToEmail = "lxb@jiuweiwang.com";
	}
}
