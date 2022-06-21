using FormExCore.ThirdParty;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class OcnForm : BorderlessForm
    {
        #region 构造

        public OcnForm()
        {
            InitializeComponent();

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            TitleFont = new Font(SystemFonts.DefaultFont.FontFamily, SystemFonts.DefaultFont.Size, FontStyle.Bold);
            Theme = OcnThemes.Primary;

            AfterPositionChanged += OcnForm_AfterPositionChanged;
        }

        private void OcnForm_AfterPositionChanged()
        {

        }

        #endregion

        #region 字段

        private OcnThemes _theme;
        private int _roundCornerDiameter;
        private string _customTitleText;
        private int _titleBarHeight = 32;
        private Color _titleColor;
        private Color _titleBarBackColor;
        private int _iconSize;
        private Image _icon;
        private Font _titleFont;
        private bool _showFormIcon = true;
        private bool _showFormTitle = true;
        private DateTime _titleClickTime = DateTime.MinValue;

        #endregion

        #region 属性

        #region 主题

        /// <summary>
        /// 主题
        /// </summary>
        [Category("Look")]
        [Browsable(true)]
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


        /// <summary>
        /// 与ShowIcon区分开来，因为如果ShowIcon为False时，任务栏上也将无法显示图标
        /// </summary>
        [Category("Custom"),
         DefaultValue(true), Description("是否显示窗体顶部左上角的图标")]
        [Browsable(true)]
        public bool ShowFormIcon
        {
            get
            {
                return _showFormIcon;
            }
            set
            {
                _showFormIcon = value;
                Invalidate();
            }
        }

        [Category("Custom"),
         DefaultValue(true)]
        [Browsable(true)]
        public bool ShowFormTitle
        {
            get
            {
                return _showFormTitle;
            }
            set
            {
                bool hasChanged = _showFormTitle != value;
                _showFormTitle = value;
                if (hasChanged)
                {
                    Invalidate();
                }
            }
        }

        [Category("Look")]
        [Browsable(true)]
        public Color TitleBarBackColor
        {
            get
            {
                return _titleBarBackColor;
            }
            set
            {
                _titleBarBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 圆角直径
        /// </summary>
        [Category("Look")]
        [Browsable(true)]
        public int RoundCornerDiameter
        {
            get
            {
                return _roundCornerDiameter;
            }
            set
            {
                _roundCornerDiameter = value;
                // 设计时不改变窗体形状. 因为在HighDPI下宽高会不正常.
                if (IsHandleCreated && !DesignMode)
                    UpdateFormRoundCorner(value);
                Invalidate();
            }
        }

        /// <summary>
        /// 标题栏文字
        /// </summary>
        [Category("Look")]
        [Browsable(true)]
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


        [Category("Look")]
        [Browsable(true)]
        public Font TitleFont
        {
            get
            {
                return _titleFont;
            }
            set
            {
                _titleFont = value;
                Invalidate();
            }
        }


        /// <summary>
        /// 标题栏高度
        /// </summary>
        [Category("Look")]
        [Browsable(true)]
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

        [Category("Look")]
        [Browsable(true)]
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

        [Category("Look")]
        [Browsable(true)]
        public int IconSize
        {
            get
            {
                return _iconSize;
            }
            set
            {
                _iconSize = value;
                Invalidate();
            }
        }


        [Category("Look")]
        [Browsable(true)]
        public new Image Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                if (value != null && _iconSize == 0)
                {
                    _iconSize = value.Width;
                }
                else
                {
                    _iconSize = 0;
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
            TitleBarBackColor = Color.FromArgb(108, 17, 150);
            TitleColor = Color.White;
        }

        protected virtual void ApplySecondary()
        {
            //BorderColor = Presets.SecondaryColor;
            BackColor = Color.White;
            ForeColor = Presets.SecondaryColor;
            TitleBarBackColor = Presets.SecondaryColor;
            TitleColor = Color.White;
        }
        protected virtual void ApplySuccess()
        {
            //BorderColor = Presets.SuccessColor;
            BackColor = Color.White;
            ForeColor = Presets.SuccessColor;
            TitleBarBackColor = Presets.SuccessColor;
            TitleColor = Color.White;
        }

        protected virtual void ApplyDanger()
        {
            //BorderColor = Presets.DangerColor;
            BackColor = Color.White;
            ForeColor = Presets.DangerColor;
            TitleBarBackColor = Presets.DangerColor;
            TitleColor = Color.White;
        }
        protected virtual void ApplyWarning()
        {
            //BorderColor = Presets.WarningColor;
            BackColor = Color.White;
            ForeColor = Presets.WarningColor;
            TitleBarBackColor = Presets.WarningColor;
            TitleColor = Color.White;
        }

        protected virtual void ApplyInfo()
        {
            //BorderColor = Presets.InfoColor;
            BackColor = Color.White;
            ForeColor = Presets.InfoColor;
            TitleBarBackColor = Presets.InfoColor;
            TitleColor = Color.Black;
        }

        protected virtual void ApplyLight()
        {
            //BorderColor = ColorEx.DarkenColor(Presets.LightColor, 20);
            BackColor = Color.White;
            ForeColor = Color.Black;
            TitleBarBackColor = Presets.LightColor;
            TitleColor = Color.Black;
        }

        protected virtual void ApplyDark()
        {
            //BorderColor = Presets.DarkColor;
            BackColor = Color.White;
            ForeColor = Presets.DarkColor;
            TitleBarBackColor = Presets.DarkColor;
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

        private void LayoutControlButtons()
        {
            this.btnClose.Location = new Point(this.Width - btnClose.Width, (TitleBarHeight - btnClose.Height) / 2);
            this.btnMax.Location = new Point(btnClose.Left - btnMax.Width, (TitleBarHeight - btnClose.Height) / 2);
            this.btnMin.Location = new Point(btnMax.Left - btnMin.Width, (TitleBarHeight - btnClose.Height) / 2);
        }
           
        #endregion

        #region 界面事件
                
        private void OcnForm_Resize(object sender, EventArgs e)
        {
            LayoutControlButtons();
        }

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

                    cp.Style |= (int)WindowStyle.WS_SYSMENU;
                }
                return cp;
            }
        }


        protected override void WndProc(ref Message m)
        {
            #region 标题栏

            if (m.Msg == Win32.WM_NCHITTEST) //  && (int)m.Result == Win32.HTCLIENT
            {
                // 点击标题栏
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y > 0 && pos.Y < TitleBarHeight)
                {
                    m.Result = new IntPtr(Win32.HTCAPTION);
                    return;
                }
                else
                {
                    WmNcHitTest(ref m);
                    return;
                }
            }

            #endregion

            if (m.Msg == Win32.WM_NCLBUTTONDOWN)
            {

                // 显示系统菜单
                Rectangle iconRectangle = GetIconRectangle();
                Point mousePosition = new Point(m.LParam.ToInt32());
                mousePosition = PointToClient(mousePosition);
                Debug.WriteLine($"{mousePosition.X}, {mousePosition.Y}");
                if (iconRectangle.Contains(mousePosition))
                {

                    var clickTime = (DateTime.Now - _titleClickTime).TotalMilliseconds;
                    if (clickTime <= SystemInformation.DoubleClickTime)
                    {
                        Close();
                        return;
                    }
                    else
                    {
                        _titleClickTime = DateTime.Now;
                    }


                    IntPtr hWnd = Handle;
                    Win32.RECT pos = new Win32.RECT();
                    Win32.GetWindowRect(hWnd, ref pos);
                    pos.top = this.DesktopBounds.Top + TitleBarHeight;
                    IntPtr hMenu = Win32.GetSystemMenu(hWnd, false);
                    // 弹出系统菜单
                    int cmd = Win32.TrackPopupMenu(hMenu, 0x100, pos.left, pos.top, 0, hWnd, IntPtr.Zero);
                    // 接收菜单的命令
                    if (cmd > 0)
                    {
                        // 执行菜单命令
                        Win32.SendMessage(hWnd, 0x112, cmd, 0);
                    }
                    return;
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
                else if (WindowState == FormWindowState.Maximized)
                {
                    UpdateFormRoundCorner(0);
                }
            }
            //ResumeDrawing(this);
        }

        protected virtual void FillTitleBarBackground(Graphics g)
        {
            int radius = RoundCornerDiameter / 2;

            Rectangle titleBarArea = new Rectangle();
            titleBarArea.X = 0;
            titleBarArea.Y = 0;
            titleBarArea.Width = this.Width;
            titleBarArea.Height = TitleBarHeight;



            using (Brush titleBarBrush = new SolidBrush(TitleBarBackColor))
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


        protected virtual Rectangle GetIconRectangle()
        {
            int width = 0;
            int height = 0;
            int left = 0;
            int top = 0;
            Rectangle iconRectangle = Rectangle.Empty;

            if (ShowFormIcon && Icon != null && IconSize > 0)
            {
                width = IconSize;
                height = IconSize;
                left = 6;
                top = (TitleBarHeight - height) / 2;
                iconRectangle = new Rectangle(left, top, width, height);
            }
            else
            {
                // 不显示图标, 但保留占位符
                width = 10;
                height = TitleBarHeight;
                top = 0;
                left = 0;
                iconRectangle = new Rectangle(left, top, width, height);
            }


            return iconRectangle;
        }

        protected virtual Rectangle GetTitleRectangle()
        {
            int width = 0;
            int height = 0;
            int left = 0;
            int top = 0;
            int margin = 6;
            Rectangle titleRectangle = Rectangle.Empty;
            Rectangle iconRectangle = GetIconRectangle();

            if (ShowFormTitle)
            {
                // 字体大小
                SizeF textSize = new SizeF();
                if (TitleText?.Length > 0)
                {
                    textSize = TextRenderer.MeasureText(TitleText, TitleFont);
                }

                // 整体标题区域大小
                width = (int)textSize.Width;
                height = (int)textSize.Height;
                left = margin + iconRectangle.Right;
                top = (TitleBarHeight - height) / 2;

                titleRectangle = new Rectangle(left, top, width, height);
            }

            return titleRectangle;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width <= 0 || Height <= 0)
            {
                base.OnPaint(e);
                return;
            }

            Bitmap cacheBitmap = new Bitmap(this.Width, this.Height);

            Graphics g = Graphics.FromImage(cacheBitmap);
            g.SmoothingMode = SmoothingMode.None;


            FillTitleBarBackground(g);

            // 画窗体图标
            if (ShowFormIcon && Icon != null)
            {
                g.DrawImage(Icon, GetIconRectangle());
            }

            // 画标题   
            if (!string.IsNullOrEmpty(TitleText))
            {
                TextRenderer.DrawText(
                    g,
                    TitleText,
                    TitleFont,
                    GetTitleRectangle(),
                    TitleColor,
                    TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
            }
            g.Dispose();


            e.Graphics.DrawImage(cacheBitmap, 0, 0);

            base.OnPaint(e);

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var framer = new DropShadow(this);
            framer.BorderRadius = RoundCornerDiameter / 2;
            framer.ShadowRadius = RoundCornerDiameter / 2;
            framer.BorderColor = ColorEx.DarkenColor(this.BackColor, 40);
            framer.ShadowOpacity = 0.8f;
            framer.Redraw(true);
            framer.Show();

        }




        #endregion

        private void btnClose_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void btnMax_MouseClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void btnMin_MouseClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #region WINAPI



        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }


        #endregion


        private void WmNcHitTest(ref Message m)
        {
            #region 判断操作的是窗体边缘, 则调整窗体大小
            if (WindowState == FormWindowState.Normal)
            {
                int wparam = m.LParam.ToInt32();
                Point mouseLocation = new Point(RenderHelper.LOWORD(wparam), RenderHelper.HIWORD(wparam));
                mouseLocation = PointToClient(mouseLocation);

                if (mouseLocation.X < 5 && mouseLocation.Y < 5)
                {
                    m.Result = new IntPtr(Win32.HTTOPLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 5 && mouseLocation.Y < 5)
                {
                    m.Result = new IntPtr(Win32.HTTOPRIGHT);
                    return;
                }

                if (mouseLocation.X < 5 && mouseLocation.Y > Height - 5)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOMLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 5 && mouseLocation.Y > Height - 5)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOMRIGHT);
                    return;
                }

                if (mouseLocation.Y <= 3)
                {
                    m.Result = new IntPtr(Win32.HTTOP);
                    return;
                }

                if (mouseLocation.Y >= Height - 3)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOM);
                    return;
                }

                if (mouseLocation.X <= 3)
                {
                    m.Result = new IntPtr(Win32.HTLEFT);
                    return;
                }

                if (mouseLocation.X >= Width - 3)
                {
                    m.Result = new IntPtr(Win32.HTRIGHT);
                    return;
                }

            }

            #endregion

            m.Result = new IntPtr(Win32.HTCLIENT);
        }


    }


}
