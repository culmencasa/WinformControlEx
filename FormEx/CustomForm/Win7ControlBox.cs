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
        public Win7ControlBox()
        {
            InitializeComponent();
            this.ParentChanged += Win7ControlBox_ParentChanged;
        }



        private void Win7ControlBox_ParentChanged(object sender, EventArgs e)
        {

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode)
            {
                Form parentForm = FormManager.GetTopForm(this) as Form;
                if (parentForm != null)
                {
                    this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
                    this.btnMaximum.Click -= new System.EventHandler(this.btnMaximum_Click);
                    this.btnMinimum.Click -= new System.EventHandler(this.btnMinimum_Click);
                    this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
                    this.btnMaximum.Click += new System.EventHandler(this.btnMaximum_Click);
                    this.btnMinimum.Click += new System.EventHandler(this.btnMinimum_Click);

                    parentForm.StyleChanged -= ParentForm_StyleChanged;
                    parentForm.StyleChanged += ParentForm_StyleChanged;

                    this.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                }
            }
        }


        private void ParentForm_StyleChanged(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
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
            form.WindowState = FormWindowState.Minimized;
        }

        // 点击最大化/还原按钮
        private void btnMaximum_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            if (form.WindowState == FormWindowState.Maximized)
            {
                form.WindowState = FormWindowState.Normal;
                this.btnMaximum.NormalImage = Properties.Resources.btnMaximum_NormalImage;
                this.btnMaximum.HoverImage = Properties.Resources.btnMaximum_HoverImage;
                this.btnMaximum.DownImage = Properties.Resources.btnMaximum_DownImage;
            }
            else
            {
                form.WindowState = FormWindowState.Maximized;
                this.btnMaximum.NormalImage = Properties.Resources.restore_normal;
                this.btnMaximum.HoverImage = Properties.Resources.restore_highlight;
                this.btnMaximum.DownImage = Properties.Resources.restore_down;
            }

        }

        // 点击关闭按钮
        private void btnClose_Click(object sender, EventArgs e)
        {
            Form form = this.ParentForm;
            form.Close();
        }

    }
}
