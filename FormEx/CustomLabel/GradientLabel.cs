using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class GradientLabel : Label
    {
        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color FirstColor { get; set; }

        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color SecondColor { get; set; }

        [DefaultValue(typeof(Color), "Color.Empty")]
        public Color BorderColor { get; set; }

        [DefaultValue(typeof(FillDirection), "TopToBottom")]
        public FillDirection GradientDirection { get; set; }

        public GradientLabel()
        {
            this.GradientDirection = FillDirection.TopToBottom;
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            if (this.FirstColor != Color.Empty && this.SecondColor != Color.Empty)
            {
                GradientFill.Fill(g, this.ClientRectangle, this.FirstColor, this.SecondColor, this.GradientDirection);
            }

            if (this.BorderColor != Color.Empty)
            {
                using (Pen borderPen = new Pen(this.BorderColor))
                {
                    g.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            
        }
    }
}
