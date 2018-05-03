#region 版权信息
/* ======================================================================== 
 * 描述信息 
 *  
 * 作者  ：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间  ：2018年3月17日15:02:03
 * 功能  ：爬虫数据监控后台使用，每半小时统计一次
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Threading;
using SpiderHelp.SaveModule;

namespace SpiderHelp.MonitorModule
{
	/// <summary>
	/// 爬虫信息-每半个小时统计一次
	/// </summary>
	public class Spiderinfo
	{
		#region 变量
		/// <summary>
		/// 爬虫序列号
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 爬虫程序名称【默认:标识信息-负责人邮箱】
		/// </summary>
		public string SpiderProgramName { get; set; }
		/// <summary>
		/// 任务名【任务管理系统的名称】
		/// </summary>
		public string TaskName { get; set; }
		/// <summary>
		/// 站点名称
		/// </summary>
		public string SiteName { get; set; }
		/// <summary>
		/// 模版名称
		/// </summary>
		public string TemplateName { get; set; }
		/// <summary>
		/// 爬虫启动时间
		/// </summary>
		public DateTime StartDT { get; set; }
		/// <summary>
		/// 最后统计时间
		/// </summary>
		public DateTime EndDT { get; set; }
		/// <summary>
		/// 请求量
		/// </summary>
		public long RequestMount { get; set; }
		/// <summary>
		/// 抓取量
		/// </summary>
		public long CatchMount { get; set; }
		/// <summary>
		/// 入库量【主要是地址去重存储的量】
		/// </summary>
		public long StoreMount { get; set; }
		/// <summary>
		/// 代理请求数量
		/// </summary>
		public long ProxyRequestMount { get; set; }
		/// <summary>
		/// 代理成功率
		/// </summary>
		public decimal ProxyPercent { get; set; }
		/// <summary>
		/// 打码次数
		/// </summary>
		public long VertifyMount { get; set; }
		/// <summary>
		/// 打码成功次数
		/// </summary>
		public long VertifyTrueMount { get; set; }
		/// <summary>
		/// 打码成功率
		/// </summary>
		public decimal VertifyPercent { get; set; }
		
		/// <summary>
		/// 已获得总量
		/// </summary>
		public long TotalMount { get; set; }
		/// <summary>
		/// 总的字节数
		/// </summary>
		public long TotalLength { get; set; }
		/// <summary>
		/// 爬虫所远行环境的IP地址
		/// </summary>
		public string IPAddress { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Ext { get; set; }
        /// <summary>
        /// 用于判断是否需要发送信息至数据库（ture:发送  false:不发送）
        /// </summary>
	    public bool Flg { get; set; }

	    #endregion

		private static string AliyunConn = "server=jiuweiwang.mysql.rds.aliyuncs.com;User Id=lxb;Password=Lxb123456;port=3309;Database=espider;pooling=false;CharSet=utf8;allowzerodatetime=True";
        /// <summary>
        /// 构造函数
        /// </summary>
	    public Spiderinfo()
	    {
            SpiderProgramName = "测试人@jiuweiwang.com";
            TaskName = "测试爬虫";
            SiteName = "测试站点";
            TemplateName = "测试模板";
            StartDT = DateTime.Now;
            EndDT = DateTime.Now;
            RequestMount = 0;
            CatchMount = 0;
            StoreMount = 0;
            ProxyRequestMount = 0;
            ProxyPercent = 0;
            VertifyMount = 0;
            VertifyPercent = 0;
            TotalMount = 0;
            TotalLength = 0;
            IPAddress = "测试Ip";
            Ext = "测试爬虫默认信息";
            Flg = true;
	    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spiderinfo"></param>
        /// <returns></returns>
		public bool Control(Spiderinfo spiderinfo)
		{
			bool flg = false;
			#region 爬虫监控系统
			string insertSql = string.Format("insert ignore into spiderinfo(SpiderProgramName,TaskName,SiteName,TemplateName,StartDT,EndDT,RequestMount,CatchMount,StoreMount,ProxyRequestMount,ProxyPercent,VertifyMount,VertifyPercent,TotalMount,TotalLength,IPAddress,Ext) values('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','{16}')", spiderinfo.SpiderProgramName, spiderinfo.TaskName, spiderinfo.SiteName, spiderinfo.TemplateName, spiderinfo.StartDT, spiderinfo.EndDT, spiderinfo.RequestMount, spiderinfo.CatchMount, spiderinfo.StoreMount, spiderinfo.ProxyRequestMount, spiderinfo.ProxyPercent, spiderinfo.VertifyMount, spiderinfo.VertifyPercent, spiderinfo.TotalMount, spiderinfo.TotalLength, spiderinfo.IPAddress, spiderinfo.Ext);
			int itryMax = 3;
			while(itryMax > 0)
			{
				int iflg = MySqlHelp.Insert(AliyunConn, insertSql, spiderinfo.TaskName + "插入数据失败记录.txt");
				if(iflg >= 0)
				{
					itryMax = 0;
					flg = true;
				}
				else
				{
					itryMax--;
				}
				string checkInfo =
					string.Format("爬虫:{0} 请求量:{1} 抓取量:{2} 入库量:{3} 代理请求量:{4} 代理成功率:{5} 打码次数:{6} 成功打码:{7} 打码成功率:{8} 总抓取量:{9} 时间:{10}",
						spiderinfo.TaskName,
						spiderinfo.RequestMount, spiderinfo.CatchMount, spiderinfo.StoreMount, spiderinfo.ProxyRequestMount,
						spiderinfo.ProxyPercent, spiderinfo.VertifyMount, spiderinfo.VertifyTrueMount, spiderinfo.VertifyPercent,
						spiderinfo.TotalMount, DateTime.Now);
				Console.WriteLine("【{0}】{1}", iflg, checkInfo);
				CLog.DiaryLog(checkInfo, spiderinfo.TaskName + "(每半小时统计一次)_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
			}
			#endregion
			return flg;
		}   

	    /// <summary>
	    /// 半小时自动统计
	    /// </summary>
	    public void AutomaticCount()
	    {
	        int isign = 1;
            #region 自动统计爬虫信息
            Thread rdsTh = new Thread(() =>
            {
                while(true)
	            {
	                if (Flg)
	                {
	                    isign = 0;
	                }
                    if (DateTime.Now.Minute%30  == 0 || (!Flg && isign == 0))
	                {
                        //当Flg第一次为false时进行写入
                        if(Flg || (!Flg && isign == 0))
	                    {
	                        long totalMount = TotalMount;
	                        try
	                        {
	                            string sql = string.Format(
	                                "select TotalMount from spiderinfo where SpiderProgramName='{0}' and TaskName='{1}' and SiteName='{2}' and TemplateName='{3}' order by id DESC limit 0,1",
	                                SpiderProgramName, TaskName, SiteName, TemplateName);
	                            totalMount =
	                                long.Parse(
	                                    SpiderHelp.SaveModule.MySqlHelp.Select(AliyunConn, sql).Rows[0][0].ToString());
	                        }
	                        catch
	                        {

	                        }
	                        TotalMount = totalMount + CatchMount;
	                        EndDT = DateTime.Now;
	                        ProxyRequestMount = RequestMount;
	                        if (CatchMount > 0 && CatchMount <= RequestMount)
	                        {
	                            ProxyPercent = Convert.ToDecimal(((double) CatchMount/RequestMount).ToString("0.000"));
	                        }
	                        if (VertifyTrueMount > 0 && VertifyTrueMount <= VertifyMount)
	                        {
	                            VertifyPercent = Convert.ToDecimal(((double)VertifyTrueMount / VertifyMount).ToString("0.000"));
	                        }
                            string rdsSql =
	                            string.Format(
	                                "insert ignore into spiderinfo(SpiderProgramName,TaskName,SiteName,TemplateName,StartDT,EndDT,RequestMount,CatchMount,StoreMount,ProxyRequestMount,ProxyPercent,VertifyMount,VertifyPercent,TotalMount,TotalLength,IPAddress,Ext) values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','{16}');",
	                                SpiderProgramName, TaskName, SiteName, TemplateName,
	                                StartDT, EndDT, RequestMount, CatchMount,
	                                StoreMount, ProxyRequestMount, ProxyPercent,
	                                VertifyMount, VertifyPercent, TotalMount, TotalLength,
	                                IPAddress, Ext);
	                        int itryMax = 3;
	                        while (itryMax > 0)
	                        {
	                            int iflg = SpiderHelp.SaveModule.MySqlHelp.Insert(AliyunConn, rdsSql,
	                                TaskName + "_数据监控失败记录");
	                            isign = Flg ? 0 : 1;
	                            if (iflg >= 0)
	                            {
	                                itryMax = 0;
	                            }
	                            else
	                            {
	                                itryMax--;
	                            }
	                            string checkInfo =
	                                string.Format(
	                                    "爬虫:{0} 请求量:{1} 抓取量:{2} 入库量:{3} 代理请求量:{4} 代理成功率:{5} 打码次数:{6} 成功打码:{7} 打码成功率:{8} 总抓取量:{9} 时间:{10}",
	                                    TaskName,
	                                    RequestMount, CatchMount, StoreMount,
	                                    ProxyRequestMount,
	                                    ProxyPercent, VertifyMount, VertifyTrueMount,
	                                    VertifyPercent,
	                                    TotalMount, DateTime.Now);
	                            CLog.DiaryLog(checkInfo,
	                                TaskName + "(每半小时统计一次)_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
	                        }
	                        StartDT = DateTime.Now;
	                        VertifyPercent = 0;
	                        VertifyMount = 0;
	                        VertifyTrueMount = 0;
	                        StoreMount = 0;
	                        ProxyRequestMount = 0;
	                        CatchMount = 0;
	                        RequestMount = 0;
	                        ProxyPercent = 0;
	                        TotalLength = 0;

                            Thread.Sleep(new TimeSpan(0, 1, 1));	                        
	                    }
	                    else
                        {
                            Thread.Sleep(new TimeSpan(0, 0, 59));
                        }
                    }
                    else
	                {	                  
                        Thread.Sleep(new TimeSpan(0, 0, 59));
	                }
                }
            });
            rdsTh.Start();
            #endregion           
	    }
	}
}
