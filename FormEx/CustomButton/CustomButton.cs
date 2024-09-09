using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms.Design;


namespace System.Windows.Forms
{
    // Mick Dohertys' .net Tips and Tricks 源代码见 http://dotnetrix.co.uk/button.htm

    /// <summary>
    /// 自定义按钮
    /// </summary>
    public class CustomButton : Control, IButtonControl
    {

        #region 构造

        public CustomButton()
            : base()
        {
            this.SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);

            BorderColor = DarkenColor(this.BackColor, 25);
            Size = new Size(52, 24);
        }

        #endregion

        #region 私有字段

        private DialogResult m_DialogResult;
        private bool m_IsDefault;

        private int m_CornerRadius = 8;
        private Corners m_RoundCorners;
        private CustomButtonState m_ButtonState = CustomButtonState.Normal;

        private ContentAlignment m_ImageAlign = ContentAlignment.MiddleCenter;
        private ContentAlignment m_TextAlign = ContentAlignment.MiddleCenter;
        private ImageList m_ImageList;
        private int m_ImageIndex = -1;

        private bool _keyPressed;
        private Rectangle _contentRect;
        private bool _imageCloseToText;

        private Color _backColor;

        #endregion

        #region IButtonControl 接口实现

        [Category("Behavior"), DefaultValue(typeof(DialogResult), "None")]
        [Description("The dialog result produced in a modal form by clicking the button.")]
        public DialogResult DialogResult
        {
            get { return m_DialogResult; }
            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                    m_DialogResult = value;
            }
        }


        public void NotifyDefault(bool value)
        {
            if (m_IsDefault != value)
                m_IsDefault = value;
            this.Invalidate();
        }


        public void PerformClick()
        {
            if (this.CanSelect)
                base.OnClick(EventArgs.Empty);
        }


        #endregion

        #region 属性

        [DefaultValue(typeof(Color), "Control")] 
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [Bindable(true)]
        public new virtual Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                Invalidate();
            }
        }


        /// <summary>
        /// 按钮状态
        /// </summary>
        [Browsable(false)]
        public CustomButtonState ButtonState
        {
            get { return m_ButtonState; }
        }


        [Browsable(false)]
        public bool IsDefault
        {
            get { return m_IsDefault; }
        }


        /// <summary>
        /// 圆角
        /// </summary>
        [Category(Consts.DefaultCategory)]
        [DefaultValue(14)]
        [Description("圆角大小")]
        public int CornerRadius
        {
            get { return m_CornerRadius; }
            set
            {
                if (m_CornerRadius == value)
                    return;
                m_CornerRadius = value;
                this.Invalidate();
            }
        }


        [Category(Consts.DefaultCategory)]
        protected override System.Drawing.Size DefaultSize
        {
            get { return new Size(75, 23); }
        }

        [Category(Consts.DefaultCategory)]
        [Description("背景色是否显渐变.")]
        public bool GradientMode
        {
            get;
            set;
        }

        [Category(Consts.DefaultCategory)]
        [Description("背景色是否显示阴影模式.")]
        public bool ShadeMode
        {
            get;
            set;
        }


        [Category("Custom"), DefaultValue(typeof(ImageList), null)]
        [Description("指定使用的图片集合")]
        public ImageList ImageList
        {
            get { return m_ImageList; }
            set
            {
                m_ImageList = value;
                this.Invalidate();
            }
        }


        [Category("Custom"), DefaultValue(-1)]
        [Description("指定使用的图片索引")]
        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public int ImageIndex
        {
            get { return m_ImageIndex; }
            set
            {
                m_ImageIndex = value;
                this.Invalidate();
            }
        }


        [Category("Custom"), DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("图片对齐方式")]
        public ContentAlignment ImageAlign
        {
            get { return m_ImageAlign; }
            set
            {
                if (!Enum.IsDefined(typeof(ContentAlignment), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
                if (m_ImageAlign == value)
                    return;
                m_ImageAlign = value;
                this.Invalidate();
            }
        }


        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Corners), "None")]
        [Description("设置几边圆角")]
        [Editor(typeof(RoundCornersEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Obsolete("不再使用此属性.")]
        public Corners RoundCorners
        {
            get { return m_RoundCorners; }
            set
            {
                if (m_RoundCorners == value)
                    return;
                m_RoundCorners = value;
                this.Invalidate();
            }
        }


        [Category("Custom"), DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("文字对齐方式")]
        public ContentAlignment TextAlign
        {
            get { return m_TextAlign; }
            set
            {
                if (!Enum.IsDefined(typeof(ContentAlignment), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
                if (m_TextAlign == value)
                    return;
                m_TextAlign = value;
                this.Invalidate();
            }
        }

        [Category("Custom"), DefaultValue(typeof(Color), "Gray")]
        [Description("边框颜色")]
        public virtual Color BorderColor { get; set; }

        [Category("Custom"), DefaultValue(typeof(bool), "False")]
        [Description("图片紧靠文本")]
        public bool ImageCloseToText
        {
            get
            {
                return _imageCloseToText;
            }
            set
            {
                _imageCloseToText = value;
            }
        }

        [Category(Consts.DefaultCategory)]
        public Color HoverColor
        {
            get;
            set;
        }


        #endregion

        #region 重写方法

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                _keyPressed = true;
                m_ButtonState = CustomButtonState.Pressed;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Space)
            {
                if (this.ButtonState == CustomButtonState.Pressed)
                    this.PerformClick();
                _keyPressed = false;
                m_ButtonState = CustomButtonState.Focused;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!_keyPressed)
                m_ButtonState = CustomButtonState.Hot;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!_keyPressed)
            {
                if (this.IsDefault)
                    m_ButtonState = CustomButtonState.Focused;
                else
                    m_ButtonState = CustomButtonState.Normal;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Focus();
                m_ButtonState = CustomButtonState.Pressed;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_ButtonState = CustomButtonState.Focused;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (new Rectangle(Point.Empty, this.Size).Contains(e.X, e.Y) && e.Button == MouseButtons.Left)
                m_ButtonState = CustomButtonState.Pressed;
            else
            {
                if (_keyPressed)
                    return;
                m_ButtonState = CustomButtonState.Hot;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            m_ButtonState = CustomButtonState.Focused;
            this.NotifyDefault(true);
        }


        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            Form parentForm = this.FindForm();
            if (parentForm == null)
                return;
            if (parentForm.Focused)
                this.NotifyDefault(false);
            m_ButtonState = CustomButtonState.Normal;
        }


        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this.Enabled)
                m_ButtonState = CustomButtonState.Normal;
            else
                m_ButtonState = CustomButtonState.Disabled;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnClick(EventArgs e)
        {
            //Click gets fired before MouseUp which is handy
            if (this.ButtonState == CustomButtonState.Pressed)
            {
                this.Focus();
                this.PerformClick();
            }
        }


        protected override void OnDoubleClick(EventArgs e)
        {
            if (this.ButtonState == CustomButtonState.Pressed)
            {
                this.Focus();
                this.PerformClick();
            }
        }


        protected override bool ProcessMnemonic(char charCode)
        {
            if (IsMnemonic(charCode, this.Text))
            {
                base.OnClick(EventArgs.Empty);
                return true;
            }
            return base.ProcessMnemonic(charCode);
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }




        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SetSlowRendering();

            #region 模拟透明度

            GraphicsContainer gc = g.BeginContainer();
            try
            {
                // 平移图形以模拟透明度   
                g.TranslateTransform(-this.Left, -this.Top);
                PaintEventArgs pe = new PaintEventArgs(g, this.Bounds);
                this.InvokePaintBackground(this.Parent, pe);
                this.InvokePaint(this.Parent, pe);
            }
            catch
            {
                // 处理异常（例如在 GroupBox 控件中）  
            }
            finally
            {
                g.ResetTransform();
                g.EndContainer(gc);
            }

            #endregion

            Color fillColor, shadeColor;
            CalculateColors(out fillColor, out shadeColor);
             

            // 创建图形路径  
            GraphicsPath path = CreateGraphicsPath(ClientRectangle);
            FillPath(g, path, fillColor, shadeColor);

            // 绘制边框
            DrawBorder(g, path);


            // 计算内容矩形  
            _contentRect = CalculateContentRectangle(path, ClientRectangle);
        }

        private void DrawBorder(Graphics g, GraphicsPath path)
        {
            if (CornerRadius > 0)
            {
                Rectangle adjustedRectangle = Rectangle.Inflate(ClientRectangle, -1, -1);
                using (GraphicsPath outerPath = CreateRoundedRectanglePath(ClientRectangle, CornerRadius))
                using (GraphicsPath innerPath = CreateRoundedRectanglePath(adjustedRectangle, CornerRadius - 1))
                using (Pen outerPen = new Pen(Parent.BackColor, 1))
                using (Pen innerPen = new Pen(BorderColor, 1))
                {
                    innerPen.Alignment = PenAlignment.Center;
                    g.DrawPath(outerPen, outerPath);
                    g.DrawPath(innerPen, innerPath);
                }
            }
            else
            {
                using (Pen borderPen = new Pen(BorderColor, 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawImage(e.Graphics);
            DrawText(e.Graphics);
            DrawFocus(e.Graphics);
            base.OnPaint(e);
        }


        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            this.Invalidate();
        }


        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {
            base.OnParentBackgroundImageChanged(e);
            this.Invalidate();
        }


        #endregion

        #region 绘制相关

        private void DrawImage(Graphics g)
        {
            if (this.ImageList == null || this.ImageIndex == -1)
                return;
            if (this.ImageIndex < 0 || this.ImageIndex >= this.ImageList.Images.Count)
                return;

            Image _Image = this.ImageList.Images[this.ImageIndex];

            Point pt = Point.Empty;

            switch (this.ImageAlign)
            {
                case ContentAlignment.TopLeft:
                    pt.X = _contentRect.Left;
                    pt.Y = _contentRect.Top;
                    break;

                case ContentAlignment.TopCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = _contentRect.Top;
                    break;

                case ContentAlignment.TopRight:
                    pt.X = _contentRect.Right - _Image.Width;
                    pt.Y = _contentRect.Top;
                    break;

                case ContentAlignment.MiddleLeft:
                    pt.X = _contentRect.Left;
                    pt.Y = (Height - _Image.Height) / 2;

                    if (_imageCloseToText)
                    {
                        var textPosition = GetTextPosition();
                        pt.X = (int)(textPosition.X - _Image.Width - 8); 
                    }
                    break;

                case ContentAlignment.MiddleCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = (Height - _Image.Height) / 2;
                    break;

                case ContentAlignment.MiddleRight:
                    pt.X = _contentRect.Right - _Image.Width;
                    pt.Y = (Height - _Image.Height) / 2;
                    break;

                case ContentAlignment.BottomLeft:
                    pt.X = _contentRect.Left;
                    pt.Y = _contentRect.Bottom - _Image.Height;
                    break;

                case ContentAlignment.BottomCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = _contentRect.Bottom - _Image.Height;
                    break;

                case ContentAlignment.BottomRight:
                    pt.X = _contentRect.Right - _Image.Width;
                    pt.Y = _contentRect.Bottom - _Image.Height;
                    break;
            }

            if (this.ButtonState == CustomButtonState.Pressed)
                pt.Offset(1, 1);

            if (this.Enabled)
                this.ImageList.Draw(g, pt, this.ImageIndex);
            else
                ControlPaint.DrawImageDisabled(g, _Image, pt.X, pt.Y, this.BackColor);

        }

        private Size GetImageSize()
        {
            Size size = new Size(0, 0);
            if (this.ImageList == null || this.ImageIndex == -1)
                return size;
            if (this.ImageIndex < 0 || this.ImageIndex >= this.ImageList.Images.Count)
                return size;

            Image _Image = this.ImageList.Images[this.ImageIndex];
            size = new Size(_Image.Width, _Image.Height);

            return size;
        }

        private RectangleF GetTextPosition()
        {
            RectangleF rectangle = new RectangleF(_contentRect.X, _contentRect.Y, _contentRect.Width, _contentRect.Height);
            StringFormat stringFormat = GetStringFormat();

            Graphics graphics = CreateGraphics();
            SizeF textSize = graphics.MeasureString(this.Text, this.Font, rectangle.Size, stringFormat);

            // 计算文字的实际绘制位置
            PointF textPosition = PointF.Empty;

            // 根据StringFormat的位置设置来确定文字在矩形内部的位置
            if (stringFormat.Alignment == StringAlignment.Center)
            {
                textPosition.X = rectangle.X + (rectangle.Width - textSize.Width) / 2;
                
                // 2024-03-28
                if (ImageCloseToText)
                {
                    var imgsize = GetImageSize();
                    textPosition.X = rectangle.X + (rectangle.Width - textSize.Width + imgsize.Width + 8) / 2;
                }
            }
            else if (stringFormat.Alignment == StringAlignment.Far)
            {
                textPosition.X = rectangle.Right - textSize.Width;
            }
            else
            {
                textPosition.X = rectangle.X;
            }

            if (stringFormat.LineAlignment == StringAlignment.Center)
            {
                textPosition.Y = rectangle.Y + (rectangle.Height - textSize.Height) / 2;
            }
            else if (stringFormat.LineAlignment == StringAlignment.Far)
            {
                textPosition.Y = rectangle.Bottom - textSize.Height;
            }
            else
            {
                textPosition.Y = rectangle.Y;
            }

            return new RectangleF(textPosition.X, textPosition.Y, textSize.Width, textSize.Height);
        }

        private StringFormat GetStringFormat()
        {
            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);

            if (ShowKeyboardCues)
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            else
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;

            switch (this.TextAlign)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }


            return sf;
        }


        private void DrawText(Graphics g)
        {
            // 2024-03-28 
            if (ImageCloseToText)
            {
                SolidBrush TextBrush = new SolidBrush(this.ForeColor);
                
                if (!this.Enabled)
                    TextBrush.Color = SystemColors.GrayText;


                RectangleF R = GetTextPosition();
               
                
                if (this.ButtonState == CustomButtonState.Pressed)
                    R.Offset(1, 1);


                if (this.Enabled)
                { 
                        g.DrawString(this.Text, this.Font, TextBrush, R); 
                } 
                else
                {
                    ControlPaint.DrawStringDisabled(g, this.Text, this.Font, this.BackColor, 
                        new Rectangle((int)R.X, (int)R.Y, (int)R.Width, (int)R.Height)
                        , TextFormatFlags.Default);
                }

                TextBrush.Dispose();
            }
            else
            {
                SolidBrush TextBrush = new SolidBrush(this.ForeColor);

                // 因字体不同可能会发生偏移
                RectangleF R = new RectangleF(_contentRect.X, _contentRect.Y, _contentRect.Width, _contentRect.Height);

                if (!this.Enabled)
                    TextBrush.Color = SystemColors.GrayText;

                StringFormat sf = GetStringFormat();

                if (this.ButtonState == CustomButtonState.Pressed)
                    R.Offset(1, 1);

                if (this.Enabled)
                    g.DrawString(this.Text, this.Font, TextBrush, R, sf);
                else
                    ControlPaint.DrawStringDisabled(g, this.Text, this.Font, this.BackColor, R, sf);
            }

        }


        private void DrawFocus(Graphics g)
        {
            Rectangle r = _contentRect;
            r.Inflate(1, 1);
            if (this.Focused && this.ShowFocusCues && this.TabStop)
                ControlPaint.DrawFocusRectangle(g, r, this.ForeColor, this.BackColor);
        }

        
        
        private GraphicsPath RoundRectangle(Rectangle r, int radius, Corners corners)
        {
            //Make sure the Path fits inside the rectangle
            //r.Width -= 1;
            //r.Height -= 1;

            //Scale the _circleRadius if it's too large to fit.
            if (radius > (r.Width))
                radius = r.Width;
            if (radius > (r.Height))
                radius = r.Height;

            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
                path.AddRectangle(r);
            else
                if ((corners & Corners.TopLeft) == Corners.TopLeft)
                path.AddArc(r.Left, r.Top, radius, radius, 180, 90);
            else
                path.AddLine(r.Left, r.Top, r.Left, r.Top);

            if ((corners & Corners.TopRight) == Corners.TopRight)
                path.AddArc(r.Right - radius, r.Top, radius, radius, 270, 90);
            else
                path.AddLine(r.Right, r.Top, r.Right, r.Top);

            if ((corners & Corners.BottomRight) == Corners.BottomRight)
                path.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
            else
                path.AddLine(r.Right, r.Bottom, r.Right, r.Bottom);

            if ((corners & Corners.BottomLeft) == Corners.BottomLeft)
                path.AddArc(r.Left, r.Bottom - radius, radius, radius, 90, 90);
            else
                path.AddLine(r.Left, r.Bottom, r.Left, r.Bottom);

            path.CloseFigure();

            return path;
        }
        

        private Color DarkenColor(Color colorIn, int percent)
        {
            //This method returns Black if you Darken by 100%

            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException("percent");

            int a, r, g, b;

            a = colorIn.A;
            r = colorIn.R - (int)((colorIn.R / 100f) * percent);
            g = colorIn.G - (int)((colorIn.G / 100f) * percent);
            b = colorIn.B - (int)((colorIn.B / 100f) * percent);

            return Color.FromArgb(a, r, g, b);
        }


        private Color LightenColor(Color colorIn, int percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException("percent");

            int a, r, g, b;

            a = colorIn.A;
            r = colorIn.R + (int)(((255f - colorIn.R) / 100f) * percent);
            g = colorIn.G + (int)(((255f - colorIn.G) / 100f) * percent);
            b = colorIn.B + (int)(((255f - colorIn.B) / 100f) * percent);

            return Color.FromArgb(a, r, g, b);
        }


        // 创建图形路径  
        private GraphicsPath CreateGraphicsPath(Rectangle rect)
        {
            if (CornerRadius > 0)
            {
                return CreateRoundedRectanglePath(rect, CornerRadius);
            }
            else
            { 
                return CreateRectanglePath(rect);
            }

            //return (CornerRadius > 0)
            //    ? RoundRectangle(rect, this.CornerRadius, this.RoundCorners)
            //    : CreateRectanglePath(rect);
        }
        // 创建标准矩形路径  
        private GraphicsPath CreateRectanglePath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath(FillMode.Winding);
            path.AddRectangle(new Rectangle(0, 0, rect.Width, rect.Height));
            path.CloseFigure();
            return path;
        }

        // 计算填充颜色和阴影颜色  
        private void CalculateColors(out Color color1, out Color color2)
        {
            switch (this.ButtonState)
            {
                case CustomButtonState.Hot:
                case CustomButtonState.Pressed:
                    {
                        color1 = HoverColor != Color.Empty ? HoverColor : BackColor;
                        color2 = HoverColor != Color.Empty ? HoverColor : BackColor;
                        if (GradientMode)
                        {
                            color1 = LightenColor(color1, 25);
                            color2 = DarkenColor(color2, 15);
                        }
                    }
                    break;
                default:
                    {
                        color1 = this.BackColor;
                        color2 = this.BackColor;
                        if (GradientMode)
                        {
                            color1 = LightenColor(color1, 25);
                            color2 = DarkenColor(color2, 15);
                        }

                    }
                    break;
            }
        }
        // 填充路径  
        private void FillPath(Graphics g, GraphicsPath path, Color fillColor, Color shadeColor)
        {
            if (this.Enabled)
            {
                using (LinearGradientBrush paintBrush = new LinearGradientBrush(ClientRectangle, fillColor, shadeColor, LinearGradientMode.Vertical))
                {
                    if (ShadeMode)
                    {
                        Blend blend = new Blend
                        {
                            Positions = new float[] { 0, 0.5F, 0.55F, 1 },
                            Factors = new float[] { 0, 0, 1, 1 }
                        };
                        paintBrush.Blend = blend;
                    }
                    g.FillPath(paintBrush, path);
                }
            }
            else
            {
                using (Brush solidBrush = new SolidBrush(fillColor))
                {
                    g.FillPath(solidBrush, path);
                }
            }
        }


        private GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, int cornerRadius)
        {
            GraphicsPath roundedRectPath = new GraphicsPath();


            // 检查并确保矩形宽度和高度有效
            if (bounds.Width <= 1 || bounds.Height <= 1)
            {
                // 如果宽度或高度太小，直接添加一个简单的矩形路径
                roundedRectPath.AddRectangle(bounds);
                return roundedRectPath;
            }

            // 限制 cornerRadius，不超过宽度或高度的一半
            cornerRadius = Math.Max(0, Math.Min(cornerRadius, Math.Min(bounds.Width, bounds.Height) / 2));
            float diameter = cornerRadius * 2F;

            // 如果 cornerRadius 为 0，处理为普通矩形
            if (cornerRadius == 0)
            {
                roundedRectPath.AddRectangle(bounds);
                return roundedRectPath;
            }


            // 添加圆角的四个弧形
            roundedRectPath.StartFigure();
            roundedRectPath.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90); // 左上角
            roundedRectPath.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90); // 右上角
            roundedRectPath.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90); // 右下角
            roundedRectPath.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90); // 左下角
            roundedRectPath.CloseFigure();

            return roundedRectPath;
        }


        // 计算内容矩形
        private Rectangle CalculateContentRectangle(GraphicsPath path, Rectangle originalRect)
        {
            Rectangle r = originalRect;
            bool inBounds = false;

            while (!inBounds && r.Width >= 1 && r.Height >= 1)
            {
                inBounds = path.IsVisible(r.Left, r.Top) &&
                            path.IsVisible(r.Right, r.Top) &&
                            path.IsVisible(r.Left, r.Bottom) &&
                            path.IsVisible(r.Right, r.Bottom);
                r.Inflate(-1, -1);
            }

            return r;
        }


        #endregion

        private CustomButtonState currentState;
        private void OnStateChange(EventArgs e)
        {
            //Repaint the button only if the state has actually changed
            if (this.ButtonState == currentState)
            {
                return;
            }
            currentState = this.ButtonState;
            this.Invalidate();
        }
    }

    [System.Flags]
    public enum Corners
    {
        None = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 4,
        BottomRight = 8,
        Left = TopLeft | BottomLeft,
        Right = TopRight | BottomRight,
        Top = TopLeft | TopRight,
        Bottom = BottomLeft | BottomRight,
        All = TopLeft | TopRight | BottomLeft | BottomRight
    }

    public enum CustomButtonState
    {
        Normal = 1,
        Hot,
        Pressed,
        Disabled,
        Focused
    }

    #region RoundCorners 属性的设计时编辑器

#pragma warning disable SYSLIB0003 // 类型或成员已过时
    [PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
    [PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
#pragma warning restore SYSLIB0003 // 类型或成员已过时
    public class RoundCornersEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override Object EditValue(ITypeDescriptorContext context, IServiceProvider provider, Object value)
        {
            if (value.GetType() != typeof(Corners) || provider == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                CheckedListBox lb = new CheckedListBox();
                lb.BorderStyle = BorderStyle.None;
                lb.CheckOnClick = true;

                lb.Items.Add("TopLeft", (((CustomButton)context.Instance).RoundCorners & Corners.TopLeft) == Corners.TopLeft);
                lb.Items.Add("TopRight", (((CustomButton)context.Instance).RoundCorners & Corners.TopRight) == Corners.TopRight);
                lb.Items.Add("BottomLeft", (((CustomButton)context.Instance).RoundCorners & Corners.BottomLeft) == Corners.BottomLeft);
                lb.Items.Add("BottomRight", (((CustomButton)context.Instance).RoundCorners & Corners.BottomRight) == Corners.BottomRight);

                edSvc.DropDownControl(lb);
                Corners cornerFlags = Corners.None;
                foreach (object o in lb.CheckedItems)
                {
                    cornerFlags = cornerFlags | (Corners)Enum.Parse(typeof(Corners), o.ToString());
                }
                lb.Dispose();
                edSvc.CloseDropDown();
                return cornerFlags;
            }
            return value;
        }


    }


    #endregion

}
