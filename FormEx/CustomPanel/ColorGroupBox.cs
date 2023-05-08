using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Utils.UI;

namespace System.Windows.Forms
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class ColorGroupBox : Control
    {
        public ColorGroupBox()
        {
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);

            Margin = new Padding(8);
            BackColor = Color.Transparent;
            BoxBorderColor = Color.DarkRed;
            BoxColor = Color.IndianRed;
            LineColor = Color.LightCoral;
            BorderRadius = 12;
            LayoutEngine.Layout(this, new LayoutEventArgs(this, "DisplayRectangle"));
        }


        protected override CreateParams CreateParams
        {
            //[SecurityPermission(SecurityAction.LinkDemand)]
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassName = null;
                cp.ExStyle |= 0x00010000; //WS_EX_CONTROLPARENT
                return cp;
            }
        }


        protected Padding _margin;
        protected Padding _padding;
        protected Padding _defaultMargin = new Padding(0);
        protected Padding _defaultPadding = new Padding(0);

        private int _borderRadius;
        private Color _boxBorderColor;
        private Color boxColor;
        private Color _lineColor;

        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 200);
            }
        }

        protected override Padding DefaultMargin => _defaultMargin;

        protected override Padding DefaultPadding => _defaultPadding;

        //[Browsable(false)]
        public new Padding Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
                UpdateLayout();
                Invalidate();
            }
        }


        //[Browsable(false)]
        public new Padding Padding
        {
            get
            {
                return _padding;
            }
            set
            {
                _padding = value;
                UpdateLayout();
                Invalidate();
            }
        }


        [Category("Custom")]
        [DefaultValue(12)]
        public int BorderRadius
        {
            get
            {
                return _borderRadius;
            }
            set
            {
                _borderRadius = value;
                UpdateLayout();
                Invalidate();
            }
        }

        [Category("Custom")]
        public int BorderSize
        {
            get;
            set;

        } = 2;

        protected int InnerBorderSize
        {
            get;
            set;
        } = 1;


        [Category("Custom")]
        [DefaultValue(typeof(Color), "DarkRed")]
        public Color BoxBorderColor
        {
            get
            {
                return _boxBorderColor;
            }
            set
            {
                _boxBorderColor = value;
                Invalidate();
            }

        }

        [Category("Custom")]
        [DefaultValue(typeof(Color), "IndianRed")]
        public Color BoxColor
        {
            get
            {
                return boxColor;
            }
            set
            {
                boxColor = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue(typeof(Color), "LightCoral")]
        public Color LineColor
        {
            get
            {

                return _lineColor;
            }
            set
            {
                _lineColor = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        public bool Show3DShadow
        {
            get;
            set;
        }


        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Localizable(true)]
        [Bindable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                ResetPadding();
                UpdateLayout();
                Invalidate();
            }
        }

        public enum CaptionPositions
        {
            None,
            Left,
            Top,
            Right,
            Bottom
        }

        protected CaptionPositions _captionPosition;
        public CaptionPositions CaptionPosition
        {
            get
            {
                return _captionPosition;
            }
            set
            {
                _captionPosition = value;
                ResetPadding();
                UpdateLayout();
                Invalidate();
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rc = base.DisplayRectangle;
                rc.X = Margin.Left + Padding.Left + (BorderRadius / 2);
                rc.Y = Margin.Top + Padding.Top + (BorderRadius / 2);
                rc.Width = ClientSize.Width - rc.X - (Margin.Right + Padding.Right + (BorderRadius / 2));
                rc.Height = ClientSize.Height - rc.Y - (Margin.Bottom + Padding.Bottom + (BorderRadius / 2));
                return rc;
            }
        }

        protected int GetGap()
        {
            return BorderSize * 2 + 4;
        }


        public Rectangle GetBorderRectangle()
        {
            Rectangle rc = base.DisplayRectangle;
            rc.X = Margin.Left + Padding.Left;
            rc.Y = Margin.Top + Padding.Top;
            rc.Width = ClientSize.Width - Margin.Left - Margin.Right - Padding.Left - Padding.Right;
            rc.Height = ClientSize.Height - Margin.Top - Margin.Bottom - Padding.Top - Padding.Bottom;
            return rc;
        }



        private void UpdateLayout()
        {
            LayoutEngine.Layout(this, new LayoutEventArgs(this, "DisplayRectangle"));
        }

        protected void ResetPadding()
        {

            if (!string.IsNullOrEmpty(this.Text))
            {
                using (var g = this.CreateGraphics())
                {
                    var size = TextRenderer.MeasureText(g, this.Text, this.Font);
                    switch (CaptionPosition)
                    {
                        case CaptionPositions.Left:
                            this.Padding = new Padding(size.Height, DefaultPadding.Top, DefaultPadding.Right, DefaultPadding.Bottom);
                            break;
                        case CaptionPositions.Top:
                            this.Padding = new Padding(DefaultPadding.Left, size.Height, DefaultPadding.Right, DefaultPadding.Bottom);
                            break;
                        case CaptionPositions.Right:
                            this.Padding = new Padding(DefaultPadding.Left, DefaultPadding.Top, size.Height, DefaultPadding.Bottom);
                            break;
                        case CaptionPositions.Bottom:
                            this.Padding = new Padding(DefaultPadding.Left, DefaultPadding.Top, DefaultPadding.Right, size.Height);
                            break;
                        case CaptionPositions.None:
                            this.Padding = DefaultPadding;
                            break;

                    }
                }
            }
            else
            {
                this.Padding = new Padding(0);
            }
        }


        //[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessMnemonic(char charCode)
        {
            if (IsMnemonic(charCode, Text) && CanSelect)
            {
                SelectNextControl(null, true, true, true, false);
                return true;
            }
            return false;
        }
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            Invalidate();
            UpdateLayout();
        }

        protected Color ParentBackColor
        {
            get
            {

                var parentBackColor = Color.Gray;
                if (Parent != null && Parent.BackColor != Color.Transparent)
                {
                    parentBackColor = Parent.BackColor;
                }

                return parentBackColor;
            }
        }

        protected Color _captionBackColor;

        [Category("Custom")]
        public Color CaptionBackColor
        {
            get
            {
                if (_captionBackColor.Equals(Color.Empty))
                {
                    return ParentBackColor;
                }

                return _captionBackColor;
            }
            set
            {
                _captionBackColor = value;
            }
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SetSlowRendering();

            var parentBackColor = Color.Gray;
            if (Parent != null && Parent.BackColor != Color.Transparent)
            {
                parentBackColor = Parent.BackColor;
            }
            var innerBorderColor1 = ColorEx.LightenColor(parentBackColor, 80);
            var innerBorderColor2 = ColorEx.DarkenColor(parentBackColor, 80); //ChangeColor(parentBackColor, 0.9f);


            using (Pen outterBorderPen = new Pen(BoxBorderColor, BorderSize))
            using (Brush brush = new SolidBrush(BoxColor))
            using (Brush fontBrush = new SolidBrush(ForeColor))
            using (Pen linePen = new Pen(LineColor, 1))
            {
                // 文字背景和边框
                StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                var fontSize = g.MeasureString(this.Text, this.Font);
                var fontArea = RectangleF.Empty;
                switch (CaptionPosition)
                {
                    case CaptionPositions.Left:
                        sf.FormatFlags = StringFormatFlags.DirectionVertical;
                        fontArea = new RectangleF(GetBorderRectangle().Left - fontSize.Height, GetBorderRectangle().Top + (BorderRadius / 2), fontSize.Height, fontSize.Width);
                        break;
                    case CaptionPositions.Top:
                        sf.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip;
                        fontArea = new RectangleF(Margin.Left + (BorderRadius / 2), GetBorderRectangle().Top - fontSize.Height, fontSize.Width, fontSize.Height);
                        break;
                    case CaptionPositions.Right:
                        sf.FormatFlags = StringFormatFlags.DirectionVertical;
                        fontArea = new RectangleF(GetBorderRectangle().Right, GetBorderRectangle().Top + (BorderRadius / 2), fontSize.Height, fontSize.Width);
                        break;
                    case CaptionPositions.Bottom:
                        sf.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip;
                        fontArea = new RectangleF(GetBorderRectangle().Left + (BorderRadius / 2), GetBorderRectangle().Bottom, fontSize.Width, fontSize.Height);
                        break;
                }
                if (CaptionPosition != CaptionPositions.None)
                {
                    using (Brush fontBgBrush = new SolidBrush(CaptionBackColor))
                    {
                        g.FillRectangle(fontBgBrush, fontArea);
                    }
                    using (Pen fontBorderPen = new Pen(BoxBorderColor, 1))
                    {
                        g.SetFastRendering();
                        g.DrawRectangles(fontBorderPen, new[] { fontArea });
                        g.SetSlowRendering();
                    }

                    // 画文字
                    g.DrawString(this.Text, this.Font, fontBrush, fontArea, sf);
                }


                // 画背景色
                if (Show3DShadow)
                {
                    using (Pen innerBorderPen2 = new Pen(innerBorderColor2, InnerBorderSize))
                    {
                        var innerBorder2 = GetBorderRectangle();
                        innerBorder2.Offset(1, 1);
                        g.DrawRoundedRectangle(innerBorderPen2, innerBorder2, BorderRadius);
                    }

                    g.FillRoundedRectangle(brush, GetBorderRectangle(), BorderRadius);
                    using (Pen innerBorderPen1 = new Pen(innerBorderColor1, InnerBorderSize))
                    {
                        var innerBorder1 = GetBorderRectangle();
                        innerBorder1.Offset(1, 1);
                        innerBorder1.Inflate(-1, -1);
                        g.DrawRoundedRectangle(innerBorderPen1, innerBorder1, BorderRadius);
                    }
                }
                else
                {

                    g.FillRoundedRectangle(brush, GetBorderRectangle(), BorderRadius);
                }

                // 画边框
                if (BorderSize > 0)
                {
                    g.DrawRoundedRectangle(outterBorderPen, GetBorderRectangle(), BorderRadius);
                }


                //var innerArea = DisplayRectangle;
                //g.DrawLine(linePen, innerArea.Left, innerArea.Top, innerArea.Right, innerArea.Top);
            }


            base.OnPaint(e);
        }

        public static Color ChangeColor(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            if (red < 0) red = 0;

            if (red > 255) red = 255;

            if (green < 0) green = 0;

            if (green > 255) green = 255;

            if (blue < 0) blue = 0;

            if (blue > 255) blue = 255;



            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
    }
}
