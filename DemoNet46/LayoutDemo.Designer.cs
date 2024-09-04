
namespace DemoNet46
{
    partial class LayoutDemo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutDemo));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.CustomGroupIcon();
            this.btnHome = new System.Windows.Forms.CustomGroupIcon();
            this.btnFavorite = new System.Windows.Forms.CustomGroupIcon();
            this.pnlHeader = new System.Windows.Forms.ClickThroughPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSwitchSidebar = new System.Windows.Forms.ImageButton();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.ContentSwitcher = new System.Windows.Forms.StealthTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pnlSidebar.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSwitchSidebar)).BeginInit();
            this.pnlContent.SuspendLayout();
            this.ContentSwitcher.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.pnlSidebar.Controls.Add(this.btnHelp);
            this.pnlSidebar.Controls.Add(this.btnHome);
            this.pnlSidebar.Controls.Add(this.btnFavorite);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(2, 48);
            this.pnlSidebar.MaximumSize = new System.Drawing.Size(200, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(200, 477);
            this.pnlSidebar.TabIndex = 1006;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.btnHelp.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.Color.White;
            this.btnHelp.GroupName = "test";
            this.btnHelp.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(123)))), ((int)(((byte)(152)))));
            this.btnHelp.IconText = "Help";
            this.btnHelp.Image = global::DemoNet46.Properties.Resources.help;
            this.btnHelp.ImageSize = 32;
            this.btnHelp.IsSelected = false;
            this.btnHelp.KeepSelected = true;
            this.btnHelp.Location = new System.Drawing.Point(1, 242);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Padding = new System.Windows.Forms.Padding(18, 8, 8, 8);
            this.btnHelp.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(123)))), ((int)(((byte)(152)))));
            this.btnHelp.SelectionStyle = System.Windows.Forms.CustomGroupIcon.SelectionStyles.AccentBar;
            this.btnHelp.ShowIconBorder = false;
            this.btnHelp.Size = new System.Drawing.Size(198, 40);
            this.btnHelp.TabIndex = 1001;
            this.btnHelp.Text = "Help";
            this.btnHelp.WrapText = false;
            this.btnHelp.SingleClick += new System.Windows.Forms.MouseEventHandler(this.btnHelp_SingleClick);
            // 
            // btnHome
            // 
            this.btnHome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.btnHome.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.GroupName = "test";
            this.btnHome.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(123)))), ((int)(((byte)(152)))));
            this.btnHome.IconText = "Home";
            this.btnHome.Image = global::DemoNet46.Properties.Resources.home;
            this.btnHome.ImageSize = 32;
            this.btnHome.IsSelected = false;
            this.btnHome.KeepSelected = true;
            this.btnHome.Location = new System.Drawing.Point(1, 162);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.Name = "btnHome";
            this.btnHome.Padding = new System.Windows.Forms.Padding(18, 8, 8, 8);
            this.btnHome.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(123)))), ((int)(((byte)(152)))));
            this.btnHome.SelectionStyle = System.Windows.Forms.CustomGroupIcon.SelectionStyles.AccentBar;
            this.btnHome.ShowIconBorder = false;
            this.btnHome.Size = new System.Drawing.Size(198, 40);
            this.btnHome.TabIndex = 1005;
            this.btnHome.Text = "Home";
            this.btnHome.WrapText = false;
            this.btnHome.SingleClick += new System.Windows.Forms.MouseEventHandler(this.btnHome_SingleClick);
            // 
            // btnFavorite
            // 
            this.btnFavorite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFavorite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(68)))));
            this.btnFavorite.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFavorite.ForeColor = System.Drawing.Color.White;
            this.btnFavorite.GroupName = "test";
            this.btnFavorite.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(123)))), ((int)(((byte)(152)))));
            this.btnFavorite.IconText = "Favorite";
            this.btnFavorite.Image = global::DemoNet46.Properties.Resources.love;
            this.btnFavorite.ImageSize = 32;
            this.btnFavorite.IsSelected = false;
            this.btnFavorite.KeepSelected = true;
            this.btnFavorite.Location = new System.Drawing.Point(1, 202);
            this.btnFavorite.Margin = new System.Windows.Forms.Padding(0);
            this.btnFavorite.Name = "btnFavorite";
            this.btnFavorite.Padding = new System.Windows.Forms.Padding(18, 8, 8, 8);
            this.btnFavorite.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(123)))), ((int)(((byte)(152)))));
            this.btnFavorite.SelectionStyle = System.Windows.Forms.CustomGroupIcon.SelectionStyles.AccentBar;
            this.btnFavorite.ShowIconBorder = false;
            this.btnFavorite.Size = new System.Drawing.Size(198, 40);
            this.btnFavorite.TabIndex = 1004;
            this.btnFavorite.Text = "Favorite";
            this.btnFavorite.WrapText = false;
            this.btnFavorite.SingleClick += new System.Windows.Forms.MouseEventHandler(this.btnFavorite_SingleClick);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(77)))), ((int)(((byte)(105)))));
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.btnSwitchSidebar);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(2, 2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(859, 46);
            this.pnlHeader.TabIndex = 1007;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(42, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 24);
            this.label1.TabIndex = 1008;
            this.label1.Text = "Menu";
            // 
            // btnSwitchSidebar
            // 
            this.btnSwitchSidebar.BackColor = System.Drawing.Color.Transparent;
            this.btnSwitchSidebar.Bms = System.Windows.Forms.ImageButton.ButtonMouseStatus.None;
            this.btnSwitchSidebar.ButtonKeepPressed = false;
            this.btnSwitchSidebar.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSwitchSidebar.HotTrackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(243)))));
            this.btnSwitchSidebar.Location = new System.Drawing.Point(12, 10);
            this.btnSwitchSidebar.Name = "btnSwitchSidebar";
            this.btnSwitchSidebar.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSwitchSidebar.NormalImage")));
            this.btnSwitchSidebar.ShowFocusLine = false;
            this.btnSwitchSidebar.Size = new System.Drawing.Size(24, 24);
            this.btnSwitchSidebar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnSwitchSidebar.TabIndex = 1007;
            this.btnSwitchSidebar.Text = "imageButton1";
            this.btnSwitchSidebar.ToolTipText = null;
            this.btnSwitchSidebar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnSwitchSidebar_MouseClick);
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(17)))), ((int)(((byte)(40)))));
            this.pnlContent.Controls.Add(this.ContentSwitcher);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(202, 48);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(659, 477);
            this.pnlContent.TabIndex = 1011;
            // 
            // ContentSwitcher
            // 
            this.ContentSwitcher.Controls.Add(this.tabPage1);
            this.ContentSwitcher.Controls.Add(this.tabPage2);
            this.ContentSwitcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentSwitcher.Location = new System.Drawing.Point(0, 0);
            this.ContentSwitcher.Multiline = true;
            this.ContentSwitcher.Name = "ContentSwitcher";
            this.ContentSwitcher.SelectedIndex = 0;
            this.ContentSwitcher.Size = new System.Drawing.Size(659, 477);
            this.ContentSwitcher.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(651, 451);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(651, 451);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LayoutDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(77)))), ((int)(((byte)(105)))));
            this.BorderColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(863, 527);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Controls.Add(this.pnlHeader);
            this.DesigntimeScaleFactorX = 1F;
            this.DesigntimeScaleFactorY = 1F;
            this.ForeColor = System.Drawing.Color.White;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(3840, 2160);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "LayoutDemo";
            this.Resizable = true;
            this.RuntimeScaleFactorX = 1F;
            this.RuntimeScaleFactorY = 1F;
            this.ShowCaptionShadow = true;
            this.Text = "部分功能测试";
            this.TitleBarHeight = 0;
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.pnlSidebar, 0);
            this.Controls.SetChildIndex(this.pnlContent, 0);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSwitchSidebar)).EndInit();
            this.pnlContent.ResumeLayout(false);
            this.ContentSwitcher.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CustomGroupIcon btnFavorite;
        private System.Windows.Forms.CustomGroupIcon btnHome;
        private System.Windows.Forms.CustomGroupIcon btnHelp;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.ClickThroughPanel pnlHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageButton btnSwitchSidebar;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.StealthTabControl ContentSwitcher;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}