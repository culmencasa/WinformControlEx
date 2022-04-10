using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class CustomPanel : Panel
    {
        #region 属性
        
        [Category("Custom")]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color FirstColor { get; set; }

        [Category("Custom")]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color SecondColor { get; set; }

        [Category("Custom")]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color BorderColor { get; set; }


        [Category("Custom")]
        [DefaultValue(typeof(FillDirection), "TopToBottom")]
        public FillDirection GradientDirection { get; set; }

        [Category("Custom")]
        public int BorderWidth { get; set; }

        [Category("Custom")]
        public int RoundBorderRadius { get; set; }

        [Category("Custom")]
        public Color InnerBackColor { get; set; }

        #endregion

        #region 构造

        public CustomPanel()
        {
            this.GradientDirection = FillDirection.TopToBottom;

			SetStyle(
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.ResizeRedraw |
				ControlStyles.SupportsTransparentBackColor, true);
			this.DoubleBuffered = true;
			UpdateStyles();

			this.BackColor = Color.Transparent;
            this.InnerBackColor = Color.Transparent;
            BorderWidth = 1;
        }

        #endregion

         

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Graphics lazyG = e.Graphics;

            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bitmap);

            #region 如果设置了渐变色

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
                        // 不减1会出现1像素的空白, 原因不明
                        g.FillRoundedRectangle(brush, BorderWidth - 1, BorderWidth - 1, this.Width - BorderWidth, this.Height - BorderWidth, RoundBorderRadius);
                    }
                }
                else
                {
                    // 填充直角矩形
                    GradientFill.Fill(g, this.ClientRectangle, this.FirstColor, this.SecondColor, this.GradientDirection);
                }
            }

			#endregion

			#region 没有渐变色

			else
			{
				using (SolidBrush borderBrush = new SolidBrush(BackColor))
				{
					if (RoundBorderRadius > 0)
					{
						g.FillRoundedRectangle(borderBrush, BorderWidth, BorderWidth, this.Width - BorderWidth * 2, this.Height - BorderWidth * 2, RoundBorderRadius);
					}
					else
					{
						g.FillRectangle(borderBrush, new Rectangle(BorderWidth, BorderWidth, this.Width - BorderWidth * 2, this.Height - BorderWidth * 2));
					}
				}

			}

			#endregion

			if (this.BorderColor != Color.Empty && BorderWidth > 0)
            {
                if (RoundBorderRadius > 0)
                {
                    // 画圆角边框   
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        g.DrawRoundedRectangle(borderPen, 0, 0, this.Width - BorderWidth, this.Height - BorderWidth, this.RoundBorderRadius);
                    }
                }
                else
                {
                    // 画直角边框
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        g.DrawRectangle(borderPen, 0, 0, this.Width - BorderWidth, this.Height - BorderWidth);
                    }
                }
            }

            lazyG.DrawImage(bitmap, 0, 0);
            g.Dispose();
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

		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);

            if (this.RoundBorderRadius > 0)
            {
                // 圆角背景效果不好
                //IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, Width, Height, RoundBorderRadius + 5 , RoundBorderRadius + 5);
                //Region = System.Drawing.Region.FromHrgn(hrgn);

            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            { 
                return new Rectangle(BorderWidth, BorderWidth, Width - BorderWidth * 2, Height - BorderWidth * 2);
            }
        }
    }
}
