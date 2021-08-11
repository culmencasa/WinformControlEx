using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{

    /// <summary>
    /// 圆角窗体
    /// </summary>
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
        private int _borderSize = 2;

        #endregion

        #region 属性


        /// <summary>
        /// 窗体边框大小. 
        /// 如果窗体中的控件有Dock或者位置与边框重合，将遮挡边框。 
        /// </summary>
        public int BorderSize
        {
            get
            {
                return _borderSize;
            }
            set
            {
                _borderSize = value;
                Update();
            }
        }

        /// <summary>
        /// 圆角直径
        /// </summary>
        public int RoundCornerDiameter
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
        /// 窗体边框颜色
        /// </summary>
        [DefaultValue(typeof(Color), "157,157,157")]
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

        /// <summary>
        /// 显示阴影分割
        /// </summary>
        [DefaultValue(true)]
        public bool ShowShade { get; set; }

        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        [DefaultValue(true)]
        public bool AllowResize { get; set; }

        /// <summary>
        /// 允许窗体移动
        /// </summary>
        public bool AllowMove { get; set; }


        /// <summary>
        /// 显示窗体阴影
        /// </summary>
        [DefaultValue(true)]
        public bool ShowFormShadow { get; set; }

        /// <summary>
        /// 标题栏高度
        /// </summary>
        public int TitleBarHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 标题栏文字
        /// </summary>
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

        /// <summary>
        /// 显示Logo图标
        /// </summary>
        public bool ShowLogo { get; set; }

        /// <summary>
        /// 标题居中显示
        /// </summary>
        public bool ShowTitleCenter { get; set; }

        public int LogoSize { get; set; }
        public Image Logo { get; set; }



        public bool DontWaitChildrenDrawBackground { get; set; }


        /// <summary>
        /// 貌似仅能限制Dock控件的区域
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle clientArea = new Rectangle();
                clientArea.X = this.BorderSize + 1;
                clientArea.Y = this.BorderSize + TitleBarHeight + 1;
                clientArea.Width = this.Width - BorderSize * 2 - 4;
                clientArea.Height = this.Height - BorderSize * 2 - TitleBarHeight - 4;

                return clientArea;
            }
        }


        #endregion

        #region 构造

        /// <summary>
        /// 
        /// </summary>
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


            InitializeDefaultValues();

            // 2021-08-02 去掉最大化等按钮，与边框及Z-Index之间的关系无法控制，换成独立的Win11ControlBox控件。
        }


        #endregion

        #region 重写基类的成员

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

                    /* 引用： 
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

                    //cp.ClassStyle |= 0x20000;  //窗体边框阴影
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
            using (Pen borderPen = new Pen(this.BorderColor, BorderSize))
            {
                g.DrawRoundedRectangle(borderPen, 0, 0, this.Width - BorderSize, this.Height - BorderSize, radius);

            }

            // 内边框
            //Color innerBorder = Color.FromArgb(100, 157, 157, 157);
            //using (Pen borderPen = new Pen(innerBorder, 2))
            //{
            //    g.DrawRoundedRectangle(borderPen, 1, 1, this.Width - 5, this.Height - 5, radius);
            //}


            // 画标题
            DrawLogoAndTitle(g);

            g.SmoothingMode = smooth;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // 注：不能写到Resize事件里
            if (IsHandleCreated && !DesignMode)
            {
                UpdateFormRoundCorner(this.RoundCornerDiameter);
                UpdateEdgePatching();
            }
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            UpdateEdgePatching();
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
                left = (this.Width - width) / 2;
            }

            // 画图标
            if (ShowLogo && Logo != null)
            {
                top = Padding.Top + (this.TitleBarHeight - LogoSize) / 2;
                g.DrawImage(Logo, left, top, LogoSize, LogoSize);
            }

            // 画文字
            if (ShowTitleText && !string.IsNullOrEmpty(TitleText))
            {
                top = Padding.Top + (this.TitleBarHeight - (int)textSize.Height) / 2;
                TextRenderer.DrawText(
                    g,
                    this.TitleText,
                    this.Font,
                    new Rectangle(left + (ShowLogo ? LogoSize : 0), top - 1, width + (ShowLogo ? LogoSize : 0), height),
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

        protected void UpdateFormRoundCorner(int diameter)
        {

            // 防止控件撑出窗体            
            IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, Width, Height, diameter / 2 + 4, diameter / 2 + 4);
            Region = System.Drawing.Region.FromHrgn(hrgn);
            this.Update();
            Win32.DeleteObject(hrgn);
        }

        /// <summary>
        /// 设置子控件的区域，避免与边框重叠
        /// </summary>
        protected void UpdateEdgePatching()
        {

            //var list = GetEdgePatchingControls();

            //if (list != null)
            //{
            //    foreach (var control in list)
            //    {
            //        /// 可显示的区域为窗体大小减去控件起始位置，再减去边框大小
            //        int width = this.Width - control.Left - BorderSize;
            //        int height = this.Height - control.Top - BorderSize;

            //        int left =  control.Left <= BorderSize ? BorderSize + 1 : control.Left;
            //        int top = control.Top <= BorderSize ? BorderSize + 1 : control.Top;


            //        IntPtr handle = Win32.CreateRoundRectRgn(left, top, width, height, RoundCornerDiameter, RoundCornerDiameter);
            //        control.Region = Region.FromHrgn(handle);
            //        Win32.DeleteObject(handle);
            //    }
            //}

        }


        #endregion

        #region 事件处理

        // 窗体加载
        private void RoundedCornerForm_Load(object sender, EventArgs e)
        {

            if (!DesignMode && ShowFormShadow)
            {
                // 如果窗体有DPI感应的话，DropShadow代码还需修改
                DropShadow _shadow = new DropShadow(this);
                _shadow.BorderRadius = this.RoundCornerDiameter / 2;
                _shadow.ShadowRadius = _shadow.BorderRadius;
                _shadow.Refresh();
            }
        }


        #endregion

    }


}
