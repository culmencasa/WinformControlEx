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
    /// 自定义窗体(不支持圆角)
    /// </summary>
    public partial class CustomForm : Form, IDpiDefined
    {
        #region 事件

        public delegate void LoadCompletedEventHandler();
        public event LoadCompletedEventHandler LoadCompleted;

        #endregion

        #region 字段

        private bool _showTitle;
        private string _customTitleText;
        private int _borderSize = 0;
        private Color _backColor = Color.Gray;
        private Color _borderColor;

        #endregion

        #region 属性


        /// <summary>
        /// 窗体边框大小. 
        /// 如果窗体中的控件有Dock或者位置与边框重合，将遮挡边框。 
        /// </summary>
        [Category("Custom")]        
        public int BorderSize
        {
            get
            {
                return _borderSize;
            }
            set
            {
                if (value < 0 || value > 4)
                {
                    return;
                }
                _borderSize = value;
                
                Invalidate();
            }
        }

        /// <summary>
        /// 窗体边框颜色
        /// </summary>
        [Category("Custom")]
        [DefaultValue(typeof(Color), "157,157,157")]
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }
        /// <summary>
        /// 背景渐变色1
        /// </summary>
        [Category("Custom")]
        public Color BackGradientLightColor { get; set; }
        /// <summary>
        /// 背景渐变色2
        /// </summary>   
        [Category("Custom")]
        public Color BackGradientDarkColor { get; set; }


        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        [Category("Custom")]
        [DefaultValue(true)]
        public bool AllowResize { get; set; }

        /// <summary>
        /// 允许窗体移动
        /// </summary>
        [Category("Custom")]
        public bool AllowMove { get; set; }


        /// <summary>
        /// 标题栏高度
        /// </summary>
        [Category("Custom")]
        public int TitleBarHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 标题栏文字
        /// </summary>
        [Category("Custom")]
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


        [Category("Custom")]
        [Description("用于绘制窗体标题的颜色")]
        public Color TitleForeColor
        {
            get;
            set;
        }

        [Category("Custom")]
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
        [Category("Custom")]
        public bool ShowLogo { get; set; }

        /// <summary>
        /// 标题居中显示
        /// </summary>
        [Category("Custom")]
        public bool ShowTitleCenter { get; set; }

        [Category("Custom")]
        public Font TitleFont
        {
            get;
            set;
        }
        [Category("Custom")]
        public int LogoSize { get; set; }
        [Category("Custom")]
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
                clientArea.X = this.BorderSize;
                clientArea.Y = this.BorderSize + TitleBarHeight;
                clientArea.Width = this.Width - BorderSize * 2;
                clientArea.Height = this.Height - BorderSize * 2 - TitleBarHeight;

                return clientArea;
            }
        }



        [DefaultValue(typeof(Color), "Gray")]
        public override Color BackColor
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

        #endregion

        #region 构造

        /// <summary>
        /// 
        /// </summary>
        public CustomForm()
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;
            this.FormBorderStyle = FormBorderStyle.None;

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();


            InitializeDefaultValues();

            TitleFont = new Font(this.Font.FontFamily.Name, 16f, FontStyle.Bold);
            Load += CustomForm_Load;
            Shown += CustomForm_Shown;

            // 防止覆盖任务栏. MaximizedBounds需要在窗体Maximize之前设置才生效
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            MaximizedBounds = new Rectangle(workingArea.Left, workingArea.Top, workingArea.Width, workingArea.Height);

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

                    if (DontWaitChildrenDrawBackground)
                        cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;  //防止因窗体控件太多出现闪烁

                    cp.ClassStyle |= 0x20000;  //窗体边框阴影
                }
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32.WM_NCHITTEST)
            {
                bool dealt = false;
                // 点击任意位置等于点击标题栏, 需要代码处理支持AeroSnap
                if (WindowState == FormWindowState.Normal)
                {
                    if (AllowResize)
                    {
                        dealt = HandleResizeMessage(ref m);
                        if (dealt)
                        {
                            return;
                        }
                    }

                    if (AllowMove)
                    {
                        m.Result = new IntPtr(Win32.HTCAPTION);
                        return;
                    }
                }
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

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //	if (WindowState == FormWindowState.Normal)
        //	{
        //		Graphics g = e.Graphics;
        //		SmoothingMode smooth = g.SmoothingMode;
        //		if (this.BackgroundImage == null)
        //		{
        //			DrawGradientBackground(e.Graphics);
        //		}
        //		else
        //		{
        //			base.OnPaintBackground(e);
        //		}
        //		g.SmoothingMode = smooth;
        //	}
        //	else if (WindowState == FormWindowState.Maximized)
        //	{
        //		base.OnPaintBackground(e);
        //	}
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SmoothingMode smooth = g.SmoothingMode;

            g.FillRectangle(Brushes.WhiteSmoke, DisplayRectangle);


            DrawTitleBackground(g);


            if (this.WindowState == FormWindowState.Normal)
            {
                if (BorderSize > 0)
                {
                    // 边框
                    using (Pen borderPen = new Pen(this.BorderColor, BorderSize))
                    {
                        if (BorderSize % 2 == 0)
                        {
                            g.DrawRectangle(borderPen, 0, 0, Width, Height);
                        }
                        else
                        {
                            g.DrawRectangle(borderPen, 0, 0,
                                Width - (int)Math.Round(BorderSize / 2.0, 0, MidpointRounding.AwayFromZero),
                                Height - (int)Math.Round(BorderSize / 2.0, 0, MidpointRounding.AwayFromZero));
                        }
                    }
                }
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

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        #endregion

        #region 公开的方法

        #endregion

        #region 可重写的方法

        protected virtual void InitializeDefaultValues()
        {
            // 初始值
            this.BackColor = Color.Gray;

            this.AllowResize = false;
            this.AllowMove = true;
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
        protected virtual void DrawLogoAndTitle(Graphics g)
        {
            int startX = 0;
            //int top = 0;
            int fullWidth = 0;
            int fullHeight = 0;
            int padding = 6;
            int space = 6;

            // 字体大小
            SizeF textSize = new SizeF(1, 1);
            if (this.TitleText.Length > 0)
            {
                textSize = TextRenderer.MeasureText(this.TitleText, this.TitleFont);
            }

            // 整体标题区域大小
            fullWidth = (ShowLogo ? LogoSize : 0) + (ShowTitleText ? (int)textSize.Width : 0) + space;
            fullHeight = Math.Max((int)textSize.Height, LogoSize);

            // 图片位置
            if (ShowTitleCenter)
            {
                startX = (this.Width - fullWidth) / 2;
            }
            else
            {
                startX = Math.Max(Padding.Left, padding);
            }

            // 画图标
            if (ShowLogo && Logo != null && Logo.Width > 0 && Logo.Height > 0)
            {
                int imageX = startX;
                int imageY = (this.TitleBarHeight - LogoSize) / 2;
                g.DrawImage(Logo, imageX, imageY, LogoSize, LogoSize);
            }

            // 画文字
            if (ShowTitleText && !string.IsNullOrEmpty(TitleText))
            {
                int imagePlaceholderWidth = (ShowLogo ? LogoSize : 0);
                int textX = startX + imagePlaceholderWidth + space;
                int textY = (this.TitleBarHeight - (int)textSize.Height) / 2;
                int textWidth = fullWidth - imagePlaceholderWidth;
                int textHeight = TitleBarHeight;
                Rectangle textArea = new Rectangle(
                        textX,
                        textY,
                        textWidth,
                        textHeight);

                TextRenderer.DrawText(
                    g,
                    TitleText,
                    TitleFont,
                    textArea,
                    TitleForeColor,
                    TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
            }
        }


        protected virtual void DrawGradientBackground(Graphics g)
        {
            g.Clear(BackColor);

            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(this.Width / 2, 0),
                new Point(this.Width / 2, this.Height),
                this.BackGradientLightColor,
                this.BackGradientDarkColor
                ))
            {
                g.FillRectangle(brush, 0, 0, this.Width, this.Height);
            }

        }


        protected virtual void DrawTitleBackground(Graphics g)
        {

        }



        #endregion

        #region 私有方法

        /// <summary>
        /// 判断操作的是窗体边缘, 则调整窗体大小
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool HandleResizeMessage(ref Message m)
        {

            int wparam = m.LParam.ToInt32();
            Point mouseLocation = new Point(RenderHelper.LOWORD(wparam), RenderHelper.HIWORD(wparam));
            mouseLocation = PointToClient(mouseLocation);


            int range = 15;

            if (mouseLocation.X < range && mouseLocation.Y < range)
            {
                m.Result = new IntPtr(Win32.HTTOPLEFT);
                return true;
            }

            if (mouseLocation.X > Width - range && mouseLocation.Y < range)
            {
                m.Result = new IntPtr(Win32.HTTOPRIGHT);
                return true;
            }

            if (mouseLocation.X < range && mouseLocation.Y > Height - range)
            {
                m.Result = new IntPtr(Win32.HTBOTTOMLEFT);
                return true;
            }

            if (mouseLocation.X > Width - range && mouseLocation.Y > Height - range)
            {
                m.Result = new IntPtr(Win32.HTBOTTOMRIGHT);
                return true;
            }

            if (mouseLocation.Y < range)
            {
                m.Result = new IntPtr(Win32.HTTOP);
                return true;
            }

            if (mouseLocation.Y > Height - range)
            {
                m.Result = new IntPtr(Win32.HTBOTTOM);
                return true;
            }

            if (mouseLocation.X < range * 2)
            {
                m.Result = new IntPtr(Win32.HTLEFT);
                return true;
            }

            if (mouseLocation.X > Width - range * 2)
            {
                m.Result = new IntPtr(Win32.HTRIGHT);
                return true;
            }

            return false;
        }


        #endregion

        #region 事件处理

        // 窗体加载
        private void CustomForm_Load(object sender, EventArgs e)
        {
            using (Graphics graphics = this.CreateGraphics())
            {
                RuntimeScaleFactorX = graphics.DpiX / 96f;
                RuntimeScaleFactorY = graphics.DpiY / 96f;
            }
       }

        private void CustomForm_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (LoadCompleted != null)
                LoadCompleted();
        }


        #endregion

        public float DesigntimeScaleFactorX { get; set; }
        public float DesigntimeScaleFactorY { get; set; }
        public float RuntimeScaleFactorX { get; set; }
        public float RuntimeScaleFactorY { get; set; }

        public float ScaleFactorRatioX
        {
            get
            {
                return RuntimeScaleFactorX / DesigntimeScaleFactorX;
            }
        }
        public float ScaleFactorRatioY
        {
            get
            {
                return RuntimeScaleFactorY / DesigntimeScaleFactorY;
            }
        }



    }


}
