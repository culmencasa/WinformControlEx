using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
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

        #endregion

        #region 字段

        private TextBoxStates _innerTextBoxState = TextBoxStates.Normal;

        private string _emptyTooltipText; 
        private bool _autoSizeFont;
        private bool _useSystemPasswordChar;

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
        [Category("自定义内容")]
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
        [Category("自定义内容")]
        public Color EmptyTooltipForeColor { get; set; }
         
        /// <summary>
        /// 文本框的内容
        /// </summary>
        [Category("自定义内容")]
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

                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        innerTextBox.UseSystemPasswordChar = false;
                    });
                }
                UpdateForeColor();
            }
        }

        /// <summary>
        /// 文本框状态
        /// </summary>
        public TextBoxStates TextBoxState
        {
            get { return _innerTextBoxState; }
            set { _innerTextBoxState = value; this.Invalidate(); }
        }


        /// <summary>
        /// 边框色
        /// </summary>
        [Category("自定义外观")]
        public Color BorderColor { get; set; }
         
        /// <summary>
        /// 边框悬浮色
        /// </summary>
        [Category("自定义外观")]
        public Color BorderHoverColor { get; set; }

        [Category("自定义外观")]
        public Color BorderHoverColor2 { get; set; }

        /// <summary>
        /// 获得焦点后的边框色
        /// </summary>
        [Category("自定义外观")]
        public Color BorderFocusColor { get; set; }

        [Category("自定义外观")]
        public Color BorderFocusColor2 { get; set; }



        /// <summary>
        /// 是否自动缩放字体
        /// </summary>
        [Category("自定义行为")]
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
        [Category("自定义行为")]
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
        [Category("自定义行为")]
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

        public int BorderRadius { get; set; }

        #endregion

        #region 事件处理

        private void PaddingTextBox_LostFocus(object sender, EventArgs e)
        {
            if (ButtonClickWorking)
                return;

            _innerTextBoxState = TextBoxStates.Normal;
            this.Invalidate();
        }

        private void PaddingTextBox_MouseLeave(object sender, EventArgs e)
        {
            if (ButtonClickWorking)
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

        /// <summary>
        /// 前景色
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return Color.Black;
            }
            set
            {
                base.ForeColor = value;
                if (!IsTextEqualEmptyTooltip())
                {
                    innerTextBox.ForeColor = value;
                }
                else
                {
                    innerTextBox.ForeColor = EmptyTooltipForeColor;
                }
            }
        }

        /// <summary>
        /// 停靠方式
        /// </summary>
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

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;             
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


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
            this.BorderColor = Color.Gray;
            this.BorderHoverColor = Color.SkyBlue;
            this.BorderFocusColor = Color.DodgerBlue;

            this.EmptyTooltipText = string.Empty;
            this.EmptyTooltipForeColor = Color.Gray;

            this.BorderRadius = 8;

            this.ForeColor = Color.Black;
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
                using (Graphics g = this.CreateGraphics())
                {
                    SizeF size = TextRenderer.MeasureText(innerTextBox.Text, innerTextBox.Font);
                     
                    int textWidth = (int)size.Width;

                    //按宽度来，还是高度来算
                    if (this.Height > this.Width)
                    { 
                        
                    }
                    if (textWidth > this.Width)
                    {
                        value = (this.Width - textWidth) / 2;
                    }
                    else
                    {
                        value = (this.Height - innerTextBox.Height) / 2;
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
