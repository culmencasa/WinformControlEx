using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.Canvas
{
    public class TextObject : CanvasObject
    {
        public TextObject()
        {
            FocusColor = Color.Gray;
        }
        public Color FocusColor
        {
            get;
            set;
        }



        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            Render();
        }

        internal override void OnParentChanged()
        {
            base.OnParentChanged();

            // 如果没有指定宽高，则根据文字内容自动计算宽高
            if (Canvas != null && this.Width == 0 && this.Height == 0)
            {
                this.Width = 100;
                var size = CalculateTextSize(); 
                this.Width = size.Width;
                this.Height = size.Height;
            }
        }


        internal override void DrawContent(Graphics g)
        {
            if (!string.IsNullOrEmpty(Text) && ForeColor != Color.Empty)
            {
                var fontBrush = Canvas.GetCachedBrush(this.ForeColor);


                // 测量文本的实际大小  
                //var textSize = CalculateTextSize();
                //if (textSize.Height > this.Height)
                //{
                //    g.DrawString(this.Text, this.Font, fontBrush, Bounds, StringFormat.GenericTypographic);

                //}
                //else
                //{
                //    g.DrawStringWrap(this.Text, this.Font, fontBrush, Bounds.ToRectangle());
                //}



                g.DrawString(this.Text, this.Font, fontBrush, Bounds);

                if (HighlightState)
                {
                    var focusPen = Canvas.GetCachedPen(FocusColor, 1, Drawing.Drawing2D.DashStyle.Dot);
                    g.DrawRectangle(focusPen, Bounds);
                }
            }
        }

        private SizeF CalculateTextSize()
        {
            using (var g = this.Canvas.CreateGraphics())
            { 
                return g.MeasureString(this.Text, this.Font);
            }
        }
    }
}
