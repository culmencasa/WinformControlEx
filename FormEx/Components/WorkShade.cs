using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    /// <summary>
    /// 添加一层提示
    /// <para>废弃不再更新</para>
    /// </summary>
    [Obsolete]
	public partial class WorkShade : Form
    {
        #region 构造

        ///<summary>
        /// 创建实例
        /// <para>用法: </para>
        /// <para>WorkShade ws = new WorkShade();</para>
        /// <para>ws.Attach(ownerForm);</para>
        /// <para>ws.FadeIn(); </para>
        /// </summary>
        public WorkShade()
        {
            this.Enable2DBuffer();

            // 防止闪烁
            //SetStyle(ControlStyles.Opaque | ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, false);

            InitializeComponent();

            // 默认值
            Diameter = 8;
            Opacity = 1d;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            OwnerLastWindowState = FormWindowState.Minimized;

            UseBlur = true;

            // 使用Visible事件作为Load事件
            this.VisibleChanged += WorkShade_VisibleChanged;
            this.Load += WorkShade_Load;
            this.Paint += WorkShade_Paint;
        }



        /// <summary>
        /// 指定多少秒后显示关闭按钮
        /// </summary>
        /// <param name="millseconds"></param>
        public WorkShade(int millseconds) : this()
        {
            this.WaitTime = millseconds;
        }


        #endregion

        #region 窗体设置

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                // 设置窗体为ToolWindow, 不响应Alt-Tab, 以免发生Alt-Tab切换到此窗体, Owner窗体却不可见.
                cp.ExStyle |= 0x80;

                return cp;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        #endregion

        #region 外部事件

        public event Action Button1Action;

        #endregion

        #region 字段


        private Bitmap _boxShadowBitmap;

        Timer fadeInTimer = null;
        Timer fadeOutTimer = null;


        #endregion

        #region 属性

        /// <summary>
        /// 虚假的透明背景图像
        /// </summary>
        protected Bitmap UnderlayerImage
        {
            get;
            set;
        }

        protected FormWindowState OwnerLastWindowState { get; set; }


        protected Size OldMaxSize { get; set; }

        protected Size OldMinSize { get; set; }


        public bool DisableOwnerResize { get; set; }

        /// <summary>
        /// 边角圆角的直径
        /// </summary>
        public int Diameter
        {
            get;
            set;
        }

        public int WaitTime
        {
            get;
            set;
        }

        public bool CoverCaptionArea
        {
            get;
            set;
        }

        public bool UseBlur
        {
            get;
            set;
        }

        public string ProgressText
        {
            get
            {
                return lblProgressText.Text;
            }
            set
            {
                lblProgressText.Text = value;
            }
        }

        public string CloseButtonText
        {
            get
            {
                return btnClose.Text;
            }
            set
            {
                btnClose.Text = value;
            }
        }

        public bool ShowButton1 { get; set; }

        #endregion

        #region 窗体状态变化事件

        public delegate void WindowStateChangedHandler(object sender, FormWindowStateArgs e);
        public event WindowStateChangedHandler FormWindowStateChanged;

        /// <summary>
        /// 最小化最大时调用事件
        /// </summary>
        /// <param name="e"></param>
        protected void OnFormWindowStateChanged(FormWindowStateArgs e)
        {
            if (FormWindowStateChanged != null)
            {
                FormWindowStateChanged(this, e);
            }
        }

        #endregion

        #region 公开的方法

        /// <summary>
        /// 连接主窗体
        /// </summary>
        /// <param name="owner">主窗体</param>
        public void Attach(Form owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException();
            }
            if (Owner == owner)
            {
                return;
            }

            if (Owner != null)
            {
                Detach();
            }

            if (owner.TopLevel == false)
            {
                throw new Exception("只能使用顶层窗体.");
            }

            Owner = owner;

            Owner.LocationChanged += SyncBoundsEventHandler;
            Owner.Resize += SyncResizeEventHandler;
            Owner.FormClosed += SyncFormCloseEventHandler;
            Owner.VisibleChanged += SyncVisiblityEventHandler;
            Owner.Activated += SyncActivationEventHandler;
            Owner.Deactivate += SyncDeactivationEventHandler;
            Owner.ResizeEnd += SyncResizeEndEventHandler;

            // 防止在显示此窗体时, 主窗体拉伸大小
            if (DisableOwnerResize)
            {
                OldMaxSize = Owner.MaximumSize;
                OldMinSize = Owner.MinimumSize;
                Owner.MaximumSize = Owner.Size;
                Owner.MinimumSize = Owner.Size;
            }

            this.FormClosing += WorkShade_FormClosing;
            this.FormClosed += WorkShade_FormClosed;

        }

        public new void Show()
        {
            base.Show();

            ShowButtonWhenTimeIsUp();
        }

        /// <summary>
        /// 重写的Show方法
        /// </summary>
        public void FadeIn()
        {
            base.Show();


            Opacity = 0;

            StartFadeInTimer();

            BrintSelfToFront();

            // 等待几秒后显示按钮
            ShowButtonWhenTimeIsUp();


        }



        /// <summary>
        /// 取消关联的事件
        /// </summary>
        public void Detach()
        {
            if (Owner != null)
            {
                Owner.LocationChanged -= SyncBoundsEventHandler;
                Owner.Resize -= SyncResizeEventHandler;
                Owner.VisibleChanged -= SyncVisiblityEventHandler;
                Owner.Activated -= SyncActivationEventHandler;
                Owner.Deactivate -= SyncDeactivationEventHandler;
                Owner.ResizeEnd -= SyncResizeEndEventHandler;
            }
        }

        /// <summary>
        /// 窗体前置
        /// </summary>
        public void BrintSelfToFront()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            TopMost = true;
            Focus();
            BringToFront();
            TopMost = false;

            //Win32.SetForegroundWindow(Handle);
        }

        public void FadeOut()
        {

            pnlCenterBox.Visible = false;

            StartFadeOutTimer();
        }

        #endregion

        #region 与主窗体保持同步

        /// <summary>
        /// 同步显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncActivationEventHandler(object sender, EventArgs e)
        {
            if (!IsOwnerAlive())
            {
                return;
            }

            // 防止两个窗体抢焦点
            if (Owner.WindowState != OwnerLastWindowState)
            {
                OwnerLastWindowState = Owner.WindowState;
                if (!this.Visible)
                {
                    base.Show();
                    //Win32.SetForegroundWindow(Handle);
                }
                BrintSelfToFront();
            }
        }

        /// <summary>
        /// 同步隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncDeactivationEventHandler(object sender, EventArgs e)
        {
            //IsOwnerPainting = false;
            //this.TopMost = false;
            //this.Hide();
        }


        /// <summary>
        /// 同步关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncFormCloseEventHandler(object sender, FormClosedEventArgs e)
        {
            this.Detach();
            this.Close();
        }

        private void SyncVisiblityEventHandler(object sender, EventArgs e)
        {
            if (IsOwnerAlive())
            {
                Visible = Owner.Visible;
            }
            else
            {
                Visible = false;
            }
        }

        private void SyncBoundsEventHandler(Object sender = null, EventArgs eventArgs = null)
        {
            if (!IsOwnerAlive())
            {
                return;
            }


            if (CoverCaptionArea)
            {
                this.StartPosition = FormStartPosition.CenterParent;
                this.Bounds = this.Owner.Bounds;
            }
            else
            {
                Rectangle bounds = new System.Drawing.Rectangle(
                    Owner.DesktopBounds.X,
                    Owner.DesktopBounds.Y + Owner.GetTitleBarHeight(),
                    Owner.ClientRectangle.Width,
                    Owner.ClientRectangle.Height);

                int factor = (int)EnvironmentEx.GetCurrentScaleFactor();

                Point leftTopPosition = new Point(0, 0);
                //if (Owner.Controls.Count > 0)
                //{
                //    leftTopPosition = Owner.Controls[0].Location;
                //}
                var os = EnvironmentEx.GetCurrentOSName();
                if (os == WindowsNames.Windows11)
                {
                    Point innerLocation = Owner.PointToScreen(leftTopPosition);
                    bounds.X = innerLocation.X - Owner.Padding.Left + 1;
                    bounds.Width -= 1;
                }
                else if (os >= WindowsNames.Windows10)
                {
                    // bug: 在win10系统上横坐标有偏差4-8个像素(目测跟系统窗体的边框和阴影有关, 但xp下没有这个问题), high-dpi下*factor倍
                    //bounds.X += 10 * factor;

                    // workaround:
                    Point innerLocation = Owner.PointToScreen(leftTopPosition);
                    bounds.X = innerLocation.X - Owner.Padding.Left;
                }
                else
                {
                    Point innerLocation = Owner.PointToScreen(leftTopPosition);
                    bounds.X = innerLocation.X - Owner.Padding.Left + 1;
                    bounds.Width -= 1;
                }


                // 貌似只有设置了StartPosition为Manual后, 设置Location属性才能正常显示位置
                this.StartPosition = FormStartPosition.Manual;
                this.Bounds = bounds;
            }


        }

        /// <summary>
        /// 当主窗体大小状态发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncResizeEventHandler(object sender, EventArgs e)
        {
            if (!IsOwnerAlive())
            {
                return;
            }

            FormWindowState lastState = OwnerLastWindowState;
            if (Owner.WindowState != lastState)
            {
                OwnerLastWindowState = Owner.WindowState;
                OnFormWindowStateChanged(new FormWindowStateArgs()
                {
                    LastWindowState = lastState,
                    NewWindowState = WindowState
                });

                OnOwnerSizeChanged();
            }

        }
        private void SyncResizeEndEventHandler(object sender, EventArgs e)
        {
            if (!DisableOwnerResize)
            {
                this.SuspendLayout();
                SyncBoundsEventHandler();
                SyncUnderlyaerImage();
                this.ResumeLayout(false);
                this.Refresh();
            }
        }

        private void OnOwnerSizeChanged()
        {
            if (Owner.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
            }
            else if (Owner.WindowState == FormWindowState.Normal)
            {
                this.pnlCenterBox.Visible = false;
                SyncBoundsEventHandler();
                SyncUnderlyaerImage();
                this.Refresh();
                this.pnlCenterBox.Visible = true;
            }
            else if (Owner.WindowState == FormWindowState.Maximized)
            {
                // 最大化时重新画背景的性能不好，先展示纯色背景, 以免出现残影
                //todo: 将对话框阴影画到底图上
                UnderlayerImage = null;
                Refresh();

                // 重设窗体大小
                SyncBoundsEventHandler();
                BrintSelfToFront();



                this.BeginInvoke((Action)delegate
                {
                    this.Paint -= WorkShade_Paint;
                    this.SuspendLayout();
                    this.SyncUnderlyaerImage();
                    this.Paint += WorkShade_Paint;
                    this.ResumeLayout(false);
                    this.Refresh();
                });

            }
            else
            {
                UseBlur = true;
                this.Visible = true;
                Invalidate();
            }
        }

        #endregion

        #region 窗体其他事件

        private void WorkShade_Load(object sender, EventArgs e)
        {
        }
        private void WorkShade_VisibleChanged(object sender, EventArgs e)
        {
            // issue: 有可能主窗体的子控件还未完全加载, 故会出现白块...
            if (this.Visible)
            {
                this.SuspendLayout();
                SyncBoundsEventHandler();
                SyncUnderlyaerImage();
                this.ResumeLayout(false);
            }
        }




        private void WorkShade_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DisableOwnerResize)
            {
                Owner.MaximumSize = OldMaxSize;
                Owner.MinimumSize = OldMinSize;
            }

        }

        private void WorkShade_FormClosed(object sender, FormClosedEventArgs e)
        {
            Owner?.Activate();
        }





        #endregion

        #region 窗体重绘事件


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (Owner != null)
            {
                g.Clear(Owner.BackColor);
            }

            //base.OnPaintBackground(e);
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!IsOwnerAlive())
            {
                return;
            }

            Graphics g = e.Graphics;
            g.SetFastRendering();
            g.Clear(Owner.BackColor);

            if (UnderlayerImage != null)
            {
                //g.DrawImageOpacity(UnderlayerImage, (float)Opacity, new Point(0, 0));
                g.DrawImageOpacity(UnderlayerImage, 1, new Point(0, 0));
            }

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (IsHandleCreated && !DesignMode)
            {
                if (IsOwnerAlive() && (Owner.WindowState != FormWindowState.Minimized))
                {
                    // 默认为圆角, 以免盖住Owner窗体边框
                    UpdateFormRoundCorner();
                }
                else
                {

                    var os = EnvironmentEx.GetCurrentOSName();
                    if (os == WindowsNames.Windows11)
                    {
                        Diameter = 7;
                        UpdateFormRoundCorner();
                    }
                    else if (os >= WindowsNames.Windows10)
                    {
                        Diameter = 0;
                        UpdateFormRoundCorner();
                    }

                }
            }
        }


        #endregion

        #region 遮罩层对话框阴影重绘事件

        private void WorkShade_Resize(object sender, EventArgs e)
        {
            this.pnlCenterBox.Location = new Point((this.Width - pnlCenterBox.Width) / 2, (this.Height - pnlCenterBox.Height) / 2);
        }

        // 画遮罩层阴影
        private void WorkShade_Paint(object sender, PaintEventArgs e)
        {
            if (!IsOwnerAlive() || UnderlayerImage == null)
                return;

            var control = this.pnlCenterBox;

            if (control.Visible)
            {
                //todo: 阴影加深, 偏移
                if (_boxShadowBitmap == null || _boxShadowBitmap.Size != this.Size)
                {
                    _boxShadowBitmap?.Dispose();
                    _boxShadowBitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
                }

                Graphics g = e.Graphics;
                g.SetFastRendering();
                var rect = new Rectangle(control.Location.X, control.Location.Y, control.Size.Width, control.Size.Height);


                using (GraphicsPath graphicPath = g.GenerateRoundedRectangle(rect, pnlCenterBox.RoundBorderRadius, RectangleEdgeFilter.All))
                {
                    DrawShadowSmooth(graphicPath, 100, 60, _boxShadowBitmap);
                }
                e.Graphics.DrawImage(_boxShadowBitmap, new Point(0, 0));
            }
        }


        #endregion

        #region DPI相关

        #endregion

        #region 按钮事件

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

            Button1Action?.Invoke();
        }

        #endregion

        #region 私有方法

        private void ShowButtonWhenTimeIsUp()
        {
            // 等待几秒后显示按钮
            if (WaitTime > 0)
            {
                Threading.Timer t = null;
                t = new System.Threading.Timer(new TimerCallback((o) =>
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke((Action)delegate
                        {
                            btnClose.Visible = true;
                        });
                    }

                    if (t != null)
                    {
                        t.Dispose();
                    }
                }), null, WaitTime, Timeout.Infinite);
            }
            else
            {
                btnClose.Visible = ShowButton1;
            }
        }

        private bool IsOwnerAlive()
        {
            if (Owner == null || Owner.IsDisposed)
                return false;

            if (!Owner.IsHandleCreated)
                return false;

            return true;
        }
        public void UpdateFormRoundCorner()
        {
            if (Diameter == 0)
            {
                this.Region = new Region(new Rectangle(0, 0, this.Width, this.Height));
            }
            else
            {
                // 防止控件撑出窗体            
                IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, this.Width, this.Height, Diameter / 2 + 4, Diameter / 2 + 4);
                this.Region = System.Drawing.Region.FromHrgn(hrgn);
                this.Update();
                Win32.DeleteObject(hrgn);
            }
        }


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);


        /// <summary>
        /// 同步主窗体的画面
        /// </summary>
        private void SyncUnderlyaerImage(bool lowquality = true)
        {
            if (!IsOwnerAlive() || (Owner.ClientRectangle.Width == 0 || Owner.ClientRectangle.Height == 0))
            {
                return;
            }

            int factor = (int)EnvironmentEx.GetCurrentScaleFactor();
            int titleHeight = Owner.GetTitleBarHeight();

            UnderlayerImage = new Bitmap(Owner.ClientRectangle.Width, Owner.ClientRectangle.Height);

            // 复制窗体图像
            // 已知问题: 修改FormBorderStyle后失效
            var g = Graphics.FromImage(UnderlayerImage);
            if (lowquality)
            {
                g.SetFastRendering();
            }
            g.Clear(System.Drawing.SystemColors.Control);
            IntPtr hDC = g.GetHdc();
            IntPtr windowDC = GetWindowDC(Owner.Handle);
            if (!Win32.BitBlt(
                hDC,
                1, // -4 * factor,
                -titleHeight, Owner.Width, Owner.Height, windowDC, 0, 0, CopyPixelOperation.SourcePaint))
            {
                // 如果失败,则使用白色
                g.Clear(System.Drawing.SystemColors.Control);
            }
            g.ReleaseHdc();

            /****** 模糊图像 ********/
            if (UseBlur)
            {
                // 换成另一个开源类GaussianBlur
                // byte[] bgMeta = ImageTool.ToArray(UnderlayerImage);
                //using (var magicker = new ImageMagick.MagickImage(bgMeta))
                //{
                //    magicker.Blur(100,2.5);
                //    UnderlayerImage = new Bitmap(ImageTool.FromArray(magicker.ToByteArray()));
                //}
                var blur = new SuperfastBlur.GaussianBlur(UnderlayerImage);
                this.UnderlayerImage = blur.Process(10);
            }
        }

        private void DrawShadowSmooth(GraphicsPath gp, int intensity, int radius, Bitmap dest)
        {
            using (Graphics g = Graphics.FromImage(dest))
            {
                g.Clear(Color.Transparent);
                g.CompositingMode = CompositingMode.SourceCopy;
                double alpha = 0;
                double astep = 0;
                double astepstep = (double)intensity / radius / (radius / 2D);
                for (int thickness = radius; thickness > 0; thickness--)
                {
                    using (Pen p = new Pen(Color.FromArgb((int)alpha, 0, 0, 0), thickness))
                    {
                        p.LineJoin = LineJoin.Round;
                        g.DrawPath(p, gp);
                    }
                    alpha += astep;
                    astep += astepstep;
                }
            }
        }


        private void StartFadeInTimer()
        {
            if (fadeOutTimer != null && fadeOutTimer.Enabled)
            {
                fadeOutTimer.Stop();
                fadeOutTimer.Dispose();
                fadeOutTimer = null;
            }

            if (fadeInTimer == null)
            {
                int duration = 300;
                int steps = 10;

                fadeInTimer = new Timer();
                fadeInTimer.Interval = duration / steps;

                int currentStep = 1;
                fadeInTimer.Tick += (arg1, arg2) =>
                {
                    if (!this.IsHandleCreated || this.IsDisposed)
                    {
                        fadeInTimer.Stop();
                        fadeInTimer = null;
                        return;
                    }

                    Opacity = Opacity + ((double)currentStep / steps);
                    currentStep++;

                    if (Opacity >= 1.00)
                    {
                        fadeInTimer.Stop();
                        fadeInTimer.Dispose();
                        fadeInTimer = null;
                    }
                };
            }
            fadeInTimer.Start();
        }

        private void StartFadeOutTimer()
        {
            if (fadeInTimer != null && fadeInTimer.Enabled)
            {
                fadeInTimer.Stop();
                fadeInTimer.Dispose();
                fadeInTimer = null;
            }

            if (fadeOutTimer == null)
            {
                int duration = 300;
                int steps = 10;
                fadeOutTimer = new Timer();
                fadeOutTimer.Interval = duration / steps;

                int currentStep = 1;
                fadeOutTimer.Tick += (arg1, arg2) =>
                {
                    if (!this.IsHandleCreated || this.IsDisposed)
                    {
                        fadeOutTimer.Stop();
                        fadeOutTimer = null;
                        return;
                    }

                    Opacity = 1 - ((double)currentStep / steps);
                    currentStep++;

                    if (Opacity <= 0.1)
                    {
                        fadeOutTimer.Stop();
                        fadeOutTimer.Dispose();
                        fadeOutTimer = null;
                        this.Close();
                    }
                };
            }


            fadeOutTimer.Start();
        }



        #endregion
    }
}
