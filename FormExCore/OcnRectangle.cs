using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinformEx;
using System.ComponentModel;

namespace FormExCore
{
    public class OcnRectangle : RoundUserControl
    {
        public OcnRectangle()
        {
        }


        /// <summary>
        /// 是否点击穿透
        /// </summary>
        [Category("Custom")]
        public bool ClickThrough
        {
            get;
            set;
        }


        protected override void WndProc(ref Message m)
        {
            if (!DesignMode && ClickThrough)
            {
                if (m.Msg == Win32.WM_NCHITTEST)
                {
                    m.Result = new IntPtr(Win32.HTTRANSPARENT);
                    return;
                }
            }

            base.WndProc(ref m);
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics lazyG = e.Graphics;

            int w = this.Width;
            int h = this.Height;
            Bitmap bitmap;
            Graphics cacheG;
            if (w > 0 && h > 0)
            {
                bitmap = new Bitmap(w, h);
                cacheG = Graphics.FromImage(bitmap);
            }
            else
            {
                // 有时候, 例如最小化, Height会变成0
                base.OnPaint(e);
                return;
            }

            using (SolidBrush borderBrush = new SolidBrush(BackColor))
            {
                if (Diameter > 0)
                {
                    cacheG.FillRoundedRectangle(borderBrush, BorderWidth, BorderWidth, this.Width - BorderWidth * 2, this.Height - BorderWidth * 2, Diameter);
                }
                else
                {
                    cacheG.FillRectangle(borderBrush, new Rectangle(BorderWidth, BorderWidth, this.Width - BorderWidth * 2, this.Height - BorderWidth * 2));
                }
            }

            if (this.BorderColor != Color.Empty && BorderWidth > 0)
            {
                if (Diameter > 0)
                {
                    // 画圆角边框   
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        cacheG.DrawRoundedRectangle(borderPen, 0, 0, this.Width - BorderWidth, this.Height - BorderWidth, this.Diameter);
                    }
                }
                else
                {
                    // 画直角边框
                    using (Pen borderPen = new Pen(this.BorderColor, BorderWidth))
                    {
                        cacheG.DrawRectangle(borderPen, 0, 0, this.Width - BorderWidth, this.Height - BorderWidth);
                    }
                }
            }

            lazyG.DrawImage(bitmap, 0, 0);
            cacheG.Dispose();

            base.OnPaint(e);
        }

    }
}
