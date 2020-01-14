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
            this.innerTextBox = new System.Windows.Forms.RoundTextBox.TabEchoTextBox();
            this.btnDropDown = new System.Windows.Forms.CustomDropDownButton();
            this.SuspendLayout();
            // 
            // innerTextBox
            // 
            this.innerTextBox.AcceptsReturn = true;
            this.innerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.innerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerTextBox.Location = new System.Drawing.Point(5, 3);
            this.innerTextBox.Name = "innerTextBox";
            this.innerTextBox.Size = new System.Drawing.Size(142, 14);
            this.innerTextBox.TabIndex = 0;
            this.innerTextBox.TabAction += new System.Action(this.innerTextBox_TabAction);
            // 
            // btnDropDown
            // 
            this.btnDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDropDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(248)))), ((int)(((byte)(254)))));
            this.btnDropDown.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.btnDropDown.BorderWidth = 1F;
            this.btnDropDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(162)))), ((int)(((byte)(187)))));
            this.btnDropDown.GradientMode = false;
            this.btnDropDown.Location = new System.Drawing.Point(129, 1);
            this.btnDropDown.Name = "btnDropDown";
            this.btnDropDown.ShadeMode = false;
            this.btnDropDown.Size = new System.Drawing.Size(20, 25);
            this.btnDropDown.TabIndex = 1;
            this.btnDropDown.TabStop = false;
            this.btnDropDown.Visible = false;
            this.btnDropDown.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnDropDown_MouseClick);
            // 
            // RoundTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.btnDropDown);
            this.Controls.Add(this.innerTextBox);
            this.Name = "RoundTextBox";
            this.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.Size = new System.Drawing.Size(150, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabEchoTextBox innerTextBox;
        private CustomDropDownButton btnDropDown;
    }
}
