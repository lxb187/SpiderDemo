using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SpiderHelp.VerificationCode
{
    /// <summary>
    /// 等比例压缩图片类
    /// </summary>
    public class GetMiniMage
    {
        /// <summary>
        /// Image压缩成Bitmap
        /// </summary>
        /// <param name="srcImage">图片资源</param>
        /// <returns>压缩后的Bitmap</returns>
        public static Bitmap PercentImage(Image srcImage)
        {
            int newW = srcImage.Width < 1130 ? srcImage.Width : 1130;
            int newH = int.Parse(Math.Round(srcImage.Height * (double)newW / srcImage.Width).ToString());
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                g.DrawImage(srcImage, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch(Exception)
            {
                return null;
            }
        }

        //将图片按百分比压缩，flag取值1到100，越小压缩比越大
        /// <summary>
        /// 将图片按百分比压缩，flag取值1到100，越小压缩比越大
        /// </summary>
        /// <param name="iSource">图片资源</param>
        /// <param name="outPath">压缩后存放的路径</param>
        /// <param name="flag">压缩值</param>
        /// <returns>是否压缩成功</returns>
        public static bool YaSuo(Image iSource, string outPath, int flag)
        {
            ImageFormat tFormat = iSource.RawFormat;
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageDecoders();
                ImageCodecInfo jpegICIinfo = null;
                for(int x = 0; x < arrayICI.Length; x++)
                {
                    if(arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if(jpegICIinfo != null)
                    iSource.Save(outPath, jpegICIinfo, ep);
                else
                    iSource.Save(outPath, tFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region GetPicThumbnail
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if(tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for(int x = 0; x < arrayICI.Length; x++)
                {
                    if(arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if(jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        #endregion
    }
}
