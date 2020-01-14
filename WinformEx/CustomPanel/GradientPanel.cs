using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class GradientPanel : Panel
    {
        #region 属性
        
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color FirstColor { get; set; }

        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color SecondColor { get; set; }

        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color BorderColor { get; set; }

        public Color BottomBorderColor { get; set; }

        [DefaultValue(typeof(FillDirection), "TopToBottom")]
        public FillDirection GradientDirection { get; set; }

        public Int32 BorderWidth { get; set; }
        public Int32 RoundBorderRadius { get; set; }
        #endregion

        #region 构造

        public GradientPanel()
        {
            this.GradientDirection = FillDirection.TopToBottom;

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);

            //this.DoubleBuffered = true;
            UpdateStyles();

            BorderWidth = 1;
        }

        #endregion

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;


            // 如果设置了渐变色
            if (this.FirstColor != Color.Empty && this.SecondColor != Color.Empty)
            {
                if (RoundBorderRadius > 0)
                {
                    // 填充圆角矩形
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        new Point(this.Width / 2, 0),
                        new Point(this.Width / 2, this.Height),
                        this.FirstColor,
                        this.SecondColor
                        ))
                    {
                        g.FillRoundedRectangle(brush, 0, 0, this.Width, this.Height, RoundBorderRadius);
                    }
                }
                else
                {
                    // 填充直角矩形
                    GradientFill.Fill(g, this.ClientRectangle, this.FirstColor, this.SecondColor, this.GradientDirection);
                }
            }

            if (this.BorderColor != Color.Empty)
            {
                if (RoundBorderRadius > 0)
                {
                    // 画圆角边框   
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        g.DrawRoundedRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1, this.RoundBorderRadius);
                    }
                }
                else
                {
                    // 画直角边框
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        g.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
            else
            {
                if (BottomBorderColor != Color.Empty)
                {
                    using (Pen borderPen = new Pen(this.BottomBorderColor, BorderWidth))
                    {
                        g.DrawLine(borderPen, 0, this.Height - 1, this.Width, this.Height - 1);
                    }
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.MouseLeave += DidMouseReallyLeave;
      
            base.OnControlAdded(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            // 鼠标在Panel控件外
            //Rectangle screenBounds = new Rectangle(this.PointToScreen(this.Location), this.Size);
            //if (!screenBounds.Contains(MousePosition))
            //{
            //    //panel1.BackColor = Color.Gray;
            //}
            //base.OnMouseLeave(e);

            DidMouseReallyLeave(this, e);
        }
        private void DidMouseReallyLeave(object sender, EventArgs e)
        {
            // 鼠标在子控件内
            if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
            {
                return;
            }

            base.OnMouseLeave(e);
        }
    }
}
