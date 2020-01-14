using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace System.Drawing
{
    public static partial class GraphicsExtension
    {
        #region 扩展方法

        #region 绘制透明图片

        /// <summary>
        ///  使用AlphaBlend API绘制带Alpha通道的半透明图片
        /// </summary>
        /// <param name="gx">Destination graphics</param>
        /// <param name="image">The image to draw</param>
        /// <param name="transparency">transparent factor</param>
        /// <param name="x">X Location</param>
        /// <param name="y">Y Location</param>
        public static void DrawAlpha(this Graphics gx, Bitmap image, byte transparency, int x, int y)
        {
            using (Graphics gxSrc = Graphics.FromImage(image))
            {
                IntPtr hdcDst = gx.GetHdc();
                IntPtr hdcSrc = gxSrc.GetHdc();
                BlendFunction blendFunction = new BlendFunction();
                blendFunction.BlendOp = (byte)BlendOperation.AC_SRC_OVER;   // Only supported blend operation
                blendFunction.BlendFlags = (byte)BlendFlags.Zero;           // Documentation says put 0 here
                blendFunction.SourceConstantAlpha = transparency;           // Constant alpha factor
                blendFunction.AlphaFormat = (byte)0;                        // Don't look for per pixel alpha
                Win32.AlphaBlend(hdcDst, x, y, image.Width, image.Height, 
                               hdcSrc, 0, 0, image.Width, image.Height, blendFunction);
                gx.ReleaseHdc(hdcDst);                                      // Required cleanup to GetHdc()
                gxSrc.ReleaseHdc(hdcSrc);                                   // Required cleanup to GetHdc()
            }
        }

        //public static void DrawAlpha(this Graphics gx, IImage image, byte transparency, int x, int y, int width, int height)
        //{ 
        //    ImageInfo imgInfo;
        //    image.GetImageInfo(out imgInfo);

        //    Bitmap buffer = new Bitmap((int)imgInfo.Width, (int)imgInfo.Height, PixelFormat.Format32bppRgb);

        //    using (Graphics gxBuffer = Graphics.FromImage(buffer))
        //    {
        //        IntPtr hdcBuffer = gxBuffer.GetHdc();
        //        IntPtr hdcOrg = gx.GetHdc();

        //        // Copy original DC into Buffer DC to see the background through the image
        //        WinGDI.BitBlt(hdcBuffer, 0, 0, (int)imgInfo.Width, (int)imgInfo.Height, hdcOrg, x, y, TernaryRasterOperations.SRCCOPY);

        //        // Draw the image, with alpha channel if any
        //        Rectangle dstRect = new Rectangle(0, 0, (int)imgInfo.Width, (int)imgInfo.Height);
        //        image.Draw(hdcBuffer, ref dstRect, IntPtr.Zero);

        //        // Alpha blend image
        //        BlendFunction blendFunction = new BlendFunction();
        //        blendFunction.BlendOp = (byte)BlendOperation.AC_SRC_OVER;  // Only supported blend operation
        //        blendFunction.BlendFlags = (byte)BlendFlags.Zero;           // Documentation says put 0 here
        //        blendFunction.SourceConstantAlpha = (byte)transparency;			// Constant alpha factor
        //        blendFunction.AlphaFormat = 0;                              // Don't look for per pixel alpha

        //        WinGDI.AlphaBlend(hdcOrg, x, y, width, height,
        //                              hdcBuffer, 0, 0, (int)imgInfo.Width, (int)imgInfo.Height, blendFunction);

        //        gx.ReleaseHdc(hdcOrg);              // Required cleanup to GetHdc()
        //        gxBuffer.ReleaseHdc(hdcBuffer);     // Required cleanup to GetHdc()
        //    }

        //    buffer.Dispose();
        //}
        /// <summary>
        ///  在指定坐标绘制带Alpha通道的半透明图片(使用Image COM对象).
        /// </summary>
        /// <param name="gx">Destination graphics</param>
        /// <param name="image">The image to draw</param>
        /// <param name="x">X Location</param>
        /// <param name="y">Y Location</param>
        //public static void DrawImageAlphaChannel(this Graphics gx, IImage image, int x, int y)
        //{
        //    ImageInfo imageInfo = new ImageInfo();
        //    image.GetImageInfo(out imageInfo);
        //    Rectangle rc = new Rectangle(x, y, (int)imageInfo.Width + x, (int)imageInfo.Height + y);
        //    IntPtr hdc = gx.GetHdc();
        //    image.Draw(hdc, ref rc, IntPtr.Zero);
        //    gx.ReleaseHdc(hdc);
        //}
        /// <summary>
        ///  在指定的区域绘制带Alpha通道的半透明图片(使用Image COM对象).
        /// </summary>
        /// <param name="gx">目标画布</param>
        /// <param name="image">要绘制的图形</param>
        /// <param name="dest">目标区域</param>
        //public static void DrawImageAlphaChannel(this Graphics gx, IImage image, Rectangle dest)
        //{
        //    //Rectangle rc = new Rectangle(dest.X, dest.Y, dest.Width, dest.Height);
        //    //Rectangle rc = new Rectangle(dest.X, dest.Y, dest.Width + dest.X, dest.Height + dest.Y);
        //    Rectangle rc = new Rectangle(dest.X, dest.Y, dest.Width + dest.X, dest.Height + dest.Y);
        //    IntPtr hdc = gx.GetHdc();
        //    image.Draw(hdc, ref rc, IntPtr.Zero);
        //    gx.ReleaseHdc(hdc);
        //}
        /// <summary>
        /// 绘制一个指定的大小的透明背景图片. (使用托管对象, 不支持半透明)
        /// </summary>
        /// <param name="gx">Destination graphics</param>
        /// <param name="image">The image to draw</param>
        /// <param name="destRect">Desctination rectangle</param>
        public static void DrawImageTransparent(this Graphics gx, Image image, Rectangle destRect)
        {
            ImageAttributes imageAttr = new ImageAttributes();
            Color transpColor = GetTransparentColor(image);
            imageAttr.SetColorKey(transpColor, transpColor);
            gx.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            imageAttr.Dispose();
        }

        public static void DrawImageTransparent(this Graphics gx, Image image, Rectangle destRect, Color transpColor)
        {
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(transpColor, transpColor);
            gx.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            imageAttr.Dispose();
        }

        /// <summary>
        /// 在指定位置绘制指定透明度的图片
        /// </summary>
        /// <param name="g">Graphics对象</param>
        /// <param name="source">源图片</param>
        /// <param name="opacity">透明度. 值范围为0.0 - 1.0</param>
        /// <param name="location">X, Y坐标位置</param>
        public static void DrawImageOpacity(this Graphics g, Bitmap source, float opacity, Point location)
        {
            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = opacity;

            // 设定颜色属性
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            g.DrawImage(source, new Rectangle(location.X, location.Y, source.Width, source.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
        }


        #endregion

        #region 填充矩形

        /// <summary>
        /// 返回指定渐变色和方向的填充矩形
        /// </summary>
        /// <param name="rc">Destination rectangle</param>
        /// <param name="startColorValue">Starting color for gradient</param>
        /// <param name="endColorValue">End color for gradient</param>
        /// <param name="fillDirection">The direction of the gradient</param>
        /// <returns>Image of the rectanle</returns>
        public static Bitmap GetGradientRectangle(Rectangle rc, Color startColorValue, Color endColorValue, FillDirection fillDirection)
        {
            Bitmap outputImage = new Bitmap(rc.Width, rc.Height);
            // Create temporary graphics
            Graphics gx = Graphics.FromImage(outputImage);

            GradientFill.Fill(
              gx,
              rc,
              startColorValue,
              endColorValue,
              fillDirection);

            return outputImage;
        }

        /// <summary>
        /// 使用指定的渐变色和方向填充一个矩形 Fills the rectagle with gradient colors
        /// </summary>
        /// <param name="gx">Destination graphics</param>
        /// <param name="rc">Desctination rectangle</param>
        /// <param name="startColorValue">Starting color for gradient</param>
        /// <param name="endColorValue">End color for gradient</param>
        /// <param name="fillDirection">The direction of the gradient</param>
        public static void FillGradientRectangle(this Graphics gx, Rectangle rc, Color startColorValue, Color endColorValue, FillDirection fillDirection)
        {
            GradientFill.Fill(
              gx,
              rc,
              startColorValue,
              endColorValue,
              fillDirection);
        }

        #endregion
        
        /// <summary>
        /// 绘制文本自动换行（超出截断）
        /// </summary>
        /// <param name="graphic">绘图图面</param>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="recangle">绘制范围</param>
        public static void DrawStringWrap(this Graphics graphic, string text, Font font, Brush textBrush, Rectangle recangle)
        {
            List<string> textRows = GetStringRows(graphic, font, text, recangle.Width);
            int rowHeight = (int)(Math.Ceiling((decimal)TextRenderer.MeasureText("测试", font).Height));
            int maxRowCount = recangle.Height / rowHeight;
            int drawRowCount = (maxRowCount < textRows.Count) ? maxRowCount : textRows.Count;
            int top = (recangle.Height - rowHeight * drawRowCount) / 2;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < drawRowCount; i++)
            {
                Rectangle fontRectanle = new Rectangle(recangle.Left, recangle.Top + top + rowHeight * i, recangle.Width, rowHeight);
                graphic.DrawString(textRows[i], font, textBrush, fontRectanle, sf);
            }
        }

        public static void CopyFromScreen(this Graphics g, int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
        {
            IntPtr desktopWindow = Win32.GetDesktopWindow();
            if (desktopWindow == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            IntPtr windowDC = Win32.GetWindowDC(desktopWindow);
            if (windowDC == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }

            IntPtr hDC = g.GetHdc();
            if (!Win32.BitBlt(
                hDC, destinationX, destinationY, blockRegionSize.Width, blockRegionSize.Height, windowDC, sourceX, sourceY, copyPixelOperation))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            Win32.ReleaseDC(desktopWindow, windowDC);
        }

        public static void SetFastRendering(this Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
        }
        public static void SetSlowRendering(this Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }

        public static Image MakeGrayscale3(this Image original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.SmoothingMode = SmoothingMode.None;
                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                       new float[][] 
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

                //create some image attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    //set the color matrix attribute
                    attributes.SetColorMatrix(colorMatrix);

                    //draw the original image on the new image
                    //using the grayscale color matrix
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                       0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                    //dispose the Graphics object
                }
            }

            return newBitmap;
        }


        public static Image MakeHighlight(this Image original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
            {
                new float[] {1.05f, 0.00f, 0.00f, 0.00f,0.00f},
                new float[] {0.00f, 1.05f, 0.00f, 0.00f, 0.00f},
                new float[] {0.00f, 0.00f, 1.05f, 0.00f, 0.00f},
                new float[] {0.00f, 0.00f, 0.00f, 1.00f, 0.00f},
                new float[] {0.05f, 0.05f, 0.05f, 0.00f, 1.00f}

            });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
           
        }
             
        /// <summary>
        ///  设置图片透明度
        /// </summary>
        /// <param name="source">图片源</param>
        /// <param name="opacity">透明度. 值范围为0.0 - 1.0</param>
        /// <returns></returns>
        public static Image SetOpacity(this Image source, float opacity)
        {
            Bitmap output = new Bitmap(source.Width, source.Height);
            using (Graphics gfx = Graphics.FromImage(output))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;

                // 设定颜色属性
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                gfx.DrawImage(source, new Rectangle(0, 0, output.Width, output.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
            }
            return output;
        }

        public static byte[] ToArray(this Image source)
        {
            return ImageTool.ToArray(source);
        }

        #endregion 扩展方法

        #region 非扩展方法


        /// <summary>
        /// 将文本分行
        /// </summary>
        /// <param name="graphic">绘图图面</param>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="width">行宽</param>
        /// <returns></returns>
        internal static List<string> GetStringRows(Graphics graphic, Font font, string text, int width)
        {
            int RowBeginIndex = 0;
            int rowEndIndex = 0;
            int textLength = text.Length;
            List<string> textRows = new List<string>();
            for (int index = 0; index < textLength; index++)            {

                rowEndIndex = index;
                if (index == textLength - 1)
                {
                    textRows.Add(text.Substring(RowBeginIndex));
                }
                else if (rowEndIndex + 1 < text.Length && text.Substring(rowEndIndex, 2) == "\r\n")
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    rowEndIndex = index += 2;
                    RowBeginIndex = rowEndIndex;
                }
                else if (TextRenderer.MeasureText(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex + 1), font).Width > width)
                {
                    textRows.Add(text.Substring(RowBeginIndex, rowEndIndex - RowBeginIndex));
                    RowBeginIndex = rowEndIndex;
                }
            }

            return textRows;
        }

        /// <summary>
        ///  从gxSrc复制图像到gxDest
        /// </summary>
        /// <param name="gxSrc">指定将要复制的来源Grahpics</param>
        /// <param name="gxDest">指定将要复制到目标Graphics</param>
        /// <param name="width">指定要复制的宽度</param>
        /// <param name="height">指定要复制的高度</param>
        /// <param name="x">指定从来源Graphics的X坐标开始复制</param>
        /// <param name="y">指定从来源Graphics的Y坐标开始复制</param>
        internal static void CopyGraphics(Graphics gxSrc, Graphics gxDest, int width, int height, int x, int y)
        {
            IntPtr srcDc = gxSrc.GetHdc();
            IntPtr destDc = gxDest.GetHdc();
            Win32.BitBlt(destDc, 0, 0, width, height, srcDc, x, y, TernaryRasterOperations.SRCCOPY);
            gxSrc.ReleaseHdc(srcDc);
            gxDest.ReleaseHdc(destDc);
        }

        internal static Color GetTransparentColor(Image image)
        {
            return ((Bitmap)image).GetPixel(0, 0);
            //return ((Bitmap)image).GetPixel(image.Width - 1, image.Height - 1);
        }

        /// <summary>
        ///  取反色
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Color GetInverseColor(Color c)
        {
            return Color.FromArgb(c.R ^ 0x80, c.G ^ 0x80, c.B ^ 0x80);
        }
           
         /// <summary>  
         /// 获取对应的灰度色
         /// </summary>  
         /// <param name="c"></param>  
         /// <returns></returns>  
        public static Color GetGrayColor(Color c)  
         {  
             //要改变ARGB  
             int i = (c.R * 19595 + c.G * 38469 + c.B * 7472) >> 16;
             return Color.FromArgb(i, i, i);
        }

        /// <summary>
        /// 设置图形边缘半透明
        /// </summary>
        /// <param name="p_Bitmap">图形</param>
        /// <param name="p_CentralTransparent">true中心透明 false边缘透明</param>
        /// <param name="p_Crossdirection">true横 false纵</param>
        /// <returns></returns>
        public static Bitmap BothAlpha(Bitmap p_Bitmap, bool p_CentralTransparent, bool p_Crossdirection)
        {
            Bitmap _SetBitmap = new Bitmap(p_Bitmap.Width, p_Bitmap.Height);
            Graphics _GraphisSetBitmap = Graphics.FromImage(_SetBitmap);
            _GraphisSetBitmap.DrawImage(p_Bitmap, new Rectangle(0, 0, p_Bitmap.Width, p_Bitmap.Height));
            _GraphisSetBitmap.Dispose();

            Bitmap _Bitmap = new Bitmap(_SetBitmap.Width, _SetBitmap.Height);
            Graphics _Graphcis = Graphics.FromImage(_Bitmap);

            Point _Left1 = new Point(0, 0);
            Point _Left2 = new Point(_Bitmap.Width, 0);
            Point _Left3 = new Point(_Bitmap.Width, _Bitmap.Height / 2);
            Point _Left4 = new Point(0, _Bitmap.Height / 2);

            if (p_Crossdirection)
            {
                _Left1 = new Point(0, 0);
                _Left2 = new Point(_Bitmap.Width / 2, 0);
                _Left3 = new Point(_Bitmap.Width / 2, _Bitmap.Height);
                _Left4 = new Point(0, _Bitmap.Height);
            }

            Point[] _Point = new Point[] { _Left1, _Left2, _Left3, _Left4 };
            PathGradientBrush _SetBruhs = new PathGradientBrush(_Point, WrapMode.TileFlipY);

            _SetBruhs.CenterPoint = new PointF(0, 0);
            _SetBruhs.FocusScales = new PointF(_Bitmap.Width / 2f, 0);
            _SetBruhs.CenterColor = Color.FromArgb(0, 255, 255, 255);
            _SetBruhs.SurroundColors = new Color[] { Color.FromArgb(255, 255, 255, 255) };
            if (p_Crossdirection)
            {
                _SetBruhs.FocusScales = new PointF(0, _Bitmap.Height);
                _SetBruhs.WrapMode = WrapMode.TileFlipXY;
            }

            if (p_CentralTransparent)
            {
                _SetBruhs.CenterColor = Color.FromArgb(255, 255, 255, 255);
                _SetBruhs.SurroundColors = new Color[] { Color.FromArgb(0, 255, 255, 255) };
            }

            _Graphcis.FillRectangle(_SetBruhs, new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height));
            _Graphcis.Dispose();

            BitmapData _NewData = _Bitmap.LockBits(new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height), ImageLockMode.ReadOnly, _Bitmap.PixelFormat);
            byte[] _NewBytes = new byte[_NewData.Stride * _NewData.Height];
            Marshal.Copy(_NewData.Scan0, _NewBytes, 0, _NewBytes.Length);
            _Bitmap.UnlockBits(_NewData);

            BitmapData _SetData = _SetBitmap.LockBits(new Rectangle(0, 0, _SetBitmap.Width, _SetBitmap.Height), ImageLockMode.ReadWrite, _SetBitmap.PixelFormat);
            byte[] _SetBytes = new byte[_SetData.Stride * _SetData.Height];
            Marshal.Copy(_SetData.Scan0, _SetBytes, 0, _SetBytes.Length);
            int _WriteIndex = 0;
            for (int i = 0; i != _SetData.Height; i++)
            {
                _WriteIndex = i * _SetData.Stride + 3;
                for (int z = 0; z != _SetData.Width; z++)
                {
                    _SetBytes[_WriteIndex] = _NewBytes[_WriteIndex];
                    _WriteIndex += 4;
                }
            }
            Marshal.Copy(_SetBytes, 0, _SetData.Scan0, _SetBytes.Length);
            _SetBitmap.UnlockBits(_SetData);
            return _SetBitmap;
        }


    
        public static GraphicsPath CreateDownTriangleFlag(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();

            int x = rect.X + (rect.Width - 10) / 2;
            int y = rect.Y + (rect.Height - 9) / 2;

            if (rect.Height % 2 == 0)
                y++;

            Point p1 = new Point(x, y);
            Point p2 = new Point(x + 9, y);
            Point p3 = new Point(x + 9, y + 1);
            Point p4 = new Point(x, y + 1);

            path.AddLines(new Point[] { p1, p2, p3, p4 });
            path.CloseFigure();

            int x1 = x, y1 = y + 4, x2 = x + 9, y2 = y + 4;
            for (int i = 1; i <= 5; i++)
            {
                if (i % 2 == 0)
                    path.AddLine(x2, y2, x1, y1);
                else
                    path.AddLine(x1, y1, x2, y2);
                x1++;
                x2--;
                y1++;
                y2++;
            }

            return path;
        }
        
        // 画倒三角
        public static GraphicsPath Create7x4In7x7DownTriangleFlag(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();

            int x = rect.X + (rect.Width - 7) / 2;
            int y = rect.Y + (rect.Height - 7) / 2 + 2;

            int x1 = x, x2 = x + 6;

            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                    path.AddLine(x1, y, x2, y);
                else
                    path.AddLine(x2, y, x1, y);
                x1++;
                x2--;
                y++;
            }
            path.CloseFigure();
            return path;
        }


        public static void SelectBitmapIntoLayerWindow(Form win, Bitmap bitmap, byte opacity)
        {
            // 判断位图是否包含Alpha通道
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ApplicationException("必须是带Alpha通道的32位位图");
            }



            // 获取设备上下文DC
            IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            try
            {
                // 获取插入到设备上下文位图的句柄
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = Win32.SelectObject(memDc, hBitmap);

                // 设置层叠窗口更新的参数
                Size newSize = new Size(bitmap.Width, bitmap.Height);	// 设置窗口同位图大小
                Point sourceLocation = new Point(0, 0);
                Point newLocation = new Point(win.Left, win.Top);
                BlendFunction blend = new BlendFunction();
                blend.BlendOp = (byte)BlendOp.AC_SRC_OVER; // 仅支持32位位图
                blend.BlendFlags = 0;											// 总是 0
                blend.SourceConstantAlpha = opacity;					// 像素的Alpha通道值
                blend.AlphaFormat = (byte)BlendOp.AC_SRC_ALPHA;// 仅支持含Alpha通道的位置

                // 更新层叠窗口
                Win32.UpdateLayeredWindow(win.Handle,
                    screenDc, ref newLocation, ref newSize, memDc, ref sourceLocation, 0, ref blend, (int)ULWPara.ULW_ALPHA);
            }
            finally
            {
                // 释放设备上下文
                Win32.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, hOldBitmap);
                    Win32.DeleteObject(hBitmap);										// 删除GDI+位图
                }
                Win32.DeleteDC(memDc);
            }
        }



        #endregion 非扩展方法



        [ThreadStatic]
        static Bitmap staticBitmap;
        static bool? canCreateFromZeroHwnd;
        public static bool CanCreateFromZeroHwnd
        {
            get
            {
                if (!canCreateFromZeroHwnd.HasValue)
                    try
                    {
                        using (Graphics g = CreateGraphicsFromZeroHwnd())
                            canCreateFromZeroHwnd = true;
                    }
                    catch
                    {
                        canCreateFromZeroHwnd = false;
                    }
                return canCreateFromZeroHwnd.Value;
            }
        }


        public static Graphics CreateGraphics()
        {
            return CreateGraphicsCore();
        }
        static Graphics CreateGraphicsCore()
        {
            if (CanCreateFromZeroHwnd)
            {
                Graphics gr = CreateGraphicsFromZeroHwnd();
                if (gr.DpiX != 96.0 || gr.DpiY != 96.0)
                    gr.Dispose();
                else
                    return gr;
            }
            return CreateGraphicsFromImage();
        }
        static Graphics CreateGraphicsFromImage()
        {
            if (staticBitmap == null)
            {
                staticBitmap = new Bitmap(10, 10, PixelFormat.Format32bppArgb);
                staticBitmap.SetResolution(96f, 96f);
            }
            return Graphics.FromImage(staticBitmap);
        }
        static Graphics CreateGraphicsFromZeroHwnd()
        {
            return Graphics.FromHwnd(IntPtr.Zero);
        }
    }
}
