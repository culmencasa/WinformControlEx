using System.Drawing;

namespace System.Windows.Forms.Canvas
{
    public class RectangleObject : CanvasObject
    {
        public RectangleObject(int widthMM, int heightMM)
        {
            var dpiX = Graphics.FromHwnd(IntPtr.Zero).DpiX;
            var dpiY = Graphics.FromHwnd(IntPtr.Zero).DpiY;

            var pixelNumerPerMM = dpiX / 25.4f;

            Width = (pixelNumerPerMM * widthMM);
            Height = (pixelNumerPerMM * heightMM);

            AllowMove = false;
            AllowResize = false;
            AllowSelect = false;
            ZIndex = 0;

            BorderColor = Color.Gray;
            FocusColor = Color.DodgerBlue;
        }

        public override int ZIndex 
        { 
            get => 0; 
            set { }
        }


        /// <summary>
        /// 边框色
        /// </summary>
        public Color BorderColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Color FocusColor
        {
            get;
            set;
        }

        internal override void DrawContent(Graphics g)
        {
            g.FillRoundedRectangle(Brushes.White, Left, Top, Width, Height, 10);
            if (HighlightState)
            {
                using (var pen = new Pen(FocusColor, 1))
                {
                    g.DrawRoundedRectangle(pen, Left, Top, Width, Height, 10);
                }
            }
            else
            {
                using (var pen = new Pen(BorderColor, 1))
                {
                    g.DrawRoundedRectangle(pen, Left, Top, Width, Height, 10);
                }
            }
        }
    }
}
