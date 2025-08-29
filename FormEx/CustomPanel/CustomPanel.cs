using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [DefaultEvent("Click")]
    public class CustomPanel : Panel
    {
        private FillDirection _gradientDirection;
        private Bitmap backgroundImageCache;
        private bool _isMouseHovering;
        private bool _isSelected;
        public delegate void SelectStatusChangedHandler(CustomPanel sender, bool isActived);


        #region 事件

        [Category(Consts.DefaultCategory)]
        public event SelectStatusChangedHandler SelectStatusChanged;

        #endregion

        #region 属性

        [Category(Consts.DefaultCategory)]
        [Description("BackColor为透明时，Panel中的控件应用布局时会引起Panel频繁重绘")]
        public override Color BackColor 
        { get => base.BackColor; 
            set => base.BackColor = value; 
        }


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


        [Category(Consts.DefaultCategory)]
        public bool Draw3DBorder { get; set; }

        [Category(Consts.DefaultCategory)]
        public Border3DStyle Border3DStyle { get; set; } = Border3DStyle.Sunken;



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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            RedrawOnDesignTime();
        }


        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (this.IsHandleCreated && !IsDisposed)
            {   
                backgroundImageCache = DrawCacheBitmap();
                if (backgroundImageCache != null)
                {
                    Invalidate();
                }
            }
        }

		protected override void OnPaint(PaintEventArgs e)
        {
			// 背景为透明时或者子Panel控件或者子控件的背景为透明时，会出现闪烁。
			if (backgroundImageCache != null)
            {
                e.Graphics.DrawImageUnscaled(backgroundImageCache, 0, 0);
            }

            base.OnPaint(e);
        }


        #region 焦点问题

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

        #endregion

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


        #region 2025-01-16 改进

        GraphicsPath CreateRoundedRectangle(RectangleF rectangle, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (cornerRadius <= 0)
            {
                path.AddRectangle(rectangle);
            }
            else
            {
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
            }

            return path;
        }


        Bitmap DrawCacheBitmap()
        {
            Bitmap bitmap;
            Graphics cacheG;
            if (Width > 0 && Height > 0)
            {
                bitmap = new Bitmap(Width, Height);
                cacheG = Graphics.FromImage(bitmap);
            }
            else
            {
                return null;
            }


            Rectangle innerRectangle = new Rectangle(
                BorderWidth / 2, // Pen是从中心开始画的，所以要减半
                BorderWidth / 2,
                ClientRectangle.Width - BorderWidth - 1,
                ClientRectangle.Height - BorderWidth - 1);
            var rectanglePath = CreateRoundedRectangle(innerRectangle, RoundBorderRadius);


            #region 绘制背景


            var color1 = FirstColor;
            var color2 = SecondColor;

            if (color1 == Color.Empty && color2 == Color.Empty)
            {
                color1 = BackColor;
                color2 = BackColor;
            }
            else if (color1 == Color.Empty)
            {
                color1 = color2;
            }
            else if (color2 == Color.Empty)
            {
                color2 = color1;
            }

            if (IsMouseHovering || IsSelected)
            {
                color1 = color2 = MouseHoverBgColor;
            }
                
            using (var brush = new LinearGradientBrush(new Point(Width / 2, 0), new Point(Width / 2, Height), color1, color2))
            {
                cacheG.FillPath(brush, rectanglePath);
            }

            #endregion

            #region 绘制边框

            if (Draw3DBorder)
            {
                ControlPaint.DrawBorder3D(cacheG, new Rectangle(0, 0, this.Width - 1, this.Height - 1), Border3DStyle, Border3DSide.All);
            }
            else if (BorderColor != Color.Empty && BorderWidth > 0)
            {
                var borderColor = BorderColor;
                var borderWidth = BorderWidth;
                if (IsSelected)
                {
                    borderColor = SelectedBorderColor;
                    borderWidth = 2;
                }
                else if (IsMouseHovering)
                {
                    borderColor = MouseHoverBorderColor; 
                }


                using (Pen borderPen = new Pen(borderColor, borderWidth))
                {
                    cacheG.SmoothingMode = SmoothingMode.AntiAlias;
                    cacheG.DrawPath(borderPen, rectanglePath);
                }
            }

            #endregion

            rectanglePath.Dispose();
            cacheG.Dispose();
            return bitmap;
        }


        void RedrawOnDesignTime()
        {
            backgroundImageCache = DrawCacheBitmap();
            if (backgroundImageCache != null)
            {
                Invalidate();
            }
        }

        #endregion

        #region 处理设置FirstColor和SecondColor背景未更新问题

        private bool needRedrawBackground;

        public new void Invalidate()
        {
            needRedrawBackground = true;
			base.Invalidate();
		}
		public new void Invalidate(bool invalidateChildren)
		{
			needRedrawBackground = true;
			base.Invalidate(invalidateChildren);
		}

		public new void Invalidate(Rectangle rect)
		{
			needRedrawBackground = true;
			base.Invalidate(rect);
		}

		public new void Invalidate(Rectangle rect, bool invalidateChildren)
		{
			needRedrawBackground = true;
			base.Invalidate(rect, invalidateChildren);
		}

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
			if (needRedrawBackground)
			{
				needRedrawBackground = false;
				backgroundImageCache = DrawCacheBitmap();
			} 
		}

		#endregion

	}
}
