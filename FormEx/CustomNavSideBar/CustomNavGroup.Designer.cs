namespace System.Windows.Forms
{
    partial class CustomNavGroup
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
            this.GroupTextPanel = new System.Windows.Forms.Panel();
            this.GroupTextLabel = new System.Windows.Forms.CustomLabel();
            this.ItemListPanel = new System.Windows.Forms.NavigationContainer();
            this.GroupTextPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupTextPanel
            // 
            this.GroupTextPanel.Controls.Add(this.GroupTextLabel);
            this.GroupTextPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupTextPanel.Location = new System.Drawing.Point(0, 0);
            this.GroupTextPanel.Name = "GroupTextPanel";
            this.GroupTextPanel.Padding = new System.Windows.Forms.Padding(3);
            this.GroupTextPanel.Size = new System.Drawing.Size(254, 38);
            this.GroupTextPanel.TabIndex = 2;
            // 
            // GroupTextLabel
            // 
            this.GroupTextLabel.BorderColor = System.Drawing.Color.Empty;
            this.GroupTextLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.GroupTextLabel.FirstColor = System.Drawing.Color.Empty;
            this.GroupTextLabel.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GroupTextLabel.Location = new System.Drawing.Point(3, 3);
            this.GroupTextLabel.Name = "GroupTextLabel";
            this.GroupTextLabel.SecondColor = System.Drawing.Color.Empty;
            this.GroupTextLabel.Size = new System.Drawing.Size(220, 32);
            this.GroupTextLabel.TabIndex = 1;
            this.GroupTextLabel.Text = "组名";
            this.GroupTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ItemListPanel
            // 
            this.ItemListPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ItemListPanel.Location = new System.Drawing.Point(0, 38);
            this.ItemListPanel.MinimumSize = new System.Drawing.Size(0, 28);
            this.ItemListPanel.Name = "ItemListPanel";
            this.ItemListPanel.Size = new System.Drawing.Size(254, 28);
            this.ItemListPanel.TabIndex = 4;
            // 
            // NavigationGroupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.ItemListPanel);
            this.Controls.Add(this.GroupTextPanel);
            this.Name = "NavigationGroupControl";
            this.Size = new System.Drawing.Size(254, 201);
            this.GroupTextPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel GroupTextPanel;
        private System.Windows.Forms.CustomLabel GroupTextLabel;
        private NavigationContainer ItemListPanel;
    }
}
