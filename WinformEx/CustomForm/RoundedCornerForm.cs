using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public partial class RoundedCornerForm : Form
    {
        public class ColorSchema
        {
            public Color BackColor { get; set; }
            public Color BorderColor { get; set; }
            public Color BackGradientLightColor { get; set; }
            public Color BackGradientDarkColor { get; set; }
            public Color ShadeColor { get; set; }

            private static ColorSchema systemSchema = null;
            private static ColorSchema defaultSchema = null;
            private static ColorSchema graySchema = null;

            public static ColorSchema System
            {
                get
                {
                    if (systemSchema == null)
                    {
                        systemSchema = new ColorSchema();

                        systemSchema.BackColor = ControlPaint.Dark(SystemColors.GradientActiveCaption, 0.2f);
                        systemSchema.BorderColor = SystemColors.InactiveBorder;
                        systemSchema.BackGradientLightColor = SystemColors.GradientInactiveCaption;
                        systemSchema.BackGradientDarkColor = ControlPaint.Dark(SystemColors.GradientActiveCaption, 0.5f);
                        systemSchema.ShadeColor = Color.FromArgb(100, 70, 130, 180);
                    }

                    return systemSchema;
                }
            }

            public static ColorSchema Default
            {
                get
                {
                    if (defaultSchema == null)
                    {
                        defaultSchema = new ColorSchema();

                        defaultSchema.BackColor = Color.FromArgb(255, 244, 244, 244);
                        defaultSchema.BackGradientLightColor = Color.FromArgb(244, 244, 244);
                        defaultSchema.BackGradientDarkColor = Color.FromArgb(73, 73, 73);
                        defaultSchema.BorderColor = Color.FromArgb(100, 100, 100);
                        defaultSchema.ShadeColor = ControlPaint.Light(defaultSchema.BackGradientDarkColor, 0.7f);
                    }

                    return defaultSchema;
                }
            }

            public static ColorSchema Gray
            {
                get
                {
                    if (graySchema == null)
                    {
                        graySchema = new ColorSchema();

                        graySchema.BackColor = Color.FromArgb(255, 217, 217, 217);
                        graySchema.BackGradientLightColor = Color.FromArgb(217, 217, 217);
                        graySchema.BackGradientDarkColor = Color.FromArgb(217, 217, 217);
                        graySchema.BorderColor = Color.FromArgb(0, 0, 0);
                        //graySchema.ShadeColor = ControlPaint.Light(graySchema.BackGradientDarkColor, 0.7f);
                    }

                    return graySchema;
                }
            }


            public static ColorSchema White
            {
                get
                {
                    if (graySchema == null)
                    {
                        graySchema = new ColorSchema();

                        graySchema.BackColor = Color.FromArgb(255, 244, 244, 244);
                        graySchema.BackGradientLightColor = Color.FromArgb(255, 244, 244, 244);
                        graySchema.BackGradientDarkColor = Color.FromArgb(255, 244, 244, 244);
                        graySchema.BorderColor = Color.FromArgb(200, 200, 200);
                        //graySchema.ShadeColor = ControlPaint.Light(graySchema.BackGradientDarkColor, 0.7f);
                    }

                    return graySchema;
                }
            }
        }

        #region 事件


        #endregion

        #region 字段

        private int _roundCornerDiameter;
        private bool _showTitle;
        private string _customTitleText;

        #endregion

        #region 属性

        /// <summary>
        /// 圆角直径
        /// </summary>
        public Int32 RoundCornerDiameter
        {
            get
            {
                return _roundCornerDiameter;
            }
            set
            {
                _roundCornerDiameter = value;
                if (IsHandleCreated && !DesignMode)
                    UpdateFormRoundCorner(value);
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor { get; set; }
        /// <summary>
        /// 背景渐变色1
        /// </summary>
        public Color BackGradientLightColor { get; set; }
        /// <summary>
        /// 背景渐变色2
        /// </summary>        
        public Color BackGradientDarkColor { get; set; }
        /// <summary>
        /// 阴影面颜色
        /// </summary>
        public Color ShadeColor
        {
            get;
            set;

        }

        [DefaultValue(true)]
        public bool ShowShade { get; set; }

        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        [DefaultValue(true)]
        public bool AllowResize { get; set; }

        public bool AllowMove { get; set; }


        [DefaultValue(true)]
        public bool ShowFormShadow { get; set; }

        public int TitleBarHeight
        {
            get
            {
                return flpControlBox.Height;
            }
            set
            {
                flpControlBox.Height = value;
            }
        }

        public string TitleText
        {
            get
            {
                return _customTitleText;
            }
            set
            {
                _customTitleText = value;
                this.Text = value;
            }
        }


        [Description("用于绘制窗体标题的颜色")]
        public Color TitleForeColor
        {
            get;
            set;
        }

        public bool ShowTitleText
        {
            get
            {
                return _showTitle;
            }
            set
            {
                bool hasChanged = _showTitle != value;
                _showTitle = value;
                if (hasChanged)
                {
                    Invalidate();
                }
            }
        }

        public bool ShowLogo { get; set; }
        public bool ShowTitleCenter { get; set; }

        public int LogoSize { get; set; }
        public Image Logo { get; set; }

        public Image CloseButtonNormalImage { get; set; }
        public Image CloseButtonDownImage { get; set; }
        public Image CloseButtonHoverImage { get; set; }


        public Image MinButtonNormalImage { get; set; }
        public Image MinButtonDownImage { get; set; }
        public Image MinButtonHoverImage { get; set; }

        public bool DontWaitChildrenDrawBackground { get; set; }

        #endregion

        #region 构造

        public RoundedCornerForm()
        {
            InitializeComponent();

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            //this.flpControlBox.AutoSize = true;
            //this.flpControlBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            InitializeDefaultValues();
        }

        #endregion

        #region 重写的成员

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    if (MaximizeBox) 
                    { 
                        cp.Style |= (int)WindowStyle.WS_MAXIMIZEBOX;
                    }
                    if (MinimizeBox) 
                    { 
                        cp.Style |= (int)WindowStyle.WS_MINIMIZEBOX;
                    }

                    #region 用户控件过多显示白块的问题

                    /*
                        It is not the kind of flicker that double-buffering can solve. Nor BeginUpdate or SuspendLayout. You've got too many controls, the BackgroundImage can make it a lot worse.

                        It starts when the UserControl paints itself. It draws the BackgroundImage, leaving holes where the child control windows go. Each child control then gets a message to paint itself, they'll fill in the hole with their window content. When you have a lot of controls, those holes are visible to the user for a while. They are normally white, contrasting badly with the BackgroundImage when it is dark. Or they can be black if the form has its Opacity or TransparencyKey property set, contrasting badly with just about anything.

                        This is a pretty fundamental limitation of Windows Forms, it is stuck with the way Windows renders windows. Fixed by WPF btw, it doesn't use windows for child controls. What you'd want is double-buffering the entire form, including the child controls. That's possible, check my code in this thread for the solution. It has side-effects though, and doesn't actually increase painting speed. The code is simple, paste this in your form (not the user control):

                        protected override CreateParams CreateParams {
                          get {
                            CreateParams cp = base.CreateParams;
                            cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                            return cp;
                          }
                        } 

                        There are many things you can do to improve painting speed, to the point that the flicker isn't noticeable anymore. Start by tackling the BackgroundImage. They can be really expensive when the source image is large and needs to be shrunk to fit the control. Change the BackgroundImageLayout property to "Tile". If that gives a noticeable speed-up, go back to your painting program and resize the image to be a better match with the typical control size. Or write code in the UC's OnResize() method to create a properly sized copy of the image so that it doesn't have to be resized every time the control repaints. Use the Format32bppPArgb pixel format for that copy, it renders about 10 times faster than any other pixel format.

                        Next thing you can do is prevent the holes from being so noticeable and contrasting badly with the image. You can turn off the WS_CLIPCHILDREN style flag for the UC, the flag that prevents the UC from painting in the area where the child controls go. Paste this code in the UserControl's code:

                        protected override CreateParams CreateParams {
                          get {
                            var parms = base.CreateParams;
                            parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                            return parms;
                          }
                        }

                        The child controls will now paint themselves on top of the background image. You might still see them painting themselves one by one, but the ugly intermediate white or black hole won't be visible.

                        Last but not least, reducing the number of child controls is always a good approach to solve slow painting problems. Override the UC's OnPaint() event and draw what is now shown in a child. Particular Label and PictureBox are very wasteful. Convenient for point and click but their light-weight alternative (drawing a string or an image) takes only a single line of code in your OnPaint() method.
                    */
                    #endregion

                    if (DontWaitChildrenDrawBackground)
                        cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;  //防止因窗体控件太多出现闪烁

                    //cp.ClassStyle |= 0x20000;  //实现窗体边框阴影效果
                }
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32.WM_NCHITTEST)
            {
                if (AllowMove)
                {
                    m.Result = new IntPtr(Win32.HTCAPTION);
                    return;
                }

                if (AllowResize)
                {
                    WmNcHitTest(ref m);
                    return;
                } // 其他区域, 将操作区域视为标题栏
                
            }

            if (m.Msg == 0xa3)
            {
                return;
            }


            if (m.Msg == Win32.WM_SYSCOMMAND)
            {
                int state = m.WParam.ToInt32();
                /* 双击任务栏放大的情况, 参见 
                 * https://msdn.microsoft.com/en-us/library/windows/desktop/ms646360%28v=vs.85%29.aspx
                 */
                if ((state & 0xFFF0) == Win32.SC_MAXMIZE) // 最大化 
                {
                }
                else if ((state & 0xFFF0) == Win32.SC_MINIMIZE)
                {
                }
                else if ((state & 0xFFF0) == Win32.SC_RESTORE)
                {

                }
                else
                {
                }
            }

            base.WndProc(ref m);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            SmoothingMode smooth = g.SmoothingMode;
            if (this.BackgroundImage == null)
            {
                DrawGradientBackground(e.Graphics);
            }
            g.SmoothingMode = smooth;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SmoothingMode smooth = g.SmoothingMode;
            base.OnPaint(e);

            g.SmoothingMode = SmoothingMode.HighSpeed;


            DrawTitleBackground(g);

            // 半径
            int radius = RoundCornerDiameter / 2;
            // 边框
            using (Pen borderPen = new Pen(this.BorderColor))
            {
                g.DrawRoundedRectangle(borderPen, 0, 0, this.Width - 2, this.Height - 2, radius);

            }

            // 内边框
            Color innerBorder = Color.FromArgb(100, 244, 244, 244);
            using (Pen borderPen = new Pen(innerBorder))
            {
                g.DrawRoundedRectangle(borderPen, 1, 1, this.Width - 4, this.Height -4, radius);
            }


            // 画标题
            DrawLogoAndTitle(g);

            g.SmoothingMode = smooth;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // 注：不能写到Resize事件里
            if (IsHandleCreated && !DesignMode)
                UpdateFormRoundCorner(this.RoundCornerDiameter);
        }

        protected override void OnStyleChanged(EventArgs e)
        {
            if (!ControlBox)
            {
                btnClose.Visible = false;
                btnMaximum.Visible = false;
                btnMinimum.Visible = false;
            }
            else
            {
                btnMaximum.Visible = MaximizeBox;
                btnMinimum.Visible = MinimizeBox;
                btnClose.Visible = true;
            }

            base.OnStyleChanged(e);

        }
        #endregion


        #region 公开的方法
        
        public void SetColorSchema(ColorSchema schema)
        {
            this.BackColor = schema.BackColor;
            this.BorderColor = schema.BorderColor;
            this.BackGradientLightColor = schema.BackGradientLightColor;
            this.BackGradientDarkColor = schema.BackGradientDarkColor;
            this.ShadeColor = schema.ShadeColor;
            this.Invalidate();
        }

        #endregion

        #region 可重写的方法

        protected virtual void InitializeDefaultValues()
        {
            //this.ShowFormShadow = true;

            // 初始值
            this.SetColorSchema(ColorSchema.White);

            this.RoundCornerDiameter = 16;
            this.AllowResize = false;
            this.AllowMove = true;
            this.ShowShade = false;
            //this.TransparencyKey = Color.Fuchsia;

            this.TitleText = "";
            this.TitleForeColor = Color.Black;
            this.ShowTitleCenter = false;
            this.ShowTitleText = true;
            this.ShowLogo = true;
            this.LogoSize = 32;
        }

        /// <summary>
        /// 画标题
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle DrawLogoAndTitle(Graphics g)
        {

            int left = 6;
            int top = 0;
            int width = 0;
            int height = 0;
            int padding = 6;

            // 字体大小
            SizeF textSize = new SizeF();
            if (this.TitleText.Length > 0)
                textSize = TextRenderer.MeasureText(this.TitleText, this.Font);

            // 整体标题区域大小
            width = (ShowLogo ? LogoSize : 0) + (ShowTitleText ? (int)textSize.Width : 0) + padding;
            height = (int)textSize.Height > LogoSize ? (int)textSize.Height : LogoSize;

            // 图片位置
            if (ShowTitleCenter)
            {
                left = (this.flpControlBox.Width - width) / 2;
            }

            // 画图标
            if (ShowLogo && Logo != null)
            {
                top = Padding.Top + (this.flpControlBox.Height - LogoSize) / 2;
                g.DrawImage(Logo, left, top, LogoSize, LogoSize);
            }

            // 画文字
            if (ShowTitleText && !string.IsNullOrEmpty(TitleText))
            {
                top = Padding.Top + (this.flpControlBox.Height - (int)textSize.Height) / 2;
                TextRenderer.DrawText(
                    g,
                    this.TitleText,
                    this.Font,
                    new Rectangle((ShowLogo ? LogoSize : 0), top - 1, width + (ShowLogo ? LogoSize : 0), height),
                    TitleForeColor,
                    TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
            }


            return new Rectangle(left, top, width, height);
        }


        protected virtual void DrawGradientBackground(Graphics g)
        {
            int radius = RoundCornerDiameter / 2;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(this.Width / 2, 0),
                new Point(this.Width / 2, this.Height),
                this.BackGradientLightColor,
                this.BackGradientDarkColor
                ))
            {
                g.FillRoundedRectangle(brush, 0, 0, this.Width, this.Height, radius);
            }


            if (ShowShade)
            {
                Rectangle shadeArea = new Rectangle(1,
                        this.Height / 2,
                        this.Width - 4,
                        this.Height / 2);
                using (LinearGradientBrush brush = new LinearGradientBrush(shadeArea, this.ShadeColor, ControlPaint.LightLight(this.ShadeColor), 90f))
                {
                    g.FillRoundedRectangle(
                        brush,
                        shadeArea,
                        radius);

                    // 填补顶部的边线(出现的原因未知)
                    using (Pen earser = new Pen(this.ShadeColor))
                    {
                        g.DrawLine(earser, shadeArea.X + radius, shadeArea.Y, shadeArea.Width - radius, shadeArea.Y);
                    }
                }
            }
        }


        protected virtual void DrawTitleBackground(Graphics g)
        { 
        
        }

        #endregion

        #region 私有方法

        private void WmNcHitTest(ref Message m)
        {
            #region 判断操作的是窗体边缘, 则调整窗体大小
            if (WindowState == FormWindowState.Normal)
            {
                int wparam = m.LParam.ToInt32();
                Point mouseLocation = new Point(RenderHelper.LOWORD(wparam), RenderHelper.HIWORD(wparam));
                mouseLocation = PointToClient(mouseLocation);

                if (mouseLocation.X < 5 && mouseLocation.Y < 5)
                {
                    m.Result = new IntPtr(Win32.HTTOPLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 5 && mouseLocation.Y < 5)
                {
                    m.Result = new IntPtr(Win32.HTTOPRIGHT);
                    return;
                }

                if (mouseLocation.X < 5 && mouseLocation.Y > Height - 5)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOMLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 5 && mouseLocation.Y > Height - 5)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOMRIGHT);
                    return;
                }

                if (mouseLocation.Y < 3)
                {
                    m.Result = new IntPtr(Win32.HTTOP);
                    return;
                }

                if (mouseLocation.Y > Height - 3)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOM);
                    return;
                }

                if (mouseLocation.X < 3)
                {
                    m.Result = new IntPtr(Win32.HTLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 3)
                {
                    m.Result = new IntPtr(Win32.HTRIGHT);
                    return;
                }
                
            }

            #endregion

            m.Result = new IntPtr(Win32.HTCLIENT);
        }

        private void UpdateFormRoundCorner(int diameter)
        {
            IntPtr hrgn= Win32.CreateRoundRectRgn(0, 0, Width, Height, diameter, diameter);
            Region = System.Drawing.Region.FromHrgn(hrgn);
            this.Update();
        }

        #endregion

        #region 事件处理

        // 点击最小化按钮 
        private void btnMinimum_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // 点击最大化/还原按钮
        private void btnMaximum_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.btnMaximum.NormalImage = Properties.Resources.btnMaximum_NormalImage;
                this.btnMaximum.HoverImage = Properties.Resources.btnMaximum_HoverImage;
                this.btnMaximum.DownImage = Properties.Resources.btnMaximum_DownImage;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.btnMaximum.NormalImage = Properties.Resources.restore_normal;
                this.btnMaximum.HoverImage = Properties.Resources.restore_highlight;
                this.btnMaximum.DownImage = Properties.Resources.restore_down;
            }

        }

        // 点击关闭按钮
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 窗体加载
        private void RoundedCornerForm_Load(object sender, EventArgs e)
        {

            if (!DesignMode && ShowFormShadow)
            {
                // 注：圆角设置需要在窗体显示时完成
                //UpdateFormRoundCorner(this.RoundCornerDiameter);

                DropShadow _shadow = new DropShadow(this);
                _shadow.BorderRadius = this.RoundCornerDiameter / 2;
                _shadow.ShadowRadius = _shadow.BorderRadius;
                _shadow.Refresh();
            }

        }

        #endregion


    }
}
