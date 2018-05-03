using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace SpiderHelp.VerificationCode
{
    /// <summary>
    /// http方式若快打码类
    /// </summary>
    public class RuoKuaiHttp
    {
        #region 帐号配置信息

        //必要的参数
        private static string UserName = "3130491961"; //用户名
        private static string Password = "jww123654"; //密码
        private static string TypeId = "6900"; //代码参数
        private static string Timeout = "90"; //超时时间【秒为单位】

        //可选参数
        private static IDictionary<object, object> param = new Dictionary<object, object>
		{
			{"username", UserName},
			{"password", Password},
			{"typeid", TypeId},
			{"timeout", Timeout},
			{"softid", 73219},
			{"softkey", "7deb33430fd4460080008e464f0e00a5"}
		};

        #endregion

        #region 若快验证码
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="numPic">图片</param>
        /// <param name="typeid">若快类型</param>
        /// <returns></returns>
        public static string GetVerificationCode(Image numPic, string typeid)
        {
            string yzm = string.Empty;
            var param = new Dictionary<object, object>
                        {                
                            {"username","3130491961"},
                            {"password","jww123654"},
                            {"typeid",typeid},
                            {"timeout","90"},
                            {"softid",72835},
                            {"softkey","ef33d73b00974ad5afbf3ad0c2ad1a6d"}
                        };
            //Image image = Image.FromStream(Http.GetStream(codeUrl, out cooikes));//加载图片有失败的情况多线程下建议try处理下。
            Image image = numPic;
            byte[] data;
            //把Image转换为byte
            using(MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Gif);
                ms.Position = 0;
                data = new byte[ms.Length];
                ms.Read(data, 0, Convert.ToInt32(ms.Length));
                ms.Flush();
            }
            string httpResult = RuoKuaiHttp.Post("http://api.ruokuai.com/create.xml", param, data);

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(httpResult);
            }
            catch
            {
            }
            XmlNode idNode = xmlDoc.SelectSingleNode("Root/Id");
            XmlNode resultNode = xmlDoc.SelectSingleNode("Root/Result");
            XmlNode errorNode = xmlDoc.SelectSingleNode("Root/Error");
            string result = string.Empty;
            string topidid = string.Empty;
            if(resultNode != null && idNode != null)
            {
                topidid = idNode.InnerText;
                result = resultNode.InnerText;
                yzm = result;
            }
            else if(errorNode != null)
            {

            }
            else
            {

            }
            return yzm;
        }
        #endregion

        #region 封装好的常用方法

        /// <summary>
        /// 基于本地图片打码
        /// </summary>
        /// <param name="imagePath">图片绝对路径</param>
        /// <param name="typeid">打码类型</param>
        /// <returns></returns>
        public static string Post(string imagePath = "D:\\dama.jpg", string typeid = "6900")
        {
            if(param.ContainsKey("typeid"))
            {
                param.Remove("typeid");
            }
            param.Add("typeid", typeid);
            byte[] fbyte = File.ReadAllBytes(imagePath);
            return Post("http://api.ruokuai.com/create.xml", param, fbyte);
        }

        #endregion

        #region Post 携带二进制图片数组

        /// <summary>
        /// HTTP POST方式请求数据(带图片)
        /// </summary>
        /// <param name="url">URL</param>        
        /// <param name="param">POST的数据</param>
        /// <param name="fileByte">图片Byte</param>
        /// <returns></returns>
        public static string Post(string url, IDictionary<object, object> param, byte[] fileByte)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.UserAgent = "RK_C# 1.2";
            wr.Method = "POST";

            //wr.Timeout = 150000;
            //wr.KeepAlive = true;

            //wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream rs = null;
            try
            {
                rs = wr.GetRequestStream();
            }
            catch
            {
                return "无法连接.请检查网络.";
            }
            string responseStr = null;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach(string key in param.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, param[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "image", "i.gif", "image/gif"); //image/jpeg
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(fileByte, 0, fileByte.Length);

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();

                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                responseStr = reader2.ReadToEnd();

            }
            catch
            {
                //throw;
            }
            finally
            {
                if(wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                wr.Abort();
                wr = null;

            }
            return responseStr;
        }

        #endregion

        #region Post
        /// <summary>
        /// 若快打码Post方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<object, object> param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "RK_C# 1.1";
            //request.Timeout = 30000;

            #region POST方法

            //如果需要POST数据  
            if(!(param == null || param.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach(string key in param.Keys)
                {
                    if(i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, param[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, param[key]);
                    }
                    i++;
                }

                byte[] data = System.Text.Encoding.UTF8.GetBytes(buffer.ToString());
                try
                {
                    using(Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                catch
                {
                    return "无法连接.请检查网络.";
                }

            }

            #endregion

            WebResponse response = null;
            string responseStr = string.Empty;
            try
            {
                response = request.GetResponse();

                if(response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch(Exception)
            {
                //throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;

        }

        #endregion
    }
}
