using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    /// <summary>
    /// 无边框窗体
    /// </summary>
    public partial class NonFrameForm : Form
    {

        #region 构造

        public NonFrameForm()
        {
            this.SetStyles();
            InitializeComponent();

            this.BorderColor = Color.Gray;
            //this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            this.TextFont = this.Font;
            this.TextShadowColor = Color.FromArgb(2, 0, 0, 0);
            this.TextForeColor = Color.FromArgb(255, 255, 255, 255);
            this.StyleChanged += new EventHandler(NoneBorderForm_StyleChanged);
        }

        #endregion

        #region 常量

        const int CS_DropSHADOW = 0x20000;

        #endregion

        #region 字段

        protected Padding _iconMargin = new Padding(3);
        private bool _showText = true;
        #endregion

        #region 属性

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor { get; set; }

        [Description("用于绘制窗体标题的字体")]
        public Font TextFont
        {
            get;
            set;

        }

        [Description("用于绘制窗体标题的颜色")]
        public Color TextForeColor
        {
            get;
            set;
        }

        [Description("如果TextWithShadow属性为True,则使用该属性绘制阴影")]
        public Color TextShadowColor
        {
            get;
            set;
        }

        [Description("如果TextWithShadow属性为True,则使用该属性获取或色泽阴影的宽度")]
        public int TextShadowWidth
        {
            get;
            set;
        }

        [Category("窗口样式"), 
        DefaultValue(true)]
        public bool ShowText
        {
            get
            {
                return _showText;
            }
            set
            {
                bool hasChanged = _showText != value;
                _showText = value;
                if (hasChanged)
                {
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 与ShowIcon区分开来，因为如果ShowIcon为False时，任务栏上也将无法显示图标
        /// </summary>
        [Category("窗口样式"),
        DefaultValue(true), Description("是否显示窗体顶部左上角的图标")]
        public bool ShowIconOnTop
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示标题阴影
        /// </summary>
        public bool ShowTextShadow
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        public bool Resizable { get; set; }

        #endregion
        
        #region 重写的成员
        
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    if (MaximizeBox) { cp.Style |= (int)WindowStyle.WS_MAXIMIZEBOX; }
                    if (MinimizeBox) { cp.Style |= (int)WindowStyle.WS_MINIMIZEBOX; }
                    cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;  //防止因窗体控件太多出现闪烁
                    cp.ClassStyle |= CS_DropSHADOW;  //实现窗体边框阴影效果
                }
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_ERASEBKGND:
                    var os = Environment.OSVersion;
                    if (os.Platform == PlatformID.Win32NT && os.Version.Major != 5)
                    {
                        return;
                    }
                    m.Result = IntPtr.Zero;
                    break;
                case Win32.WM_NCHITTEST:
                    WmNcHitTest(ref m);
                    return;
                case Win32.WM_NCLBUTTONDBLCLK:
                    if (!this.MaximizeBox)
                    {
                        return;
                    }
                    break;
                case Win32.WM_SYSCOMMAND:
                    if (m.WParam.ToInt32() == Win32.SC_MAXMIZE) // 最大化
                    {

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.None;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                //g.Clip = new Region(g.VisibleClipBounds);

                if (BackgroundImage != null)
                {
                    switch (BackgroundImageLayout)
                    {
                        case ImageLayout.Stretch:
                        case ImageLayout.Zoom:
                            g.DrawImage(
                                this.BackgroundImage,
                                ClientRectangle, ClientRectangle, GraphicsUnit.Pixel);
                            break;
                        case ImageLayout.Center:
                        case ImageLayout.None:
                        case ImageLayout.Tile:
                            {

                                g.DrawImage(
                                    this.BackgroundImage,
                                    ClientRectangle, ClientRectangle, GraphicsUnit.Pixel);
                            }
                            break;
                    }
                }


                // 图标
                if (ShowIconOnTop && Icon != null)
                {
                    g.DrawIcon(Icon, GetIconRectangle());
                }

                // 标题
                if (ShowText && Text.Length != 0)
                {
                    if (ShowTextShadow)
                    {
                        using (Image textImg = RenderHelper.GetStringImgWithShadowEffect(Text, TextFont, TextForeColor, TextShadowColor, TextShadowWidth))
                        {
                            g.DrawImage(textImg, GetFormTextRectangle().Location);
                        }
                    }
                    else
                    {
                        TextRenderer.DrawText(
                            e.Graphics,
                            Text, TextFont,
                            GetFormTextRectangle(),
                            TextForeColor,
                            TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
                    }
                }

                base.OnPaint(e);

                // 画边框
                Rectangle borderRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                //g.DrawRoundedRectangleAlpha(this.BorderColor, Color.Transparent, borderRect, new Size(2, 2), 255);
                using (Pen borderPen = new Pen(this.BorderColor))
                {
                    g.DrawRectangle(borderPen, borderRect);
                }
                //RenderHelper.DrawFormFringe(this, e.Graphics, System.Windows.Forms.Properties.Resources.fringe_bkg, 5);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Useless

        protected virtual void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        //调整窗体大小
        private void WmNcHitTest(ref Message m)  
        {
            int wparam = m.LParam.ToInt32();
            Point mouseLocation = new Point(RenderHelper.LOWORD(wparam), RenderHelper.HIWORD(wparam));
            mouseLocation = PointToClient(mouseLocation);

            if (WindowState != FormWindowState.Maximized)
            {
                if (Resizable)
                {
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

                    if (mouseLocation.Y < 3)
                    {
                        m.Result = new IntPtr(Win32.HTTOP);
                        return;
                    }

                    if (mouseLocation.Y > Height - 3)
                    {
                        m.Result = new IntPtr(Win32.HTBOTTOM);
                        return;
                    }

                    if (mouseLocation.X < 3)
                    {
                        m.Result = new IntPtr(Win32.HTLEFT);
                        return;
                    }

                    if (mouseLocation.X > Width - 3)
                    {
                        m.Result = new IntPtr(Win32.HTRIGHT);
                        return;
                    }
                }
            }

            m.Result = new IntPtr(Win32.HTCAPTION);
            //m.Result = new IntPtr(Win32.HTCLIENT);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取图标范围
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetIconRectangle()
        {
            if (ShowIconOnTop && base.Icon != null)
            {
                return new Rectangle(_iconMargin.Left, _iconMargin.Top, SystemInformation.SmallIconSize.Width, SystemInformation.SmallIconSize.Width);
            }
            return Rectangle.Empty;
        }

        /// <summary>
        /// 获取标题范围
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetFormTextRectangle()
        {
            if (Text.Length > 0)
            {
                Rectangle iconRectangle = this.GetIconRectangle();
                // 过时的
                //SizeF textSize = this.CreateGraphics().MeasureString(this.Text, this.TextFont);
                SizeF textSize = TextRenderer.MeasureText(this.Text, this.TextFont);
                
                int x = iconRectangle.Right + _iconMargin.Right;
                int y = _iconMargin.Top;
                int width = (int)textSize.Width * 2;
                int height = (int)textSize.Height;
                if (ShowIconOnTop)
                {
                    if (height > iconRectangle.Height)
                    {
                        y = _iconMargin.Top + (height - iconRectangle.Height) / 2;
                    }
                    else if (height < iconRectangle.Bottom)
                    {
                        y = _iconMargin.Top + (int)Math.Round((iconRectangle.Bottom - height) / 2M, 0, MidpointRounding.AwayFromZero);
                    }
                }


                return new Rectangle(x, y, width, height);
            }
            return Rectangle.Empty;
        }

        #endregion

        #region 事件处理

        // 设置是否显示控制按钮
        private void NoneBorderForm_StyleChanged(object sender, EventArgs e)
        {
            if (!ControlBox)
            {
                btnClose.Visible = false;
                btnMaximum.Visible = false;
                btnMinimum.Visible = false;
            }
            else
            {
                btnMaximum.Visible = MaximizeBox;
                btnMinimum.Visible = MinimizeBox;
                btnClose.Visible = true;
            }
        }
        
        // 窗体调整大小
        private void NoneBorderForm_Resize(object sender, EventArgs e)
        {
            this.flpControlBox.Location = new Point(this.Width - this.flpControlBox.Width, 0);
        }

        // 点击最小化按钮 
        private void btnMinimum_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // 点击最大化/还原按钮
        private void btnMaximum_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.btnMaximum.NormalImage = Properties.Resources.btnMaximum_NormalImage;
                this.btnMaximum.HoverImage = Properties.Resources.btnMaximum_HoverImage;
                this.btnMaximum.DownImage = Properties.Resources.btnMaximum_DownImage;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.btnMaximum.NormalImage = Properties.Resources.restore_normal;
                this.btnMaximum.HoverImage = Properties.Resources.restore_highlight;
                this.btnMaximum.DownImage = Properties.Resources.restore_down;
            }
            
        }

        // 点击关闭按钮
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
