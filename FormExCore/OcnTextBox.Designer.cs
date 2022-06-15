namespace FormExCore
{
    partial class OcnTextBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.innerTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // innerTextBox
            // 
            this.innerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.innerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.innerTextBox.Location = new System.Drawing.Point(10, 7);
            this.innerTextBox.Name = "innerTextBox";
            this.innerTextBox.Size = new System.Drawing.Size(230, 17);
            this.innerTextBox.TabIndex = 0;
            this.innerTextBox.Click += new System.EventHandler(this.innerTextBox_Click);
            this.innerTextBox.TextChanged += new System.EventHandler(this.innerTextBox_TextChanged);
            this.innerTextBox.Enter += new System.EventHandler(this.innerTextBox_Enter);
            this.innerTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.innerTextBox_KeyPress);
            this.innerTextBox.Leave += new System.EventHandler(this.innerTextBox_Leave);
            this.innerTextBox.MouseEnter += new System.EventHandler(this.innerTextBox_MouseEnter);
            this.innerTextBox.MouseLeave += new System.EventHandler(this.innerTextBox_MouseLeave);
            // 
            // OcnTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.innerTextBox);
            this.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OcnTextBox";
            this.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.Size = new System.Drawing.Size(250, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox innerTextBox;
    }
}
