using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing
{
    public partial class Win32
    {
        public const int ANSI_CHARSET = 0;
        public const uint ANTIALIASED_QUALITY = 4;
        public const int BI_ALPHABITFIELDS = 6;
        public const int BI_BITFIELDS = 3;
        public const int BI_RGB = 0;
        public const uint CLEARTYPE_QUALITY = 5;
        public const int DEFAULT_CHARSET = 1;
        public const uint DEFAULT_QUALITY = 0;
        public const uint DRAFT_QUALITY = 1;
        public const int DT_BOTTOM = 8;
        public const int DT_CALCRECT = 0x400;
        public const int DT_CENTER = 1;
        public const int DT_EDITCONTROL = 0x2000;
        public const int DT_END_ELLIPSIS = 0x8000;
        public const int DT_EXPANDTABS = 0x40;
        public const int DT_EXTERNALLEADING = 0x200;
        public const int DT_INTERNAL = 0x1000;
        public const int DT_LEFT = 0;
        public const int DT_MODIFYSTRING = 0x10000;
        public const int DT_NOCLIP = 0x100;
        public const int DT_NOFULLWIDTHCHARBREAK = 0x80000;
        public const int DT_NOPREFIX = 0x800;
        public const int DT_PATH_ELLIPSIS = 0x4000;
        public const int DT_RIGHT = 2;
        public const int DT_RTLREADING = 0x20000;
        public const int DT_SINGLELINE = 0x20;
        public const int DT_TABSTOP = 0x80;
        public const int DT_TOP = 0;
        public const int DT_VCENTER = 4;
        public const int DT_WORD_ELLIPSIS = 0x40000;
        public const int DT_WORDBREAK = 0x10;
        public const uint FF_DECORATIVE = 80;
        public const uint FF_DONTCARE = 0;
        public const uint FF_MODERN = 0x30;
        public const uint FF_ROMAN = 0x10;
        public const uint FF_SCRIPT = 0x40;
        public const uint FF_SWISS = 0x20;
        public const int FW_BOLD = 700;
        public const int FW_DONTCARE = 0;
        public const int FW_EXTRABOLD = 800;
        public const int FW_EXTRALIGHT = 200;
        public const int FW_LIGHT = 300;
        public const int FW_MEDIUM = 500;
        public const int FW_NORMAL = 400;
        public const int FW_SEMIBOLD = 600;
        public const int FW_THIN = 100;
        internal const int GPTR = 0x40;
        public const int LOGPIXELSX = 0x58;
        public const int LOGPIXELSY = 90;
        public const uint NONANTIALIASED_QUALITY = 3;
        public const int OPAQUE = 2;
        public const uint PROOF_QUALITY = 2;
        public const int SRCCOPY = 0xcc0020;
        public const int TRANSPARENT = 1;


        // new
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CTLCOLOREDIT = 0x133;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_PAINT = 0xF;
        public const int WM_CREATE = 0x0001;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCMOUSEMOVE = 0x00A0;

        // 控制栏命令
        public const int WM_SYSCOMMAND = 0x112;
        public const int SC_MAXMIZE = 0xF030;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MOVE = 0xF010;          //向窗口发送移动的消息 
        public const int SC_RESTORE = 0xF120;

        // 点击命令
        public const int WM_NCHITTEST = 0x0084;
        public const int HTTRANSPARENT = -1;

        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 0x10;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTCAPTION = 2;
        public const int HTCLIENT = 1;

        public const int WM_FALSE = 0;
        public const int WM_TRUE = 1;
        // end


        // scrollbar
        private const uint SB_HORZ = 0; //Horrizontal Scroll
        private const uint SB_VERT = 1; //Vertical Scroll
        private const uint ESB_DISABLE_BOTH = 0x3;
        private const uint ESB_ENABLE_BOTH = 0x0;
        

        // SetWindowPos
        public const int SWP_NOSIZE = 0x0001,
           SWP_NOMOVE = 0x0002,
           SWP_NOZORDER = 0x0004,
           SWP_NOREDRAW = 0x0008,
           SWP_NOACTIVATE = 0x0010,
           SWP_FRAMECHANGED = 0x0020,  /* The frame changed: send WM_NCCALCSIZE */
           SWP_SHOWWINDOW = 0x0040,
           SWP_HIDEWINDOW = 0x0080,
           SWP_NOCOPYBITS = 0x0100,
           SWP_NOOWNERZORDER = 0x0200,  /* Don't do owner Z ordering */
           SWP_NOSENDCHANGING = 0x0400;

        public const int WS_BORDER = 0x00800000;

        #region user32.dll

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 自定义的结构
        /// </summary>
        public struct CustomSendMessageParam
        {
            public int i;
            public string s;
        }
        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);



        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, Boolean bRedraw);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
            
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,         // 信息发往的窗口的句柄
            int Msg,                // 消息ID
            int wParam,          // 参数1
            ref CustomSendMessageParam lParam //参数2
        );


        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam            // 参数2
        );

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowScrollBar(System.IntPtr hWnd, int wBar, bool bShow);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        ///     Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs..</param>
        /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the  following values: GWL_EXSTYLE, GWL_HINSTANCE, GWL_ID, GWL_STYLE, GWL_USERDATA, GWL_WNDPROC
        /// </param>
        /// <param name="dwNewLong">The replacement value.</param>
        /// <returns> If the function succeeds, the return value is the previous value of the specified 32-bit integer.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, IntPtr lpString, int nMaxCount);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BlendFunction pblend, Int32 dwFlags);

        #endregion

        /// <summary>
        /// 判断网络连接
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet")]
        public extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        [DllImport("msimg32.dll")]
        extern public static Int32 AlphaBlend(IntPtr hdcDest, Int32 xDest, Int32 yDest, Int32 cxDest, Int32 cyDest,
            IntPtr hdcSrc, Int32 xSrc, Int32 ySrc, Int32 cxSrc, Int32 cySrc, BlendFunction blendFunction);

        

        [DllImport("message.dll", SetLastError = true)]
        public static extern IntPtr LoadImageDec(string file);


        #region coredll

        [DllImport("coredll", SetLastError = true)]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);


        [DllImport("coredll", SetLastError = true)]
        public static extern int FillRect(IntPtr hDC, ref System.Drawing.Rectangle lprc, IntPtr hbr);

        [DllImport("coredll", SetLastError = true)]
        public static extern int FillRgn(IntPtr hdc, IntPtr hrgn, IntPtr hbr);

        [DllImport("coredll", SetLastError = true)]
        public static extern bool Polygon(IntPtr hdc, ref POINT[] lpPoints, int nCount);



        #endregion




    }
    

}
