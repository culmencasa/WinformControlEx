
namespace System.Windows.Forms
{
    partial class Win7ControlBox
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
            this.flpControlBox = new System.Windows.Forms.ClickThroughFlowPanel();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.btnMaximum = new System.Windows.Forms.ImageButton();
            this.btnMinimum = new System.Windows.Forms.ImageButton();
            this.flpControlBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimum)).BeginInit();
            this.SuspendLayout();
            // 
            // flpControlBox
            // 
            this.flpControlBox.BackColor = System.Drawing.Color.Transparent;
            this.flpControlBox.Controls.Add(this.btnClose);
            this.flpControlBox.Controls.Add(this.btnMaximum);
            this.flpControlBox.Controls.Add(this.btnMinimum);
            this.flpControlBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpControlBox.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpControlBox.Location = new System.Drawing.Point(0, 0);
            this.flpControlBox.Margin = new System.Windows.Forms.Padding(2);
            this.flpControlBox.Name = "flpControlBox";
            this.flpControlBox.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.flpControlBox.Size = new System.Drawing.Size(96, 21);
            this.flpControlBox.TabIndex = 1001;
            this.flpControlBox.WrapContents = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.ButtonKeepPressed = false;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnClose.DownImage = global::System.Windows.Forms.Properties.Resources.btnClose_DownImage;
            this.btnClose.HoverImage = global::System.Windows.Forms.Properties.Resources.btnClose_HoverImage;
            this.btnClose.Location = new System.Drawing.Point(55, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.NormalImage = global::System.Windows.Forms.Properties.Resources.btnClose_NormalImage;
            this.btnClose.ShowFocusLine = false;
            this.btnClose.Size = new System.Drawing.Size(39, 20);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            this.btnClose.ToolTipText = "关闭";
            // 
            // btnMaximum
            // 
            this.btnMaximum.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximum.ButtonKeepPressed = false;
            this.btnMaximum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnMaximum.DownImage = global::System.Windows.Forms.Properties.Resources.btnMaximum_DownImage;
            this.btnMaximum.HoverImage = global::System.Windows.Forms.Properties.Resources.btnMaximum_HoverImage;
            this.btnMaximum.Location = new System.Drawing.Point(27, 0);
            this.btnMaximum.Margin = new System.Windows.Forms.Padding(0);
            this.btnMaximum.Name = "btnMaximum";
            this.btnMaximum.NormalImage = global::System.Windows.Forms.Properties.Resources.btnMaximum_NormalImage;
            this.btnMaximum.ShowFocusLine = false;
            this.btnMaximum.Size = new System.Drawing.Size(28, 20);
            this.btnMaximum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnMaximum.TabIndex = 1;
            this.btnMaximum.TabStop = false;
            this.btnMaximum.ToolTipText = "最大化";
            // 
            // btnMinimum
            // 
            this.btnMinimum.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimum.ButtonKeepPressed = false;
            this.btnMinimum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnMinimum.DownImage = global::System.Windows.Forms.Properties.Resources.btnMinimum_DownImage;
            this.btnMinimum.HoverImage = global::System.Windows.Forms.Properties.Resources.btnMinimum_HoverImage;
            this.btnMinimum.Location = new System.Drawing.Point(-1, 0);
            this.btnMinimum.Margin = new System.Windows.Forms.Padding(0);
            this.btnMinimum.Name = "btnMinimum";
            this.btnMinimum.NormalImage = global::System.Windows.Forms.Properties.Resources.btnMinimum_NormalImage;
            this.btnMinimum.ShowFocusLine = false;
            this.btnMinimum.Size = new System.Drawing.Size(28, 20);
            this.btnMinimum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnMinimum.TabIndex = 0;
            this.btnMinimum.TabStop = false;
            this.btnMinimum.ToolTipText = "最小化";
            // 
            // Win7ControlBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.Controls.Add(this.flpControlBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Win7ControlBox";
            this.Size = new System.Drawing.Size(96, 21);
            this.flpControlBox.ResumeLayout(false);
            this.flpControlBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected ClickThroughFlowPanel flpControlBox;
        protected ImageButton btnClose;
        protected ImageButton btnMaximum;
        protected ImageButton btnMinimum;
    }
}
