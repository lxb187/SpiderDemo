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
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using SpiderHelp.ExtStaticModule;

namespace SpiderHelp.SaveModule
{
    /// <summary>
    /// 文件读写帮助类
    /// </summary>
	public class FileIoHelp
    {
        //读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
        private static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        //将List转换为TXT文件
        /// <summary>
        /// Int类型list转化为Txt文本
        /// </summary>
        /// <param name="list">读取的list</param>
        /// <param name="txtFilePath">存入的文本路径</param>
        public static bool IntListToTxt(List<int> list, string txtFilePath)
        {
            try
            {              
                //创建一个文件流，用以写入或者创建一个StreamWriter 
                Decoder d = Encoding.Default.GetDecoder();
                FileStream fs = new FileStream(txtFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Flush();
                // 使用StreamWriter来往文件中写入内容 
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                for(int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
                //关闭此文件t 
                sw.Flush();
                sw.Close();
                fs.Close();             
            }
            catch(Exception ex)
            {
                CLog.FileIoLog($"FileIoHelp.IntListToTxt>>>{ex.Message}", "IntListToTxt-Error");
                return false;
            }
            return true;
        }

        //将List转换为TXT文件
        /// <summary>
        /// list转化为Txt文本
        /// </summary>
        /// <param name="list">读取的list</param>
        /// <param name="txtFilePath">存入的文本路径</param>
        public static bool ListToTxt(List<string> list, string txtFilePath)
        {
            try
            {
                //创建一个文件流，用以写入或者创建一个StreamWriter 
                FileStream fs = new FileStream(txtFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Flush();
                // 使用StreamWriter来往文件中写入内容 
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                for(int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
                //关闭此文件t 
                sw.Flush();
                sw.Close();
                fs.Close();                
            }
            catch(Exception ex)
            {
                CLog.FileIoLog($"FileIoHelp.ListToTxt>>>{ex.Message}", "ListToTxt-Error");
                return false;
            }
            return true;
        }

        //读取文本文件转换为List 
	    /// <summary>
	    /// 读取文本文件转换为List
	    /// </summary>
        /// <param name="filePath">读取的文本路径</param>
	    /// <returns></returns>
	    public static List<string> TxtToList(string filePath)
        {
            List<string> list = new List<string>();
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                //使用StreamReader类来读取文件 
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                // 从数据流中读取每一行，直到文件的最后一行
                string tmp = sr.ReadLine();
                while(tmp != null)
                {
                    list.Add(tmp);
                    tmp = sr.ReadLine();
                }
                //关闭此StreamReader对象 
                sr.Close();
                fs.Close();
            }
            catch(Exception ex)
            {                
                list = new List<string>();
                CLog.FileIoLog($"FileIoHelp.TxtToList>>>{ex.Message}", "TxtToList-Error");
            }
            return list;
        }

        /// <summary>
        /// 遍历无子文件夹下所有csv格式的文件名
        /// </summary>
        /// <param name="filepath">不含子文件夹的路径</param>
        /// <returns>返回所有csv格式的文件名</returns>
        public static List<string> SearchCsvFpath(string filepath)
        {
            List<string> lsList = new List<string>();
            try
            {
                DirectoryInfo theFolder = new DirectoryInfo(filepath);
                FileInfo[] fileInfo = theFolder.GetFiles();
                foreach(FileInfo nextFile in fileInfo) //遍历文件
                {
                    if(Path.GetExtension(nextFile.FullName).Contains("csv"))
                    {
                        lsList.Add(nextFile.FullName);
                    }
                    else
                    {
                        Console.WriteLine(nextFile.FullName);
                    }
                }
            }
            catch(Exception ex)
            {
                CLog.FileIoLog($"FileIoHelp.SearchCsvFpath>>>{ex.Message}", "SearchCsvFpath-Error");
            }
            return lsList;
        }

		/// <summary>
		/// 文件写入
		/// (同名文件追加)
		/// </summary>
		/// <param name="sFilePath">文件路径</param>
		/// <param name="sFileName">文件名(带后缀名)</param>
		/// <param name="strLog">文件内容</param>
		public static bool WriteAppend(string sFilePath, string sFileName, string strLog)
		{
		    bool flg = true;
		    try
		    {
		        //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
		        LogWriteLock.EnterWriteLock();
		        //文件的绝对路径
		        string sFullName = sFilePath + "\\" + sFileName;
		        //验证路径是否存在
		        if (!Directory.Exists(sFilePath))
		        {
		            //不存在则创建
		            Directory.CreateDirectory(sFilePath);
		        }
		        File.AppendAllText(sFullName, strLog, Encoding.UTF8);
		    }
		    catch (Exception ex)
		    {
		        flg = false;
		        CLog.FileIoLog($"FileIoHelp.WriteAppend>>>{ex.Message}", "WriteAppend-Error");
		    }
		    finally
		    {
		        //退出写入模式，释放资源占用
		        LogWriteLock.ExitWriteLock();
		    }
		    return flg;
        }

		/// <summary>
		/// 文件写入
		/// (同名文件覆盖)
		/// </summary>
		/// <param name="sFilePath">文件路径</param>
		/// <param name="sFileName">文件名(带后缀)</param>
		/// <param name="strLog">文件内容</param>
		public static bool WriteFile(string sFilePath, string sFileName, string strLog)
		{
            bool flg = true;
		    try
		    {
		        //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
		        LogWriteLock.EnterWriteLock();
                //文件的绝对路径
                string sFullName = sFilePath + "\\" + sFileName;
		        //验证路径是否存在
                if (!Directory.Exists(sFilePath))
                {
                    //不存在则创建
                    Directory.CreateDirectory(sFilePath);                   
                }
                File.WriteAllText(sFullName, strLog, Encoding.UTF8);
            }
		    catch(Exception ex)
		    {
		        flg = false;
                CLog.FileIoLog($"FileIoHelp.WriteFile>>>{ex.Message}", "WriteFile-Error");
		    }
		    finally
		    {
		        //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
            return flg;
		}
       
		/// <summary>
		/// 图片下载
		/// </summary>
		/// <param name="url">下载链接</param>
		/// <param name="path">图片存放路径</param>
		/// <param name="fileName">图片名称(带后缀)</param>
		/// <returns>是否成功下载</returns>
		public static bool DoGetImage(string url, string path, string fileName)
		{
		    try
		    {
		        //验证路径是否存在
                if (!Directory.Exists(path))
                {
                    //不存在则创建
                    Directory.CreateDirectory(path);
                }
                WebClient myWebClient = new WebClient();
                myWebClient.DownloadFile(new Uri(url), path + "\\\\" + fileName);		            
		    }
		    catch (Exception ex)
		    {
                CLog.FileIoLog($"FileIoHelp.DoGetImage>>>{ex.Message}", "DoGetImage-Error");
		        return false;
		    }
            return true;
		}

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="pathUrl">文件系统指定路径</param>
        /// <param name="siteName">站点名</param>
        /// <param name="templateName">模板名</param>
        /// <param name="fileName">文件名带上后缀</param>
        /// <param name="mainHtml">文件内容</param>
        /// <returns>是否下载成功</returns>
        public static bool FileDown(string pathUrl, string siteName, string templateName, string fileName, string mainHtml)
        {
            bool flg = true;
            string pathNow = pathUrl + DateTime.Now.ToString("yyyyMMdd") + @"\" + siteName + @"\" + templateName;
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
                LogWriteLock.EnterWriteLock();
                //文件的绝对路径
                string sFileName = pathNow + @"\" + fileName; 
                //验证路径是否存在
                if (!Directory.Exists(pathNow)) 
                {
                    //不存在则创建
                    Directory.CreateDirectory(pathNow);
                }              
                File.WriteAllText(sFileName, mainHtml, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                flg = false;
                CLog.FileIoLog($"FileIoHelp.FileDown>>>{ex.Message}", "FileDown-Error");
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
            return flg;
        }

        /// <summary>
        /// 图片下载
        /// </summary>
        /// <param name="pathUrl">文件系统指定路径</param>
        /// <param name="fileName">图片来源文件名带后缀</param>
        /// <param name="fileCode">图片存放路径代码</param>
        /// <param name="imgUrl">图片链接，默认jpg格式</param>
        public static bool ImgDown(string pathUrl, string fileName, string fileCode, string imgUrl)
        {
            bool flg = true;
            string imgPathNow = pathUrl + @"Source\" + fileCode + @"\" + ExtStatic.ToMD5_32(fileName);            
            int imgErrorCount = 3;
            while (imgErrorCount >0)
            {
                try
                {
                    //验证路径是否存在
                    if (!Directory.Exists(imgPathNow))
                    {
                        //不存在则创建
                        Directory.CreateDirectory(imgPathNow);
                    }
                    WebClient myWebClient = new WebClient();
                    string imgPath = imgPathNow + @"\" + ExtStatic.ToMD5_32(imgUrl) + ".jpg";
                    myWebClient.DownloadFile(new Uri(imgUrl), imgPath);
                    imgErrorCount = 0;
                    flg = true;
                }
                catch (Exception ex)
                {
                    flg = false;
                    imgErrorCount--;
                    CLog.FileIoLog($"FileIoHelp.ImgDown>>>{ex.Message}", "ImgDown-Error");                   
                }
            }
            return flg;
        }

	    /// <summary>
	    /// 图片下载
	    /// </summary>
	    /// <param name="pathUrl">文件系统指定路径</param>
	    /// <param name="fileName">图片来源文件名带后缀</param>
	    /// <param name="fileCode">图片存放路径代码</param>
	    /// <param name="imgUrl">图片链接</param>
	    /// <param name="imgType">图片类型（类似png格式）</param>
	    public static bool ImgDown(string pathUrl, string fileName, string fileCode, string imgUrl, string imgType)
	    {
	        bool flg = true;
	        string imgPathNow = pathUrl + @"Source\" + fileCode + @"\" + ExtStatic.ToMD5_32(fileName);
	        int imgErrorCount = 3;
	        while (imgErrorCount > 0)
	        {
	            try
	            {
	                //验证路径是否存在
                    if (!Directory.Exists(imgPathNow))
	                {
	                    //不存在则创建
                        Directory.CreateDirectory(imgPathNow);
	                }
	                WebClient myWebClient = new WebClient();
	                string imgPath = imgPathNow + @"\" + ExtStatic.ToMD5_32(imgUrl) + "." + imgType;
	                myWebClient.DownloadFile(new Uri(imgUrl), imgPath);
	                imgErrorCount = 0;
	                flg = true;
	            }
	            catch (Exception ex)
	            {
	                flg = false;
                    imgErrorCount--;
	                CLog.FileIoLog($"FileIoHelp.ImgDown>>>{ex.Message}", "ImgDown-Error");	               
	            }
	        }
	        return flg;
	    }

        /// <summary>
        /// 序列化对象集合并存储到指定路径
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="list">泛型对象集合</param>
        /// <param name="filePathName">序列化对象集合文件存储的路径加文件名</param>
        /// <returns>失败记录在运行路径Log\\SerializeMethod下</returns>
        public static bool SerializeMethod<T>(List<T> list, string filePathName)
        {
            try
            {
                using (FileStream fs = new FileStream(filePathName, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, list);
                    return true;
                }
            }
            catch (Exception ex)
            {
                CLog.Log($"FileIoHelp.SerializeMethod>>>{ex.Message}", $"SerializeMethod\\SerializeMethod-Error_{DateTime.Now:yyyyMMdd}");
                return false;
            }
        }

        /// <summary>
        /// 反序列化指定路径下的文件到对象集合
        /// </summary>
        /// <param name="filePathName">文件所在路径加文件名</param>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <returns>失败返回null，失败记录在运行路径Log\\ReserializeMethod下</returns>
        public static List<T> ReserializeMethod<T>(string filePathName)
        {
            try
            {
                using (FileStream fs = new FileStream(filePathName, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    List<T> list = (List<T>)bf.Deserialize(fs);
                    return list;
                }
            }
            catch (Exception ex)
            {
                CLog.Log($"FileIoHelp.ReserializeMethod>>>{ex.Message}", $"ReserializeMethod\\ReserializeMethod-Error_{DateTime.Now:yyyyMMdd}");
                return null;
            }
        }
    }
}
