using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace System.Windows.Forms
{
    /// <summary>
    /// 圆角文本框控件
    /// 
    /// 已知问题: 
    ///         1.有时会与其他控件抢焦点, 造成其他按钮点击失效    
    /// </summary>
    public partial class RoundTextBox : NonFlickerUserControl
    {
        public class TabEchoTextBox : TextBox
        {
            public event Action TabAction;
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                if (keyData == Keys.Tab)
                {
                    if (TabAction != null)
                        TabAction();
                    return false;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #region 枚举

        public enum TextBoxStates
        {
            /// <summary>
            /// 正常状态
            /// </summary>
            Normal = 0,
            /// <summary>
            ///  /鼠标进入
            /// </summary>
            Highlight = 1,
            /// <summary>
            /// 控件禁止
            /// </summary>
            Disabled = 2
        }

        #endregion

        #region 委托


        public event Action ActionBegin;

        [Category(Consts.DefaultCategory)]
        [Browsable(true)]
        public new event EventHandler TextChanged;


        [Category(Consts.DefaultCategory)]
        public event EventHandler CancelButtonClick;

        #endregion

        #region 字段

        private TextBoxStates _innerTextBoxState = TextBoxStates.Normal;

        private string _emptyTooltipText;
        private bool _autoSizeFont;
        private bool _useSystemPasswordChar;
        private Color _textContentBackColor;
        private Color _textContentForeColor;
        private Font _originalFont;
        //public bool ButtonClickWorking { get; set; }

        private int _borderRadius;
        private bool _autoScrollbar;
        private int _defaultPadding = 7;
        private int _customLeftIndent;

        #endregion

        #region 构造

        public RoundTextBox()
        {
            InitializeComponent();

            DoPrepareWork();

        }

        #endregion

        #region 属性

        /// <summary>
        /// 内容为空时的提示文字
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public string EmptyTooltipText
        {
            get
            {
                return _emptyTooltipText;
            }
            set
            {
                _emptyTooltipText = value;
                _innerTextBox.Text = _emptyTooltipText;
                _innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            }
        }

        /// <summary>
        /// 内容为空时提示文字的颜色
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public Color EmptyTooltipForeColor { get; set; }

        /// <summary>
        /// 文本框的内容
        /// </summary>
        [Category(Consts.DefaultCategory)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                if (this.IsTextEqualEmptyTooltip())
                {
                    return string.Empty;
                }

                return _innerTextBox.Text;
            }
            set
            {
                _innerTextBox.Text = value;



                if (string.IsNullOrEmpty(value))
                {
                    _innerTextBox.Text = this.EmptyTooltipText;
                    _innerTextBox.ForeColor = this.EmptyTooltipForeColor;

                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            _innerTextBox.UseSystemPasswordChar = false;
                        });
                    }
                }

                UpdateForeColor();

                OnAutoScrollbarChanged();
            }
        }


        /// <summary>
        /// 文本框状态
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public TextBoxStates TextBoxState
        {
            get { return _innerTextBoxState; }
            set { _innerTextBoxState = value; this.Invalidate(); }
        }

        [Category(Consts.DefaultCategory)]
        public Color TextContentBackColor
        {
            get
            {
                return _textContentBackColor;
            }
            set
            {
                _textContentBackColor = value;
                _innerTextBox.BackColor = value;
                Invalidate();
            }
        }

        [Category(Consts.DefaultCategory)]
        public Color TextContentForeColor
        {
            get
            {
                return _textContentForeColor;
            }
            set
            {
                _textContentForeColor = value;

                if (!IsTextEqualEmptyTooltip())
                {
                    base.ForeColor = value;
                    _innerTextBox.ForeColor = value;
                }
                else
                {
                    base.ForeColor = value;
                    _innerTextBox.ForeColor = EmptyTooltipForeColor;
                }

                Invalidate();
            }
        }


        /// <summary>
        /// 边框色
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public Color BorderColor { get; set; }

        /// <summary>
        /// 边框悬浮色
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public Color BorderHoverColor { get; set; }

        [Category(Consts.DefaultCategory)]
        public Color BorderHoverColor2 { get; set; }

        /// <summary>
        /// 获得焦点后的边框色
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public Color BorderFocusColor { get; set; }

        [Category(Consts.DefaultCategory)]
        public Color BorderFocusColor2 { get; set; }



        /// <summary>
        /// 是否自动缩放字体
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public bool AutoSizeFont
        {
            get
            {
                return _autoSizeFont;
            }
            set
            {
                bool isChanged = _autoSizeFont != value;
                _autoSizeFont = value;

                if (isChanged)
                {
                    ResizeTextBoxFont();
                    RecalculateInnerTextBoxPosition();
                }
            }
        }


        /// <summary>
        /// 内容显示为密码字符
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public bool UseSystemPasswordChar
        {
            get
            {
                return _useSystemPasswordChar;
            }
            set
            {
                _useSystemPasswordChar = value;
                if (IsTextEqualEmptyTooltip())
                {
                    _innerTextBox.UseSystemPasswordChar = false;
                }
                else
                {
                    _innerTextBox.UseSystemPasswordChar = value;
                }
            }
        }



        /// <summary>
        /// 圆角半径
        /// </summary>
        [Category(Consts.DefaultCategory)]
        [DefaultValue(6)]
        public int BorderRadius
        {
            get
            {
                return _borderRadius;
            }
            set
            {
                _borderRadius = value;
                RecalculateInnerTextBoxPosition();
            }
        }

        [Category(Consts.DefaultCategory)]
        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }



        [Category(Consts.DefaultCategory)]
        public bool ReadOnly
        {
            get
            {
                return _innerTextBox.ReadOnly;
            }
            set
            {
                _innerTextBox.ReadOnly = value;
                //innerTextBox.BackColor = this.BackColor;
            }
        }


        [Category(Consts.DefaultCategory)]
        public bool EnterSendTab
        {
            get;
            set;
        }

        [Category(Consts.DefaultCategory)]
        public bool AutoScrollbar
        {
            get
            {
                return _autoScrollbar;
            }
            set
            {
                _autoScrollbar = value;
                OnAutoScrollbarChanged();
            }
        }


        [Category(Consts.DefaultCategory)]
        public int MaxLength
        {
            get
            {
                return _innerTextBox.MaxLength;
            }
            set
            {
                _innerTextBox.MaxLength = value;
            }
        }


        [Category(Consts.DefaultCategory)]
        public AutoCompleteStringCollection AutoCompleteSource
        {
            get
            {
                return _innerTextBox.AutoCompleteCustomSource;
            }
            set
            {
                _innerTextBox.AutoCompleteCustomSource = value;
            }

        }

        private bool _showClearButton;
        [Category(Consts.DefaultCategory)]
        public bool ShowClearButton
        {
            get
            {

                return _showClearButton;
            }
            set
            {
                _showClearButton = value;
                UpdateClearButtonVisibility();
            }
        }




        /// <summary>
        /// 前景色
        /// </summary>
        [Category(Consts.DefaultCategory)]
        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (!IsTextEqualEmptyTooltip())
                {
                    base.ForeColor = value;
                    _innerTextBox.ForeColor = value;
                }
                else
                {
                    base.ForeColor = value;
                    _innerTextBox.ForeColor = EmptyTooltipForeColor;
                }
            }
        }

        /// <summary>
        /// 停靠方式
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public bool Multiline
        {
            get
            {
                return _innerTextBox.Multiline;
            }
            set
            {
                _innerTextBox.Multiline = value;
                RecalculateInnerTextBoxPosition();
            }
        }

        [Category(Consts.DefaultCategory)]
        public int CustumLeftIndent
        {
            get
            {
                return _customLeftIndent;
            }
            set
            {
                _customLeftIndent = value;
            }
        }

        public bool UseUpperCase
        {
            get;
            set;
        }


        #endregion

        private void UpdateClearButtonVisibility()
        {

            string currentText = _innerTextBox.Text;
            if (!string.IsNullOrEmpty(currentText) && currentText != EmptyTooltipText)
            {
                if (_showClearButton)
                {
                    btnCancel.Visible = true;
                }
            }
            else
            {
                if (_showClearButton)
                {
                    btnCancel.Visible = false;
                }
            }
        }

        #region 事件处理

        private void RoundTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            //_innerTextBoxState = TextBoxStates.Normal;
            //this.Invalidate();
        }

        private void PaddingTextBox_LostFocus(object sender, EventArgs e)
        {
            //if (!ButtonClickWorking)
            //    return;

            _innerTextBoxState = TextBoxStates.Normal;
            this.Invalidate();
        }

        private void PaddingTextBox_MouseLeave(object sender, EventArgs e)
        {
            //if (!ButtonClickWorking)
            //    return;

            if (!_innerTextBox.Focused)
            {
                _innerTextBoxState = TextBoxStates.Normal;
            }

            this.Invalidate();
        }

        private void innerTextBox_GotFocus(object sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Highlight;
            if (IsTextEqualEmptyTooltip())
            {
                _innerTextBox.UseSystemPasswordChar = false;
                _innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            }
            else
            {
                _innerTextBox.UseSystemPasswordChar = this.UseSystemPasswordChar;
                _innerTextBox.ForeColor = this.ForeColor;
            }
            this.Invalidate();
        }

        private void innerTextBox_MouseHover(object sender, EventArgs e)
        {
            //_innerTextBoxState = TextBoxStates.Highlight;
            //this.Invalidate();

        }

        private void innerTextBox_MouseEnter(object sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Highlight;
            this.Invalidate();
            Timer t = new Timer();
            t.Interval = 500;
            t.Tick += (a, b) =>
            {
                if (!this.IsHandleCreated || this.IsDisposed)
                {
                    t.Stop();
                    t.Dispose();
                    return;
                }
                bool hittest = _innerTextBox.Bounds.Contains(PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y)));
                if (!hittest)
                {
                    _innerTextBoxState = TextBoxStates.Normal;
                    this.Invalidate();
                    t.Stop();
                    t.Dispose();
                }
            };
            t.Start();
        }

        private void innerTextBox_Enter(object sender, EventArgs e)
        {
            if (_innerTextBox.Text != string.Empty)
            {
                if (IsTextEqualEmptyTooltip())
                {
                    // 获得焦点时，清除提示文字
                    _innerTextBox.Text = string.Empty;
                    _innerTextBox.ForeColor = this.ForeColor;
                }
            }

            if (IsTextEqualEmptyTooltip())
            {
                _innerTextBox.UseSystemPasswordChar = false;
                if (this.UseSystemPasswordChar)
                {
                    _innerTextBox.ForeColor = this.EmptyTooltipForeColor;
                }
            }
            else
            {
                _innerTextBox.UseSystemPasswordChar = this.UseSystemPasswordChar;
                if (this.UseSystemPasswordChar)
                {
                    _innerTextBox.ForeColor = this.ForeColor;
                }
            }


        }

        private void innerTextBox_Leave(object sender, EventArgs e)
        {
            if (_innerTextBox.Text != string.Empty)
            {
                if (IsTextEqualEmptyTooltip())
                {
                    _innerTextBox.ForeColor = this.EmptyTooltipForeColor;
                }
                else
                {
                    _innerTextBox.ForeColor = this.ForeColor;
                }
            }
            else
            {
                _innerTextBox.Text = this.EmptyTooltipText;
                _innerTextBox.ForeColor = this.EmptyTooltipForeColor;
                this.BeginInvoke((MethodInvoker)delegate
                {
                    _innerTextBox.UseSystemPasswordChar = false;
                });
            }

            //if (ButtonClickWorking)
            //{
            //    return;
            //}

            _innerTextBoxState = TextBoxStates.Normal;
            this.Invalidate();

            this.OnLeave(e);
        }

        private void innerTextBox_LostFocus(object sender, EventArgs e)
        {
            if (IsTextEqualEmptyTooltip())
            {
                _innerTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                _innerTextBox.UseSystemPasswordChar = this.UseSystemPasswordChar;
            }

        }

        private void innerTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateClearButtonVisibility();

            if (_innerTextBox.Text != EmptyTooltipText)
            {
                if (UseUpperCase)
                {
                    string inputText = _innerTextBox.Text;
                    if (inputText != null && inputText.Length > 0)
                    {
                        string upperCaseText = inputText.ToUpper();
                        _innerTextBox.Text = upperCaseText;
                        // 将光标移动到文本末尾以确保用户可以继续输入
                        _innerTextBox.SelectionStart = _innerTextBox.Text.Length;
                    }
                }

                TextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void innerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }

        private void innerTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        private void innerTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        private void innerTextBox_TabAction()
        {
            //this.Focus();
            //SendKeys.Send("{tab}");
        }
        #endregion

        #region 重写的成员

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (EnterSendTab)
            {
                if (keyData == (Keys.Enter))
                {
                    SendKeys.Send("{TAB}");
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        /*
         * 1. 字体改变时，且Dock为None, 文本框高度跟着改变
         * 2. 控件拉伸时，如果AutoSizeFont, 字体跟着改变大小
         * 3. 
为         */
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);


            _innerTextBox.Font = this.Font;

            SizeF textSize = TextRenderer.MeasureText("ABC100", _innerTextBox.Font);
            if (textSize.Height >= this.Height)
            {
                // 大于外框大小，则扩大。 
                ResizeControlHeight();
            }
            else
            {
                // 小于外框大小， 则居中. 不缩小外框大小，用户自己拉伸。
                RecalculateInnerTextBoxPosition();
            }

        }


        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
            {
                _innerTextBoxState = TextBoxStates.Normal;
                _innerTextBox.Enabled = true;
            }
            else
            {
                _innerTextBoxState = TextBoxStates.Disabled;
                _innerTextBox.Enabled = false;
            }
            this.Invalidate();
            base.OnEnabledChanged(e);
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (AutoSizeFont)
            {
                ResizeTextBoxFont();
            }

            RecalculateInnerTextBoxPosition();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (this.Parent != null)
            {
                _originalFont = this.Parent.Font;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            // 填充边框外背景色
            if (BackColor == Color.Transparent)
            {
                Brush bgbrush = new SolidBrush(Parent.BackColor);
                g.FillRectangle(bgbrush, this.ClientRectangle);
                bgbrush.Dispose();
            }
            else
            {
                g.Clear(BackColor);
            }

            // 填充边框内背景色
            using (Brush brush = new SolidBrush(TextContentBackColor))
            {
                g.FillRoundedRectangle(brush,
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1, BorderRadius);
            }

            switch (_innerTextBoxState)
            {
                case TextBoxStates.Normal:
                    DrawNormalTextBox(g);
                    break;
                case TextBoxStates.Highlight:
                    DrawHighLightTextBox(g);
                    break;
                case TextBoxStates.Disabled:
                    DrawDisabledTextBox(g);
                    break;
                default:
                    break;
            }

            base.OnPaint(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            OnAutoScrollbarChanged();
        }




        #endregion


        #region 私有方法


        private bool IsTextEqualEmptyTooltip()
        {
            bool result = false;
            if (_innerTextBox.Text != string.Empty)
            {
                if (_innerTextBox.Text == this.EmptyTooltipText)
                {
                    return true;
                }
            }

            return result;
        }


        /// <summary>
        /// 设置各个控件初始值
        /// </summary>
        private void DoPrepareWork()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);

            this.BorderColor = Color.Gray;
            this.BorderHoverColor = Color.SkyBlue;
            this.BorderFocusColor = Color.DodgerBlue;

            this.EmptyTooltipText = string.Empty;
            this.EmptyTooltipForeColor = Color.Gray;

            this.BorderRadius = 6;

            this.BackColor = Color.Transparent;
            this.ForeColor = Color.Black;
            this.TextContentBackColor = Color.White;


            this.Padding = new Padding(_defaultPadding);

            _innerTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _innerTextBox.AutoCompleteSource = Forms.AutoCompleteSource.CustomSource;
            _innerTextBox.AcceptsReturn = true;
            _innerTextBox.Dock = DockStyle.Fill;
            _innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            _innerTextBox.MouseEnter += new EventHandler(innerTextBox_MouseEnter);
            _innerTextBox.MouseHover += new EventHandler(innerTextBox_MouseHover);
            _innerTextBox.GotFocus += new EventHandler(innerTextBox_GotFocus);
            _innerTextBox.LostFocus += new EventHandler(innerTextBox_LostFocus);

            _innerTextBox.TextChanged += this.innerTextBox_TextChanged;
            _innerTextBox.Enter += this.innerTextBox_Enter;
            _innerTextBox.Leave += this.innerTextBox_Leave;

            _innerTextBox.KeyDown += new KeyEventHandler(innerTextBox_KeyDown);
            _innerTextBox.KeyPress += new KeyPressEventHandler(innerTextBox_KeyPress);
            _innerTextBox.KeyUp += new KeyEventHandler(innerTextBox_KeyUp);

            btnCancel.MouseEnter += new EventHandler(innerTextBox_MouseEnter);
            btnCancel.MouseHover += innerTextBox_MouseHover;

            this.MouseEnter += innerTextBox_MouseEnter;
            this.MouseHover += innerTextBox_MouseHover;
            this.MouseLeave += PaddingTextBox_MouseLeave;
            this.LostFocus += PaddingTextBox_LostFocus;
            this.MouseMove += RoundTextBox_MouseMove;


        }




        private void DrawNormalTextBox(Graphics g)
        {
            using (Pen borderPen = new Pen(this.BorderColor))
            {
                g.DrawRoundedRectangle(
                    borderPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1), BorderRadius);
            }
        }

        private void DrawHighLightTextBox(Graphics g)
        {
            using (Pen highLightPen = new Pen(BorderHoverColor))
            {
                Rectangle drawRect = new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1);

                if (_innerTextBox.Focused && !BorderHoverColor2.IsEmpty)
                {
                    highLightPen.Color = BorderHoverColor2;
                }
                g.DrawRoundedRectangle(highLightPen, drawRect, BorderRadius);

                // 外边框
                drawRect.Inflate(-1, -1);
                highLightPen.Color = BorderFocusColor;
                if (_innerTextBox.Focused && !BorderFocusColor2.IsEmpty)
                {
                    highLightPen.Color = BorderFocusColor2;
                }
                g.DrawRoundedRectangle(highLightPen, drawRect, BorderRadius - 2);
            }
        }

        private void DrawDisabledTextBox(Graphics g)
        {
            using (Pen disabledPen = new Pen(SystemColors.ControlDark))
            {
                g.DrawRoundedRectangle(disabledPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1), BorderRadius);
            }
        }


        // 根据高度重新设定文本框的字体大小
        private Font GetFontForTextBoxHeight(int TextBoxHeight, Font OriginalFont)
        {
            float desiredheight = (float)TextBoxHeight;

            Font fnt = new Font(OriginalFont.FontFamily,
                                OriginalFont.Size,
                                OriginalFont.Style,
                                GraphicsUnit.Pixel);

            if (desiredheight < 8)
                desiredheight = 8;

            float FontEmSize = fnt.FontFamily.GetEmHeight(fnt.Style);
            float FontLineSpacing = fnt.FontFamily.GetLineSpacing(fnt.Style);

            int defaultPaddingSize = _borderRadius / 2 + 7;
            if (defaultPaddingSize < _defaultPadding)
            {
                defaultPaddingSize = _defaultPadding;
            }

            float emSize = (desiredheight - defaultPaddingSize) * FontEmSize / FontLineSpacing;

            fnt = new Font(fnt.FontFamily, emSize, fnt.Style, GraphicsUnit.Pixel);

            return fnt;
        }

        /// <summary>
        /// 自适应文本框字体大小
        /// <para>不改变控件大小，按当前控件大小重新设定字体大小</para>
        /// </summary>
        private void ResizeTextBoxFont()
        {
            if (AutoSizeFont)
            {
                // 缩放字体以适应文本框
                int height = this.Height - _defaultPadding * 2;
                _innerTextBox.Font = GetFontForTextBoxHeight(height, this.Font);
            }
            else
            {
                _innerTextBox.Font = this.Font;
                this.Invalidate();
            }
        }

        private void ResizeControlHeight()
        {

            // 仅在单行和未停靠时修改控件大小
            if (this.Dock == DockStyle.None && !Multiline)
            {
                // 如果字体超出了文本框，将文本框扩大 
                SizeF minSize = TextRenderer.MeasureText("10000", _originalFont);
                int minHeight = (int)minSize.Height + _defaultPadding * 2;

                SizeF textSize = TextRenderer.MeasureText("10000", _innerTextBox.Font);
                int newHeight = (int)textSize.Height + _defaultPadding * 2;
                if (newHeight < minHeight)
                {
                    newHeight = minHeight;
                }
                this.Height = newHeight;
            }

        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                //base.Padding = new Padding(_borderRadius / 2 + 7);
                base.Padding = value;
            }
        }


        /// <summary>
        /// 单行模式下，文本垂直居中。
        /// 多行模式下，文本居左上。
        /// </summary>
        private void RecalculateInnerTextBoxPosition()
        {
            int defaultPaddingSize = _borderRadius / 2 + 7;
            if (defaultPaddingSize < _defaultPadding)
            {
                defaultPaddingSize = _defaultPadding;
            }


            if (Multiline)
            {
                this.Padding = new Padding(defaultPaddingSize);
            }
            else
            {
                this.Padding = new Padding(defaultPaddingSize);



                _innerTextBox.Dock = DockStyle.Fill;

                int innerHeight = _innerTextBox.Height;
                int top = (this.Height - innerHeight) / 2;

                int innerWidth = _innerTextBox.Width;

                //this.Padding = new Padding(
                //    defaultPaddingSize,
                //    top,
                //    defaultPaddingSize,
                //    bottom);

                _innerTextBox.Dock = DockStyle.None;
                _innerTextBox.Location = new Point(defaultPaddingSize, top);
                //_innerTextBox.Width = this.Width - defaultPaddingSize * 2 - (ShowClearButton ? btnCancel.Width : 0);

                _innerTextBox.Width = innerWidth;
            }
        }

        private void OnAutoScrollbarChanged()
        {
            if (IsHandleCreated && AutoScrollbar)
            {
                int mode = 0;
                SizeF textSize = TextRenderer.MeasureText(_innerTextBox.Text, _innerTextBox.Font);
                if (textSize.Width > this.Width - BorderRadius * 2)
                {
                    mode += 2;
                }

                if (textSize.Height > this.Height - BorderRadius * 2)
                {
                    mode += 4;
                }

                switch (mode)
                {
                    case 0:
                        _innerTextBox.ScrollBars = ScrollBars.None;
                        break;
                    case 2:
                        _innerTextBox.ScrollBars = ScrollBars.Horizontal;
                        break;
                    case 4:
                        _innerTextBox.ScrollBars = ScrollBars.Vertical;
                        break;
                    case 6:
                        _innerTextBox.ScrollBars = ScrollBars.Both;
                        break;
                }

            }
            else
            {
                _innerTextBox.ScrollBars = ScrollBars.None;
            }
        }

        #endregion


        private void UpdateForeColor()
        {
            if (IsTextEqualEmptyTooltip())
            {
                _innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            }
            else
            {
                _innerTextBox.ForeColor = this.ForeColor;
            }
        }


        private void btnDropDown_MouseClick(object sender, MouseEventArgs e)
        {
            if (ActionBegin != null)
            {
                ActionBegin();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _innerTextBox.Text = string.Empty;

            CancelButtonClick?.Invoke(sender, e);
        }
    }
}
