using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FormExCore.ThirdParty
{
    /// <summary>
    /// https://github.com/mganss/BorderlessForm
    /// </summary>
    public class BorderlessForm : Form
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // 好像没什么用
            //if (!DesignMode)
            //{
            //    SetWindowRegion(Handle, 0, 0, Width, Height);
            //}
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);



        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {

                #region   无边框处理

                case (int)WindowMessages.WM_NCCALCSIZE:
                    {
                        // Provides new coordinates for the window client area.
                        WmNCCalcSize(ref m);
                        break;
                    }

                case (int)WindowMessages.WM_NCPAINT:
                    {
                        // Here should all our painting occur, but...
                        WmNCPaint(ref m);
                        break;
                    }
                case (int)WindowMessages.WM_NCACTIVATE:
                    {
                        // ... WM_NCACTIVATE does some painting directly 
                        // without bothering with WM_NCPAINT ...
                        WmNCActivate(ref m);
                        break;
                    }

                #endregion

                //case (int)WindowMessages.WM_SETTEXT:
                //        {
                //            // ... and some painting is required in here as well
                //            //WmSetText(ref m);
                //            break;
                //        }
                case (int)WindowMessages.WM_WINDOWPOSCHANGED:
                    {
                        WmWindowPosChanged(ref m);

                        AfterPositionChanged?.Invoke();
                        break;
                    }
                case 174: // ignore magic message number
                    {
                        break;
                    }
                default:
                    {
                        base.WndProc(ref m);
                        break;
                    }
            }
        }





        public FormWindowState MinMaxState
        {
            get
            {
                var s = Win32.GetWindowLong(Handle, Win32.GWL_STYLE);
                var max = (s & (int)WindowStyle.WS_MAXIMIZE) > 0;
                if (max) return FormWindowState.Maximized;
                var min = (s & (int)WindowStyle.WS_MINIMIZE) > 0;
                if (min) return FormWindowState.Minimized;
                return FormWindowState.Normal;
            }
        }

        private void WmNCCalcSize(ref Message m)
        {
            // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowmessages/wm_nccalcsize.asp
            // http://groups.google.pl/groups?selm=OnRNaGfDEHA.1600%40tk2msftngp13.phx.gbl

            var r = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));
            var max = MinMaxState == FormWindowState.Maximized;

            if (max)
            {
                var x = Win32.GetSystemMetrics(Win32.SM_CXSIZEFRAME);
                var y = Win32.GetSystemMetrics(Win32.SM_CYSIZEFRAME);
                var p = Win32.GetSystemMetrics(Win32.SM_CXPADDEDBORDER);
                var w = x + p;
                var h = y + p;

                r.left += w;
                r.top += h;
                r.right -= w;
                r.bottom -= h;

                var appBarData = new APPBARDATA();
                appBarData.cbSize = Marshal.SizeOf(typeof(APPBARDATA));
                var autohide = (Win32.SHAppBarMessage(Win32.ABM_GETSTATE, ref appBarData) & Win32.ABS_AUTOHIDE) != 0;
                if (autohide) r.bottom -= 1;

                Marshal.StructureToPtr(r, m.LParam, true);
            }

            m.Result = IntPtr.Zero;
        }

        private void WmNCActivate(ref Message msg)
        {
            // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowmessages/wm_ncactivate.asp

            bool active = (msg.WParam == new IntPtr(1));

            if (MinMaxState == FormWindowState.Minimized)
                DefWndProc(ref msg);
            else
            {
                // repaint title bar
                //PaintNonClientArea(msg.HWnd, (IntPtr)1);

                // allow to deactivate window
                msg.Result = new IntPtr(1);
            }
        }


        private void WmSetText(ref Message msg)
        {
            // allow the system to receive the new window title
            DefWndProc(ref msg);

            // repaint title bar
            //PaintNonClientArea(msg.HWnd, (IntPtr)1);
        }
        private void WmNCPaint(ref Message msg)
        {
            // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/pantdraw_8gdw.asp
            // example in q. 2.9 on http://www.syncfusion.com/FAQ/WindowsForms/FAQ_c41c.aspx#q1026q

            // The WParam contains handle to clipRegion or 1 if entire window should be repainted
            //PaintNonClientArea(msg.HWnd, (IntPtr)msg.WParam);

            // we handled everything
            msg.Result = new IntPtr(1);
        }



        private void SetWindowRegion(IntPtr hwnd, int left, int top, int right, int bottom)
        {
            var rgn = CreateRectRgn(0, 0, 0, 0);
            var hrg = new HandleRef((object)this, rgn);
            var r = Win32.GetWindowRgn(hwnd, hrg.Handle);
            Win32.RECT box;
            Win32.GetRgnBox(hrg.Handle, out box);
            if (box.left != left || box.top != top || box.right != right || box.bottom != bottom)
            {
                var hr = new HandleRef((object)this, CreateRectRgn(left, top, right, bottom));
                Win32.SetWindowRgn(hwnd, hr.Handle, Win32.IsWindowVisible(hwnd));
            }
            Win32.DeleteObject(rgn);
        }

        private void WmWindowPosChanged(ref Message m)
        {
            DefWndProc(ref m);
            UpdateBounds();
            WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));

            SetWindowRegion(m.HWnd, 0, 0, pos.cx, pos.cy);

            m.Result = new IntPtr(1);
        }

        public event Action AfterPositionChanged;

    }
}
