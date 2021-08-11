using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{

    public class ClickThroughPanel : Panel
    {
        public ClickThroughPanel()
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
            if (!DesignMode)
            { 
                if (m.Msg == Win32.WM_NCHITTEST)
                {
                    m.Result = new IntPtr(Win32.HTTRANSPARENT);
                    return;
                }
            }

            base.WndProc(ref m);
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            //if (!DesignMode)
            //{
            //    this.SendToBack();
            //}
        }
    }
}
