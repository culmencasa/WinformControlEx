
namespace DemoNet46
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.pnlTaskbar = new System.Windows.Forms.GradientPanel();
			this.gradientPanel1 = new System.Windows.Forms.GradientPanel();
			this.btnStart = new System.Windows.Forms.ImageButton();
			this.tileIcon1 = new System.Windows.Forms.TileIcon();
			this.portraitIcon1 = new System.Windows.Forms.PortraitIcon();
			this.pnlStart = new System.Windows.Forms.GradientPanel();
			this.tileIconList1 = new System.Windows.Forms.TileIconList();
			this.tileIcon3 = new System.Windows.Forms.TileIcon();
			this.tileIcon4 = new System.Windows.Forms.TileIcon();
			this.tileIcon2 = new System.Windows.Forms.TileIcon();
			this.tileIcon5 = new System.Windows.Forms.TileIcon();
			this.btnLogoff = new System.Windows.Forms.CustomButton();
			this.btnShutdown = new System.Windows.Forms.CustomButton();
			this.portraitIcon2 = new System.Windows.Forms.PortraitIcon();
			this.portraitIcon3 = new System.Windows.Forms.PortraitIcon();
			this.portraitIcon4 = new System.Windows.Forms.PortraitIcon();
			this.pnlTaskbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.btnStart)).BeginInit();
			this.pnlStart.SuspendLayout();
			this.tileIconList1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlTaskbar
			// 
			this.pnlTaskbar.BackColor = System.Drawing.Color.Black;
			this.pnlTaskbar.BorderColor = System.Drawing.Color.Black;
			this.pnlTaskbar.BorderWidth = 0;
			this.pnlTaskbar.Controls.Add(this.gradientPanel1);
			this.pnlTaskbar.Controls.Add(this.btnStart);
			this.pnlTaskbar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlTaskbar.FirstColor = System.Drawing.Color.Gray;
			this.pnlTaskbar.Location = new System.Drawing.Point(0, 597);
			this.pnlTaskbar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.pnlTaskbar.Name = "pnlTaskbar";
			this.pnlTaskbar.RoundBorderRadius = 0;
			this.pnlTaskbar.SecondColor = System.Drawing.Color.Black;
			this.pnlTaskbar.Size = new System.Drawing.Size(1200, 78);
			this.pnlTaskbar.TabIndex = 1;
			// 
			// gradientPanel1
			// 
			this.gradientPanel1.BackColor = System.Drawing.Color.Transparent;
			this.gradientPanel1.BorderColor = System.Drawing.Color.Empty;
			this.gradientPanel1.BorderWidth = 0;
			this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.gradientPanel1.FirstColor = System.Drawing.Color.Black;
			this.gradientPanel1.Location = new System.Drawing.Point(1070, 0);
			this.gradientPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gradientPanel1.Name = "gradientPanel1";
			this.gradientPanel1.RoundBorderRadius = 0;
			this.gradientPanel1.SecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.gradientPanel1.Size = new System.Drawing.Size(130, 78);
			this.gradientPanel1.TabIndex = 1;
			// 
			// btnStart
			// 
			this.btnStart.BackColor = System.Drawing.Color.Transparent;
			this.btnStart.DialogResult = System.Windows.Forms.DialogResult.None;
			this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnStart.DownImage = global::DemoNet46.Properties.Resources.Win7Pressed;
			this.btnStart.HoverImage = global::DemoNet46.Properties.Resources.Win7Hover;
			this.btnStart.Location = new System.Drawing.Point(0, 0);
			this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnStart.Name = "btnStart";
			this.btnStart.NormalImage = global::DemoNet46.Properties.Resources.Win7Normal;
			this.btnStart.ShowFocusLine = false;
			this.btnStart.Size = new System.Drawing.Size(90, 78);
			this.btnStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "imageButton1";
			this.btnStart.ToolTipText = null;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// tileIcon1
			// 
			this.tileIcon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
			this.tileIcon1.DefaultImage = null;
			this.tileIcon1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tileIcon1.Image = null;
			this.tileIcon1.Location = new System.Drawing.Point(2, 2);
			this.tileIcon1.Margin = new System.Windows.Forms.Padding(0);
			this.tileIcon1.Name = "tileIcon1";
			this.tileIcon1.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
			this.tileIcon1.Size = new System.Drawing.Size(297, 76);
			this.tileIcon1.TabIndex = 3;
			// 
			// portraitIcon1
			// 
			this.portraitIcon1.AllowDrag = false;
			this.portraitIcon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
			this.portraitIcon1.BrushColor = System.Drawing.Color.Transparent;
			this.portraitIcon1.Caption = "图标选中";
			this.portraitIcon1.DragSender = null;
			this.portraitIcon1.FillDegree = 100;
			this.portraitIcon1.FirstColor = System.Drawing.Color.White;
			this.portraitIcon1.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
			this.portraitIcon1.ForeColor = System.Drawing.Color.Black;
			this.portraitIcon1.GrayImage = ((System.Drawing.Image)(resources.GetObject("portraitIcon1.GrayImage")));
			this.portraitIcon1.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
			this.portraitIcon1.Image = global::DemoNet46.Properties.Resources.bigo;
			this.portraitIcon1.IsSelected = true;
			this.portraitIcon1.Location = new System.Drawing.Point(186, 24);
			this.portraitIcon1.Margin = new System.Windows.Forms.Padding(0);
			this.portraitIcon1.Name = "portraitIcon1";
			this.portraitIcon1.Opacity = 100;
			this.portraitIcon1.RoundedCornerAngle = 10;
			this.portraitIcon1.SecondColor = System.Drawing.Color.White;
			this.portraitIcon1.ShowCaption = true;
			this.portraitIcon1.ShowGrayImage = false;
			this.portraitIcon1.ShowIconBorder = false;
			this.portraitIcon1.Size = new System.Drawing.Size(152, 136);
			this.portraitIcon1.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.Center;
			this.portraitIcon1.TabIndex = 1000;
			// 
			// pnlStart
			// 
			this.pnlStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pnlStart.AutoScroll = true;
			this.pnlStart.BackColor = System.Drawing.Color.Transparent;
			this.pnlStart.BorderColor = System.Drawing.Color.White;
			this.pnlStart.BorderWidth = 2;
			this.pnlStart.Controls.Add(this.tileIconList1);
			this.pnlStart.Controls.Add(this.btnLogoff);
			this.pnlStart.Controls.Add(this.btnShutdown);
			this.pnlStart.Controls.Add(this.portraitIcon2);
			this.pnlStart.Controls.Add(this.portraitIcon1);
			this.pnlStart.FirstColor = System.Drawing.Color.Black;
			this.pnlStart.Location = new System.Drawing.Point(3, 108);
			this.pnlStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.pnlStart.Name = "pnlStart";
			this.pnlStart.RoundBorderRadius = 12;
			this.pnlStart.SecondColor = System.Drawing.Color.Gray;
			this.pnlStart.Size = new System.Drawing.Size(794, 492);
			this.pnlStart.TabIndex = 1001;
			this.pnlStart.Visible = false;
			// 
			// tileIconList1
			// 
			this.tileIconList1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tileIconList1.AutoScroll = true;
			this.tileIconList1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.tileIconList1.BorderColor = System.Drawing.Color.Gray;
			this.tileIconList1.BorderSize = 1;
			this.tileIconList1.Controls.Add(this.tileIcon1);
			this.tileIconList1.Controls.Add(this.tileIcon3);
			this.tileIconList1.Controls.Add(this.tileIcon4);
			this.tileIconList1.Controls.Add(this.tileIcon2);
			this.tileIconList1.Controls.Add(this.tileIcon5);
			this.tileIconList1.Location = new System.Drawing.Point(376, 24);
			this.tileIconList1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tileIconList1.Name = "tileIconList1";
			this.tileIconList1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.tileIconList1.Size = new System.Drawing.Size(394, 384);
			this.tileIconList1.TabIndex = 1002;
			// 
			// tileIcon3
			// 
			this.tileIcon3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
			this.tileIcon3.DefaultImage = null;
			this.tileIcon3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tileIcon3.Image = ((System.Drawing.Image)(resources.GetObject("tileIcon3.Image")));
			this.tileIcon3.Location = new System.Drawing.Point(2, 78);
			this.tileIcon3.Margin = new System.Windows.Forms.Padding(0);
			this.tileIcon3.Name = "tileIcon3";
			this.tileIcon3.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
			this.tileIcon3.Size = new System.Drawing.Size(297, 76);
			this.tileIcon3.TabIndex = 5;
			// 
			// tileIcon4
			// 
			this.tileIcon4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
			this.tileIcon4.DefaultImage = null;
			this.tileIcon4.Dock = System.Windows.Forms.DockStyle.Top;
			this.tileIcon4.Image = ((System.Drawing.Image)(resources.GetObject("tileIcon4.Image")));
			this.tileIcon4.Location = new System.Drawing.Point(2, 154);
			this.tileIcon4.Margin = new System.Windows.Forms.Padding(0);
			this.tileIcon4.Name = "tileIcon4";
			this.tileIcon4.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
			this.tileIcon4.Size = new System.Drawing.Size(297, 76);
			this.tileIcon4.TabIndex = 6;
			// 
			// tileIcon2
			// 
			this.tileIcon2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
			this.tileIcon2.DefaultImage = null;
			this.tileIcon2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tileIcon2.Image = ((System.Drawing.Image)(resources.GetObject("tileIcon2.Image")));
			this.tileIcon2.Location = new System.Drawing.Point(2, 230);
			this.tileIcon2.Margin = new System.Windows.Forms.Padding(0);
			this.tileIcon2.Name = "tileIcon2";
			this.tileIcon2.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
			this.tileIcon2.Size = new System.Drawing.Size(297, 76);
			this.tileIcon2.TabIndex = 7;
			// 
			// tileIcon5
			// 
			this.tileIcon5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
			this.tileIcon5.DefaultImage = null;
			this.tileIcon5.Dock = System.Windows.Forms.DockStyle.Top;
			this.tileIcon5.Image = ((System.Drawing.Image)(resources.GetObject("tileIcon5.Image")));
			this.tileIcon5.Location = new System.Drawing.Point(2, 306);
			this.tileIcon5.Margin = new System.Windows.Forms.Padding(0);
			this.tileIcon5.Name = "tileIcon5";
			this.tileIcon5.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
			this.tileIcon5.Size = new System.Drawing.Size(297, 76);
			this.tileIcon5.TabIndex = 8;
			// 
			// btnLogoff
			// 
			this.btnLogoff.BackColor = System.Drawing.Color.Gold;
			this.btnLogoff.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.btnLogoff.CornerRadius = 12;
			this.btnLogoff.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.btnLogoff.ForeColor = System.Drawing.Color.SaddleBrown;
			this.btnLogoff.GradientMode = false;
			this.btnLogoff.Location = new System.Drawing.Point(620, 417);
			this.btnLogoff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnLogoff.Name = "btnLogoff";
			this.btnLogoff.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
			this.btnLogoff.ShadeMode = false;
			this.btnLogoff.Size = new System.Drawing.Size(152, 62);
			this.btnLogoff.TabIndex = 1001;
			this.btnLogoff.Text = "注销";
			// 
			// btnShutdown
			// 
			this.btnShutdown.BackColor = System.Drawing.Color.White;
			this.btnShutdown.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.btnShutdown.CornerRadius = 12;
			this.btnShutdown.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.btnShutdown.GradientMode = true;
			this.btnShutdown.Location = new System.Drawing.Point(459, 417);
			this.btnShutdown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnShutdown.Name = "btnShutdown";
			this.btnShutdown.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
			this.btnShutdown.ShadeMode = false;
			this.btnShutdown.Size = new System.Drawing.Size(152, 62);
			this.btnShutdown.TabIndex = 1001;
			this.btnShutdown.Text = "关机";
			// 
			// portraitIcon2
			// 
			this.portraitIcon2.AllowDrag = false;
			this.portraitIcon2.BrushColor = System.Drawing.Color.Transparent;
			this.portraitIcon2.Caption = "图标灰度";
			this.portraitIcon2.DragSender = null;
			this.portraitIcon2.FillDegree = 100;
			this.portraitIcon2.FirstColor = System.Drawing.Color.White;
			this.portraitIcon2.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
			this.portraitIcon2.ForeColor = System.Drawing.Color.White;
			this.portraitIcon2.GrayImage = ((System.Drawing.Image)(resources.GetObject("portraitIcon2.GrayImage")));
			this.portraitIcon2.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
			this.portraitIcon2.Image = global::DemoNet46.Properties.Resources.bigo;
			this.portraitIcon2.IsSelected = false;
			this.portraitIcon2.Location = new System.Drawing.Point(18, 24);
			this.portraitIcon2.Margin = new System.Windows.Forms.Padding(0);
			this.portraitIcon2.Name = "portraitIcon2";
			this.portraitIcon2.Opacity = 100;
			this.portraitIcon2.RoundedCornerAngle = 10;
			this.portraitIcon2.SecondColor = System.Drawing.Color.White;
			this.portraitIcon2.ShowCaption = true;
			this.portraitIcon2.ShowGrayImage = true;
			this.portraitIcon2.ShowIconBorder = false;
			this.portraitIcon2.Size = new System.Drawing.Size(152, 136);
			this.portraitIcon2.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.Center;
			this.portraitIcon2.TabIndex = 1000;
			// 
			// portraitIcon3
			// 
			this.portraitIcon3.AllowDrag = false;
			this.portraitIcon3.BrushColor = System.Drawing.Color.Transparent;
			this.portraitIcon3.Caption = "回收站";
			this.portraitIcon3.DragSender = null;
			this.portraitIcon3.FillDegree = 100;
			this.portraitIcon3.FirstColor = System.Drawing.Color.White;
			this.portraitIcon3.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
			this.portraitIcon3.ForeColor = System.Drawing.Color.White;
			this.portraitIcon3.GrayImage = ((System.Drawing.Image)(resources.GetObject("portraitIcon3.GrayImage")));
			this.portraitIcon3.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
			this.portraitIcon3.Image = global::DemoNet46.Properties.Resources.Recycle;
			this.portraitIcon3.IsSelected = false;
			this.portraitIcon3.Location = new System.Drawing.Point(1024, 422);
			this.portraitIcon3.Margin = new System.Windows.Forms.Padding(0);
			this.portraitIcon3.Name = "portraitIcon3";
			this.portraitIcon3.Opacity = 100;
			this.portraitIcon3.RoundedCornerAngle = 10;
			this.portraitIcon3.SecondColor = System.Drawing.Color.White;
			this.portraitIcon3.ShowCaption = true;
			this.portraitIcon3.ShowGrayImage = true;
			this.portraitIcon3.ShowIconBorder = false;
			this.portraitIcon3.Size = new System.Drawing.Size(152, 136);
			this.portraitIcon3.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.AutoResize;
			this.portraitIcon3.TabIndex = 1004;
			// 
			// portraitIcon4
			// 
			this.portraitIcon4.AllowDrag = false;
			this.portraitIcon4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
			this.portraitIcon4.BrushColor = System.Drawing.Color.Transparent;
			this.portraitIcon4.Caption = "我的电脑";
			this.portraitIcon4.DragSender = null;
			this.portraitIcon4.FillDegree = 100;
			this.portraitIcon4.FirstColor = System.Drawing.Color.White;
			this.portraitIcon4.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
			this.portraitIcon4.ForeColor = System.Drawing.Color.White;
			this.portraitIcon4.GrayImage = ((System.Drawing.Image)(resources.GetObject("portraitIcon4.GrayImage")));
			this.portraitIcon4.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
			this.portraitIcon4.Image = global::DemoNet46.Properties.Resources.bigo;
			this.portraitIcon4.IsSelected = false;
			this.portraitIcon4.Location = new System.Drawing.Point(873, 422);
			this.portraitIcon4.Margin = new System.Windows.Forms.Padding(0);
			this.portraitIcon4.Name = "portraitIcon4";
			this.portraitIcon4.Opacity = 100;
			this.portraitIcon4.RoundedCornerAngle = 10;
			this.portraitIcon4.SecondColor = System.Drawing.Color.White;
			this.portraitIcon4.ShowCaption = true;
			this.portraitIcon4.ShowGrayImage = false;
			this.portraitIcon4.ShowIconBorder = false;
			this.portraitIcon4.Size = new System.Drawing.Size(152, 136);
			this.portraitIcon4.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.Center;
			this.portraitIcon4.TabIndex = 1005;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::DemoNet46.Properties.Resources.earth_mountain;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(1200, 675);
			this.Controls.Add(this.portraitIcon4);
			this.Controls.Add(this.portraitIcon3);
			this.Controls.Add(this.pnlStart);
			this.Controls.Add(this.pnlTaskbar);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximumSize = new System.Drawing.Size(3360, 2100);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "窗口标题";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Controls.SetChildIndex(this.pnlTaskbar, 0);
			this.Controls.SetChildIndex(this.pnlStart, 0);
			this.Controls.SetChildIndex(this.portraitIcon3, 0);
			this.Controls.SetChildIndex(this.portraitIcon4, 0);
			this.pnlTaskbar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.btnStart)).EndInit();
			this.pnlStart.ResumeLayout(false);
			this.tileIconList1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.ImageButton btnStart;
		private System.Windows.Forms.GradientPanel pnlTaskbar;
		private System.Windows.Forms.GradientPanel gradientPanel1;
		private System.Windows.Forms.TileIcon tileIcon1;
		private System.Windows.Forms.PortraitIcon portraitIcon1;
		private System.Windows.Forms.GradientPanel pnlStart;
		private System.Windows.Forms.PortraitIcon portraitIcon2;
		private System.Windows.Forms.TileIconList tileIconList1;
		private System.Windows.Forms.TileIcon tileIcon3;
		private System.Windows.Forms.TileIcon tileIcon4;
		private System.Windows.Forms.TileIcon tileIcon2;
		private System.Windows.Forms.TileIcon tileIcon5;
		private System.Windows.Forms.CustomButton btnLogoff;
		private System.Windows.Forms.CustomButton btnShutdown;
		private System.Windows.Forms.PortraitIcon portraitIcon3;
		private System.Windows.Forms.PortraitIcon portraitIcon4;
	}
}

