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


        protected virtual Rectangle GetImageRectangle(Graphics g)
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
                left = 0;
                top = (this.Height - height) / 2;

                var parentForm = FormManager.GetTopForm(this.Parent);
                if (parentForm is IDpiDefined)
                {
                    var dpiForm = parentForm as IDpiDefined;
                    width = (int)(width * dpiForm.ScaleFactorRatioX);
                    height =(int)(height * dpiForm.ScaleFactorRatioY);
                    top = (this.Height - height) / 2;
                }

                if (AlignCenter)
                {
                    SizeF textSize = g.MeasureString(Text, Font);
                    left = (this.Width - width - (int)textSize.Width - marginToLeftSide * 2) / 2; 
                }
                else
                {
                    left = marginToLeftSide;
                }
            }
            else
            {
                width = 0;
                height = this.Height;
                top = 0;
                left = 0;

                var parentForm = FormManager.GetTopForm(this.Parent);
                if (parentForm is IDpiDefined)
                {
                    var dpiForm = parentForm as IDpiDefined;
                    width = (int)(width * dpiForm.ScaleFactorRatioX);
                    height = (int)(height * dpiForm.ScaleFactorRatioY);
                    top = (this.Height - height) / 2;
                }
            }

            iconRectangle = new Rectangle(left, top, width, height);

            return iconRectangle;
        }

        protected virtual Rectangle GetTextRectangle(Graphics g)
        {
            int width = 0;
            int height = 0;
            int left = 0;
            int top = 0;

            int textMarginToImage = 6;

            Rectangle titleRectangle = Rectangle.Empty;
            Rectangle iconRectangle = GetImageRectangle(g);

            // 字体大小
            SizeF textSize = new SizeF();
            if (Text?.Length > 0)
            {
                textSize = g.MeasureString(Text, Font);
            }

            // 整体标题区域大小
            width = (int)Math.Floor(textSize.Width) + 2;
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
                g.DrawImage(Image, GetImageRectangle(g));
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
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    g.DrawString(Text, Font, sb, GetTextRectangle(g));
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
