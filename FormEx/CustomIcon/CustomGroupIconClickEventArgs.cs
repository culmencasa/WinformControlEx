using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public class CustomGroupIconClickEventArgs : EventArgs
    {
        public bool Cancel { get; set; }

        public TileIcon LastSelection { get; set; }

        public TileIcon NewSelection { get; set; }
    }
}
