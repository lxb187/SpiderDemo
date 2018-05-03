#region 版权信息
/* ======================================================================== 
 * 描述信息   
 * 作者：lxb@jiuweiwang.com
 * 计算机：LXB-PC   
 * 时间：2018/4/11 19:00:14 
 * CLR：4.0.30319.42000 
 * 功能描述：
 * 
 * 修改者：           
 * 时间：               
 * 修改说明： 
* ======================================================================== 
*/
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SpiderHelp.ExtStaticModule
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public static class ImgHelp
    {
        /// <summary>
        /// 从大图中截取一部分图片
        /// </summary>
        /// <param name="fromImage">来源图片</param>
        /// <param name="offsetX">从偏移X坐标位置开始截取</param>
        /// <param name="offsetY">从偏移Y坐标位置开始截取</param>
        /// <param name="width">保存图片的宽度</param>
        /// <param name="height">保存图片的高度</param>
        /// <returns>图片</returns>
        public static Image CaptureImage(Image fromImage, int offsetX, int offsetY, int width, int height)
        {
            //创建新图位图
            Bitmap bitmap = new Bitmap(width, height);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
            graphic.DrawImage(fromImage, 0, 0, new Rectangle(offsetX, offsetY, width, height), GraphicsUnit.Pixel);
            //从作图区生成新图
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //释放资源   
            //saveImage.Dispose();
            graphic.Dispose();
            bitmap.Dispose();
            return saveImage;
        }

        /// <summary>
        /// 根据流显图
        /// </summary>
        /// <param name="photo">图片字节</param>
        /// <returns>图片</returns>
        public static Image ShowPic(byte[] photo)
        {
            byte[] bytes = photo;
            MemoryStream ms = new MemoryStream(bytes)
            {
                Position = 0
            };
            Image img = Image.FromStream(ms);
            ms.Close();
            return img;
        }
    }
}