using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinformEx
{
    /// <summary>
    /// 自定义圆角控件基类
    /// </summary>
    public partial class RoundUserControl : NonFlickerUserControl
    {
        public RoundUserControl()
        {
            InitializeComponent();
            base.BackColor = Color.Transparent;
            this.ShowBorder = true;
            this.BorderColor = Color.Gray;
            this.BorderWidth = 1;
            this.HoverBorderColor = Color.FromArgb(72, 191, 249);
        }

        private Color _backColor = SystemColors.Control;
        private Color _borderColor = Color.Gray;
        private int _diameter = 8;
        private int _borderWidth = 1;
        
        private bool _isMouseHovering;

        public Color HoverBorderColor { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public new Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                base.BackColor = Color.Transparent;
                _backColor = value;
                this.Refresh();
            }
        }

        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        public bool ShowBorder { get; set; }

        public int BorderWidth
        {
            get
            {
                return _borderWidth;
            }
            set
            {
                _borderWidth = value;
                Invalidate();
            }
        }
        /// <summary>
        /// 圆角直径
        /// </summary>
        public int Diameter
        {
            get
            {
                return _diameter;
            }
            set
            {
                _diameter = value;
                this.Invalidate();
            }
        }

        protected SolidBrush BorderBrush { get; set; }
        protected Pen BorderPen { get; set; }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle innerRect = GetBorderRect();

            // bug: 设计模式下还是无法同步更新背景色
            BorderBrush = new SolidBrush(BackColor);
            BorderPen = new Pen(BorderColor, BorderWidth);

            if (Diameter > 0)
            {
                g.FillRoundedRectangle(BorderBrush, innerRect, Diameter);
            }
            else
            {
                g.FillRectangle(BorderBrush, innerRect);
            }

            if (ShowBorder)
            {
                if (_isMouseHovering && HoverBorderColor != Color.Empty)
                {
                    using (Pen hoverBorderPen = new Pen(HoverBorderColor, BorderWidth))
                    {
                        if (Diameter > 0)
                        {
                            g.DrawRoundedRectangle(hoverBorderPen, innerRect, Diameter);
                        }
                        else
                        {
                            g.DrawRectangle(hoverBorderPen, innerRect);
                        }
                    }
                }
                else
                {
                    if (Diameter > 0)
                    {
                        g.DrawRoundedRectangle(BorderPen, innerRect, Diameter);
                    }
                    else
                    {
                        g.DrawRectangle(BorderPen, innerRect);
                    }
                }
            }

            base.OnPaint(e);

            if (BorderBrush != null)
                BorderBrush.Dispose();
            if (BorderPen != null)
                BorderPen.Dispose();
        }

        protected virtual Rectangle GetBorderRect()
        {
            return new Rectangle(0, 0, this.Width - 1, this.Height - 1);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isMouseHovering = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isMouseHovering = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }


    }
}
