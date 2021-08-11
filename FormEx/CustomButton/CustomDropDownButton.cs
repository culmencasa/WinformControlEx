using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    /// <summary>
    /// 下拉按钮类
    /// </summary>
    public class CustomDropDownButton : CustomButton
    {
        public float BorderWidth { get; set; }

        public new Color BorderColor { get; set; }

        public CustomDropDownButton()
        {
            this.Size = new Size(20, 20);
            this.ForeColor = Color.FromArgb(122, 162, 187);
            this.BackColor = Color.FromArgb(237, 248, 254);
            this.BorderColor = Color.White;
            this.BorderWidth = 1;

        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.Clear(this.BackColor);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawDropDownButton(g);
            DrawBorder(g);
        }



        /// <summary>
        /// 画下拉按钮
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawDropDownButton(Graphics g)
        {
            using (Brush bgBrush = new SolidBrush(this.BackColor))
            {
                g.FillRectangle(bgBrush, this.ClientRectangle);
            }

            using (GraphicsPath path = GraphicsExtension.Create7x4In7x7DownTriangleFlag(this.ClientRectangle))
            {
                using (Pen p = new Pen(this.ForeColor))
                {
                    g.DrawPath(p, path);
                }
            }
        }

        public virtual void DrawBorder(Graphics g)
        {
            using (Pen p = new Pen(this.BorderColor))
            {
                g.DrawRectangle(p, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
    }

}
