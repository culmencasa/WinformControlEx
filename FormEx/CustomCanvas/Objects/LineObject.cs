using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace System.Windows.Forms.Canvas
{
    public class LineObject : CanvasObject
    {
        public LineObject()
        {
            Width = 100;
            Height = 1;
            StartPoint = new PointF(0, 0);
            EndPoint = new PointF(Width, 0);
        }

        public override float Left
        {
            get => base.Left;
            set
            {
                base.Left = value;
                UpdatePoints();
            }
        }


        public override float Top
        {
            get => base.Top;
            set
            {
                base.Top = value;
                UpdatePoints();
            }
        } 

        public override float Width 
        { 
            get => base.Width;
            set
            {
                base.Width = value;
                UpdatePoints();
            }
        }

        public override float Height
        {
            get => base.Height;
            set
            {
                base.Height = value; 
            }
        }

        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }

        private void UpdatePoints()
        {
            StartPoint = new PointF(Left, Top + Height / 2); // StartPoint位于(Left, Top + Height / 2)
            EndPoint = new PointF(Left + Width, Top + Height / 2); // EndPoint始终在水平线上
        }
        internal override void DrawContent(Graphics g)
        {
            g.DrawLine(Pens.Black, StartPoint, EndPoint);
        }
    }
}
