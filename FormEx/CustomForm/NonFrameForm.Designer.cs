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
            this.FormBorderImage = new System.Windows.Forms.ImageList(this.components);
            this.win7ControlBox1 = new System.Windows.Forms.Win7ControlBox();
            this.SuspendLayout();
            // 
            // FormBorderImage
            // 
            this.FormBorderImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FormBorderImage.ImageStream")));
            this.FormBorderImage.TransparentColor = System.Drawing.Color.Transparent;
            this.FormBorderImage.Images.SetKeyName(0, "fringe_bkg.png");
            // 
            // win7ControlBox1
            // 
            this.win7ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.win7ControlBox1.AutoSize = true;
            this.win7ControlBox1.Location = new System.Drawing.Point(635, 0);
            this.win7ControlBox1.Margin = new System.Windows.Forms.Padding(2);
            this.win7ControlBox1.Name = "win7ControlBox1";
            this.win7ControlBox1.Size = new System.Drawing.Size(97, 21);
            this.win7ControlBox1.TabIndex = 0;
            // 
            // NonFrameForm
            // 
            this.ClientSize = new System.Drawing.Size(733, 261);
            this.Controls.Add(this.win7ControlBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NonFrameForm";
            this.SizeChanged += new System.EventHandler(this.NonFrameForm_SizeChanged);
            this.Resize += new System.EventHandler(this.NonFrameForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private ImageList FormBorderImage;
        private Win7ControlBox win7ControlBox1;
    }
}