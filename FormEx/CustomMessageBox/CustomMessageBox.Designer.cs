namespace System.Windows.Forms
{
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class CustomMessageBox
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WareCheckerApp.Dialog", typeof(CustomMessageBox).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }


        #region 设计器生成的代码

        private void InitializeComponent()
        {
            this.pnlIconArea = new System.Windows.Forms.Panel();
            this.IconControl = new System.Windows.Forms.PictureBox();
            this.pnlTextArea = new System.Windows.Forms.Panel();
            this.lblContent = new System.Windows.Forms.Label();
            this.pnlLockSpace = new System.Windows.Forms.Panel();
            this.pnlControlArea = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlStrechSpace = new System.Windows.Forms.Panel();
            this.CaptionLabel = new System.Windows.Forms.Label();
            this.pnlIconArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconControl)).BeginInit();
            this.pnlTextArea.SuspendLayout();
            this.pnlLockSpace.SuspendLayout();
            this.pnlControlArea.SuspendLayout();
            this.pnlStrechSpace.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIconArea
            // 
            this.pnlIconArea.BackColor = System.Drawing.Color.Transparent;
            this.pnlIconArea.Controls.Add(this.IconControl);
            this.pnlIconArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIconArea.Location = new System.Drawing.Point(0, 0);
            this.pnlIconArea.Name = "pnlIconArea";
            this.pnlIconArea.Padding = new System.Windows.Forms.Padding(20, 20, 0, 20);
            this.pnlIconArea.Size = new System.Drawing.Size(72, 111);
            this.pnlIconArea.TabIndex = 0;
            // 
            // IconControl
            // 
            this.IconControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.IconControl.Location = new System.Drawing.Point(22, 20);
            this.IconControl.Name = "IconControl";
            this.IconControl.Size = new System.Drawing.Size(48, 48);
            this.IconControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.IconControl.TabIndex = 0;
            this.IconControl.TabStop = false;
            // 
            // pnlTextArea
            // 
            this.pnlTextArea.Controls.Add(this.lblContent);
            this.pnlTextArea.Controls.Add(this.CaptionLabel);
            this.pnlTextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTextArea.Location = new System.Drawing.Point(72, 0);
            this.pnlTextArea.Name = "pnlTextArea";
            this.pnlTextArea.Padding = new System.Windows.Forms.Padding(10, 20, 30, 30);
            this.pnlTextArea.Size = new System.Drawing.Size(278, 111);
            this.pnlTextArea.TabIndex = 0;
            // 
            // lblContent
            // 
            this.lblContent.AutoEllipsis = true;
            this.lblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblContent.Location = new System.Drawing.Point(10, 46);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(238, 35);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "这是一条提示消息 ";
            this.lblContent.UseCompatibleTextRendering = true;
            // 
            // pnlLockSpace
            // 
            this.pnlLockSpace.Controls.Add(this.pnlTextArea);
            this.pnlLockSpace.Controls.Add(this.pnlIconArea);
            this.pnlLockSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLockSpace.Location = new System.Drawing.Point(2, 42);
            this.pnlLockSpace.Name = "pnlLockSpace";
            this.pnlLockSpace.Size = new System.Drawing.Size(350, 111);
            this.pnlLockSpace.TabIndex = 1;
            // 
            // pnlControlArea
            // 
            this.pnlControlArea.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlControlArea.AutoSize = true;
            this.pnlControlArea.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlControlArea.Controls.Add(this.btnOK);
            this.pnlControlArea.Controls.Add(this.btnYes);
            this.pnlControlArea.Controls.Add(this.btnNo);
            this.pnlControlArea.Controls.Add(this.btnCancel);
            this.pnlControlArea.Location = new System.Drawing.Point(-1, 3);
            this.pnlControlArea.Name = "pnlControlArea";
            this.pnlControlArea.Size = new System.Drawing.Size(352, 38);
            this.pnlControlArea.TabIndex = 102;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.Location = new System.Drawing.Point(91, 3);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(82, 32);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "是";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.Location = new System.Drawing.Point(179, 3);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(82, 32);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "否";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(267, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 32);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlStrechSpace
            // 
            this.pnlStrechSpace.BackColor = System.Drawing.SystemColors.Control;
            this.pnlStrechSpace.Controls.Add(this.pnlControlArea);
            this.pnlStrechSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStrechSpace.Location = new System.Drawing.Point(2, 153);
            this.pnlStrechSpace.Name = "pnlStrechSpace";
            this.pnlStrechSpace.Padding = new System.Windows.Forms.Padding(10, 6, 10, 10);
            this.pnlStrechSpace.Size = new System.Drawing.Size(350, 45);
            this.pnlStrechSpace.TabIndex = 0;
            // 
            // CaptionLabel
            // 
            this.CaptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CaptionLabel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CaptionLabel.Location = new System.Drawing.Point(10, 20);
            this.CaptionLabel.Name = "CaptionLabel";
            this.CaptionLabel.Size = new System.Drawing.Size(238, 26);
            this.CaptionLabel.TabIndex = 1;
            this.CaptionLabel.Text = "标题";
            // 
            // CustomMessageBox
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(354, 200);
            this.Controls.Add(this.pnlStrechSpace);
            this.Controls.Add(this.pnlLockSpace);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FullScreen = false;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1040);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(335, 200);
            this.Name = "CustomMessageBox";
            this.ShowCaption = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.UseDropShadow = false;
            this.Load += new System.EventHandler(this.CustomMessageBox_Load);
            this.Controls.SetChildIndex(this.pnlLockSpace, 0);
            this.Controls.SetChildIndex(this.pnlStrechSpace, 0);
            this.pnlIconArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IconControl)).EndInit();
            this.pnlTextArea.ResumeLayout(false);
            this.pnlLockSpace.ResumeLayout(false);
            this.pnlControlArea.ResumeLayout(false);
            this.pnlStrechSpace.ResumeLayout(false);
            this.pnlStrechSpace.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private Panel pnlIconArea;
        private PictureBox IconControl;
        private Panel pnlTextArea;
        private Label lblContent;
        private Panel pnlLockSpace;
        private System.Windows.Forms.FlowLayoutPanel pnlControlArea;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Panel pnlStrechSpace;
        private System.ComponentModel.IContainer components;
        private Label CaptionLabel;
    }
}
