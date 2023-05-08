
namespace System.Windows.Forms
{
    partial class CustomNavItem
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.RightPanel = new System.Windows.Forms.CustomPanel();
            this.SubheadlineLabel = new System.Windows.Forms.Label();
            this.HeadLineLabel = new System.Windows.Forms.Label();
            this.CaptionLabel = new System.Windows.Forms.Label();
            this.LeftPanel = new System.Windows.Forms.CustomPanel();
            this.NavIcon = new System.Windows.Forms.ImageButton();
            this.RightPanel.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NavIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // RightPanel
            // 
            this.RightPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RightPanel.BackColor = System.Drawing.Color.Transparent;
            this.RightPanel.BorderColor = System.Drawing.Color.Empty;
            this.RightPanel.BorderWidth = 1;
            this.RightPanel.Controls.Add(this.SubheadlineLabel);
            this.RightPanel.Controls.Add(this.HeadLineLabel);
            this.RightPanel.Controls.Add(this.CaptionLabel);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.FirstColor = System.Drawing.Color.Empty;
            this.RightPanel.Location = new System.Drawing.Point(38, 0);
            this.RightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightPanel.RoundBorderRadius = 0;
            this.RightPanel.SecondColor = System.Drawing.Color.Empty;
            this.RightPanel.Size = new System.Drawing.Size(203, 83);
            this.RightPanel.TabIndex = 4;
            this.RightPanel.MouseEnter += new System.EventHandler(this.NavigationItemControl_MouseEnter);
            this.RightPanel.MouseLeave += new System.EventHandler(this.NavigationItemControl_MouseLeave);
            // 
            // SubheadlineLabel
            // 
            this.SubheadlineLabel.AutoEllipsis = true;
            this.SubheadlineLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SubheadlineLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SubheadlineLabel.ForeColor = System.Drawing.Color.DimGray;
            this.SubheadlineLabel.Location = new System.Drawing.Point(4, 49);
            this.SubheadlineLabel.Name = "SubheadlineLabel";
            this.SubheadlineLabel.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.SubheadlineLabel.Size = new System.Drawing.Size(198, 26);
            this.SubheadlineLabel.TabIndex = 2;
            this.SubheadlineLabel.Visible = false;
            this.SubheadlineLabel.TextChanged += new System.EventHandler(this.SubheadlineLabel_TextChanged);
            this.SubheadlineLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NavigationItemControl_MouseClick);
            this.SubheadlineLabel.MouseEnter += new System.EventHandler(this.NavigationItemControl_MouseEnter);
            this.SubheadlineLabel.MouseLeave += new System.EventHandler(this.NavigationItemControl_MouseLeave);
            // 
            // HeadLineLabel
            // 
            this.HeadLineLabel.AutoEllipsis = true;
            this.HeadLineLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeadLineLabel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HeadLineLabel.Location = new System.Drawing.Point(4, 25);
            this.HeadLineLabel.Name = "HeadLineLabel";
            this.HeadLineLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.HeadLineLabel.Size = new System.Drawing.Size(198, 24);
            this.HeadLineLabel.TabIndex = 1;
            this.HeadLineLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.HeadLineLabel.TextChanged += new System.EventHandler(this.HeadLineLabel_TextChanged);
            this.HeadLineLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NavigationItemControl_MouseClick);
            this.HeadLineLabel.MouseEnter += new System.EventHandler(this.NavigationItemControl_MouseEnter);
            this.HeadLineLabel.MouseLeave += new System.EventHandler(this.NavigationItemControl_MouseLeave);
            // 
            // CaptionLabel
            // 
            this.CaptionLabel.AutoEllipsis = true;
            this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CaptionLabel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CaptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CaptionLabel.Location = new System.Drawing.Point(4, 1);
            this.CaptionLabel.Name = "CaptionLabel";
            this.CaptionLabel.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.CaptionLabel.Size = new System.Drawing.Size(198, 24);
            this.CaptionLabel.TabIndex = 0;
            this.CaptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CaptionLabel.Visible = false;
            this.CaptionLabel.TextChanged += new System.EventHandler(this.CaptionLabel_TextChanged);
            this.CaptionLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NavigationItemControl_MouseClick);
            this.CaptionLabel.MouseEnter += new System.EventHandler(this.NavigationItemControl_MouseEnter);
            this.CaptionLabel.MouseLeave += new System.EventHandler(this.NavigationItemControl_MouseLeave);
            // 
            // LeftPanel
            // 
            this.LeftPanel.AutoSize = true;
            this.LeftPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LeftPanel.BackColor = System.Drawing.Color.Transparent;
            this.LeftPanel.BorderColor = System.Drawing.Color.Empty;
            this.LeftPanel.BorderWidth = 1;
            this.LeftPanel.Controls.Add(this.NavIcon);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.FirstColor = System.Drawing.Color.Empty;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Margin = new System.Windows.Forms.Padding(0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.RoundBorderRadius = 0;
            this.LeftPanel.SecondColor = System.Drawing.Color.Empty;
            this.LeftPanel.Size = new System.Drawing.Size(38, 83);
            this.LeftPanel.TabIndex = 3;
            this.LeftPanel.MouseEnter += new System.EventHandler(this.NavigationItemControl_MouseEnter);
            this.LeftPanel.MouseLeave += new System.EventHandler(this.NavigationItemControl_MouseLeave);
            // 
            // NavIcon
            // 
            this.NavIcon.BackColor = System.Drawing.Color.Transparent;
            this.NavIcon.Bms = System.Windows.Forms.ImageButton.ButtonMouseStatus.None;
            this.NavIcon.ButtonKeepPressed = false;
            this.NavIcon.DialogResult = System.Windows.Forms.DialogResult.None;
            this.NavIcon.Location = new System.Drawing.Point(3, 4);
            this.NavIcon.Name = "NavIcon";
            this.NavIcon.ShowFocusLine = false;
            this.NavIcon.Size = new System.Drawing.Size(32, 32);
            this.NavIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.NavIcon.TabIndex = 0;
            this.NavIcon.ToolTipText = null;
            this.NavIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NavigationItemControl_MouseClick);
            this.NavIcon.MouseEnter += new System.EventHandler(this.NavigationItemControl_MouseEnter);
            this.NavIcon.MouseLeave += new System.EventHandler(this.NavigationItemControl_MouseLeave);
            // 
            // NavigationItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.MinimumSize = new System.Drawing.Size(150, 28);
            this.Name = "NavigationItemControl";
            this.Size = new System.Drawing.Size(241, 83);
            this.Load += new System.EventHandler(this.NavigationItemControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NavigationItemControl_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NavigationItemControl_MouseClick);
            this.MouseEnter += new System.EventHandler(this.NavigationItemControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.NavigationItemControl_MouseLeave);
            this.RightPanel.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NavIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label HeadLineLabel;
        private System.Windows.Forms.ImageButton NavIcon;
        private System.Windows.Forms.Label SubheadlineLabel;
        private System.Windows.Forms.Label CaptionLabel;
        private System.Windows.Forms.CustomPanel LeftPanel;
        private System.Windows.Forms.CustomPanel RightPanel;
    }
}
