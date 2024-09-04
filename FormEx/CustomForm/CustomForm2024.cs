using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class CustomForm2024 : Form, IDpiDefined
    {
        public CustomForm2024()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            UpdateStyles();



            // 设置窗体事件 
            Resize += CustomForm_Resize;
            MouseDown += CustomForm_MouseDown;


            // 防止覆盖任务栏. MaximizedBounds需要在窗体Maximize之前设置才生效
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            MaximizedBounds = new Rectangle(workingArea.Left, workingArea.Top, workingArea.Width, workingArea.Height);

            // Load事件会在子控件加载后触发, 所以DPI计算不能放在Load事件中
#if COMPILE_NET40 || COMPILE_NET46 || COMPILE_NET48
            using (Graphics graphics = this.CreateGraphics())
            {
                RuntimeScaleFactorX = graphics.DpiX / 96f;
                RuntimeScaleFactorY = graphics.DpiY / 96f;
            }
#else
            RuntimeScaleFactorX = DeviceDpi / 96F;
            RuntimeScaleFactorY = DeviceDpi / 96F;
#endif


            TitleFont = new Font(familyName:this.Font.FontFamily.Name, 16f * ScaleFactorRatioY,  style: Drawing.FontStyle.Bold);
        }
         

        #region 字段

        private Color _borderColor;
        private string _customTitleText;
        private int _borderSize = 2;

        #endregion

        #region 设计器属性

        /// <summary>
        /// 窗体边框颜色
        /// </summary>
        [Category(Consts.DefaultCategory)]
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
        /// 标题栏文字
        /// </summary>
        [Category(Consts.DefaultCategory)]
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

        [Category(Consts.DefaultCategory)]
        [Description("用于绘制窗体标题的颜色")]
        public Color TitleForeColor
        {
            get;
            set;
        }
        /// <summary>
        /// 窗体边框大小. 
        /// 如果窗体中的控件有Dock或者位置与边框重合，将遮挡边框。 
        /// </summary>
        [Category(Consts.DefaultCategory)]
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
        [Category(Consts.DefaultCategory)]
        public int LogoSize { get; set; }
        [Category(Consts.DefaultCategory)]
        public Image Logo { get; set; }



        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        [Category(Consts.DefaultCategory)]
        [DefaultValue(true)]
        public bool AllowResize { get; set; }

        /// <summary>
        /// 允许窗体移动
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public bool AllowMove { get; set; }

        /// <summary>
        /// 标题栏字体
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public Font TitleFont
        {
            get;
            set;
        }

        /// <summary>
        /// 标题栏高度
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public int TitleBarHeight
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// 貌似仅能限制Dock控件的区域
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                // 初始化 clientArea 并考虑 Padding
                Rectangle clientArea = new Rectangle();

                // 计算 X 和 Y 值，考虑 Padding 和窗口状态
                clientArea.X = WindowState == FormWindowState.Maximized ? Padding.Left : BorderSize + Padding.Left;
                clientArea.Y = (WindowState == FormWindowState.Maximized ? Padding.Top : BorderSize + Padding.Top) + TitleBarHeight;

                // 计算 Width 和 Height，考虑 Padding 和窗口状态
                clientArea.Width = this.Width - (WindowState == FormWindowState.Maximized ? Padding.Horizontal : BorderSize * 2 + Padding.Horizontal);
                clientArea.Height = this.Height - (WindowState == FormWindowState.Maximized ? Padding.Vertical : BorderSize * 2 + Padding.Vertical) - TitleBarHeight;

                return clientArea;
            }
        }


        #region 无边框支持AeroSnap
         
        protected override void WndProc(ref Message m)
        {
            // 定义消息常量  
            const int WM_NCCALCSIZE = 0x0083; // 标题栏标准 - 窗口快照  
            const int WM_NCHITTEST = 0x0084;  // 鼠标输入通知，确定窗口的哪个部分对应于一个点  
            const int resizeAreaSize = 10;     // 调整窗口大小的有效区域大小  

            // 进行窗口的边界检测  
            const int HTCLIENT = 1;    // 窗口的客户区域  
            const int HTLEFT = 10;     // 窗口的左边界  
            const int HTRIGHT = 11;    // 窗口的右边界  
            const int HTTOP = 12;      // 窗口的上边界  
            const int HTTOPLEFT = 13;  // 窗口的左上角  
            const int HTTOPRIGHT = 14; // 窗口的右上角  
            const int HTBOTTOM = 15;   // 窗口的下边界  
            const int HTBOTTOMLEFT = 16; // 窗口的左下角  
            const int HTBOTTOMRIGHT = 17; // 窗口的右下角  

            // 检查鼠标位置  
            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m); // 调用基类的WndProc  

                if (this.WindowState == FormWindowState.Normal) // 如果窗口状态是正常  
                {
                    if ((int)m.Result == HTCLIENT) // 如果鼠标在客户区域内  
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32()); // 获取屏幕坐标  
                        Point clientPoint = this.PointToClient(screenPoint); // 将屏幕坐标转换为客户区域坐标  

                        // 判断鼠标位置进行窗口调整  
                        if (clientPoint.Y <= resizeAreaSize) // 如果在窗口上边界的调整区域内  
                        {
                            if (clientPoint.X <= resizeAreaSize) // 在左上角  
                                m.Result = (IntPtr)HTTOPLEFT; // 对角线调整  
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) // 在上边中间区域  
                                m.Result = (IntPtr)HTTOP; // 垂直调整  
                            else // 在右上角  
                                m.Result = (IntPtr)HTTOPRIGHT; // 对角线调整  
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) // 如果在中间区域  
                        {
                            if (clientPoint.X <= resizeAreaSize) // 在左边  
                                m.Result = (IntPtr)HTLEFT; // 水平调整  
                            else if (clientPoint.X > (this.Width - resizeAreaSize)) // 在右边  
                                m.Result = (IntPtr)HTRIGHT; // 水平调整  
                        }
                        else // 在下边区域  
                        {
                            if (clientPoint.X <= resizeAreaSize) // 在左下角  
                                m.Result = (IntPtr)HTBOTTOMLEFT; // 对角线调整  
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) // 在下边中间区域  
                                m.Result = (IntPtr)HTBOTTOM; // 垂直调整  
                            else // 在右下角  
                                m.Result = (IntPtr)HTBOTTOMRIGHT; // 对角线调整  
                        }
                    }
                }
                return;
            }

            // 隐藏窗体的原生边框。这样才能保留AeroSnap特性。 
            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                // 负作用：会导致窗体最大化后超出屏幕范围一点点。
                return;
            }
             

            base.WndProc(ref m);
        }

        private void CustomForm_Resize(object sender, EventArgs e)
        { 
            BorderlessPatch(); 
        }
        private void CustomForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                if (TitleBarHitTest(e.Location))
                {
                    Win32.ReleaseCapture();
                    Win32.SendMessage(this.Handle, 0x112, 0xf012, 0);
                }
            }

            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {

                if (TitleBarHitTest(e.Location))
                {
                    if (WindowState == FormWindowState.Normal)
                    {
                        WindowState = FormWindowState.Maximized;
                    }
                    else if (WindowState == FormWindowState.Maximized)
                    {
                        WindowState = FormWindowState.Normal;
                    }
                }
            }
        }

        private bool TitleBarHitTest(Point point)
        {
            var titleBarRect = new Rectangle(0, 0, this.DisplayRectangle.Width, TitleBarHeight);
            return titleBarRect.Contains(point);
        }


        private void BorderlessPatch()
        {  
            switch (this.WindowState)
            {
                case FormWindowState.Maximized:
                    {
                        this.Padding = new Padding(8, 8, 8, 8);
                    }
                    break;

                case FormWindowState.Normal:
                    {
                        if (this.Padding.Top != this.BorderSize)
                        {
                            this.Padding = new Padding(BorderSize);
                        }

                        // 当窗体被拖到顶部变为最大化后，再变为Normal时，会超出屏幕范围。
                        Rectangle screen = Screen.FromControl(this).WorkingArea;  
                        if (this.Location.X < screen.Left || this.Location.X + this.Width > screen.Right ||
                            this.Location.Y < screen.Top || this.Location.Y + this.Height > screen.Bottom)
                        {
                            // 这两句没有作用，但似乎能防止窗体屏幕外显示
                            this.Left = 449;
                            this.Top = 77;

                        }
                    }
                    break;

            }
        }

        #endregion

        #region IDpiDefined

        public float DesigntimeScaleFactorX { get; set; } = 1;
        public float DesigntimeScaleFactorY { get; set; } = 1;
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

        #endregion



        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var smooth = g.SmoothingMode;

            g.Clear(BackColor);
            //g.FillRectangle(Brushes.WhiteSmoke, DisplayRectangle);


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
                textSize = g.MeasureString(this.TitleText, this.TitleFont); 
            }

            var ShowLogo = true;
            var ShowTitleText = true;
            var ShowTitleCenter = false;

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
                        (int)(textWidth),
                        (int)(textHeight));

                TextRenderer.DrawText(
                    g,
                    TitleText,
                    this.TitleFont,
                    textArea,
                    TitleForeColor,
                    TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
            }
        }

        protected virtual void DrawTitleBackground(Graphics g)
        {

        }

    }
}
