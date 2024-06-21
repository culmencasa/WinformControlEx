using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class CustomNotification : Form
    {
        #region 构造

        private CustomNotification()
        {
            // 不使用设计器代码
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(183, 203, 229);
            ClientSize = new Size(400, 83);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            Location = new Point(-1000, -1000);
            MinimumSize = new Size(324, 83);
            Padding = new Padding(24);
            Font = new Font("微软雅黑", 10);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            FormClosing += Form_FormClosing;
            Paint += Form_Paint;
            ResumeLayout(false);



            _popupTimer = new System.Windows.Forms.Timer();
            _popupTimer.Tick += new EventHandler(PopupTimer_Tick);
            this.MouseHover += new EventHandler(Form_MouseHover);
            this.MouseLeave += new EventHandler(Form_MouseLeave);
            this.MouseClick += new MouseEventHandler(Form_MouseClick);
        }

        public CustomNotification(string caption, string brief, string time) : this()
        {
            this.CaptionText = caption;
            this.ContentText = brief;
            this.TimeText = time;
            this.Text = caption;

            AdjustWindowSizeToContent();
        }

        public CustomNotification(string caption, string brief, string time, Point startPosition) : this(caption, brief, time)
        {
            this._initialPos = startPosition;
        }

        #endregion

        #region 枚举

        public enum Spread
        {
            Horizontally,
            Vertically
        }

        public enum PopupStatus
        {
            Hidden,
            Appearing,
            Visible,
            Disappearing
        }

        #endregion

        #region 事件

        public event EventHandler MessageDisappear;
        public event EventHandler MessageClick;

        #endregion

        #region 私有字段

        private int _space = 24;

        #endregion

        #region 保护字段

        protected int _slideUpIncrement = 10;       // 滑入时像素增量
        protected int _slideDownIncrement = 15;     // 滑出时像素增量
        protected int _appearingDelay = 300;        // 显示延迟时间
        protected int _disappearingDelay = 350;     // 隐藏延迟时间
        protected int _stayDealy = 5000;            // 停留时间
        protected double _opacityIncrement = 0.05;  // 透明度变量
        protected bool _isMouseOver;                // 是否鼠标悬停
        protected bool _isRightClicked;             // 是否点击右键 
        protected bool _closeAction;                // 是否关闭
        protected System.Windows.Forms.Timer _popupTimer;               // 弹出计时器
        protected Spread _spreadDirection = Spread.Vertically;          // 弹出方向
        protected PopupStatus _currentStatus = PopupStatus.Hidden;      // 当前窗体状态
        protected Screen _primaryScreen = Screen.PrimaryScreen;         // 主显示屏
        protected Point _initialPos = new Point(0, 0);                  // 初始位置

        #endregion

        #region 公有字段

        internal string CaptionText;
        internal string ContentText;
        internal string TimeText;
        internal bool UseTransparency = true;
        internal double SpecificOpacity = 0.80;
        internal Rectangle CaptionRectangle;
        internal Rectangle ContentRectangle;
        internal Rectangle TimeRectangle;

        #endregion

        #region 属性

        [Category("Custom")]
        public Font CaptionFont { get; set; }
        [Category("Custom")]
        public Font ContentFont { get; set; }
        [Category("Custom")]
        public Font TimeFont { get; set; }

        [Category("Custom")]
        public Color NormalTitleColor { get; set; } = Color.FromArgb(21, 66, 139);


        [Category("Custom")]
        public Color NormalContentColor { get; set; } = Color.FromArgb(21, 66, 139);

        [Category("Custom")]
        public bool FadeInOut { get; set; } = true;

        /// <summary>
        ///  显示延时 (毫秒)
        /// </summary>
        [Category("Custom")]
        public int AppearingDelay
        {
            get
            {
                return _appearingDelay;
            }
            set
            {
                _appearingDelay = value;

                switch (_spreadDirection)
                {
                    case Spread.Horizontally:
                        if (value >= 10)
                            _slideUpIncrement = this.Width / Math.Min((value / 10), this.Width);
                        else
                            _slideUpIncrement = this.Width;
                        break;
                    case Spread.Vertically:
                        if (value >= 10)
                            _slideUpIncrement = this.Height / Math.Min((value / 10), this.Height);
                        else
                            _slideUpIncrement = this.Height;
                        break;
                }
            }
        }
        /// <summary>
        ///  消失延时 (毫秒)
        /// </summary>
        [Category("Custom")]
        public int DisappearingDelay
        {
            get
            {
                return _disappearingDelay;
            }
            set
            {
                _disappearingDelay = value;
                switch (_spreadDirection)
                {
                    case Spread.Horizontally:
                        if (value >= 10)
                            _slideDownIncrement = this.Width / Math.Min((value / 10), this.Width);
                        else
                            _slideDownIncrement = this.Width;
                        break;
                    case Spread.Vertically:
                        if (value >= 10)
                            _slideDownIncrement = this.Height / Math.Min((value / 10), this.Height);
                        else
                            _slideDownIncrement = this.Height;
                        break;
                }
            }
        }
        /// <summary>
        ///  停留延时 (毫秒)
        /// </summary>
        [Category("Custom")]
        public int StayDelay
        {
            get
            {
                return _stayDealy;
            }
            set
            {
                _stayDealy = value;
            }
        }
        /// <summary>
        ///  初始位置
        /// </summary>
        [Category("Custom")]
        public Point InitialPos
        {
            get
            {
                if (_initialPos.X == 0 && _initialPos.Y == 0)
                {
                    switch (_spreadDirection)
                    {
                        case Spread.Horizontally:
                            _initialPos = new Point(
                                _primaryScreen.Bounds.Width,
                                _primaryScreen.WorkingArea.Height + _primaryScreen.WorkingArea.Y - PopupSize.Height
                            );
                            break;
                        case Spread.Vertically:
                            _initialPos = new Point(
                                _primaryScreen.WorkingArea.Width + _primaryScreen.WorkingArea.X - PopupSize.Width,
                                _primaryScreen.Bounds.Height
                            );
                            break;
                    }
                }
                return _initialPos;
            }
            set
            {
                _initialPos = value;
            }
        }
        /// <summary>
        ///  窗体大小
        /// </summary>
        [Category("Custom")]
        public Size PopupSize
        {
            get
            {
                return this.Size;
            }
        }
        /// <summary>
        ///  当前窗体状态
        /// </summary>
        [Category("Custom")]
        public PopupStatus CurrentStatus
        {
            get
            {
                return _currentStatus;
            }
        }
        /// <summary>
        ///  弹出的方向
        /// </summary>
        [Category("Custom")]
        public Spread SpreadDirection
        {
            get
            {
                return _spreadDirection;
            }
            set
            {
                _spreadDirection = value;
            }
        }

        #endregion

        #region 虚方法

        // 计算窗体大小 
        protected virtual void AdjustWindowSizeToContent()
        {
            var g = this.CreateGraphics();
            var scale = g.DpiX / 96.0f;

            CaptionFont = new Font(this.Font.FontFamily.Name, this.Font.Size * scale, FontStyle.Bold, GraphicsUnit.Pixel);
            ContentFont = new Font(this.Font.FontFamily.Name, this.Font.Size * scale, FontStyle.Regular, GraphicsUnit.Pixel);
            TimeFont = new Font(this.Font.FontFamily.Name, this.Font.Size * scale, FontStyle.Bold, GraphicsUnit.Pixel);

            var spaceBetweenCaptionAndContent = _space;

            SetCaptionArea(g);
            SetTimeArea(g);
            SetContentArea(g);


            // 固定宽度, 高度根据内容自动调整
            this.Size = new Size(
                this.Width,
                Math.Max(CaptionRectangle.Height
                        + ContentRectangle.Height
                        + Padding.Top
                        + Padding.Bottom +
                        +spaceBetweenCaptionAndContent,
                    MinimumSize.Height));
        }

        void SetCaptionArea(Graphics g)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.NoClip;
            sf.Trimming = StringTrimming.None;
            SizeF captionSize = g.MeasureString(CaptionText, CaptionFont, this.ClientRectangle.Width, sf);
            this.CaptionRectangle = new Rectangle(
                Padding.Left,
                Padding.Top,
                (int)captionSize.Width + _space,
                (int)captionSize.Height);
        }

        void SetContentArea(Graphics g)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.FitBlackBox;
            sf.Trimming = StringTrimming.None;

            var captionArea = CaptionRectangle;
            SizeF contentSize = g.MeasureString(ContentText, ContentFont,
this.Width - Padding.Right, sf);
            ContentRectangle = new Rectangle(
                Padding.Left,
                captionArea.Bottom + 24,
                (int)contentSize.Width,
                (int)contentSize.Height);
        }

        void SetTimeArea(Graphics g)
        {
            var captionArea = CaptionRectangle;
            SizeF timeSize = g.MeasureString(TimeText, TimeFont);
            TimeRectangle = new Rectangle(
                this.Width - Padding.Right - (int)timeSize.Width,
                Padding.Top,
                (int)timeSize.Width + _space,
                (int)timeSize.Height);
        }


        // 绘制标头文字
        protected virtual void DrawCaptionText(Graphics g)
        {
            if (CaptionText != null && CaptionText.Length != 0)
            {
                g.DrawString(CaptionText, CaptionFont, new SolidBrush(NormalTitleColor), CaptionRectangle);
            }
        }
        protected virtual void DrawTimeText(Graphics g)
        {
            g.DrawString(TimeText, TimeFont, new SolidBrush(NormalTitleColor), TimeRectangle);
        }


        // 绘制内容文字
        protected virtual void DrawContentText(Graphics g)
        {
            //var scale = grfx.DpiX / 96.0f;
            //grfx.ScaleTransform(grfx.DpiX / 96, grfx.DpiY / 96);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            sf.FormatFlags = StringFormatFlags.FitBlackBox;
            sf.Trimming = StringTrimming.None;

            SizeF ContentSize = g.MeasureString(ContentText, ContentFont, this.ClientRectangle.Width - Padding.Right, sf);
            Size ContentRoughSize = new Size(
                Convert.ToInt32(Math.Ceiling(ContentSize.Width)),
                Convert.ToInt32(Math.Ceiling(ContentSize.Height)));
            this.ContentRectangle = new Rectangle(
                new Point(Padding.Left, Padding.Top + CaptionRectangle.Height + Padding.Bottom),
                ContentRoughSize
            );

            if (ContentText != null && ContentText.Length != 0)
            {

                g.DrawString(ContentText, ContentFont, new SolidBrush(NormalContentColor), ContentRectangle, sf);
            }
        }

        // 隐藏后
        protected virtual void OnMessageDisappear(object sender, EventArgs e)
        {
            if (this.MessageDisappear != null)
            {
                MessageDisappear(sender, e);
            }
        }

        protected virtual void OnMessageClick(object sender, EventArgs e)
        {
            if (MessageClick != null)
            {
                MessageClick(sender, e);
            }
        }


        #endregion

        #region 公开的方法

        /// <summary>
        ///  显示弹出警告
        /// </summary>
        public void ShowPopupInfo()
        {
            if (this._currentStatus == PopupStatus.Appearing)
                return;

            this._currentStatus = PopupStatus.Appearing;
            // 设置窗体初始位置
            this.Location = InitialPos;
            // 设置透明变量
            double TotalOpacity = 1.00;
            if (UseTransparency)
            {
                TotalOpacity = SpecificOpacity;
            }
            if (_spreadDirection == Spread.Horizontally)
            {
                this._opacityIncrement = TotalOpacity / (PopupSize.Width / _slideUpIncrement);
            }
            else
            {
                this._opacityIncrement = TotalOpacity / (PopupSize.Height / _slideUpIncrement);
            }
            // 显示窗体避免抢占焦点            
            //ShowWindow(this.Handle, 4);

            base.Show();
            this.Opacity = 0;
            // 设置初始显示间隔
            _popupTimer.Interval = 10;
            _popupTimer.Start();
        }
        /// <summary>
        ///  关闭弹出窗口
        /// </summary>
        public void ClosePopupInfo()
        {
            if (PopupStatus.Visible == _currentStatus)
            {
                _closeAction = true;
                _isMouseOver = false;
                _popupTimer.Interval = 10;
                _popupTimer.Start();
                _currentStatus = PopupStatus.Disappearing;
            }
        }
        #endregion

        #region 重写的属性

        /// <summary>
        /// 窗体不抢占焦点
        /// </summary>
        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region 重写的方法


        #endregion

        #region 事件处理

        // 鼠标移出窗体
        protected void Form_MouseLeave(object sender, EventArgs e)
        {
            if (Form.MousePosition.X > this.ClientRectangle.X + this.Width ||
                Form.MousePosition.Y > this.ClientRectangle.Y + this.Height ||
                Form.MousePosition.X < this.ClientRectangle.X ||
                Form.MousePosition.Y < this.ClientRectangle.Y)
            {
                _isMouseOver = false;
            }
        }
        // 鼠标停留窗体
        protected void Form_MouseHover(object sender, EventArgs e)
        {
            this._isMouseOver = true;
        }
        // 右键隐藏
        protected void Form_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ClosePopupInfo();
            }
            else if (e.Button == MouseButtons.Left)
            {
                ClosePopupInfo();
                OnMessageClick(this, EventArgs.Empty);
            }
        }

        // Timer
        protected void PopupTimer_Tick(object sender, EventArgs e)
        {
            switch (_currentStatus)
            {
                case PopupStatus.Appearing:
                    if (_spreadDirection == Spread.Vertically)
                    {
                        #region 垂直显示

                        if (this.Location.Y <= _primaryScreen.WorkingArea.Height + _primaryScreen.WorkingArea.Y - PopupSize.Height)
                        {
                            _popupTimer.Stop();
                            _currentStatus = PopupStatus.Visible;
                            this.Location = new Point(
                                InitialPos.X,
                                _primaryScreen.WorkingArea.Height + _primaryScreen.WorkingArea.Y - PopupSize.Height
                            );

                            // 设置显示之后的等待时间间隔
                            _popupTimer.Interval = _stayDealy;
                            _popupTimer.Start();
                        }
                        else
                        {
                            this.Location = new Point(
                                InitialPos.X,
                                this.Location.Y - _slideUpIncrement
                            );
                            if (FadeInOut)
                            {
                                this.Opacity = this.Opacity + this._opacityIncrement;
                            }
                        }

                        #endregion
                    }
                    else if (_spreadDirection == Spread.Horizontally)
                    {
                        #region 水平显示

                        if (this.Location.X <= _primaryScreen.Bounds.Width - PopupSize.Width)
                        {
                            _popupTimer.Stop();
                            _currentStatus = PopupStatus.Visible;
                            this.Location = new Point(
                                _primaryScreen.Bounds.Width - PopupSize.Width,
                                _initialPos.Y);

                            // 设置显示之后的等待时间间隔
                            _popupTimer.Interval = _stayDealy;
                            _popupTimer.Start();
                        }
                        else
                        {
                            this.Location = new Point(this.Location.X - _slideUpIncrement, _initialPos.Y);
                            if (FadeInOut)
                            {
                                this.Opacity = this.Opacity + _opacityIncrement;
                            }
                        }

                        #endregion
                    }
                    break;
                case PopupStatus.Visible:
                    _popupTimer.Stop();
                    _popupTimer.Interval = 10;
                    if (!_isMouseOver)   // 如果鼠标没有悬浮
                    {
                        _currentStatus = PopupStatus.Disappearing;
                        _popupTimer.Start();
                    }

                    break;
                case PopupStatus.Disappearing:
                    if (_spreadDirection == Spread.Vertically)
                    {
                        #region 如果窗体被移动后, 使用淡出隐藏

                        //if (this.Location.X != InitialPos.X || this.Location.Y != primaryScreen.WorkingArea.Height + primaryScreen.WorkingArea.Y - PopupSize.Height)
                        //{
                        //    this.Opacity = this.Opacity - 0.1;
                        //    if (Opacity == 0)    // 如果透明度减为0, 则关闭
                        //    {
                        //        OnHidden(this, new EventArgs());
                        //        if (closeAction)
                        //        {
                        //            this.Close();
                        //        }
                        //        else
                        //        {
                        //            base.Hide();
                        //        }
                        //    }
                        //    break;
                        //}                        

                        #endregion

                        #region 垂直隐藏

                        // TODO: 如果鼠标悬停, 则再次显示(待优化)
                        if (_isMouseOver && !_closeAction)
                        {
                            _currentStatus = PopupStatus.Appearing;
                        }
                        else
                        {
                            if (this.Location.Y >= _primaryScreen.Bounds.Height)
                            {
                                _popupTimer.Stop();
                                _currentStatus = PopupStatus.Hidden;
                                OnMessageDisappear(this, new EventArgs());
                                if (_closeAction)
                                {
                                    this.Close();
                                }
                                else
                                {
                                    base.Hide();
                                }
                            }
                            else
                            {
                                if (FadeInOut)
                                {
                                    this.Opacity = this.Opacity - _opacityIncrement;
                                }
                                this.Location = new Point(
                                    InitialPos.X,
                                    this.Location.Y + _slideDownIncrement
                                );
                            }
                        }

                        #endregion
                    }
                    else if (_spreadDirection == Spread.Horizontally)
                    {
                        #region 如果窗体被移动后, 使用淡出隐藏

                        //if (this.Location.X != primaryScreen.Bounds.Width - PopupSize.Width || this.Location.Y != initialPos.Y)
                        //{
                        //    this.Opacity = this.Opacity - 0.1;
                        //    break;
                        //}

                        #endregion

                        #region 水平隐藏

                        if (_isMouseOver && !_closeAction)
                        {
                            _currentStatus = PopupStatus.Appearing;
                        }
                        else
                        {
                            if (this.Location.X >= _primaryScreen.Bounds.Width)
                            {
                                _popupTimer.Stop();
                                _currentStatus = PopupStatus.Hidden;
                                OnMessageDisappear(this, new EventArgs());
                                if (_closeAction)
                                {
                                    this.Close();
                                }
                                else
                                {
                                    base.Hide();
                                }
                            }
                            else
                            {
                                if (FadeInOut)
                                {
                                    this.Opacity = this.Opacity - _opacityIncrement;
                                }
                                this.Location = new Point(this.Location.X + _slideDownIncrement, _initialPos.Y);
                            }
                        }

                        #endregion
                    }
                    break;
            }
        }
        // 窗体重绘
        protected void Form_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
            {
                return;
            }

            g.Clear(Color.White);

            //LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(225, 230, 236), Color.FromArgb(194, 217, 247), LinearGradientMode.Vertical);
            g.DrawRectangle(Pens.SteelBlue, new Rectangle(0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1));
            DrawCaptionText(e.Graphics);
            DrawTimeText(e.Graphics);
            DrawContentText(e.Graphics);
            //brush.Dispose();
        }

        // 窗体关闭
        protected void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_currentStatus != PopupStatus.Hidden)
            {
                ClosePopupInfo();
                e.Cancel = true;
            }
        }

        #endregion

    }

}
