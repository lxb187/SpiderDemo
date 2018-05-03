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
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpiderHelp.ExtStaticModule
{
    /// <summary>
    /// 转化类
    /// </summary>
	public class MyConvert
	{
        /// <summary>
        /// response.ResponseHtml获取Json数据类型
        /// </summary>
        /// <param name="curHtml">response.ResponseHtml</param>
        /// <returns>响应的Json数据</returns>
        public static JObject ToJson(string curHtml)
        {
            JObject obj = new JObject();
            try
            {
                StringReader sr = new StringReader(curHtml);
                JsonSerializer serializer = new JsonSerializer();
                obj = serializer.Deserialize(new JsonTextReader(sr), new JObject().GetType()) as JObject;
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

        /// <summary>
        /// response.ResponseHtml获取Json数据类型
        /// (返回结果可强制转化为json)
        /// </summary>
        /// <param name="curHtml">response.ResponseHtml</param>
        /// <returns>响应的Json数据</returns>
        public static Object ToObject(string curHtml)
        {
            Object obj = new Object();
            try
            {
                StringReader sr = new StringReader(curHtml);
                JsonSerializer serializer = new JsonSerializer();
                obj = serializer.Deserialize(new JsonTextReader(sr), new Object().GetType()) as IEnumerable<object>;
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

        /// <summary>
        /// 读取response.ResponseHtml转化为HtmlNode
        /// </summary>
        /// <param name="chtml">response.ResponseHtml</param>
        /// <returns>HtmlNode</returns>
        public static HtmlNode ToHtmlNode(string chtml)
        {
            HtmlAgilityPack.HtmlDocument htmlNode = new HtmlDocument();
            htmlNode.LoadHtml(chtml);
            HtmlAgilityPack.HtmlNode item = htmlNode.DocumentNode;
            return item;
        }

        /// <summary>
        /// unicode转中文（符合js规则的）
        /// </summary>
        /// <returns></returns>
        public static string ToJsString(string str)
        {
            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(str, delegate(Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }

        /// <summary>
        /// MD5值压缩
        /// </summary>
        /// <param name="encryptString">原字符串</param>
        /// <returns>返回32位小写字符串</returns>
        public static string ToUserMd5(string encryptString)
        {
            byte[] result = Encoding.UTF8.GetBytes(encryptString);
            MD5 md5 = MD5.Create();
            byte[] output = md5.ComputeHash(result);
            string encryptResult = BitConverter.ToString(output).Replace("-", "").ToLower();
            return encryptResult;
        }

		/// <summary>
        /// 时间戳转为DateTime时间
		/// </summary>
		/// <param name="timeStamp">时间戳字符串</param>
        /// <returns>DateTime时间</returns>
		public static DateTime ToUixDateTime(string timeStamp)
		{
            if(timeStamp.Length == 10)
            {
                timeStamp = timeStamp + "0000";
            }
            if(timeStamp.Length == 7)
            {
                timeStamp = timeStamp + "0000000";
            }
			DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			long lTime = Int64.Parse(timeStamp);
			TimeSpan toNow = new TimeSpan(lTime);
			return dtStart.Add(toNow);
		}

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">DateTime时间</param>
        /// <param name="slength">需要转化的长度</param>
        /// <returns>Unix时间戳格式</returns>
        public static long ToUnixTimeStamp(System.DateTime time, int slength=10)
        {
            long ii = 10000000;
            if (slength == 10)
            {
                ii = 10000000;
            }
            else if(slength==13)
            {
                ii = 10000;
            }
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / ii;            //除10000调整为13位
		    return t;
		}      

        /// <summary>
        /// 将c# DateTime时间格式转换为中国标准时间时间格式
        /// </summary>
        /// <param name="time">DateTime时间</param>
        /// <returns>转化后的字符串</returns>
        public static string ToDateTimeStr(System.DateTime time)
        {
            string str = time.ToString("ddd MMM dd yyy HH:mm:ss ", System.Globalization.CultureInfo.GetCultureInfo("en-us")) + "GMT 0800 (中国标准时间)";
            return str;
        }

        /// <summary>
        /// 含月天时分的字符串转化为时间
        /// </summary>
        /// <param name="date">字符串如:20时23分前</param>
        /// <returns>yyyy-MM-dd</returns>
        public static string ToQccDate(string date)
        {
            //处理更新日期
            string datestr = DateTime.Now.ToString("yyyy-MM-dd");
            if(date.Contains("天") || date.Contains("时") || date.Contains("分") || date.Contains("月"))
            {
                if(date.Contains("天"))
                {
                    Regex reg = new Regex("\\d+");//从左到右  匹配连续数字
                    string str = reg.Match(date).ToString();
                    datestr = DateTime.Now.AddDays(-Int32.Parse(str)).ToString("yyyy-MM-dd");
                }
                if(date.Contains("时") || date.Contains("分"))
                {
                    datestr = DateTime.Now.ToString("yyyy-MM-dd");
                }
                if(date.Contains("月"))
                {
                    Regex reg = new Regex("\\d+");//从左到右  匹配连续数字
                    string str = reg.Match(date).ToString();
                    datestr = DateTime.Now.AddMonths(-Int32.Parse(str)).ToString("yyyy-MM-dd");
                }
            }
            else
            {
                datestr = date;
            }
            return datestr;
        }
	}
}
