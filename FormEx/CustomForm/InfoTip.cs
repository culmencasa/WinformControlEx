using System.Collections.Generic;
using System.Drawing;
using Utils.UI;

namespace System.Windows.Forms
{
    /// <summary>
    /// 提示小窗口
    /// </summary>
    public partial class InfoTip : RoundedCornerForm
    {
        public enum Position
        {
            Right,
            FollowMouse
        }

        #region 构造

        public InfoTip()
        {
            InitializeComponent();

            _borderSize = 0;

            RoundCornerDiameter = 0;
            TitleText = string.Empty;
            ContentText = string.Empty;
            MaxSize = new Size(300, 250);

            this.InfoIcon.Image = Properties.Resources.lightbub;
        }

        #endregion

        #region 字段

        private string _titleText;
        private string _contentText;

        private DropShadow _dropShadow;

        #endregion

        #region 属性

        public new string TitleText
        {
            get
            {
                return _titleText;
            }
            set
            {
                _titleText = value;
                OnTitleChanged();
            }
        }

        public string ContentText
        {
            get
            {
                return _contentText;
            }
            set
            {
                _contentText = value;
                OnContentChanged();
            }
        }

        private Size MaxSize
        {
            get;
            set;
        }

        /// <summary>
        /// 显示时长。默认为5秒。
        /// </summary>
        public int TimeOut
        {
            get;
            private set;
        }

        /// <summary>
        /// 剩余显示时间
        /// </summary>
        private int Remain
        {
            get;
            set;
        }

        /// <summary>
        /// 显示计时器
        /// </summary>
        protected Timer ShowTimer
        {
            get;
            private set;
        }

        /// <summary>
        /// 表示窗体是否正在关闭
        /// </summary>
        private bool IsDeclining
        {
            get;
            set;
        }

        public Position InfoTipShowPosition
        {
            get;
            set;
        } = Position.FollowMouse;

        #endregion

        #region 方法

        private void OnTitleChanged()
        {
            lblTitle.Text = TitleText;

        }
        private void OnContentChanged()
        {
            lblText.Text =  ContentText;

            if (ContentText.Length == 0)
            {
                return;
            }

            RecalculateSize();

            if (IsHandleCreated)
            {
                _dropShadow?.Redraw(true);
            }

        }

        private void RecalculateSize()
        {

            #region 计算宽度

            var controlNewWidth = lblText.Width;
            var paddingLR = this.Padding.Left + lblText.Padding.Left + lblText.Padding.Right + this.Padding.Right + lblText.Margin.All * 2;
            var paddingTB = this.Padding.Top + this.Padding.Bottom + lblText.Padding.Top + lblText.Padding.Bottom;
            var font = lblText.Font;
            var widthFirstSize = new Size(150, 30);
            if (IsHandleCreated)
            {
                widthFirstSize = TextRenderer.MeasureText(ContentText, font);
            }
            else
            {
                using (var g = this.CreateGraphics())
                {
                    var tempSizeF = g.MeasureString(ContentText, font, new SizeF(float.MaxValue, 30));
                    widthFirstSize = new Size((int)tempSizeF.Width, (int)tempSizeF.Height);
                }
            }
            if (widthFirstSize.Width > MaxSize.Width)
            {
                controlNewWidth = MaxSize.Width;
            }
            else
            {
                controlNewWidth = widthFirstSize.Width + iconHolder.Width + paddingLR;
            }

            #endregion


            #region 计算高度


            if (string.IsNullOrEmpty(TitleText))
            {
                lblTitle.Height = 5;
            }
            else
            {
                lblTitle.Height = 30;
            }


            var textSize = TextRenderer.MeasureText(ContentText, lblText.Font,
                new Size(controlNewWidth, int.MaxValue), TextFormatFlags.WordBreak);

            var controlNewHeight = 0;
            var titleHeight = lblTitle.Height;
            if (textSize.Height > MaxSize.Height - titleHeight)
            {
                controlNewHeight = MaxSize.Height;
                // 如果设置了AutoEllipsis, Label会在显示省略号时显示Tooltip
            }
            else
            {
                controlNewHeight = titleHeight + textSize.Height + paddingTB + 10;
            }

            #endregion


            this.Size = new Size(controlNewWidth, controlNewHeight);

        }

        private void SetPosition(Control control)
        {
            switch (InfoTipShowPosition)
            {
                case Position.Right:
                    var boundsOnScreen = control.RectangleToScreen(control.ClientRectangle);
                    this.Location = new Point(boundsOnScreen.Right, boundsOnScreen.Top);
                    break;
                case Position.FollowMouse:
                    this.Location = new Point(MousePosition.X + 32, MousePosition.Y);
                    break;
            }
        }

        private bool HitTest(Control control)
        {
            return control.RectangleToScreen(control.ClientRectangle).Contains(MousePosition);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RecalculateSize();
        }
        protected override void DropShadowSetup()
        {
            DropShadow ds = new DropShadow(this);
            ds.BorderRadius = RoundCornerDiameter / 2;
            ds.ShadowRadius = RoundCornerDiameter  / 2;
            ds.BorderColor = ColorEx.DarkenColor(this.BackColor, 30);
            ds.ShadowOpacity = 0.8f;
            ds.Refresh();

            _dropShadow = ds;
        }
        #endregion

        #region 静态成员

        static Dictionary<string, InfoTip> pairs = new Dictionary<string, InfoTip>();

        public static void SetToolTip(Control control, string tooltip, string title = null, Position position = Position.FollowMouse,  int timeout=5000)
        {
            if (control == null)
            {
                throw new ArgumentNullException();
            }

            var infoTipWindow = default(InfoTip);
            //var topWindow = GetTopForm(control);
            var keyName = GetFullName(control);

            // 重用缓存
            var isCacheAlive = pairs.ContainsKey(control.Name);
            if (isCacheAlive)
            {
                infoTipWindow = pairs[control.Name];
                if (!infoTipWindow.IsHandleCreated || infoTipWindow.IsDisposed || infoTipWindow.IsDeclining)
                {
                    isCacheAlive = false;
                }
            }

            if (isCacheAlive)
            {
                infoTipWindow.TitleText = title;
                infoTipWindow.ContentText = tooltip;
                infoTipWindow.Remain += infoTipWindow.TimeOut; // 延长显示时间
                infoTipWindow.InfoTipShowPosition = position;
                //infoTipWindow.Show(topWindow);    // 模态窗体
                infoTipWindow.Show(); 
                infoTipWindow.SetPosition(control);
                infoTipWindow.Activate();
            }
            else
            {
                // 创建新窗体
                infoTipWindow = new InfoTip();
                infoTipWindow.TitleText = title;
                infoTipWindow.ContentText = tooltip;
                infoTipWindow.TimeOut = timeout;
                infoTipWindow.Remain = timeout;
                infoTipWindow.InfoTipShowPosition = position;
                infoTipWindow.FormClosing += (a, b) =>
                {
                    infoTipWindow.IsDeclining = true;
                    pairs.Remove(control.Name);
                };
                pairs.Add(control.Name, infoTipWindow);

                // 开启定时器
                var showTimer = new Timer();
                showTimer.Interval = 500;
                showTimer.Tick += (a, b) =>
                {
                    if (infoTipWindow.HitTest(infoTipWindow))
                    {
                        return;
                    }

                    infoTipWindow.Remain -= showTimer.Interval;
                    if (infoTipWindow.Remain <= 0)
                    {
                        showTimer.Stop();
                        //infoTipWindow.Visible = false;  // 模态窗体要求先设为False
                        infoTipWindow.Close();
                        return;
                    }
                    else
                    {
                        //todo: hittest

                        if (!infoTipWindow.HitTest(control))
                        {
                            showTimer.Stop();
                            infoTipWindow.Close();
                            return;
                        }
                        infoTipWindow.TopMost = true;
                    }

                };
                //infoTipWindow.Show(topWindow);    // 模态窗体
                infoTipWindow.Show();
                infoTipWindow.SetPosition(control);
                showTimer.Enabled = true;
            }


        }
        public static string GetFullName(Control control)
        {
            if (control.Parent == null)
                return control.Name;
            return GetFullName(control.Parent) + "." + control.Name;
        }

        public static Form GetTopForm(Control parentControl)
        {
            if (parentControl == null)
                return null;

            Form topForm = null;
            while (topForm == null)
            {
                topForm = parentControl as Form;
                if (topForm == null)
                {
                    return GetTopForm(parentControl.Parent);
                }
                else if (!topForm.TopLevel)
                {
                    return GetTopForm(topForm.Parent);
                }
            }

            return topForm;
        }


        #endregion
    }
}
