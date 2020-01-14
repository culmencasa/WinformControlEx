using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class QQRadioButton : RadioButton
    {
        
        private static readonly ContentAlignment RightAlignment = ContentAlignment.TopRight | ContentAlignment.BottomRight | ContentAlignment.MiddleRight;
        private static readonly ContentAlignment LeftAlignment = ContentAlignment.TopLeft | ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft;


        #region 字段

        private ImageList imageList1;
        private IContainer components;
        private QQControlStates _state = QQControlStates.Normal;
        private Image _radioImage;

        #endregion

        #region 属性

        public Image RadioImage
        {
            get { return _radioImage; }
            set { _radioImage = value; }
        }

        public Color BorderColor { get; set; }

        public Color HighlightInnerColor { get; set; }

        public Color HighlightColor { get; set; }

        [Description("获取QQRadioButton左边正方形的宽度")]
        protected virtual int CheckRectWidth
        {
            get { return 12; }
        }


        #endregion

        #region 构造

        public QQRadioButton()
        {
            SetStyles();
            InitializeComponent();
            this.BackColor = Color.Transparent;
            this.BorderColor = ColorTable.QQBorderColor;
            this.HighlightColor = ColorTable.QQHighlightColor;
            this.HighlightInnerColor = ColorTable.QQHighlightInnerColor;
            _radioImage = this.imageList1.Images[0];
        }

        #endregion

        #region 设计器生成的代码

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QQRadioButton));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "dot.png");
            this.ResumeLayout(false);

        }

        #endregion

        #region 重写的成员

        protected override void OnMouseEnter(EventArgs e)
        {
            _state = QQControlStates.Highlight;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _state = QQControlStates.Normal;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _state = QQControlStates.Down;
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                if (ClientRectangle.Contains(mevent.Location))
                {
                    _state = QQControlStates.Highlight;
                }
                else
                {
                    _state = QQControlStates.Normal;
                }
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
            {
                _state = QQControlStates.Normal;
            }
            else
            {
                _state = QQControlStates.Disabled;
            }
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            base.OnPaintBackground(pevent);

            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle circleRect, textRect;
            CalculateRect(out circleRect, out textRect);

            if (Enabled == false)
            {
                _state = QQControlStates.Disabled;
            }

            switch (_state)
            {
                case QQControlStates.Highlight:
                case QQControlStates.Down:
                    DrawHighLightCircle(g, circleRect);
                    break;
                case QQControlStates.Disabled:
                    DrawDisabledCircle(g, circleRect);
                    break;
                default:
                    DrawNormalCircle(g, circleRect);
                    break;
            }

            Color textColor = (Enabled == true) ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_radioImage != null)
                {
                    _radioImage.Dispose();
                }
            }

            _radioImage = null;
            base.Dispose(disposing);
        }

        #endregion

        #region 私有方法

        private void DrawNormalCircle(Graphics g, Rectangle circleRect)
        {
            g.FillEllipse(Brushes.White, circleRect);
            using (Pen borderPen = new Pen(this.BorderColor))
            {
                g.DrawEllipse(borderPen, circleRect);
            }
            if (Checked)
            {
                circleRect.Inflate(-2, -2);
                g.DrawImage(
                    _radioImage,
                    new Rectangle(circleRect.X + 1, circleRect.Y + 1, circleRect.Width - 1, circleRect.Height - 1),
                    0,
                    0,
                    _radioImage.Width,
                    _radioImage.Height,
                    GraphicsUnit.Pixel);
            }
        }

        private void DrawHighLightCircle(Graphics g, Rectangle circleRect)
        {
            DrawNormalCircle(g, circleRect);
            using (Pen p = new Pen(HighlightInnerColor))
            {
                g.DrawEllipse(p, circleRect);

                circleRect.Inflate(1, 1);
                p.Color = this.HighlightColor;

                g.DrawEllipse(p, circleRect);
            }


        }

        private void DrawDisabledCircle(Graphics g, Rectangle circleRect)
        {
            g.DrawEllipse(SystemPens.ControlDark, circleRect);
            if (Checked)
            {
                circleRect.Inflate(-2, -2);
                g.DrawImage(
                    _radioImage,
                    new Rectangle(circleRect.X + 1, circleRect.Y + 1, circleRect.Width - 1, circleRect.Height - 1),
                    0,
                    0,
                    _radioImage.Width,
                    _radioImage.Height,
                    GraphicsUnit.Pixel);
            }
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        private void CalculateRect(out Rectangle circleRect, out Rectangle textRect)
        {
            circleRect = new Rectangle(
                0, 0, CheckRectWidth, CheckRectWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAlignment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = (RightToLeft == RightToLeft.Yes);

            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        circleRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                circleRect.X = 1;

                textRect = new Rectangle(
                    circleRect.Right + 2,
                    0,
                    Width - circleRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        circleRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                circleRect.X = Width - CheckRectWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - CheckRectWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        circleRect.Y = 2;
                        textRect.Y = circleRect.Bottom + 2;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                }

                circleRect.X = (Width - CheckRectWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        private static TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }


        #endregion


    }
}
