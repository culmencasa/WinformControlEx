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
    [DefaultEvent("Click")]
    public class CustomPanel : Panel
    {
        private FillDirection _gradientDirection;
        private bool _isMouseHovering;
        private bool _isSelected;
        public delegate void SelectStatusChangedHandler(CustomPanel sender, bool isActived);


        #region 事件

        [Category(Consts.DefaultCategory)]
        public event SelectStatusChangedHandler SelectStatusChanged;

        #endregion

        #region 属性


        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color FirstColor { get; set; }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color SecondColor { get; set; }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(FillDirection), "TopToBottom")]
        public FillDirection GradientDirection { 
            get => _gradientDirection;
            set { _gradientDirection = value; Invalidate(); }
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color BorderColor { get; set; }


        [Category(Consts.DefaultCategory)]
        public int BorderWidth { get; set; }

        [Category(Consts.DefaultCategory)]
        public int RoundBorderRadius { get; set; }

        /// <summary>
        /// 没有用
        /// </summary>
        [Category(Consts.DefaultCategory)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color InnerBackColor { get; set; }

         
        protected bool IsMouseHovering
        {
            get
            {
                return _isMouseHovering;
            }
            set
            {
                _isMouseHovering = value;

                Invalidate();
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                bool hasChanged = _isSelected != value;
                _isSelected = value;
                Invalidate();
                if (hasChanged)
                {
                    SelectStatusChanged?.Invoke(this, IsSelected);
                }
            }
        }

        [Category(Consts.DefaultCategory)]
        public Color MouseHoverBgColor { get; set; }

        [Category(Consts.DefaultCategory)]
        public Color MouseHoverBorderColor { get; set; }

        [Category(Consts.DefaultCategory)]
        public bool MouseHoverShowFocus { get; set; }

        [Category(Consts.DefaultCategory)]
        public bool MouseClickSwitchSelectStatus { get; set; }

        [Category(Consts.DefaultCategory)]
        public Color SelectedBorderColor { get; set; }



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

            SelectedBorderColor = Color.FromArgb(153, 209, 255);
            MouseHoverBorderColor = Color.FromArgb(153, 209, 255);
            MouseHoverBgColor = Color.FromArgb(204, 232, 255);
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

            if (IsMouseHovering || IsSelected)
            {
                using (SolidBrush backgroundBrush = new SolidBrush(MouseHoverBgColor))
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
            else
            {

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
            }


            #region 边框

            if (IsSelected)
            {
                DrawBorder(cacheG, SelectedBorderColor, 2);
            }
            else if (IsMouseHovering)
            {
                DrawBorder(cacheG, MouseHoverBorderColor, BorderWidth); 
            } 
            else if (this.BorderColor != Color.Empty && BorderWidth > 0)
            {
                DrawBorder(cacheG, BorderColor, BorderWidth);
            }

            #endregion


            lazyG.DrawImage(bitmap, 0, 0);
            cacheG.Dispose();

            base.OnPaint(e);
        }


        private void DrawBorder(Graphics g, Color borderColor, int borderWidth)
        {
            if (RoundBorderRadius > 0)
            {
                // 画圆角边框   
                using (Pen borderPen = new Pen(borderColor, borderWidth))
                {
                    //cacheG.DrawRoundedRectangle(borderPen,
                    //    1, 1, (float)this.Width - BorderWidth * 2, (float)this.Height - BorderWidth * 2, 
                    //    this.RoundBorderRadius);


                    Rectangle innerRectangle = new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 2 * borderWidth,
                        ClientRectangle.Height - 2 * borderWidth);


                    using (GraphicsPath innerPath = CreateRoundedRectangle(innerRectangle, RoundBorderRadius))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        g.DrawPath(borderPen, innerPath);
                    }
                }
            }
            else
            {
                // 画直角边框
                using (Pen borderPen = new Pen(borderColor, borderWidth))
                {
                    g.DrawRectangle(borderPen,
                        borderWidth / 2,
                        borderWidth / 2,
                        (float)this.Width - borderWidth, (float)this.Height - borderWidth);
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(RectangleF rectangle, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            float x = rectangle.X;
            float y = rectangle.Y;
            float width = rectangle.Width;
            float height = rectangle.Height;

            path.AddArc(x, y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            path.AddLine(x + cornerRadius, y, x + width - cornerRadius, y);
            path.AddArc(x + width - cornerRadius * 2, y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            path.AddLine(x + width, y + cornerRadius, x + width, y + height - cornerRadius);
            path.AddArc(x + width - cornerRadius * 2, y + height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            path.AddLine(x + width - cornerRadius, y + height, x + cornerRadius, y + height);
            path.AddArc(x, y + height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            path.AddLine(x, y + height - cornerRadius, x, y + cornerRadius);

            path.CloseFigure();

            return path;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.MouseLeave += DidMouseReallyLeave;
      
            base.OnControlAdded(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (MouseClickSwitchSelectStatus)
            {
                IsSelected = !IsSelected;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (MouseHoverShowFocus)
            {
                IsMouseHovering = true;
            }

            base.OnMouseEnter(e);

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

            if (MouseHoverShowFocus)
            {
                IsMouseHovering = false;
            }
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
                int boderWidth = BorderWidth > 0 && BorderColor != Color.Empty ? BorderWidth : 0;
                int x = boderWidth + Padding.Left + RoundBorderRadius;
                int y = boderWidth + Padding.Top + RoundBorderRadius;
                int width = Width - boderWidth * 2 - RoundBorderRadius * 2 - Padding.Left - Padding.Right;
                int height = Height - boderWidth * 2 - RoundBorderRadius * 2 - Padding.Top - Padding.Bottom;

                return new Rectangle(x, y, width, height);
            }
        }


        #endregion
    }
}
