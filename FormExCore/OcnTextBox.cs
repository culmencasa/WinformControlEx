using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Utils.UI;
using static System.Windows.Forms.RoundTextBox;

namespace FormExCore
{
    [DefaultEvent("TextChanged")]
    public partial class OcnTextBox : UserControl
    {

        #region 构造

        public OcnTextBox()
        {
            Initialize();
            Theme = OcnThemes.Primary;
        }

        #endregion

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

        #region 事件

        [Browsable(true)]
        public new event EventHandler? TextChanged;

        #endregion

        #region 字段

        private TextBox _innerTextBox = new TextBox();
        private TextBoxStates _innerTextBoxState = TextBoxStates.Normal;
        private Color _borderColor = Color.MediumSlateBlue;
        private Color _borderFocusColor = Color.Empty;
        private int _borderSize = 1;
        private bool _underlinedStyle = false;

        private bool _readOnly = false;
        private bool _enabled = true;

        private int _borderRadius = 8;
        private Color _placeholderColor = Color.DarkGray;
        private string _placeholderText = "";
        private bool _isPlaceholder = false;
        private bool _usePasswordChar = false;
        private string _text;


        private OcnThemes _theme;

        #endregion

        #region 设计器属性

        /// <summary>
        /// 
        /// </summary>
        [Category("Custom")]

        public OcnThemes Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                OnThemeChanged();
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OceanPresets Preset
        {
            get;
            private set;
        } = OceanPresets.Instance;


        [Category("Custom")]
        [DefaultValue(typeof(Padding), "10, 5, 10, 5")]
        public new Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }

        [Category("Custom")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        public Color BorderFocusColor
        {
            get { return _borderFocusColor; }
            set { _borderFocusColor = value; }
        }

        [Category("Custom"), DefaultValue(typeof(Color), "Gray")]
        public Color DisabledColor
        {
            get;
            set;
        } = Color.Gray;

        [Category("Custom")]
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                if (value >= 1)
                {
                    _borderSize = value;
                    Invalidate();
                }
            }
        }

        [Category("Custom")]
        public bool UnderlinedStyle
        {
            get { return _underlinedStyle; }
            set
            {
                _underlinedStyle = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        public bool UsePasswordChar
        {
            get { return _usePasswordChar; }
            set
            {
                _usePasswordChar = value;

                _innerTextBox.UseSystemPasswordChar = value;
            }
        }


        [Category("Custom")]
        public bool Multiline
        {
            get { return _innerTextBox.Multiline; }
            set { _innerTextBox.Multiline = value; }
        }

        [Category("Custom")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                _innerTextBox.BackColor = value;
            }
        }

        [Category("Custom")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                _innerTextBox.ForeColor = value;
            }
        }

        [Category("Custom")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                _innerTextBox.Font = value;
                if (DesignMode)
                    UpdateTextBoxLocation();
            }
        }

        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [Bindable(true)]
        public override string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                _innerTextBox.Text = value;
                SetPlaceholder();
                Invalidate();
            }
        }

        [Category("Custom")]
        public int BorderRadius
        {
            get
            {
                return _borderRadius;
            }
            set
            {
                if (value >= 0)
                {
                    _borderRadius = value;
                    Invalidate();
                }
            }
        }

        [Category("Custom")]
        public Color PlaceholderColor
        {
            get
            {
                return _placeholderColor;
            }
            set
            {
                _placeholderColor = value;
            }
        }

        [Category("Custom")]
        public string PlaceholderText
        {
            get
            {
                return _placeholderText;
            }
            set
            {
                _placeholderText = value;
                SetPlaceholder();
                Invalidate();
            }
        }


        [Category("Custom")]
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                OnReadOnlyChanged();
            }
        }


        #endregion

        #region 属性

        protected bool ThemeApplied
        {
            get;
            set;
        }

        #endregion

        #region 公开的方法

        public new void Focus()
        {
            _innerTextBox.Select();
        }

        public void Clear()
        {
            _innerTextBox.Text = string.Empty;
        }

        #endregion

        #region 可重写的方法

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (DesignMode)
            {
                UpdateTextBoxLocation();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateTextBoxLocation();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;


            var borderPenColor = Color.Empty;
            switch (_innerTextBoxState)
            {
                case TextBoxStates.Normal:
                    borderPenColor = BorderColor;
                    break;
                case TextBoxStates.Highlight:
                    borderPenColor = BorderFocusColor;
                    break;
                case TextBoxStates.Disabled:
                    borderPenColor = DisabledColor;
                    break;
                default:
                    break;
            }


            if (_borderRadius > 1)
            {
                var rectBorderSmooth = ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -_borderSize, -_borderSize);
                int smoothSize = _borderSize > 0 ? _borderSize : 1;




                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, _borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, _borderRadius - _borderSize))
                using (Pen penBorderSmooth = new Pen(Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderPenColor, _borderSize))
                {
                    Region = new Region(pathBorderSmooth);
                    if (_borderRadius > 15)
                    {
                        SetTextBoxRoundedRegion();
                    }
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = PenAlignment.Center;


                    if (_underlinedStyle)
                    {
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, Height - _borderSize, Width, Height - _borderSize);
                    }
                    else
                    {
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        graph.DrawPath(penBorder, pathBorder);
                    }
                }
            }
            else
            {
                using (Pen penBorder = new Pen(borderPenColor, _borderSize))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;

                    if (_underlinedStyle)
                    {
                        graph.DrawLine(penBorder, 0, Height - _borderSize, Width, Height - _borderSize);
                    }
                    else
                    {
                        graph.DrawRectangle(penBorder, 0, 0, Width - _borderSize, Height - _borderSize);
                    }
                }
            }


            // 绘制占位符文本
            if (_isPlaceholder)
            {
                using (Brush placeholderBrush = new SolidBrush(_placeholderColor))
                {
                    SizeF textSize = graph.MeasureString(_placeholderText, Font);
                    float verticalPosition = (Height - textSize.Height) / 2;

                    graph.DrawString(_placeholderText, Font, placeholderBrush, new PointF(this.Padding.Left + this.BorderSize, verticalPosition)); // 水平位置固定为10
                }
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

        protected virtual void OnTextChanged()
        {
            if (TextChanged != null)
            {
                TextChanged.Invoke(_innerTextBox, EventArgs.Empty);
            }
        }

        protected virtual void OnReadOnlyChanged()
        {
            if (ReadOnly)
            {
                _innerTextBox.ReadOnly = true;
            }
            else
            {
                _innerTextBox.ReadOnly = false;
            }
        }




        #endregion

        #region 自身事件

        private void OcnTextBox_Enter(object? sender, EventArgs e)
        {
            _innerTextBox.Visible = true;
            _innerTextBox.Focus();
            Invalidate();
        }
        private void OcnTextBox_Leave(object? sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Normal;
            SetPlaceholder();
            Invalidate();
        }

        private void OcnTextBox_MouseEnter(object? sender, EventArgs e)
        {

        }

        private void OcnTextBox_GotFocus(object? sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Highlight;
            this.Invalidate();
        }


        #endregion

        #region InnertTextBox事件


        private void InnerTextBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged();
        }
        private void InnerTextBox_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
        private void InnerTextBox_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }
        private void InnerTextBox_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }
        private void InnerTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void InnerTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void InnerTextBox_Enter(object? sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Highlight;
            RemovePlaceholder();
            Invalidate();
        }

        private void InnerTextBox_Leave(object? sender, EventArgs e)
        {
            _innerTextBoxState = TextBoxStates.Normal;
            Invalidate();
        }

        private void InnerTextBox_GotFocus(object? sender, EventArgs e)
        {
            this.OnGotFocus(e);
        }

        private void InnerTextBox_LostFocus(object? sender, EventArgs e)
        {
        }

        #endregion

        #region 私有方法

        private void Initialize()
        {
            this.SuspendLayout();
            Padding = new Padding(10, 5, 10, 5);
            MinimumSize = new Size(30, 22);

            MouseEnter += OcnTextBox_MouseEnter;
            Enter += OcnTextBox_Enter;
            Leave += OcnTextBox_Leave;
            GotFocus += OcnTextBox_GotFocus;

            _innerTextBox.BorderStyle = BorderStyle.None;
            _innerTextBox.Anchor = AnchorStyles.Left;
            _innerTextBox.Size = new Size(230, 17);
            _innerTextBox.Click += InnerTextBox_Click;
            _innerTextBox.TextChanged += InnerTextBox_TextChanged;
            _innerTextBox.Enter += InnerTextBox_Enter;
            _innerTextBox.KeyPress += InnerTextBox_KeyPress;
            _innerTextBox.Leave += InnerTextBox_Leave;
            _innerTextBox.MouseEnter += InnerTextBox_MouseEnter;
            _innerTextBox.MouseLeave += InnerTextBox_MouseLeave;

            _innerTextBox.KeyDown += InnerTextBox_KeyDown;
            _innerTextBox.GotFocus += InnerTextBox_GotFocus;
            _innerTextBox.LostFocus += InnerTextBox_LostFocus;
            this.Controls.Add(_innerTextBox);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private bool IsTextEqualPlaceholderText()
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(_innerTextBox.Text))
            {
                if (_innerTextBox.Text == this.PlaceholderText)
                {
                    return true;
                }
            }

            return result;
        }

        private void SetPlaceholder()
        {
            if (_innerTextBox.Text.Length == 0 && _placeholderText.Length > 0)
            {
                _isPlaceholder = true;
                _innerTextBox.Visible = false;
            }
            else
            {
                _isPlaceholder = false;
                _innerTextBox.Visible = true;
            }
        }

        private void RemovePlaceholder()
        {
            _isPlaceholder = false;
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void SetTextBoxRoundedRegion()
        {
            GraphicsPath pathTxt;
            if (Multiline)
            {
                pathTxt = GetFigurePath(_innerTextBox.ClientRectangle, _borderRadius - _borderSize);
                _innerTextBox.Region = new Region(pathTxt);
            }
            else
            {
                pathTxt = GetFigurePath(_innerTextBox.ClientRectangle, _borderSize * 2);
                _innerTextBox.Region = new Region(pathTxt);
            }
            pathTxt.Dispose();
        }

        private void UpdateTextBoxLocation()
        {
            int txtHeight = TextRenderer.MeasureText("Text", Font).Height + 1;
            _innerTextBox.Location = new Point(Padding.Left + BorderSize + 1, (Height - txtHeight) / 2);
            _innerTextBox.Width = this.Width - Padding.Left - Padding.Right - BorderSize * 2;

        }


        #endregion

        #region 主题改变

        protected virtual void OnThemeChanged()
        {
            switch (Theme)
            {
                case OcnThemes.Primary:
                    ApplyPrimary();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Secondary:
                    ApplySecondary();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Success:
                    ApplySuccess();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Danger:
                    ApplyDanger();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Warning:
                    ApplyWarning();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Info:
                    ApplyInfo();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Light:
                    ApplyLight();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Dark:
                    ApplyDark();
                    ThemeApplied = true;
                    break;
                default:
                    ThemeApplied = false;
                    break;
            }
        }

        private void ApplyPrimary()
        {
            BorderColor = Preset.PrimaryColor;
            BackColor = Color.White;
            ForeColor = Preset.PrimaryColor;
            BorderFocusColor = ColorEx.DarkenColor(Preset.PrimaryColor, 20);
        }

        private void ApplySecondary()
        {
            BorderColor = Preset.SecondaryColor;
            BackColor = Color.White;
            ForeColor = Preset.SecondaryColor;
            BorderFocusColor = ColorEx.DarkenColor(Preset.SecondaryColor, 20);
        }
        private void ApplySuccess()
        {
            BorderColor = Preset.SuccessColor;
            BackColor = Color.White;
            ForeColor = Preset.SuccessColor;
            BorderFocusColor = ColorEx.DarkenColor(Preset.SuccessColor, 20);
        }

        private void ApplyDanger()
        {
            BorderColor = Preset.DangerColor;
            BackColor = Color.White;
            ForeColor = Preset.DangerColor;
            BorderFocusColor = ColorEx.DarkenColor(Preset.DangerColor, 20);
        }
        private void ApplyWarning()
        {
            BorderColor = Preset.WarningColor;
            BackColor = Color.White;
            ForeColor = Preset.WarningColor;
            BorderFocusColor = ColorEx.DarkenColor(Preset.WarningColor, 20);
        }

        private void ApplyInfo()
        {
            BorderColor = Preset.InfoColor;
            BackColor = Color.White;
            ForeColor = Preset.InfoColor;
            BorderFocusColor = ColorEx.DarkenColor(Preset.InfoColor, 20);
        }

        private void ApplyLight()
        {
            BorderColor = ColorEx.DarkenColor(Preset.LightColor, 20);
            BackColor = Color.White;
            ForeColor = Color.Black;
            BorderFocusColor = ColorEx.DarkenColor(Preset.InfoColor, 20);
        }

        private void ApplyDark()
        {
            BorderColor = Preset.DarkColor;
            BackColor = Color.White;
            ForeColor = Preset.DarkColor;
            BorderFocusColor = ColorEx.DarkenColor(Preset.DarkColor, 20);
        }


        #endregion


    }
}
