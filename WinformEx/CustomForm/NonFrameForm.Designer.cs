using System.Windows.Forms;
namespace System.Windows.Forms
{
    partial class NonFrameForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NonFrameForm));
            this.flpControlBox = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.btnMaximum = new System.Windows.Forms.ImageButton();
            this.btnMinimum = new System.Windows.Forms.ImageButton();
            this.FormBorderImage = new System.Windows.Forms.ImageList(this.components);
            this.flpControlBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimum)).BeginInit();
            this.SuspendLayout();
            // 
            // flpControlBox
            // 
            this.flpControlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpControlBox.AutoSize = true;
            this.flpControlBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpControlBox.BackColor = System.Drawing.Color.Transparent;
            this.flpControlBox.Controls.Add(this.btnClose);
            this.flpControlBox.Controls.Add(this.btnMaximum);
            this.flpControlBox.Controls.Add(this.btnMinimum);
            this.flpControlBox.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpControlBox.Location = new System.Drawing.Point(636, 0);
            this.flpControlBox.Name = "flpControlBox";
            this.flpControlBox.Size = new System.Drawing.Size(95, 20);
            this.flpControlBox.TabIndex = 999;
            this.flpControlBox.WrapContents = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnClose.DownImage = ((System.Drawing.Image)(resources.GetObject("btnClose.DownImage")));
            this.btnClose.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnClose.HoverImage")));
            this.btnClose.Location = new System.Drawing.Point(56, 0);
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
            this.btnMaximum.Location = new System.Drawing.Point(28, 0);
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
            this.btnMinimum.Location = new System.Drawing.Point(0, 0);
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
            // FormBorderImage
            // 
            this.FormBorderImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FormBorderImage.ImageStream")));
            this.FormBorderImage.TransparentColor = System.Drawing.Color.Transparent;
            this.FormBorderImage.Images.SetKeyName(0, "fringe_bkg.png");
            // 
            // NonFrameForm
            // 
            this.ClientSize = new System.Drawing.Size(733, 261);
            this.Controls.Add(this.flpControlBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NonFrameForm";
            this.Resize += new System.EventHandler(this.NoneBorderForm_Resize);
            this.flpControlBox.ResumeLayout(false);
            this.flpControlBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion



        protected ImageButton btnMinimum;
        protected ImageButton btnMaximum;
        protected ImageButton btnClose;
        protected FlowLayoutPanel flpControlBox;
        private ImageList FormBorderImage;
    }
}