namespace Ocean
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OcnMessageBox));
            this.pnlTextArea = new System.Windows.Forms.Panel();
            this.lblContent = new System.Windows.Forms.Label();
            this.pnlIconArea = new System.Windows.Forms.Panel();
            this.pbExceptionIcon = new System.Windows.Forms.PictureBox();
            this.pnlStrechSpace = new System.Windows.Forms.Panel();
            this.pnlControlArea = new System.Windows.Forms.Panel();
            this.pnlLockSpace = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.ImageButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnOK = new Ocean.OcnButton();
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
            this.pnlTextArea.Size = new System.Drawing.Size(316, 111);
            this.pnlTextArea.TabIndex = 0;
            // 
            // lblContent
            // 
            this.lblContent.AutoEllipsis = true;
            this.lblContent.BackColor = System.Drawing.Color.IndianRed;
            this.lblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblContent.Location = new System.Drawing.Point(10, 25);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(276, 56);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "这是一条消息 ";
            this.lblContent.UseCompatibleTextRendering = true;
            // 
            // pnlIconArea
            // 
            this.pnlIconArea.Controls.Add(this.pbExceptionIcon);
            this.pnlIconArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIconArea.Location = new System.Drawing.Point(0, 0);
            this.pnlIconArea.Name = "pnlIconArea";
            this.pnlIconArea.Padding = new System.Windows.Forms.Padding(20, 20, 10, 20);
            this.pnlIconArea.Size = new System.Drawing.Size(78, 111);
            this.pnlIconArea.TabIndex = 0;
            // 
            // pbExceptionIcon
            // 
            this.pbExceptionIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbExceptionIcon.Location = new System.Drawing.Point(20, 20);
            this.pbExceptionIcon.Name = "pbExceptionIcon";
            this.pbExceptionIcon.Size = new System.Drawing.Size(48, 48);
            this.pbExceptionIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbExceptionIcon.TabIndex = 0;
            this.pbExceptionIcon.TabStop = false;
            // 
            // pnlStrechSpace
            // 
            this.pnlStrechSpace.BackColor = System.Drawing.SystemColors.Control;
            this.pnlStrechSpace.Controls.Add(this.pnlControlArea);
            this.pnlStrechSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStrechSpace.Location = new System.Drawing.Point(1, 168);
            this.pnlStrechSpace.Name = "pnlStrechSpace";
            this.pnlStrechSpace.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlStrechSpace.Size = new System.Drawing.Size(394, 53);
            this.pnlStrechSpace.TabIndex = 2;
            // 
            // pnlControlArea
            // 
            this.pnlControlArea.Controls.Add(this.btnOK);
            this.pnlControlArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControlArea.Location = new System.Drawing.Point(10, 0);
            this.pnlControlArea.Name = "pnlControlArea";
            this.pnlControlArea.Size = new System.Drawing.Size(374, 48);
            this.pnlControlArea.TabIndex = 102;
            // 
            // pnlLockSpace
            // 
            this.pnlLockSpace.Controls.Add(this.pnlTextArea);
            this.pnlLockSpace.Controls.Add(this.pnlIconArea);
            this.pnlLockSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLockSpace.Location = new System.Drawing.Point(1, 57);
            this.pnlLockSpace.Name = "pnlLockSpace";
            this.pnlLockSpace.Size = new System.Drawing.Size(394, 111);
            this.pnlLockSpace.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Bms = System.Windows.Forms.ImageButton.ButtonMouseStatus.None;
            this.btnClose.ButtonKeepPressed = false;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnClose.DownImage = ((System.Drawing.Image)(resources.GetObject("btnClose.DownImage")));
            this.btnClose.HotTrackColor = System.Drawing.Color.Empty;
            this.btnClose.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnClose.HoverImage")));
            this.btnClose.Location = new System.Drawing.Point(356, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnClose.NormalImage")));
            this.btnClose.ShowFocusLine = false;
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 4;
            this.btnClose.ToolTipText = null;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnOK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(82)))), ((int)(((byte)(179)))));
            this.btnOK.CornerRadius = 8;
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.GradientMode = false;
            this.btnOK.IsOutline = false;
            this.btnOK.Location = new System.Drawing.Point(270, 6);
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
            this.btnOK.Theme = Ocean.OcnButton.Themes.Primary;
            // 
            // OcnMessageBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BorderSize = 1;
            this.ClientSize = new System.Drawing.Size(396, 222);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlStrechSpace);
            this.Controls.Add(this.pnlLockSpace);
            this.LogoSize = 30;
            this.MaximumSize = new System.Drawing.Size(2560, 1400);
            this.Name = "OcnMessageBox";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.ShowLogo = false;
            this.ShowTitleCenter = true;
            this.Text = "消息";
            this.TitleBarHeight = 56;
            this.TitleFont = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TitleText = "消息";
            this.pnlTextArea.ResumeLayout(false);
            this.pnlIconArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptionIcon)).EndInit();
            this.pnlStrechSpace.ResumeLayout(false);
            this.pnlControlArea.ResumeLayout(false);
            this.pnlLockSpace.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTextArea;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Panel pnlIconArea;
        private System.Windows.Forms.PictureBox pbExceptionIcon;
        private System.Windows.Forms.Panel pnlStrechSpace;
        private System.Windows.Forms.Panel pnlControlArea;
        private System.Windows.Forms.Panel pnlLockSpace;
        private OcnButton btnOK;
        private System.Windows.Forms.ImageButton btnClose;
        private System.Windows.Forms.Timer timer1;
    }
}