using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpiderHelp.SaveModule
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class CLog
    {
        //读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
        private static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        /// <summary>
        /// 写入信息日志方法,默认的路径为运行目录下的Log的日志文件夹以.log文件名结尾
        /// </summary>
        /// <param name="strLog">日记内容</param>
        /// <param name="sFilePathName">程序运行路径Log下的路径带上文件名</param>
        public static void Log(string strLog, string sFilePathName)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
                LogWriteLock.EnterWriteLock();
                //文件的绝对路径
                string sFullName = $"{System.AppDomain.CurrentDomain.BaseDirectory}\\Log\\{sFilePathName}.log";
                //验证路径是否存在
                if (!Directory.Exists("Log"))
                {
                    //不存在则创建
                    Directory.CreateDirectory("Log");
                }
                File.AppendAllText(sFullName, $"******{DateTime.Now}******\r\n{strLog}\r\n", Encoding.UTF8);
            }           
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 写入信息日志方法,默认的路径为运行目录下的Log的日志文件夹以时间和.log文件名结尾。
        /// </summary>
        /// <param name="message">日志信息</param>
        public static void DiaryLog(string message)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
                LogWriteLock.EnterWriteLock();
                string filePath = $"Diary_Log\\{DateTime.Now:yyyy-MM-dd}";
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                string fullName = string.Format(
                    System.AppDomain.CurrentDomain.BaseDirectory + "{0}\\DiaryLog_{1}.txt",
                    filePath, DateTime.Now.ToString("yyyyMMddHH"));
                System.IO.File.AppendAllText(fullName, $"******{DateTime.Now}******\r\n{message}\r\n",
                    System.Text.Encoding.GetEncoding("utf-8"));
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 写入自定义日志文件信息
        /// </summary>
        /// <param name="message">写入的信息</param>
        /// <param name="title">文件名称：.log默认后缀</param>
        public static void DiaryLog(string message, string title)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
                LogWriteLock.EnterWriteLock();
                string filePath = $"Diary_Log\\{DateTime.Now:yyyy-MM-dd}";
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                string fullName = string.Format(
                    System.AppDomain.CurrentDomain.BaseDirectory + "{0}\\{1}_DiaryLog_{2}.txt",
                    filePath, title, DateTime.Now.ToString("yyyyMMddHH"));
                System.IO.File.AppendAllText(fullName, $"******{DateTime.Now}******\r\n{message}\r\n",
                    System.Text.Encoding.GetEncoding("utf-8"));
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 数据库日志文件信息
        /// </summary>
        /// <param name="message">写入的信息</param>
        /// <param name="title">文件名称：.txt默认后缀</param>
        public static void DbLog(string message, string title)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
                LogWriteLock.EnterWriteLock();
                string filePath = $"Db_Log\\{DateTime.Now:yyyy-MM-dd}";
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string fullName = string.Format(
                    System.AppDomain.CurrentDomain.BaseDirectory + "{0}\\{1}_DbLog_{2}.txt",
                    filePath, title, DateTime.Now.ToString("yyyyMMddHH"));
                System.IO.File.AppendAllText(fullName,
                    $"******{DateTime.Now}******\r\n{message}\r\n",
                    System.Text.Encoding.GetEncoding("utf-8"));
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 文件读写日志文件信息
        /// </summary>
        /// <param name="message">写入的信息</param>
        /// <param name="title">文件名称：.txt默认后缀</param>
        public static void FileIoLog(string message, string title)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
                LogWriteLock.EnterWriteLock();
                string filePath = $"FileIo_Log\\{DateTime.Now:yyyy-MM-dd}";
                if(!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                string fullName = string.Format(
                    System.AppDomain.CurrentDomain.BaseDirectory + "{0}\\{1}_FileIo_{2}.txt",
                    filePath, title, DateTime.Now.ToString("yyyyMMddHH"));
                System.IO.File.AppendAllText(fullName, $"******{DateTime.Now}******\r\n{message}\r\n",
                    System.Text.Encoding.GetEncoding("utf-8"));
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 邮件发送日志文件信息
        /// </summary>
        /// <param name="message">写入的信息</param>
        /// <param name="title">文件名称：.txt默认后缀</param>
        public static void EmailLog(string message, string title)
        {
            try
            {
                //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入		        
                LogWriteLock.EnterWriteLock();
                string filePath = $"Email_Log\\{DateTime.Now:yyyy-MM-dd}";
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string fullName = string.Format(
                    System.AppDomain.CurrentDomain.BaseDirectory + "{0}\\{1}_EmailLog_{2}.txt",
                    filePath, title, DateTime.Now.ToString("yyyyMMddHH"));
                System.IO.File.AppendAllText(fullName,
                    $"******{DateTime.Now}******\r\n{message}\r\n",
                    System.Text.Encoding.GetEncoding("utf-8"));
            }
            finally
            {
                //退出写入模式，释放资源占用
                LogWriteLock.ExitWriteLock();
            }
        }
    }
}
