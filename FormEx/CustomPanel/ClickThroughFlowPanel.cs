using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace System.Windows.Forms
{
    public class ClickThroughFlowPanel : FlowLayoutPanel
    {
        public ClickThroughFlowPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        protected override void WndProc(ref Message m)
        {
            // Do not allow this window to become active - it should be "transparent" 
            // to mouse clicks i.e. Mouse clicks pass through this window
            if (m.Msg == Win32.WM_NCHITTEST)
            {
                m.Result = new IntPtr(Win32.HTTRANSPARENT);
                return;
            }

            base.WndProc(ref m);
        }
    }
}
