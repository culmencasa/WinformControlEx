using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    public partial class Win32
    {

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
