﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public class TransparentControl : Control
    {
        private Color brushColor = Color.Transparent;

        private int opacity = 100;

        //private Timer delay;

        public Color BrushColor
        {
            get
            {
                return this.brushColor;
            }
            set
            {
                this.brushColor = value;
                base.RecreateHandle();
            }
        }

        public int Opacity
        {
            get
            {
                if (this.opacity > 100)
                {
                    this.opacity = 100;
                }
                else if (this.opacity < 0)
                {
                    this.opacity = 0;
                }
                return this.opacity;
            }
            set
            {
                this.opacity = value;
                base.RecreateHandle();
                Invalidate();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                CreateParams expr_08 = createParams;
                expr_08.ExStyle |= 32; // 设置扩展样式，使当前控件能够以分层窗口的方式创建
                return createParams;
            }
        }

        public TransparentControl()
        {
            //this.delay = new Timer();
            //this.delay.Interval = 50;
            //this.delay.Tick += new EventHandler(this.TimerOnTick);
            //this.delay.Enabled = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
             
        //    Color colorWithOpacity = Color.FromArgb((int)(opacity * 2.55), brushColor);
        //    using (SolidBrush brush = new SolidBrush(colorWithOpacity))
        //    {
        //        e.Graphics.FillRectangle(brush, ClientRectangle);
        //    }
        //}


        protected override void OnMove(EventArgs e)
        {
            base.RecreateHandle();
        }

        private void TimerOnTick(object source, EventArgs e)
        {
            base.RecreateHandle();
            //this.delay.Stop();
        }
    }
}
