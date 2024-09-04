using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms.Canvas
{
    public class CanvasSaveInfo
    {
        public List<CanvasObject> Elements
        {
            get;
            set;
        }

        public float CenterX
        {
            get;
            set;
        }

        public float CenterY
        {
            get;
            set;
        }

        public float PaperWidth
        {
            get;
            set;
        }

        public float PaperHeight
        { 
            get;
            set;
        }

        public float Zoom
        {
            get;
            set;
        }

        public PointF Offset
        {
            get;
            set;
        } 
    }
     
}
