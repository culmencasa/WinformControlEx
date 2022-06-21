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
        [DllImport("shell32.dll")]
        public static extern int SHAppBarMessage(uint dwMessage, [In] ref APPBARDATA pData);

    }
}
