using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{

    public class QQCheckBox : CheckBox
    {
        #region Field

        private QQControlStates _state = QQControlStates.Normal;
        private Image _checkImg;

        private static readonly ContentAlignment RightAlignment = ContentAlignment.TopRight | ContentAlignment.BottomRight | ContentAlignment.MiddleRight;
        private ImageList imageList1;
        private IContainer components;
        private static readonly ContentAlignment LeftAlignment = ContentAlignment.TopLeft | ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft;

        #endregion

        #region Constructor

        public QQCheckBox()
        {
            SetStyles();

            this.InitializeComponent();

            this.BackColor = Color.Transparent;
            this.BorderColor = ColorTable.QQBorderColor;
            this.HighlightColor = ColorTable.QQHighlightColor;
            this.HighlightInnerColor = ColorTable.QQHighlightInnerColor;
            _checkImg = imageList1.Images[0];
        }

        #endregion

        #region Properites

        public Color BorderColor { get; set; }

        public Color HighlightInnerColor { get; set; }

        public Color HighlightColor { get; set; }

        [Description("获取QQCheckBox左边正方形的宽度")]
        protected virtual int CheckRectWidth
        {
            get { return 13; }
        }

        #endregion

        #region Override

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

            Rectangle checkRect, textRect;
            CalculateRect(out checkRect, out textRect);

            if (Enabled == false)
            {
                _state = QQControlStates.Disabled;
            }

            switch (_state)
            {
                case QQControlStates.Highlight:
                case QQControlStates.Down:
                    DrawHighLightCheckRect(g, checkRect);
                    break;
                case QQControlStates.Disabled:
                    DrawDisabledCheckRect(g, checkRect);
                    break;
                default:
                    DrawNormalCheckRect(g, checkRect);
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
                if (_checkImg != null)
                {
                    _checkImg.Dispose();
                }

            }

            _checkImg = null;

            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void DrawNormalCheckRect(Graphics g, Rectangle checkRect)
        {
            g.FillRectangle(Brushes.White, checkRect);
            using (Pen borderPen = new Pen(this.BorderColor))
            {
                g.DrawRectangle(borderPen, checkRect);
            }
            if (Checked)
            {
                g.DrawImage(_checkImg, checkRect, 0, 0, _checkImg.Width, _checkImg.Height, GraphicsUnit.Pixel);
            }
        }

        private void DrawHighLightCheckRect(Graphics g, Rectangle checkRect)
        {
            DrawNormalCheckRect(g, checkRect);
            using (Pen p = new Pen(this.HighlightInnerColor))
            {
                g.DrawRectangle(p, checkRect);

                checkRect.Inflate(1, 1);
                p.Color = this.HighlightColor;

                g.DrawRectangle(p, checkRect);
            }


        }

        private void DrawDisabledCheckRect(Graphics g, Rectangle checkRect)
        {
            g.DrawRectangle(SystemPens.ControlDark, checkRect);
            if (Checked)
            {
                g.DrawImage(_checkImg, checkRect, 0, 0, _checkImg.Width, _checkImg.Height, GraphicsUnit.Pixel);
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

        private void CalculateRect(out Rectangle checkRect, out Rectangle textRect)
        {
            checkRect = new Rectangle(
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
                        checkRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                checkRect.X = 1;

                textRect = new Rectangle(
                    checkRect.Right + 2,
                    0,
                    Width - checkRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        checkRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                checkRect.X = Width - CheckRectWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - CheckRectWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        checkRect.Y = 2;
                        textRect.Y = checkRect.Bottom + 2;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                }

                checkRect.X = (Width - CheckRectWidth) / 2;

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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QQCheckBox));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "check.png");
            this.ResumeLayout(false);

        }
    }
}
