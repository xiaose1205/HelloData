
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Net;

namespace HelloData.Util
{
    /// <summary>
    /// 一些常用的static函数
    /// </summary>
    public class staticFunction
    {
        public static string AppPath()
        {
            string _ApplicationPath = HttpContext.Current.Request.ApplicationPath;
            if (_ApplicationPath != "/")
                _ApplicationPath += "/";
            return _ApplicationPath;
        }
        #region IP地址互转整数
        /// <summary>
        /// 将IP地址转为整数形式
        /// </summary>
        /// <returns>整数</returns>
        public static long IP2Long(IPAddress ip)
        {
            int x = 3;
            long o = 0;
            foreach (byte f in ip.GetAddressBytes())
            {
                o += (long)f << 8 * x--;
            }
            return o;
        }
        /// <summary>
        /// 将整数转为IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static IPAddress Long2IP(long l)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[3 - i] = (byte)(l >> 8 * i & 255);
            }
            return new IPAddress(b);
        }
        #endregion
        #region 时间处理
        /// <summary>
        /// 获得当前时间
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNow(string type)
        {
            if (type == "date")
            {
                return DateTime.Now.ToShortDateString();
            }
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion
        #region 图片处理
        #region 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbPhoto(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion
        #region 在图片上增加文字水印
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        /// <param name="addText">水印文字</param>
        protected static void AddWater(string Path, string Path_sy, string addText)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }
        #endregion

        #region 在图片上生成图片水印
        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkFilename">水印文件名</param>
        /// <param name="watermarkStatus">图片水印位置:0=不使用 1=左上 2=中上 3=右上 4=左中 ... 9=右下</param>
        /// <param name="quality">是否是高质量图片 取值范围0--100</param> 
        /// <param name="watermarkTransparency">图片水印透明度 取值范围1--10 (10为不透明)</param>

        public static void AddImageSignPic(string Path, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img);

            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            System.Drawing.Image watermark = new System.Drawing.Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
            {
                return;
            }

            System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
            System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();

            colorMap.OldColor = System.Drawing.Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            System.Drawing.Imaging.ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency / 10.0F);
            }

            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new System.Drawing.Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, System.Drawing.GraphicsUnit.Pixel, imageAttributes);

            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo ici = null;
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                {
                    ici = codec;
                }
            }
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 80;
            }
            qualityParam[0] = quality;

            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
            {
                img.Save(filename, ici, encoderParams);
            }
            else
            {
                img.Save(filename);
            }

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        protected static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }
        #endregion
        #endregion
        #region sql语句生成代码
        /// <summary>
        /// 分页sql语句生成代码
        /// </summary>
        /// <param name="SelectFields"></param>
        /// <param name="TblName"></param>
        /// <param name="FldName">排序字段,唯一性</param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="OrderType"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>

        public static string GetSql(string SelectFields, string TblName, string FldName, int PageSize, int PageIndex, string OrderType, string whereStr)
        {
            string StrTemp = "";
            string StrSql = "";
            string StrOrder = "";
            //根据排序方式生成相关代码
            if (OrderType.ToUpper() == "ASC")
            {
                StrTemp = "> (SELECT MAX(" + FldName + ")";
                StrOrder = " ORDER BY " + FldName + " ASC";
            }
            else
            {
                StrTemp = "< (SELECT MIN(" + FldName + ")";
                StrOrder = " ORDER BY " + FldName + " DESC";
            }
            PageIndex = Convert.ToInt32("0" + PageIndex.ToString());
            PageIndex = PageIndex == 0 ? 1 : PageIndex;
            //若是第1页则无须复杂的语句
            if (PageIndex == 1)
            {
                StrTemp = "";
                if (whereStr != "")
                    StrTemp = " Where " + whereStr;
                StrSql = "SELECT TOP " + PageSize + " " + SelectFields + " From " + TblName + "" + StrTemp + StrOrder;
            }
            else
            {
                //若不是第1页，构造sql语句
                StrSql = "SELECT TOP " + PageSize + " " + SelectFields + " From " + TblName + " WHERE " + FldName + "" + StrTemp + " From (SELECT TOP " + (PageIndex - 1) * PageSize + " " + FldName + " From " + TblName + "";
                if (whereStr != "")
                    StrSql += " Where " + whereStr;
                StrSql += StrOrder + ") As Tbltemp)";
                if (whereStr != "")
                    StrSql += " And " + whereStr;
                StrSql += StrOrder;
            }
            //返回sql语句
            return StrSql;
        }

        /// <summary>
        /// 连表分页sql语句生成代码
        /// <param name="SelectFields">连表查询的字段</param>
        /// <param name="TblNameA">表1</param>
        /// <param name="TblNameB">表2</param>
        /// <param name="FldName">表1的排序字段名</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="OrderType">排序方式：DESC ASC</param>
        /// <param name="joinStr">连接条件</param>
        /// <param name="whereStr1">外围条件带A.</param>
        /// <param name="whereStr2">分页条件:不带A.</param>
        /// </summary>

        public static string GetSql(string SelectFields, string TblNameA, string TblNameB, string FldName, int PageSize, int PageIndex, string OrderType, string joinStr, string whereStr1, string whereStr2)
        {
            string StrTemp = "";
            string StrSql = "";
            string StrOrder1 = "";
            string StrOrder2 = "";
            //根据排序方式生成相关代码
            if (OrderType.ToUpper() == "ASC")
            {
                StrTemp = "> (SELECT MAX(" + FldName + ")";
                StrOrder1 = " ORDER BY A." + FldName + " ASC";
                StrOrder2 = " ORDER BY " + FldName + " ASC";
            }
            else
            {
                StrTemp = "< (SELECT MIN(" + FldName + ")";
                StrOrder1 = " ORDER BY A." + FldName + " DESC";
                StrOrder2 = " ORDER BY " + FldName + " DESC";
            }
            PageIndex = Convert.ToInt32("0" + PageIndex.ToString());
            PageIndex = PageIndex == 0 ? 1 : PageIndex;
            //若是第1页则无须复杂的语句
            if (PageIndex == 1)
            {
                StrTemp = "";
                if (whereStr1 != "")
                    StrTemp = " WHERE " + whereStr1;
                StrSql = "SELECT TOP " + PageSize + " " + SelectFields + " FROM [" + TblNameA + "] A LEFT JOIN [" + TblNameB + "] B on " + joinStr + " " + StrTemp + StrOrder1;
            }
            else
            {
                //若不是第1页，构造sql语句
                StrSql = "SELECT TOP " + PageSize + " " + SelectFields + " FROM [" + TblNameA + "] A LEFT JOIN [" + TblNameB + "] B on " + joinStr + " WHERE A." + FldName + "" + StrTemp + " From (SELECT TOP " + (PageIndex - 1) * PageSize + " " + FldName + " From [" + TblNameA + "] ";
                if (whereStr2 != "")
                    StrSql += " Where " + whereStr2;
                StrSql += StrOrder2 + ") As Tbltemp)";
                if (whereStr1 != "")
                    StrSql += " And " + whereStr1;
                StrSql += StrOrder1;
            }
            //返回sql语句
            return StrSql;
        }
        #endregion
    }
}
