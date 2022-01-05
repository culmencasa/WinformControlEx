
namespace DemoNet46
{
    partial class TestButtonForm
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
            this.btnShowException = new System.Windows.Forms.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowException
            // 
            this.btnShowException.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnShowException.CornerRadius = 8;
            this.btnShowException.GradientMode = false;
            this.btnShowException.Location = new System.Drawing.Point(16, 20);
            this.btnShowException.Name = "btnShowException";
            this.btnShowException.ShadeMode = false;
            this.btnShowException.Size = new System.Drawing.Size(65, 26);
            this.btnShowException.TabIndex = 0;
            this.btnShowException.Text = "异常窗体";
            this.btnShowException.Click += new System.EventHandler(this.btnShowException_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnShowException);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 115);
            this.groupBox1.TabIndex = 1000;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // TestButtonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(415, 269);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "TestButtonForm";
            this.Resizable = true;
            this.Text = "TestButtonForm";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CustomButton btnShowException;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}