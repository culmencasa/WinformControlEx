using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class SplashForm : Form
    {
        public SplashForm(Bitmap bitmap)
        {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.Size = bitmap.Size;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            
            this.Show();
            SelectBitmap(bitmap);
        }


        #region 窗体淡出

        //从左到右显示
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        //从右到左显示
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        //从上到下显示
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        //从下到上显示
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        //若使用了AW_HIDE标志，则使窗口向内重叠，即收缩窗口；否则使窗口向外扩展，即展开窗口
        public const Int32 AW_CENTER = 0x00000010;
        //隐藏窗口，缺省则显示窗口
        public const Int32 AW_HIDE = 0x00010000;
        //激活窗口。在使用了AW_HIDE标志后不能使用这个标志
        public const Int32 AW_ACTIVATE = 0x00020000;
        //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        public const Int32 AW_SLIDE = 0x00040000;
        //透明度从高到低
        public const Int32 AW_BLEND = 0x00080000;
        
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //AnimateWindow(Handle, 3000, AW_ACTIVATE);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (e.Cancel == false)
            {
                AnimateWindow(this.Handle, 300, AW_CENTER | AW_BLEND | AW_HIDE);
            }
        }

        #endregion


        private void SelectBitmap(Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ApplicationException("The bitmap must be 32bpp with alpha-channel.");
            }

            IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
            IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                hOldBitmap = Win32.SelectObject(memDc, hBitmap);

                Size newSize = new Size(bitmap.Width, bitmap.Height);	// Size window to match bitmap
                Point sourceLocation = new Point(0, 0);
                Point newLocation = new Point(this.Left, this.Top);		// Same as this window
                BlendFunction blend = new BlendFunction();
                blend.BlendOp = (byte)BlendOperation.AC_SRC_OVER;						// Only works with a 32bpp bitmap
                blend.BlendFlags = 0;											// Always 0
                blend.SourceConstantAlpha = 255;										// Set to 255 for per-pixel alpha values
                blend.AlphaFormat = (byte)BlendOperation.AC_SRC_ALPHA;						// Only works when the bitmap contains an alpha channel

                Win32.UpdateLayeredWindow(Handle, screenDc, ref newLocation, ref newSize,memDc, ref sourceLocation, 0, ref blend, (int)ULWPara.ULW_ALPHA);
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, hOldBitmap);
                    Win32.DeleteObject(hBitmap);										// Remove bitmap resources
                }
                Win32.DeleteDC(memDc);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= (int)WindowStyleEx.WS_EX_LAYERED;
                return createParams;
            }
        }


    }
}
