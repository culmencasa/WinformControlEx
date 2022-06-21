namespace FormExCore
{
    partial class OcnForm
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
            this.btnClose = new FormExCore.OcnSvgButton();
            this.btnMax = new FormExCore.OcnSvgButton();
            this.btnMin = new FormExCore.OcnSvgButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMin)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnClose.HoverColor = System.Drawing.SystemColors.Highlight;
            this.btnClose.Location = new System.Drawing.Point(650, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.NormalColor = System.Drawing.Color.DimGray;
            this.btnClose.Padding = new System.Windows.Forms.Padding(5);
            this.btnClose.Size = new System.Drawing.Size(54, 32);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.SourceName = "shut";
            this.btnClose.SourcePath = "";
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.UseSourcePath = false;
            this.btnClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseClick);
            // 
            // btnMax
            // 
            this.btnMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMax.BackColor = System.Drawing.Color.Transparent;
            this.btnMax.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnMax.HoverColor = System.Drawing.SystemColors.Highlight;
            this.btnMax.Location = new System.Drawing.Point(602, 0);
            this.btnMax.Name = "btnMax";
            this.btnMax.NormalColor = System.Drawing.Color.DimGray;
            this.btnMax.Padding = new System.Windows.Forms.Padding(5);
            this.btnMax.Size = new System.Drawing.Size(48, 32);
            this.btnMax.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMax.SourceName = "maximize";
            this.btnMax.SourcePath = "";
            this.btnMax.TabIndex = 1;
            this.btnMax.TabStop = false;
            this.btnMax.UseSourcePath = false;
            this.btnMax.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnMax_MouseClick);
            // 
            // btnMin
            // 
            this.btnMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMin.BackColor = System.Drawing.Color.Transparent;
            this.btnMin.HoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnMin.HoverColor = System.Drawing.SystemColors.Highlight;
            this.btnMin.Location = new System.Drawing.Point(554, 0);
            this.btnMin.Name = "btnMin";
            this.btnMin.NormalColor = System.Drawing.Color.DimGray;
            this.btnMin.Padding = new System.Windows.Forms.Padding(5);
            this.btnMin.Size = new System.Drawing.Size(48, 32);
            this.btnMin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnMin.SourceName = "minimize";
            this.btnMin.SourcePath = "";
            this.btnMin.TabIndex = 2;
            this.btnMin.TabStop = false;
            this.btnMin.UseSourcePath = false;
            this.btnMin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnMin_MouseClick);
            // 
            // OcnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(704, 450);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.btnMax);
            this.Controls.Add(this.btnClose);
            this.DoubleBuffered = true;
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "OcnForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OceanForm";
            this.Resize += new System.EventHandler(this.OcnForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OcnSvgButton btnClose;
        private OcnSvgButton btnMax;
        private OcnSvgButton btnMin;
    }
}