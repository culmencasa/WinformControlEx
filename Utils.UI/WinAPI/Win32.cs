using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace System
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


    }
    

}
