using FormExCore.ThirdParty;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Utils.UI;

namespace FormExCore
{
    /// <summary>
    /// 窗体
    /// </summary>
    public partial class OcnForm : Form
    {
        #region 构造

        public OcnForm()
        {
            InitializeComponent();

            Theme = OcnThemes.Primary;


            FormBorderStyle = FormBorderStyle.None;
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            StartPosition = FormStartPosition.CenterScreen;

        }

        #endregion

        #region 字段

        private OcnThemes _theme;
        private int _roundCornerDiameter;
        private string _customTitleText;
        private int _titleBarHeight;
        private Color _titleColor;
        private int _logoSize;
        private Image _logo;

        #endregion

        #region 属性

        #region 主题

        /// <summary>
        /// 主题
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
                bool changing = _theme != value;
                _theme = value;

                if (changing)
                {
                    OnThemeChanged();
                }
            }
        }
        protected bool ThemeApplied { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OceanPresets Presets
        {
            get;
            private set;
        } = OceanPresets.Instance;

        #endregion


        #region 无边框窗体属性

        public Color TitleBackColor
        {
            get;
            set;
        }


        [Category("Custom")]
        public int RoundCornerDiameter
        {
            get
            {
                return _roundCornerDiameter;
            }
            set
            {
                _roundCornerDiameter = value;
                // 设计时不改变窗体形状. 在HighDPI下宽高会不正常.
                if (IsHandleCreated && !DesignMode)
                    UpdateFormRoundCorner(value);
                Invalidate();
            }
        }

        /// <summary>
        /// 标题栏文字
        /// </summary>
        [Category("Custom")]
        public string TitleText
        {
            get
            {
                return _customTitleText;
            }
            set
            {
                _customTitleText = value;
                this.Text = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 标题栏高度
        /// </summary>
        [Category("Custom")]
        public int TitleBarHeight
        {
            get
            {
                return _titleBarHeight;
            }
            set
            {
                _titleBarHeight = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        public Color TitleColor
        {
            get
            {
                return _titleColor;
            }
            set
            {
                _titleColor = value;
            }
        }

        [Category("Custom")]
        public int LogoSize
        {
            get
            {
                return _logoSize;
            }
            private set
            {
                _logoSize = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        public Image Logo
        {
            get
            {
                return _logo;
            }
            set
            {
                _logo = value;
                if (value != null)
                {
                    _logoSize = value.Width;
                }
                Invalidate();
            }
        }


        #endregion


        #endregion

        #region 私有方法

        #region 主题改变

        protected virtual void OnThemeChanged()
        {
            // 防止在调用此方法时, 子类控件还未完全实例化而报空引用异常
            if (!this.IsHandleCreated)
            {
                EventHandler? onLoadRegister = null;
                onLoadRegister = delegate (object sender, EventArgs e)
                {
                    OnThemeChanged();
                    if (onLoadRegister != null)
                    {
                        this.HandleCreated -= onLoadRegister;
                    }
                };
                this.HandleCreated += onLoadRegister;
                return;
            }


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


        protected virtual void ApplyPrimary()
        {
            //BorderColor = Presets.PrimaryColor;
            BackColor = Color.White;
            ForeColor = Presets.PrimaryColor;
            TitleBackColor = Color.FromArgb(108, 17, 150);
            TitleColor = Color.White;
        }

        protected virtual void ApplySecondary()
        {
            //BorderColor = Presets.SecondaryColor;
            BackColor = Color.White;
            ForeColor = Presets.SecondaryColor;
            TitleBackColor = Presets.SecondaryColor;
            TitleColor = Color.White;
        }
        protected virtual void ApplySuccess()
        {
            //BorderColor = Presets.SuccessColor;
            BackColor = Color.White;
            ForeColor = Presets.SuccessColor;
            TitleBackColor = Presets.SuccessColor;
            TitleColor = Color.White;
        }

        protected virtual void ApplyDanger()
        {
            //BorderColor = Presets.DangerColor;
            BackColor = Color.White;
            ForeColor = Presets.DangerColor;
            TitleBackColor = Presets.DangerColor;
            TitleColor = Color.White;
        }
        protected virtual void ApplyWarning()
        {
            //BorderColor = Presets.WarningColor;
            BackColor = Color.White;
            ForeColor = Presets.WarningColor;
            TitleBackColor = Presets.WarningColor;
            TitleColor = Color.White;
        }

        protected virtual void ApplyInfo()
        {
            //BorderColor = Presets.InfoColor;
            BackColor = Color.White;
            ForeColor = Presets.InfoColor;
            TitleBackColor = Presets.InfoColor;
            TitleColor = Color.Black;
        }

        protected virtual void ApplyLight()
        {
            //BorderColor = ColorEx.DarkenColor(Presets.LightColor, 20);
            BackColor = Color.White;
            ForeColor = Color.Black;
            TitleBackColor = Presets.LightColor;
            TitleColor = Color.Black;
        }

        protected virtual void ApplyDark()
        {
            //BorderColor = Presets.DarkColor;
            BackColor = Color.White;
            ForeColor = Presets.DarkColor;
            TitleBackColor = Presets.DarkColor;
            TitleColor = Color.White;
        }


        #endregion

        protected void UpdateFormRoundCorner(int diameter)
        {
            if (diameter == 0)
            {
                if (WindowState == FormWindowState.Normal)
                {
                    if (MaximumSize.Width > 0 && MaximumSize.Height > 0)
                    {
                        Region = new Region(new Rectangle(0, 0, MaximumSize.Width, MaximumSize.Height));
                    }
                }
                else if (WindowState == FormWindowState.Maximized)
                {
                    Region = new Region(Screen.PrimaryScreen.WorkingArea);
                }
            }
            else
            {
                // 防止控件撑出窗体
                IntPtr hrgn = Win32.CreateRoundRectRgn(0, 0, Width, Height, diameter, diameter);
                Region = System.Drawing.Region.FromHrgn(hrgn);
                this.Update();
                Win32.DeleteObject(hrgn);
            }
        }

        #endregion

        #region 界面事件



        #endregion

        #region 重写的方法

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    if (MaximizeBox)
                    {
                        cp.Style |= (int)WindowStyle.WS_MAXIMIZEBOX;
                    }
                    if (MinimizeBox)
                    {
                        cp.Style |= (int)WindowStyle.WS_MINIMIZEBOX;
                    }
                }
                return cp;
            }
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_NCHITTEST:

                    Point pos = new Point(m.LParam.ToInt32());
                    pos = this.PointToClient(pos);
                    if (pos.Y > 0 && pos.Y < TitleBarHeight)
                    {
                        m.Result = new IntPtr(Win32.HTCAPTION);
                        return;
                    }

                    break;
            }

            if (m.Msg == 0xa3)
            {
                return;
            }


            if (m.Msg == Win32.WM_SYSCOMMAND)
            {
                int state = m.WParam.ToInt32();
                /* 双击任务栏放大的情况, 参见 
                 * https://msdn.microsoft.com/en-us/library/windows/desktop/ms646360%28v=vs.85%29.aspx
                 */
                if ((state & 0xFFF0) == Win32.SC_MAXMIZE) // 最大化 
                {
                }
                else if ((state & 0xFFF0) == Win32.SC_MINIMIZE)
                {
                }
                else if ((state & 0xFFF0) == Win32.SC_RESTORE)
                {

                }
                else
                {
                }
            }
            base.WndProc(ref m);
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                int gap = RoundCornerDiameter / 4; // 半径的一半
                Rectangle clientArea = new Rectangle();
                clientArea.X = gap;
                clientArea.Y = TitleBarHeight;
                clientArea.Width = this.Width - gap * 2;
                clientArea.Height = this.Height - TitleBarHeight - gap;

                return clientArea;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // 注：不能写到Resize事件里
            if (IsHandleCreated && !DesignMode)
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    UpdateFormRoundCorner(this.RoundCornerDiameter);
                }
                else
                {
                    UpdateFormRoundCorner(0);
                }
            }
        }

        protected virtual void DrawTitleBackground(Graphics g)
        {
            int radius = RoundCornerDiameter / 2;

            Rectangle titleBarArea = new Rectangle();
            titleBarArea.X = 0;
            titleBarArea.Y = 0;
            titleBarArea.Width = this.Width;
            titleBarArea.Height = TitleBarHeight;



            using (Brush titleBarBrush = new SolidBrush(TitleBackColor))
            {
                if (radius > 0)
                {
                    g.FillRoundedRectangle(titleBarBrush, titleBarArea, radius,
                        RectangleEdgeFilter.TopLeft | RectangleEdgeFilter.TopRight);
                }
                else
                {
                    g.FillRectangle(titleBarBrush, titleBarArea);
                }
            }

        }


        /// <summary>
        /// 画标题
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle DrawLogoAndTitle(Graphics g)
        {

            int left = 6;
            int top = 0;
            int width = 0;
            int height = 0;

            // 字体大小
            SizeF textSize = new SizeF();
            if (TitleText?.Length > 0)
            {
                textSize = TextRenderer.MeasureText(this.TitleText, this.Font);
            }


            // 整体标题区域大小
            width = LogoSize + (int)textSize.Width;
            height = (int)textSize.Height > LogoSize ? (int)textSize.Height : LogoSize;

            // 画图标
            if (Logo != null)
            {
                top = (this.TitleBarHeight - LogoSize) / 2;
                g.DrawImage(Logo, left, top, LogoSize, LogoSize);
            }

            // 标题
            if (!string.IsNullOrEmpty(TitleText))
            {
                top = (this.TitleBarHeight - (int)textSize.Height) / 2;
                TextRenderer.DrawText(
                    g,
                    TitleText,
                    Font,
                    new Rectangle(
                        left,
                        top,
                        width,
                        height),
                    TitleColor,
                    TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
            }


            return new Rectangle(left, top, width, height);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SmoothingMode smooth = g.SmoothingMode;

            base.OnPaint(e);

            g.SmoothingMode = SmoothingMode.HighSpeed;


            DrawTitleBackground(g);

            //if (this.WindowState == FormWindowState.Normal)
            //{
            //    if (BorderSize > 0)
            //    {
            //        // 半径
            //        int radius = RoundCornerDiameter / 2;
            //        if (!ShowFormShadow && radius > 0)
            //        {
            //            radius = radius + 1; // + 1 防止Region盖住
            //        }

            //        // 边框 2022-04-20 mzc 在win10下边框值为奇数时显示不正常?
            //        using (Pen borderPen = new Pen(this.BorderColor, BorderSize))
            //        {
            //            if (BorderSize % 2 == 0)
            //            {
            //                g.DrawRoundedRectangle(borderPen, 0, 0, Width, Height, radius);
            //            }
            //            else
            //            {
            //                g.DrawRoundedRectangle(borderPen, 0, 0,
            //                    Width - (int)Math.Round(BorderSize / 2.0, 0, MidpointRounding.AwayFromZero) - 1,
            //                    Height - (int)Math.Round(BorderSize / 2.0, 0, MidpointRounding.AwayFromZero) - 1, radius);
            //            }
            //        }

            //        // 内边框
            //        //if (radius > 0)
            //        //{
            //        //    Color innerBorder = BorderColor; // ColorEx.DarkenColor(BorderColor, 50);
            //        //    using (Pen borderPen = new Pen(innerBorder, BorderSize))
            //        //    {
            //        //        if (BorderSize % 2 == 0)
            //        //        {
            //        //            g.DrawRoundedRectangle(borderPen, -BorderSize, -BorderSize, Width, Height, radius);
            //        //        }
            //        //        else
            //        //        {
            //        //            g.DrawRoundedRectangle(borderPen, -BorderSize, -BorderSize,
            //        //                Width - (int)Math.Round(BorderSize / 2.0, 0, MidpointRounding.AwayFromZero),
            //        //                Height - (int)Math.Round(BorderSize / 2.0, 0, MidpointRounding.AwayFromZero),
            //        //                radius); 
            //        //        }
            //        //    }
            //        //}
            //    }

            //}


            // 画标题
            DrawLogoAndTitle(g);

            g.SmoothingMode = smooth;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode)
            {
                var framer = new DropShadow(this);
                framer.BorderRadius = RoundCornerDiameter / 2;
                framer.ShadowRadius = RoundCornerDiameter / 2;
                framer.BorderColor = ColorEx.DarkenColor(this.BackColor, 40);
                framer.ShadowOpacity = 0.8f;
                framer.Redraw(true);
                framer.Show();
            }
        }
        #endregion




    }


}
