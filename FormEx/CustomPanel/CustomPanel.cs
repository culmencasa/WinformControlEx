using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    /// <summary>
    /// 渐变, 圆角, 边框面板
    /// </summary>
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
        [DefaultValue(typeof(FillDirection), "TopToBottom")]
        public FillDirection GradientDirection { get; set; }

        [Category("Custom")]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color BorderColor { get; set; }


        [Category("Custom")]
        public int BorderWidth { get; set; }

        [Category("Custom")]
        public int RoundBorderRadius { get; set; }

        /// <summary>
        /// 没有用
        /// </summary>
        [Category("Custom")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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


        #region 重写的成员

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics lazyG = e.Graphics;

            int w = this.Width;
            int h = this.Height;
            Bitmap bitmap;
            Graphics cacheG;
            if (w > 0 && h > 0)
            {
                bitmap = new Bitmap(w, h);
                cacheG = Graphics.FromImage(bitmap);
            }
            else
            {
                // 有时候, 例如最小化, Height会变成0
                base.OnPaint(e);
                return;
            } 

            #region 背景（如果设置了渐变色）

            if (this.FirstColor != Color.Empty && this.SecondColor != Color.Empty
                 && FirstColor != SecondColor)
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
                        cacheG.FillRoundedRectangle(brush,
                            BorderWidth / 2,
                            BorderWidth / 2, 
                            (this.Width - BorderWidth), 
                            (this.Height - BorderWidth), RoundBorderRadius);
                    }
                }
                else
                {
                    // 填充直角矩形
                    GradientFill.Fill(cacheG, 
                        ClientRectangle,
                        this.FirstColor, this.SecondColor, this.GradientDirection);
                }
            }

			#endregion

			#region 背景（没有渐变色）

			else
			{
                var backColor = BackColor;
                if (FirstColor != Color.Empty)
                {
                    backColor = FirstColor;
                }
                if (SecondColor != Color.Empty)
                {
                    backColor = SecondColor;
                }
				using (SolidBrush backgroundBrush = new SolidBrush(backColor))
				{
                    if (RoundBorderRadius > 0)
                    {
                        cacheG.FillRoundedRectangle(backgroundBrush, BorderWidth / 2, BorderWidth / 2, this.Width - BorderWidth, this.Height - BorderWidth, RoundBorderRadius);
                    }
                    else
                    {
                        cacheG.FillRectangle(backgroundBrush, new Rectangle(BorderWidth / 2, BorderWidth / 2, this.Width - BorderWidth, this.Height - BorderWidth));
                    }
                }

            }

            #endregion

            #region 边框

            if (this.BorderColor != Color.Empty && BorderWidth > 0)
            {
                if (RoundBorderRadius > 0)
                {
                    // 画圆角边框   
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        cacheG.DrawRoundedRectangle(borderPen, 
                            BorderWidth / 2, BorderWidth / 2, (float)this.Width - BorderWidth, (float)this.Height - BorderWidth, 
                            this.RoundBorderRadius);
                    }
                }
                else
                {
                    // 画直角边框
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        cacheG.DrawRectangle(borderPen, 
                            BorderWidth / 2, 
                            BorderWidth / 2, 
                            (float)this.Width - BorderWidth, (float)this.Height - BorderWidth);
                    }
                }
            }

            #endregion


            lazyG.DrawImage(bitmap, 0, 0);
            cacheG.Dispose();

            base.OnPaint(e);
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
                int x = BorderWidth + Padding.Left + RoundBorderRadius;
                int y = BorderWidth + Padding.Top + RoundBorderRadius;
                int width = Width - BorderWidth * 2 - RoundBorderRadius * 2  - Padding.Left - Padding.Right;
                int height = Height - BorderWidth * 2 - RoundBorderRadius  * 2- Padding.Left - Padding.Right;
                     
                return new Rectangle(x, y, width, height);
            }
        }


        #endregion
    }
}
