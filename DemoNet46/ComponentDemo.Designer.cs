namespace DemoNet46
{
    partial class ComponentDemo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentDemo));
            this.btnWorkShade = new System.Windows.Forms.CustomButton();
            this.btnShowException = new System.Windows.Forms.CustomButton();
            this.btnBackgroundWorkShade = new System.Windows.Forms.CustomButton();
            this.btnRoundForm = new System.Windows.Forms.CustomButton();
            this.colorGroupBox1 = new System.Windows.Forms.ColorGroupBox();
            this.customLabel2 = new System.Windows.Forms.CustomLabel();
            this.btnShowInfoTip = new System.Windows.Forms.CustomButton();
            this.circularProgressBar1 = new System.Windows.Forms.CircularProgressBar();
            this.roundButton1 = new System.Windows.Forms.RoundButton();
            this.colorGroupBox2 = new System.Windows.Forms.ColorGroupBox();
            this.customLabel3 = new System.Windows.Forms.CustomLabel();
            this.btnProgressGo = new System.Windows.Forms.CustomButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.separator1 = new System.Windows.Forms.Separator();
            this.customNavSideBar1 = new System.Windows.Forms.CustomNavSideBar();
            this.customPanel2 = new System.Windows.Forms.CustomPanel();
            this.colorGroupBox3 = new System.Windows.Forms.ColorGroupBox();
            this.customLabel4 = new System.Windows.Forms.CustomLabel();
            this.colorGroupBox1.SuspendLayout();
            this.colorGroupBox2.SuspendLayout();
            this.customPanel2.SuspendLayout();
            this.colorGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnWorkShade
            // 
            this.btnWorkShade.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnWorkShade.BorderColor = System.Drawing.Color.Teal;
            this.btnWorkShade.CornerRadius = 8;
            this.btnWorkShade.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWorkShade.ForeColor = System.Drawing.Color.White;
            this.btnWorkShade.GradientMode = true;
            this.btnWorkShade.Location = new System.Drawing.Point(23, 150);
            this.btnWorkShade.Name = "btnWorkShade";
            this.btnWorkShade.ShadeMode = false;
            this.btnWorkShade.Size = new System.Drawing.Size(122, 39);
            this.btnWorkShade.TabIndex = 0;
            this.btnWorkShade.Text = "遮盖窗体";
            this.btnWorkShade.Click += new System.EventHandler(this.btnWorkShade_Click);
            // 
            // btnShowException
            // 
            this.btnShowException.BackColor = System.Drawing.Color.ForestGreen;
            this.btnShowException.BorderColor = System.Drawing.Color.DarkGreen;
            this.btnShowException.CornerRadius = 8;
            this.btnShowException.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowException.ForeColor = System.Drawing.Color.White;
            this.btnShowException.GradientMode = true;
            this.btnShowException.Location = new System.Drawing.Point(23, 103);
            this.btnShowException.Name = "btnShowException";
            this.btnShowException.ShadeMode = false;
            this.btnShowException.Size = new System.Drawing.Size(122, 39);
            this.btnShowException.TabIndex = 0;
            this.btnShowException.Text = "异常消息窗体";
            this.btnShowException.Click += new System.EventHandler(this.btnShowException_Click);
            // 
            // btnBackgroundWorkShade
            // 
            this.btnBackgroundWorkShade.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnBackgroundWorkShade.BorderColor = System.Drawing.Color.DodgerBlue;
            this.btnBackgroundWorkShade.CornerRadius = 8;
            this.btnBackgroundWorkShade.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBackgroundWorkShade.ForeColor = System.Drawing.Color.White;
            this.btnBackgroundWorkShade.GradientMode = true;
            this.btnBackgroundWorkShade.Location = new System.Drawing.Point(23, 197);
            this.btnBackgroundWorkShade.Name = "btnBackgroundWorkShade";
            this.btnBackgroundWorkShade.ShadeMode = false;
            this.btnBackgroundWorkShade.Size = new System.Drawing.Size(122, 39);
            this.btnBackgroundWorkShade.TabIndex = 0;
            this.btnBackgroundWorkShade.Text = "带后台的遮盖窗体";
            this.btnBackgroundWorkShade.Click += new System.EventHandler(this.btnBackgroundWorkShade_Click);
            // 
            // btnRoundForm
            // 
            this.btnRoundForm.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRoundForm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnRoundForm.CornerRadius = 0;
            this.btnRoundForm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRoundForm.ForeColor = System.Drawing.Color.White;
            this.btnRoundForm.GradientMode = true;
            this.btnRoundForm.Location = new System.Drawing.Point(23, 244);
            this.btnRoundForm.Name = "btnRoundForm";
            this.btnRoundForm.ShadeMode = false;
            this.btnRoundForm.Size = new System.Drawing.Size(122, 39);
            this.btnRoundForm.TabIndex = 1000;
            this.btnRoundForm.Text = "圆角窗体";
            this.btnRoundForm.Click += new System.EventHandler(this.btnRoundForm_Click);
            // 
            // colorGroupBox1
            // 
            this.colorGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.colorGroupBox1.BorderSize = 0;
            this.colorGroupBox1.BoxBorderColor = System.Drawing.Color.CornflowerBlue;
            this.colorGroupBox1.BoxColor = System.Drawing.Color.White;
            this.colorGroupBox1.CaptionBackColor = System.Drawing.SystemColors.Control;
            this.colorGroupBox1.CaptionPosition = System.Windows.Forms.ColorGroupBox.CaptionPositions.None;
            this.colorGroupBox1.Controls.Add(this.customLabel2);
            this.colorGroupBox1.Controls.Add(this.btnShowInfoTip);
            this.colorGroupBox1.Controls.Add(this.btnRoundForm);
            this.colorGroupBox1.Controls.Add(this.btnShowException);
            this.colorGroupBox1.Controls.Add(this.btnWorkShade);
            this.colorGroupBox1.Controls.Add(this.btnBackgroundWorkShade);
            this.colorGroupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.colorGroupBox1.LineColor = System.Drawing.Color.DimGray;
            this.colorGroupBox1.Location = new System.Drawing.Point(1, 1);
            this.colorGroupBox1.Name = "colorGroupBox1";
            this.colorGroupBox1.Show3DShadow = false;
            this.colorGroupBox1.Size = new System.Drawing.Size(175, 413);
            this.colorGroupBox1.TabIndex = 1002;
            this.colorGroupBox1.Text = "窗体相关";
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.BorderColor = System.Drawing.Color.Empty;
            this.customLabel2.FirstColor = System.Drawing.Color.Empty;
            this.customLabel2.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.ForeColor = System.Drawing.Color.Gray;
            this.customLabel2.Location = new System.Drawing.Point(17, 54);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.SecondColor = System.Drawing.Color.Empty;
            this.customLabel2.Size = new System.Drawing.Size(110, 31);
            this.customLabel2.TabIndex = 1001;
            this.customLabel2.Text = "窗体例子";
            // 
            // btnShowInfoTip
            // 
            this.btnShowInfoTip.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnShowInfoTip.BorderColor = System.Drawing.Color.Indigo;
            this.btnShowInfoTip.CornerRadius = 0;
            this.btnShowInfoTip.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowInfoTip.ForeColor = System.Drawing.Color.White;
            this.btnShowInfoTip.GradientMode = true;
            this.btnShowInfoTip.Location = new System.Drawing.Point(23, 291);
            this.btnShowInfoTip.Name = "btnShowInfoTip";
            this.btnShowInfoTip.RoundCorners = ((System.Windows.Forms.Corners)((((System.Windows.Forms.Corners.TopLeft | System.Windows.Forms.Corners.TopRight) 
            | System.Windows.Forms.Corners.BottomLeft) 
            | System.Windows.Forms.Corners.BottomRight)));
            this.btnShowInfoTip.ShadeMode = false;
            this.btnShowInfoTip.Size = new System.Drawing.Size(122, 39);
            this.btnShowInfoTip.TabIndex = 1000;
            this.btnShowInfoTip.Text = "提示窗体";
            this.btnShowInfoTip.Click += new System.EventHandler(this.btnShowInfoTip_Click);
            // 
            // circularProgressBar1
            // 
            this.circularProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar1.ForeColor = System.Drawing.Color.DimGray;
            this.circularProgressBar1.Location = new System.Drawing.Point(28, 117);
            this.circularProgressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.circularProgressBar1.Maximum = 100F;
            this.circularProgressBar1.Name = "circularProgressBar1";
            this.circularProgressBar1.ProgressBackColor = System.Drawing.Color.DimGray;
            this.circularProgressBar1.ProgressBeginColor = System.Drawing.Color.Black;
            this.circularProgressBar1.ProgressEndColor = System.Drawing.Color.Crimson;
            this.circularProgressBar1.ProgressValueColor = System.Drawing.Color.DodgerBlue;
            this.circularProgressBar1.ProgressWidth = 16F;
            this.circularProgressBar1.Size = new System.Drawing.Size(120, 120);
            this.circularProgressBar1.TabIndex = 1003;
            this.circularProgressBar1.TextColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.circularProgressBar1.TextColor2 = System.Drawing.Color.Black;
            this.circularProgressBar1.TextColor3 = System.Drawing.Color.DimGray;
            this.circularProgressBar1.TextFont1 = new System.Drawing.Font("Arial", 14F);
            this.circularProgressBar1.TextFont2 = new System.Drawing.Font("Arial", 20F);
            this.circularProgressBar1.TextFont3 = new System.Drawing.Font("Arial", 14F);
            this.circularProgressBar1.TextLine1 = "怒气";
            this.circularProgressBar1.TextLine2 = "100";
            this.circularProgressBar1.TextLine3 = "%";
            this.circularProgressBar1.Value = 100F;
            // 
            // roundButton1
            // 
            this.roundButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.roundButton1.ButtonColor = System.Drawing.Color.Maroon;
            this.roundButton1.Diameter = 54;
            this.roundButton1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.roundButton1.ForeColor = System.Drawing.Color.White;
            this.roundButton1.ImageEnter = null;
            this.roundButton1.ImageNormal = null;
            this.roundButton1.Location = new System.Drawing.Point(28, 103);
            this.roundButton1.Margin = new System.Windows.Forms.Padding(2);
            this.roundButton1.Name = "roundButton1";
            this.roundButton1.OutlineColor = System.Drawing.Color.IndianRed;
            this.roundButton1.OutlineHoverColor = System.Drawing.Color.Red;
            this.roundButton1.OutlineWidth = 4;
            this.roundButton1.Size = new System.Drawing.Size(54, 54);
            this.roundButton1.TabIndex = 1004;
            this.roundButton1.Text = "99";
            // 
            // colorGroupBox2
            // 
            this.colorGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.colorGroupBox2.BorderSize = 0;
            this.colorGroupBox2.BoxBorderColor = System.Drawing.Color.CornflowerBlue;
            this.colorGroupBox2.BoxColor = System.Drawing.Color.White;
            this.colorGroupBox2.CaptionBackColor = System.Drawing.SystemColors.Control;
            this.colorGroupBox2.CaptionPosition = System.Windows.Forms.ColorGroupBox.CaptionPositions.None;
            this.colorGroupBox2.Controls.Add(this.customLabel3);
            this.colorGroupBox2.Controls.Add(this.circularProgressBar1);
            this.colorGroupBox2.Controls.Add(this.btnProgressGo);
            this.colorGroupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.colorGroupBox2.Location = new System.Drawing.Point(176, 1);
            this.colorGroupBox2.Name = "colorGroupBox2";
            this.colorGroupBox2.Show3DShadow = false;
            this.colorGroupBox2.Size = new System.Drawing.Size(179, 413);
            this.colorGroupBox2.TabIndex = 1005;
            this.colorGroupBox2.Text = "colorGroupBox2";
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.BorderColor = System.Drawing.Color.Empty;
            this.customLabel3.FirstColor = System.Drawing.Color.Empty;
            this.customLabel3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.ForeColor = System.Drawing.Color.Gray;
            this.customLabel3.Location = new System.Drawing.Point(22, 54);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.SecondColor = System.Drawing.Color.Empty;
            this.customLabel3.Size = new System.Drawing.Size(86, 31);
            this.customLabel3.TabIndex = 1001;
            this.customLabel3.Text = "进度条";
            // 
            // btnProgressGo
            // 
            this.btnProgressGo.BackColor = System.Drawing.Color.White;
            this.btnProgressGo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnProgressGo.CornerRadius = 0;
            this.btnProgressGo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProgressGo.GradientMode = true;
            this.btnProgressGo.Location = new System.Drawing.Point(28, 244);
            this.btnProgressGo.Name = "btnProgressGo";
            this.btnProgressGo.ShadeMode = false;
            this.btnProgressGo.Size = new System.Drawing.Size(122, 39);
            this.btnProgressGo.TabIndex = 1000;
            this.btnProgressGo.Text = "增加进度";
            this.btnProgressGo.Click += new System.EventHandler(this.btnProgressGo_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "halfbrick1.png");
            this.imageList1.Images.SetKeyName(1, "halfbrick2.png");
            // 
            // separator1
            // 
            this.separator1.BackColor = System.Drawing.Color.Transparent;
            this.separator1.Direction = System.Windows.Forms.Separator.SeparationDirections.Vertical;
            this.separator1.Dock = System.Windows.Forms.DockStyle.Left;
            this.separator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.separator1.Location = new System.Drawing.Point(203, 40);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(10, 415);
            this.separator1.TabIndex = 1007;
            this.separator1.Text = "separator1";
            // 
            // customNavSideBar1
            // 
            this.customNavSideBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.customNavSideBar1.BorderColor = System.Drawing.Color.Empty;
            this.customNavSideBar1.BorderWidth = 1;
            this.customNavSideBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.customNavSideBar1.FirstColor = System.Drawing.Color.Empty;
            this.customNavSideBar1.Location = new System.Drawing.Point(0, 40);
            this.customNavSideBar1.Name = "customNavSideBar1";
            this.customNavSideBar1.RoundBorderRadius = 0;
            this.customNavSideBar1.SecondColor = System.Drawing.Color.Empty;
            this.customNavSideBar1.Size = new System.Drawing.Size(203, 415);
            this.customNavSideBar1.TabIndex = 1008;
            // 
            // customPanel2
            // 
            this.customPanel2.BackColor = System.Drawing.Color.Transparent;
            this.customPanel2.BorderColor = System.Drawing.Color.Empty;
            this.customPanel2.BorderWidth = 1;
            this.customPanel2.Controls.Add(this.colorGroupBox3);
            this.customPanel2.Controls.Add(this.colorGroupBox2);
            this.customPanel2.Controls.Add(this.colorGroupBox1);
            this.customPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel2.FirstColor = System.Drawing.Color.Empty;
            this.customPanel2.Location = new System.Drawing.Point(213, 40);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.RoundBorderRadius = 0;
            this.customPanel2.SecondColor = System.Drawing.Color.Empty;
            this.customPanel2.Size = new System.Drawing.Size(603, 415);
            this.customPanel2.TabIndex = 1009;
            // 
            // colorGroupBox3
            // 
            this.colorGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.colorGroupBox3.BorderSize = 0;
            this.colorGroupBox3.BoxBorderColor = System.Drawing.Color.CornflowerBlue;
            this.colorGroupBox3.BoxColor = System.Drawing.Color.White;
            this.colorGroupBox3.CaptionBackColor = System.Drawing.SystemColors.Control;
            this.colorGroupBox3.CaptionPosition = System.Windows.Forms.ColorGroupBox.CaptionPositions.None;
            this.colorGroupBox3.Controls.Add(this.customLabel4);
            this.colorGroupBox3.Controls.Add(this.roundButton1);
            this.colorGroupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.colorGroupBox3.Location = new System.Drawing.Point(355, 1);
            this.colorGroupBox3.Name = "colorGroupBox3";
            this.colorGroupBox3.Show3DShadow = false;
            this.colorGroupBox3.Size = new System.Drawing.Size(179, 413);
            this.colorGroupBox3.TabIndex = 1006;
            this.colorGroupBox3.Text = "colorGroupBox3";
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.BorderColor = System.Drawing.Color.Empty;
            this.customLabel4.FirstColor = System.Drawing.Color.Empty;
            this.customLabel4.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.ForeColor = System.Drawing.Color.Gray;
            this.customLabel4.Location = new System.Drawing.Point(22, 54);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.SecondColor = System.Drawing.Color.Empty;
            this.customLabel4.Size = new System.Drawing.Size(62, 31);
            this.customLabel4.TabIndex = 1001;
            this.customLabel4.Text = "其他";
            // 
            // ComponentDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 455);
            this.Controls.Add(this.customPanel2);
            this.Controls.Add(this.separator1);
            this.Controls.Add(this.customNavSideBar1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2240, 1400);
            this.MinimizeBox = false;
            this.Name = "ComponentDemo";
            this.Resizable = true;
            this.Text = "组件窗口";
            this.UseDropShadow = false;
            this.Controls.SetChildIndex(this.customNavSideBar1, 0);
            this.Controls.SetChildIndex(this.separator1, 0);
            this.Controls.SetChildIndex(this.customPanel2, 0);
            this.colorGroupBox1.ResumeLayout(false);
            this.colorGroupBox1.PerformLayout();
            this.colorGroupBox2.ResumeLayout(false);
            this.colorGroupBox2.PerformLayout();
            this.customPanel2.ResumeLayout(false);
            this.colorGroupBox3.ResumeLayout(false);
            this.colorGroupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CustomButton btnWorkShade;
        private System.Windows.Forms.CustomButton btnShowException;
        private System.Windows.Forms.CustomButton btnBackgroundWorkShade;
        private System.Windows.Forms.CustomButton btnRoundForm;
        private System.Windows.Forms.ColorGroupBox colorGroupBox1;
        private System.Windows.Forms.CircularProgressBar circularProgressBar1;
        private System.Windows.Forms.RoundButton roundButton1;
        private System.Windows.Forms.ColorGroupBox colorGroupBox2;
        private System.Windows.Forms.CustomButton btnShowInfoTip;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Separator separator1;
        private System.Windows.Forms.CustomNavSideBar customNavSideBar1;
        private System.Windows.Forms.CustomLabel customLabel2;
        private System.Windows.Forms.CustomButton btnProgressGo;
        private System.Windows.Forms.CustomLabel customLabel3;
        private System.Windows.Forms.CustomPanel customPanel2;
        private System.Windows.Forms.ColorGroupBox colorGroupBox3;
        private System.Windows.Forms.CustomLabel customLabel4;
    }
}