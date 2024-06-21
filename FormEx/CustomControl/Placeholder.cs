using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class Placeholder : NonFlickerUserControl
    {

        /// <summary>
        /// 是否点击穿透
        /// </summary>
        [Category("Custom")]
        public bool ClickThrough
        {
            get;
            set;
        }


        protected override void WndProc(ref Message m)
        {
            if (!DesignMode && ClickThrough)
            {
                if (m.Msg == Win32.WM_NCHITTEST)
                {
                    m.Result = new IntPtr(Win32.HTTRANSPARENT);
                    return;
                }
            }

            base.WndProc(ref m);
        }


    }
}
