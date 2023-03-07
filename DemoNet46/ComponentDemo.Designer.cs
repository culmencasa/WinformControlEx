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
            this.colorGroupBox1 = new System.Windows.Forms.ColorGroupBox();
            this.circularProgressBar1 = new System.Windows.Forms.CircularProgressBar();
            this.roundButton1 = new System.Windows.Forms.RoundButton();
            this.colorGroupBox2 = new System.Windows.Forms.ColorGroupBox();
            this.btnShowInfoTip = new System.Windows.Forms.CustomButton();
            this.colorGroupBox1.SuspendLayout();
            this.colorGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnWorkShade
            // 
            this.btnWorkShade.BackColor = System.Drawing.Color.CadetBlue;
            this.btnWorkShade.BorderColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnWorkShade.CornerRadius = 8;
            this.btnWorkShade.ForeColor = System.Drawing.Color.White;
            this.btnWorkShade.GradientMode = true;
            this.btnWorkShade.Location = new System.Drawing.Point(23, 81);
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
            this.btnShowException.Location = new System.Drawing.Point(23, 31);
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
            this.btnBackgroundWorkShade.Location = new System.Drawing.Point(23, 130);
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
            this.btnRoundForm.Location = new System.Drawing.Point(162, 31);
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
            // colorGroupBox1
            // 
            this.colorGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.colorGroupBox1.BorderSize = 1;
            this.colorGroupBox1.BoxBorderColor = System.Drawing.Color.Gray;
            this.colorGroupBox1.BoxColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.colorGroupBox1.CaptionBackColor = System.Drawing.SystemColors.Control;
            this.colorGroupBox1.CaptionPosition = System.Windows.Forms.ColorGroupBox.CaptionPositions.None;
            this.colorGroupBox1.Controls.Add(this.btnRoundForm);
            this.colorGroupBox1.Controls.Add(this.btnShowException);
            this.colorGroupBox1.Controls.Add(this.btnWorkShade);
            this.colorGroupBox1.Controls.Add(this.btnBackgroundWorkShade);
            this.colorGroupBox1.LineColor = System.Drawing.Color.DimGray;
            this.colorGroupBox1.Location = new System.Drawing.Point(6, 27);
            this.colorGroupBox1.Name = "colorGroupBox1";
            this.colorGroupBox1.Show3DShadow = false;
            this.colorGroupBox1.Size = new System.Drawing.Size(308, 225);
            this.colorGroupBox1.TabIndex = 1002;
            this.colorGroupBox1.Text = "窗体相关";
            // 
            // circularProgressBar1
            // 
            this.circularProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar1.ForeColor = System.Drawing.Color.DimGray;
            this.circularProgressBar1.Location = new System.Drawing.Point(19, 18);
            this.circularProgressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.circularProgressBar1.Maximum = 100F;
            this.circularProgressBar1.Name = "circularProgressBar1";
            this.circularProgressBar1.ProgressBackColor = System.Drawing.Color.DimGray;
            this.circularProgressBar1.ProgressBeginColor = System.Drawing.Color.RoyalBlue;
            this.circularProgressBar1.ProgressEndColor = System.Drawing.Color.DarkSlateBlue;
            this.circularProgressBar1.ProgressValueColor = System.Drawing.Color.DodgerBlue;
            this.circularProgressBar1.ProgressWidth = 16F;
            this.circularProgressBar1.Size = new System.Drawing.Size(167, 167);
            this.circularProgressBar1.TabIndex = 1003;
            this.circularProgressBar1.TextColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.circularProgressBar1.TextColor2 = System.Drawing.Color.Black;
            this.circularProgressBar1.TextColor3 = System.Drawing.Color.DimGray;
            this.circularProgressBar1.TextFont1 = new System.Drawing.Font("Arial", 14F);
            this.circularProgressBar1.TextFont2 = new System.Drawing.Font("Arial", 20F);
            this.circularProgressBar1.TextFont3 = new System.Drawing.Font("Arial", 14F);
            this.circularProgressBar1.TextLine1 = "怒气";
            this.circularProgressBar1.TextLine2 = "100";
            this.circularProgressBar1.TextLine3 = "%";
            this.circularProgressBar1.Value = 100F;
            // 
            // roundButton1
            // 
            this.roundButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundButton1.ButtonColor = System.Drawing.Color.Maroon;
            this.roundButton1.Diameter = 54;
            this.roundButton1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.roundButton1.ForeColor = System.Drawing.Color.White;
            this.roundButton1.ImageEnter = null;
            this.roundButton1.ImageNormal = null;
            this.roundButton1.Location = new System.Drawing.Point(268, 18);
            this.roundButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.roundButton1.Name = "roundButton1";
            this.roundButton1.OutlineColor = System.Drawing.Color.IndianRed;
            this.roundButton1.OutlineHoverColor = System.Drawing.Color.Red;
            this.roundButton1.OutlineWidth = 4;
            this.roundButton1.Size = new System.Drawing.Size(54, 54);
            this.roundButton1.TabIndex = 1004;
            this.roundButton1.Text = "99";
            // 
            // colorGroupBox2
            // 
            this.colorGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.colorGroupBox2.BorderSize = 2;
            this.colorGroupBox2.BoxBorderColor = System.Drawing.Color.Silver;
            this.colorGroupBox2.BoxColor = System.Drawing.Color.Gainsboro;
            this.colorGroupBox2.CaptionBackColor = System.Drawing.SystemColors.Control;
            this.colorGroupBox2.CaptionPosition = System.Windows.Forms.ColorGroupBox.CaptionPositions.None;
            this.colorGroupBox2.Controls.Add(this.btnShowInfoTip);
            this.colorGroupBox2.Controls.Add(this.circularProgressBar1);
            this.colorGroupBox2.Controls.Add(this.roundButton1);
            this.colorGroupBox2.Location = new System.Drawing.Point(314, 27);
            this.colorGroupBox2.Name = "colorGroupBox2";
            this.colorGroupBox2.Show3DShadow = false;
            this.colorGroupBox2.Size = new System.Drawing.Size(343, 225);
            this.colorGroupBox2.TabIndex = 1005;
            this.colorGroupBox2.Text = "colorGroupBox2";
            // 
            // btnShowInfoTip
            // 
            this.btnShowInfoTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnShowInfoTip.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnShowInfoTip.CornerRadius = 16;
            this.btnShowInfoTip.GradientMode = false;
            this.btnShowInfoTip.Location = new System.Drawing.Point(41, 168);
            this.btnShowInfoTip.Name = "btnShowInfoTip";
            this.btnShowInfoTip.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnShowInfoTip.ShadeMode = false;
            this.btnShowInfoTip.Size = new System.Drawing.Size(122, 39);
            this.btnShowInfoTip.TabIndex = 1000;
            this.btnShowInfoTip.Text = "显示提示";
            this.btnShowInfoTip.Click += new System.EventHandler(this.btnShowInfoTip_Click);
            // 
            // ComponentDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.colorGroupBox2);
            this.Controls.Add(this.colorGroupBox1);
            this.MaximumSize = new System.Drawing.Size(2240, 1400);
            this.Name = "ComponentDemo";
            this.Text = "ComponentDemo";
            this.Controls.SetChildIndex(this.colorGroupBox1, 0);
            this.Controls.SetChildIndex(this.colorGroupBox2, 0);
            this.colorGroupBox1.ResumeLayout(false);
            this.colorGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CustomButton btnWorkShade;
        private System.Windows.Forms.CustomButton btnShowException;
        private System.Windows.Forms.CustomButton btnBackgroundWorkShade;
        private System.Windows.Forms.CustomButton btnRoundForm;
        private System.Windows.Forms.ColorGroupBox colorGroupBox1;
        private System.Windows.Forms.CircularProgressBar circularProgressBar1;
        private System.Windows.Forms.RoundButton roundButton1;
        private System.Windows.Forms.ColorGroupBox colorGroupBox2;
        private System.Windows.Forms.CustomButton btnShowInfoTip;
    }
}