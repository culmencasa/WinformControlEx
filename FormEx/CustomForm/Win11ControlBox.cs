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
    /// <summary>
    /// 仿win11控制栏
    /// ......效果很糟糕, 就这样吧
    /// </summary>
    public partial class Win11ControlBox : ClickThroughPanel
    {
        public Win11ControlBox()
        {
            InitializeComponent();

            btnClose.NormalImageChanged += BtnClose_NormalImageChanged;
            btnMaximum.NormalImageChanged += BtnMaximum_NormalImageChanged;
            btnMinimum.NormalImageChanged += BtnMinimum_NormalImageChanged;
            btnClose.Resize += BtnClose_Resize;
            btnMaximum.Resize += BtnMaximum_Resize;
            btnMinimum.Resize += BtnMinimum_Resize;

            this.Size = new Size(150, 30);
        }

        #region 控件模式

        float closeButtonRate = 1;

        float maxButtonRate = 1;

        float minButtonRate = 1;
        /// <summary>
        /// 刷新按钮比例
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rate"></param>
        protected void RefreshButtonRate(ImageButton source, ref float rate)
        {
            var image = source.NormalImage;
            if (image == null)
            {
                rate = 1;
            }
            else if (rate == 1)
            {
                rate = image.Width * 1f / image.Height;
            }
        }

        /// <summary>
        /// 重置按钮大小
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rate"></param>
        protected void ResizeButton(ImageButton source, float rate)
        {
            int newHeight = this.Height;
            int newWidth = Convert.ToInt16(newHeight * rate);

            source.Width = newWidth;
            source.Height = newHeight;
        }

        /// <summary>
        /// 重置控件大小
        /// </summary>
        protected void ResizeControlBox()
        {
            this.Size = new Size((btnClose.Visible ? btnClose.Width : 0) + (btnMaximum.Visible ? btnMaximum.Width : 0) + (btnMinimum.Visible ? btnMinimum.Width : 0), this.Height);
        }


        private void BtnMinimum_NormalImageChanged(object sender, EventArgs e)
        {
            RefreshButtonRate(btnMinimum, ref minButtonRate);
        }

        private void BtnMaximum_NormalImageChanged(object sender, EventArgs e)
        {
            RefreshButtonRate(btnMaximum, ref maxButtonRate);
        }

        private void BtnClose_NormalImageChanged(object sender, EventArgs e)
        {
            RefreshButtonRate(btnClose, ref closeButtonRate);
        }


        private void BtnClose_Resize(object sender, EventArgs e)
        {
            if (IsHandleCreated && DesignMode)
            {
                ResizeButton(btnClose, closeButtonRate);
            }

        }
        private void BtnMinimum_Resize(object sender, EventArgs e)
        {
            if (IsHandleCreated && DesignMode)
            {
                ResizeButton(btnMinimum, minButtonRate);
            }
        }
        private void BtnMaximum_Resize(object sender, EventArgs e)
        {
            if (IsHandleCreated && DesignMode)
            {
                ResizeButton(btnMaximum, maxButtonRate);
            }
        }


        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

            if (DesignMode)
            {
                ResizeControlBox();
            }
        }


        public Form ParentForm { get; set; }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            RefreshButtonRate(btnClose, ref closeButtonRate);
            RefreshButtonRate(btnMaximum, ref maxButtonRate);
            RefreshButtonRate(btnMinimum, ref minButtonRate);
            ResizeButton(btnClose, closeButtonRate);
            ResizeButton(btnMinimum, minButtonRate);
            ResizeButton(btnMaximum, maxButtonRate);
            ResizeControlBox();

            if (!DesignMode)
            {
                ParentForm = FormManager.GetTopForm(this) as Form;
                if (ParentForm != null)
                {
                    btnClose.Click -= new EventHandler(this.btnClose_Click);
                    btnMaximum.Click -= new EventHandler(this.btnMaximum_Click);
                    btnMinimum.Click -= new EventHandler(this.btnMinimum_Click);
                    btnClose.Click += new EventHandler(this.btnClose_Click);
                    btnMaximum.Click += new EventHandler(this.btnMaximum_Click);
                    btnMinimum.Click += new EventHandler(this.btnMinimum_Click);

                    ParentForm.StyleChanged -= ParentForm_StyleChanged;
                    ParentForm.StyleChanged += ParentForm_StyleChanged;
                    
                    if (DrawByParent)
                    {
                        btnClose.Visible = false;
                        btnMaximum.Visible = false;
                        btnMinimum.Visible = false;
                        this.BackColor = Color.Transparent;
                        ParentForm.MouseDown += ParentForm_MouseDown;
                        ParentForm.Paint += ParentForm_Paint;
                    }
                }
            }
        }



        private void ParentForm_StyleChanged(object sender, EventArgs e)
        {
            Form form = ParentForm;
            if (form == null)
                return;

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


        // 点击最小化按钮 
        private void btnMinimum_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            if (form != null)
            {
                form.WindowState = FormWindowState.Minimized;
            }
        }

        // 点击最大化/还原按钮
        private void btnMaximum_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            if (form != null)
            {
                if (form.WindowState == FormWindowState.Maximized)
                {
                    form.WindowState = FormWindowState.Normal;
                    this.btnMaximum.NormalImage = Properties.Resources.max1;
                    this.btnMaximum.HoverImage = Properties.Resources.max2;
                    this.btnMaximum.DownImage = Properties.Resources.max3;
                }
                else
                {
                    form.WindowState = FormWindowState.Maximized;
                    this.btnMaximum.NormalImage = Properties.Resources.max4;
                    this.btnMaximum.HoverImage = Properties.Resources.max5;
                    this.btnMaximum.DownImage = Properties.Resources.max6;
                }
            }

        }

        // 点击关闭按钮
        private void btnClose_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            if (form != null)
            {
                form.Close();
            }
        }

        public Image CloseButtonNormalImage
        {
            get
            {
                return btnClose.Image;
            }
            set
            {
                btnClose.Image = value;
            }
        }
        public Image CloseButtonDownImage
        {
            get
            {
                return btnClose.DownImage;
            }
            set
            {
                btnClose.DownImage = value;
            }
        }
        public Image CloseButtonHoverImage
        {
            get
            {
                return btnClose.HoverImage;
            }
            set
            {
                btnClose.HoverImage = value;
            }
        }

        public Image MinButtonNormalImage
        {
            get
            {
                return btnMinimum.Image;
            }
            set
            {
                btnMinimum.Image = value;
            }
        }
        public Image MinButtonDownImage
        {
            get
            {
                return btnMinimum.DownImage;
            }
            set
            {
                btnMinimum.DownImage = value;
            }

        }
        public Image MinButtonHoverImage
        {
            get
            {
                return btnMinimum.HoverImage;
            }
            set
            {
                btnMinimum.HoverImage = value;
            }
        }

        public Image MaxButtonNormalImage
        {
            get
            {
                return btnMaximum.Image;
            }
            set
            {
                btnMaximum.Image = value;
            }
        }
        public Image MaxButtonDownImage
        {
            get
            { return btnMaximum.DownImage; }
            set
            {
                btnMaximum.DownImage = value;
            }
        }
        public Image MaxButtonHoverImage
        {
            get
            {
                return btnMaximum.HoverImage;
            }
            set
            {
                btnMaximum.HoverImage = value;
            }
        }

        #endregion

        #region 自绘模式 (未完成)

        [Browsable(false)]
        public bool DrawByParent { get; set; }

        private bool CloseButtonDown;
        private void ParentForm_MouseDown(object sender, MouseEventArgs e)
        {
            Point userPoint = e.Location;


            CloseButtonDown = false;
            if (CloseButtonBounds().Contains(userPoint))
            {
                CloseButtonDown = true;
            }

        }


        public Rectangle CloseButtonBounds()
        {
            return new Rectangle(ParentForm.Width - btnClose.Width, 1, btnClose.Width, btnClose.Height);
        }

        private void ParentForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Form form = ParentForm;

            if (CloseButtonDown)
            {
                g.DrawImage(CloseButtonDownImage, form.Width - btnClose.Width, 1);
            }
            else
            {
                g.DrawImage(CloseButtonNormalImage, form.Width - btnClose.Width, 1);
            }
            g.DrawImage(MaxButtonDownImage, form.Width - btnMaximum.Width - btnClose.Width, 1);
            g.DrawImage(MinButtonNormalImage, form.Width - btnMinimum.Width - btnMaximum.Width - btnClose.Width, 1);

        }


        #endregion

    }
}
