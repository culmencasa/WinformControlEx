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

        #region 构造函数

        public CustomForm2024()
        {
        }


        #endregion

        #region 窗体设置

        /// <summary>
        /// 获取当前窗体是否允许最大化
        /// </summary>
        protected virtual bool AllowMaximize
        {
            get
            {
                return this.MaximizeBox;
            }
        }

        /// <summary>
        /// 初始化双缓冲
        /// </summary>
        private void InitializeDoubleBuffering()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            UpdateStyles();
        }
        /// <summary>
        /// 初始化Dpi缩放
        /// </summary>
        private void InitializeDpiScaling()
        {
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
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                if (AllowMaximize)
                {
                    // 防止覆盖任务栏. MaximizedBounds需要在窗体Maximize之前设置才生效
                    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
                    MaximizedBounds = new Rectangle(workingArea.Left, workingArea.Top, workingArea.Width, workingArea.Height);
                }

                // 开启双缓冲
                InitializeDoubleBuffering();

                // 设置窗体事件 
                InitializeAeroSnapEvents();

                // Load事件会在子控件加载后触发, 所以DPI计算不能放在Load事件中
                InitializeDpiScaling();
            }
            base.OnLoad(e);
        }


        #endregion

        #region 重写边框（不支持圆角）

        private Color _borderColor;
        private int _borderSize = 2;


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


        /// <summary>
        /// 重设客户区域
        /// </summary>
        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle clientArea = new Rectangle();
                clientArea.X = (WindowState == FormWindowState.Maximized ? Padding.Left : BorderSize + Padding.Left);
                clientArea.Y = (WindowState == FormWindowState.Maximized ? Padding.Top : BorderSize + Padding.Top) + TitleBarHeight;
                clientArea.Width = this.Width - (WindowState == FormWindowState.Maximized ? Padding.Horizontal : BorderSize * 2 + Padding.Horizontal);
                clientArea.Height = this.Height - (WindowState == FormWindowState.Maximized ? Padding.Vertical : BorderSize * 2 + Padding.Vertical) - TitleBarHeight;

                return clientArea;
            }
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var smooth = g.SmoothingMode;

            g.Clear(BackColor);

            if (this.WindowState == FormWindowState.Normal)
            {
                if (BorderSize > 0)
                {
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


            g.SmoothingMode = smooth;

            base.OnPaint(e);
        }

        #endregion

        #region 无边框支持AeroSnap

        /// <summary>
        /// 标题栏高度  
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public int TitleBarHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 处理窗体消息
        /// </summary>
        /// <param name="m"></param>
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
                            if (clientPoint.X <= resizeAreaSize)
                            {
                                // 在左上角
                                m.Result = (IntPtr)HTTOPLEFT;
                            }
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize))
                            {
                                // 在上边中间区域垂直调整
                                m.Result = (IntPtr)HTTOP;
                            }
                            else
                            {
                                // 在右上角  
                                m.Result = (IntPtr)HTTOPRIGHT;
                            }
                        }
                        else if (clientPoint.Y <= (this.Size.Height - resizeAreaSize)) // 如果在中间区域  
                        {
                            if (clientPoint.X <= resizeAreaSize)
                            {
                                // 在左边水平调整  
                                m.Result = (IntPtr)HTLEFT;
                            }
                            else if (clientPoint.X > (this.Width - resizeAreaSize))
                            {
                                // 在右边水平调整  
                                m.Result = (IntPtr)HTRIGHT;
                            }
                            else
                            {
                                // 当TitleBarHeight为0时，拖动窗体空白区域移动
                                if (TitleBarHeight == 0)
                                {
                                    m.Result = new IntPtr(Win32.HTCAPTION);
                                    return;
                                }
                            }
                        }
                        else // 在下边区域  
                        {
                            if (clientPoint.X <= resizeAreaSize) // 在左下角  
                            {
                                m.Result = (IntPtr)HTBOTTOMLEFT; // 对角线调整  
                            }
                            else if (clientPoint.X < (this.Size.Width - resizeAreaSize)) // 在下边中间区域  
                            {
                                m.Result = (IntPtr)HTBOTTOM; // 垂直调整  
                            }
                            else // 在右下角  
                            {
                                m.Result = (IntPtr)HTBOTTOMRIGHT; // 对角线调整  
                            }
                        }
                    }
                }
            }

            // 隐藏窗体的边框
            if (m.Msg == WM_NCCALCSIZE && m.WParam.ToInt32() == 1)
            {
                // 负作用：会导致窗体最大化后超出屏幕范围一点点。
                return;
            }


            base.WndProc(ref m);
        }

        private void InitializeAeroSnapEvents()
        {
            Resize += AeroSnap_Resize;
            MouseDown += AeroSnap_MouseDown;
        }

        private void AeroSnap_Resize(object sender, EventArgs e)
        {

        }


        private void AeroSnap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks >= 2)
                {
                    if (AllowMaximize)
                    {
                        // 设置了标题栏，则仅允许标题栏双击最大化
                        if (TitleBarHeight > 0 && TitleBarHitTest(e.Location))
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
                        else
                        {
                            WindowState = (WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal);
                        }
                    }
                }
                else
                {
                    DoMove();
                }
            }

        }


        private bool TitleBarHitTest(Point point)
        {
            var titleBarRect = new Rectangle(0, 0, this.DisplayRectangle.Width, TitleBarHeight);
            return titleBarRect.Contains(point);
        }

        private void DoMove()
        {
            Win32.ReleaseCapture();
            Win32.SendMessage(this.Handle, 0x112, 0xf012, 0);
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


    }
}
