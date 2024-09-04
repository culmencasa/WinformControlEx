using System.Drawing;

namespace System.Windows.Forms.Canvas
{

    public class QRcodeObject : CanvasObject
    {
        #region 字段



        #endregion

        #region 属性

        private Image Image { get; set; }


        public string QRContent { get; set; }


        /// <summary>
        /// 边框色
        /// </summary>
        public Color BorderColor
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

        public QRcodeObject()
        {
            // default colors
            BorderColor = Color.Blue;
            NormalColor = Color.Black;
            HighLightColor = Color.Orange;  


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
                ForeColor = HighLightColor; 
            }
            else
            {
                ForeColor = NormalColor;
            }

            Render();
        }

        #endregion

        #region 公开方法

        #endregion
    }
}
