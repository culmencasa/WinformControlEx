
namespace System.Windows.Forms
{
    partial class InfoTip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoTip));
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.iconHolder = new System.Windows.Forms.Panel();
            this.wrapper = new System.Windows.Forms.Panel();
            this.pnlTextLayout = new System.Windows.Forms.Panel();
            this.lblText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.iconHolder.SuspendLayout();
            this.wrapper.SuspendLayout();
            this.pnlTextLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(162, 33);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title1";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblTitle.UseCompatibleTextRendering = true;
            this.lblTitle.UseMnemonic = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // iconHolder
            // 
            this.iconHolder.Controls.Add(this.pictureBox1);
            this.iconHolder.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconHolder.Location = new System.Drawing.Point(0, 0);
            this.iconHolder.Name = "iconHolder";
            this.iconHolder.Size = new System.Drawing.Size(35, 133);
            this.iconHolder.TabIndex = 2;
            // 
            // wrapper
            // 
            this.wrapper.Controls.Add(this.pnlTextLayout);
            this.wrapper.Controls.Add(this.iconHolder);
            this.wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wrapper.Location = new System.Drawing.Point(3, 1);
            this.wrapper.Name = "wrapper";
            this.wrapper.Size = new System.Drawing.Size(202, 133);
            this.wrapper.TabIndex = 3;
            // 
            // pnlTextLayout
            // 
            this.pnlTextLayout.Controls.Add(this.lblText);
            this.pnlTextLayout.Controls.Add(this.lblTitle);
            this.pnlTextLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTextLayout.Location = new System.Drawing.Point(35, 0);
            this.pnlTextLayout.Name = "pnlTextLayout";
            this.pnlTextLayout.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.pnlTextLayout.Size = new System.Drawing.Size(167, 133);
            this.pnlTextLayout.TabIndex = 3;
            // 
            // lblText
            // 
            this.lblText.AutoEllipsis = true;
            this.lblText.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblText.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblText.Location = new System.Drawing.Point(0, 33);
            this.lblText.Margin = new System.Windows.Forms.Padding(3);
            this.lblText.Name = "lblText";
            this.lblText.Padding = new System.Windows.Forms.Padding(0, 5, 5, 0);
            this.lblText.Size = new System.Drawing.Size(162, 93);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Content";
            // 
            // InfoTip
            // 
            this.AllowMove = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(208, 137);
            this.Controls.Add(this.wrapper);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoTip";
            this.RoundCornerDiameter = 8;
            this.ShowFormShadow = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.iconHolder.ResumeLayout(false);
            this.wrapper.ResumeLayout(false);
            this.pnlTextLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel iconHolder;
        private System.Windows.Forms.Panel wrapper;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Panel pnlTextLayout;
    }
}