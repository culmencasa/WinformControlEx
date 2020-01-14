
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D; 

namespace System.Windows.Forms
{
    public partial class RoundButton : Button
    {
        #region 字段


        private int _diameter;
        private Image _imageEnter;
        private Image _imageNormal;

        #endregion

        #region 属性

        /// <summary>
        /// 圆形按钮的半径属性
        /// </summary>
        [CategoryAttribute("布局"), BrowsableAttribute(true), ReadOnlyAttribute(false)]
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

        [CategoryAttribute("外观"), BrowsableAttribute(true), ReadOnlyAttribute(false)]  
        public Image ImageEnter
        {
            get 
            {
                return _imageEnter;
            }
            set
            {
                _imageEnter = value;
            }
        }

        [CategoryAttribute("外观"), BrowsableAttribute(true), ReadOnlyAttribute(false)]  
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


        [BrowsableAttribute(false)]
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

        [BrowsableAttribute(false)]
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
        public int OutlineWidth { get; set; }
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color OutlineColor { get; set; }

        public bool MouseOver { get; set; }

        #endregion

        #region 构造

        public RoundButton()
        {
            Diameter = 50;
            OutlineWidth = 4;
            OutlineColor = Color.FromArgb(139,216,254);

            this.Height = this.Width = Diameter;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }


        #endregion

        #region 重写的成员

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

        }

        //重写OnPaint
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, Diameter, Diameter);
            this.Region = new Region(path);

            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.Clear(Color.White);


            DrawImage(g);

            DrawBorder(g);

            //base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Height!=Diameter)
            {
                Diameter=Width = Height;
            }
            else if (Width!=Diameter)
            {
                Diameter = Height = Width;
            }
            
        }

        
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseOver = true;
            Invalidate();
        } 
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseOver = false;
            Invalidate();
            
        }

        #endregion

        protected virtual void DrawImage(Graphics g)
        {
            if (!MouseOver)
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

        protected virtual void DrawBorder(Graphics g)
        {
            // 外框
            //using (Pen p = new Pen(Color.Transparent, 2))
            //{
            //    g.DrawPath(p, path);
            //}
            // 在距离边界（Region）一定距离开始画圆以避开边界锯齿
            //if (Padding != Padding.Empty)
            //{
            //    GraphicsPath outlinePath = new GraphicsPath();
            //    outlinePath.AddEllipse(Padding.Left, Padding.Top, Width - Padding.Left * 2, Height - Padding.Top * 2);
            //    using (Pen p = new Pen(Color.White, 2))
            //    {
            //        g.DrawPath(p, outlinePath);
            //    }
            //}

            if (MouseOver)
            {
                GraphicsPath outlinePath2 = new GraphicsPath();
                outlinePath2.AddEllipse(0, 0, Diameter + 1, Diameter + 1);
                using (Pen p = new Pen(Color.FromArgb(232, 255, 255), OutlineWidth))
                {
                    g.DrawPath(p, outlinePath2);
                }
            }
            else
            {
                GraphicsPath outlinePath2 = new GraphicsPath();
                outlinePath2.AddEllipse(0, 0, Diameter + 1, Diameter + 1);
                using (Pen p = new Pen(OutlineColor, OutlineWidth))
                {
                    //g.FillPath(Brushes.White, outlinePath2);
                    g.DrawPath(p, outlinePath2);
                }
            }
        }
    }
}
