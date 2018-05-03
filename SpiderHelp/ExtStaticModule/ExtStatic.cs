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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpiderHelp.ExtStaticModule
{
    /// <summary>
    /// 静态扩展方法
    /// </summary>
    public static class ExtStatic
    {
        /// <summary>
        /// 利用反射和泛型
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            // 定义集合
            List<T> ts = new List<T>();
            //// 获得此模型的类型
            //Type type = typeof(T);
            
            //遍历DataTable中所有的数据行
            foreach(DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性
                foreach(PropertyInfo pi in propertys)
                {
                    //定义一个临时变量
                    var tempName = pi.Name;
                    //检查DataTable是否包含此列（列名==对象的属性名）  
                    if(dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if(!pi.CanWrite) continue;//该属性不可写，直接跳出
                        //取值
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性
                        if(value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                //对象添加到泛型集合中
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 获取请求包的Html信息
        /// 默认utf8编码格式
        /// </summary>
        /// <param name="response">response</param>
        /// <returns>非null的html</returns>
        public static string ResponseHtml(this HttpWebResponse response)
        {
            string curHtml = "";
            Stream responseStream = null;
            Stream streamToRead = null;
            try
            {
                responseStream = response.GetResponseStream();
                streamToRead = responseStream;
                if(response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    streamToRead = new GZipStream(streamToRead ?? throw new InvalidOperationException(), CompressionMode.Decompress);
                }
                else if(response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    streamToRead = new DeflateStream(streamToRead ?? throw new InvalidOperationException(), CompressionMode.Decompress);
                }
                StreamReader streamReader = new StreamReader(streamToRead ?? throw new InvalidOperationException(), Encoding.UTF8);
                curHtml = streamReader.ReadToEnd();
            }
            catch
            {
                curHtml = "";
            }
            finally
            {
                responseStream?.Close();
                streamToRead?.Close();
            }
            return curHtml;
        }

        /// <summary>
        /// 获取请求包的Html信息
        /// </summary>
        /// <param name="response">response</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>非null的html</returns>
        public static string ResponseHtml(this HttpWebResponse response, Encoding encoding)
        {
            string curHtml = "";
            try
            {
                if(response != null)
                {
                    using(Stream responseStream = response.GetResponseStream())
                    {
                        Stream streamToRead = responseStream;
                        if(streamToRead != null)
                        {
                            if(response.ContentEncoding.ToLower().Contains("gzip"))
                            {
                                streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                            }
                            else if(response.ContentEncoding.ToLower().Contains("deflate"))
                            {
                                streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                            }
                            using(StreamReader streamReader = new StreamReader(streamToRead, encoding))
                            {
                                curHtml = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                curHtml = "";
            }
            return curHtml;
        }

        /// <summary>
        /// 自动获取请求包的Html信息
        /// </summary>
        /// <param name="response">response</param>
        /// <returns>请求包的html内容</returns>
        public static string ResponseHtmlAuto(this HttpWebResponse response)
        {
            string curHtml = "";
            Stream stream = response?.GetResponseStream();
            if(stream != null)
            {
                if(response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                }
                else if(response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    stream = new DeflateStream(stream, CompressionMode.Decompress);
                }
                Encoding encoding = null;
                #region 自动获取编码的方式
                string temp = new StreamReader(stream, Encoding.Default).ReadToEnd();
                Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                if(charter.Length > 0)
                {
                    charter = charter.ToLower().Replace("iso-8859-1", "gbk").Replace("http-equiv=content-type", "");
                    encoding = Encoding.GetEncoding(charter.Trim());
                }
                else
                {
                    if (response.CharacterSet != null)
                    {
                        if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                        {
                            encoding = Encoding.UTF8;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                            {
                                encoding = Encoding.UTF8;
                            }
                            else
                            {
                                encoding = Encoding.GetEncoding(response.CharacterSet);
                            }
                        }
                    }
                }
                #endregion
                //Content-Type: text/html;charset=UTF-8
                if (encoding != null)
                {
                    curHtml = new StreamReader(stream, encoding).ReadToEnd();
                }                      
            }
            return curHtml;
        }

        /// <summary>
        /// Json数据类型
        /// </summary>
        /// <param name="response">响应流</param>
        /// <returns>响应的Json数据</returns>
        public static JObject ResponseJson(this HttpWebResponse response)
        {
            JObject obj;
            try
            {
                StreamReader sr = response.ContentEncoding.ToLower().Contains("gzip") ? new StreamReader(new GZipStream(response.GetResponseStream() ?? throw new InvalidOperationException(), CompressionMode.Decompress)) : new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
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
        /// Json数据类型
        /// </summary>
        /// <param name="response">响应流</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应的Json数据</returns>
        public static JObject ResponseJson(this HttpWebResponse response, Encoding encoding)
        {
            JObject obj;
            try
            {
                string CurHtml = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), encoding).ReadToEnd();
                StringReader sr = new StringReader(CurHtml);
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
        /// 字符串转字典
        /// </summary>
        /// <param name="info">字符串</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, string> ConvertDictionary(this string info)
        {
            Dictionary<string, string> lsdic = new Dictionary<string, string>();
            string[] dicts = info.Split(new string[] { "&", "=" }, StringSplitOptions.None);
            try
            {
                for(int i = 1; i < dicts.Length; i = i + 2)
                {
                    if(!lsdic.ContainsKey(dicts[i]))
                    {
                        lsdic.Add(dicts[i - 1], dicts[i]);
                    }
                }
            }
            catch
            {
                lsdic = new Dictionary<string, string>();
            }
            return lsdic;
        }

        /// <summary>
        /// 传入明文，返回用MD5加密后的字符串
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string ToMD5_32(this string str)
        {
            string passwordFormat = System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString();
#pragma warning disable 618
            string result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, passwordFormat);
#pragma warning restore 618
            return result;
        }
    }
}
