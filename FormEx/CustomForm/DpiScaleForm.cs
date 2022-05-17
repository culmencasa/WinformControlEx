using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;
using System.Diagnostics;
using System.Drawing.Imaging;


namespace System.Windows.Forms
{

    /// <summary>
    /// DpiScaleForm 缩放窗体
    /// 实现思路: 计算当前DPI比例获得factor值, 调用Form的Scale()传入factor值. 调整控件字体.
    /// 
    /// 使用说明:
    /// 1. 窗体继承DpiScaleForm
    /// 2. 保证窗体设计器是在VisualStudio 100%缩放比例下运行的.或者Windows显示设置为100%(96 DPI).
    /// 3. 如果窗体AutoScaleMode默认是Font, 系统会自动根据字体缩放. 效果因系统而异.
    /// 4. 在窗体构造的InitializeComponent()之后, 调用UseDpiScale=true或AutoDpiScale=true.
    /// 5. UseDpiScale=true 将强制按照当前DPI比例缩放.
    /// 6. AutoDpiScale=true 仅当AutoScaleMode属性不等于Font时才会按照DPI比例缩放.
    /// 
    /// 备注: DpiScaleForm类只做了简单布局的缩放测试. 
    ///      较复杂的窗体布局可能不正常. 特殊控件也未做处理. 
    ///      显示器切换的情况未考虑.
    ///      仅作参考.
    ///      
    /// 对于.NET framework 4.7以上Winform 关于DPI的处理, 参见文章 https://docs.telerik.com/devtools/winforms/telerik-presentation-framework/dpi-support?_ga=2.20289336.1856590203.1623301720-198642324.1623301720
    /// 更多: https://www.telerik.com/blogs/winforms-scaling-at-large-dpi-settings-is-it-even-possible-
    /// 
    /// 补充:  2022-05-05 
    ///       1.在Visual Studio自动缩放关闭的条件下设计Winform程序
    ///       "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" /noScale
    ///       
    ///       2.Windows10缩放200%的环境下, Winform程序运行时窗体变得很小且布局错乱
    ///         在app.manifest中禁用所有dpiAware相关的
    ///         <dpiAwareness xmlns="http://schemas.microsoft.com/SMI/2016/WindowsSettings">PerMonitorV2</dpiAwareness>
    ///         则窗体会以200%放大,布局正常
    ///         
    ///       3.在app.manifest中启用
    ///         <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
    ///         则窗体会缩放至正常比例
    ///         
    /// </summary>
    public class DpiScaleForm : Form
    {
        #region 常量

        public const int WM_CREATE = 0x0001;

        #endregion

        #region 字段

        private bool _useDpiScale;
        private bool _autoDpiScale;
        private SizeF currentScaleFactor = new SizeF(1f, 1f);

        private Dictionary<string, Point> originalLocations = new Dictionary<string, Point>();
        private Dictionary<string, Size> originalSizes = new Dictionary<string, Size>();

        #endregion

        #region 属性

        /// <summary>
        /// 设计时窗体的DPI因子，默认为1（即 100%, 96DPI)
        /// 需要在设计器上设置，否则在ResumeLayout()调用之后，ScaleControl()会使用DesignFactor的默认值而不是用户值
        /// </summary>
        public float DesignFactor { get; set; } = 1.0f;


        /// <summary>
        /// 设置是否使用自定义DPI缩放
        /// </summary>
        public bool UseDpiScale
        {
            get
            {
                return _useDpiScale;
            }
            set
            {
                bool changing = _useDpiScale != value;
                _useDpiScale = value;

                if (changing)
                {
                    OnApplyUseDpiScale();
                }
            }
        }

        /// <summary>
        /// 自动缩放
        /// </summary>
        public bool AutoDpiScale
        {
            get
            {
                return _autoDpiScale;
            }
            set
            {
                bool changing = _autoDpiScale != value;
                _autoDpiScale = value;
                if (changing)
                {
                    OnApplyAutoDpiScale();
                }
            }
        }

        /// <summary>
        /// 当前系统的DPI缩放因子
        /// </summary>
        public SizeF CurrentScaleFactor
        {
            get { return currentScaleFactor; }
            private set { currentScaleFactor = value; }
        }

        #endregion

        #region 构造

        public DpiScaleForm()
        {
            // Test：
            // 1.测试用例：如果一个窗体A在另一个系统下不能按设计时的大小正常显示，重新创建一个窗体B，将窗体A的控件都复制到窗体B，窗体B能以设计时的大小正常显示。
            //   说明实际缩放的效果，跟窗体当时设计时（创建时）的DPI设置有关。
            //   即使两个窗体AutoScaleDimensions值都是一样的。
            //   究竟是什么影响了窗体的运行效果，还未找到原因。
            // 2.缩小测试：设计时系统DPI为200%，窗体的AutoScaleMode为DPI, 添加manifest设置，在100%，150%显示的Win7和Win10系统下能正常显示。
            // 3.

            //AutoScaleDimensions记录的是设计时, IContainer使用某一种字体时的宽高比, 不同字体该属性的值不一样

            CurrentScaleFactor = new SizeF(0, 0);
        }


        #endregion

        #region 静态方法

        public static int ScaleInt(int value, SizeF scaleFactor)
        {
            return (int)Math.Round(value * scaleFactor.Width, MidpointRounding.AwayFromZero);
        }

        protected static Bitmap ScaleImage(Image source, Size oldSize, float factor)
        {
            var scaled = new Size((int)Math.Floor(oldSize.Width * factor), (int)Math.Floor(oldSize.Height * factor));

            Bitmap bitmap = new Bitmap(scaled.Width, scaled.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(source, 0, 0, scaled.Width, scaled.Height);
            }

            return bitmap;
        }



        #endregion

        #region 属性变更器

        protected virtual void OnApplyAutoDpiScale()
        {
            if (DesignMode)
                return;

            if (AutoDpiScale)
            {
                // Font模式下, 使用Window的缩放
                if (this.AutoScaleMode == AutoScaleMode.Font)
                {
                    return;
                }
                else
                {
                    var factor = GetCurrentScaleFactor();

                    if (factor > 1)
                    {
                        bool dpiAwareSupport = Environment.OSVersion.Version.Major >= 6;
                        if (dpiAwareSupport)
                        {
                            SetProcessDPIAware();
                            //SetThreadAwarenessContext();
                            //SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.PROCESS_SYSTEM_DPI_AWARE);
                        }
                        else
                        {
                            DesignFactor = AutoScaleDimensions.Width / 96;
                            UseDpiScale = true;
                        }

                    }                    

                }
            }
        }

        protected virtual void OnApplyUseDpiScale()
        {
            if (DesignMode)
                return;

            if (UseDpiScale)
            {
				// 更改AutoScaleMode属性会触发窗体的ScaleControl方法
				//this.AutoScaleMode = AutoScaleMode.Dpi;

				//if (!this.IsHandleCreated)
				//{
				//	this.HandleCreated += (a, b) => { OnApplyUseDpiScale(); };
				//	return;
				//}


				var factor = GetCurrentScaleFactor();
                factor = (DesignFactor / factor);
                CurrentScaleFactor = new SizeF(factor, factor);

                //SetScaleDimension(CurrentScaleFactor);

                this.Scale(CurrentScaleFactor);
                this.ScaleFonts(factor);
                this.ScaleSpecialControl(this, factor);
            }
        }
         

		#endregion


		#region 调整控件字体及大小

		protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_CREATE:
                    {
                        // https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-enablenonclientdpiscaling?redirectedfrom=MSDN

                        //if (UseDpiScale)
                        //{
                        //    EnableNonClientDpiScaling(this.Handle);
                        //}
                    }
                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 缩放控件
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="specified"></param>
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            // 跳过设计模式
            if (DesignMode)
            {
                base.ScaleControl(factor, specified);
                return;
            }

            // 自动模式
            if (AutoDpiScale)
            {
                // 不处理Font模式
                if (this.AutoScaleMode == AutoScaleMode.Font)
                {
                    base.ScaleControl(new SizeF(1, 1), specified);
                }
                else
                {
                    base.ScaleControl(factor, specified);
                }
            }
            // 强制模式
            else if (UseDpiScale)
            {
                base.ScaleControl(this.CurrentScaleFactor, specified);
            }
            else
            {
                base.ScaleControl(factor, specified);
            }
        }


        /// <summary>
        /// 缩放字体
        /// </summary>
        /// <param name="scaleFactor"></param>
        public void ScaleFonts(float scaleFactor)
        {
            // 递归
            ScaleControlFont(this, scaleFactor);
        }

        /// <summary>
        /// 递归设置控件字体
        /// </summary>
        /// <param name="control"></param>
        /// <param name="factor"></param>
        protected virtual void ScaleControlFont(Control control, float factor)
        {
            if (factor <= 1)
                return;

            // 方法1: 直接设置容器控件的字体后, 其子控件字体也会跟着变化.
            // 方法2: 不设置容器控件的字体, 递归设置其子控件的字体.
            if (control is Form || control is GroupBox || control is ContainerControl || control is ScrollableControl)
            {
                foreach (Control child in control.Controls)
                {
                    ScaleControlFont(child, factor);
                }
            }
            else
            {
                //todo: 这里有点问题，字体的缩放并不一定和DPI缩放比例成正比

                control.Font = new Font(control.Font.FontFamily,
                       control.Font.Size * factor,
                       control.Font.Style, control.Font.Unit);
            }

        }

        /// <summary>
        /// 缩放个别控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="factor"></param>
        protected virtual void ScaleSpecialControl(Control control, float factor)
        {
            /* 例子
            foreach (Control child in control.Controls)
            {
                if (child as PictureBox != null)
                {
                    var picbox = (PictureBox)child;
                    if (picbox.SizeMode == PictureBoxSizeMode.AutoSize)
                    {
                        picbox.Image = ScaleImage(picbox.Image, picbox.Size, factor);
                    }
                    else
                    {

                    }
                }

                ScaleSpecialControl(child, factor);
            }
            */

            // 缩放用户控件会有各种各样的问题
        }



        #endregion

        #region 获取当前DPI缩放


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(IntPtr hDC, int nIndex);


        /// <summary>
        /// 见http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        /// </summary>
        public enum DeviceCap
        {
            /// <summary>
            /// Logical pixels inch in X
            /// </summary>
            LOGPIXELSX = 88,
            /// <summary>
            /// Logical pixels inch in Y
            /// </summary>
            LOGPIXELSY = 90,
            VERTRES = 10,
            DESKTOPVERTRES = 117,

        }


        /// <summary>
        /// 获取缩放值, 使用DeviceCap
        /// </summary>
        /// <returns></returns>
        private float GetScalingFactor()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktop = g.GetHdc();
                int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
                int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);


                float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;


                return ScreenScalingFactor;
            }
        }

        /// <summary>
        /// 获取缩放值, 使用ManagementClass类 （备用）
        /// 测试1: 在分辨率为3840x2160的显示器，Win10系统，DPI缩放为100%和200%时，该方法都返回192(即96的2倍)
        /// 测试2: 在分辨率为
        /// </summary>
        /// <returns></returns>
        private float GetScalingFactorUseMC()
        {
            int PixelsPerXLogicalInch = 0;
            int PixelsPerYLogicalInch = 0;
            using (ManagementClass mc = new ManagementClass("Win32_DesktopMonitor"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject item in moc)
                    {
                        PixelsPerXLogicalInch = int.Parse((item.Properties["PixelsPerXLogicalInch"].Value.ToString()));
                        PixelsPerYLogicalInch = int.Parse((item.Properties["PixelsPerYLogicalInch"].Value.ToString()));
                        break;
                    }
                }
            }

            if (PixelsPerXLogicalInch <= 0)
            {
                return 1;
            }

            float factor = PixelsPerXLogicalInch * 1f / 96;

            return factor;
        }

        /// <summary>
        /// 获取缩放值, 使用Graphics.DpiX属性(备用）
        /// 测试1: 在分辨率为3840x2160的显示器，Win10系统，DPI缩放为100%, 200%, 300%时，该方法都返回96
        /// </summary>
        /// <returns></returns>
        private float GetScaleFactorUseGDI()
        {
            float factor = 1;
            if (this.IsHandleCreated)
            {
                using (Graphics currentGraphics = Graphics.FromHwnd(this.Handle))
                {
                    float dpixRatio = currentGraphics.DpiX / 96;
                    float dpiyRatio = currentGraphics.DpiY / 96;

                    factor = dpixRatio;
                }
            }
            else
            {
                using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    float dpiX = graphics.DpiX;
                    float dpiY = graphics.DpiY;

                    factor = dpiY / 96f;
                }
            }

            return factor;
        }

        /// <summary>
        /// 获取当前的缩放值(SizeF单位)
        /// 备注: 由于几个方法都不准, 多次检查
        /// </summary>
        /// <returns></returns>
        public float GetCurrentScaleFactor()
        {
            float factor = GetScalingFactor();
            // 再次检查
            if (factor == 1)
            {
                factor = GetScalingFactorUseMC();
            }
            // 再次检查
            if (factor == 1)
            {
                factor = GetScaleFactorUseGDI();
            }

            return factor;
        }

        #endregion

        #region 保存控件在设计器中的大小和位置, 根据DPI值进行缩放(弃用)

        protected void SaveControls(Control parent)
        {
            if (parent is Form)
            {
                originalSizes.Add(parent.Name, parent.Size);
            }

            foreach (Control con in parent.Controls)
            {
                if (con.Dock == DockStyle.None)
                {
                    originalLocations.Add(con.Name, con.Location);
                }

                originalSizes.Add(con.Name, con.Size);

                if (con.Controls.Count > 0)
                {
                    SaveControls(con);
                }
            }
        }

        protected void SetControls(Control parent)
        {
            if (parent is Form)
            {
                var size = originalSizes[parent.Name];

                parent.Width = ScaleInt(size.Width, this.CurrentScaleFactor);
                parent.Height = ScaleInt(size.Height, this.CurrentScaleFactor);
            }
            foreach (Control con in parent.Controls)
            {
                con.Left = ScaleInt(con.Left, this.CurrentScaleFactor);
                con.Top = ScaleInt(con.Top, this.CurrentScaleFactor);

                var size = originalSizes[con.Name];
                con.Width = ScaleInt(size.Width, this.CurrentScaleFactor);
                con.Height = ScaleInt(size.Height, this.CurrentScaleFactor);

                if (con.Controls.Count > 0)
                {
                    SetControls(con);
                }
            }
        }

        #endregion

        #region Win10 APIs

        /// <summary>
        /// DPI_AWARENESS_CONTEXT
        /// https://docs.microsoft.com/en-us/windows/win32/hidpi/dpi-awareness-context
        /// </summary>
        public enum DPI_AWARENESS_CONTEXT
        {
            DPI_AWARENESS_CONTEXT_UNSPECIFIED = 0,
            DPI_AWARENESS_CONTEXT_UNAWARE = -1,
            DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2, 
            DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -3,
            DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = -4,    // PerMonitorV2 Win10 1703以上版本
            DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED = -5,       // Win10 1809以上版本
        }

        /// <summary>
        /// DPI_AWARENESS属于DPI_AWARENESS_CONTEXT包含的内容
        /// 在新系统下，DPI感知在线程，进程和窗体级别上定义，而不是仅仅是应用程序
        /// https://docs.microsoft.com/en-us/windows/win32/api/windef/ne-windef-dpi_awareness
        /// </summary>
        public enum DPI_AWARENESS
        {
            DPI_AWARENESS_INVALID = -1,
            DPI_AWARENESS_UNAWARE = 0,                  // 对应DPI_AWARENESS_CONTEXT_UNAWARE
            DPI_AWARENESS_SYSTEM_AWARE = 1,             // 对应DPI_AWARENESS_CONTEXT_SYSTEM_AWARE 
            DPI_AWARENESS_PER_MONITOR_AWARE = 2         // 对应DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnableNonClientDpiScaling(IntPtr hwnd);


        [DllImport("User32.dll")]
        public static extern DPI_AWARENESS_CONTEXT GetThreadDpiAwarenessContext();

        [DllImport("User32.dll")]
        public static extern DPI_AWARENESS_CONTEXT GetWindowDpiAwarenessContext(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern DPI_AWARENESS_CONTEXT SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT dpiContext);

        [DllImport("User32.dll")]
        public static extern DPI_AWARENESS GetAwarenessFromDpiAwarenessContext(DPI_AWARENESS_CONTEXT value);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsValidDpiAwarenessContext(DPI_AWARENESS_CONTEXT value);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AreDpiAwarenessContextsEqual(DPI_AWARENESS_CONTEXT dpiContextA, DPI_AWARENESS_CONTEXT dpiContextB);

        public static DPI_AWARENESS GetWindowAwarenessContext(Form window)
        {
            var handle = window.Handle;

            try
            {
                var context = GetWindowDpiAwarenessContext(handle);
                return GetAwarenessFromDpiAwarenessContext(context);
            }
            catch (EntryPointNotFoundException)
            {
                return DPI_AWARENESS.DPI_AWARENESS_INVALID;
            }

        }

        public static DPI_AWARENESS SetThreadAwarenessContext()
        {
            try
            {
                var context = DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2;
                context = SetThreadDpiAwarenessContext(context);

                //if ((int)context == -2147459056)
                //{
                //    uint? windowDpi = null;

                //    if (Process.GetCurrentProcess().MainWindowHandle != IntPtr.Zero)
                //    {
                //        windowDpi = GetDpiForWindow(Process.GetCurrentProcess().MainWindowHandle);
                //    }

                //}


                var awarenss = GetAwarenessFromDpiAwarenessContext(context);
                if (awarenss == DPI_AWARENESS.DPI_AWARENESS_INVALID || awarenss == DPI_AWARENESS.DPI_AWARENESS_UNAWARE)
                {
                    context = DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE;
                    context = SetThreadDpiAwarenessContext(context);
                    awarenss = GetAwarenessFromDpiAwarenessContext(context);
                    if (awarenss == DPI_AWARENESS.DPI_AWARENESS_INVALID || awarenss == DPI_AWARENESS.DPI_AWARENESS_UNAWARE)
                    {
                        context = DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_SYSTEM_AWARE;
                        context = SetThreadDpiAwarenessContext(context);
                        awarenss = GetAwarenessFromDpiAwarenessContext(context);
                        if (awarenss == DPI_AWARENESS.DPI_AWARENESS_INVALID || awarenss == DPI_AWARENESS.DPI_AWARENESS_UNAWARE)
                        {
                            context = DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED;
                            context = SetThreadDpiAwarenessContext(context);
                            awarenss = GetAwarenessFromDpiAwarenessContext(context);
                        }
                    }
                }
                

                return awarenss;
            }
            catch (EntryPointNotFoundException)
            {
                return DPI_AWARENESS.DPI_AWARENESS_INVALID;
            }
        }


        [DllImport("user32.dll")]
        internal static extern uint GetDpiForWindow(IntPtr hWnd);


        #endregion

        #region MyRegion


        public enum PROCESS_DPI_AWARENESS
        {
            PROCESS_DPI_UNAWARE = 0,
            PROCESS_SYSTEM_DPI_AWARE = 1,
            PROCESS_PER_MONITOR_DPI_AWARE = 2
        }

        [DllImport("SHcore.dll")]
        internal static extern int GetProcessDpiAwareness(IntPtr hWnd, out PROCESS_DPI_AWARENESS value);
        [DllImport("Shcore.dll", ExactSpelling = true)]
        internal static extern int SetProcessDpiAwareness(PROCESS_DPI_AWARENESS value);

        private void UpdateDpiAwareness()
        {
            PROCESS_DPI_AWARENESS dpiAwareness;
            int hr = GetProcessDpiAwareness(Process.GetCurrentProcess().MainWindowHandle, out dpiAwareness);
            if (hr != 0)
                MessageBox.Show(this, Marshal.GetExceptionForHR(hr).Message, "Error in GetProcessDpiAwareness!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //comboBox1.SelectedItem = dpiAwareness;
        }
        #endregion



        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        public void SetScaleDimension(SizeF fieldValue)
        {
            var type = typeof(ContainerControl);

            Reflection.PropertyInfo propertyInfo = type.GetProperty("AutoScaleDimensions");
            propertyInfo.SetValue(this, fieldValue, null);

            //var field = type.GetField("autoScaleDimensions", Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Instance | Reflection.BindingFlags.CreateInstance);

            //field.SetValue(this, fieldValue);
        }


    }


}

