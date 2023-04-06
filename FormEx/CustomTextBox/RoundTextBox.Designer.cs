namespace System.Windows.Forms
{
    partial class RoundTextBox
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoundTextBox));
            this._innerTextBox = new System.Windows.Forms.RoundTextBox.TabEchoTextBox();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // _innerTextBox
            // 
            this._innerTextBox.AcceptsReturn = true;
            this._innerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._innerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._innerTextBox.Location = new System.Drawing.Point(5, 3);
            this._innerTextBox.Name = "_innerTextBox";
            this._innerTextBox.Size = new System.Drawing.Size(142, 21);
            this._innerTextBox.TabIndex = 0;
            this._innerTextBox.TabAction += new System.Action(this.innerTextBox_TabAction);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Bms = System.Windows.Forms.ImageButton.ButtonMouseStatus.None;
            this.btnCancel.ButtonKeepPressed = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnCancel.DownImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.DownImage")));
            this.btnCancel.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.HoverImage")));
            this.btnCancel.Location = new System.Drawing.Point(124, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.NormalImage")));
            this.btnCancel.ShowFocusLine = false;
            this.btnCancel.Size = new System.Drawing.Size(20, 20);
            this.btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "imageButton1";
            this.btnCancel.ToolTipText = null;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // RoundTextBox
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this._innerTextBox);
            this.Name = "RoundTextBox";
            this.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.Size = new System.Drawing.Size(150, 27);
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabEchoTextBox _innerTextBox;
        private ImageButton btnCancel;
    }
}
