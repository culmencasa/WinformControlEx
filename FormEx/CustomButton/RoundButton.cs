
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [DefaultEvent("Click")]
    public partial class RoundButton : NonFlickerUserControl
    {
        #region 字段


        private int _diameter;
        private Image _imageEnter;
        private Image _imageNormal;

        private int _outlineWidth;
        private Color _outlineColor;
        private Color _buttonColor;

        #endregion

        #region 属性

        /// <summary>
        /// 圆形按钮的半径属性
        /// </summary>
        [Category("Custom"), Browsable(true), ReadOnly(false)]
        public int Diameter
        {
            get
            {
                return _diameter;
            }
            set
            {
                _diameter = value;
                //this.Height = this.Width = Diameter;

                base.Size = new Size(value, value);
            }
        }

        [Category("Custom"), Browsable(true), ReadOnly(false)]
        public Image ImageEnter
        {
            get
            {
                return _imageEnter;
            }
            set
            {
                _imageEnter = value;
                Invalidate();
            }
        }

        [Category("Custom"), Browsable(true), ReadOnly(false)]
        public Image ImageNormal
        {
            get
            {
                return _imageNormal;
            }
            set
            {
                _imageNormal = value;

                base.BackgroundImage = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 覆盖掉这个属性
        /// </summary>
        [Category("Custom")]
        [Browsable(false)]
        public new Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
            }
        }

        [Category("Custom")]
        [Browsable(false)]
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
            }
        }

        /// <summary>
        /// 边框大小
        /// </summary>
        [Category("Custom")]
        public int OutlineWidth { get { return _outlineWidth; } set { _outlineWidth = value; Invalidate(); } }
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Category("Custom")]
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; Invalidate(); }
        }

        [Category("Custom")]
        public Color OutlineHoverColor { get; set; }

        [Category("Custom")]
        public Color ButtonColor
        {
            get
            {
                return _buttonColor;
            }
            set
            {
                _buttonColor = value;
                Invalidate();

            }
        }


        [Category("Custom")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                Invalidate();
            }
        }

        protected bool MouseHovering { get; set; }

        #endregion

        #region 构造

        public RoundButton()
        {
            Diameter = 50;
            OutlineWidth = 2;
            OutlineColor = Color.FromArgb(139, 216, 254);
            OutlineHoverColor = Color.LightBlue;
            ButtonColor = Color.Blue;

            this.Height = this.Width = Diameter;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            this.Font = new Font("Arial", 10, FontStyle.Bold);
            this.ForeColor = Color.White;
            this.Text = "1";
        }


        #endregion

        #region 重写的成员

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);



        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        //重写OnPaint
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // 不使用region, 锯齿太多了
            //GraphicsPath circlePath = new GraphicsPath();
            //circlePath.AddEllipse(0, 0, Diameter, Diameter);
            //this.Region = new Region(circlePath);

            Graphics g = e.Graphics;
            g.SetSlowRendering();
            //g.Clear(BackColor);

            Rectangle outerRect = new System.Drawing.Rectangle(Padding.Left, Padding.Top, Width - Padding.Left * 2, Height - Padding.Top * 2);
            Rectangle borderRect = System.Drawing.Rectangle.Inflate(outerRect, -OutlineWidth / 2, -OutlineWidth / 2);


            // 1. 画底色
            using (SolidBrush bgBrush = new SolidBrush(ButtonColor))
            using (GraphicsPath outerPath = new GraphicsPath())
            {
                outerPath.AddEllipse(outerRect);
                g.FillPath(bgBrush, outerPath);
            }

            // 2. 铺图形
            DrawImageIfItHasBeenSet(g);

            // 3. 画文字
            DrawText(g);

            // 4. 画边框
            using (GraphicsPath borderPath = new GraphicsPath())
            {
                borderPath.AddEllipse(borderRect);
                DrawBorder(g, borderPath);
            }

        }


        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (Height != Diameter)
            {
                Diameter = Width = Height;
            }
            else if (Width != Diameter)
            {
                Diameter = Height = Width;
            }
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseHovering = true;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseHovering = false;
            Invalidate();

        }

        #endregion

        #region 私有方法

        protected virtual void DrawImageIfItHasBeenSet(Graphics g)
        {
            if (!MouseHovering)
            {
                if (base.BackgroundImage != null)
                    g.DrawImage(base.BackgroundImage, this.ClientRectangle);
                else if (ImageNormal != null)
                    g.DrawImage(ImageNormal, this.ClientRectangle);
            }
            else
            {
                if (ImageEnter != null)
                    g.DrawImage(ImageEnter, this.ClientRectangle);
            }
        }

        protected virtual void DrawBorder(Graphics g, GraphicsPath path)
        {
            using (Pen p = new Pen(MouseHovering ? OutlineHoverColor : OutlineColor, OutlineWidth))
            {
                g.DrawPath(p, path);
            }
        }

        protected virtual void DrawText(Graphics g)
        {
            using (SolidBrush brush = new SolidBrush(ForeColor))
            {
                SizeF stringSize = g.MeasureString(this.Text, this.Font);
                int posX = Convert.ToInt32((Width - stringSize.Width) / 2);
                int posY = Convert.ToInt32((Height - stringSize.Height) / 2);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.Character;

                RectangleF fontRectanle = new RectangleF(posX + 1, posY + 1, stringSize.Width, stringSize.Height);
                g.DrawString(this.Text, this.Font, brush, fontRectanle, sf);
            }
        }


        #endregion
    }
}
