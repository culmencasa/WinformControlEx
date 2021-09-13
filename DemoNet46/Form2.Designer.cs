
namespace DemoNet46
{
	partial class Form2
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
			this.win7ControlBox1 = new System.Windows.Forms.CustomForm.Win7ControlBox();
			this.SuspendLayout();
			// 
			// win7ControlBox1
			// 
			this.win7ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.win7ControlBox1.BackColor = System.Drawing.Color.Black;
			this.win7ControlBox1.Location = new System.Drawing.Point(633, 1);
			this.win7ControlBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.win7ControlBox1.Name = "win7ControlBox1";
			this.win7ControlBox1.Size = new System.Drawing.Size(159, 23);
			this.win7ControlBox1.TabIndex = 0;
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.BackGradientDarkColor = System.Drawing.Color.Black;
			this.BackGradientLightColor = System.Drawing.Color.Black;
			this.BorderColor = System.Drawing.Color.DimGray;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.win7ControlBox1);
			this.Name = "Form2";
			this.Text = "Form2";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CustomForm.Win7ControlBox win7ControlBox1;
	}
}