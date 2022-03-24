using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Utils
{
    public static class ImageUtils
    {
        /// <summary>
        /// 图片转byte[]
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Image img)
        {
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    img.Save(ms, img.RawFormat);
            //    return ms.ToArray();
            //}

            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        /// byte[]转图片
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public static Image FromByteArray(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }
        
        /// <summary>
        /// 获取流数据的图片格式
        /// </summary>
        /// <param name="file"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageFormat GetImageFormat(Stream file, out string format)
        {
            if (file == null)
            {
                format = string.Empty;
                return null;
            }

            byte[] sb = new byte[2];
            file.Read(sb, 0, sb.Length);

            string strFlag = sb[0].ToString() + sb[1].ToString();
            switch (strFlag)
            {
                //JPG格式
                case "255216":
                    format = ".jpg";
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
                //GIF格式
                case "7173":
                    format = ".gif";
                    return System.Drawing.Imaging.ImageFormat.Gif;
                //BMP格式
                case "6677":
                    format = ".bmp";
                    return System.Drawing.Imaging.ImageFormat.Bmp;
                //PNG格式
                case "13780":
                    format = ".png";
                    return System.Drawing.Imaging.ImageFormat.Png;
                //其他格式
                default:
                    format = string.Empty;
                    return null;
            }
        }

        /// <summary>
        /// 获取指定Image对象的图片格式
        /// </summary>
        /// <param name="_img"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageFormat GetImageFormat(Image _img, out string format)
        {
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                format = ".jpg";
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                format = ".gif";
                return System.Drawing.Imaging.ImageFormat.Gif;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {
                format = ".png";
                return System.Drawing.Imaging.ImageFormat.Png;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {
                format = ".bmp";
                return System.Drawing.Imaging.ImageFormat.Bmp;
            }
            format = string.Empty;
            return null;
        }
        
        /// <summary>
        /// GDI方式重设大小
        /// </summary>
        /// <param name="image"></param>
        /// <param name="newWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="onlyResizeIfWider"></param>
        /// <returns></returns>
        public static Image Resize(this Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth)
            {
                newWidth = image.Width;
            }

            // 判断是以宽度为比例还是高为比例等比缩放(即图片是竖型还是宽型)
            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }
        
        /// <summary>
        /// 效果最好
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="coeff"></param>
        /// <returns></returns>
        public static Bitmap ResizeNearest(this Bitmap bmp, float coeff)
        {
            int oldWidth = bmp.Width;
            int oldHeight = bmp.Height;
            int newWidth = Convert.ToInt16(oldWidth * coeff);
            int newHeight = Convert.ToInt16(oldHeight * coeff);

            float coWidth = (float)(oldWidth - 1) / (float)(newWidth - 1);
            float coHeigth = (float)(oldHeight - 1) / (float)(newHeight - 1);

            Bitmap res = new Bitmap(newWidth, newHeight);
            int x0, y0;
            for (int y = 0; y < newHeight; y++)
            {
                y0 = Convert.ToInt16(y * coHeigth);
                for (int x = 0; x < newWidth; x++)
                {
                    x0 = Convert.ToInt16(x * coWidth);
                    Color pixel = bmp.GetPixel(x0, y0);
                    res.SetPixel(x, y, pixel);
                }
            }

            return res;
        }

        /// <summary>
        ///  双线性
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="coeff"></param>
        /// <returns></returns>
        public static Bitmap ResizeBilinear(this Bitmap bmp, float coeff)
        {
            int oldWidth = bmp.Width;
            int oldHeight = bmp.Height;
            int newWidth = Convert.ToInt16(oldWidth * coeff);
            int newHeight = Convert.ToInt16(oldHeight * coeff);

            float coWidth = (float)(oldWidth - 1) / (float)(newWidth - 1);
            float coHeigth = (float)(oldHeight - 1) / (float)(newHeight - 1);

            Bitmap res = new Bitmap(newWidth, newHeight);
            float x, y;
            int x1, y1;
            int x2, y2;
            for (int newY = 0; newY < newHeight; newY++)
            {
                for (int newX = 0; newX < newWidth; newX++)
                {
                    x = newX * coWidth;
                    y = newY * coHeigth;
                    x1 = Convert.ToInt16(Math.Floor(x));
                    y1 = Convert.ToInt16(Math.Floor(y));
                    if (x1 > oldWidth - 2) x1 = oldWidth - 2;
                    if (y1 > oldHeight - 2) y1 = oldHeight - 2;
                    x2 = x1 + 1;
                    y2 = y1 + 1;

                    Color Q11 = bmp.GetPixel(x1, y1);
                    Color Q12 = bmp.GetPixel(x1, y2);
                    Color Q21 = bmp.GetPixel(x2, y1);
                    Color Q22 = bmp.GetPixel(x2, y2);
                    int R = BilinearValue(x, y, x1, y1, x2, y2, Q11.R, Q21.R, Q22.R);
                    int G = BilinearValue(x, y, x1, y1, x2, y2, Q11.G, Q21.G, Q22.G);
                    int B = BilinearValue(x, y, x1, y1, x2, y2, Q11.B, Q21.B, Q22.B);
                    Color pixel = Color.FromArgb(R, G, B);
                    res.SetPixel(newX, newY, pixel);
                }
            }

            return res;
        }

        private static int BilinearValue(float x, float y, int x1, int y1, int x2, int y2,
            int Q11, int Q21, int Q22)
        {
            int R1;
            int R2;
            int P;
            R1 = Convert.ToInt16((x2 - x1) * Q11 + (x - x1) * Q21);
            R2 = Convert.ToInt16((x2 - x) * Q11 + (x - x1) * Q22);
            P = Convert.ToInt16((y2 - y) * R1 + (y - y1) * R2);
            if (P < 0) P = 0;
            if (P > 255) P = 255;
            return P;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bitmap ConvertTo1Bpp(Bitmap source)
        {
            PixelFormat sourcePf = source.PixelFormat;
            if ((sourcePf & PixelFormat.Indexed) == 0 || Image.GetPixelFormatSize(sourcePf) == 1)
                return BitmapTo1Bpp(source);
            using (Bitmap bm32 = new Bitmap(source))
                return BitmapTo1Bpp(bm32);
        }
        
        public static Bitmap BitmapTo1Bpp(Bitmap source)
        {
            Rectangle rect = new Rectangle(0, 0, source.Width, source.Height);
            Bitmap dest = new Bitmap(rect.Width, rect.Height, PixelFormat.Format1bppIndexed);
            dest.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            BitmapData sourceData = source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format1bppIndexed);
            BitmapData targetData = dest.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);
            int actualDataWidth = (rect.Width + 7) / 8;
            int h = source.Height;
            int origStride = sourceData.Stride;
            int targetStride = targetData.Stride;
            // buffer for one line of image data.
            byte[] imageData = new byte[actualDataWidth];
            long sourcePos = sourceData.Scan0.ToInt64();
            long destPos = targetData.Scan0.ToInt64();
            // Copy line by line, skipping by stride but copying actual data width
            for (int y = 0; y < h; y++)
            {
                Marshal.Copy(new IntPtr(sourcePos), imageData, 0, actualDataWidth);
                Marshal.Copy(imageData, 0, new IntPtr(destPos), actualDataWidth);
                sourcePos += origStride;
                destPos += targetStride;
            }
            dest.UnlockBits(targetData);
            source.UnlockBits(sourceData);
            return dest;
        }

        /// <summary>
        /// 替换Image.FromFile内存不足问题
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Bitmap FromFile(string fileName)
        {
            // 打开文件    
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]    
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream    
            Stream stream = new MemoryStream(bytes);

            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始    
            stream.Seek(0, SeekOrigin.Begin);

            MemoryStream mstream = null;
            try
            {
                mstream = new MemoryStream(bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (ArgumentException )
            {
                return null;
            }
            finally
            {
                stream.Close();
            }
        }
        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }


        /// <summary>
        /// 读取图片文件, 并设置新的宽度和DPI, 转化为单色, 输出为字节数组
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="xDPI"></param>
        /// <param name="yDPI"></param>
        /// <returns></returns>
        public static byte[] ConvertToMonochrome(string sourceFile, int newWidth, int newHeight, float xDPI, float yDPI)
        {
            var source = new Bitmap(sourceFile);
            float coeff;
            if (source.Width > source.Height)
            {

                coeff = newHeight * 1f / source.Height;
            }
            else
            {
                coeff = newWidth * 1f / source.Width;
            }

            // 1.重设大小
            Bitmap newBitmap = source.ResizeNearest(coeff);
            // 2.重设DPI
            newBitmap.SetResolution(xDPI, yDPI);
            // 3.减少颜色深度
            newBitmap = ImageUtils.ConvertTo1Bpp(newBitmap);
            // 4.输出字节码
            // 这里按原图格式保存(一般是.PNG文件), 在读取的时候再转化成PCX格式
            var bytes = newBitmap.ToByteArray();

            // 5.测试代码: 转换为pcx格式
            //using (var image = new MagickImage())
            //{
            //    var settings = new MagickReadSettings();
            //    settings.UseMonochrome = true;  // 这一句貌似可以转成1BPP
            //    settings.ColorSpace = ColorSpace.Transparent;

            //    image.Read(bytes, settings);
            //    // Magick类库会把1BPP的图片反色, 这里再次反色还原
            //    image.Negate(true);      
            //    image.Format = MagickFormat.Pcx;
            //    image.ColorType = ColorType.Grayscale;
            //    image.BitDepth(2); 
            //    image.Quantize(new QuantizeSettings() { Colors = 2, DitherMethod = DitherMethod.No });
            //    image.Write("converted.pcx");
            //}


            return bytes;

        }



    }
}
