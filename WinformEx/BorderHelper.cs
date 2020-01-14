using System.Drawing;
using System.Drawing.Imaging; 

namespace System.Windows.Forms
{
    /// <summary>
    /// 为控件添加外边框
    /// </summary>
    public class BorderHelper
    {
        #region 字段

        private int offset = 1;
        private Color borderColor;
        private int _diameter;

        private Control _attachedControl;

        #endregion

        #region 构造

        public BorderHelper(Control control)
        {
            this.borderColor = Color.Black;
            this.Attach(control);

        }

        #endregion

        #region 公开方法

        public void Attach(Control control)
        {
            this._attachedControl = control;
            this.SetStyle(control);
            this.HookParentPaintEvent(control);
        }

        #endregion

        #region 属性

        public int Diameter
        {
            get { return _diameter; }
            set { 
                _diameter = value;

                IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, _attachedControl.Width, _attachedControl.Height, _diameter, _diameter);
                _attachedControl.Region = System.Drawing.Region.FromHrgn(hrgn);
                _attachedControl.Update();
            
            }
        }

        public Color BorderColor
        {
            get
            {
                return borderColor;
            }
            set
            {
                borderColor = value;
                if (this._attachedControl.Parent != null)
                {                   
                   Rectangle rect = new Rectangle(_attachedControl.Left - offset, _attachedControl.Top - offset, _attachedControl.Width + offset + 1, _attachedControl.Height + offset + 1);
                   _attachedControl.Parent.Invalidate(rect);                   
                }
            }
        }

        #endregion

        #region 自定义方法

        private void SetStyle(Control control)
        {
            if (control is TextBox)
            {
                ((TextBox)control).BorderStyle = BorderStyle.None;
                offset = 3;
            }
            else
            {
                control.ShowBorder(false);
            }
           
        }      

        private void HookParentPaintEvent(Control control)
        {
            if (control.Parent != null)
            {
                control.Parent.Paint += new PaintEventHandler(Parent_Paint);
            }
            else
            {
                control.ParentChanged += delegate(object sender, EventArgs e)
                {
                    if (control.Parent != null)
                    {
                        control.Parent.Paint  += new PaintEventHandler(Parent_Paint);
                    }

                };
            }
        }

        #endregion

        #region 事件处理


        void Parent_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(_attachedControl.Left - offset, _attachedControl.Top - offset, _attachedControl.Width + offset, _attachedControl.Height + offset);
            Rectangle clipRect = new Rectangle(e.ClipRectangle.Left, e.ClipRectangle.Top - 10, e.ClipRectangle.Width, e.ClipRectangle.Height);
            // 检查是否需要画边框
            if (bounds.IntersectsWith(clipRect))
            {
                if (Diameter <= 0)
                {
                    Rectangle rect = new Rectangle(_attachedControl.Left - offset, _attachedControl.Top - offset, _attachedControl.Width + offset, _attachedControl.Height + offset);
                    using (Pen pen = new Pen(this.borderColor))
                    {
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                }
                else
                {
                    int rad = 3;
                    Rectangle rect = new Rectangle(_attachedControl.Left - offset - rad, _attachedControl.Top - offset, _attachedControl.Width + offset + 4, _attachedControl.Height + offset);
                    using (Pen pen = new Pen(this.borderColor))
                    {
                        g.DrawRoundedRectangle(pen, rect, Diameter);
                    }

                }
            }

        }

        #endregion

        #region 绘制圆角 (非路径方式画圆, 不够平滑，暂时没用)

        /// <summary>
        ///  绘制圆角矩形 (虚假圆角)
        /// </summary>
        /// <param name="gx"></param>
        /// <param name="borderColor"></param>
        /// <param name="backColor"></param>
        /// <param name="rc"></param>
        /// <param name="size"></param>
        public void DrawRoundedRectangle(Graphics gx, Color borderColor, Color backColor, Rectangle rc, Size size)
        {
            Point[] points = new Point[8];

            // 设置多边形的点
            points[0].X = rc.Left + size.Width / 2;
            points[0].Y = rc.Top + 1;
            points[1].X = rc.Right - size.Width / 2;
            points[1].Y = rc.Top + 1;

            points[2].X = rc.Right;
            points[2].Y = rc.Top + size.Height / 2;
            points[3].X = rc.Right;
            points[3].Y = rc.Bottom - size.Height / 2;

            points[4].X = rc.Right - size.Width / 2;
            points[4].Y = rc.Bottom;
            points[5].X = rc.Left + size.Width / 2;
            points[5].Y = rc.Bottom;

            points[6].X = rc.Left + 1;
            points[6].Y = rc.Bottom - size.Height / 2;
            points[7].X = rc.Left + 1;
            points[7].Y = rc.Top + size.Height / 2;

            // 设置笔刷
            Brush fillBrush = new SolidBrush(backColor);
            Pen borderPen = new Pen(borderColor);

            // 画四个角
            gx.DrawLine(borderPen, rc.Left + size.Width / 2, rc.Top, rc.Right - size.Width / 2, rc.Top);
            gx.FillEllipse(fillBrush, rc.Right - size.Width, rc.Top, size.Width, size.Height);
            gx.DrawEllipse(borderPen, rc.Right - size.Width, rc.Top, size.Width, size.Height);

            gx.DrawLine(borderPen, rc.Right, rc.Top + size.Height / 2, rc.Right, rc.Bottom - size.Height / 2);
            gx.FillEllipse(fillBrush, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height);
            gx.DrawEllipse(borderPen, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height);

            gx.DrawLine(borderPen, rc.Right - size.Width / 2, rc.Bottom, rc.Left + size.Width / 2, rc.Bottom);
            gx.FillEllipse(fillBrush, rc.Left, rc.Bottom - size.Height, size.Width, size.Height);
            gx.DrawEllipse(borderPen, rc.Left, rc.Bottom - size.Height, size.Width, size.Height);

            gx.DrawLine(borderPen, rc.Left, rc.Bottom - size.Height / 2, rc.Left, rc.Top + size.Height / 2);
            gx.FillEllipse(fillBrush, rc.Left, rc.Top, size.Width, size.Height);
            gx.DrawEllipse(borderPen, rc.Left, rc.Top, size.Width, size.Height);

            // 填充背景以移除内部弧线
            gx.FillPolygon(fillBrush, points);

            fillBrush.Dispose();
            borderPen.Dispose();
        }

        /// <summary>
        /// 绘制渐变填充的半透明圆角矩形
        /// </summary>
        /// <param name="gx">Destination graphics</param>
        /// <param name="rc">Destination rectangle</param>
        /// <param name="startColorValue">Starting color for gradient</param>
        /// <param name="endColorValue">End color for gradient</param>
        /// <param name="borderColor">The color of the border</param>
        /// <param name="size">The size of the rounded corners</param>
        /// <param name="transparency">Transparency constant</param>
        public void DrawGradientRoundedRectangleAlpha(Graphics gx, Rectangle rc, Color startColorValue, Color endColorValue, Color borderColor, Size size, byte transparency, FillDirection direction)
        {
            // Prepare image for gradient
            Bitmap gradientImage = new Bitmap(rc.Width, rc.Height);
            // Create temporary graphics
            Graphics gxGradient = Graphics.FromImage(gradientImage);
            // This is our rectangle
            Rectangle roundedRect = new Rectangle(0, 0, rc.Width, rc.Height);
            // Fill in gradient
            GradientFill.Fill(
                gxGradient,
                roundedRect,
                startColorValue,
                endColorValue,
                direction);

            // Prepare the copy of the screen graphics
            Bitmap tempBitmap = new Bitmap(rc.Width, rc.Height);
            Graphics tempGx = Graphics.FromImage(tempBitmap);
            // Copy from the screen's graphics to the temp graphics
            CopyGraphics(gx, tempGx, rc.Width, rc.Height, rc.X, rc.Y);
            // Draw the gradient image with transparency on the temp graphics
            tempGx.DrawAlpha(gradientImage, transparency, rc.X, rc.Y);
            // Cut out the transparent image 
            gxGradient.DrawImage(tempBitmap, new Rectangle(0, 0, rc.Width, rc.Height), rc, GraphicsUnit.Pixel);
            // Prepare for imposing
            roundedRect.Width--;
            roundedRect.Height--;
            // Impose the rounded rectangle with transparent color
            Bitmap borderImage = ImposeRoundedRectangle(roundedRect, borderColor, size);
            // Draw the transparent rounded rectangle
            ImageAttributes attrib = new ImageAttributes();
            attrib.SetColorKey(Color.Green, Color.Green);
            gxGradient.DrawImage(borderImage, new Rectangle(0, 0, rc.Width, rc.Height), 0, 0, borderImage.Width, borderImage.Height, GraphicsUnit.Pixel, attrib);
            // OK... now are ready to draw the final image on the original graphics
            gx.DrawImageTransparent(gradientImage, rc);

            // Clean up
            attrib.Dispose();
            tempGx.Dispose();
            tempBitmap.Dispose();
            gradientImage.Dispose();
            gxGradient.Dispose();
        }

        /// <summary>
        /// Draws gradient filled rounded rectangle
        /// </summary>
        /// <param name="gx">目标 graphics</param>
        /// <param name="rc">目标 rectangle</param>
        /// <param name="startColorValue">Starting color for gradient</param>
        /// <param name="endColorValue">End color for gradient</param>
        /// <param name="borderColor">The color of the border</param>
        /// <param name="size">The size of the rounded corners</param>
        public void DrawGradientRoundedRectangle(Graphics gx, Rectangle rc, Color startColorValue, Color endColorValue, Color borderColor, Size size, FillDirection direction)
        {
            Bitmap bitmap = GetGradiendRoundedRectangle(new Rectangle(0, 0, rc.Width, rc.Height), startColorValue, endColorValue, borderColor, size, direction);
            gx.DrawImageTransparent(bitmap, rc);
        }

        /// <summary>
        /// 绘制指定透明度的矩形
        /// </summary>
        /// <param name="gx"></param>
        /// <param name="borderColor"></param>
        /// <param name="backColor"></param>
        /// <param name="rc"></param>
        /// <param name="transparency"></param>
        public void DrawRectangleAlpha(Graphics gx, Color borderColor, Color backColor, Rectangle rc, byte transparency)
        {
            Bitmap tempBitmap = new Bitmap(rc.Width, rc.Height);
            Graphics tempGraphics = Graphics.FromImage(tempBitmap);
            using (Brush backColorBrush = new SolidBrush(backColor))
            {
                tempGraphics.FillRectangle(backColorBrush, new Rectangle(0, 0, rc.Width, rc.Height));
            }

            using (Pen borderPen = new Pen(borderColor))
            {
                tempGraphics.DrawRectangle(borderPen, new Rectangle(0, 0, rc.Width, rc.Height));
            }

            gx.DrawAlpha(tempBitmap, transparency, rc.X, rc.Y);
            tempBitmap.Dispose();
            tempGraphics.Dispose();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gx"></param>
        /// <param name="borderColor"></param>
        /// <param name="backColor"></param>
        /// <param name="rc"></param>
        /// <param name="size"></param>
        /// <param name="transparency"></param>
        public void DrawRoundedRectangleAlpha(Graphics gx, Color borderColor, Color backColor, Rectangle rc, Size size, byte transparency)
        {
            // Prepare image for gradient
            Bitmap roundedImage = new Bitmap(rc.Width, rc.Height);
            // Create temporary graphics
            Graphics gxRounded = Graphics.FromImage(roundedImage);
            // This is our rectangle
            Rectangle roundedRect = new Rectangle(0, 0, rc.Width, rc.Height);
            // Draw rounded rect
            DrawRoundedRectangle(gxRounded, borderColor, backColor, roundedRect, size);
            //DrawRoundedRect(gxRounded, borderPen, backColor, new Rectangle(0, 0, rc.Width, rc.Width), size);

            // Prepare the copy of the screen graphics
            using (Bitmap tempBitmap = new Bitmap(rc.Width, rc.Height))
            {
                using (Graphics tempGx = Graphics.FromImage(tempBitmap))
                {
                    // Copy from the screen's graphics to the temp graphics
                    CopyGraphics(gx, tempGx, rc.Width, rc.Height, rc.X, rc.Y);
                    // Draw the gradient image with transparency on the temp graphics
                    tempGx.DrawAlpha(roundedImage, transparency, rc.X, rc.Y);
                }
                // Cut out the transparent image 
                gxRounded.DrawImage(tempBitmap, new Rectangle(0, 0, rc.Width, rc.Height), rc, GraphicsUnit.Pixel);
            }

            // Prepare for imposing
            roundedRect.Width--;
            roundedRect.Height--;
            // Impose the rounded rectangle with transparent color
            Bitmap borderImage = ImposeRoundedRectangle(roundedRect, borderColor, size);
            // Draw the transparent rounded rectangle
            using (ImageAttributes attrib = new ImageAttributes())
            {
                attrib.SetColorKey(Color.Green, Color.Green);
                gxRounded.DrawImage(borderImage, new Rectangle(0, 0, rc.Width, rc.Height), 0, 0, borderImage.Width, borderImage.Height, GraphicsUnit.Pixel, attrib);
                // OK... now are ready to draw the final image on the original graphics
                gx.DrawImageTransparent(roundedImage, rc);
            }

            // Clean up

            roundedImage.Dispose();
            gxRounded.Dispose();
        }

        
        /// <summary>
        /// 创建渐变填充的圆角矩形位图
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="startColorValue"></param>
        /// <param name="endColorValue"></param>
        /// <param name="borderColor"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Bitmap GetGradiendRoundedRectangle(Rectangle rc, Color startColorValue, Color endColorValue, Color borderColor, Size size, FillDirection direction)
        {
            Bitmap outputImage = new Bitmap(rc.Width, rc.Height);
            // Create temporary graphics
            Graphics gx = Graphics.FromImage(outputImage);

            GradientFill.Fill(
                gx,
                rc,
                startColorValue,
                endColorValue,
                direction);

            Rectangle roundedRect = rc;
            roundedRect.Width--;
            roundedRect.Height--;

            Bitmap borderImage = ImposeRoundedRectangle(roundedRect, borderColor, size);

            ImageAttributes attrib = new ImageAttributes();
            attrib.SetColorKey(Color.Green, Color.Green);

            gx.DrawImage(borderImage, rc, 0, 0, borderImage.Width, borderImage.Height, GraphicsUnit.Pixel, attrib);

            // Clean up
            attrib.Dispose();
            gx.Dispose();

            return outputImage;
        }

        /// <summary>
        /// 返回一个以白色为背景画布的圆角矩形位图(设置绿色为透明色)
        ///  注: 这里有个问题, 当以白色作为渐变的开始颜色和终止颜色时, 会造成无法显示白色. 
        ///        因为白色被作为(假透明)背景擦除了
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="borderColor"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        internal Bitmap ImposeRoundedRectangle(Rectangle rc, Color borderColor, Size size)
        {
            Bitmap bitmap = new Bitmap(rc.Width + 1, rc.Height + 1);
            using (Graphics gxTemp = Graphics.FromImage(bitmap))
            {
                gxTemp.Clear(Color.White);
                DrawRoundedRectangle(gxTemp, borderColor, Color.Green, rc, size);
            }
            return bitmap;
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
        internal void CopyGraphics(Graphics gxSrc, Graphics gxDest, int width, int height, int x, int y)
        {
            IntPtr srcDc = gxSrc.GetHdc();
            IntPtr destDc = gxDest.GetHdc();
            Win32.BitBlt(destDc, 0, 0, width, height, srcDc, x, y, TernaryRasterOperations.SRCCOPY);
            gxSrc.ReleaseHdc(srcDc);
            gxDest.ReleaseHdc(destDc);
        }
        #endregion


    }
}
