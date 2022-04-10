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
    public partial class Win11ControlBox : UserControl
    {
        public Win11ControlBox()
        {
            InitializeComponent();


        }


        protected override void OnResize(EventArgs eventargs)
        {

            //btnClose.Dock = DockStyle.None;
            //btnMaximum.Dock = DockStyle.None;
            //btnMinimum.Dock = DockStyle.None;


            base.OnResize(eventargs);

            this.Size = new Size(btnClose.Width + btnMaximum.Width + btnMinimum.Width, this.Height);
        }

        


        public Form ParentForm { get; set; }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!DesignMode)
            {
                ParentForm = FormManager.GetTopForm(this) as Form;
                if (ParentForm != null)
                {
                    this.BackColor = ParentForm.BackColor;
                    btnClose.Click -= new EventHandler(this.btnClose_Click);
                    btnMaximum.Click -= new EventHandler(this.btnMaximum_Click);
                    btnMinimum.Click -= new EventHandler(this.btnMinimum_Click);
                    btnClose.Click += new EventHandler(this.btnClose_Click);
                    btnMaximum.Click += new EventHandler(this.btnMaximum_Click);
                    btnMinimum.Click += new EventHandler(this.btnMinimum_Click);

                    ParentForm.StyleChanged -= ParentForm_StyleChanged;
                    ParentForm.StyleChanged += ParentForm_StyleChanged;
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

        //protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        //{
        //    Console.WriteLine(factor.ToString());
        //    base.ScaleControl(new SizeF(0.5f,0.5f), specified);
        //}




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

    }
}
