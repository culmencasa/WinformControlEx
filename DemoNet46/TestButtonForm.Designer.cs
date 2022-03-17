
namespace DemoNet46
{
    partial class TestButtonForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnShowException = new System.Windows.Forms.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customButton1 = new System.Windows.Forms.CustomButton();
            this.btnWorkShade = new System.Windows.Forms.CustomButton();
            this.btnFavorite = new System.Windows.Forms.CustomGroupIcon();
            this.btnHome = new System.Windows.Forms.CustomGroupIcon();
            this.btnHelp = new System.Windows.Forms.CustomGroupIcon();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.ClickThroughPanel();
            this.win7ControlBox1 = new System.Windows.Forms.CustomForm.Win7ControlBox();
            this.groupBox1.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowException
            // 
            this.btnShowException.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnShowException.CornerRadius = 8;
            this.btnShowException.GradientMode = false;
            this.btnShowException.Location = new System.Drawing.Point(16, 20);
            this.btnShowException.Name = "btnShowException";
            this.btnShowException.ShadeMode = false;
            this.btnShowException.Size = new System.Drawing.Size(65, 26);
            this.btnShowException.TabIndex = 0;
            this.btnShowException.Text = "异常窗体";
            this.btnShowException.Click += new System.EventHandler(this.btnShowException_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.customButton1);
            this.groupBox1.Controls.Add(this.btnWorkShade);
            this.groupBox1.Controls.Add(this.btnShowException);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(397, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(234, 131);
            this.groupBox1.TabIndex = 1000;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // customButton1
            // 
            this.customButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.customButton1.CornerRadius = 8;
            this.customButton1.GradientMode = false;
            this.customButton1.Location = new System.Drawing.Point(16, 85);
            this.customButton1.Name = "customButton1";
            this.customButton1.ShadeMode = false;
            this.customButton1.Size = new System.Drawing.Size(65, 23);
            this.customButton1.TabIndex = 1;
            this.customButton1.Text = "customButton1";
            // 
            // btnWorkShade
            // 
            this.btnWorkShade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnWorkShade.CornerRadius = 8;
            this.btnWorkShade.GradientMode = false;
            this.btnWorkShade.Location = new System.Drawing.Point(16, 52);
            this.btnWorkShade.Name = "btnWorkShade";
            this.btnWorkShade.ShadeMode = false;
            this.btnWorkShade.Size = new System.Drawing.Size(65, 26);
            this.btnWorkShade.TabIndex = 0;
            this.btnWorkShade.Text = "遮盖层";
            this.btnWorkShade.Click += new System.EventHandler(this.btnWorkShade_Click);
            // 
            // btnFavorite
            // 
            this.btnFavorite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFavorite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.btnFavorite.DefaultImage = null;
            this.btnFavorite.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFavorite.ForeColor = System.Drawing.Color.White;
            this.btnFavorite.GroupName = "test";
            this.btnFavorite.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(92)))), ((int)(((byte)(143)))));
            this.btnFavorite.IconText = "Favorite";
            this.btnFavorite.Image = global::DemoNet46.Properties.Resources.love;
            this.btnFavorite.IsSelected = false;
            this.btnFavorite.KeepSelected = true;
            this.btnFavorite.Location = new System.Drawing.Point(1, 202);
            this.btnFavorite.Margin = new System.Windows.Forms.Padding(0);
            this.btnFavorite.Name = "btnFavorite";
            this.btnFavorite.Padding = new System.Windows.Forms.Padding(18, 8, 8, 8);
            this.btnFavorite.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(202)))), ((int)(((byte)(176)))));
            this.btnFavorite.ShowIconBorder = false;
            this.btnFavorite.Size = new System.Drawing.Size(200, 40);
            this.btnFavorite.TabIndex = 1004;
            this.btnFavorite.WrapText = false;
            // 
            // btnHome
            // 
            this.btnHome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.btnHome.DefaultImage = null;
            this.btnHome.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.GroupName = "test";
            this.btnHome.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(92)))), ((int)(((byte)(143)))));
            this.btnHome.IconText = "Home";
            this.btnHome.Image = global::DemoNet46.Properties.Resources.home;
            this.btnHome.IsSelected = false;
            this.btnHome.KeepSelected = true;
            this.btnHome.Location = new System.Drawing.Point(1, 162);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.Name = "btnHome";
            this.btnHome.Padding = new System.Windows.Forms.Padding(18, 8, 8, 8);
            this.btnHome.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(202)))), ((int)(((byte)(176)))));
            this.btnHome.ShowIconBorder = false;
            this.btnHome.Size = new System.Drawing.Size(200, 40);
            this.btnHome.TabIndex = 1005;
            this.btnHome.WrapText = false;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.btnHelp.DefaultImage = null;
            this.btnHelp.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.Color.White;
            this.btnHelp.GroupName = "test";
            this.btnHelp.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(92)))), ((int)(((byte)(143)))));
            this.btnHelp.IconText = "Help";
            this.btnHelp.Image = global::DemoNet46.Properties.Resources.help;
            this.btnHelp.IsSelected = false;
            this.btnHelp.KeepSelected = true;
            this.btnHelp.Location = new System.Drawing.Point(1, 242);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Padding = new System.Windows.Forms.Padding(18, 8, 8, 8);
            this.btnHelp.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(202)))), ((int)(((byte)(176)))));
            this.btnHelp.ShowIconBorder = false;
            this.btnHelp.Size = new System.Drawing.Size(200, 40);
            this.btnHelp.TabIndex = 1001;
            this.btnHelp.WrapText = false;
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.pnlSidebar.Controls.Add(this.btnHelp);
            this.pnlSidebar.Controls.Add(this.btnHome);
            this.pnlSidebar.Controls.Add(this.btnFavorite);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 69);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(200, 445);
            this.pnlSidebar.TabIndex = 1006;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(77)))), ((int)(((byte)(105)))));
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(860, 69);
            this.pnlHeader.TabIndex = 1007;
            // 
            // win7ControlBox1
            // 
            this.win7ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.win7ControlBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(77)))), ((int)(((byte)(105)))));
            this.win7ControlBox1.Location = new System.Drawing.Point(765, 0);
            this.win7ControlBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.win7ControlBox1.Name = "win7ControlBox1";
            this.win7ControlBox1.Size = new System.Drawing.Size(97, 22);
            this.win7ControlBox1.TabIndex = 0;
            // 
            // TestButtonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.BorderColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(860, 514);
            this.Controls.Add(this.win7ControlBox1);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlHeader);
            this.ForeColor = System.Drawing.Color.White;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(3840, 2160);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "TestButtonForm";
            this.Resizable = true;
            this.ShowCaptionShadow = true;
            this.Text = "部分功能测试";
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.pnlSidebar, 0);
            this.Controls.SetChildIndex(this.win7ControlBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CustomButton btnShowException;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CustomButton btnWorkShade;
        private System.Windows.Forms.CustomButton customButton1;
        private System.Windows.Forms.CustomGroupIcon btnFavorite;
        private System.Windows.Forms.CustomGroupIcon btnHome;
        private System.Windows.Forms.CustomGroupIcon btnHelp;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.ClickThroughPanel pnlHeader;
        private System.Windows.Forms.CustomForm.Win7ControlBox win7ControlBox1;
    }
}