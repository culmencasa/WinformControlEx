using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils.UI
{
    public class WindowWrapper : IWin32Window
    {
        private IntPtr _handle;

        public WindowWrapper(IntPtr handle)
        {
            _handle = handle;
        }

        public IntPtr Handle
        {
            get { return _handle; }
        }
    }
}
