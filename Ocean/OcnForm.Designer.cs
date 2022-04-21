namespace Ocean
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OcnForm));
            this.win11ControlBox1 = new System.Windows.Forms.Win11ControlBox();
            this.SuspendLayout();
            // 
            // win11ControlBox1
            // 
            this.win11ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.win11ControlBox1.CloseButtonDownImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.CloseButtonDownImage")));
            this.win11ControlBox1.CloseButtonHoverImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.CloseButtonHoverImage")));
            this.win11ControlBox1.CloseButtonNormalImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.CloseButtonNormalImage")));
            this.win11ControlBox1.DrawByParent = false;
            this.win11ControlBox1.Location = new System.Drawing.Point(630, 1);
            this.win11ControlBox1.Margin = new System.Windows.Forms.Padding(0);
            this.win11ControlBox1.MaxButtonDownImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.MaxButtonDownImage")));
            this.win11ControlBox1.MaxButtonHoverImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.MaxButtonHoverImage")));
            this.win11ControlBox1.MaxButtonNormalImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.MaxButtonNormalImage")));
            this.win11ControlBox1.MinButtonDownImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.MinButtonDownImage")));
            this.win11ControlBox1.MinButtonHoverImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.MinButtonHoverImage")));
            this.win11ControlBox1.MinButtonNormalImage = ((System.Drawing.Image)(resources.GetObject("win11ControlBox1.MinButtonNormalImage")));
            this.win11ControlBox1.Name = "win11ControlBox1";
            this.win11ControlBox1.ParentForm = null;
            this.win11ControlBox1.Size = new System.Drawing.Size(150, 30);
            this.win11ControlBox1.TabIndex = 1;
            // 
            // OcnForm
            // 
            this.AllowResize = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackGradientDarkColor = System.Drawing.Color.White;
            this.BackGradientLightColor = System.Drawing.Color.White;
            this.BorderSize = 1;
            this.ClientSize = new System.Drawing.Size(782, 450);
            this.Controls.Add(this.win11ControlBox1);
            this.Name = "OcnForm";
            this.RoundCornerDiameter = 28;
            this.ShowFormShadow = true;
            this.Text = "OceanForm";
            this.TitleBarHeight = 32;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Win11ControlBox win11ControlBox1;
    }
}