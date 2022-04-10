using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils.UI;

namespace Ocean
{

    /// <summary>
    /// 
    /// </summary>
    public class OcnButton : CustomButton
    {
        public OcnButton()
        {
            IsOutline = true;
            GradientMode = false;
            ShadeMode = false;
            CornerRadius = 8;
            RoundCorners = Corners.TopLeft | Corners.TopRight | Corners.BottomLeft | Corners.BottomRight;
            BorderColor = Color.FromArgb(121, 82, 179);
            ForeColor = Color.FromArgb(121, 82, 179);
            MouseOverBackColor = Color.FromArgb(108, 17, 150);
            MouseOverForeColor = Color.White;

            Size = new Size(97, 35);
            Text = "35px按钮";


        }

        public enum Themes
        {
            Primary,
            Secondary,
            Success,
            Danger,
            Warning,
            Info,
            Light,
            Dark
        }

        private Themes _theme;
        private bool _isOutline;
        private Color _savedBackColor;
        private Color _savedForeColor;

        public Themes Theme
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

        [Category("Custom")]
        public bool IsOutline
        {
            get
            {
                return _isOutline;
            }
            set
            {
                _isOutline = value;
                OnThemeChanged();
                Invalidate();
            }
        }

        [Category("Custom")]
        public Color MouseOverBackColor
        {
            get;
            set;
        }

        [Category("Custom")]
        public Color MouseOverForeColor
        {
            get;
            set;
        }

        public OceanPresets Presets
        {
            get;
            set;
        } = OceanPresets.Instance;

        protected virtual void OnThemeChanged()
        {

            switch (Theme)
            {
                case Themes.Primary:
                    ApplyPrimary();
                    break;
                case Themes.Secondary:
                    ApplySecondary();
                    break;
                case Themes.Success:
                    ApplySuccess();
                    break;
                case Themes.Danger:
                    ApplyDanger();
                    break;
                case Themes.Warning:
                    ApplyWarning();
                    break;
                case Themes.Info:
                    ApplyInfo();
                    break;
                case Themes.Light:
                    ApplyLight();
                    break;
                case Themes.Dark:
                    ApplyDark();
                    break;
            }
        }


        private void ApplyPrimary()
        {
            if (IsOutline)
            {
                BorderColor = Presets.PrimaryColor;
                BackColor = Color.White;
                ForeColor = Presets.PrimaryColor;
                MouseOverBackColor = Color.FromArgb(108, 17, 150);
                MouseOverForeColor = Color.White;
            }
            else
            {
                BorderColor = Presets.PrimaryColor;
                BackColor = Presets.PrimaryColor;
                ForeColor = Color.White;
                MouseOverBackColor = ColorEx.DarkenColor(Presets.PrimaryColor, 20); // Color.FromArgb(108, 17, 150);
                MouseOverForeColor = Color.White;
            }
        }

        private void ApplySecondary()
        {
            if (IsOutline)
            {
                BorderColor = Presets.SecondaryColor;
                BackColor = Color.White;
                ForeColor = Presets.SecondaryColor;
                MouseOverBackColor = Presets.SecondaryColor;
                MouseOverForeColor = Color.White;
            }
            else
            {
                BorderColor = Presets.SecondaryColor;
                BackColor = Presets.SecondaryColor;
                ForeColor = Color.White;
                MouseOverBackColor = ColorEx.DarkenColor(Presets.SecondaryColor, 20);
                MouseOverForeColor = Color.White;
            }
        }
        private void ApplySuccess()
        {

            if (IsOutline)
            {
                BorderColor = Presets.SuccessColor;
                BackColor = Color.White;
                ForeColor = Presets.SuccessColor;
                MouseOverBackColor = Presets.SuccessColor;
                MouseOverForeColor = Color.White;
            }
            else
            {
                BorderColor = Presets.SuccessColor;
                BackColor = Presets.SuccessColor;
                ForeColor = Color.White;
                MouseOverBackColor = ColorEx.DarkenColor(Presets.SuccessColor, 20);
                MouseOverForeColor = Color.White;
            }
        }

        private void ApplyDanger()
        {
            if (IsOutline)
            {
                BorderColor = Presets.DangerColor;
                BackColor = Color.White;
                ForeColor = Presets.DangerColor;
                MouseOverBackColor = Presets.DangerColor;
                MouseOverForeColor = Color.White;
            }
            else
            {
                BorderColor = Presets.DangerColor;
                BackColor = Presets.DangerColor;
                ForeColor = Color.White;
                MouseOverBackColor = ColorEx.DarkenColor(Presets.DangerColor, 20);
                MouseOverForeColor = Color.White;
            }
        }
        private void ApplyWarning()
        {
            if (IsOutline)
            {
                BorderColor = Presets.WarningColor;
                BackColor = Color.White;
                ForeColor = Presets.WarningColor;
                MouseOverBackColor = Presets.WarningColor;
                MouseOverForeColor = Color.White;
            }
            else
            {
                BorderColor = Presets.WarningColor;
                BackColor = Presets.WarningColor;
                ForeColor = Color.Black;
                MouseOverBackColor = ColorEx.LightenColor(Presets.WarningColor, 20);
                MouseOverForeColor = Color.Black;
            }
        }

        private void ApplyInfo()
        {
            if (IsOutline)
            {
                BorderColor = Presets.InfoColor;
                BackColor = Color.White;
                ForeColor = Presets.InfoColor;
                MouseOverBackColor = Presets.InfoColor;
                MouseOverForeColor = Color.Black;
            }
            else
            {
                BorderColor = Presets.InfoColor;
                BackColor = Presets.InfoColor;
                ForeColor = Color.Black;
                MouseOverBackColor = ColorEx.LightenColor(Presets.InfoColor, 20);
                MouseOverForeColor = Color.Black;
            }
        }

        private void ApplyLight()
        {
            if (IsOutline)
            {
                BorderColor = ColorEx.DarkenColor(Presets.LightColor, 20);
                BackColor = Color.White;
                ForeColor = Color.Black;
                MouseOverBackColor = Presets.LightColor; 
                MouseOverForeColor = Color.Black;
            }
            else
            {
                BorderColor = Presets.LightColor;
                BackColor = Presets.LightColor;
                ForeColor = Color.Black;
                MouseOverBackColor = ColorEx.LightenColor(Presets.LightColor, 20);
                MouseOverForeColor = Color.Black;
            }
        }

        private void ApplyDark()
        {
            if (IsOutline)
            {
                BorderColor = Presets.DarkColor;
                BackColor = Color.White;
                ForeColor = Presets.DarkColor;
                MouseOverBackColor = Presets.DarkColor;
                MouseOverForeColor = Color.White;
            }
            else
            {
                BorderColor = Presets.DarkColor;
                BackColor = Presets.DarkColor;
                ForeColor = Color.White;
                MouseOverBackColor = ColorEx.DarkenColor(Presets.DarkColor, 20);
                MouseOverForeColor = Color.White;
            }
        }

        #region 鼠标悬浮效果

        Timer _mouseOverTimer = null;
        Timer _mouseLeaveTimer = null;
        int colorTransition = 100;


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            if (IsOutline)
            {
                if (_mouseOverTimer == null)
                {
                    _savedBackColor = BackColor;
                    _savedForeColor = ForeColor;

                    colorTransition = 100;
                    _mouseOverTimer = new Timer();
                    _mouseOverTimer.Interval = 10;
                    _mouseOverTimer.Tick += mouseOverTimer_Tick;
                    _mouseOverTimer.Start();
                }
            }
            else
            {
                _savedBackColor = BackColor;
                _savedForeColor = ForeColor;

                BackColor = MouseOverBackColor;
                ForeColor = MouseOverForeColor;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsOutline)
            {
                if (_mouseLeaveTimer == null)
                {
                    colorTransition = 10;
                    _mouseLeaveTimer = new Timer();
                    _mouseLeaveTimer.Interval = 10;
                    _mouseLeaveTimer.Tick += mouseLeaveTimer_Tick; ;
                    _mouseLeaveTimer.Start();
                }
            }
            else
            {
                BackColor = _savedBackColor;
                ForeColor = _savedForeColor;
            }
        }

        private void mouseOverTimer_Tick(object sender, EventArgs e)
        {
            BackColor = ColorEx.LightenColor(MouseOverBackColor, colorTransition);
            ForeColor = MouseOverForeColor;
            colorTransition -= 10;
            if (colorTransition <= 50)
            {
                BackColor = MouseOverBackColor;
                _mouseOverTimer.Stop();
                _mouseOverTimer = null;
            }
            Invalidate();
        }

        private void mouseLeaveTimer_Tick(object sender, EventArgs e)
        {
            BackColor = ColorEx.LightenColor(MouseOverBackColor, colorTransition);
            colorTransition += 10;
            if (colorTransition >= 50)
            {
                BackColor = _savedBackColor;
                ForeColor = _savedForeColor;
                _mouseLeaveTimer.Stop();
                _mouseLeaveTimer = null;
            }
            Invalidate();
        }


        #endregion
    }
}
