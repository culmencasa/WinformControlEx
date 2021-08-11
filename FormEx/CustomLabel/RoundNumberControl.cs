using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class RoundNumberControl : NonFlickerUserControl
    {
        private int _value;
        private bool _isActived;

        public RoundNumberControl()
        {
            this.Size = new Size(20, 20);
            this.Value = 1;
            this.Font = new Font("Arial", 7, FontStyle.Regular, GraphicsUnit.Point);
            this.ForeColor = Color.FromArgb(166,185,200);
            this.ActiveForeColor = Color.FromArgb(13,122,179);
            this.CoverColor = Color.FromArgb(17, 89, 129);
            this.ActiveCoverColor = Color.White;
        }

        #region 属性

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.Invalidate();
            }
        }

        public Color ActiveForeColor
        {
            get;
            set;
        }

        public Color ActiveCoverColor
        {
            get;
            set;
        }

        public Color CoverColor
        {
            get;
            set;
        }

        public bool IsActived
        {
            get
            {
                return _isActived;
            }
            set
            {
                _isActived = value;
                this.Invalidate();
            }
        }
            
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawNumber(g);

            base.OnPaint(e);
        }


        protected void DrawNumber(Graphics g)
        {
            Bitmap tempBmp = new Bitmap(this.Width, this.Height);
            Graphics tempGraphics = Graphics.FromImage(tempBmp);
            tempGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            Font tempFont = new Font(this.Font.Name, this.Font.SizeInPoints, FontStyle.Bold, GraphicsUnit.Point);
            Color coverColor  = this.CoverColor;
            Color foreColor = this.ForeColor;
            if (IsActived)
            {
                coverColor = this.ActiveCoverColor;
                foreColor = this.ActiveForeColor;
            }            
            SolidBrush txtBrush = new SolidBrush(foreColor);
            SolidBrush coverBrush = new SolidBrush(coverColor);


            string numberValue = this.Value.ToString();
            SizeF stringSize = TextRenderer.MeasureText(numberValue, tempFont);
            float x = (this.Width - stringSize.Width) / 2 + 1;
            float y = (this.Height - stringSize.Height) / 2;
            float width = stringSize.Width;
            float height = stringSize.Height;


            // 画序号边框
            float circlePadding = 2;
            float circleWidth = this.Width - circlePadding; 
            float circleHeight = circleWidth; // 正圆的宽高一样
            float circleX = (this.Width - circleWidth) / 2;
            float circleY = (this.Height - circleHeight - 2) / 2; // 减2点的偏移
            tempGraphics.FillEllipse(coverBrush, circleX, circleY, circleWidth, circleHeight);

            // 画序号
            //tempGraphics.DrawString(numberValue, tempFont, txtShadowBrush,
            //    new RectangleF(x + 1, y + 1, width, height));
            //tempGraphics.DrawString(numberValue, tempFont, txtShadowBrush,
            //    new RectangleF(x - 1, y - 1, width, height));
            tempGraphics.DrawString(numberValue, tempFont, txtBrush,
                new RectangleF(x, y, width, height));


            // 将缓存画到原图上
            g.DrawImageOpacity(tempBmp, 1f, new Point(0, 0));

            tempFont.Dispose();
            tempGraphics.Dispose();
            tempBmp.Dispose();

        }

    }
}
