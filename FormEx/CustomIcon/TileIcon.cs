using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    /// <summary>
    /// 平铺样式的图标
    /// </summary>
    [DefaultEvent("SingleClick")]
    public class TileIcon : NonFlickerUserControl
    {
        #region 事件

        public event MouseEventHandler SingleClick;

        #endregion

        #region 字段

        protected Image _image;
        protected Image _defaultImage;
        protected bool _defaultImageDefined;
        protected Color _lastBackColor;
        protected Color _selectedBackColor;
        protected string _iconText;
        protected bool _isSelected;
        protected bool _isHovering;
        protected ToolTip _tooltip = new ToolTip();

        #endregion

        #region 构造

        public TileIcon()
        {
            HoverBackColor = Color.DodgerBlue;
            _selectedBackColor = Color.DodgerBlue;
            HotTrack = true;

            this.ShowImage = true;
            this.ShowSplitter = true;
            this.Padding = new Padding(5);
            this.Margin = new Padding(0);
            this.BackColor = Color.FromArgb(248, 248, 248);
            this.MouseEnter += new EventHandler(TileIcon_MouseEnter);
            this.MouseLeave += new EventHandler(TileIcon_MouseLeave);
            this.MouseHover += TileIcon_MouseHover;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        #endregion

        #region 属性


        [Category("Custom")]
        public Color HoverBackColor { get; set; }


        [Category("Custom")]
        public Color SelectedBackColor
        {
            get { return _selectedBackColor; }
            set { _selectedBackColor = value; }
        }

        [Category("Custom")]
        [DefaultValue(null)]
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        [DefaultValue(null)]
        public Drawing.Image DefaultImage
        {
            get
            {
                return _defaultImage;
            }
            set
            {
                _defaultImage = value;

                _defaultImageDefined = (_defaultImage != null);
            }
        }

        /// <summary>
        /// 这个Text属性无法在设计器中保存下来. 请使用IconText替代.
        /// </summary>
        [Category("Custom")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                return _iconText;
            }
            set
            {
                _iconText = value;
                Invalidate();
            }
        }

        [Category("Custom")]
        public string IconText
        {
            get
            {
                return _iconText;
            }
            set
            {
                _iconText = value;
                Invalidate();
            }
        }


        [Category("Custom")]
        [DefaultValue(typeof(Boolean), "True")]
        public bool ShowImage
        {
            get;
            set;
        }

        [Category("Custom")]
        [DefaultValue(typeof(Boolean), "True")]
        public bool ShowSplitter { get; set; }

        [Category("Custom")]
        [DefaultValue(typeof(Boolean), "True")]
        public bool ShowIconBorder { get; set; }


        [Category("Custom")]
        [DefaultValue(typeof(Boolean), "True")]
        public bool WrapText { get; set; }

        [Category("Custom")]
        [DefaultValue(typeof(Boolean), "True")]
        public bool HotTrack { get; set; }

        [Category("Custom")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool KeepSelected { get; set; }

        [Category("Custom")]
        public virtual bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                Invalidate();
            }
        }

        #endregion

        #region 私有方法

        #endregion

        #region 传承的方法

        protected virtual void DrawDefaultImage()
        {
            Rectangle imgRect = GetImageArea();
            _defaultImage = new Bitmap(imgRect.Width, imgRect.Height);
            using (Graphics g = Graphics.FromImage(_defaultImage))
            {
                //g.DrawGradientRoundedRectangle(new Rectangle(0, 0, imgRect.Width, imgRect.Height), Color.White, Color.White, Color.WhiteSmoke, new Size(3, 3), FillDirection.TopToBottom);

                g.DrawGradientRoundedRectangle(
                    new Rectangle(0, 0, imgRect.Width, imgRect.Height),
                    Color.White,
                    Color.WhiteSmoke,
                    FillDirection.TopToBottom,
                    Color.White,
                    3);
            }
        }

        protected virtual void DrawImage(Graphics g)
        {
            Image img = this.Image == null ? this.DefaultImage : this.Image;
            if (img != null)
            {
                Rectangle rect = this.GetImageArea();
                g.DrawImage(img, rect);
            }
        }

        protected virtual void DrawImageBorder(Graphics g)
        {
            if (!ShowIconBorder)
                return;

            Rectangle rect = this.GetImageArea();
            //rect.Offset(-1, -1);
            g.DrawRectangle(Pens.LightGray, rect);
            rect.Inflate(-1, -1);
            g.DrawRectangle(Pens.White, rect);
        }

        protected virtual void DrawText(Graphics g)
        {
            using (Brush textBrush = new SolidBrush(this.ForeColor))
            using (StringFormat textFormat = new StringFormat())
            {
                textFormat.LineAlignment = StringAlignment.Center;
                textFormat.Alignment = StringAlignment.Near;

                int x, y, width, height;
                x = this.Padding.Left + (this.Height - this.Padding.Top - this.Padding.Bottom) + this.Padding.Left;
                if (!ShowImage)
                {
                    x = this.Padding.Left;
                }
                y = 0;

                // 随控件大小换行
                if (WrapText)
                {
                    width = this.Width - this.Padding.Right - x;
                }
                else
                {
                    // 不换行
                    SizeF stringSize = TextRenderer.MeasureText(IconText, Font);
                    width = (int)stringSize.Width * 2;
                }
                height = this.Height;
                Rectangle textArea = new Rectangle(x, y, width, height);

                g.DrawString(this.IconText, this.Font, textBrush, textArea, textFormat);
            }
        }

        protected virtual void DrawSplitter(Graphics g)
        {
            Pen borderPen = new Pen(this.Parent.BackColor, 2f);
            g.DrawRectangle(borderPen, new Rectangle(0, 0, this.Width, this.Height));
            borderPen.Dispose();
        }

        protected virtual Rectangle GetImageArea()
        {
            int x, y, width, height;
            x = this.Padding.Left;
            y = this.Padding.Top;
            // 图片大小为控件高度的正方形
            width = this.Height - this.Padding.Bottom - this.Padding.Top;
            height = width;

            return new Rectangle(x, y, width, height);
        }

        protected virtual void DrawSelectedBackground(Graphics g)
        {
            if (KeepSelected && IsSelected)
            {
                var activeColor = SelectedBackColor;
                if (_isHovering)
                {
                    activeColor = HoverBackColor;
                }
                using (SolidBrush brush = new SolidBrush(activeColor))
                {
                    g.FillRectangle(brush, this.ClientRectangle);
                }
            }
        }

        #endregion

        #region 重写的方法

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SetSlowRendering();

            DrawSelectedBackground(g);
            

            if (ShowImage)
            {
                DrawImage(g);
                DrawImageBorder(g);
            }

            DrawText(g);

            if (ShowSplitter)
                DrawSplitter(g);

            g.SetFastRendering();
            base.OnPaint(e);

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (KeepSelected)
            {
                IsSelected = !IsSelected;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                if (SingleClick != null)
                {
                    SingleClick(this, e);
                }
            }
        }

        #endregion

        #region 公开方法

        public TileIcon Clone()
        {
            return this.MemberwiseClone() as TileIcon;
        }

        #endregion

        #region 事件处理


        protected virtual void TileIcon_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = _lastBackColor;
            _isHovering = false;
            Invalidate();
        }

        protected virtual void TileIcon_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
            _lastBackColor = this.BackColor;
            if (HotTrack)
            {
                this.BackColor = HoverBackColor;
                _isHovering = true;
            }
            Invalidate();
        }


        private void TileIcon_MouseHover(object sender, EventArgs e)
        {
            Rectangle iconArea = this.GetImageArea();
            if (this.RectangleToScreen(iconArea).Contains(MousePosition))
            {
                _tooltip.SetToolTip(this, this.IconText);
            }
            else
            {
                _tooltip.SetToolTip(this, null);
            }
        }




        #endregion
    }
}
