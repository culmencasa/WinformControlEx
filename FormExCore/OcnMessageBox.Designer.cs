using System.Windows.Forms;

namespace FormExCore
{
    partial class OcnMessageBox
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
            this.pnlTextArea = new System.Windows.Forms.ClickThroughPanel();
            this.lblContent = new System.Windows.Forms.Label();
            this.btnClose = new FormExCore.OcnSvgButton();
            this.pnlIconArea = new System.Windows.Forms.ClickThroughPanel();
            this.pbExceptionIcon = new System.Windows.Forms.PictureBox();
            this.pnlStrechSpace = new System.Windows.Forms.ClickThroughPanel();
            this.pnlControlArea = new System.Windows.Forms.ClickThroughFlowPanel();
            this.btnCancel = new FormExCore.OcnButton();
            this.btnNo = new FormExCore.OcnButton();
            this.btnYes = new FormExCore.OcnButton();
            this.btnOK = new FormExCore.OcnButton();
            this.boxActionSeparator = new System.Windows.Forms.Separator();
            this.pnlLockSpace = new System.Windows.Forms.ClickThroughPanel();
            this.TitleBar = new System.Windows.Forms.ClickThroughPanel();
            this.lblCaption = new System.Windows.Forms.Label();
            this.pnlTextArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.pnlIconArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptionIcon)).BeginInit();
            this.pnlStrechSpace.SuspendLayout();
            this.pnlControlArea.SuspendLayout();
            this.pnlLockSpace.SuspendLayout();
            this.TitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTextArea
            // 
            this.pnlTextArea.Controls.Add(this.lblContent);
            this.pnlTextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTextArea.Location = new System.Drawing.Point(78, 0);
            this.pnlTextArea.Name = "pnlTextArea";
            this.pnlTextArea.Padding = new System.Windows.Forms.Padding(10, 25, 30, 30);
            this.pnlTextArea.Size = new System.Drawing.Size(299, 99);
            this.pnlTextArea.TabIndex = 0;
            // 
            // lblContent
            // 
            this.lblContent.AutoEllipsis = true;
            this.lblContent.BackColor = System.Drawing.Color.Transparent;
            this.lblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblContent.Location = new System.Drawing.Point(10, 25);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(259, 44);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "这是一条消息 这是一条消息 这是一条消息 这是一条消息 这是一条消息 ";
            this.lblContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblContent.UseCompatibleTextRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverBackColor = System.Drawing.Color.Empty;
            this.btnClose.HoverColor = System.Drawing.Color.Empty;
            this.btnClose.Location = new System.Drawing.Point(341, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.NormalColor = System.Drawing.Color.White;
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.SourceName = "shut";
            this.btnClose.SourcePath = "";
            this.btnClose.TabIndex = 5;
            this.btnClose.UseSourcePath = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlIconArea
            // 
            this.pnlIconArea.Controls.Add(this.pbExceptionIcon);
            this.pnlIconArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIconArea.Location = new System.Drawing.Point(0, 0);
            this.pnlIconArea.Name = "pnlIconArea";
            this.pnlIconArea.Padding = new System.Windows.Forms.Padding(20, 20, 10, 20);
            this.pnlIconArea.Size = new System.Drawing.Size(78, 99);
            this.pnlIconArea.TabIndex = 0;
            // 
            // pbExceptionIcon
            // 
            this.pbExceptionIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbExceptionIcon.Location = new System.Drawing.Point(20, 20);
            this.pbExceptionIcon.Name = "pbExceptionIcon";
            this.pbExceptionIcon.Size = new System.Drawing.Size(48, 49);
            this.pbExceptionIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbExceptionIcon.TabIndex = 0;
            this.pbExceptionIcon.TabStop = false;
            // 
            // pnlStrechSpace
            // 
            this.pnlStrechSpace.BackColor = System.Drawing.Color.Transparent;
            this.pnlStrechSpace.Controls.Add(this.pnlControlArea);
            this.pnlStrechSpace.Controls.Add(this.boxActionSeparator);
            this.pnlStrechSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStrechSpace.Location = new System.Drawing.Point(0, 139);
            this.pnlStrechSpace.Name = "pnlStrechSpace";
            this.pnlStrechSpace.Size = new System.Drawing.Size(377, 102);
            this.pnlStrechSpace.TabIndex = 2;
            // 
            // pnlControlArea
            // 
            this.pnlControlArea.Controls.Add(this.btnCancel);
            this.pnlControlArea.Controls.Add(this.btnNo);
            this.pnlControlArea.Controls.Add(this.btnYes);
            this.pnlControlArea.Controls.Add(this.btnOK);
            this.pnlControlArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControlArea.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlControlArea.Location = new System.Drawing.Point(0, 10);
            this.pnlControlArea.Name = "pnlControlArea";
            this.pnlControlArea.Padding = new System.Windows.Forms.Padding(6, 3, 7, 0);
            this.pnlControlArea.Size = new System.Drawing.Size(377, 88);
            this.pnlControlArea.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnCancel.CornerRadius = 8;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnCancel.GradientMode = false;
            this.btnCancel.IsOutline = true;
            this.btnCancel.Location = new System.Drawing.Point(264, 6);
            this.btnCancel.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(17)))), ((int)(((byte)(150)))));
            this.btnCancel.MouseOverForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnCancel.ShadeMode = false;
            this.btnCancel.Size = new System.Drawing.Size(97, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.Theme = FormExCore.OcnThemes.Primary;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.White;
            this.btnNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnNo.CornerRadius = 8;
            this.btnNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnNo.GradientMode = false;
            this.btnNo.IsOutline = true;
            this.btnNo.Location = new System.Drawing.Point(161, 6);
            this.btnNo.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(17)))), ((int)(((byte)(150)))));
            this.btnNo.MouseOverForeColor = System.Drawing.Color.White;
            this.btnNo.Name = "btnNo";
            this.btnNo.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnNo.ShadeMode = false;
            this.btnNo.Size = new System.Drawing.Size(97, 35);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "否";
            this.btnNo.Theme = FormExCore.OcnThemes.Primary;
            this.btnNo.Visible = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnYes.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnYes.CornerRadius = 8;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.GradientMode = false;
            this.btnYes.IsOutline = false;
            this.btnYes.Location = new System.Drawing.Point(58, 6);
            this.btnYes.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(66)))), ((int)(((byte)(144)))));
            this.btnYes.MouseOverForeColor = System.Drawing.Color.White;
            this.btnYes.Name = "btnYes";
            this.btnYes.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnYes.ShadeMode = false;
            this.btnYes.Size = new System.Drawing.Size(97, 35);
            this.btnYes.TabIndex = 4;
            this.btnYes.Text = "是";
            this.btnYes.Theme = FormExCore.OcnThemes.Primary;
            this.btnYes.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnOK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnOK.CornerRadius = 8;
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.GradientMode = false;
            this.btnOK.IsOutline = false;
            this.btnOK.Location = new System.Drawing.Point(264, 47);
            this.btnOK.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(66)))), ((int)(((byte)(144)))));
            this.btnOK.MouseOverForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnOK.ShadeMode = false;
            this.btnOK.Size = new System.Drawing.Size(97, 35);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.Theme = FormExCore.OcnThemes.Primary;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // boxActionSeparator
            // 
            this.boxActionSeparator.BackColor = System.Drawing.Color.Transparent;
            this.boxActionSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.boxActionSeparator.Location = new System.Drawing.Point(0, 0);
            this.boxActionSeparator.Name = "boxActionSeparator";
            this.boxActionSeparator.Size = new System.Drawing.Size(377, 10);
            this.boxActionSeparator.TabIndex = 2;
            this.boxActionSeparator.Text = "separator1";
            // 
            // pnlLockSpace
            // 
            this.pnlLockSpace.Controls.Add(this.pnlTextArea);
            this.pnlLockSpace.Controls.Add(this.pnlIconArea);
            this.pnlLockSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLockSpace.Location = new System.Drawing.Point(0, 40);
            this.pnlLockSpace.Name = "pnlLockSpace";
            this.pnlLockSpace.Size = new System.Drawing.Size(377, 99);
            this.pnlLockSpace.TabIndex = 3;
            // 
            // TitleBar
            // 
            this.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(71)))), ((int)(((byte)(150)))));
            this.TitleBar.Controls.Add(this.lblCaption);
            this.TitleBar.Controls.Add(this.btnClose);
            this.TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleBar.Location = new System.Drawing.Point(0, 0);
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.Size = new System.Drawing.Size(377, 40);
            this.TitleBar.TabIndex = 4;
            // 
            // lblCaption
            // 
            this.lblCaption.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCaption.AutoSize = true;
            this.lblCaption.ForeColor = System.Drawing.Color.White;
            this.lblCaption.Location = new System.Drawing.Point(15, 13);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(56, 17);
            this.lblCaption.TabIndex = 6;
            this.lblCaption.Text = "消息标题";
            // 
            // OcnMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(377, 199);
            this.ControlBox = false;
            this.Controls.Add(this.pnlStrechSpace);
            this.Controls.Add(this.pnlLockSpace);
            this.Controls.Add(this.TitleBar);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2560, 1400);
            this.MinimizeBox = false;
            this.Name = "OcnMessageBox";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "消息";
            this.Load += new System.EventHandler(this.OcnMessageBox_Load);
            this.pnlTextArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.pnlIconArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptionIcon)).EndInit();
            this.pnlStrechSpace.ResumeLayout(false);
            this.pnlControlArea.ResumeLayout(false);
            this.pnlLockSpace.ResumeLayout(false);
            this.TitleBar.ResumeLayout(false);
            this.TitleBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ClickThroughPanel pnlTextArea;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.ClickThroughPanel pnlIconArea;
        private System.Windows.Forms.PictureBox pbExceptionIcon;
        private System.Windows.Forms.ClickThroughPanel pnlStrechSpace;
        private System.Windows.Forms.ClickThroughPanel pnlLockSpace;
        private OcnButton btnOK;
        private OcnSvgButton btnClose;
        private ClickThroughFlowPanel pnlControlArea;
        private Separator boxActionSeparator;
        private OcnButton btnCancel;
        private OcnButton btnNo;
        private OcnButton btnYes;
        private ClickThroughPanel TitleBar;
        private Label lblCaption;
    }
}