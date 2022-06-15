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
            InitializeComponent();

            Theme = OcnThemes.Primary;

            innerTextBox.KeyDown += InnerTextBox_KeyDown;
            innerTextBox.GotFocus += InnerTextBox_GotFocus;
            innerTextBox.LostFocus += InnerTextBox_LostFocus;
            innerTextBox.Enter += InnerTextBox_Enter;
            innerTextBox.Leave += InnerTextBox_Leave;
        }


        TextBoxStates innerTextBoxState = TextBoxStates.Normal;
        private void InnerTextBox_Enter(object? sender, EventArgs e)
        {
            if (innerTextBox.Text != string.Empty)
            {
                if (IsTextEqualPlaceholderText())
                {
                    // 获得焦点时，清除提示文字
                    innerTextBox.Text = string.Empty;
                    innerTextBox.ForeColor = this.ForeColor;
                }
            }

            if (IsTextEqualPlaceholderText())
            {
                innerTextBox.UseSystemPasswordChar = false;
                if (this.UsePasswordChar)
                {
                    innerTextBox.ForeColor = this.PlaceholderForeColor;
                }
            }
            else
            {
                innerTextBox.UseSystemPasswordChar = this.UsePasswordChar;
                if (this.UsePasswordChar)
                {
                    innerTextBox.ForeColor = this.PlaceholderForeColor;
                }
                else
                {
                    innerTextBox.ForeColor = this.ForeColor;
                }
            }

        }
        private void InnerTextBox_Leave(object? sender, EventArgs e)
        {
            if (innerTextBox.Text != string.Empty)
            {
                if (IsTextEqualPlaceholderText())
                {
                    innerTextBox.ForeColor = this.PlaceholderForeColor;
                }
                else
                {
                    innerTextBox.ForeColor = this.ForeColor;
                }
            }
            else
            {
                innerTextBox.Text = this.PlaceholderText;
                innerTextBox.ForeColor = this.PlaceholderForeColor;
                this.BeginInvoke((MethodInvoker)delegate
                {
                    innerTextBox.UseSystemPasswordChar = false;
                });
            }

            innerTextBoxState = TextBoxStates.Normal;
            this.Invalidate();

            this.OnLeave(e);
        }

        private void InnerTextBox_GotFocus(object? sender, EventArgs e)
        {
            innerTextBoxState = TextBoxStates.Highlight;
            if (IsTextEqualPlaceholderText())
            {
                innerTextBox.UseSystemPasswordChar = false;
                innerTextBox.ForeColor = this.PlaceholderForeColor;
            }
            else
            {
                innerTextBox.UseSystemPasswordChar = UsePasswordChar;
                innerTextBox.ForeColor = this.ForeColor;
            }
            this.Invalidate();
        }

        private void InnerTextBox_LostFocus(object? sender, EventArgs e)
        {
            if (IsTextEqualPlaceholderText())
            {
                innerTextBox.UseSystemPasswordChar = false;
                innerTextBox.ForeColor = this.PlaceholderForeColor;
            }
            else
            {
                innerTextBox.UseSystemPasswordChar = this.UsePasswordChar;

                innerTextBox.ForeColor = this.ForeColor;
            }

        }

        #endregion

        #region 事件

        [Browsable(true)]
        public new event EventHandler? TextChanged;

        #endregion

        #region 字段

        private Color _borderColor = Color.MediumSlateBlue;
        private Color _borderFocusColor = Color.Empty;
        private int _borderSize = 1;
        private bool _underlinedStyle = false;
        private bool _isFocused = false;

        private int _borderRadius = 8;
        private Color _placeholderColor = Color.DarkGray;
        private string _placeholderText = "";
        private bool _isPlaceholder = false;
        private bool _usePasswordChar = false;


        private OcnThemes _theme;

        #endregion

        #region 属性

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
        protected bool ThemeApplied
        {
            get;
            set;
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OceanPresets Presets
        {
            get;
            private set;
        } = OceanPresets.Instance;



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


                innerTextBox.UseSystemPasswordChar = value;
            }
        }

        [Category("Custom")]
        public Color PlaceholderForeColor { get; set; } = Color.Gray;

        [Category("Custom")]
        public bool Multiline
        {
            get { return innerTextBox.Multiline; }
            set { innerTextBox.Multiline = value; }
        }

        [Category("Custom")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                innerTextBox.BackColor = value;
            }
        }

        [Category("Custom")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                innerTextBox.ForeColor = value;
            }
        }

        [Category("Custom")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                innerTextBox.Font = value;
                if (DesignMode)
                    UpdateControlHeight();
            }
        }

        [Category("Custom")]
        [Browsable(true)]
        public override string Text
        {
            get
            {
                if (_isPlaceholder)
                {
                    return "";
                }
                else return innerTextBox.Text;
            }
            set
            {
                innerTextBox.Text = value;
                //SetPlaceholder();
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
                if (_isPlaceholder)
                    innerTextBox.ForeColor = value;
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
            }
        }


        #endregion

        #region 公开的方法

        public new void Focus()
        {
            innerTextBox.Select();
        }

        public void Clear()
        {
            innerTextBox.Text = string.Empty;
        }

        #endregion

        #region 重写的方法
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (DesignMode)
            {
                UpdateControlHeight();
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlHeight();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graph = e.Graphics;

            if (_borderRadius > 1)
            {
                var rectBorderSmooth = ClientRectangle;
                var rectBorder = Rectangle.Inflate(rectBorderSmooth, -_borderSize, -_borderSize);
                int smoothSize = _borderSize > 0 ? _borderSize : 1;

                using (GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, _borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, _borderRadius - _borderSize))
                using (Pen penBorderSmooth = new Pen(Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(_borderColor, _borderSize))
                {
                    Region = new Region(pathBorderSmooth);
                    if (_borderRadius > 15)
                    {
                        SetTextBoxRoundedRegion();
                    }
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    penBorder.Alignment = PenAlignment.Center;
                    if (_isFocused)
                    {
                        penBorder.Color = _borderFocusColor;
                    }

                    if (_underlinedStyle)
                    {
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        graph.SmoothingMode = SmoothingMode.None;
                        graph.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
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
                using (Pen penBorder = new Pen(_borderColor, _borderSize))
                {
                    Region = new Region(ClientRectangle);
                    penBorder.Alignment = PenAlignment.Inset;
                    if (_isFocused)
                    {
                        penBorder.Color = _borderFocusColor;
                    }

                    if (_underlinedStyle)
                    {
                        graph.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
                    }
                    else
                    {
                        graph.DrawRectangle(penBorder, 0, 0, Width - 0.5F, Height - 0.5F);
                    }
                }
            }
        }




        #endregion

        #region 私有方法

        private bool IsTextEqualPlaceholderText()
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(innerTextBox.Text))
            {
                if (innerTextBox.Text == this.PlaceholderText)
                {
                    return true;
                }
            }

            return result;
        }
        private void SetPlaceholder()
        {
            if (UsePasswordChar)
            {
                if (IsTextEqualPlaceholderText())
                {
                    _isPlaceholder = true;
                    innerTextBox.Text = _placeholderText;
                    innerTextBox.ForeColor = _placeholderColor;
                    innerTextBox.UseSystemPasswordChar = false;
                }
                else
                {
                    innerTextBox.UseSystemPasswordChar = UsePasswordChar;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(innerTextBox.Text) && _placeholderText != "")
                {
                    _isPlaceholder = true;
                    innerTextBox.Text = _placeholderText;
                    innerTextBox.ForeColor = _placeholderColor;
                }
                else
                {
                    _isPlaceholder = false;
                    innerTextBox.Text = _placeholderText;
                    innerTextBox.ForeColor = _placeholderColor;
                }
            }

        }

        private void RemovePlaceholder()
        {
            if (_isPlaceholder && _placeholderText != "")
            {
                _isPlaceholder = false;
                innerTextBox.Text = "";
                innerTextBox.ForeColor = ForeColor;
                if (_usePasswordChar)
                {
                    innerTextBox.UseSystemPasswordChar = true;
                }
            }
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
                pathTxt = GetFigurePath(innerTextBox.ClientRectangle, _borderRadius - _borderSize);
                innerTextBox.Region = new Region(pathTxt);
            }
            else
            {
                pathTxt = GetFigurePath(innerTextBox.ClientRectangle, _borderSize * 2);
                innerTextBox.Region = new Region(pathTxt);
            }
            pathTxt.Dispose();
        }

        private void UpdateControlHeight()
        {
            if (innerTextBox.Multiline == false)
            {
                int txtHeight = TextRenderer.MeasureText("Text", Font).Height + 1;
                innerTextBox.Multiline = true;
                innerTextBox.MinimumSize = new Size(0, txtHeight);
                innerTextBox.Multiline = false;

                Height = innerTextBox.Height + Padding.Top + Padding.Bottom;
            }
        }

        protected virtual void OnTextChanged()
        {
            if (TextChanged != null)
            {
                TextChanged.Invoke(innerTextBox, EventArgs.Empty);
            }
        }


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
            BorderColor = Presets.PrimaryColor;
            BackColor = Color.White;
            ForeColor = Presets.PrimaryColor;
            BorderFocusColor = ColorEx.DarkenColor(Presets.PrimaryColor, 20);
        }

        private void ApplySecondary()
        {
            BorderColor = Presets.SecondaryColor;
            BackColor = Color.White;
            ForeColor = Presets.SecondaryColor;
            BorderFocusColor = ColorEx.DarkenColor(Presets.SecondaryColor, 20);
        }
        private void ApplySuccess()
        {
            BorderColor = Presets.SuccessColor;
            BackColor = Color.White;
            ForeColor = Presets.SuccessColor;
            BorderFocusColor = ColorEx.DarkenColor(Presets.SuccessColor, 20);
        }

        private void ApplyDanger()
        {
            BorderColor = Presets.DangerColor;
            BackColor = Color.White;
            ForeColor = Presets.DangerColor;
            BorderFocusColor = ColorEx.DarkenColor(Presets.DangerColor, 20);
        }
        private void ApplyWarning()
        {
            BorderColor = Presets.WarningColor;
            BackColor = Color.White;
            ForeColor = Presets.WarningColor;
            BorderFocusColor = ColorEx.DarkenColor(Presets.WarningColor, 20);
        }

        private void ApplyInfo()
        {
            BorderColor = Presets.InfoColor;
            BackColor = Color.White;
            ForeColor = Presets.InfoColor;
            BorderFocusColor = ColorEx.DarkenColor(Presets.InfoColor, 20);
        }

        private void ApplyLight()
        {
            BorderColor = ColorEx.DarkenColor(Presets.LightColor, 20);
            BackColor = Color.White;
            ForeColor = Color.Black;
            BorderFocusColor = ColorEx.DarkenColor(Presets.InfoColor, 20);
        }

        private void ApplyDark()
        {
            BorderColor = Presets.DarkColor;
            BackColor = Color.White;
            ForeColor = Presets.DarkColor;
            BorderFocusColor = ColorEx.DarkenColor(Presets.DarkColor, 20);
        }


        #endregion



        #endregion


        #region innertTextBox事件


        private void innerTextBox_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged();
        }
        private void innerTextBox_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
        private void innerTextBox_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }
        private void innerTextBox_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }
        private void innerTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        private void innerTextBox_Enter(object sender, EventArgs e)
        {
            _isFocused = true;
            Invalidate();
            RemovePlaceholder();
        }
        private void innerTextBox_Leave(object sender, EventArgs e)
        {
            _isFocused = false;
            Invalidate();
            //SetPlaceholder();
        }

        private void InnerTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        #endregion
    }
}
