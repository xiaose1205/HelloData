namespace HelloData.Util
{
    public class UP_img
    {
        /// 生成缩略图
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
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
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),//在指定位置并且按指定大小绘制原图片的指定部分
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);  //以jpg格式保存缩略图
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
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        public void AddWater(string Path, string Path_sy)
        {
            string addText = "ROYcms!NT";
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
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();
            image.Save(Path_syp);
            image.Dispose();
        }
    }
}

 //UP_img up_img = new UP_img();
 //       if (FileUpload1.HasFile)
 //       {
 //           string fileContentType = FileUpload1.PostedFile.ContentType;
 //           if (fileContentType == "image/bmp" || fileContentType == "image/gif" || fileContentType == "image/pjpeg")
 //           {
 //               string name = FileUpload1.PostedFile.FileName;                         // 客户端文件路径
 //               FileInfo file = new FileInfo(name);
 //               string fileName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+file.Name;                                           // 文件名称
 //               //string fileName_s = "x_" + file.Name;                                  // 缩略图文件名称
 //               //string fileName_sy = "text_" + file.Name;                              // 水印图文件名称（文字）
 //               //string fileName_syp = "w_" + file.Name;                            // 水印图文件名称（图片）
 //               string webFilePath = Server.MapPath("../ImgUpload/" + fileName);          // 服务器端文件路径
 //               string webFilePath_s = Server.MapPath("../ImgUpload/S_" + fileName);　　  // 服务器端缩略图路径
 //               //string webFilePath_sy = Server.MapPath("ImgUpload/" + fileName_sy);　  // 服务器端带水印图路径(文字)
 //               string webFilePath_syp = Server.MapPath("../ImgUpload/W_" + fileName);　// 服务器端带水印图路径(图片)
 //               string webFilePath_sypf = Server.MapPath("../ImgUpload/water.png");　               // 服务器端水印图路径(图片)
 //               try
 //               {
 //                   FileUpload1.SaveAs(webFilePath);                                   // 使用 SaveAs 方法保存文件
 //                   //AddWater(webFilePath, webFilePath_sy);
 //                   up_img.AddWaterPic(webFilePath, webFilePath_syp, webFilePath_sypf);
 //                   up_img.MakeThumbnail(webFilePath, webFilePath_s, 130, 130, "W");          // 生成缩略图方法
 //                   Response.Write("文件上传成功!");
 //                   TextBox_img.Text = fileName;
 //                   System.IO.File.Delete(webFilePath);
 //               }
 //               catch (Exception ex)
 //               {
 //                   Response.Write("文件上传失败!");
 //               }
 //           }
 //           else
 //           {
 //               Response.Write("文件类型不符!");
 //           }
 //       }