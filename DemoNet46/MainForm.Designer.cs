
namespace DemoNet46
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlTaskbar = new System.Windows.Forms.CustomPanel();
            this.gradientPanel1 = new System.Windows.Forms.CustomPanel();
            this.lblTime = new System.Windows.Forms.CustomLabel();
            this.btnStart = new System.Windows.Forms.ImageButton();
            this.portraitIcon1 = new System.Windows.Forms.PortraitIcon();
            this.pnlStart = new System.Windows.Forms.CustomPanel();
            this.tileIconList1 = new System.Windows.Forms.TileIconList();
            this.tileIcon3 = new System.Windows.Forms.TileIcon();
            this.tileIcon4 = new System.Windows.Forms.TileIcon();
            this.tileIcon1 = new System.Windows.Forms.TileIcon();
            this.btnLogoff = new System.Windows.Forms.CustomButton();
            this.btnShutdown = new System.Windows.Forms.CustomButton();
            this.portraitIcon2 = new System.Windows.Forms.PortraitIcon();
            this.btnRecycle = new System.Windows.Forms.PortraitIcon();
            this.btnShowTestForm = new System.Windows.Forms.PortraitIcon();
            this.sysTimer = new System.Windows.Forms.Timer(this.components);
            this.btnShowThemeDemo = new System.Windows.Forms.PortraitIcon();
            this.btnShowComponentDemo = new System.Windows.Forms.PortraitIcon();
            this.pnlTaskbar.SuspendLayout();
            this.gradientPanel1.SuspendLayout();
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
            this.pnlTaskbar.InnerBackColor = System.Drawing.Color.Transparent;
            this.pnlTaskbar.Location = new System.Drawing.Point(0, 848);
            this.pnlTaskbar.Name = "pnlTaskbar";
            this.pnlTaskbar.RoundBorderRadius = 0;
            this.pnlTaskbar.SecondColor = System.Drawing.Color.Black;
            this.pnlTaskbar.Size = new System.Drawing.Size(1400, 52);
            this.pnlTaskbar.TabIndex = 1;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.gradientPanel1.BorderColor = System.Drawing.Color.Empty;
            this.gradientPanel1.BorderWidth = 0;
            this.gradientPanel1.Controls.Add(this.lblTime);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.gradientPanel1.FirstColor = System.Drawing.Color.Black;
            this.gradientPanel1.InnerBackColor = System.Drawing.Color.Transparent;
            this.gradientPanel1.Location = new System.Drawing.Point(1313, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.RoundBorderRadius = 0;
            this.gradientPanel1.SecondColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gradientPanel1.Size = new System.Drawing.Size(87, 52);
            this.gradientPanel1.TabIndex = 1;
            // 
            // lblTime
            // 
            this.lblTime.BorderColor = System.Drawing.Color.Empty;
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime.FirstColor = System.Drawing.Color.Empty;
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(0, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.SecondColor = System.Drawing.Color.Empty;
            this.lblTime.Size = new System.Drawing.Size(87, 52);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "时间";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.ButtonKeepPressed = false;
            this.btnStart.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStart.DownImage = global::DemoNet46.Properties.Resources.Win7Pressed;
            this.btnStart.HotTrackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(243)))));
            this.btnStart.HoverImage = global::DemoNet46.Properties.Resources.Win7Hover;
            this.btnStart.Location = new System.Drawing.Point(0, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.NormalImage = global::DemoNet46.Properties.Resources.Win7Normal;
            this.btnStart.ShowFocusLine = false;
            this.btnStart.Size = new System.Drawing.Size(60, 52);
            this.btnStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "imageButton1";
            this.btnStart.ToolTipText = null;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // portraitIcon1
            // 
            this.portraitIcon1.AllowDrag = false;
            this.portraitIcon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.portraitIcon1.Caption = "图标选中";
            this.portraitIcon1.DragSender = null;
            this.portraitIcon1.FillDegree = 100;
            this.portraitIcon1.FirstColor = System.Drawing.Color.White;
            this.portraitIcon1.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.portraitIcon1.ForeColor = System.Drawing.Color.Black;
            this.portraitIcon1.GrayImage = ((System.Drawing.Image)(resources.GetObject("portraitIcon1.GrayImage")));
            this.portraitIcon1.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
            this.portraitIcon1.Image = ((System.Drawing.Image)(resources.GetObject("portraitIcon1.Image")));
            this.portraitIcon1.IsSelected = true;
            this.portraitIcon1.Location = new System.Drawing.Point(124, 16);
            this.portraitIcon1.Margin = new System.Windows.Forms.Padding(0);
            this.portraitIcon1.Name = "portraitIcon1";
            this.portraitIcon1.RoundedCornerAngle = 10;
            this.portraitIcon1.SecondColor = System.Drawing.Color.White;
            this.portraitIcon1.ShowCaption = true;
            this.portraitIcon1.ShowGrayImage = false;
            this.portraitIcon1.ShowIconBorder = false;
            this.portraitIcon1.Size = new System.Drawing.Size(101, 91);
            this.portraitIcon1.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.AutoResize;
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
            this.pnlStart.InnerBackColor = System.Drawing.Color.Transparent;
            this.pnlStart.Location = new System.Drawing.Point(2, 522);
            this.pnlStart.Name = "pnlStart";
            this.pnlStart.RoundBorderRadius = 12;
            this.pnlStart.SecondColor = System.Drawing.Color.Gray;
            this.pnlStart.Size = new System.Drawing.Size(529, 328);
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
            this.tileIconList1.Controls.Add(this.tileIcon3);
            this.tileIconList1.Controls.Add(this.tileIcon4);
            this.tileIconList1.Controls.Add(this.tileIcon1);
            this.tileIconList1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tileIconList1.Location = new System.Drawing.Point(251, 16);
            this.tileIconList1.Name = "tileIconList1";
            this.tileIconList1.Padding = new System.Windows.Forms.Padding(1);
            this.tileIconList1.Size = new System.Drawing.Size(263, 256);
            this.tileIconList1.TabIndex = 1002;
            // 
            // tileIcon3
            // 
            this.tileIcon3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tileIcon3.HoverBackColor = System.Drawing.Color.DodgerBlue;
            this.tileIcon3.IconText = "IntelliJ IDEA";
            this.tileIcon3.Image = ((System.Drawing.Image)(resources.GetObject("tileIcon3.Image")));
            this.tileIcon3.IsSelected = false;
            this.tileIcon3.KeepSelected = false;
            this.tileIcon3.Location = new System.Drawing.Point(1, 1);
            this.tileIcon3.Margin = new System.Windows.Forms.Padding(0);
            this.tileIcon3.Name = "tileIcon3";
            this.tileIcon3.Padding = new System.Windows.Forms.Padding(5);
            this.tileIcon3.SelectedBackColor = System.Drawing.Color.DodgerBlue;
            this.tileIcon3.ShowIconBorder = false;
            this.tileIcon3.Size = new System.Drawing.Size(261, 51);
            this.tileIcon3.TabIndex = 5;
            this.tileIcon3.WrapText = false;
            // 
            // tileIcon4
            // 
            this.tileIcon4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tileIcon4.HoverBackColor = System.Drawing.Color.DodgerBlue;
            this.tileIcon4.IconText = "Photoshop";
            this.tileIcon4.Image = ((System.Drawing.Image)(resources.GetObject("tileIcon4.Image")));
            this.tileIcon4.IsSelected = false;
            this.tileIcon4.KeepSelected = false;
            this.tileIcon4.Location = new System.Drawing.Point(1, 52);
            this.tileIcon4.Margin = new System.Windows.Forms.Padding(0);
            this.tileIcon4.Name = "tileIcon4";
            this.tileIcon4.Padding = new System.Windows.Forms.Padding(5);
            this.tileIcon4.SelectedBackColor = System.Drawing.Color.DodgerBlue;
            this.tileIcon4.ShowIconBorder = false;
            this.tileIcon4.Size = new System.Drawing.Size(261, 51);
            this.tileIcon4.TabIndex = 6;
            this.tileIcon4.WrapText = false;
            // 
            // tileIcon1
            // 
            this.tileIcon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tileIcon1.HoverBackColor = System.Drawing.Color.DodgerBlue;
            this.tileIcon1.IconText = "Android Studio";
            this.tileIcon1.Image = ((System.Drawing.Image)(resources.GetObject("tileIcon1.Image")));
            this.tileIcon1.IsSelected = false;
            this.tileIcon1.KeepSelected = false;
            this.tileIcon1.Location = new System.Drawing.Point(1, 103);
            this.tileIcon1.Margin = new System.Windows.Forms.Padding(0);
            this.tileIcon1.Name = "tileIcon1";
            this.tileIcon1.Padding = new System.Windows.Forms.Padding(5);
            this.tileIcon1.SelectedBackColor = System.Drawing.Color.DodgerBlue;
            this.tileIcon1.ShowIconBorder = false;
            this.tileIcon1.Size = new System.Drawing.Size(261, 51);
            this.tileIcon1.TabIndex = 7;
            this.tileIcon1.WrapText = false;
            // 
            // btnLogoff
            // 
            this.btnLogoff.BackColor = System.Drawing.Color.Gold;
            this.btnLogoff.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnLogoff.CornerRadius = 12;
            this.btnLogoff.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogoff.ForeColor = System.Drawing.Color.SaddleBrown;
            this.btnLogoff.GradientMode = false;
            this.btnLogoff.Location = new System.Drawing.Point(413, 278);
            this.btnLogoff.Name = "btnLogoff";
            this.btnLogoff.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnLogoff.ShadeMode = false;
            this.btnLogoff.Size = new System.Drawing.Size(101, 41);
            this.btnLogoff.TabIndex = 1001;
            this.btnLogoff.Text = "注销";
            // 
            // btnShutdown
            // 
            this.btnShutdown.BackColor = System.Drawing.Color.White;
            this.btnShutdown.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnShutdown.CornerRadius = 12;
            this.btnShutdown.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShutdown.GradientMode = true;
            this.btnShutdown.Location = new System.Drawing.Point(306, 278);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnShutdown.ShadeMode = false;
            this.btnShutdown.Size = new System.Drawing.Size(101, 41);
            this.btnShutdown.TabIndex = 1001;
            this.btnShutdown.Text = "关机";
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // portraitIcon2
            // 
            this.portraitIcon2.AllowDrag = false;
            this.portraitIcon2.Caption = "图标灰度";
            this.portraitIcon2.DragSender = null;
            this.portraitIcon2.FillDegree = 100;
            this.portraitIcon2.FirstColor = System.Drawing.Color.White;
            this.portraitIcon2.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.portraitIcon2.ForeColor = System.Drawing.Color.White;
            this.portraitIcon2.GrayImage = ((System.Drawing.Image)(resources.GetObject("portraitIcon2.GrayImage")));
            this.portraitIcon2.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
            this.portraitIcon2.Image = ((System.Drawing.Image)(resources.GetObject("portraitIcon2.Image")));
            this.portraitIcon2.IsSelected = false;
            this.portraitIcon2.Location = new System.Drawing.Point(12, 16);
            this.portraitIcon2.Margin = new System.Windows.Forms.Padding(0);
            this.portraitIcon2.Name = "portraitIcon2";
            this.portraitIcon2.RoundedCornerAngle = 10;
            this.portraitIcon2.SecondColor = System.Drawing.Color.White;
            this.portraitIcon2.ShowCaption = true;
            this.portraitIcon2.ShowGrayImage = true;
            this.portraitIcon2.ShowIconBorder = false;
            this.portraitIcon2.Size = new System.Drawing.Size(101, 91);
            this.portraitIcon2.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.AutoResize;
            this.portraitIcon2.TabIndex = 1000;
            // 
            // btnRecycle
            // 
            this.btnRecycle.AllowDrag = false;
            this.btnRecycle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecycle.Caption = "回收站";
            this.btnRecycle.DragSender = null;
            this.btnRecycle.FillDegree = 100;
            this.btnRecycle.FirstColor = System.Drawing.Color.White;
            this.btnRecycle.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.btnRecycle.ForeColor = System.Drawing.Color.White;
            this.btnRecycle.GrayImage = ((System.Drawing.Image)(resources.GetObject("btnRecycle.GrayImage")));
            this.btnRecycle.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
            this.btnRecycle.Image = ((System.Drawing.Image)(resources.GetObject("btnRecycle.Image")));
            this.btnRecycle.IsSelected = false;
            this.btnRecycle.Location = new System.Drawing.Point(1274, 727);
            this.btnRecycle.Margin = new System.Windows.Forms.Padding(0);
            this.btnRecycle.Name = "btnRecycle";
            this.btnRecycle.RoundedCornerAngle = 10;
            this.btnRecycle.SecondColor = System.Drawing.Color.White;
            this.btnRecycle.ShowCaption = true;
            this.btnRecycle.ShowGrayImage = false;
            this.btnRecycle.ShowIconBorder = false;
            this.btnRecycle.Size = new System.Drawing.Size(101, 91);
            this.btnRecycle.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.AutoResize;
            this.btnRecycle.TabIndex = 1004;
            this.btnRecycle.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.btnRecycle_MouseDoubleClick);
            // 
            // btnShowTestForm
            // 
            this.btnShowTestForm.AllowDrag = false;
            this.btnShowTestForm.Caption = "布局演示";
            this.btnShowTestForm.DragSender = null;
            this.btnShowTestForm.FillDegree = 100;
            this.btnShowTestForm.FirstColor = System.Drawing.Color.White;
            this.btnShowTestForm.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.btnShowTestForm.ForeColor = System.Drawing.Color.White;
            this.btnShowTestForm.GrayImage = ((System.Drawing.Image)(resources.GetObject("btnShowTestForm.GrayImage")));
            this.btnShowTestForm.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
            this.btnShowTestForm.Image = ((System.Drawing.Image)(resources.GetObject("btnShowTestForm.Image")));
            this.btnShowTestForm.IsSelected = false;
            this.btnShowTestForm.Location = new System.Drawing.Point(40, 181);
            this.btnShowTestForm.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowTestForm.Name = "btnShowTestForm";
            this.btnShowTestForm.RoundedCornerAngle = 10;
            this.btnShowTestForm.SecondColor = System.Drawing.Color.White;
            this.btnShowTestForm.ShowCaption = true;
            this.btnShowTestForm.ShowGrayImage = false;
            this.btnShowTestForm.ShowIconBorder = false;
            this.btnShowTestForm.Size = new System.Drawing.Size(101, 91);
            this.btnShowTestForm.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.AutoResize;
            this.btnShowTestForm.TabIndex = 1005;
            this.btnShowTestForm.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.btnShowTestForm_MouseDoubleClick);
            // 
            // sysTimer
            // 
            this.sysTimer.Enabled = true;
            this.sysTimer.Interval = 500;
            this.sysTimer.Tick += new System.EventHandler(this.sysTimer_Tick);
            // 
            // btnShowThemeDemo
            // 
            this.btnShowThemeDemo.AllowDrag = false;
            this.btnShowThemeDemo.Caption = "主题演示";
            this.btnShowThemeDemo.DragSender = null;
            this.btnShowThemeDemo.FillDegree = 100;
            this.btnShowThemeDemo.FirstColor = System.Drawing.Color.White;
            this.btnShowThemeDemo.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.btnShowThemeDemo.ForeColor = System.Drawing.Color.White;
            this.btnShowThemeDemo.GrayImage = ((System.Drawing.Image)(resources.GetObject("btnShowThemeDemo.GrayImage")));
            this.btnShowThemeDemo.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
            this.btnShowThemeDemo.Image = ((System.Drawing.Image)(resources.GetObject("btnShowThemeDemo.Image")));
            this.btnShowThemeDemo.IsSelected = false;
            this.btnShowThemeDemo.Location = new System.Drawing.Point(40, 63);
            this.btnShowThemeDemo.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowThemeDemo.Name = "btnShowThemeDemo";
            this.btnShowThemeDemo.RoundedCornerAngle = 10;
            this.btnShowThemeDemo.SecondColor = System.Drawing.Color.White;
            this.btnShowThemeDemo.ShowCaption = true;
            this.btnShowThemeDemo.ShowGrayImage = false;
            this.btnShowThemeDemo.ShowIconBorder = false;
            this.btnShowThemeDemo.Size = new System.Drawing.Size(101, 91);
            this.btnShowThemeDemo.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.AutoResize;
            this.btnShowThemeDemo.TabIndex = 1005;
            this.btnShowThemeDemo.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.btnShowThemeDemo_MouseDoubleClick);
            // 
            // btnShowComponentDemo
            // 
            this.btnShowComponentDemo.AllowDrag = false;
            this.btnShowComponentDemo.Caption = "组件演示";
            this.btnShowComponentDemo.DragSender = null;
            this.btnShowComponentDemo.FillDegree = 100;
            this.btnShowComponentDemo.FirstColor = System.Drawing.Color.White;
            this.btnShowComponentDemo.FocusBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.btnShowComponentDemo.ForeColor = System.Drawing.Color.White;
            this.btnShowComponentDemo.GrayImage = ((System.Drawing.Image)(resources.GetObject("btnShowComponentDemo.GrayImage")));
            this.btnShowComponentDemo.HoverBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(224)))), ((int)(((byte)(243)))));
            this.btnShowComponentDemo.Image = ((System.Drawing.Image)(resources.GetObject("btnShowComponentDemo.Image")));
            this.btnShowComponentDemo.IsSelected = false;
            this.btnShowComponentDemo.Location = new System.Drawing.Point(40, 299);
            this.btnShowComponentDemo.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowComponentDemo.Name = "btnShowComponentDemo";
            this.btnShowComponentDemo.RoundedCornerAngle = 10;
            this.btnShowComponentDemo.SecondColor = System.Drawing.Color.White;
            this.btnShowComponentDemo.ShowCaption = true;
            this.btnShowComponentDemo.ShowGrayImage = false;
            this.btnShowComponentDemo.ShowIconBorder = false;
            this.btnShowComponentDemo.Size = new System.Drawing.Size(101, 91);
            this.btnShowComponentDemo.SizeMode = System.Windows.Forms.PortraitIcon.IconSizeMode.AutoResize;
            this.btnShowComponentDemo.TabIndex = 1006;
            this.btnShowComponentDemo.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.btnShowComponentDemo_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DemoNet46.Properties.Resources.earth_mountain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.btnShowComponentDemo);
            this.Controls.Add(this.pnlStart);
            this.Controls.Add(this.btnShowThemeDemo);
            this.Controls.Add(this.btnShowTestForm);
            this.Controls.Add(this.btnRecycle);
            this.Controls.Add(this.pnlTaskbar);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "这是一个窗口标题";
            this.Controls.SetChildIndex(this.pnlTaskbar, 0);
            this.Controls.SetChildIndex(this.btnRecycle, 0);
            this.Controls.SetChildIndex(this.btnShowTestForm, 0);
            this.Controls.SetChildIndex(this.btnShowThemeDemo, 0);
            this.Controls.SetChildIndex(this.pnlStart, 0);
            this.Controls.SetChildIndex(this.btnShowComponentDemo, 0);
            this.pnlTaskbar.ResumeLayout(false);
            this.gradientPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnStart)).EndInit();
            this.pnlStart.ResumeLayout(false);
            this.tileIconList1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.ImageButton btnStart;
		private System.Windows.Forms.CustomPanel pnlTaskbar;
		private System.Windows.Forms.CustomPanel gradientPanel1;
		private System.Windows.Forms.PortraitIcon portraitIcon1;
		private System.Windows.Forms.CustomPanel pnlStart;
		private System.Windows.Forms.PortraitIcon portraitIcon2;
		private System.Windows.Forms.TileIconList tileIconList1;
		private System.Windows.Forms.TileIcon tileIcon3;
		private System.Windows.Forms.TileIcon tileIcon4;
		private System.Windows.Forms.CustomButton btnLogoff;
		private System.Windows.Forms.CustomButton btnShutdown;
		private System.Windows.Forms.PortraitIcon btnRecycle;
		private System.Windows.Forms.PortraitIcon btnShowTestForm;
        private System.Windows.Forms.CustomLabel lblTime;
        private System.Windows.Forms.Timer sysTimer;
        private System.Windows.Forms.TileIcon tileIcon1;
        private System.Windows.Forms.PortraitIcon btnShowThemeDemo;
        private System.Windows.Forms.PortraitIcon btnShowComponentDemo;
    }
}

