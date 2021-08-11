using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    /// <summary>
    /// 平铺样式的图标
    /// </summary>
    public class TileIcon : NonFlickerUserControl
    {
        #region 字段

        protected Image _image;
        protected Image _defaultImage;
        protected bool _defaultImageDefined;
        protected Color _lastBackColor;
        protected Color _activedBackColor;

        #endregion

        #region 构造

        public TileIcon()
        {
            _activedBackColor = Color.DodgerBlue;

            this.ShowImage = true;
            this.ShowSplitter = true;
            this.Padding = new Padding(5);
            this.Margin = new Padding(0);
            this.BackColor = Color.FromArgb(248, 248, 248);
            this.MouseEnter += new EventHandler(TileIcon_MouseEnter);
            this.MouseLeave += new EventHandler(TileIcon_MouseLeave);
        }

        protected virtual void TileIcon_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = _lastBackColor;
            Invalidate();
        }

        protected virtual void TileIcon_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
            _lastBackColor = this.BackColor;
            this.BackColor = _activedBackColor;
            Invalidate();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        #endregion

        #region 属性

        [Category("样式")]
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        [Category("样式")]
        public Drawing.Image DefaultImage
        {
            get
            {
                return _defaultImage;
            }
            set
            {
                _defaultImage = value;
                
                _defaultImageDefined = (_defaultImage != null);
            }
        }
        [Category("样式")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Boolean), "True")]
        public bool ShowImage
        {
            get;
            set;
        }

        [DefaultValue(typeof(Boolean), "True")]
        public bool ShowSplitter { get; set; }

        #endregion

        #region 私有方法
        
        #endregion

        #region 传承的方法

        protected virtual void DrawDefaultImage()
        {
            Rectangle imgRect = GetImageArea();
            _defaultImage = new Bitmap(imgRect.Width, imgRect.Height);
            using (Graphics g = Graphics.FromImage(_defaultImage))
            {
                //g.DrawGradientRoundedRectangle(new Rectangle(0, 0, imgRect.Width, imgRect.Height), Color.White, Color.White, Color.WhiteSmoke, new Size(3, 3), FillDirection.TopToBottom);

                g.DrawGradientRoundedRectangle(
                    new Rectangle(0, 0, imgRect.Width, imgRect.Height),
                    Color.White,
                    Color.WhiteSmoke,
                    FillDirection.TopToBottom,
                    Color.White,
                    3);
            }
        }

        protected virtual void DrawImage(Graphics g)
        {
            Image img = this.Image == null ? this.DefaultImage : this.Image;
            if (img != null)
            {
                Rectangle rect = this.GetImageArea();
                g.DrawImage(img, rect);
            }
        }

        protected virtual void DrawImageBorder(Graphics g)
        {
            Rectangle rect = this.GetImageArea();
            //rect.Offset(-1, -1);
            g.DrawRectangle(Pens.LightGray, rect);
            rect.Inflate(-1, -1);
            g.DrawRectangle(Pens.White, rect);
        }

        protected virtual void DrawText(Graphics g)
        { 
                using (Brush textBrush = new SolidBrush(this.ForeColor))
                using (StringFormat textFormat = new StringFormat())
                {
                    textFormat.LineAlignment = StringAlignment.Center;

                    int x, y, width, height;
                    x = this.Padding.Left + (this.Height - this.Padding.Top - this.Padding.Bottom) + this.Padding.Left;
                    if (!ShowImage)
                    {
                        x = this.Padding.Left;
                    }
                    y  = 0;
                    width = this.Width - this.Padding.Right - x;
                    height  = this.Height;
                    Rectangle textArea = new Rectangle(x, y, width, height);
                    g.DrawString(this.Text, this.Font, textBrush, textArea, textFormat);
                }
        }

        protected virtual void DrawSplitter(Graphics g)
        {
            Pen borderPen = new Pen(this.Parent.BackColor, 2f);
            g.DrawRectangle(borderPen, new Rectangle(0, 0, this.Width, this.Height));
            borderPen.Dispose();
        }

        protected virtual Rectangle GetImageArea()
        {
            int x, y, width, height;
            x = this.Padding.Left;
            y = this.Padding.Top;
            width = this.Height - this.Padding.Bottom - this.Padding.Top;
            height = width;

            return new Rectangle(x, y, width, height);
        }

        #endregion

        #region 重写的方法

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //using (Bitmap bmp = new Bitmap(this.Width, this.Height, Drawing.Imaging.PixelFormat.Format32bppArgb))
            //using(Graphics g = Graphics.FromImage(bmp))
            //{
            //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //    g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //    if (ShowImage)
            //    {
            //        DrawImage(g);
            //        DrawImageBorder(g);
            //    }

            //    DrawText(g);

            //    if (ShowSplitter)
            //        DrawSplitter(g);

            //    e.Graphics.DrawImage(bmp, 0, 0);
            //}

            Graphics g = e.Graphics;
            if (ShowImage)
            {
                DrawImage(g);
                DrawImageBorder(g);
            }

            DrawText(g);

            if (ShowSplitter)
                DrawSplitter(g);


            base.OnPaint(e);

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        #endregion

        #region 公开方法

        public TileIcon Clone()
        {
            return this.MemberwiseClone() as TileIcon;
        }

        #endregion


    }
}
