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
            this.btnRoundForm = new System.Windows.Forms.CustomButton();
            this.SuspendLayout();
            // 
            // btnWorkShade
            // 
            this.btnWorkShade.BackColor = System.Drawing.Color.CadetBlue;
            this.btnWorkShade.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnWorkShade.CornerRadius = 8;
            this.btnWorkShade.ForeColor = System.Drawing.Color.White;
            this.btnWorkShade.GradientMode = true;
            this.btnWorkShade.Location = new System.Drawing.Point(30, 96);
            this.btnWorkShade.Name = "btnWorkShade";
            this.btnWorkShade.ShadeMode = false;
            this.btnWorkShade.Size = new System.Drawing.Size(122, 39);
            this.btnWorkShade.TabIndex = 0;
            this.btnWorkShade.Text = "遮盖层";
            this.btnWorkShade.Click += new System.EventHandler(this.btnWorkShade_Click);
            // 
            // btnShowException
            // 
            this.btnShowException.BackColor = System.Drawing.Color.OrangeRed;
            this.btnShowException.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnShowException.CornerRadius = 8;
            this.btnShowException.ForeColor = System.Drawing.Color.White;
            this.btnShowException.GradientMode = true;
            this.btnShowException.Location = new System.Drawing.Point(30, 46);
            this.btnShowException.Name = "btnShowException";
            this.btnShowException.ShadeMode = true;
            this.btnShowException.Size = new System.Drawing.Size(122, 39);
            this.btnShowException.TabIndex = 0;
            this.btnShowException.Text = "异常消息窗体";
            this.btnShowException.Click += new System.EventHandler(this.btnShowException_Click);
            // 
            // btnBackgroundWorkShade
            // 
            this.btnBackgroundWorkShade.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnBackgroundWorkShade.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnBackgroundWorkShade.CornerRadius = 8;
            this.btnBackgroundWorkShade.ForeColor = System.Drawing.Color.White;
            this.btnBackgroundWorkShade.GradientMode = true;
            this.btnBackgroundWorkShade.Location = new System.Drawing.Point(30, 145);
            this.btnBackgroundWorkShade.Name = "btnBackgroundWorkShade";
            this.btnBackgroundWorkShade.ShadeMode = true;
            this.btnBackgroundWorkShade.Size = new System.Drawing.Size(122, 39);
            this.btnBackgroundWorkShade.TabIndex = 0;
            this.btnBackgroundWorkShade.Text = "带后台的遮盖层";
            this.btnBackgroundWorkShade.Click += new System.EventHandler(this.btnBackgroundWorkShade_Click);
            // 
            // btnRoundForm
            // 
            this.btnRoundForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnRoundForm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnRoundForm.CornerRadius = 16;
            this.btnRoundForm.GradientMode = false;
            this.btnRoundForm.Location = new System.Drawing.Point(169, 46);
            this.btnRoundForm.Name = "btnRoundForm";
            this.btnRoundForm.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnRoundForm.ShadeMode = false;
            this.btnRoundForm.Size = new System.Drawing.Size(122, 39);
            this.btnRoundForm.TabIndex = 1000;
            this.btnRoundForm.Text = "圆角窗体";
            this.btnRoundForm.Click += new System.EventHandler(this.btnRoundForm_Click);
            // 
            // ComponentDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRoundForm);
            this.Controls.Add(this.btnBackgroundWorkShade);
            this.Controls.Add(this.btnWorkShade);
            this.Controls.Add(this.btnShowException);
            this.Name = "ComponentDemo";
            this.Text = "ComponentDemo";
            this.Controls.SetChildIndex(this.btnShowException, 0);
            this.Controls.SetChildIndex(this.btnWorkShade, 0);
            this.Controls.SetChildIndex(this.btnBackgroundWorkShade, 0);
            this.Controls.SetChildIndex(this.btnRoundForm, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CustomButton btnWorkShade;
        private System.Windows.Forms.CustomButton btnShowException;
        private System.Windows.Forms.CustomButton btnBackgroundWorkShade;
        private System.Windows.Forms.CustomButton btnRoundForm;
    }
}