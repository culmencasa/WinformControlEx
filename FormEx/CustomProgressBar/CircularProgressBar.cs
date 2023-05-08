using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public partial class CircularProgressBar : Control
    {

        #region 字段


        private float _value = 0;
        private float _maximum = 100;
        private float _progressWidth;
        private int _startAngle = 140;
        private int _sweepAngle = 260;
        private string _textLine1 = "标题";
        private string _textLine2 = "主体内容";
        private string _textLine3 = "附注";
        private Font _textFont1 = new Font("Arial", 14F, FontStyle.Regular);
        private Font _textFont2 = new Font("Arial", 20F, FontStyle.Bold);
        private Font _textFont3 = new Font("Arial", 12F, FontStyle.Regular);
        private Color _textColor1 = Color.FromArgb(40, 40, 40);
        private Color _textColor2 = Color.Black;
        private Color _textColor3 = Color.DimGray;
        private Color _progressBeginColor = Color.SteelBlue;
        private Color _progressEndColor = Color.MediumPurple;
        private Color _progressBackColor = Color.DimGray;


        #endregion

        #region 属性

        [Category("Custom")]
        [Description("进度条的最大值")]
        public float Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
            }
        }
          
        [Category("Custom")]
        [Description("进度条的当前值")]
        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (value >= Maximum)
                { 
                    _value = Maximum;
                }
                Invalidate();
            }
        }


        [Category("Custom")]
        [Description("进度条的当前值")]
        public Color ProgressBackColor
        {
            get
            {
                return _progressBackColor;
            }
            set
            {
                _progressBackColor = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        [DefaultValue(140)]
        [Description("设置圆弧的起始角度. 范围0-360.")]
        public int StartAngle
        {
            get
            {
                return _startAngle;
            }
            set
            {
                _startAngle = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue(260)]
        [Description("设置圆弧的终止角度. 范围0-360.")]
        public int SweepAngle
        {
            get
            {
                return _sweepAngle;
            }
            set
            {
                _sweepAngle = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        public string TextLine1
        {
            get
            {
                return _textLine1;
            }
            set
            {
                _textLine1 = value;
                Invalidate();
            }
        }
        [Category("Custom")]
        public string TextLine2

        {
            get
            {
                return _textLine2;
            }
            set
            {
                _textLine2 = value;
                Invalidate();
            }
        }
        [Category("Custom")]
        public string TextLine3

        {
            get
            {
                return _textLine3;
            }
            set
            {
                _textLine3 = value;
            }
        }

        [Category("Custom")]
        public Font TextFont1
        {
            get
            {
                return _textFont1;
            }
            set
            {
                _textFont1 = value;
                Invalidate();
            }
        }
        [Category("Custom")]
        public Font TextFont2
        {
            get
            {
                return _textFont2;
            }
            set
            {
                _textFont2 = value;
                Invalidate();
            }
        }
        [Category("Custom")]
        public Font TextFont3
        {
            get
            {
                return _textFont3;
            }
            set
            {
                _textFont3 = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        public Color ProgressBeginColor
        {
            get
            {
                return _progressBeginColor;
            }
            set
            {
                _progressBeginColor = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        public Color ProgressEndColor
        {
            get
            {
                return _progressEndColor;
            }
            set
            {
                _progressEndColor = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        public Color TextColor1
        {
            get
            {
                return _textColor1;
            }
            set
            {
                _textColor1 = value;
                Invalidate();
            }
        }
        [Category("Custom")]
        public Color TextColor2
        {
            get
            {
                return _textColor2;
            }
            set
            {
                _textColor2 = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        public Color TextColor3
        {
            get
            {
                return _textColor3;
            }
            set
            {
                _textColor3 = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        [Description("进度条颜色")]
        public Color ProgressValueColor
        {
            get;
            set;
        } = Color.SteelBlue;


        [Browsable(false)]
        public Brush ProgressGradientBrush
        {
            get;
            private set;
        }


        [Category("Custom")]
        [Description("进度条的宽度")]
        public float ProgressWidth
        {
            get
            {
                return _progressWidth;
            }
            set
            {
                _progressWidth = value;
                Invalidate();
            }
        }

        #endregion

        #region 构造

        public CircularProgressBar()
        {
            this.Size = new Size(130, 130);

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        #endregion

        #region 重写的成员

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            using (Bitmap bitmap = new Bitmap(Width, Height))
            using (Graphics bufferedGraphics = Graphics.FromImage(bitmap))
            {
                bufferedGraphics.SmoothingMode = SmoothingMode.HighQuality;
                bufferedGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias;

                bufferedGraphics.Clear(Color.Transparent);

                // 画边框
                using (Pen penBorder = new Pen(ProgressBackColor)
                {
                    Width = ProgressWidth,
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                })
                {
                    float width = this.Width - penBorder.Width - 1f;
                    float height = this.Height - penBorder.Width - 1f;

                    if (width > 0 && height > 0)
                    {
                        bufferedGraphics.DrawArc(penBorder,
                        0 + penBorder.Width / 2f,
                        0 + penBorder.Width / 2f,
                        width,
                        height,
                        StartAngle,
                        SweepAngle);
                    }
                }

                // 渐变, 留出边框 
                using (ProgressGradientBrush = new LinearGradientBrush(ClientRectangle,
                    ProgressBeginColor,
                    ProgressEndColor,
                    LinearGradientMode.ForwardDiagonal))
                using (Pen progressPen = new Pen(ProgressGradientBrush) { Width = ProgressWidth - 2f, StartCap = LineCap.Round, EndCap = LineCap.Round })
                {
                    float x = progressPen.Width / 2f + 1f;
                    float y = progressPen.Width / 2f + 1f ;
                    float width = (float)this.Width - progressPen.Width - 2.5f;
                    float height = (float)this.Height - progressPen.Width - 2.5f;


                    if (width > 0 && height > 0)
                    {
                        // 进度
                        bufferedGraphics.DrawArc(progressPen,
                            x,
                            y,
                            width,
                            height,
                            StartAngle,
                            (_value / this._maximum) * SweepAngle);
                    }
                }


                if (!string.IsNullOrEmpty(TextLine1))
                {
                    DrawText(bufferedGraphics, TextLine1, TextFont1, TextColor1, 0.25f);
                }
                if (!string.IsNullOrEmpty(TextLine2))
                {
                    DrawText(bufferedGraphics, TextLine2, TextFont2, TextColor2, 0.45f);
                }
                if (!string.IsNullOrEmpty(TextLine3))
                {
                    DrawText(bufferedGraphics, TextLine3, TextFont3, TextColor3, 0.7f);
                }

                e.Graphics.DrawImage(bitmap, new Point(0, 0));
            }
        }

        #endregion


        protected virtual void DrawText(Graphics g, string text, Font font, Color color, float vPercent)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                SizeF stringSize = g.MeasureString(text, font);
                int posX = Convert.ToInt32((Width - stringSize.Width) / 2);
                int posY = Convert.ToInt32(Height * vPercent );

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.Trimming = StringTrimming.Character;

                RectangleF fontRectanle = new RectangleF(posX + 1, posY + 1, stringSize.Width, stringSize.Height);
                g.DrawString(text, font, brush, fontRectanle, sf);
            }
        }
    }
}