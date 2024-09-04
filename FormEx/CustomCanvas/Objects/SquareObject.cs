using System.Drawing;

namespace System.Windows.Forms.Canvas
{

    public class SquareObject : CanvasObject
    {
        #region 字段


        private Color _borderColor;


        #endregion

        #region 属性

        /// <summary>
        /// 边框色
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                Render();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color FocusColor
        {
            get;
            set;
        }


        /// <summary>
        /// 正常色(背景)
        /// </summary>
        public Color NormalColor
        {
            get;
            set;
        }

        /// <summary>
        /// 强调色(背景)
        /// </summary>
        public Color HighLightColor
        {
            get;
            set;
        }

        #endregion

        #region 构造

        public SquareObject()
        {
            // default colors
            _borderColor = Color.Black;
            NormalColor = Color.LightGray;
            HighLightColor = Color.Orange; 
            FocusColor = Color.Blue;


            BackColor = NormalColor;
        }

        #endregion

        #region CanvasElement抽象方法实现

        internal override void DrawContent(Graphics g)
        {
            var bgBrush = Canvas.GetCachedBrush(BackColor);
            g.FillRectangle(bgBrush, this.Bounds);


            if (HighlightState)
            {
                var borderPen = Canvas.GetCachedPen(FocusColor, 1);
                g.DrawRectangle(borderPen, Left, Top, Width, Height);
            }
            else
            {
                var borderPen = Canvas.GetCachedPen(BorderColor, 1);
                g.DrawRectangle(borderPen, Left, Top, Width, Height);
            }
        }


        protected override void OnHighlighStateChanged()
        {
            base.OnHighlighStateChanged();
            if (HighlightState)
            {
                BackColor = HighLightColor;
            }
            else
            {
                BackColor = NormalColor;
            }

            Render();
        }

        #endregion

        #region 公开方法

        #endregion
    }
}
