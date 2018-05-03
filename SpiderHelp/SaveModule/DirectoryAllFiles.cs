using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderHelp.SaveModule
{
    /// <summary>
    /// 文件遍历类
    /// </summary>
    public class DirectoryAllFiles
    {
        /// <summary>
        /// 文件信息类，包含名称和路径
        /// </summary>
        public class FileInformation
        {
            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 文件路径
            /// </summary>
            public string FilePath { get; set; }
        }
        private static List<FileInformation> FileList = new List<FileInformation>();
        /// <summary>
        /// 得到当前文件夹下面所有的文件信息
        /// </summary>
        /// <param name="dir">文件夹目录</param>
        /// <returns>文件信息</returns>
        public static List<FileInformation> GetAllFiles(DirectoryInfo dir)
        {
            FileInfo[] allFile = dir.GetFiles();
            foreach(FileInfo fi in allFile)
            {
                FileList.Add(new FileInformation { FileName = fi.Name, FilePath = fi.FullName });
            }
            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach(DirectoryInfo d in allDir)
            {
                GetAllFiles(d);
            }
            return FileList;
        }
    }
}
