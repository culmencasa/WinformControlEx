using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{ 
    public class ThreeDLine : Control
    {
        #region 构造函数

        public ThreeDLine()
        { 
            this.DoubleBuffered = true; 

        }

        #endregion

        #region 字段

        private Color _color1 = SystemColors.ControlDark;
        private Color _color2 = SystemColors.ControlLightLight;
        private Size _size = new Size(80, 2);

        #endregion

        #region 属性

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "ControlDark")]
        public Color Color1
        {
            get => _color1;
            set => _color1 = value;
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "ControlLightLight")]
        public Color Color2
        {
            get => _color2;
            set => _color2 = value;
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Size), "80,2")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = new Size(value.Width, 2);
                base.Size = _size;
                this.Invalidate();
            }
        }


        #endregion

        #region 重写方法

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _size = new Size(this.Width, 2);
            this.Height = 2;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Pen pen1 = new Pen(Color1, 1))
            using (Pen pen2 = new Pen(Color2, 1))
            {
                e.Graphics.DrawLine(pen1, 0, 0, this.Width, 0);
                e.Graphics.DrawLine(pen2, 0, 1, this.Width, 1);
            }

            base.OnPaint(e);

        }

        #endregion
    }
}
