namespace System.Windows.Forms
{
    partial class DropDownList
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
            this.ucItemPanel = new System.Windows.Forms.TileIconList();
            this.SuspendLayout();
            // 
            // ucItemPanel
            // 
            this.ucItemPanel.AutoScroll = true;
            this.ucItemPanel.BackColor = System.Drawing.Color.White;
            this.ucItemPanel.BorderColor = System.Drawing.Color.Empty;
            this.ucItemPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucItemPanel.Location = new System.Drawing.Point(0, 0);
            this.ucItemPanel.Name = "ucItemPanel";
            this.ucItemPanel.Size = new System.Drawing.Size(178, 120);
            this.ucItemPanel.TabIndex = 0;
            // 
            // DropDownList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(178, 120);
            this.ControlBox = false;
            this.Controls.Add(this.ucItemPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DropDownList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DropDownList";
            this.ResumeLayout(false);

        }

        #endregion

        private TileIconList ucItemPanel;

    }
}