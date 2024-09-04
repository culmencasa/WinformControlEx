using System.Drawing;
using System.IO;

namespace System.Windows.Forms.Canvas
{

    public class PictureObject : CanvasObject
    {
        #region 字段


        private Color _borderColor;


        #endregion

        #region 属性

        public Image Image { get; set; }

        /// <summary>
        /// 边框色
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                Render();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color FocusColor
        {
            get;
            set;
        }


        /// <summary>
        /// 正常色(背景)
        /// </summary>
        public Color NormalColor
        {
            get;
            set;
        }

        /// <summary>
        /// 强调色(背景)
        /// </summary>
        public Color HighLightColor
        {
            get;
            set;
        }

        #endregion

        #region 构造

        public PictureObject()
        {
            // default colors
            _borderColor = Color.Black;
            NormalColor = Color.LightGray;
            HighLightColor = Color.Orange; 
            FocusColor = Color.Blue;


            BackColor = NormalColor;
        }

        #endregion

        #region CanvasElement抽象方法实现

        internal override void DrawContent(Graphics g)
        {
            if (HighlightState)
            {
                var borderPen = Canvas.GetCachedPen(BorderColor, 1, Drawing.Drawing2D.DashStyle.Dot);
                g.DrawRectangle(borderPen, Left, Top, Width, Height);
            }

            // 以Zoom方式缩放
            if (Image != null && Image.Width > 0 && Image.Height > 0)
            {
                RectangleF DisplayingRectangle = RectangleF.Empty;
                RectangleF ImageRectangle = new RectangleF(0, 0, Image.Width, Image.Height);

                // 宽高比
                float imageAspectRatio = (float)Image.Width / Image.Height;
                float displayAspectRatio = (float)this.Width / this.Height;

                if (displayAspectRatio > imageAspectRatio)
                {
                    // 高度固定，宽度缩放
                    DisplayingRectangle.Height = this.Height;
                    DisplayingRectangle.Width = this.Height * imageAspectRatio;
                }
                else
                {
                    // 宽度固定，高度缩放
                    DisplayingRectangle.Width = this.Width;
                    DisplayingRectangle.Height = this.Width / imageAspectRatio;
                }

                DisplayingRectangle.X = Left + (this.Width - DisplayingRectangle.Width) / 2;
                DisplayingRectangle.Y = Top + (this.Height - DisplayingRectangle.Height) / 2;

                g.DrawImage(Image, DisplayingRectangle, ImageRectangle, GraphicsUnit.Pixel);
            }
        }


        protected override void OnHighlighStateChanged()
        {
            base.OnHighlighStateChanged();
            if (HighlightState)
            {
                BackColor = HighLightColor;
            }
            else
            {
                BackColor = NormalColor;
            }

            Render();
        }

        #endregion

        #region 公开方法

        public Image DefaultImage()
        {
            var bitmap = new Bitmap(64, 64);

            // 获取Graphics对象
            Graphics g = Graphics.FromImage(bitmap);
            g.SetSlowRendering();
            g.DrawRectangle(Pens.LightGray, 0, 0, 64, 64);

            // 定义山的形状，调整坐标使其适应64x64的区域并居中
            Point[] mountainPoints = {
                new Point(0, 40),  // 左下角
                new Point(32, 20),  // 山峰
                new Point(44, 35),  // 中间山峰
                new Point(56, 25),  // 右边的高峰
                new Point(64, 50),  // 右下角
                new Point(0, 50)    // 左下角
            };

            // 创建用于填充山形的刷子
            using (Brush mountainBrush = new SolidBrush(Color.FromArgb(244,244,244)))
            {
                g.FillPolygon(mountainBrush, mountainPoints);  
            } 

            return bitmap;
        }

        public string ToBase64String()
        {
            if (Image != null)
            {
                return ConvertImageToBase64(Image);
            }

            return string.Empty;
        }

        #endregion


        string ConvertImageToBase64(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            { 
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png); 
                byte[] imageBytes = memoryStream.ToArray(); 
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
