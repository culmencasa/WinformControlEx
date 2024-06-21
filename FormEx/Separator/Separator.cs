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
    /// 分隔线控件
    /// </summary>
    public class Separator : Control
    {
        #region 枚举

        public enum SeparationDirections
        { 
            Horizontal,
            Vertical
        }

        #endregion

        #region 字段

        Color _lineColor = Color.FromArgb(184, 183, 188); 
        SeparationDirections _direction = SeparationDirections.Horizontal;
        DashStyle _penStyle = DashStyle.Solid;
        int _penWeight = 1;

        #endregion

        #region 构造

        public Separator()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            this.Size = new Size(120, 10);
            this.BackColor = Color.Transparent;
        }

        #endregion

        #region 属性

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

        [Category("Custom")]
        [DefaultValue(DashStyle.Solid)]
        public DashStyle PenStyle
        {
            get
            {
                return _penStyle;
            }
            set
            {
                _penStyle = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue(1)]
        public int PenWeight
        {
            get
            {
                return _penWeight;
            }
            set
            {
                _penWeight = value;
                Invalidate();
            }
        }

        #endregion

        #region 重写的方法

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            switch (Direction)
            {
                case SeparationDirections.Horizontal:
                    using (Pen pen = new Pen(LineColor, PenWeight))
                    {
                        pen.DashStyle = PenStyle; 
                        e.Graphics.DrawLine(pen, 0 + Padding.Left, 5, Width - Padding.Left - Padding.Right, 5);
                    }    
                    break;
                case SeparationDirections.Vertical:
                    using (Pen pen = new Pen(LineColor, PenWeight))
                    {
                        pen.DashStyle = PenStyle;
                        e.Graphics.DrawLine(pen, 5, 0 + Padding.Top, 5, Height - Padding.Top - Padding.Bottom);
                    }
                    break;
            }
        }


        #endregion
    }

}
