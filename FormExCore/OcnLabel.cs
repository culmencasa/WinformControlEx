using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormExCore
{
    public class OcnLabel : Control
    {
        #region 构造

        public OcnLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;


            this.ClientSizeChanged += OcnLabel_ClientSizeChanged;
        }

        #endregion

        #region 设计时


        // 已废弃, 原来继承于Label, 用于覆盖初始设置
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

        #endregion

        #region 字段

        private string _text;
        private Image _image;
        private ContentAlignment _textAlign;
        private int _indent = 6;
        private int _iconSize = 16;

        private bool _multiline = true;
        private bool _ellipsis = false;
        private bool _alignCenter = false;
        private Control _parentControl;

        #endregion

        #region 属性

        [Category("Custom")]
        public override string Text 
        { 
            get => _text;
            set
            {
                _text = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue(null)]
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

        [Category("Custom")]
        public int IconSize
        {
            get
            {
                return _iconSize;
            }
            set
            {
                _iconSize = value;
            }
        }

        [Category("Custom")]
        public bool AlignCenter
        {
            get
            {
                return _alignCenter;
            }
            set
            {
                _alignCenter = value;
            }
        }

        /// <summary>
        /// 获取或设置 Label 是否支持多行文本
        /// </summary>
        [Category("Custom")]
        public bool Multiline
        {
            get { return _multiline; }
            set
            {
                _multiline = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 获取或设置 Label 是否使用省略号显示过长的文本
        /// </summary>
        [Category("Custom")]
        public bool Ellipsis
        {
            get { return _ellipsis; }
            set
            {
                _ellipsis = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContentAlignment TextAlign
        {
            get
            {
                return _textAlign;
            }
            set
            { 
                _textAlign = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        public int Indent
        {
            get
            {
                return _indent;
            }
            set
            {
                _indent = value;
                Invalidate();
            }
        }


        /// <summary>
        /// 是否点击穿透
        /// </summary>
        [Category("Custom")]
        public bool ClickThrough
        {
            get;
            set;
        }



        #endregion

        #region 私有方法

        /// <summary>
        /// 计算图标的区域
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetIconRectangle()
        {
            int width = 0;
            int height = 0;
            int left = 0;
            int top = 0;

            int marginToLeftSide = Indent;

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

        /// <summary>
        /// 画图标
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawIcon(Graphics g)
        {
            if (Image != null)
            {
                g.DrawImage(Image, GetIconRectangle());
            }
        }

        /// <summary>
        /// 画文本
        /// </summary>
        /// <param name="g"></param>
        protected virtual void DrawText(Graphics g)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                using (SolidBrush sb = new SolidBrush(ForeColor))
                {
                    // 计算绘制文本的矩形
                    var rect = new RectangleF(0, 0, Width, Height);

                    int textMarginToImage = 6;
                    Rectangle titleRectangle = Rectangle.Empty;
                    Rectangle iconRectangle = GetIconRectangle();
                    if (iconRectangle.Width == 0)
                    {
                        textMarginToImage = 3;
                    }


                    StringFormat sf = GetStringFormat();

                    // 自动换行
                    if (Multiline)
                    {
                        sf.FormatFlags |= StringFormatFlags.LineLimit;
                        sf.Trimming = StringTrimming.Word;
                    }
                    else
                    {
                        sf.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip;
                        sf.Trimming = StringTrimming.None;
                    }

                    // 文本显示位置X
                    int textAreaLeft = this.Indent + iconRectangle.Width + textMarginToImage;
                    // 文本可显示宽度
                    float textAreaWidth = rect.Width - textAreaLeft;
                    // 根据以上条件计算文本宽高
                    var textSize = g.MeasureString(Text, Font, (int)textAreaWidth, sf);

                    // 省略号
                    if (Ellipsis)
                    {
                        sf.Trimming = StringTrimming.EllipsisCharacter;
                    }

                    // 将绘制文本的矩形调整为文本的实际大小
                    rect.Width = textSize.Width;
                    rect.Height = textSize.Height;

                    // 根据控件的大小和文本的大小调整绘制文本的位置
                    switch (TextAlign)
                    {
                        case ContentAlignment.TopLeft:
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.BottomLeft:
                            rect.X = textAreaLeft;
                            break;
                        case ContentAlignment.TopCenter:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.BottomCenter:
                            rect.X = (Width - textSize.Width) / 2;
                            break;
                        case ContentAlignment.TopRight:
                        case ContentAlignment.MiddleRight:
                        case ContentAlignment.BottomRight:
                            rect.X = Width - textSize.Width;
                            break;
                    }

                    switch (TextAlign)
                    {
                        case ContentAlignment.TopLeft:
                        case ContentAlignment.TopCenter:
                        case ContentAlignment.TopRight:
                            rect.Y = 0;
                            break;
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.MiddleRight:
                            rect.Y = (Height - textSize.Height) / 2;
                            break;
                        case ContentAlignment.BottomLeft:
                        case ContentAlignment.BottomCenter:
                        case ContentAlignment.BottomRight:
                            rect.Y = Height - textSize.Height;
                            break;
                    }


                    if (textSize.Height > Height)
                    {
                        rect.Y = 0;
                    }



                    // 绘制文本
                    g.DrawString(Text, Font, sb, rect, sf);

                }
            }
        }

        /// <summary>
        /// 文本对齐方式对应的布局
        /// </summary>
        /// <returns></returns>
        private StringFormat GetStringFormat()
        {

            StringFormat sf = new StringFormat();

            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }

            return sf;
        }


        #endregion

        #region 重写


        protected override void WndProc(ref Message m)
        {
            if (!DesignMode && ClickThrough)
            {
                if (m.Msg == Win32.WM_NCHITTEST)
                {
                    m.Result = new IntPtr(Win32.HTTRANSPARENT);
                    return;
                }
            }

            base.WndProc(ref m);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SetSlowRendering();

            DrawIcon(g);

            DrawText(g);

            base.OnPaint(e);
        }


        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (!DesignMode)
            {
                _parentControl = Parent;
                _parentControl.BackColorChanged -= ParentControl_BackColorChanged;
                _parentControl.BackColorChanged += ParentControl_BackColorChanged;
            }
        }

        private void ParentControl_BackColorChanged(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                BackColor = _parentControl.BackColor;
            }
        }

        #endregion
    }
}
