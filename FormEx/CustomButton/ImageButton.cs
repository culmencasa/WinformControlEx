using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Windows.Forms
{

    /// <summary>
    /// 图片按钮控件
    /// ref: http://www.codeproject.com/Articles/29010/WinForm-ImageButton
    /// </summary>
    public class ImageButton : PictureBox, IButtonControl
    {
        #region 事件

        /// <summary>
        /// 按钮常规图片设置后事件
        /// </summary>
        public event EventHandler NormalImageChanged;

        #endregion

        #region  私有字段

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private DialogResult _dialogResult;
        private Image _hoverImage;
        private Image _downImage;
        private Image _normalImage;
        private bool _hover = false;
        private bool _down = false;
        private bool _isDefault = false;
        private bool _holdingSpace = false;
        private bool _leftClick = false;

        private ToolTip _toolTip = new ToolTip();
        private Color _hotTrackColor = Color.Transparent; //Color.FromArgb(244, 244, 243);

        #endregion
         
        #region 构造

        public ImageButton()
            : base()
        {
            // 启用焦点
            EnableTabStop();

            this.TabStop = true;

            this.BackColor = Color.Transparent;
            this.Size = new Size(75, 23);
        }


        #endregion

        #region 实现 IButtonControl 接口成员

        public DialogResult DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                {
                    _dialogResult = value;
                }
            }
        }

        public void NotifyDefault(bool value)
        {
            if (_isDefault != value)
            {
                _isDefault = value;
            }
        }

        public void PerformClick()
        {
            base.OnClick(EventArgs.Empty);
        }

        #endregion


        #region 公开属性


        /// <summary>
        /// 是否显示焦点虚线
        /// </summary>
        [Category("Behavior")]
        public bool ShowFocusLine
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image NormalImage
        {
            get
            {
                return _normalImage;
            }
            set
            {
                _normalImage = value;
                if (!(_hover || _down))
                {
                    Image = value;
                }

                OnNormalImageChanged();
            }
        }


        [Category("Appearance")]
        [DefaultValue(null)]
        public Image HoverImage
        {
            get { return _hoverImage; }
            set
            {
                _hoverImage = value;
                if (_hover)
                {
                    Image = value;
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image DownImage
        {
            get { return _downImage; }
            set
            {
                _downImage = value;
                if (_down)
                {
                    Image = value;
                }
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        public override Color ForeColor
        {
            get;
            set;
        }


        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
         
        [Category("Appearance")]
        public new PictureBoxSizeMode SizeMode { get { return base.SizeMode; } set { base.SizeMode = value; } }

        [Category("Appearance")]
        public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { base.BorderStyle = value; } }

        /// <summary>
        /// 鼠标悬浮背景色
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color HotTrackColor
        {
            get
            {
                return _hotTrackColor;
            }
            set
            {
                _hotTrackColor = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        [Description("当鼠标放在控件可见处的提示文本")]
        public string ToolTipText { get; set; }


        #endregion

        #region 隐藏属性

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image Image { get { return base.Image; } set { base.Image = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new String ImageLocation { get { return base.ImageLocation; } set { base.ImageLocation = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image ErrorImage { get { return base.ErrorImage; } set { base.ErrorImage = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image InitialImage { get { return base.InitialImage; } set { base.InitialImage = value; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool WaitOnLoad { get { return base.WaitOnLoad; } set { base.WaitOnLoad = value; } }

        #endregion

        #region 重写的方法

        protected override void OnMouseEnter(EventArgs e)
        {
            if (ToolTipText != string.Empty)
            {
                HideToolTip();
                ShowTooTip(ToolTipText);
            }

            base.OnMouseEnter(e);

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _hover = true;

            if (_down)
            {
                _bms = ButtonMouseStatus.Pressed | ButtonMouseStatus.Focused;
            }
            else
            {
                _bms = ButtonMouseStatus.Focused;
            }

            if (ButtonKeepPressed)
            {
                base.OnMouseMove(e);
                return;
            }

            if (_down)
            {
                if ((_downImage != null) && (Image != _downImage))
                {
                    Image = _downImage;
                    Invalidate();
                }
            }
            else
            {
                if (_hoverImage != null)
                {
                    Image = _hoverImage;
                }
                else
                {
                    Image = _normalImage;
                }
                Invalidate();
            }


            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            _bms = _bms & (~ButtonMouseStatus.Focused) | ButtonMouseStatus.FocusLost;

            if (ButtonKeepPressed)
            {
            }
            else
            {
                Image = _normalImage;
            }
            base.OnMouseLeave(e);

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.Focus();

            if (e.Button == MouseButtons.Left)
            {
                _leftClick = true;
                //OnMouseUp(null);
                _down = true;
                _bms = (_bms & (~ButtonMouseStatus.Released)) | ButtonMouseStatus.Pressed;

                if (ButtonKeepPressed)
                {
                    ReleasePressing();
                    return;
                }

                if (_downImage != null)
                {
                    Image = _downImage;
                    Invalidate();
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _leftClick = false;

            if (this.IsHandleCreated == false || IsDisposed)
            {
                base.OnMouseUp(e);
                return;
            }

            if (this.MouseIsOverControl())
            {
                _bms = ButtonMouseStatus.Focused | ButtonMouseStatus.Released;
            }
            else
            {
                _bms = ButtonMouseStatus.FocusLost | ButtonMouseStatus.Released;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (ButtonKeepPressed)
                {
                    base.OnMouseUp(e);
                    return;
                }


                if (_down)
                {
                    _down = false;
                    Image = _normalImage;
                    Invalidate();
                }
                //else if (_hover)
                //{
                //    if (_HoverImage != null)
                //        Image = _HoverImage;
                //}
                //else
                //{
                //    Image = _NormalImage;
                //}

            }
            base.OnMouseUp(e);
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == WM_KEYUP)
            {
                if (_holdingSpace)
                {
                    if ((int)msg.WParam == (int)Keys.Space)
                    {
                        OnMouseUp(null);
                        PerformClick();
                    }
                    else if ((int)msg.WParam == (int)Keys.Escape
                        || (int)msg.WParam == (int)Keys.Tab)
                    {
                        _holdingSpace = false;
                        OnMouseUp(null);
                    }
                }
                return true;
            }
            else if (msg.Msg == WM_KEYDOWN)
            {
                if ((int)msg.WParam == (int)Keys.Space)
                {
                    _holdingSpace = true;
                    OnMouseDown(null);
                }
                else if ((int)msg.WParam == (int)Keys.Enter)
                {
                    PerformClick();
                }
                return true;
            }
            else
                return base.PreProcessMessage(ref msg);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _holdingSpace = false;
            //OnMouseUp(null);
            base.OnLostFocus(e);
        }

        protected override void OnClick(EventArgs e)
        {
            // picturebox的click不区别左右键
            if (_leftClick)
            {
                base.OnClick(e);
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            // 画鼠标悬浮背景色
            if (_hover)
            {
                using (SolidBrush brush = new SolidBrush(HotTrackColor))
                {
                    g.FillRectangle(brush, this.ClientRectangle);
                }
            }
            base.OnPaint(pe);


            if ((!string.IsNullOrEmpty(Text)) && (pe != null) && (base.Font != null))
            {
                SizeF drawStringSize = TextRenderer.MeasureText(base.Text, base.Font);
                PointF drawPoint;
                if (base.Image != null)
                {
                    drawPoint = new PointF(
                        base.Image.Width / 2 - drawStringSize.Width / 2,
                        base.Image.Height / 2 - drawStringSize.Height / 2);
                }
                else
                {
                    drawPoint = new PointF(
                        base.Width / 2 - drawStringSize.Width / 2,
                        base.Height / 2 - drawStringSize.Height / 2);
                }

                using (SolidBrush drawBrush = new SolidBrush(this.ForeColor))
                {
                    pe.Graphics.DrawString(base.Text, base.Font, drawBrush, drawPoint);
                }
            }

            // 3. TabStop焦点
            if (ShowFocusLine && this.Focused)
            {
                var rc = this.ClientRectangle;
                rc.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_toolTip != null)
                {
                    _toolTip.Dispose();
                }
            }
            _toolTip = null;
            base.Dispose(disposing);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Refresh();
            base.OnTextChanged(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);


        }
        protected override void OnLeave(EventArgs e)
        {
            //todo: 不能通过Tab来丢失焦点
            this.Invalidate();
            base.OnLeave(e);
        }

        #endregion


        #region 私有方法

        #region 增加提示文本功能

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="toolTipText"></param>
        private void ShowTooTip(string toolTipText)
        {
            _toolTip.Active = true;
            _toolTip.SetToolTip(this, toolTipText);
        }

        /// <summary>
        /// 关闭显示
        /// </summary>
        private void HideToolTip()
        {
            _toolTip.Active = false;
        }

        #endregion


        #region 使控件支持焦点

        [Category("Custom")]
        [Browsable(true), EditorBrowsable()]
        public new bool TabStop
        {
            get
            {
                return base.TabStop;
            }
            set
            {
                base.TabStop = value;
            }
        }

        [Category("Custom")]
        [Browsable(true), EditorBrowsable()]
        public new int TabIndex
        {
            get
            {
                return base.TabIndex;
            }
            set
            {
                base.TabIndex = value;
            }
        }

        /// <summary>
        /// 使控件支持焦点
        /// </summary>
        protected virtual void EnableTabStop()
        {
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        #endregion


        #region 让按钮保持按下状态

        private bool _buttonKeepPressed;

        /// <summary>
        /// 设置或获取按钮按下状态
        /// </summary>
        [Category("Behavior")]
        public bool ButtonKeepPressed
        {
            get
            {
                return _buttonKeepPressed;
            }
            set
            {
                _buttonKeepPressed = value;
                if (value == false)
                {
                    Image = NormalImage;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 使按钮成为按下状态
        /// </summary>
        public void KeepPress()
        {
            ButtonKeepPressed = true;
            Image = DownImage;
            Invalidate();

            _bms = ButtonMouseStatus.Pressed;
        }

        /// <summary>
        /// 使按钮成为正常状态
        /// </summary>
        public void ReleasePressing()
        {
            ButtonKeepPressed = false;
            Image = NormalImage;
            Invalidate();

            _bms = ButtonMouseStatus.Released;
        }

        #endregion

        #endregion


        #region 保护方法

        protected virtual void OnNormalImageChanged()
        {
            NormalImageChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion



        [Flags]
        public enum ButtonMouseStatus
        {
            None  = 0,
            Focused = 1,
            Pressed = 2,
            Released = 4 ,
            FocusLost = 8
        }


        ButtonMouseStatus _bms = ButtonMouseStatus.None;

        public ButtonMouseStatus Bms

        {
            get
            {
                return _bms;
            }
            set
            {
                _bms = value;
            }
        }
    }
}
