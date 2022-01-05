using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class FormWindowStateArgs : EventArgs
    {
        public FormWindowState LastWindowState { get; set; }

        public FormWindowState NewWindowState { get; set; }
    }
}
