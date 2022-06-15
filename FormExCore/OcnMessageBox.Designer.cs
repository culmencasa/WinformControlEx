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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OcnMessageBox));
            this.pnlTextArea = new System.Windows.Forms.Panel();
            this.lblContent = new System.Windows.Forms.Label();
            this.pnlIconArea = new System.Windows.Forms.Panel();
            this.pbExceptionIcon = new System.Windows.Forms.PictureBox();
            this.pnlStrechSpace = new System.Windows.Forms.Panel();
            this.pnlControlArea = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new FormExCore.OcnButton();
            this.btnNo = new FormExCore.OcnButton();
            this.btnYes = new FormExCore.OcnButton();
            this.btnOK = new FormExCore.OcnButton();
            this.boxActionSeparator = new System.Windows.Forms.Separator();
            this.pnlLockSpace = new System.Windows.Forms.Panel();
            this.btnClose = new FormExCore.OcnSvgButton();
            this.pnlTextArea.SuspendLayout();
            this.pnlIconArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptionIcon)).BeginInit();
            this.pnlStrechSpace.SuspendLayout();
            this.pnlControlArea.SuspendLayout();
            this.pnlLockSpace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTextArea
            // 
            this.pnlTextArea.Controls.Add(this.lblContent);
            this.pnlTextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTextArea.Location = new System.Drawing.Point(78, 0);
            this.pnlTextArea.Name = "pnlTextArea";
            this.pnlTextArea.Padding = new System.Windows.Forms.Padding(10, 25, 30, 30);
            this.pnlTextArea.Size = new System.Drawing.Size(302, 99);
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
            this.lblContent.Size = new System.Drawing.Size(262, 44);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "这是一条消息 这是一条消息 这是一条消息 这是一条消息 这是一条消息 ";
            this.lblContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblContent.UseCompatibleTextRendering = true;
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
            this.pnlStrechSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStrechSpace.Location = new System.Drawing.Point(8, 140);
            this.pnlStrechSpace.Name = "pnlStrechSpace";
            this.pnlStrechSpace.Size = new System.Drawing.Size(380, 54);
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
            this.pnlControlArea.Size = new System.Drawing.Size(380, 88);
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
            this.btnCancel.Location = new System.Drawing.Point(267, 6);
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
            this.btnNo.Location = new System.Drawing.Point(164, 6);
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
            this.btnYes.Location = new System.Drawing.Point(61, 6);
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
            this.btnOK.Location = new System.Drawing.Point(267, 47);
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
            this.boxActionSeparator.Size = new System.Drawing.Size(380, 10);
            this.boxActionSeparator.TabIndex = 2;
            this.boxActionSeparator.Text = "separator1";
            // 
            // pnlLockSpace
            // 
            this.pnlLockSpace.Controls.Add(this.pnlTextArea);
            this.pnlLockSpace.Controls.Add(this.pnlIconArea);
            this.pnlLockSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLockSpace.Location = new System.Drawing.Point(8, 41);
            this.pnlLockSpace.Name = "pnlLockSpace";
            this.pnlLockSpace.Size = new System.Drawing.Size(380, 99);
            this.pnlLockSpace.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.HoverColor = System.Drawing.Color.Empty;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(359, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.NormalColor = System.Drawing.Color.Empty;
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnClose.SourceName = "close_button";
            this.btnClose.SourcePath = "";
            this.btnClose.TabIndex = 5;
            this.btnClose.TabStop = false;
            this.btnClose.UseSourcePath = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // OcnMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(396, 202);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlStrechSpace);
            this.Controls.Add(this.pnlLockSpace);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            
            this.MaximumSize = new System.Drawing.Size(2560, 1400);
            this.Name = "OcnMessageBox";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Text = "消息";
            this.TitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(17)))), ((int)(((byte)(150)))));
            this.TitleBarHeight = 40;
            this.TitleText = "消息";
            this.Load += new System.EventHandler(this.OcnMessageBox_Load);
            this.Controls.SetChildIndex(this.pnlLockSpace, 0);
            this.Controls.SetChildIndex(this.pnlStrechSpace, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlTextArea.ResumeLayout(false);
            this.pnlIconArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptionIcon)).EndInit();
            this.pnlStrechSpace.ResumeLayout(false);
            this.pnlControlArea.ResumeLayout(false);
            this.pnlLockSpace.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTextArea;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Panel pnlIconArea;
        private System.Windows.Forms.PictureBox pbExceptionIcon;
        private System.Windows.Forms.Panel pnlStrechSpace;
        private System.Windows.Forms.Panel pnlLockSpace;
        private OcnButton btnOK;
        private OcnSvgButton btnClose;
        private FlowLayoutPanel pnlControlArea;
        private Separator boxActionSeparator;
        private OcnButton btnCancel;
        private OcnButton btnNo;
        private OcnButton btnYes;
    }
}