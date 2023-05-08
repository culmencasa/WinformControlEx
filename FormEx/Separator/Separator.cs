using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class Separator : Control
    {
        public enum SeparationDirections
        { 
            Horizontal,
            Vertical
        }

        Color _lineColor = Color.FromArgb(184, 183, 188);

        SeparationDirections _direction = SeparationDirections.Horizontal;

        public Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            this.Size = new Size(120, 10);
            this.BackColor = Color.Transparent;
        }


        [Category("Custom")]
        [DefaultValue(typeof(Color), "184, 183, 188")]
        public Color LineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                _lineColor = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue(SeparationDirections.Horizontal)]
        public SeparationDirections Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                if (value == SeparationDirections.Horizontal)
                {
                    Size = new Size(120, 10);
                }
                else
                {
                    Size = new Size(10, 120);
                }
                Invalidate();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (Direction)
            {
                case SeparationDirections.Horizontal:
                    e.Graphics.DrawLine(new Pen(LineColor), 0 + Padding.Left, 5, Width - Padding.Left - Padding.Right, 5);
                    break;
                case SeparationDirections.Vertical:
                    e.Graphics.DrawLine(new Pen(LineColor), 5, 0 + Padding.Top, 5, Height - Padding.Top - Padding.Bottom);
                    break;
            }
        }
    }

}
