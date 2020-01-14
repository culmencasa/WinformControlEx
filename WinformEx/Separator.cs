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
        public Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            this.Size = new Size(120, 10);
            this.BackColor = Color.Transparent;
        }

        private Color _lineColor = Color.FromArgb(184, 183, 188);

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
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawLine(new Pen(LineColor), 0, 5, Width, 5);
        }
    }

}
