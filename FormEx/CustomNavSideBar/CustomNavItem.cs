using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    [Browsable(false)]
    [DefaultEvent("SingleClick")]
    public partial class CustomNavItem : NonFlickerUserControl
    {
        public CustomNavItem()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            if (DesignMode)
            {
                UpdateComponentsHeight();
            }

        }

        #region 事件

        public event EventHandler SingleClick;

        #endregion

        #region 属性

        [Category("Custom")]
        [DefaultValue("")]
        public string Caption {
            get => CaptionLabel.Text;
            set
            {
                bool isChanging = CaptionLabel.Text != value;
                CaptionLabel.Text = value;
                if (isChanging)
                {
                    CaptionLabel.Visible = !string.IsNullOrEmpty(value);
                    if (DesignMode)
                    {
                        UpdateComponentsHeight();
                    }
                }
            }
        }


        [Category("Custom")]
        [DefaultValue("")]
        public string HeadLine
        {
            get => HeadLineLabel.Text;
            set
            {
                bool isChanging = HeadLineLabel.Text != value;
                HeadLineLabel.Text = value;
                if (isChanging)
                {
                    HeadLineLabel.Visible = !string.IsNullOrEmpty(value);
                    if (DesignMode)
                    {
                        UpdateComponentsHeight();
                    }
                }
            }
        }

        [Category("Custom")]
        [DefaultValue("")]
        public string Subheadline
        {
            get => SubheadlineLabel.Text;
            set
            {
                bool isChanging = SubheadlineLabel.Text != value;
                SubheadlineLabel.Text = value;
                if (isChanging)
                {
                    SubheadlineLabel.Visible = !string.IsNullOrEmpty(value);
                    if (DesignMode)
                    {
                        UpdateComponentsHeight();
                    }
                }
            }
        }

        [Category("Custom")]
        [DefaultValue(null)]
        public Image Icon
        {
            get
            {
                return NavIcon.NormalImage;
            }
            set
            {
                NavIcon.NormalImage = value;
                if (value == null)
                {
                    NavIcon.Visible = false;
                }
                else
                {
                    NavIcon.Width = 32;
                    NavIcon.Visible = true;
                }
            }
        }

        [Category("Custom")]
        [DefaultValue(12)]
        public int IndentSize
        {
            get
            {
                return Padding.Left;
            }
            set
            {
                Padding = new Padding(
                    value,
                    Padding.Top,
                    Padding.Right,
                    Padding.Bottom
                );
            }
        }

        [Category("Custom")]
        [DefaultValue("Color.Empty")]
        public Color HotTrackBgColor
        {
            get;
            set;
        }

        [Category("Custom")]
        [DefaultValue("Color.Empty")]
        public Color HotTrackForeColor
        {
            get;
            set;
        }


        private Color NormalForeColor
        {
            get;
            set;
        }

        private Color _unifiedColor = Color.Black;
        public Color UnifiedColor
        {
            get
            {
                return _unifiedColor;
            }
            set
            {
                _unifiedColor = value;

                CaptionLabel.ForeColor = value;
                HeadLineLabel.ForeColor = value;
                SubheadlineLabel.ForeColor = value;
            }
        }


        private bool ShowHotTrack
        {
            get;
            set;
        }

        #endregion

        #region 私有方法


        internal void UpdateComponentsHeight()
        {
            int count = 0;
            int height = 0;
            if (CaptionLabel.Visible && CaptionLabel.Text.Length > 0)
            {
                height += CaptionLabel.Height;
                count++;
            }
            if (HeadLineLabel.Visible && HeadLineLabel.Text.Length > 0)
            {
                height += HeadLineLabel.Height;
                count++;
            }
            if (SubheadlineLabel.Visible && SubheadlineLabel.Text.Length > 0)
            {
                height += SubheadlineLabel.Height;
                count++;
            }

            //if (RightPanel.AutoSize)
            //{
            //    this.AutoSize = false;
            //    this.AutoSizeMode = AutoSizeMode.GrowOnly;
            //    RightPanel.AutoSize = false;
            //    RightPanel.AutoSizeMode = AutoSizeMode.GrowOnly;
            //    RightPanel.Height = height;
            //    Height = height;
            //    RightPanel.AutoSize = true;
            //}
            //else
            //{
            //    Height = height;
            //}
            Height = height;

            if (count == 1)
            {
                if (CaptionLabel.Visible)
                {
                    CaptionLabel.Height = this.Height - CaptionLabel.Padding.Top - CaptionLabel.Padding.Bottom;
                }
                else if (HeadLineLabel.Visible)
                {
                    HeadLineLabel.Height = this.Height - HeadLineLabel.Padding.Top - HeadLineLabel.Padding.Bottom;
                }
                else if (SubheadlineLabel.Visible)
                {
                    SubheadlineLabel.Height = this.Height - SubheadlineLabel.Padding.Top - SubheadlineLabel.Padding.Bottom;
                }

            }


            if (NavIcon.Image == null)
            {
                NavIcon.Visible = false;
            }
            else
            {
                NavIcon.Visible = true;
                NavIcon.Location = new Point(NavIcon.Location.X, (this.Height - NavIcon.Height) / 2);
            }
        }


        #endregion


        private void NavigationItemControl_Load(object sender, EventArgs e)
        {
            NormalForeColor = CaptionLabel.ForeColor;

            UpdateComponentsHeight();
        }

        private void NavigationItemControl_Paint(object sender, PaintEventArgs e)
        {
            if (ShowHotTrack && HotTrackBgColor != Color.Empty)
            {
                Rectangle rect = new Rectangle(0, 0, this.Width - Padding.Left - Padding.Right - 1, this.Height - Padding.Top - Padding.Bottom - 1);
                using (var bufferBitmap = new Bitmap(this.Width, this.Height))
                using (var bufferGraphics = Graphics.FromImage(bufferBitmap))
                {
                    using (Brush brush = new SolidBrush(HotTrackBgColor))
                    {
                        bufferGraphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        bufferGraphics.FillRoundedRectangle(brush, 
                            0, 0,
                            this.Width - Padding.Left - Padding.Right - 1,
                            this.Height - Padding.Top - Padding.Bottom - 1,
                            8);
                    }

                    e.Graphics.DrawImageUnscaled(bufferBitmap, Point.Empty);
                }


            }
        }






        private void NavigationItemControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SingleClick?.Invoke(this, e);
            }
        }


        private void NavigationItemControl_MouseEnter(object sender, EventArgs e)
        {
            ShowHotTrack = true;
            if (HotTrackForeColor != Color.Empty)
            {
                CaptionLabel.ForeColor = HotTrackForeColor;
                HeadLineLabel.ForeColor = HotTrackForeColor;
                SubheadlineLabel.ForeColor = HotTrackForeColor;
            }

            Invalidate();
        }

        private void NavigationItemControl_MouseLeave(object sender, EventArgs e)
        {
            if (ShowHotTrack)
            {
                ShowHotTrack = false;
                if (HotTrackForeColor != Color.Empty)
                {
                    CaptionLabel.ForeColor = NormalForeColor;
                    HeadLineLabel.ForeColor = NormalForeColor;
                    SubheadlineLabel.ForeColor = NormalForeColor;
                }

                Invalidate();
            }
        }

        private void SubheadlineLabel_TextChanged(object sender, EventArgs e)
        {
            using (Graphics g = SubheadlineLabel.CreateGraphics())
            {
                SizeF size = g.MeasureString(SubheadlineLabel.Text, SubheadlineLabel.Font, SubheadlineLabel.Width);
                SubheadlineLabel.Height = (int)Math.Ceiling(size.Height) + SubheadlineLabel.Padding.Top + SubheadlineLabel.Padding.Bottom;
            }
        }

        private void CaptionLabel_TextChanged(object sender, EventArgs e)
        {
            using (Graphics g = CaptionLabel.CreateGraphics())
            {
                SizeF size = g.MeasureString(CaptionLabel.Text, CaptionLabel.Font, CaptionLabel.Width);
                CaptionLabel.Height = (int)Math.Ceiling(size.Height) + CaptionLabel.Padding.Top + CaptionLabel.Padding.Bottom;
            }
        }

        private void HeadLineLabel_TextChanged(object sender, EventArgs e)
        {
            using (Graphics g = HeadLineLabel.CreateGraphics())
            {
                SizeF size = g.MeasureString(HeadLineLabel.Text, HeadLineLabel.Font, HeadLineLabel.Width);
                HeadLineLabel.Height = (int)Math.Ceiling(size.Height) + HeadLineLabel.Padding.Top + HeadLineLabel.Padding.Bottom;
            }
        }
    }
}
