using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public partial class SlidePopup: RoundedCornerForm
    {
        #region 枚举

        /// <summary>
        /// 窗口伸展的方向
        /// </summary>
        public enum Spread
        { 
            Horizontally,
            Vertically
        }

        /// <summary>
        /// 窗口的弹出状态
        /// </summary>
        public enum PopupStatus
        {
            Hidden,
            Appearing,
            Visible,
            Disappearing
        }

        #endregion

        #region 静态成员
        
        [DllImport("user32.dll")]
        protected static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        #endregion

        #region 保护字段

        protected int slideUpIncrement = 10;      // 滑入时像素增量
        protected int slideDownIncrement = 15;  // 滑出时像素增量
        protected int appearingDelay = 250;
        protected int disappearingDelay = 250;
        protected int stayDealy = 3000;
        protected double opacityIncrement = 0.05;
        protected bool isMouseOver;
        protected bool isRightClicked;

        protected bool closeAction;
        protected Timer popupTimer;
        protected Spread spreadDirection;
        protected PopupStatus currentStatus = PopupStatus.Hidden;
        protected Screen primaryScreen = Screen.PrimaryScreen;
        protected Point initialPos = new Point(0, 0);

        #endregion
        
        #region 公有字段

        public string CaptionText;
        public string ContentText;
        public string EventTimeText;
        public bool FadeInOut;
        public bool UseTransparency;
        public double SpecificOpacity = 0.80;
        public Bitmap BackgroundBitmap;
        public Font CaptionFont = SystemFonts.CaptionFont;//new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
        public Font ContentFont = SystemFonts.DefaultFont;//new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel);
        public Font TimeFont = SystemFonts.SmallCaptionFont;//new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel);

        public Rectangle TitleRectangle;
        public Rectangle ContentRectangle;
        public Color normalTitleColor = Color.FromArgb(255, 0, 0);
        public Color hoverTitleColor = Color.FromArgb(255, 0, 0);
        public Color NormalContentColor = Color.FromArgb(244, 244, 244);
        public Color hoverContentColor = Color.FromArgb(0, 0, 0x66);

        #endregion
        
        #region 属性

        /// <summary>
        ///  显示延时 (毫秒)
        /// </summary>
        public int AppearingDelay
        {
            get
            {
                return appearingDelay;
            }
            set
            {
                appearingDelay = value;

                switch (spreadDirection)
                {
                    case Spread.Horizontally:
                        if (value >= 10)
                            slideUpIncrement = this.Width / Math.Min((value / 10), this.Width);
                        else
                            slideUpIncrement = this.Width;
                        break;
                    case Spread.Vertically:
                        if (value >= 10)
                            slideUpIncrement = this.Height / Math.Min((value / 10), this.Height);
                        else
                            slideUpIncrement = this.Height;
                        break;
                }
            }
        }
        /// <summary>
        ///  消失延时 (毫秒)
        /// </summary>
        public int DisappearingDelay
        {
            get
            {
                return disappearingDelay;
            }
            set
            {
                disappearingDelay = value;
                switch (spreadDirection)
                {
                    case Spread.Horizontally:
                        if (value >= 10)
                            slideDownIncrement = this.Width / Math.Min((value / 10), this.Width);
                        else
                            slideDownIncrement = this.Width;
                        break;
                    case Spread.Vertically:
                        if (value >= 10)
                            slideDownIncrement = this.Height / Math.Min((value / 10), this.Height);
                        else
                            slideDownIncrement = this.Height;
                        break;
                }
            }
        }
        /// <summary>
        ///  停留延时 (毫秒)
        /// </summary>
        public int StayDelay
        {
            get
            {
                return stayDealy;
            }
            set
            {
                stayDealy = value;
            }
        }
        /// <summary>
        ///  初始位置
        /// </summary>
        public Point InitialPos
        {
            get
            {
                if (initialPos.X == 0 && initialPos.Y == 0)
                {
                    switch (spreadDirection)
                    { 
                        case Spread.Horizontally:
                            initialPos = new Point(
                                primaryScreen.Bounds.Width, 
                                primaryScreen.WorkingArea.Height + primaryScreen.WorkingArea.Y - PopupSize.Height
                            );
                            break;
                        case Spread.Vertically:                            
                            initialPos = new Point(
                                primaryScreen.WorkingArea.Width + primaryScreen.WorkingArea.X - PopupSize.Width, 
                                primaryScreen.Bounds.Height
                            );
                            break;
                    }
                }
                return initialPos;
            }
            set
            {
                initialPos = value;
            }
        }
        /// <summary>
        ///  窗体大小
        /// </summary>
        public Size PopupSize
        {
            get
            {
                if (BackgroundBitmap != null)
                {
                    return BackgroundBitmap.Size;
                }
                else
                {
                    return this.Size;
                }
            }
        }

        public PopupStatus CurrentStatus
        {
            get
            {
                return currentStatus;
            }
        }

        public Spread SpreadDirection
        {
            get
            {
                return spreadDirection;
            }
            set
            {
                spreadDirection = value;
            }
        }
        #endregion
        
        #region 构造函数

        public SlidePopup()
        {
            InitializeComponent();


            popupTimer = new Timer();
            popupTimer.Tick += new EventHandler(PopupTimer_Tick);
            this.MouseHover += new EventHandler(frmAlarmPopup_MouseHover);
            this.MouseLeave += new EventHandler(frmAlarmPopup_MouseLeave);   
        }

        /// <summary>
        ///  实例化AlarmPopup窗体
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="brief">摘要</param>
        /// <param name="time">时间</param>
        public SlidePopup(string caption, string brief, string time) : this()
        {
            InitializeComponent();

            this.AllowMove = false;
            this.CaptionText = caption;
            this.ContentText = brief;
            this.EventTimeText = time;      
        }
        /// <summary>
        ///  实例化AlarmPopup窗体
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="brief"></param>
        /// <param name="time"></param>
        /// <param name="position"></param>
        public SlidePopup(string caption, string brief, string time, Point position)
            : this(caption, brief, time)
        {
            this.initialPos = position;
        }

        #endregion


        #region 方法

        #region 绘图方法

        protected void DrawText(Graphics grfx)
        {
            if (CaptionText != null && CaptionText.Length != 0)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.FitBlackBox;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                Rectangle TitleRectangle = new Rectangle(10, 5, 80, 30);
                //if (bIsMouseOverTitle)
                //    grfx.DrawString(CaptionText, hoverTitleFont, new SolidBrush(hoverTitleColor), TitleRectangle, sf);
                //else
                //    grfx.DrawString(CaptionText, normalTitleFont, new SolidBrush(normalTitleColor), TitleRectangle, sf);
                grfx.DrawString(CaptionText, CaptionFont, new SolidBrush(normalTitleColor), TitleRectangle, sf);
            }

            if (ContentText != null && ContentText.Length != 0)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                sf.Trimming = StringTrimming.Word;							// Added Rev 002
                Rectangle ContentRectangle = new Rectangle(TitleRectangle.X, TitleRectangle.Bottom + 5, this.Width - TitleRectangle.X, this.Height - TitleRectangle.Bottom - 10);
                //if (bIsMouseOverContent)
                //{
                //    grfx.DrawString(contentText, hoverContentFont, new SolidBrush(hoverContentColor), ContentRectangle, sf);
                //    if (EnableSelectionRectangle)
                //        ControlPaint.DrawBorder3D(grfx, RealContentRectangle, Border3DStyle.Etched, Border3DSide.Top | Border3DSide.Bottom | Border3DSide.Left | Border3DSide.Right);

                //}
                //else
                //    grfx.DrawString(contentText, normalContentFont, new SolidBrush(normalContentColor), ContentRectangle, sf);
                grfx.DrawString(ContentText, ContentFont, new SolidBrush(NormalContentColor), ContentRectangle, sf);
            }
        }
        /// <summary>
        ///  绘制背景
        /// </summary>
        /// <param name="pea"></param>
        //protected override void OnPaintBackground(PaintEventArgs pea)
        //{
        //    Graphics grfx = pea.Graphics;
        //    grfx.PageUnit = GraphicsUnit.Pixel;

        //    Graphics offScreenGraphics;
        //    Bitmap offscreenBitmap;

        //    if (BackgroundBitmap != null)
        //    {
        //        offscreenBitmap = new Bitmap(BackgroundBitmap.Width, BackgroundBitmap.Height);
        //        offScreenGraphics = Graphics.FromImage(offscreenBitmap);

        //        offScreenGraphics.DrawImage(BackgroundBitmap, 0, 0, BackgroundBitmap.Width, BackgroundBitmap.Height);
        //    }
        //    else
        //    {
        //        offscreenBitmap = new Bitmap(this.Width, this.Height);
        //        offScreenGraphics = Graphics.FromImage(offscreenBitmap);

        //        //offScreenGraphics.DrawImage(BackgroundBitmap, 0, 0, BackgroundBitmap.Width, BackgroundBitmap.Height);
        //    }

        //    //DrawCloseButton(offScreenGraphics);
        //    DrawText(offScreenGraphics);
        //    grfx.DrawImage(offscreenBitmap, 0, 0);

        //}
        /// <summary>
        ///  设置背景图片及其透明色
        /// </summary>
        /// <param name="strFilename"></param>
        /// <param name="transparencyColor"></param>
        /// <returns></returns>
        public void SetBackgroundBitmap(string strFilename, Color transparencyColor)
        {
            this.BackColor = transparencyColor;
            BackgroundBitmap = new Bitmap(strFilename);
            Width = BackgroundBitmap.Width;
            Height = BackgroundBitmap.Height;
            Region = BitmapToRegion(BackgroundBitmap, transparencyColor);
        }
        /// <summary>
        ///  设置背景图片及其透明色
        /// </summary>
        /// <param name="image"></param>
        /// <param name="transparencyColor"></param>
        /// <returns></returns>
        public void SetBackgroundBitmap(Image image, Color transparencyColor)
        {
            this.BackColor = transparencyColor;
            BackgroundBitmap = new Bitmap(image);
            Width = BackgroundBitmap.Width;
            Height = BackgroundBitmap.Height;
            Region = BitmapToRegion(BackgroundBitmap, transparencyColor);
        }

        protected Region BitmapToRegion(Bitmap bitmap, Color transparencyColor)
        {
            if (bitmap == null)
                throw new ArgumentNullException("Bitmap", "Bitmap cannot be null!");

            int height = bitmap.Height;
            int width = bitmap.Width;

            GraphicsPath path = new GraphicsPath();

            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                {
                    if (bitmap.GetPixel(i, j) == transparencyColor)
                        continue;

                    int x0 = i;

                    while ((i < width) && (bitmap.GetPixel(i, j) != transparencyColor))
                        i++;

                    path.AddRectangle(new Rectangle(x0, j, i - x0, 1));
                }

            Region region = new Region(path);
            path.Dispose();
            return region;
        }

        #endregion

        /// <summary>
        ///  显示弹出警告
        /// </summary>
        public void ShowPopupAlert()
        {
            if (this.currentStatus == PopupStatus.Appearing)                 
                return;

            this.currentStatus = PopupStatus.Appearing;
            // 设置窗体初始位置
            this.Location = InitialPos;
            // 设置透明变量
            double TotalOpacity = 1.00;
            if (UseTransparency)
            {
                TotalOpacity = SpecificOpacity;
            }
            if (spreadDirection == Spread.Horizontally)
            {
                this.opacityIncrement = TotalOpacity / (PopupSize.Width / slideUpIncrement);
            }
            else
            {
                this.opacityIncrement = TotalOpacity / (PopupSize.Height / slideUpIncrement);
            }
            // 显示窗体避免抢占焦点            
            ShowWindow(this.Handle, 4);

            this.Opacity = FadeInOut ? 0 : 1;

            // 设置初始显示间隔
            popupTimer.Interval = 10;
            popupTimer.Start();
        }

        #endregion



        #region 事件

        // 窗体加载显示
        private void frmAlarm_Load(object sender, EventArgs e)
        {
        }
        // 鼠标移出窗体
        protected void frmAlarmPopup_MouseLeave(object sender, EventArgs e)
        {
            if (Form.MousePosition.X > this.ClientRectangle.X + this.Width ||
                Form.MousePosition.Y > this.ClientRectangle.Y + this.Height ||
                Form.MousePosition.X < this.ClientRectangle.X ||
                Form.MousePosition.Y < this.ClientRectangle.Y)
            {
                isMouseOver = false;
            }
        }
        // 鼠标停留窗体
        protected void frmAlarmPopup_MouseHover(object sender, EventArgs e)
        {
            this.isMouseOver = true;
        }
        // 弹出Timer
        protected void PopupTimer_Tick(object sender, EventArgs e)
        {
            switch (currentStatus)
            {
                case PopupStatus.Appearing:
                    if (spreadDirection == Spread.Vertically)
                    {
                        #region 垂直显示

                        if (this.Location.Y <= primaryScreen.WorkingArea.Height + primaryScreen.WorkingArea.Y - PopupSize.Height)
                        {
                            popupTimer.Stop();
                            currentStatus = PopupStatus.Visible;
                            this.Location = new Point(
                                InitialPos.X,
                                primaryScreen.WorkingArea.Height + primaryScreen.WorkingArea.Y - PopupSize.Height
                            );
                            this.Activate();
                            // 设置显示之后的等待时间间隔
                            popupTimer.Interval = stayDealy;
                            popupTimer.Start();
                        }
                        else
                        {
                            this.Location = new Point(
                                InitialPos.X,
                                this.Location.Y - slideUpIncrement
                            );
                            if (FadeInOut)
                            {
                                this.Opacity = this.Opacity + this.opacityIncrement;
                            }
                        }

                        #endregion
                    }
                    else if (spreadDirection == Spread.Horizontally)
                    {
                        #region 水平显示

                        if (this.Location.X <= primaryScreen.Bounds.Width - PopupSize.Width)
                        {
                            popupTimer.Stop();
                            currentStatus = PopupStatus.Visible;
                            this.Location = new Point(
                                primaryScreen.Bounds.Width - PopupSize.Width,
                                initialPos.Y);

                            this.Activate();
                            // 设置显示之后的等待时间间隔
                            popupTimer.Interval = stayDealy;
                            popupTimer.Start();
                        }
                        else
                        {
                            this.Location = new Point(this.Location.X - slideUpIncrement, initialPos.Y);
                            if (FadeInOut)
                            {
                                this.Opacity = this.Opacity + opacityIncrement;
                            }
                        }

                        #endregion
                    }
                    break;
                case PopupStatus.Visible:
                    popupTimer.Stop();
                    popupTimer.Interval = 10;
                    if (!isMouseOver)   // 如果鼠标没有悬浮
                    {
                        currentStatus = PopupStatus.Disappearing;
                    }

                    popupTimer.Start();
                    break;
                case PopupStatus.Disappearing:
                    if (spreadDirection == Spread.Vertically)
                    {
                        #region 垂直隐藏

                        if (isMouseOver && !closeAction)
                        {
                            currentStatus = PopupStatus.Appearing;
                        }
                        else
                        {
                            if (this.Location.Y >= primaryScreen.Bounds.Height)
                            {
                                popupTimer.Stop();
                                currentStatus = PopupStatus.Hidden;
                                //this.Opacity = SpecificOpacity;
                                base.Hide();
                            }
                            else
                            {
                                this.Location = new Point(
                                    InitialPos.X,
                                    this.Location.Y + slideDownIncrement
                                );
                                if (FadeInOut)
                                {
                                    this.Opacity = this.Opacity - opacityIncrement;
                                }
                            }
                        }

                        #endregion
                    }
                    else if (spreadDirection == Spread.Horizontally)
                    {
                        #region 水平隐藏

                        if (isMouseOver && !closeAction)
                        {
                            currentStatus = PopupStatus.Appearing;
                        }
                        else
                        {
                            if (this.Location.X >= primaryScreen.Bounds.Width)
                            {
                                popupTimer.Stop();
                                currentStatus = PopupStatus.Hidden;
                                //this.Opacity = SpecificOpacity;
                                base.Hide();
                            }
                            else
                            {
                                this.Location = new Point(this.Location.X + slideDownIncrement, initialPos.Y);
                                if (FadeInOut)
                                {
                                    this.Opacity = this.Opacity - opacityIncrement;
                                }
                            }
                        }

                        #endregion
                    }
                    break;
            }
        }
        // 右键隐藏
        private void frmAlarmPopup_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (PopupStatus.Visible == currentStatus)
                {
                    closeAction = true;
                    isMouseOver = false;
                    popupTimer.Interval = 10;
                    popupTimer.Start();
                    currentStatus = PopupStatus.Disappearing;
                }
            }
        }


        #endregion

        private void SlidePopup_Paint(object sender, PaintEventArgs e)
        {
            DrawText(e.Graphics);
        }



        #region 调用系统api
        /*  调用系统api
        //从左到右显示
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        //从右到左显示
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        //从上到下显示
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        //从下到上显示
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        //若使用了AW_HIDE标志，则使窗口向内重叠，即收缩窗口；否则使窗口向外扩展，即展开窗口
        public const Int32 AW_CENTER = 0x00000010;
        //隐藏窗口，缺省则显示窗口
        public const Int32 AW_HIDE = 0x00010000;
        //激活窗口。在使用了AW_HIDE标志后不能使用这个标志
        public const Int32 AW_ACTIVATE = 0x00020000;
        //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        public const Int32 AW_SLIDE = 0x00040000;
        //透明度从高到低
        public const Int32 AW_BLEND = 0x00080000;
        

        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr whnd, int dwtime, int dwflag);

        //AnimateWindow(this.Handle, 3000, AW_HOR_POSITIVE | AW_HIDE);
        */
        #endregion

    }
}
