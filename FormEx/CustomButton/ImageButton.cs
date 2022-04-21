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

    // http://www.codeproject.com/Articles/29010/WinForm-ImageButton

    public class ImageButton : PictureBox, IButtonControl
    {
        #region  Fileds

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private DialogResult _DialogResult;
        private Image _HoverImage;
        private Image _DownImage;
        private Image _NormalImage;
        private bool _hover = false;
        private bool _down = false;
        private bool _isDefault = false;
        private bool _holdingSpace = false;
        private bool _leftClick = false;

        private ToolTip _toolTip = new ToolTip();

        #endregion

        #region Constructor

        public ImageButton()
            : base()
        {
            // 1.让PictureBox支持焦点
            EnableTabStop();

            this.TabStop = true;

            this.BackColor = Color.Transparent;
            this.Size = new Size(75, 23);
        }


        #endregion

        #region IButtonControl Members

        public DialogResult DialogResult
        {
            get
            {
                return _DialogResult;
            }
            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                {
                    _DialogResult = value;
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


        public event EventHandler NormalImageChanged;
        protected virtual void OnNormalImageChanged()
        {
            NormalImageChanged?.Invoke(this, EventArgs.Empty);
        }

        #region  Properties


        public bool ShowFocusLine
        {
            get;
            set;
        }

        [Category("Custom")]
        [Description("Image to show when the button is not in any other state.")]
        [DefaultValue(null)]
        public Image NormalImage
        {
            get
            {
                return _NormalImage;
            }
            set
            {
                _NormalImage = value;
                if (!(_hover || _down))
                {
                    Image = value;
                }

                OnNormalImageChanged();
            }
        }
        [Category("Custom")]
        [Description("Image to show when the button is hovered over.")]
        [DefaultValue(null)]
        public Image HoverImage
        {
            get { return _HoverImage; }
            set { _HoverImage = value; if (_hover) Image = value; }
        }

        [Category("Custom")]
        [Description("Image to show when the button is depressed.")]
        [DefaultValue(null)]
        public Image DownImage
        {
            get { return _DownImage; }
            set { _DownImage = value; if (_down) Image = value; }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Custom")]
        [Description("The text associated with the control.")]
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

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Custom")]
        [Description("The font used to display text in the control.")]
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
        [Category("Custom")]
        public override System.Drawing.Color ForeColor
        {
            get;
            set;
        }


        #endregion

        #region Description Changes
        [Description("Controls how the ImageButton will handle image placement and control sizing.")]
        public new PictureBoxSizeMode SizeMode { get { return base.SizeMode; } set { base.SizeMode = value; } }

        [Description("Controls what type of border the ImageButton should have.")]
        public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { base.BorderStyle = value; } }
        #endregion

        #region Hiding

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

        public void KeepPress()
        {
            ButtonKeepPressed = true;
            Image = DownImage;
            Invalidate();
        }

        public void Release()
        {
            ButtonKeepPressed = false;
            Image = NormalImage;
            Invalidate();
        }

        #region override

        protected override void OnMouseEnter(EventArgs e)
        {
            //show tool tip 
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

            if (ButtonKeepPressed)
            {
                base.OnMouseMove(e);
                return;
            }

            if (_down)
            {
                if ((_DownImage != null) && (Image != _DownImage))
                {
                    Image = _DownImage;
                    Invalidate();
                }
            }
            else
            {
                if (_HoverImage != null)
                {
                    Image = _HoverImage;
                }
                else
                {
                    Image = _NormalImage;
                }
                Invalidate();
            }


            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;

            if (ButtonKeepPressed)
            {
            }
            else
            {
                Image = _NormalImage;
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

                if (ButtonKeepPressed)
                {
                    Release();
                    return;
                }

                if (_DownImage != null)
                {
                    Image = _DownImage;
                    Invalidate();
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _leftClick = false;

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
                    Image = _NormalImage;
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

            // 3.让PictureBox支持焦点
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
                    _toolTip.Dispose();
            }
            _toolTip = null;
            base.Dispose(disposing);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Refresh();
            base.OnTextChanged(e);
        }

        #endregion

        #region 提示文本

        [Category("Custom")]
        [Description("当鼠标放在控件可见处的提示文本")]
        public string ToolTipText { get; set; }


        private void ShowTooTip(string toolTipText)
        {
            _toolTip.Active = true;
            _toolTip.SetToolTip(this, toolTipText);
        }

        private void HideToolTip()
        {
            _toolTip.Active = false;
        }

        #endregion


        #region 2.让PictureBox支持焦点

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


        protected void EnableTabStop()
        {
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        #endregion


        #region 让按钮保持按下状态

        private bool _buttonKeepPressed;
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

        #endregion


        private Color _hotTrackColor = Color.FromArgb(244, 244, 243);
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

    }
}
