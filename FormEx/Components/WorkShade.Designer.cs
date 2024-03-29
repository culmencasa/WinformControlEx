﻿
namespace System.Windows.Forms
{
	partial class WorkShade
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

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		protected virtual void InitializeComponent()
		{
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlCenterBox = new System.Windows.Forms.CustomPanel();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pnlCenterBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(98, 105);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(128, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "跳过";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlCenterBox
            // 
            this.pnlCenterBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCenterBox.BackColor = System.Drawing.Color.Transparent;
            this.pnlCenterBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlCenterBox.BorderWidth = 1;
            this.pnlCenterBox.Controls.Add(this.lblProgressText);
            this.pnlCenterBox.Controls.Add(this.btnClose);
            this.pnlCenterBox.Controls.Add(this.progressBar1);
            this.pnlCenterBox.FirstColor = System.Drawing.Color.White;
            this.pnlCenterBox.InnerBackColor = System.Drawing.Color.Transparent;
            this.pnlCenterBox.Location = new System.Drawing.Point(220, 103);
            this.pnlCenterBox.Name = "pnlCenterBox";
            this.pnlCenterBox.RoundBorderRadius = 20;
            this.pnlCenterBox.SecondColor = System.Drawing.Color.White;
            this.pnlCenterBox.Size = new System.Drawing.Size(324, 168);
            this.pnlCenterBox.TabIndex = 2;
            // 
            // lblProgressText
            // 
            this.lblProgressText.AutoSize = true;
            this.lblProgressText.Location = new System.Drawing.Point(103, 76);
            this.lblProgressText.Name = "lblProgressText";
            this.lblProgressText.Size = new System.Drawing.Size(119, 12);
            this.lblProgressText.TabIndex = 2;
            this.lblProgressText.Text = "正在运行, 请稍等...";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(40, 51);
            this.progressBar1.MarqueeAnimationSpeed = 1;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(245, 10);
            this.progressBar1.Step = 5;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            // 
            // WorkShade
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 374);
            this.Controls.Add(this.pnlCenterBox);
            this.Name = "WorkShade";
            this.Resize += new System.EventHandler(this.WorkShade_Resize);
            this.pnlCenterBox.ResumeLayout(false);
            this.pnlCenterBox.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnClose;
        private CustomPanel pnlCenterBox;
        private Label lblProgressText;
    }
}
