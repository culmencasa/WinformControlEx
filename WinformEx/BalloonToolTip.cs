using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    /// <remarks>
    /// This classes implements the balloon tooltip.
    /// </remarks>
    public class BalloonToolTip : IDisposable
    {
        #region Unmanaged shit

        [DllImport("user32.dll")]
        static extern IntPtr CreateWindowEx(int exstyle, string classname, string windowtitle,
            int style, int x, int y, int width, int height, IntPtr parent,
            int menu, int nullvalue, int nullptr);

        [DllImport("user32.dll")]
        static extern int DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        const int WS_POPUP = unchecked((int)0x80000000);
        const int TTS_BALLOON = 0x40;
        const int TTS_NOPREFIX = 0x02;
        const int TTS_ALWAYSTIP = 0x01;

        const int CW_USEDEFAULT = unchecked((int)0x80000000);

        const int WM_USER = 0x0400;

        IntPtr HWND_TOPMOST = new IntPtr(-1);

        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOACTIVATE = 0x0010;

        const int WS_EX_TOPMOST = 0x00000008;

        [StructLayout(LayoutKind.Sequential)]
        public struct TOOLINFO
        {
            public int cbSize;
            public int uFlags;
            public IntPtr hwnd;
            public IntPtr id;
            private RECT m_rect;
            public IntPtr nullvalue;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string text;
            public uint param;

            public Rectangle rect
            {
                get
                {
                    return new Rectangle(m_rect.left, m_rect.top, m_rect.right - m_rect.left, m_rect.bottom - m_rect.top);
                }
                set
                {
                    m_rect.left = value.Left;
                    m_rect.top = value.Top;
                    m_rect.right = value.Right;
                    m_rect.bottom = value.Bottom;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        const int TTF_TRANSPARENT = 0x0100;
        const int TTF_TRACK = 0x0020;
        const int TTF_ABSOLUTE = 0x0080;

        #endregion

        #region Managed wrapper over some tooltip messages.
        // The most useful part of the wrappers in this section is their documentation.

        /// <summary>
        /// Send TTM_SETTITLE message to the tooltip.
        /// TODO [very low]: implement the custom icon, if needed. 
        /// </summary>
        /// <param name="_wndTooltip">HWND of the tooltip</param>
        /// <param name="_icon">Standard icon</param>
        /// <param name="_title">The title string</param>
        internal static int SetTitle(IntPtr _wndTooltip, ToolTipIcon _icon, string _title)
        {
            const int TTM_SETTITLE = WM_USER + 33;

            var tempptr = IntPtr.Zero;
            try
            {
                tempptr = Marshal.StringToHGlobalUni(_title);
                return SendMessage(_wndTooltip, TTM_SETTITLE, (int)_icon, tempptr);
            }
            finally
            {
                if (IntPtr.Zero != tempptr)
                {
                    Marshal.FreeHGlobal(tempptr);
                    tempptr = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Send a message that wants LPTOOLINFO as the lParam
        /// </summary>
        /// <param name="_wndTooltip">HWND of the tooltip.</param>
        /// <param name="_msg">window message to send</param>
        /// <param name="_wParam">wParam value</param>
        /// <param name="_ti">TOOLINFO structure that goes to the lParam field. The cbSize field must be set.</param>
        internal static int SendToolInfoMessage(IntPtr _wndTooltip, int _msg, int _wParam, TOOLINFO _ti)
        {
            var tempptr = IntPtr.Zero;
            try
            {
                tempptr = Marshal.AllocHGlobal(_ti.cbSize);
                Marshal.StructureToPtr(_ti, tempptr, false);

                return SendMessage(_wndTooltip, _msg, _wParam, tempptr);
            }
            finally
            {
                if (IntPtr.Zero != tempptr)
                {
                    Marshal.FreeHGlobal(tempptr);
                    tempptr = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Registers a tool with a ToolTip control
        /// </summary>
        /// <param name="_wndTooltip">HWND of the tooltip</param>
        /// <param name="_ti">TOOLINFO structure containing information that the ToolTip control needs to display text for the tool.</param>
        /// <returns>Returns true if successful.</returns>
        internal static bool AddTool(IntPtr _wndTooltip, TOOLINFO _ti)
        {
            const int TTM_ADDTOOL = WM_USER + 50;
            int res = SendToolInfoMessage(_wndTooltip, TTM_ADDTOOL, 0, _ti);
            return Convert.ToBoolean(res);
        }

        /// <summary>
        /// Registers a tool with a ToolTip control
        /// </summary>
        /// <param name="_wndTooltip">HWND of the tooltip</param>
        /// <param name="_ti">TOOLINFO structure containing information that the ToolTip control needs to display text for the tool.</param>
        internal static void DelTool(IntPtr _wndTooltip, TOOLINFO _ti)
        {
            const int TTM_DELTOOL = WM_USER + 51;
            SendToolInfoMessage(_wndTooltip, TTM_DELTOOL, 0, _ti);
        }

        internal static void UpdateTipText(IntPtr _wndTooltip, TOOLINFO _ti)
        {
            const int TTM_UPDATETIPTEXT = WM_USER + 57;
            SendToolInfoMessage(_wndTooltip, TTM_UPDATETIPTEXT, 0, _ti);
        }

        /// <summary>
        /// Activates or deactivates a tracking ToolTip
        /// </summary>
        /// <param name="_wndTooltip">HWND of the tooltip</param>
        /// <param name="bActivate">Value specifying whether tracking is being activated or deactivated</param>
        /// <param name="_ti">Pointer to a TOOLINFO structure that identifies the tool to which this message applies</param>
        internal static void TrackActivate(IntPtr _wndTooltip, bool bActivate, TOOLINFO _ti)
        {
            const int TTM_TRACKACTIVATE = WM_USER + 17;
            SendToolInfoMessage(_wndTooltip, TTM_TRACKACTIVATE, Convert.ToInt32(bActivate), _ti);
        }

        /// <summary>
        /// returns (LPARAM) MAKELONG( pt.X, pt.Y )
        /// </summary>
        internal static IntPtr makeLParam(Point pt)
        {
            int res = (pt.X & 0xFFFF);
            res |= ((pt.Y & 0xFFFF) << 16);
            return new IntPtr(res);
        }

        /// <summary>
        /// Sets the position of a tracking ToolTip
        /// <summary>
        /// <param name="_wndTooltip">HWND of the tooltip.</param>
        /// <param name="pt">The point at which the tracking ToolTip will be displayed, in screen coordinates.</param>
        internal static void TrackPosition(IntPtr _wndTooltip, Point pt)
        {
            const int TTM_TRACKPOSITION = WM_USER + 18;
            SendMessage(_wndTooltip, TTM_TRACKPOSITION, 0, makeLParam(pt));
        }

        /// <summary>
        /// Sets the maximum width for a ToolTip window
        /// </summary>
        /// <param name="_wndTooltip">HWND of the tooltip.</param>
        /// <param name="pxWidth">Maximum ToolTip window width, or -1 to allow any width</param>
        /// <returns>the previous maximum ToolTip width</returns>
        internal static int SetMaxTipWidth(IntPtr _wndTooltip, int pxWidth)
        {
            const int TTM_SETMAXTIPWIDTH = WM_USER + 24;
            return SendMessage(_wndTooltip, TTM_SETMAXTIPWIDTH, 0, new IntPtr(pxWidth));
        }

        #endregion

        /// <summary>
        /// Sets the information that a ToolTip control maintains for a tool (not currently used).
        /// </summary>
        /// <param name="act"></param>
        internal void AlterToolInfo(Action<TOOLINFO> act)
        {
            const int TTM_GETTOOLINFO = WM_USER + 53;
            const int TTM_SETTOOLINFO = WM_USER + 54;

            var tempptr = IntPtr.Zero;
            try
            {
                tempptr = Marshal.AllocHGlobal(m_toolinfo.cbSize);
                Marshal.StructureToPtr(m_toolinfo, tempptr, false);

                SendMessage(m_wndToolTip, TTM_GETTOOLINFO, 0, tempptr);

                m_toolinfo = (TOOLINFO)Marshal.PtrToStructure(tempptr, typeof(TOOLINFO));

                act(m_toolinfo);

                Marshal.StructureToPtr(m_toolinfo, tempptr, false);

                SendMessage(m_wndToolTip, TTM_SETTOOLINFO, 0, tempptr);
            }
            finally
            {
                Marshal.FreeHGlobal(tempptr);
            }
        }

        readonly Control m_ownerControl;

        // The ToolTip's HWND
        IntPtr m_wndToolTip = IntPtr.Zero;

        /// <summary>
        /// The maximum width for a ToolTip window.
        /// If a ToolTip string exceeds the maximum width, the control breaks the text into multiple lines.
        /// </summary>
        int m_pxMaxWidth = 200;

        TOOLINFO m_toolinfo = new TOOLINFO();

        public BalloonToolTip(Control owner)
        {
            m_ownerControl = owner;

            // See http://msdn.microsoft.com/en-us/library/bb760252(VS.85).aspx#tooltip_sample_rect
            m_toolinfo.cbSize = Marshal.SizeOf(typeof(TOOLINFO));
            m_toolinfo.uFlags = TTF_TRANSPARENT | TTF_TRACK;
            m_toolinfo.hwnd = m_ownerControl.Handle;
            m_toolinfo.rect = m_ownerControl.Bounds;
        }

        /// <summary>Throws an exception if there's no alive WIN32 window.</summary>
        void VerifyControlIsAlive()
        {
            if (IntPtr.Zero == m_wndToolTip)
                throw new ApplicationException("The control is not created, or is already destroyed.");
        }

        /// <summary>Create the balloon window.</summary>
        public void Create()
        {
            if (IntPtr.Zero != m_wndToolTip)
                Destroy();

            // Create the tooltip control.
            m_wndToolTip = CreateWindowEx(WS_EX_TOPMOST, "tooltips_class32",
                string.Empty,
                WS_POPUP | TTS_BALLOON | TTS_NOPREFIX | TTS_ALWAYSTIP,
                CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
                m_ownerControl.Handle, 0, 0, 0);

            if (IntPtr.Zero == m_wndToolTip)
                throw new Win32Exception();

            if (!SetWindowPos(m_wndToolTip, HWND_TOPMOST,
                        0, 0, 0, 0,
                        SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE))
                throw new Win32Exception();

            SetMaxTipWidth(m_wndToolTip, m_pxMaxWidth);
        }

        bool m_bVisible = false;

        /// <summary>return true if the balloon is currently visible.</summary>
        public bool bVisible
        {
            get
            {
                if (IntPtr.Zero == m_wndToolTip)
                    return false;
                return m_bVisible;
            }
        }

        /// <summary>Balloon title.
        /// The balloon will only use the new value on the next Show() operation.</summary>
        public string strTitle;

        /// <summary>Balloon title icon.
        /// The balloon will only use the new value on the next Show() operation.</summary>
        public ToolTipIcon icon;

        /// <summary>Balloon title icon.
        /// The new value is updated immediately.</summary>
        public string strText
        {
            get { return m_toolinfo.text; }
            set
            {
                m_toolinfo.text = value;
                if (bVisible)
                    UpdateTipText(m_wndToolTip, m_toolinfo);
            }
        }

        /// <summary>Show the balloon.</summary>
        /// <param name="pt">The balloon stem position, in the owner's client coordinates.</param>
        public void Show(Point pt)
        {
            VerifyControlIsAlive();

            if (m_bVisible) Hide();

            // http://www.deez.info/sengelha/2008/06/12/balloon-tooltips/
            if (!AddTool(m_wndToolTip, m_toolinfo))
                throw new ApplicationException("Unable to register the tooltip");

            SetTitle(m_wndToolTip, icon, strTitle);

            TrackPosition(m_wndToolTip, m_ownerControl.PointToScreen(pt));

            TrackActivate(m_wndToolTip, true, m_toolinfo);

            m_bVisible = true;
        }

        /// <summary>Hide the balloon if it's visible.
        /// If the balloon was previously hidden, this method does nothing.</summary>
        public void Hide()
        {
            VerifyControlIsAlive();

            if (!m_bVisible) return;

            TrackActivate(m_wndToolTip, false, m_toolinfo);

            DelTool(m_wndToolTip, m_toolinfo);

            m_bVisible = false;
        }

        /// <summary>Destroy the balloon.</summary>
        public void Destroy()
        {
            if (IntPtr.Zero == m_wndToolTip)
                return;
            if (m_bVisible) Hide();

            DestroyWindow(m_wndToolTip);
            m_wndToolTip = IntPtr.Zero;
        }

        void IDisposable.Dispose()
        {
            Destroy();
        }
    }
}