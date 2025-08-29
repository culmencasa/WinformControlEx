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
			this.btnClose = new Button();
			this.btnMax = new Button();
			this.btnMin = new Button(); 
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.BackColor = System.Drawing.Color.Transparent;
			this.btnClose.Location = new System.Drawing.Point(1351, 0);
			this.btnClose.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.btnClose.Name = "btnClose";
			this.btnClose.Padding = new System.Windows.Forms.Padding(0);
			this.btnClose.Size = new System.Drawing.Size(64, 56); 
			this.btnClose.TabIndex = 0;
			this.btnClose.TabStop = false; 
			this.btnClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnClose_MouseClick);
			// 
			// btnMax
			// 
			this.btnMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMax.BackColor = System.Drawing.Color.Transparent;
			this.btnMax.Location = new System.Drawing.Point(1290, 0);
			this.btnMax.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.btnMax.Name = "btnMax"; 
			this.btnMax.Padding = new System.Windows.Forms.Padding(0);
			this.btnMax.Size = new System.Drawing.Size(64, 56); 
			this.btnMax.TabIndex = 1;
			this.btnMax.TabStop = false; 
			this.btnMax.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnMax_MouseClick);
			// 
			// btnMin
			// 
			this.btnMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMin.BackColor = System.Drawing.Color.Transparent;
 
			this.btnMin.Location = new System.Drawing.Point(1231, 0);
			this.btnMin.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.btnMin.Name = "btnMin"; 
			this.btnMin.Padding = new System.Windows.Forms.Padding(0);
			this.btnMin.Size = new System.Drawing.Size(64, 56);
 
			this.btnMin.TabIndex = 2;
			this.btnMin.TabStop = false; 
			this.btnMin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnMin_MouseClick);
			// 
			// OcnForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1408, 900);
			this.Controls.Add(this.btnMin);
			this.Controls.Add(this.btnMax);
			this.Controls.Add(this.btnClose);
			this.DoubleBuffered = true;
			this.Location = new System.Drawing.Point(0, 0);
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.Name = "OcnForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "OceanForm";
			this.Resize += new System.EventHandler(this.OcnForm_Resize); 
			this.ResumeLayout(false);

        }

        #endregion

        private Button btnClose;
        private Button btnMax;
        private Button btnMin;
    }
}