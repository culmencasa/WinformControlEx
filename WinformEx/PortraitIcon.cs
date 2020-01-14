using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.ComponentModel;

namespace System.Windows.Forms
{
    /// <summary>
    /// 图标控件
    /// </summary>
    public class PortraitIcon : NonFlickerUserControl
    {
        #region 字段

        protected int _leftBarSize;
        protected int _rightBarSize;


        // 边框部分
        protected Color _borderColor;
        protected Size _borderRadius;
        protected int _borderWidth = 1;




        // 图片部分
        protected Image _vividImage;
        protected Image _grayImage;
        protected Rectangle _imageArea;

        protected Size _imageSize = new Size(32, 32);
        private IconSizeMode _sizeMode;

        // 文字部分
        protected string _caption = string.Empty;
        protected string _textToDisplay = string.Empty;

        protected Padding _captionMargin;

        // 移动变量
        protected bool _isDragging;
        protected bool _isHovering;

        private int _DDradius = 40;
        private int _mX = 0;
        private int _mY = 0;

        Cursor _tempCursor = null;


        #endregion

        #region 公开属性
        
        /// <summary>
        /// 文本
        /// </summary>
        public new string Text
        {
            get
            {
                return this.Caption;
            }
            set
            {
                this.Caption = value;
            }
        }

        /// <summary>
        /// 控件显示的标题文本
        /// </summary>
        [DefaultValue("")]
        public string Caption 
        {
            get
            {
                return _caption;
            }
            set
            {
                bool isChanged = (value != _caption);
                _caption = value;
                if (isChanged)
                {
                    OnCaptionChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 是否显示标题
        /// </summary>
        public bool ShowCaption { get; set; }

        public bool ShowIconBorder { get; set; }

        /// <summary>
        /// 是否允许拖动
        /// </summary>
        public bool AllowDrag { get; set; }

        /// <summary>
        /// 圆角
        /// </summary>
        public int RoundedCornerAngle { get; set; }

        /// <summary>
        /// 选中后的背景色
        /// </summary>
        public Color FocusBackgroundColor { get; set; }
        /// <summary>
        /// 悬浮的背景色
        /// </summary>
        public Color HoverBackgroundColor { get; set; }

        /// <summary>
        /// 背景色1
        /// </summary>
        public Color FirstColor { get; set; }

        /// <summary>
        /// 背景色2
        /// </summary>
        public Color SecondColor { get; set; }
               
        /// <summary>
        /// 图像
        /// </summary>
        [DefaultValue(typeof(Image), "null")]
        public Image Image
        {
            get
            {
                return _vividImage;
            }
            set
            {
                bool valueChanged = _vividImage != value;
                _vividImage = value;
                if (valueChanged)
                {
                    OnIconImageChanged(EventArgs.Empty);
                }


            }
        }


        /// <summary>
        /// 灰度图像
        /// </summary>
        [Browsable(false)]
        [DefaultValue(typeof(Image), "null")]
        public Image GrayImage
        {
            get
            {
                if (_grayImage != null)
                {
                    return _grayImage; 
                }

                if (_vividImage != null)
                {
                    _grayImage = _vividImage.MakeGrayscale3();
                    return _grayImage;
                }

                return null;
            }
            set
            {
                _grayImage = value;
            }
        }

        /// <summary>
        /// 是否选中 (外部容器使用)
        /// </summary>
        public bool IsSelected { get; set; }

        public bool IsDragging
        {
            get
            {
                return _isDragging;
            }
        }

        /// <summary>
        /// 是否显示为灰色图像
        /// </summary>
        public bool ShowGrayImage { get; set; }


        protected Padding CaptionMargin
        {
            get { return _captionMargin; }
            set { _captionMargin = value; }
        }


        public Object DragSender { get; set; }

        /// <summary>
        /// 图像的大小模式
        /// </summary>
        public IconSizeMode SizeMode
        {
            get 
            { 
                return _sizeMode; 
            }
            set 
            {
                _sizeMode = value;
                _imageArea = this.GetImageArea();
                Invalidate();
            }
        }

        #endregion

        #region 保护属性

        protected Size ImageSize
        {
            get { return _imageSize; }
        }


        #endregion

        private int _FillDegree = 100;
        public int FillDegree
        {
            get { return _FillDegree; }
            set
            {
                if (value >= 100)
                {
                    FirstColor = Color.DimGray;
                    SecondColor = Color.DarkGray;
                }
                else if (value > 90)
                {
                    FirstColor = Color.Orange;
                    SecondColor = Color.DarkOrange;
                }
                else if (value > 80)
                {
                    FirstColor = Color.Gold;
                    SecondColor = Color.DarkGoldenrod;
                }
                else
                {
                    FirstColor = Color.Green;
                    SecondColor = Color.DarkGreen;
                }
                _FillDegree = value;
            }
        }

        #region 构造方法

        /// <summary>
        /// 创建一个IconView实例
        /// </summary>
        public PortraitIcon()
        {
            InitialDefaultValues();



        }

        #endregion

        #region 私有方法

        // 初始化默认值
        private void InitialDefaultValues()
        {
            _leftBarSize = 10;
            _rightBarSize = 10;

            _borderRadius = new System.Drawing.Size(1, 1);
            _borderColor = Color.FromArgb(102, 167, 232);

            _captionMargin = new System.Windows.Forms.Padding(0);

            _imageArea = this.GetImageArea();


            this.BackColor = Color.Transparent;
            this.ForeColor = Color.Black;
            this.FirstColor = Color.White;
            this.SecondColor = Color.White;
            this.FocusBackgroundColor = Color.FromArgb(209,232,255);
            this.HoverBackgroundColor = Color.FromArgb(184, 224, 243);
            this.Margin = new Padding(0);
            this.Padding = new Padding(0);

            this.RoundedCornerAngle = 10;
            this.Caption = string.Empty;
            this.AllowDrag = false;
            this.AllowDrop = false;
            this.ShowCaption = true;
            this.Size = new Size(32, 32);

        }

        private void OnIconImageChanged(EventArgs e)
        {
            if (_grayImage != null)
            {
                _grayImage.Dispose();
            }
            if (_vividImage != null)
            {
                _grayImage = _vividImage.MakeGrayscale3();
                // 更新图像区域
                _imageArea = this.GetImageArea();
            }
            else
            {
                _grayImage = null;
            }

            Invalidate();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 设置图像的显示模型
        /// </summary>
        /// <param name="model"></param>
        public virtual void SetDisplayModel(IconModel model)
        {
            this.Font = model.IconFont;
            this.Padding = model.Padding;
            this.Margin = model.Margin;
            this.Size = new Size(
                model.CustomSize.Width + this.Padding.Left + this.Padding.Right + _borderWidth * 2, 
                model.CustomSize.Height + this.Padding.Top + this.Padding.Bottom + _borderWidth * 2 + (int)this.GetTextArea().Height + _captionMargin.Top + _captionMargin.Bottom
            );

            _imageSize = model.CustomSize;

        }


        #endregion

        #region 重写的成员

        protected override void OnGotFocus(EventArgs e)
        {
            this.BackColor = this.FocusBackgroundColor;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.BackColor = Color.Transparent;
            base.OnLostFocus(e);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            _imageArea = this.GetImageArea();
            Invalidate();
            base.OnResize(eventargs);

        }

        #region 鼠标

        protected override void OnMouseEnter(EventArgs e)
        {
            this.BackColor = this.HoverBackgroundColor;
            this._isHovering = true;
            this.Invalidate();

            base.OnMouseEnter(e);
        }
        
        protected override void OnMouseLeave(EventArgs e)
        {
            this.BackColor = Color.Transparent;
            this._isHovering = false;
            this.Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this.Focus();
            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
            _mX = e.X;
            _mY = e.Y;
            this._isDragging = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isDragging = false;
            base.OnMouseUp(e);

            _tempCursor.Dispose();
            _tempCursor = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                if (e.Button == MouseButtons.Left && _DDradius > 0 && this.AllowDrag)
                {
                    int num1 = _mX - e.X;
                    int num2 = _mY - e.Y;
                    if (((num1 * num1) + (num2 * num2)) > _DDradius)
                    {
                        this.DoDragDrop(new DragDropInfo(this.DragSender, this), DragDropEffects.All);
                        _isDragging = false;
                        return;
                    }
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            try
            {
                base.OnGiveFeedback(gfbevent);
                if (gfbevent.Effect != DragDropEffects.None)
                {
                    gfbevent.UseDefaultCursors = false;

                    if (_tempCursor == null || Cursor.Current.Handle != _tempCursor.Handle)
                    {
                        if (_tempCursor != null)
                            _tempCursor.Dispose();

                        // 
                        using (Bitmap tempBitmap = new Bitmap(this.Image.Width + 20, this.Image.Height + 32))
                        using (Graphics g = Graphics.FromImage(tempBitmap))
                        using (StringFormat textformat = new StringFormat())
                        using (SolidBrush fontBrush = new SolidBrush(Color.White))
                        {
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            g.FillRoundedRectangle(
                                Brushes.LightSlateGray,
                                0,
                                tempBitmap.Height - 20,
                                tempBitmap.Width - 4,
                                16,
                                8,
                                RectangleEdgeFilter.All);

                            // 头像
                            Rectangle imageRect = new Rectangle(
                                (tempBitmap.Width - this.Image.Width) / 2,
                                8,
                                this.Image.Width,
                                this.Image.Height);
                            g.FillRectangle(Brushes.White, imageRect);
                            g.DrawRectangle(Pens.LightBlue, imageRect);
                            imageRect.Inflate(-4, -4);
                            g.DrawImageTransparent(this.Image, imageRect);

                            textformat.Alignment = StringAlignment.Center;
                            RectangleF textRect = new RectangleF(
                                    0,
                                    this.Image.Height + this.Padding.Bottom + 12,
                                    tempBitmap.Width,
                                    tempBitmap.Height);
                            g.DrawString(this.Caption, this.Font, fontBrush, textRect, textformat);

                            _tempCursor = new Cursor(tempBitmap.GetHicon());
                            Cursor.Current = _tempCursor;

                        }
                    }
                }
                //{
                    //Effect = gfbevent.Effect;
                //}
            }
            catch (Exception)
            {
                this.OnGiveFeedback(gfbevent);
            }
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (this.AllowDrop)
            {
                drgevent.Effect = DragDropEffects.Move;
            }

            base.OnDragEnter(drgevent);
        }

        #endregion

        #region 重绘

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            using (Brush coverBrush = new LinearGradientBrush(this.ClientRectangle, this.FirstColor, this.SecondColor, LinearGradientMode.Vertical), fontBrush = new SolidBrush(this.ForeColor))
            {

                if (_isDragging)
                    DrawSelectedBorder(g);
                else if (_isHovering)
                    DrawSelectedBorder(g);
                else
                    DrawImageFrame(g);

                if (this.Image != null)
                {
                    if (ShowGrayImage)
                    {
                        //g.DrawImage(GrayImage, GetImageArea());
                        g.DrawImage(GrayImage, _imageArea);
                    }
                    else
                    {
                        g.DrawImage(Image, _imageArea);
                    }
                }

                if (this.ShowCaption && !String.IsNullOrEmpty(this.Caption))
                {
                    StringFormat textformat = new StringFormat();
                    textformat.Alignment = StringAlignment.Center;
                    textformat.LineAlignment = StringAlignment.Center;
                    g.DrawString(_textToDisplay, this.Font, fontBrush, this.GetTextArea(), textformat);
                    textformat.Dispose();
                }

            }
            base.OnPaint(e);

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            DrawSelectedBackground(e.Graphics);
        }

        #endregion

        #endregion


        internal void SetDragRelease()
        {
            _isDragging = false;
            this.Invalidate();
        }

        // 标题文本发生改变时
        protected virtual void OnCaptionChanged(EventArgs e)
        {
            _textToDisplay = this.Caption;
            this.GetTextArea();
            this.Invalidate();
        }

        protected virtual void DrawSelectedBackground(Graphics g)
        {
            if (this.IsSelected)
            {
                this.BackColor = this.FocusBackgroundColor;
                DrawSelectedBorder(g);
            }
        }

        // 绘制控件边框
        protected virtual void DrawSelectedBorder(Graphics g)
        {
            if (this.IsSelected)
            {
                using (Pen borderPen = new Pen(_borderColor, _borderWidth))
                {
                    g.DrawRectangle(borderPen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                }
            }
        }

        // 绘制图像边框
        protected virtual void DrawImageFrame(Graphics g)
        {
            if (this.Image == null)
                return;

            if (ShowIconBorder)
            {
                Color backColorStart = Color.White;
                Color backColorEnd = Color.Wheat;
                Color borderColor = Color.LightBlue;
                Size borderSize = new System.Drawing.Size(0, 0);

                //gx.DrawGradientRoundedRectangleAlpha(
                //    GetBorderBounds(),
                //    backColorStart,
                //    backColorEnd,
                //    borderColor,
                //    borderSize,
                //    110,
                //    FillDirection.TopToBottom);
                //g.DrawRoundedRectangleAlpha(Color.LightBlue, Color.White, GetImageBorderBounds(), new Size(3, 3), 255);

                g.DrawGradientRoundedRectangle(
                    GetImageBorderBounds(), 
                    backColorStart, 
                    backColorEnd,
                    FillDirection.TopToBottom, 
                    borderColor, 8);
                //gx.DrawRoundedRectangle(Color.LightBlue, Color.FromArgb(225, 218, 193), GetBorderBounds(), new Size(2, 2));
            }
        }
        
        protected virtual Rectangle GetImageBorderBounds()
        {
            Rectangle borderBounds = new Rectangle();
            borderBounds.X = 0;
            borderBounds.Y = 0;
            borderBounds.Width = this.Width;
            borderBounds.Height = this.Height - Convert.ToInt32(GetTextArea().Height) - _captionMargin.Bottom - _captionMargin.Top;

            return borderBounds;
        }

        /// <summary>
        /// 获取图像区域
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetImageArea()
        {
            int x = 0, y = 0, width = 1, height = 1;
            if (_sizeMode == IconSizeMode.Stretch)
            {
                x = this.Padding.Left + _borderWidth;
                y = this.Padding.Top + _borderWidth;
                width = this.Width - this.Padding.Left - this.Padding.Right - _borderWidth * 2;
                height = this.Height - this.Padding.Bottom - this.Padding.Top - Convert.ToInt32(this.GetTextArea().Height) - _borderWidth * 2 - _captionMargin.Bottom - _captionMargin.Top;
            }
            else if (_sizeMode == IconSizeMode.Center)
            {
                if (this.Image != null)
                {
                    x = (this.Width - this.Image.Width) / 2;
                    y = (this.Height - this.Image.Height) / 2;
                    width = this.Image.Width;
                    height = this.Image.Height;
                }
            }
            else if (_sizeMode == IconSizeMode.Normal)
            {
                x = this.Padding.Left + _borderWidth;
                y = this.Padding.Top + _borderWidth;
                width = _imageSize.Width;
                height = _imageSize.Height;
            }

            width = width > 0 ? width : 1;
            height = height > 0 ? height : 1;

            return new Rectangle(x, y, width, height);
        }
        
        /// <summary>
        /// 获取文字区域
        /// </summary>
        /// <returns></returns>
        protected virtual RectangleF GetTextArea()
        {
            RectangleF rect = RectangleF.Empty;

            if (!String.IsNullOrEmpty(this.Caption))
            {
                string textToDraw = this.Caption;
                string ellipsisText = "...";

                SizeF stringSize = SizeF.Empty;
                SizeF ellipsisSize = SizeF.Empty;
                using (Graphics g = this.CreateGraphics())
                {
                    stringSize = g.MeasureString(textToDraw, this.Font);
                    ellipsisSize = g.MeasureString(ellipsisText, this.Font);

                    // 文字过长的处理
                    bool trimming = false;
                    int textLength = this.Caption.Length;
                    int textToRemove = this.Caption.Length - 1;
                    float availableWidth = this.Bounds.Width;
                    while (textToRemove > 0 && stringSize.Width > availableWidth)
                    {
                        textToDraw = this.Caption.Remove(textToRemove);
                        stringSize = TextRenderer.MeasureText(textToDraw + ellipsisText, this.Font);

                        textToRemove--;
                        trimming = true;
                    }

                    if (trimming == true)
                    {
                        _textToDisplay = textToDraw + ellipsisText;
                    }
                    else
                    {
                        _textToDisplay = textToDraw;
                    }
                }

                float stringPosX = (this.Width - stringSize.Width) / 2;
                float stringPosY = this.Height - stringSize.Height;

                rect = new RectangleF(stringPosX, stringPosY, stringSize.Width, stringSize.Height);
            }
            else
            { 
                SizeF stringSize = SizeF.Empty;
                using (Graphics g = this.CreateGraphics())
                {
                    stringSize = TextRenderer.MeasureText("　", this.Font);
                }

                rect = new RectangleF(0, 0, stringSize.Width, stringSize.Height);
            }

            return rect;
        }

    }
}
