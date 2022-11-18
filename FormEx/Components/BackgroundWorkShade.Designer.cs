
namespace System.Windows.Forms
{
	partial class BackgroundWorkShade
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
		protected void InitializeComponent()
		{
            this.pnlCenterBox = new System.Windows.Forms.CustomPanel();
            this.pbWorkProgress = new System.Windows.Forms.CustomProgressBar();
            this.btnClose = new System.Windows.Forms.CustomButton();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.bgwJob = new System.ComponentModel.AbortableBackgroundWorker();
            this.pnlCenterBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCenterBox
            // 
            this.pnlCenterBox.BackColor = System.Drawing.Color.Transparent;
            this.pnlCenterBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlCenterBox.BorderWidth = 1;
            this.pnlCenterBox.Controls.Add(this.pbWorkProgress);
            this.pnlCenterBox.Controls.Add(this.btnClose);
            this.pnlCenterBox.Controls.Add(this.lblProgressText);
            this.pnlCenterBox.FirstColor = System.Drawing.Color.White;
            this.pnlCenterBox.Location = new System.Drawing.Point(220, 103);
            this.pnlCenterBox.Name = "pnlCenterBox";
            this.pnlCenterBox.RoundBorderRadius = 20;
            this.pnlCenterBox.SecondColor = System.Drawing.Color.White;
            this.pnlCenterBox.Size = new System.Drawing.Size(324, 168);
            this.pnlCenterBox.TabIndex = 2;
            // 
            // pbWorkProgress
            // 
            this.pbWorkProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pbWorkProgress.BackColor = System.Drawing.Color.Transparent;
            this.pbWorkProgress.BorderColor = System.Drawing.Color.Empty;
            this.pbWorkProgress.Location = new System.Drawing.Point(23, 71);
            this.pbWorkProgress.MaxValue = 100F;
            this.pbWorkProgress.MinValue = 0F;
            this.pbWorkProgress.Name = "pbWorkProgress";
            this.pbWorkProgress.Padding = new System.Windows.Forms.Padding(5);
            this.pbWorkProgress.ProgressBackColor = System.Drawing.Color.MidnightBlue;
            this.pbWorkProgress.ProgressBarColor = System.Drawing.Color.RoyalBlue;
            this.pbWorkProgress.Size = new System.Drawing.Size(279, 23);
            this.pbWorkProgress.TabIndex = 0;
            this.pbWorkProgress.Value = 0F;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnClose.CornerRadius = 16;
            this.btnClose.GradientMode = true;
            this.btnClose.Location = new System.Drawing.Point(110, 113);
            this.btnClose.Name = "btnClose";
            this.btnClose.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnClose.ShadeMode = false;
            this.btnClose.Size = new System.Drawing.Size(105, 32);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "取消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblProgressText
            // 
            this.lblProgressText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressText.AutoEllipsis = true;
            this.lblProgressText.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProgressText.Location = new System.Drawing.Point(23, 29);
            this.lblProgressText.Name = "lblProgressText";
            this.lblProgressText.Size = new System.Drawing.Size(279, 39);
            this.lblProgressText.TabIndex = 1;
            this.lblProgressText.Text = "正在运行, 请稍等...";
            this.lblProgressText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bgwJob
            // 
            this.bgwJob.WorkerReportsProgress = true;
            this.bgwJob.WorkerSupportsCancellation = true;
            this.bgwJob.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwJob_DoWork);
            this.bgwJob.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwJob_ProgressChanged);
            this.bgwJob.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwJob_RunWorkerCompleted);
            // 
            // BackgroundWorkShade
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(764, 374);
            this.Controls.Add(this.pnlCenterBox);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "BackgroundWorkShade";
            this.Resize += new System.EventHandler(this.WorkShade_Resize);
            this.pnlCenterBox.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
        private CustomPanel pnlCenterBox;
        private Label lblProgressText;
        private System.ComponentModel.AbortableBackgroundWorker bgwJob;
        private CustomButton btnClose;
        private CustomProgressBar pbWorkProgress;
    }
}
