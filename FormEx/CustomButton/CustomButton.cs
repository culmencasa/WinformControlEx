using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Security.Permissions;
using System.Drawing.Drawing2D;


// Mick Dohertys' .net Tips and Tricks 源代码见 http://dotnetrix.co.uk/button.htm
namespace System.Windows.Forms
{

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
        }

        #endregion

        #region Private Instance Variables

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

        #endregion

        #region IButtonControl Implementation

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

        #region Properties


        /// <summary>
        /// 按钮状态
        /// </summary>
        [Browsable(false)]
        public CustomButtonState ButtonState
        {
            get { return m_ButtonState; }
        }


        /// <summary>
        /// 圆角
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(14)]
        [Description("Defines the radius of the controls RoundedCorners.")]
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


        //DefaultSize
        protected override System.Drawing.Size DefaultSize
        {
            get { return new Size(75, 23); }
        }


        //IsDefault
        [Browsable(false)]
        public bool IsDefault
        {
            get { return m_IsDefault; }
        }

        [Category("Appearance")]
        [Description("背景色是否显渐变.")]
        public bool GradientMode
        {
            get;
            set;
        }

        [Category("Appearance")]
        [Description("背景色是否显示阴影模式.")]
        public bool ShadeMode
        {
            get;
            set;
        }


        //ImageList
        [Category("Appearance"), DefaultValue(typeof(ImageList), null)]
        [Description("The image list to get the image to display in the face of the control.")]
        public ImageList ImageList
        {
            get { return m_ImageList; }
            set
            {
                m_ImageList = value;
                this.Invalidate();
            }
        }


        //ImageIndex
        [Category("Appearance"), DefaultValue(-1)]
        [Description("The index of the image in the image list to display in the face of the control.")]
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


        //ImageAlign
        [Category("Appearance"), DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("The alignment of the image that will be displayed in the face of the control.")]
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


        //RoundCorners
        [Category("Appearance")]
        [DefaultValue(typeof(Corners), "None")]
        [Description("Gets/sets the corners of the control to round.")]
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


        //TextAlign
        [Category("Appearance"), DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("The alignment of the text that will be displayed in the face of the control.")]
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

        public Color BorderColor { get; set; }

        #endregion

        #region Overriden Methods

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
            GraphicsContainer g = pevent.Graphics.BeginContainer();
            Rectangle translateRect = this.Bounds;
            pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
            PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, translateRect);
            this.InvokePaintBackground(this.Parent, pe);
            this.InvokePaint(this.Parent, pe);
            pevent.Graphics.ResetTransform();
            pevent.Graphics.EndContainer(g);

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

            Rectangle r = this.ClientRectangle;
            GraphicsPath path = RoundRectangle(r, this.CornerRadius, this.RoundCorners);

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

        #region Internal Draw Methods

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


        private void DrawText(Graphics g)
        {
            SolidBrush TextBrush = new SolidBrush(this.ForeColor);

            // 因字体不同可能会发生偏移
            RectangleF R = new RectangleF(contentRect.X, contentRect.Y, contentRect.Width, contentRect.Height);

            if (!this.Enabled)
                TextBrush.Color = SystemColors.GrayText;

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

            if (this.ButtonState == CustomButtonState.Pressed)
                R.Offset(1, 1);

            if (this.Enabled)
                g.DrawString(this.Text, this.Font, TextBrush, R, sf);
            else
                ControlPaint.DrawStringDisabled(g, this.Text, this.Font, this.BackColor, R, sf);

        }


        private void DrawFocus(Graphics g)
        {
            Rectangle r = contentRect;
            r.Inflate(1, 1);
            if (this.Focused && this.ShowFocusCues && this.TabStop)
                ControlPaint.DrawFocusRectangle(g, r, this.ForeColor, this.BackColor);
        }


        #endregion

        #region Helper Methods

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


        //The ControlPaint Class has methods to Lighten and Darken Colors, but they return a Solid Color.
        //The Following 2 methods return a modified color with original Alpha.
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
            //This method returns White if you lighten by 100%

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

    #region Custom TypeEditor for RoundCorners property

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
