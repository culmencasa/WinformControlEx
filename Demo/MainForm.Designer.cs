namespace Demo
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.separator1 = new System.Windows.Forms.Separator();
            this.gradientLabel1 = new System.Windows.Forms.GradientLabel();
            this.btnShowNoFrameForm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // separator1
            // 
            this.separator1.BackColor = System.Drawing.Color.Transparent;
            this.separator1.Location = new System.Drawing.Point(175, 104);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(302, 19);
            this.separator1.TabIndex = 0;
            this.separator1.Text = "separator1";
            // 
            // gradientLabel1
            // 
            this.gradientLabel1.AutoSize = true;
            this.gradientLabel1.BorderColor = System.Drawing.Color.Empty;
            this.gradientLabel1.FirstColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.gradientLabel1.GradientDirection = System.Drawing.FillDirection.LeftToRight;
            this.gradientLabel1.Location = new System.Drawing.Point(173, 60);
            this.gradientLabel1.Name = "gradientLabel1";
            this.gradientLabel1.SecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.gradientLabel1.Size = new System.Drawing.Size(53, 12);
            this.gradientLabel1.TabIndex = 1;
            this.gradientLabel1.Text = "渐变标签";
            // 
            // btnShowNoFrameForm
            // 
            this.btnShowNoFrameForm.Location = new System.Drawing.Point(449, 215);
            this.btnShowNoFrameForm.Name = "btnShowNoFrameForm";
            this.btnShowNoFrameForm.Size = new System.Drawing.Size(88, 27);
            this.btnShowNoFrameForm.TabIndex = 2;
            this.btnShowNoFrameForm.Text = "无边框窗体";
            this.btnShowNoFrameForm.UseVisualStyleBackColor = true;
            this.btnShowNoFrameForm.Click += new System.EventHandler(this.btnShowNoFrameForm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(175, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "splitter:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowNoFrameForm);
            this.Controls.Add(this.gradientLabel1);
            this.Controls.Add(this.separator1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Separator separator1;
        private System.Windows.Forms.GradientLabel gradientLabel1;
        private System.Windows.Forms.Button btnShowNoFrameForm;
        private System.Windows.Forms.Label label1;
    }
}

