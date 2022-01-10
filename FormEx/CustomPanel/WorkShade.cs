using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    /// </summary>
	public partial class WorkShade : Form
	{
        #region 构造

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

            // 使用Visible事件作为Load事件
            this.VisibleChanged += WorkShade_VisibleChanged;
		}

        public WorkShade(int millseconds) :this()
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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (IsHandleCreated && !DesignMode)
            {
                // 默认为圆角, 以免盖住Owner窗体边框
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.UpdateFormRoundCorner(Diameter);
                }
                else
                {
                    this.UpdateFormRoundCorner(0);
                }
            }
        }


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


        public bool PreventResizeWhenShown { get; set; }

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
            if (owner.TopLevel == false)
                throw new Exception("只能使用顶层窗体.");

            Owner = owner;


            Owner.LocationChanged += SyncBounds;
            Owner.Resize += SyncResize;
            Owner.FormClosed += SyncFormClose;
            Owner.VisibleChanged += SyncVisiblity;
            Owner.Activated += SyncActivation;
            Owner.Deactivate += SyncDeactivation;
            Owner.ResizeEnd += SyncResizeEnd;
            // 防止在显示此窗体时, 主窗体拉伸大小
            if (PreventResizeWhenShown)
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
            this.BrintSelfToFront();

            Threading.Timer t = null;
            t = new System.Threading.Timer(new TimerCallback((o)=> {
                this.Invoke((Action)delegate
                {
                    btnClose.Visible = true; 
                });

                if (t != null)
                {
                    t.Dispose();
                }
            }), null, WaitTime, Timeout.Infinite);
            

        }

        public void Detach()
        {
            if (Owner != null)
            {
                Owner.LocationChanged -= SyncBounds;
                Owner.Resize -= SyncResize;
                Owner.FormClosed -= SyncFormClose;
                Owner.VisibleChanged -= SyncVisiblity;
                Owner.Activated -= SyncActivation;
                Owner.Deactivate -= SyncDeactivation;
                Owner.ResizeEnd -= SyncResizeEnd;
            }
        }

        private void SyncResizeEnd(object sender, EventArgs e)
        {
            if (!PreventResizeWhenShown)
            {
                this.SuspendLayout();
                SyncBounds();
                SyncUnderlyaerImage();
                this.ResumeLayout(false);
                this.Refresh();
            }
        }

        private void WorkShade_FormClosing(object sender, FormClosingEventArgs e)
        {
            Owner.LocationChanged -= SyncBounds;
            Owner.Resize -= SyncResize;
            Owner.FormClosed -= SyncFormClose;
            Owner.VisibleChanged -= SyncVisiblity;
            Owner.Activated -= SyncActivation;
            Owner.Deactivate -= SyncDeactivation;

            if (PreventResizeWhenShown)
            {
                Owner.MaximumSize = OldMaxSize;
                Owner.MinimumSize = OldMinSize;
            }

        }
        private void WorkShade_FormClosed(object sender, FormClosedEventArgs e)
        {
            Detach();
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
        }

        #endregion


        #region 与主窗体保持同步

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);


        /// <summary>
        /// 同步主窗体的画面
        /// </summary>
        private void SyncUnderlyaerImage()
        {
            if (Owner.ClientRectangle.Width == 0 || Owner.ClientRectangle.Height == 0)
                return;

            int factor = (int)EnvironmentEx.GetCurrentScaleFactor();
            int titleHeight = Owner.GetTitleBarHeight();



            UnderlayerImage = new Bitmap(Owner.ClientRectangle.Width, Owner.ClientRectangle.Height);

            // 复制窗体图像
            // 已知问题: 修改FormBorderStyle后失效
            var g = Graphics.FromImage(UnderlayerImage);
            IntPtr hDC = g.GetHdc();
            IntPtr windowDC = GetWindowDC(Owner.Handle);
            if (!Win32.BitBlt(
                hDC, -4 * factor, -titleHeight, Owner.Width, Owner.Height, windowDC, 0, 0, CopyPixelOperation.SourceCopy))
            {
                // 如果失败,则使用白色
                g.Clear(Color.White);
            }
            g.ReleaseHdc();

            /****** 模糊图像 ********/
            // 换成另一个开源类GaussianBlur
            // byte[] bgMeta = ImageTool.ToArray(UnderlayerImage);
            //using (var magicker = new ImageMagick.MagickImage(bgMeta))
            //{
            //    magicker.Blur(100,2.5);
            //    UnderlayerImage = new Bitmap(ImageTool.FromArray(magicker.ToByteArray()));
            //}
            var blur = new SuperfastBlur.GaussianBlur(UnderlayerImage);
            var result = blur.Process(10);
            result.Save("workshade.jpg", Drawing.Imaging.ImageFormat.Jpeg);

            
            this.UnderlayerImage = ImageTool.LoadBitmap("workshade.jpg");
        }

        /// <summary>
        /// 同步显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncActivation(object sender, EventArgs e)
        {
            if (Owner == null)
                return;

            // 防止两个窗体抢焦点
            if (Owner.WindowState != OwnerLastWindowState && Owner.WindowState == FormWindowState.Normal)
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
        private void SyncDeactivation(object sender, EventArgs e)
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
        private void SyncFormClose(object sender, FormClosedEventArgs e)
        {
            Owner.LocationChanged -= SyncBounds;
            Owner.Resize -= SyncResize;
            Owner.FormClosed -= SyncFormClose;
            Owner.VisibleChanged -= SyncVisiblity;
            Owner.Activated -= SyncActivation;
            Owner.Deactivate -= SyncDeactivation;
            this.Close();
        }

        private void SyncVisiblity(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                Visible = Owner.Visible;
            }
            else
            {
                Visible = false;
            }
        }

        private void SyncBounds(Object sender = null, EventArgs eventArgs = null)
        {
            if (Owner == null)
                return;

            //this.StartPosition = FormStartPosition.CenterParent;
            //this.Bounds = this.Owner.Bounds;
            //return;


            Rectangle bounds = new System.Drawing.Rectangle(
                Owner.DesktopBounds.X,
                Owner.DesktopBounds.Y + Owner.GetTitleBarHeight(),
                Owner.ClientRectangle.Width,
                Owner.ClientRectangle.Height);

            int factor = (int)EnvironmentEx.GetCurrentScaleFactor();

            var os = EnvironmentEx.GetCurrentOSName();
            if (os >= WindowsNames.Windows10)
            {
                // 在win10系统上横坐标有偏差4-8个像素(目测跟系统窗体的边框有关, 但xp下没有这个问题), high-dpi下*factor倍
                bounds.X += 4 * factor;
            }

            // 貌似只有设置了StartPosition为Manual后, 设置Location属性才能正常显示位置
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = bounds;



        }

        private void SyncResize(object sender, EventArgs e)
        {
            if (Owner == null)
                return;

            if (Owner.IsDisposed)
                return;


            FormWindowState lastState = OwnerLastWindowState;
            // 检测窗体状态变化, 更新控制栏的图标
            if (Owner.WindowState != lastState)
            {
                OwnerLastWindowState = Owner.WindowState;

                OnFormWindowStateChanged(new FormWindowStateArgs()
                {
                    LastWindowState = lastState,
                    NewWindowState = WindowState
                });
            }


            if (Owner.WindowState == FormWindowState.Minimized || Owner.WindowState == FormWindowState.Maximized)
            {
                this.Visible = false;
                return;
            }
            else
            {
                
                this.Visible = true;
            }

            Invalidate();

        }

        #endregion


        private void WorkShade_VisibleChanged(object sender, EventArgs e)
        {
            this.SuspendLayout();
            SyncBounds();
            SyncUnderlyaerImage();
            this.ResumeLayout(false);

        }


        protected override void OnPaintBackground(PaintEventArgs e)
		{
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            if (UnderlayerImage != null)
            {
                g.DrawImageOpacity(UnderlayerImage, (float)Opacity, new Point(0, 0));
            }

            base.OnPaint(e);
        }


        #region DPI相关

        #endregion




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WorkShade_Resize(object sender, EventArgs e)
        {
            this.contentPanel.Location = new Point((this.Width - contentPanel.Width) / 2, (this.Height - contentPanel.Height) / 2);
        }
    }
}
