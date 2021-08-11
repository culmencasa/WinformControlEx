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

    }
}
