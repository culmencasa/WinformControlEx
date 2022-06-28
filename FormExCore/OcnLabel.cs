using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormExCore
{
    public class OcnLabel : Label
    {
        public OcnLabel()
        {
            this.ClientSizeChanged += OcnLabel_ClientSizeChanged;
        }

        private void OcnLabel_ClientSizeChanged(object? sender, EventArgs e)
        {
            this.ClientSizeChanged -= OcnLabel_ClientSizeChanged;
            if (DesignMode && AutoSize)
            {
                AutoSize = false;
                TextAlign = ContentAlignment.MiddleLeft;
                Size = new Size(130, 25);
            }
        }

        [Category("Custom")]
        public new Image Image
        {
            get
            {
                return base.Image;
            }
            set
            {
                base.Image = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        public int IconSize
        {
            get;
            set;
        } = 16;

        [Category("Custom")]
        public bool AlignCenter
        {
            get;
            set;
        }


        [Browsable(false)]
        public override ContentAlignment TextAlign { get => base.TextAlign; set => base.TextAlign = value; }


        protected virtual Rectangle GetImageRectangle()
        {
            int width = 0;
            int height = 0;
            int left = 0;
            int top = 0;

            int marginToLeftSide = 6;

            Rectangle iconRectangle = Rectangle.Empty;

            if (Image != null && Image.Width > 0 && Image.Height > 0)
            {
                width = IconSize;
                height = IconSize;
                left = marginToLeftSide;
                top = (this.Height - height) / 2;
                
                if (AlignCenter)
                {
                    SizeF textSize = TextRenderer.MeasureText(Text, Font);
                    left = (this.Width - IconSize - (int)textSize.Width - marginToLeftSide * 2) / 2;

                }
                iconRectangle = new Rectangle(left, top, width, height);
            }
            else
            {
                width = 0;
                height = this.Height;
                top = 0;
                left = 0;
                iconRectangle = new Rectangle(left, top, width, height);
            }


            return iconRectangle;
        }

        protected virtual Rectangle GetTextRectangle()
        {
            int width = 0;
            int height = 0;
            int left = 0;
            int top = 0;

            int textMarginToImage = 6;

            Rectangle titleRectangle = Rectangle.Empty;
            Rectangle iconRectangle = GetImageRectangle();

            // 字体大小
            SizeF textSize = new SizeF();
            if (Text?.Length > 0)
            {
                textSize = TextRenderer.MeasureText(Text, Font);
            }

            // 整体标题区域大小
            width = (int)textSize.Width;
            height = (int)textSize.Height;
            left = iconRectangle.Right + textMarginToImage;
            top = (this.Height - height) / 2;

            titleRectangle = new Rectangle(left, top, width, height);
            return titleRectangle;
        }

        protected virtual void DrawImage(Graphics g)
        {
            if (Image != null)
            {
                g.DrawImage(Image, GetImageRectangle());
            }

        }

        protected virtual void DrawText(Graphics g)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                using (SolidBrush sb = new SolidBrush(ForeColor))
                {
                    
                    //StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
                    //sf.Alignment = StringAlignment.Center;
                    //sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(Text, Font, sb, GetTextRectangle());
                }
            }
        }

        public new event PaintEventHandler Paint;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawImage(g);
            DrawText(g);

            Paint?.Invoke(this, e);
        }
    }
}
