
namespace System.Windows.Forms
{
    partial class SlidePopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlidePopup));
            this.SuspendLayout();
            // 
            // SlidePopup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.BorderColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(141, 119);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(-1000, -1000);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlidePopup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAlarm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SlidePopup_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmAlarmPopup_MouseClick);
            this.MouseLeave += new System.EventHandler(this.frmAlarmPopup_MouseLeave);
            this.MouseHover += new System.EventHandler(this.frmAlarmPopup_MouseHover);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


    }
}