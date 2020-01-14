
namespace System.Windows.Forms
{
    partial class RoundedCornerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoundedCornerForm));
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
            this.flpControlBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpControlBox.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpControlBox.Location = new System.Drawing.Point(2, 0);
            this.flpControlBox.Name = "flpControlBox";
            this.flpControlBox.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flpControlBox.Size = new System.Drawing.Size(254, 20);
            this.flpControlBox.TabIndex = 1000;
            this.flpControlBox.WrapContents = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnClose.DownImage = ((System.Drawing.Image)(resources.GetObject("btnClose.DownImage")));
            this.btnClose.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnClose.HoverImage")));
            this.btnClose.Location = new System.Drawing.Point(212, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnClose.NormalImage")));
            this.btnClose.ShowFocusLine = false;
            this.btnClose.Size = new System.Drawing.Size(39, 20);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            this.btnClose.ToolTipText = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMaximum
            // 
            this.btnMaximum.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnMaximum.DownImage = ((System.Drawing.Image)(resources.GetObject("btnMaximum.DownImage")));
            this.btnMaximum.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnMaximum.HoverImage")));
            this.btnMaximum.Location = new System.Drawing.Point(184, 0);
            this.btnMaximum.Margin = new System.Windows.Forms.Padding(0);
            this.btnMaximum.Name = "btnMaximum";
            this.btnMaximum.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnMaximum.NormalImage")));
            this.btnMaximum.ShowFocusLine = false;
            this.btnMaximum.Size = new System.Drawing.Size(28, 20);
            this.btnMaximum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnMaximum.TabIndex = 1;
            this.btnMaximum.TabStop = false;
            this.btnMaximum.ToolTipText = "最大化";
            this.btnMaximum.Click += new System.EventHandler(this.btnMaximum_Click);
            // 
            // btnMinimum
            // 
            this.btnMinimum.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnMinimum.DownImage = ((System.Drawing.Image)(resources.GetObject("btnMinimum.DownImage")));
            this.btnMinimum.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnMinimum.HoverImage")));
            this.btnMinimum.Location = new System.Drawing.Point(156, 0);
            this.btnMinimum.Margin = new System.Windows.Forms.Padding(0);
            this.btnMinimum.Name = "btnMinimum";
            this.btnMinimum.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnMinimum.NormalImage")));
            this.btnMinimum.ShowFocusLine = false;
            this.btnMinimum.Size = new System.Drawing.Size(28, 20);
            this.btnMinimum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnMinimum.TabIndex = 0;
            this.btnMinimum.TabStop = false;
            this.btnMinimum.ToolTipText = "最小化";
            this.btnMinimum.Click += new System.EventHandler(this.btnMinimum_Click);
            // 
            // RoundedCornerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(258, 261);
            this.Controls.Add(this.flpControlBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RoundedCornerForm";
            this.Padding = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RoundForm";
            this.Load += new System.EventHandler(this.RoundedCornerForm_Load);
            this.flpControlBox.ResumeLayout(false);
            this.flpControlBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected ClickThroughFlowPanel flpControlBox;
        protected System.Windows.Forms.ImageButton btnClose;
        protected System.Windows.Forms.ImageButton btnMaximum;
        protected System.Windows.Forms.ImageButton btnMinimum;
    }
}