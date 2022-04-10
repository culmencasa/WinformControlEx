namespace DemoNet46
{
    partial class ComponentDemo
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
            this.btnWorkShade = new System.Windows.Forms.CustomButton();
            this.btnShowException = new System.Windows.Forms.CustomButton();
            this.btnBackgroundWorkShade = new System.Windows.Forms.CustomButton();
            this.SuspendLayout();
            // 
            // btnWorkShade
            // 
            this.btnWorkShade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnWorkShade.CornerRadius = 8;
            this.btnWorkShade.GradientMode = false;
            this.btnWorkShade.Location = new System.Drawing.Point(31, 69);
            this.btnWorkShade.Name = "btnWorkShade";
            this.btnWorkShade.ShadeMode = false;
            this.btnWorkShade.Size = new System.Drawing.Size(122, 26);
            this.btnWorkShade.TabIndex = 0;
            this.btnWorkShade.Text = "遮盖层";
            this.btnWorkShade.Click += new System.EventHandler(this.btnWorkShade_Click);
            // 
            // btnShowException
            // 
            this.btnShowException.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnShowException.CornerRadius = 8;
            this.btnShowException.GradientMode = false;
            this.btnShowException.Location = new System.Drawing.Point(31, 27);
            this.btnShowException.Name = "btnShowException";
            this.btnShowException.ShadeMode = false;
            this.btnShowException.Size = new System.Drawing.Size(122, 26);
            this.btnShowException.TabIndex = 0;
            this.btnShowException.Text = "异常消息窗体";
            this.btnShowException.Click += new System.EventHandler(this.btnShowException_Click);
            // 
            // btnBackgroundWorkShade
            // 
            this.btnBackgroundWorkShade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnBackgroundWorkShade.CornerRadius = 8;
            this.btnBackgroundWorkShade.GradientMode = false;
            this.btnBackgroundWorkShade.Location = new System.Drawing.Point(31, 110);
            this.btnBackgroundWorkShade.Name = "btnBackgroundWorkShade";
            this.btnBackgroundWorkShade.ShadeMode = false;
            this.btnBackgroundWorkShade.Size = new System.Drawing.Size(122, 26);
            this.btnBackgroundWorkShade.TabIndex = 0;
            this.btnBackgroundWorkShade.Text = "带后台的遮盖层";
            this.btnBackgroundWorkShade.Click += new System.EventHandler(this.btnBackgroundWorkShade_Click);
            // 
            // ComponentDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnBackgroundWorkShade);
            this.Controls.Add(this.btnWorkShade);
            this.Controls.Add(this.btnShowException);
            this.Name = "ComponentDemo";
            this.Text = "ComponentDemo";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CustomButton btnWorkShade;
        private System.Windows.Forms.CustomButton btnShowException;
        private System.Windows.Forms.CustomButton btnBackgroundWorkShade;
    }
}