using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class Win7ControlBox : UserControl
    {
        #region 字段

        IDpiDefined DpiParent { get; set; }

        #endregion
        
        #region 构造

        public Win7ControlBox()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            UpdateStyles();

            this.BackColor = Color.Transparent;

            InitializeComponent();


            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.btnClose.Click += btnClose_Click;
            this.btnMaximum.Click += btnMaximum_Click;
            this.btnMinimum.Click += btnMinimum_Click;

            // 将用户控件添加到窗体的Controls集合中时，并不会触发ParentChanged事件。这是因为在添加过程中，窗体被设置为用户控件的父级，而不是通过父级控件的Controls集合添加。
            ParentChanged += Win7ControlBox_ParentChanged;

            // 因此, ParentChanged事件的操作放到Load事件里
            Load += Win7ControlBox_Load;
        }

        #endregion



        #region Win7ControlBox的事件处理

        private void Win7ControlBox_Load(object sender, EventArgs e)
        {
            DpiParent = this.ParentForm as IDpiDefined;

            btnClose.NormalImage = ScaleImage(Properties.Resources.btnClose_NormalImage);
            btnClose.HoverImage = ScaleImage(Properties.Resources.btnClose_HoverImage);
            btnClose.DownImage = ScaleImage(Properties.Resources.btnClose_DownImage);

            btnMaximum.NormalImage = ScaleImage(Properties.Resources.btnMaximum_NormalImage);
            btnMaximum.HoverImage = ScaleImage(Properties.Resources.btnMaximum_HoverImage);
            btnMaximum.DownImage = ScaleImage(Properties.Resources.btnMaximum_DownImage);


            btnMinimum.NormalImage = ScaleImage(Properties.Resources.btnMinimum_NormalImage);
            btnMinimum.HoverImage = ScaleImage(Properties.Resources.btnMinimum_HoverImage);
            btnMinimum.DownImage = ScaleImage(Properties.Resources.btnMinimum_DownImage);



            if (ParentForm != null)
            {
                ParentForm.Resize += ParentForm_Resize;
                ParentForm.StyleChanged += ParentForm_StyleChanged;
                ParentForm.Shown += ParentForm_Shown;
            }

            ReloadButtonImage();
            ApplyLayout();
            ShrinkOrGrow();
            AlignRight();
        }

        private void ParentForm_Resize(object? sender, EventArgs e)
        {
            ReloadButtonImage();
            ApplyLayout();
            ShrinkOrGrow();
            AlignRight();
        }

        private void Win7ControlBox_ParentChanged(object sender, EventArgs e)
        {
            DpiParent = this.ParentForm as IDpiDefined;

            if (Parent as Form == null)
            {
                //throw new Exception("控件只能添加到窗体上.");
            }
        }

        private void btnMinimum_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            ParentForm.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximum_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            if (form.WindowState == FormWindowState.Maximized)
            {
                form.WindowState = FormWindowState.Normal;
            }
            else
            {
                form.WindowState = FormWindowState.Maximized;
            }

            ReloadButtonImage();

        }

        private void ReloadButtonImage()
        {
            Form form = this.ParentForm;
            if (form.WindowState == FormWindowState.Normal)
            {
                this.btnMaximum.NormalImage = ScaleImage(Properties.Resources.btnMaximum_NormalImage);
                this.btnMaximum.HoverImage = ScaleImage(Properties.Resources.btnMaximum_HoverImage);
                this.btnMaximum.DownImage = ScaleImage(Properties.Resources.btnMaximum_DownImage);
            }
            else if (form.WindowState == FormWindowState.Maximized)
            {
                this.btnMaximum.NormalImage = ScaleImage(Properties.Resources.restore_normal);
                this.btnMaximum.HoverImage = ScaleImage(Properties.Resources.restore_highlight);
                this.btnMaximum.DownImage = ScaleImage(Properties.Resources.restore_down);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            form.Close();
        }

        #endregion

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }


        #region ParentForm的事件处理

        private void ParentForm_Shown(object sender, EventArgs e)
        {
            BringToFront();
        }


        private void ParentForm_StyleChanged(object sender, EventArgs e)
        {
            ReloadButtonImage();
            ApplyLayout();
            ShrinkOrGrow();
            AlignRight();
        }

        #endregion

        #region 方法

        private void ApplyLayout()
        {
            Form form = this.ParentForm;
            if (form != null)
            {
                if (!form.ControlBox)
                {
                    btnClose.Visible = false;
                    btnMaximum.Visible = false;
                    btnMinimum.Visible = false;
                }
                else
                {
                    btnMaximum.Visible = form.MaximizeBox;
                    btnMinimum.Visible = form.MinimizeBox;
                    btnClose.Visible = true;
                }
            }
        }

        private void ShrinkOrGrow()
        {
            int controlBoxWidth = btnClose.Width;
            if (ParentForm != null)
            {
                controlBoxWidth += ParentForm.MaximizeBox ? btnMaximum.Width : 0;
                controlBoxWidth += ParentForm.MinimizeBox ? btnMinimum.Width : 0;
            }
            else
            {
                controlBoxWidth += btnMaximum.Width;
                controlBoxWidth += btnMinimum.Width;
            }

            this.Width = controlBoxWidth;
        }

        private void AlignRight()
        {
            int border = 2;

            if (Parent != null)
            {
                Location = new Point(Parent.Width - Parent.Padding.Right - this.Width - border, Parent.Padding.Top);
            }
            else if (ParentForm != null)
            {
                Location = new Point(ParentForm.Width - Parent.Padding.Right - Width - border, Parent.Padding.Top);
            }
        }

        private Image ScaleImage(Image originalImage)
        {
            float dpiScaleFactorX = 1;
            float dpiScaleFactorY = 1;
            if (DpiParent != null
                && (DpiParent.ScaleFactorRatioX != float.PositiveInfinity && DpiParent.ScaleFactorRatioX != float.NegativeInfinity)
                && (DpiParent.ScaleFactorRatioY != float.PositiveInfinity && DpiParent.ScaleFactorRatioY != float.NegativeInfinity))
            {
                dpiScaleFactorX = DpiParent.ScaleFactorRatioX;
                dpiScaleFactorY = DpiParent.ScaleFactorRatioY;
            }

            if (dpiScaleFactorX > 1 || dpiScaleFactorY > 1)
            {
                double ratio = Math.Min(dpiScaleFactorX, dpiScaleFactorY);

                int newWidth = (int)(originalImage.Width * ratio);
                int newHeight = (int)(originalImage.Height * ratio);
                Image newImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                return newImage;
            }
            else
            {
                return originalImage;
            }
        }

        #endregion

    }
}
