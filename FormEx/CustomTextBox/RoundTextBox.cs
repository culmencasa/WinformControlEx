﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Utils;

namespace System.Windows.Forms
{
    /// <summary>
    /// 圆角文本框
    /// 已知Bug: 有时会与其他控件抢焦点, 造成其他按钮点击失效
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

        [Category("Custom")]
        [Browsable(true)]
        public new event EventHandler TextChanged;


        #endregion

        #region 字段

        private TextBoxStates _innerTextBoxState = TextBoxStates.Normal;

        private string _emptyTooltipText; 
        private bool _autoSizeFont;
        private bool _useSystemPasswordChar;
        private Color _textContentBackColor;
        private Color _textContentForeColor;

        public bool ButtonClickWorking { get; set; }


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
        [Category("Custom")]
        public string EmptyTooltipText
        {
            get
            {
                return _emptyTooltipText;
            }
            set
            {
                _emptyTooltipText = value;
                innerTextBox.Text = _emptyTooltipText;
                innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            }
        }
         
        /// <summary>
        /// 内容为空时提示文字的颜色
        /// </summary>
        [Category("Custom")]
        public Color EmptyTooltipForeColor { get; set; }
         
        /// <summary>
        /// 文本框的内容
        /// </summary>
        [Category("Custom")]
        public override string Text
        {
            get
            {
                if (this.IsTextEqualEmptyTooltip())
                {
                    return string.Empty;
                }

                return innerTextBox.Text;
            }
            set
            {
                innerTextBox.Text = value;



                if (string.IsNullOrEmpty(value))
                {
                    innerTextBox.Text = this.EmptyTooltipText;
                    innerTextBox.ForeColor = this.EmptyTooltipForeColor;

                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            innerTextBox.UseSystemPasswordChar = false;
                        });
                    }
                }

                UpdateForeColor();
            }
        }

        [Category("Custom")]
        public string TextContent
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// 文本框状态
        /// </summary>
        [Category("Custom")]
        public TextBoxStates TextBoxState
        {
            get { return _innerTextBoxState; }
            set { _innerTextBoxState = value; this.Invalidate(); }
        }

        [Category("Custom")]
        public Color TextContentBackColor
        {
            get
            {
                return _textContentBackColor;
            }
            set
            {
                _textContentBackColor = value;
                innerTextBox.BackColor = value;
                Invalidate();
            }
        }

        [Category("Custom")]
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
                    innerTextBox.ForeColor = value;
                }
                else
                {
                    base.ForeColor = value;
                    innerTextBox.ForeColor = EmptyTooltipForeColor;
                }

                Invalidate();
            }
        }


        /// <summary>
        /// 边框色
        /// </summary>
        [Category("Custom")]
        public Color BorderColor { get; set; }
         
        /// <summary>
        /// 边框悬浮色
        /// </summary>
        [Category("Custom")]
        public Color BorderHoverColor { get; set; }

        [Category("Custom")]
        public Color BorderHoverColor2 { get; set; }

        /// <summary>
        /// 获得焦点后的边框色
        /// </summary>
        [Category("Custom")]
        public Color BorderFocusColor { get; set; }

        [Category("Custom")]
        public Color BorderFocusColor2 { get; set; }



        /// <summary>
        /// 是否自动缩放字体
        /// </summary>
        [Category("Custom")]
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
                    this.Padding = new Forms.Padding(3);
                    ReizeTextBoxFont();
                }
            }
        }

        /// <summary>
        /// 内容显示为密码字符
        /// </summary>
        [Category("Custom")]
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
                    innerTextBox.UseSystemPasswordChar = false;
                }
                else
                {
                    innerTextBox.UseSystemPasswordChar = value;
                }
            }
        }

        /// <summary>
        /// 是否显示下拉按钮
        /// </summary>
        [Category("Custom")]
        [DefaultValue(false)]
        public bool ShowDropDownButton
        {
            get
            {
                return btnDropDown.Visible;
            }
            set
            {
                btnDropDown.Visible = value;
            }
        }


        /// <summary>
        /// 圆角半径
        /// </summary>
        [Category("Custom")]
        [DefaultValue(6)]
        public int BorderRadius { get; set; }

        [Category("Custom")]
        [DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor { 
            get => base.BackColor; 
            set => base.BackColor = value;
        }



        [Category("Custom")]
        public bool ReadOnly
        {
            get
            {
                return innerTextBox.ReadOnly;
            }
            set
            {
                //innerTextBox.ReadOnly = value;
                //innerTextBox.BackColor = this.BackColor;
            }
        }


        [Category("Custom")]
        public bool EnterSendTab
        {
            get;
            set;
        }


        #endregion

        #region 事件处理

        private void PaddingTextBox_LostFocus(object sender, EventArgs e)
        {
            if (!ButtonClickWorking)
                return;

            _innerTextBoxState = TextBoxStates.Normal;
            this.Invalidate();
        }

        private void PaddingTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (!ButtonClickWorking)
                return;

            if (!innerTextBox.Focused)
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
                innerTextBox.UseSystemPasswordChar = false;
                innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            }
            else
            {
                innerTextBox.UseSystemPasswordChar = this.UseSystemPasswordChar;
                innerTextBox.ForeColor = this.ForeColor;
            }
            this.Invalidate();
        }

        private void innerTextBox_MouseHover(object sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Highlight;
            this.Invalidate();
        }

        private void innerTextBox_MouseEnter(object sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Highlight;
            this.Invalidate();
        }

        private void innerTextBox_Enter(object sender, EventArgs e)
        {
            if (innerTextBox.Text != string.Empty)
            {
                if (IsTextEqualEmptyTooltip())
                {
                    // 获得焦点时，清除提示文字
                    innerTextBox.Text = string.Empty;
                    innerTextBox.ForeColor = this.ForeColor;
                }
            }

            if (IsTextEqualEmptyTooltip())
            {
                innerTextBox.UseSystemPasswordChar = false;
                if (this.UseSystemPasswordChar)
                {
                    innerTextBox.ForeColor = this.EmptyTooltipForeColor;
                }
            }
            else
            {
                innerTextBox.UseSystemPasswordChar = this.UseSystemPasswordChar;
                if (this.UseSystemPasswordChar)
                {
                    innerTextBox.ForeColor = this.ForeColor;
                }
            }


        }

        private void innerTextBox_Leave(object sender, EventArgs e)
        {
            if (innerTextBox.Text != string.Empty)
            {
                if (IsTextEqualEmptyTooltip())
                {
                    innerTextBox.ForeColor = this.EmptyTooltipForeColor;
                }
                else
                {
                    innerTextBox.ForeColor = this.ForeColor;
                }
            }
            else
            {
                innerTextBox.Text = this.EmptyTooltipText;
                innerTextBox.ForeColor = this.EmptyTooltipForeColor;
                this.BeginInvoke((MethodInvoker)delegate {
                    innerTextBox.UseSystemPasswordChar = false;
                });
            }

            if (ButtonClickWorking)
            {
                return;
            }

            _innerTextBoxState = TextBoxStates.Normal;
            this.Invalidate();

            this.OnLeave(e);
        }

        private void innerTextBox_LostFocus(object sender, EventArgs e)
        {
            if (IsTextEqualEmptyTooltip())
            {
                innerTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                innerTextBox.UseSystemPasswordChar = this.UseSystemPasswordChar;
            }
        }

        private void innerTextBox_TextChanged(object sender, EventArgs e)
        {
            string value = innerTextBox.Text;
            if (value != null && value.Length > 0 && value != EmptyTooltipText && value != innerTextBox.Text)
            {
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

        /// <summary>
        /// 前景色
        /// </summary>
        [Category("Custom")]
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
                    innerTextBox.ForeColor = value;
                }
                else
                {
                    base.ForeColor = value;
                    innerTextBox.ForeColor = EmptyTooltipForeColor;
                }
            }
        }

        /// <summary>
        /// 停靠方式
        /// </summary>
        [Category("Custom")]
        public override DockStyle Dock
        {
            get
            {
                return base.Dock;
            }
            set
            {
                base.Dock = value;
                innerTextBox.Dock = DockStyle.Fill;
            }
        }
         

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
            {
                _innerTextBoxState = TextBoxStates.Normal;
                innerTextBox.Enabled = true;
            }
            else
            {
                _innerTextBoxState = TextBoxStates.Disabled;
                innerTextBox.Enabled = false;
            }
            this.Invalidate();
            base.OnEnabledChanged(e);
        }
         
        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);

            if (this.Dock != DockStyle.None)
            {
                ReizeTextBoxFont();
            }
            else
            {
                ResizePadding();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Dock != DockStyle.None)
            {
                ReizeTextBoxFont();
            }
            else
            {
                ResizePadding();
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
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

            //if (Text.Length == 0 && !string.IsNullOrEmpty(EmptyTextTip) && !Focused)
            //{
            //    TextRenderer.DrawText(g, EmptyTextTip, Font, ClientRectangle, EmptyTextTipColor, GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
            //}

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

        #endregion

        #region 私有方法

        private bool IsTextEqualEmptyTooltip()
        {
            bool result = false;
            if (innerTextBox.Text != string.Empty)
            {
                if (innerTextBox.Text == this.EmptyTooltipText)
                {
                    return true;
                } 
            }

            return result;
        }


        /// <summary>
        /// 初始化
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
            this.Padding = new Padding((this.Height - innerTextBox.Height) / 2);
            
            innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            innerTextBox.MouseEnter += new EventHandler(innerTextBox_MouseEnter);
            innerTextBox.MouseHover += new EventHandler(innerTextBox_MouseHover);
            innerTextBox.GotFocus += new EventHandler(innerTextBox_GotFocus);
            innerTextBox.LostFocus += new EventHandler(innerTextBox_LostFocus);

            innerTextBox.TextChanged += this.innerTextBox_TextChanged;
            innerTextBox.Enter += this.innerTextBox_Enter;
            innerTextBox.Leave += this.innerTextBox_Leave;

            innerTextBox.KeyDown += new KeyEventHandler(innerTextBox_KeyDown);
            innerTextBox.KeyPress += new KeyPressEventHandler(innerTextBox_KeyPress);
            innerTextBox.KeyUp += new KeyEventHandler(innerTextBox_KeyUp);

            btnDropDown.MouseEnter += new EventHandler(innerTextBox_MouseEnter);
            btnDropDown.MouseHover += innerTextBox_MouseHover;

            this.MouseEnter += innerTextBox_MouseEnter;
            this.MouseHover += innerTextBox_MouseHover;
            this.MouseLeave += new EventHandler(PaddingTextBox_MouseLeave);
            this.LostFocus += new EventHandler(PaddingTextBox_LostFocus);
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

                if (innerTextBox.Focused && !BorderHoverColor2.IsEmpty)
                {
                    highLightPen.Color = BorderHoverColor2;
                }
                g.DrawRoundedRectangle(highLightPen, drawRect, BorderRadius);

                //InnerRect
                drawRect.Inflate(-1, -1);
                highLightPen.Color = BorderFocusColor;
                if (innerTextBox.Focused && !BorderFocusColor2.IsEmpty)
                {
                    highLightPen.Color = BorderFocusColor2;
                }
                g.DrawRoundedRectangle(highLightPen, drawRect, BorderRadius);
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
            // What is the target size of the text box?
            float desiredheight = (float)TextBoxHeight;

            // Set the font from the existing TextBox font.
            // We use the fnt = new Font(...) method so we can ensure that
            //  we're setting the GraphicsUnit to Pixels.  This avoids all
            //  the DPI conversions between point & pixel.
            Font fnt = new Font(OriginalFont.FontFamily,
                                OriginalFont.Size,
                                OriginalFont.Style,
                                GraphicsUnit.Pixel);

            // TextBoxes never size below 8 pixels. This consists of the
            // 4 pixels above & 3 below of whitespace, and 1 pixel line of
            // greeked text.
            if (desiredheight < 8)
                desiredheight = 8;

            // Determine the Em sizes of the font and font line spacing
            // These values are constant for each font at the given font style.
            // and screen DPI.
            float FontEmSize = fnt.FontFamily.GetEmHeight(fnt.Style);
            float FontLineSpacing = fnt.FontFamily.GetLineSpacing(fnt.Style);

            // emSize is the target font size.  TextBoxes have a total of
            // 7 pixels above and below the FontHeight of the font.
            float emSize = (desiredheight - 7) * FontEmSize / FontLineSpacing;

            // Create the font, with the proper size to change the TextBox Height to the desired size.
            fnt = new Font(fnt.FontFamily, emSize, fnt.Style, GraphicsUnit.Pixel);

            return fnt;
        }

        private void ReizeTextBoxFont()
        {
            if (AutoSizeFont)
            {
                int height = this.Height - this.Padding.Top - this.Padding.Bottom;
                innerTextBox.Font = GetFontForTextBoxHeight(height, this.Font);
            }
            else
            {
                innerTextBox.Font = this.Font;
                this.ResizePadding();
                this.Invalidate();
            }

        }

        private void ResizePadding()
        {
            if (this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
            {
                int value = 3;
                using (Graphics g = this.CreateGraphics())
                {
                    SizeF size = g.MeasureString(innerTextBox.Text, innerTextBox.Font);
                    value = (int)size.Width;

                    if (value > this.Width)
                    {
                        value = 3;
                    }
                    else
                    {
                        value = (this.Width - value) / 2;
                    }
                }
                this.Padding = new Padding(value + 2, value, value, value);
            }
            else
            {
                int value = 3;

                string text = innerTextBox.Text;
                if (string.IsNullOrEmpty(text))
                {
                    text = EmptyTooltipText;
                }

                if (string.IsNullOrEmpty(text))
                {
                    return;
                }
                SizeF size = TextRenderer.MeasureText(text, innerTextBox.Font);

                int textWidth = (int)size.Width;
                int textHeight = (int)size.Height;

                //按宽度来，还是高度来算
                if (this.Height > this.Width)
                {
                    // Left Top
                    if (textWidth > this.Width)
                    {
                        value = textWidth / 4;
                    }
                    else
                    {
                        value = (this.Width - textWidth) / 4;
                    }
                }
                else
                {
                    // Left Center

                    // 如果获取Height失败
                    if (textHeight > this.Height)
                    {
                        value = textHeight / 4;
                    }
                    else
                    {
                        value = (this.Height - textHeight) / 4;
                    }
                }

                this.Padding = new Padding(value + 2, value, value, value);
            }

            if (ShowDropDownButton)
            {
                this.Padding = new Padding(this.Padding.Left, this.Padding.Top, this.btnDropDown.Width + 2, this.Padding.Bottom);
            }
            this.btnDropDown.Size = new Size(this.btnDropDown.Width, this.Height - 4);
            this.btnDropDown.Location = new Point(this.Width - this.btnDropDown.Width - (int)this.btnDropDown.BorderWidth * 2, (this.Height - this.btnDropDown.Height) / 2);

        }

        #endregion


        private void UpdateForeColor()
        {
            if (IsTextEqualEmptyTooltip())
            {
                innerTextBox.ForeColor = this.EmptyTooltipForeColor;
            }
            else
            {
                innerTextBox.ForeColor = this.ForeColor;
            }
        }
        

        private void btnDropDown_MouseClick(object sender, MouseEventArgs e)
        {
            if (ActionBegin != null)
            {
                ActionBegin();
            }
        }
        
    }
}
