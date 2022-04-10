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
    /// 后台任务和进度条状态提示浮窗
    /// </summary>
	public partial class BackgroundWorkShade : Form
    {
        #region BackgroundWorker相关

        #region 委托和事件

        /// <summary>
        /// 进度报告委托
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        public delegate void ProcessReportEventHandler(AbortableBackgroundWorker worker, ProgressChangedEventArgs e);
        /// <summary>
        /// 进度报告事件
        /// </summary>
        public event ProcessReportEventHandler ProcessReported;

        /// <summary>
        /// 后台任务委托
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        public delegate void ProcedureEventHandler(AbortableBackgroundWorker worker, DoWorkEventArgs e);
        /// <summary>
        /// 后台任务预告委托
        /// </summary>
        /// <returns></returns>
        public delegate PredictionInfo GivenPrediction();

        #endregion

        #region PredictionInfo类

        /// <summary>
        /// 预告信息(显示在进度条上的提示文字)
        /// </summary>
        public class PredictionInfo
        { 
            /// <summary>
            /// 下一步要做什么
            /// </summary>
            public string WhatsNext { get; set; }
            /// <summary>
            /// 大概要多长时间
            /// </summary>
            public int HowLongWillItTake { get; set; }
            /// <summary>
            /// 完成到一个什么样的进度(百分比)
            /// </summary>
            public int CompletedPercent { get; set; }
        }

        #endregion


        #region 相关字段和属性


        protected object procedureLockObject = new object();

        protected Dictionary<GivenPrediction, ProcedureEventHandler> ProcedureList
        {
            get;
            private set;
        }

        public ShadeStates ShadeState { get; private set; }

        protected bool IsQuickerInstance { get; set; }

        #endregion

        #region 暴露的方法

        /// <summary>
        /// 设置任务
        /// </summary>
        /// <param name="prediction">预告, 时间需要自己根据任务实际时长估量</param>
        /// <param name="procedure">后台调用的过程</param>
        public void Setup(GivenPrediction prediction, ProcedureEventHandler procedure)
        {
            lock (procedureLockObject)
            {
                if (ProcedureList == null)
                {
                    ProcedureList = new Dictionary<GivenPrediction, ProcedureEventHandler>();
                }

                if (!ProcedureList.ContainsKey(prediction))
                {
                    ProcedureList.Add(prediction, procedure);
                }
            }
        }

        #endregion

        #region 重写的方法

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ShadeState = ShadeStates.ShownFirstTime;
            bgwJob.RunWorkerAsync();
        }

        #endregion

        #region 后台事件

        private void bgwJob_DoWork(object sender, DoWorkEventArgs e)
        {
            AbortableBackgroundWorker bgw = sender as AbortableBackgroundWorker;

            while (true)
            {
                if (bgw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                   
                lock (procedureLockObject)
                {
                    if (this.ProcedureList != null && ProcedureList.Count > 0)
                    {
                        var kvPair = ProcedureList.Take(1).First();
                        var prediction = kvPair.Key;

                        // 滚动条
                        var userState = prediction.Invoke();
                        bgw.ReportProgress(0, userState);

                        // 调用操作
                        var process = kvPair.Value;
                        if (process != null)
                        {
                            process.Invoke(bgw, e);
                        }

                        ProcedureList.Remove(prediction);

                        // 如果滚动条进度到达100%, 则退出
                        if (!IsQuickerInstance)
                        {
                            if (userState.CompletedPercent >= pbWorkProgress.MaxValue)
                            {
                                e.Result = true;
                                return;
                            }
                        }
                        else
                        {
                            // 如果是IsQuickerInstance, 则隐藏
                            if (userState.CompletedPercent >= pbWorkProgress.MaxValue)
                            {
                                pbWorkProgress.Invoke(new Action(()=>
                                {
                                    pbWorkProgress.Complete();
                                }));
                            }
                        }
                    }
                }
            }
        }

        private void bgwJob_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AbortableBackgroundWorker bgw = sender as AbortableBackgroundWorker;
            var predictionInfo = e.UserState as PredictionInfo;
            if (predictionInfo != null)
            {
                lblProgressText.Text = predictionInfo.WhatsNext;
                pbWorkProgress.MakeProgress(predictionInfo.CompletedPercent, predictionInfo.HowLongWillItTake);
            }

            OnProcessReported(bgw, e);
        }

        private void bgwJob_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (!IsQuickerInstance)
                {
                    this.Close();
                }
                ExceptionMessageBox box = new ExceptionMessageBox(e.Error);
                box.Show();
            }
            else if (e.Cancelled)
            {
                //
                if (!IsQuickerInstance)
                {
                    this.FadeOut();
                }
            }
            else if (e.Result != null && (bool)e.Result)
            {
                //
                this.FadeOut();
            }
        }

        protected virtual void OnProcessReported(AbortableBackgroundWorker worker, ProgressChangedEventArgs e)
        {
            if (ProcessReported != null)
            {
                ProcessReported(worker, e);
            }
        }

        #endregion

        #region 静态单例

        static BackgroundWorkShade quicker;

        /// <summary>
        /// 不使用透明背景, 保留在静态字段中, 为了加速启动用
        /// </summary>
        public static BackgroundWorkShade Quicker
        {
            get
            {
                if (quicker == null)
                {
                    quicker = new BackgroundWorkShade();
                    //quicker.UseBlur = false;
                    //quicker.UseLayerImage = false;
                    quicker.IsQuickerInstance = true;
                    quicker.pbWorkProgress.OnProgressCompleted += delegate
                    {
                        quicker.Clean();
                        quicker.Hide();
                    };
                }

                return quicker;
            }
        }

        #endregion

        #endregion

        #region 构造

        public BackgroundWorkShade()
        {
            this.Enable2DBuffer();

            InitializeComponent();

            // 默认值
            Diameter = 8;
            Opacity = 1d;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            OwnerLastWindowState = FormWindowState.Minimized;
            
            UseLayerImage = true;
            UseBlur = true;

            // 使用Visible事件作为Load事件
            this.FormClosing += BackgroundWorkShade_FormClosing;
            this.VisibleChanged += WorkShade_VisibleChanged;
            this.Load += WorkShade_Load;
            this.Paint += WorkShade_PaintDialogBoxShadow;

            ShadeState = ShadeStates.Created;
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

        #endregion

        #region 外部事件


        #endregion

        #region 枚举

        public enum ShadeStates
        {
            None,
            Created,
            ShownFirstTime,
            Hidden,
            Closed
        }

        #endregion

        #region 字段


        private Bitmap _boxShadowBitmap;



        #endregion

        #region 属性

        public bool UseLayerImage
        {
            get;
            set;
        }

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

            // 这两个事件会影响窗体显示, 屏蔽掉
            //Owner.VisibleChanged += SyncVisiblityEventHandler;
            //Owner.Activated += SyncActivationEventHandler;
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


        /// <summary>
        /// 显示窗体
        /// </summary>
        public new void Show()
        {
            Opacity = 1;
            base.Show();
            BrintSelfToFront();

        }

        /// <summary>
        /// 隐藏窗体
        /// </summary>
        public new void Hide()
        {
            base.Hide();
            ShadeState = ShadeStates.Hidden;
            if (Owner != null)
            {
                Owner.Focus();
            }
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

            this.FormClosing -= WorkShade_FormClosing;
            this.FormClosed -= WorkShade_FormClosed;
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

        /// <summary>
        /// 淡出并关闭窗体
        /// </summary>
        public void FadeOut()
        {
            int duration = 500;
            int steps = 10;
            Timer timer = new Timer();
            timer.Interval = duration / steps;

            int currentStep = 1;
            timer.Tick += (arg1, arg2) =>
            {
                Opacity = 1 - ((double)currentStep / steps);
                currentStep++;

                if (Opacity <= 0.1)
                {
                    timer.Stop();
                    timer.Dispose();
                    this.Close();
                }
            };

            timer.Start();
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
            if (!IsOwnerAlive() || ShadeState < ShadeStates.ShownFirstTime)
            {
                return;
            }

            // 防止两个窗体抢焦点
            if (Owner.WindowState != OwnerLastWindowState)
            {
                OwnerLastWindowState = Owner.WindowState;
                if (!this.Visible)
                {
                    Show();
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
            if (ShadeState >= ShadeStates.ShownFirstTime)
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
            else if (Owner.WindowState == FormWindowState.Normal && this.Visible)
            {
                this.pnlCenterBox.Visible = false;
                SyncBoundsEventHandler();
                SyncUnderlyaerImage();
                this.Refresh();
                this.pnlCenterBox.Visible = true;
            }
            else if (Owner.WindowState == FormWindowState.Maximized && this.Visible)
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
                    this.Paint -= WorkShade_PaintDialogBoxShadow;
                    this.SuspendLayout();
                    this.SyncUnderlyaerImage();
                    this.Paint += WorkShade_PaintDialogBoxShadow;
                    this.ResumeLayout(false);
                    this.Refresh();
                });

            }
            else
            {
                //this.Visible = true;
                //Invalidate();
            }
        }

        #endregion

        #region 窗体其他事件

        private void WorkShade_Load(object sender, EventArgs e)
        {
        }
        private void WorkShade_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.SuspendLayout();
                SyncBoundsEventHandler();
                SyncUnderlyaerImage();
                this.ResumeLayout(false);
            }
        }

        private void BackgroundWorkShade_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果是Owner关闭, 则无条件关闭
            if (e.CloseReason == CloseReason.FormOwnerClosing)
            {
                e.Cancel = false;
                return;
            }

            // 如果使用的是Quicker实例, 则隐藏代替关闭
            if (IsQuickerInstance && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
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
            Clean();
        }

        #endregion

        #region 窗体重绘事件


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (UseLayerImage)
            {
                if (Owner != null)
                {
                    g.Clear(Owner.BackColor);
                }
            }
            else
            {
                g.Clear(Color.Black);
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

            if (UseLayerImage)
            {
                if (Owner != null)
                {
                    g.Clear(Owner.BackColor);
                }
                if (UnderlayerImage != null)
                {
                    g.DrawImageOpacity(UnderlayerImage, (float)Opacity, new Point(0, 0));
                }
            }

            base.OnPaint(e);
        }


        #endregion

        #region 遮罩层对话框阴影重绘事件

        /// <summary>
        /// 居中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkShade_Resize(object sender, EventArgs e)
        {
            this.pnlCenterBox.Location = new Point((this.Width - pnlCenterBox.Width) / 2, (this.Height - pnlCenterBox.Height) / 2);
        }

        /// <summary>
        /// 画遮罩层阴影
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkShade_PaintDialogBoxShadow(object sender, PaintEventArgs e)
        {
            if (UseLayerImage)
            {
                return;
            }

            if (!IsOwnerAlive() || UnderlayerImage == null)
            {
                return;
            }

            if (_boxShadowBitmap == null || _boxShadowBitmap.Size != this.Size)
            {
                _boxShadowBitmap?.Dispose();
                _boxShadowBitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppArgb);
            }

            var control = this.pnlCenterBox;
            Graphics g = e.Graphics;
            g.SetFastRendering();
            var rect = new Rectangle(control.Location.X, control.Location.Y, control.Size.Width, control.Size.Height);
            
            using (GraphicsPath graphicPath = g.GenerateRoundedRectangle(rect, pnlCenterBox.RoundBorderRadius, RectangleEdgeFilter.All))
            {
                DrawShadowSmooth(graphicPath, 100, 60, _boxShadowBitmap);
            }
            e.Graphics.DrawImage(_boxShadowBitmap, new Point(0, 0));

        }


        #endregion

        #region DPI相关

        #endregion

        #region 按钮事件

        private void btnClose_Click(object sender, EventArgs e)
        {
            Clean();
            this.Close();
        }

        #endregion

        #region 私有方法

        private void Clean()
        {
            // 如果是Quicker实例, 则不关闭后台线程. 
            if (!IsQuickerInstance)
            {
                if (bgwJob != null)
                {
                    bgwJob.CancelAsync();
                    Thread.Sleep(1);
                    if (bgwJob.IsBusy)
                    {
                        bgwJob.Abort();
                    }
                }
            }

            lock (procedureLockObject)
            {
                if (ProcedureList != null)
                {
                    ProcedureList.Clear();
                }
            }

            pbWorkProgress.Reset();
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
            else
            {
                g.SetSlowRendering();
            }
            g.Clear(System.Drawing.SystemColors.Control);
            IntPtr hDC = g.GetHdc();
            IntPtr windowDC = GetWindowDC(Owner.Handle);
            if (!Win32.BitBlt(
                hDC,
                -8 * factor,
                -titleHeight, 
                Owner.ClientRectangle.Width + (8 * factor), 
                Owner.ClientRectangle.Height + titleHeight,
                windowDC, 
                0, 
                0, 
                CopyPixelOperation.SourcePaint))
            {
                // 如果失败,则使用白色
                g.Clear(System.Drawing.SystemColors.Control);
            }
            g.ReleaseHdc();

            /****** 模糊图像 ********/
            if (UseBlur)
            {
                var blur = new SuperfastBlur.GaussianBlur(UnderlayerImage);
                this.UnderlayerImage = blur.Process(20);
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




        #endregion
  }
}
