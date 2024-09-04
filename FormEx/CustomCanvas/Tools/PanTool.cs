using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.Canvas
{
    internal class PanTool
    {
        CustomCanvas Canvas
        {
            get;
            set;
        }

        public PanTool(CustomCanvas canvas)
        { 
            Canvas = canvas;
        }
    }
}
