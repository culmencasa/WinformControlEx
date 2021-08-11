using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Drawing
{

    // update-layered-window
    public enum ULWPara
    {
        ULW_COLORKEY = 0x00000001,
        ULW_ALPHA = 0x00000002,
        ULW_OPAQUE = 0x00000004,
        ULW_EX_NORESIZE = 0x00000008,
    }

    public enum BlendOp : byte
    {
        AC_SRC_OVER = 0x00,
        AC_SRC_ALPHA = 0x01,
    }

    public enum WindowMessages
    {
        WM_MOVE = 0x0003,
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,

        WM_ACTIVATEAPP = 0x001C,

        WM_SETCURSOR = 0x0020,
        WM_MOUSEACTIVATE = 0x0021,
        WM_GETMINMAXINFO = 0x24,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_WINDOWPOSCHANGED = 0x0047,

        // non client area
        WM_NCCREATE = 0x0081,
        WM_NCDESTROY = 0x0082,
        WM_NCCALCSIZE = 0x0083,
        WM_NCHITTEST = 0x84,
        WM_NCPAINT = 0x0085,
        WM_NCACTIVATE = 0x0086,

        // non client mouse
        WM_NCMOUSEMOVE = 0x00A0,
        WM_NCLBUTTONDOWN = 0x00A1,
        WM_NCLBUTTONUP = 0x00A2,
        WM_NCLBUTTONDBLCLK = 0x00A3,
        WM_NCRBUTTONDOWN = 0x00A4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_NCRBUTTONDBLCLK = 0x00A6,
        WM_NCMBUTTONDOWN = 0x00A7,
        WM_NCMBUTTONUP = 0x00A8,
        WM_NCMBUTTONDBLCLK = 0x00A9,

        WM_SYSCOMMAND = 0x0112,
        WM_PARENTNOTIFY = 0x0210,

        WM_MDINEXT = 0x224,
    }

    /// <summary>
    /// Location of cursor hot spot returnet in WM_NCHITTEST.
    /// </summary>
    public enum NCHITTEST
    {
        /// <summary>
        /// On the screen background or on a dividing line between windows 
        /// (same as HTNOWHERE, except that the DefWindowProc function produces a system beep to indicate an error).
        /// </summary>
        HTERROR = (-2),
        /// <summary>
        /// In a window currently covered by another window in the same thread 
        /// (the message will be sent to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).
        /// </summary>
        HTTRANSPARENT = (-1),
        /// <summary>
        /// On the screen background or on a dividing line between windows.
        /// </summary>
        HTNOWHERE = 0,
        /// <summary>In a client area.</summary>
        HTCLIENT = 1,
        /// <summary>In a title bar.</summary>
        HTCAPTION = 2,
        /// <summary>In a window menu or in a Close button in a child window.</summary>
        HTSYSMENU = 3,
        /// <summary>In a size box (same as HTSIZE).</summary>
        HTGROWBOX = 4,
        /// <summary>In a menu.</summary>
        HTMENU = 5,
        /// <summary>In a horizontal scroll bar.</summary>
        HTHSCROLL = 6,
        /// <summary>In the vertical scroll bar.</summary>
        HTVSCROLL = 7,
        /// <summary>In a Minimize button.</summary>
        HTMINBUTTON = 8,
        /// <summary>In a Maximize button.</summary>
        HTMAXBUTTON = 9,
        /// <summary>In the left border of a resizable window 
        /// (the user can click the mouse to resize the window horizontally).</summary>
        HTLEFT = 10,
        /// <summary>
        /// In the right border of a resizable window 
        /// (the user can click the mouse to resize the window horizontally).
        /// </summary>
        HTRIGHT = 11,
        /// <summary>In the upper-horizontal border of a window.</summary>
        HTTOP = 12,
        /// <summary>In the upper-left corner of a window border.</summary>
        HTTOPLEFT = 13,
        /// <summary>In the upper-right corner of a window border.</summary>
        HTTOPRIGHT = 14,
        /// <summary>	In the lower-horizontal border of a resizable window 
        /// (the user can click the mouse to resize the window vertically).</summary>
        HTBOTTOM = 15,
        /// <summary>In the lower-left corner of a border of a resizable window 
        /// (the user can click the mouse to resize the window diagonally).</summary>
        HTBOTTOMLEFT = 16,
        /// <summary>	In the lower-right corner of a border of a resizable window 
        /// (the user can click the mouse to resize the window diagonally).</summary>
        HTBOTTOMRIGHT = 17,
        /// <summary>In the border of a window that does not have a sizing border.</summary>
        HTBORDER = 18,

        HTOBJECT = 19,
        /// <summary>In a Close button.</summary>
        HTCLOSE = 20,
        /// <summary>In a Help button.</summary>
        HTHELP = 21,
    }



    // 以下枚举来自 Windows Mobile 5.0 Pocket PC SDK 的 wingdi.h 文件
    public enum BlendOperation : byte
    {
        AC_SRC_OVER = 0x00
    }

    public enum BlendFlags : byte
    {
        Zero = 0x00
    }

    public enum SourceConstantAlpha : byte
    {
        Transparent = 0x00,
        Opaque = 0xFF
    }

    public enum AlphaFormat : byte
    {
        AC_SRC_ALPHA = 0x01
    }

    public enum TernaryRasterOperations : uint
    {
        /// <summary>dest = source</summary>
        SRCCOPY = 0x00CC0020,
        /// <summary>dest = source OR dest</summary>
        SRCPAINT = 0x00EE0086,
        /// <summary>dest = source AND dest</summary>
        SRCAND = 0x008800C6,
        /// <summary>dest = source XOR dest</summary>
        SRCINVERT = 0x00660046,
        /// <summary>dest = source AND (NOT dest)</summary>
        SRCERASE = 0x00440328,
        /// <summary>dest = (NOT source)</summary>
        NOTSRCCOPY = 0x00330008,
        /// <summary>dest = (NOT src) AND (NOT dest)</summary>
        NOTSRCERASE = 0x001100A6,
        /// <summary>dest = (source AND pattern)</summary>
        MERGECOPY = 0x00C000CA,
        /// <summary>dest = (NOT source) OR dest</summary>
        MERGEPAINT = 0x00BB0226,
        /// <summary>dest = pattern</summary>
        PATCOPY = 0x00F00021,
        /// <summary>dest = DPSnoo</summary>
        PATPAINT = 0x00FB0A09,
        /// <summary>dest = pattern XOR dest</summary>
        PATINVERT = 0x005A0049,
        /// <summary>dest = (NOT dest)</summary>
        DSTINVERT = 0x00550009,
        /// <summary>dest = BLACK</summary>
        BLACKNESS = 0x00000042,
        /// <summary>dest = WHITE</summary>
        WHITENESS = 0x00FF0062
    }

}
