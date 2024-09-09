using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class CustomLabel : Label
    {
        #region 字段

        private bool badgeAlike;
        private int radius;
        private int borderWidth;
        private Color borderColor;
        private Color badgeColor;

        #endregion

        #region 属性


        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "Color.Green")]
        public Color BorderColor 
        { 
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(1)]
        public int BorderWidth
        {
            get => borderWidth; 
            set
            {
                borderWidth = value;
                Invalidate();
            }
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(50)]
        public int Radius {
            
            get => radius;
            set
            {
                radius = value;
                Invalidate();
            }
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(false)]
        public bool BadgeAlike
        {
            get
            {
                return badgeAlike;
            }
            set
            {
                badgeAlike = value;
                if (value)
                {
                    this.AutoSize = false;
                    this.TextAlign = ContentAlignment.MiddleCenter;
                    UpdateMinimalSize();
                    radius = 50;
                    if (BadgeColor == Color.Empty)
                    {
                        BadgeColor = Color.SlateBlue;
                    }
                    
                }

                AdaptColor();
                Invalidate();
            }
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color BadgeColor
        {
            get
            {
                return badgeColor;
            }
            set
            {
                badgeColor = value;
                AdaptColor();
                Invalidate();
            } 
        }

        public Rectangle BadgeArea
        {
            get
            {
                return new Rectangle(
                             Margin.Left + BorderWidth,
                             Margin.Top + BorderWidth,
                             this.Width - Margin.Left - Margin.Right - this.borderWidth * 2,
                             this.Height - Margin.Top - Margin.Bottom - this.borderWidth * 2);
            }
        }

        #endregion

        #region 构造

        public CustomLabel()
        {
            badgeColor = Color.SlateBlue;
            borderColor = Color.DarkSlateBlue;
            radius = 50;
            borderWidth = 1;
        }

        #endregion

        #region 重写的方法

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (BadgeAlike)
            {
                // 防止标签变得过小
                UpdateMinimalSize();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);


            if (BadgeAlike)
            {
                // 创建一个 Graphics 对象。
                Graphics g = e.Graphics;

                // 设置画笔和画刷的颜色。
                Pen pen = new Pen(Color.Black, 2);
                SolidBrush brush = new SolidBrush(Color.Red);

                var radius = this.Radius;
                // 限制 _circleRadius 值， 防止AddArc变形
                radius = (int)Math.Min(radius, Math.Min(BadgeArea.Width, BadgeArea.Height) / 2.0F);

                if (BadgeColor != Color.Empty)
                {
                    using (Brush badgeBrush = new SolidBrush(BadgeColor))
                    {
                        var badgeArea = new RectangleF(
                            Margin.Left + BorderWidth,
                            Margin.Top + BorderWidth,
                            this.Width - Margin.Left - Margin.Right - this.borderWidth * 2,
                            this.Height - Margin.Top - Margin.Bottom - this.borderWidth * 2);

                        g.FillRoundedRectangle(badgeBrush, badgeArea, radius);
                    }
                }


                if (this.BorderWidth > 0)
                {
                    using (Pen borderPen = new Pen(this.BorderColor, this.BorderWidth))
                    {
                        var borderArea = new RectangleF(
                            Margin.Left + BorderWidth, 
                            Margin.Top + BorderWidth, 
                            this.Width - Margin.Left - Margin.Right - this.borderWidth * 2, 
                            this.Height - Margin.Top - Margin.Bottom - this.borderWidth * 2);

                        g.DrawRoundedRectangle(borderPen, borderArea, radius);
                    }
                }

                pen.Dispose();
                brush.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }



        #endregion

        #region 私有方法

        private void UpdateMinimalSize()
        {
            Size textSize = TextRenderer.MeasureText(this.Text, this.Font);
            var minWidth = textSize.Width + 10;
            var minHeight = textSize.Height + 10;
            if (this.Width < minWidth)
            {
                this.Width = minWidth;
            }
            if (this.Height < minHeight)
            {
                this.Height = minHeight;
            }
        }

        protected virtual void AdaptColor()
        {
            // 获取 Label 控件的背景颜色
            Color backgroundColor = this.BackColor;
            if (BadgeAlike)
            { 
                backgroundColor = BadgeColor;
            }

            if (backgroundColor == Color.Empty || backgroundColor == Color.Transparent)
            {
                this.ForeColor = Color.Black;
                return;
            }


            // 计算背景颜色的亮度
            double brightness = (backgroundColor.R * 0.299 + backgroundColor.G * 0.587 + backgroundColor.B * 0.114) / 255.0;

            // 根据背景颜色的亮度设置字体颜色
            if (brightness > 0.5)
            {
                this.ForeColor = Color.Black;
            }
            else
            {
                this.ForeColor = Color.White;
            }
        
        }

        #endregion
    }
}
