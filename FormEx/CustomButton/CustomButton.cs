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

        private bool keyPressed;
        private Rectangle contentRect;
        private bool imageCloseToText;

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
        [Category("Custom")]
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


        [Category("Custom")]
        protected override System.Drawing.Size DefaultSize
        {
            get { return new Size(75, 23); }
        }

        [Category("Custom")]
        [Description("背景色是否显渐变.")]
        public bool GradientMode
        {
            get;
            set;
        }

        [Category("Custom")]
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


        [Category("Custom")]
        [DefaultValue(typeof(Corners), "None")]
        [Description("设置几边圆角")]
        [Editor(typeof(RoundCornersEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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
                return imageCloseToText;
            }
            set
            {
                imageCloseToText = value;
            }
        }

        #endregion

        #region 重写方法

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                keyPressed = true;
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
                keyPressed = false;
                m_ButtonState = CustomButtonState.Focused;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!keyPressed)
                m_ButtonState = CustomButtonState.Hot;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!keyPressed)
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
                if (keyPressed)
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


        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Simulate Transparency
            try
            {
                GraphicsContainer g = pevent.Graphics.BeginContainer();
                Rectangle translateRect = this.Bounds;
                pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
                PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, translateRect);
                this.InvokePaintBackground(this.Parent, pe);
                this.InvokePaint(this.Parent, pe);
                pevent.Graphics.ResetTransform();
                pevent.Graphics.EndContainer(g);
            }
            catch
            {
                base.OnPaintBackground(pevent);
            }



            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            Color shadeColor, fillColor;
            Color darkColor = DarkenColor(this.BackColor, 10);
            Color darkDarkColor = DarkenColor(this.BackColor, 15);
            Color lightColor = LightenColor(this.BackColor, 25);
            Color lightLightColor = LightenColor(this.BackColor, 60);

            // 不显示渐变
            if (!GradientMode)
            {
                darkColor = this.BackColor;
                darkDarkColor = this.BackColor;
                lightColor = this.BackColor;
                lightLightColor = this.BackColor;
            }
            else
            {

            }


            if (this.ButtonState == CustomButtonState.Hot)
            {
                fillColor = lightColor;
                shadeColor = darkDarkColor;
            }
            else if (this.ButtonState == CustomButtonState.Pressed)
            {
                fillColor = this.BackColor;
                shadeColor = this.BackColor;
            }
            else
            {
                fillColor = this.BackColor;
                shadeColor = darkDarkColor;
            }

            GraphicsPath path;
            Rectangle r = ClientRectangle;
            if (CornerRadius > 0)
            {
                path = RoundRectangle(r, this.CornerRadius, this.RoundCorners);
            }
            else
            {
                path = new GraphicsPath(FillMode.Winding);
                path.AddRectangle(new Rectangle(0, 0, Width - 1, Height - 1));
                path.CloseFigure();
            }

            if (this.Enabled)
            {
                LinearGradientBrush paintBrush = new LinearGradientBrush(r, fillColor, shadeColor, LinearGradientMode.Vertical);

                if (ShadeMode)
                {
                    Blend b = new Blend();
                    b.Positions = new float[] { 0, 0.45F, 0.55F, 1 };
                    b.Factors = new float[] { 0, 0, 1, 1 };
                    paintBrush.Blend = b;
                }

                pevent.Graphics.FillPath(paintBrush, path);
                paintBrush.Dispose();
            }
            else
            {
                using (Brush solidBrush = new SolidBrush(darkDarkColor))
                {
                    pevent.Graphics.FillPath(solidBrush, path);
                }
            }

            //...and border
            Pen drawingPen = new Pen(BorderColor);
            pevent.Graphics.DrawPath(drawingPen, path);
            drawingPen.Dispose();

            //Get the Rectangle to be used for Content
            bool inBounds = false;
            //We could use some Math to get this from the radius but I'm 
            //not great at Math so for the example this hack will suffice.
            while (!inBounds && r.Width >= 1 && r.Height >= 1)
            {
                inBounds = path.IsVisible(r.Left, r.Top) &&
                            path.IsVisible(r.Right, r.Top) &&
                            path.IsVisible(r.Left, r.Bottom) &&
                            path.IsVisible(r.Right, r.Bottom);
                r.Inflate(-1, -1);

            }

            contentRect = r;
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
                    pt.X = contentRect.Left;
                    pt.Y = contentRect.Top;
                    break;

                case ContentAlignment.TopCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = contentRect.Top;
                    break;

                case ContentAlignment.TopRight:
                    pt.X = contentRect.Right - _Image.Width;
                    pt.Y = contentRect.Top;
                    break;

                case ContentAlignment.MiddleLeft:
                    pt.X = contentRect.Left;
                    pt.Y = (Height - _Image.Height) / 2;

                    if (imageCloseToText)
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
                    pt.X = contentRect.Right - _Image.Width;
                    pt.Y = (Height - _Image.Height) / 2;
                    break;

                case ContentAlignment.BottomLeft:
                    pt.X = contentRect.Left;
                    pt.Y = contentRect.Bottom - _Image.Height;
                    break;

                case ContentAlignment.BottomCenter:
                    pt.X = (Width - _Image.Width) / 2;
                    pt.Y = contentRect.Bottom - _Image.Height;
                    break;

                case ContentAlignment.BottomRight:
                    pt.X = contentRect.Right - _Image.Width;
                    pt.Y = contentRect.Bottom - _Image.Height;
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
            RectangleF rectangle = new RectangleF(contentRect.X, contentRect.Y, contentRect.Width, contentRect.Height);
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
                RectangleF R = new RectangleF(contentRect.X, contentRect.Y, contentRect.Width, contentRect.Height);

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
            Rectangle r = contentRect;
            r.Inflate(1, 1);
            if (this.Focused && this.ShowFocusCues && this.TabStop)
                ControlPaint.DrawFocusRectangle(g, r, this.ForeColor, this.BackColor);
        }


        #endregion

        #region 辅助方法

        private GraphicsPath RoundRectangle(Rectangle r, int radius, Corners corners)
        {
            //Make sure the Path fits inside the rectangle
            r.Width -= 1;
            r.Height -= 1;

            //Scale the radius if it's too large to fit.
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


        #endregion

        private CustomButtonState currentState;
        private void OnStateChange(EventArgs e)
        {
            //Repaint the button only if the state has actually changed
            if (this.ButtonState == currentState)
                return;
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
