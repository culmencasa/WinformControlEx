
namespace System.Windows.Forms.CustomForm
{
    partial class Win11ControlBox
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMinimum = new System.Windows.Forms.ImageButton();
            this.btnMaximum = new System.Windows.Forms.ImageButton();
            this.btnClose = new System.Windows.Forms.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMinimum
            // 
            this.btnMinimum.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnMinimum.DownImage = global::System.Windows.Forms.Properties.Resources.min3;
            this.btnMinimum.HoverImage = global::System.Windows.Forms.Properties.Resources.min2;
            this.btnMinimum.Margin = new System.Windows.Forms.Padding(0);
            this.btnMinimum.MinimumSize = new System.Drawing.Size(90, 0);
            this.btnMinimum.Name = "btnMinimum";
            this.btnMinimum.NormalImage = global::System.Windows.Forms.Properties.Resources.min1;
            this.btnMinimum.ShowFocusLine = false;
            this.btnMinimum.Size = new System.Drawing.Size(90, 54);
            this.btnMinimum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnMinimum.TabIndex = 0;
            this.btnMinimum.TabStop = false;
            this.btnMinimum.ToolTipText = "最小化";
            // 
            // btnMaximum
            // 
            this.btnMaximum.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximum.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnMaximum.DownImage = global::System.Windows.Forms.Properties.Resources.max3;
            this.btnMaximum.HoverImage = global::System.Windows.Forms.Properties.Resources.max2;
            this.btnMaximum.Margin = new System.Windows.Forms.Padding(0);
            this.btnMaximum.MinimumSize = new System.Drawing.Size(90, 0);
            this.btnMaximum.Name = "btnMaximum";
            this.btnMaximum.NormalImage = global::System.Windows.Forms.Properties.Resources.max1;
            this.btnMaximum.ShowFocusLine = false;
            this.btnMaximum.Size = new System.Drawing.Size(90, 54);
            this.btnMaximum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnMaximum.TabIndex = 1;
            this.btnMaximum.TabStop = false;
            this.btnMaximum.ToolTipText = "最大化";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnClose.DownImage = global::System.Windows.Forms.Properties.Resources.close3;
            this.btnClose.HoverImage = global::System.Windows.Forms.Properties.Resources.close2;
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.MinimumSize = new System.Drawing.Size(90, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.NormalImage = global::System.Windows.Forms.Properties.Resources.close1;
            this.btnClose.ShowFocusLine = false;
            this.btnClose.Size = new System.Drawing.Size(90, 54);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            this.btnClose.ToolTipText = "关闭";
            // 
            // Win11ControlBox
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnMinimum);
            this.Controls.Add(this.btnMaximum);
            this.Controls.Add(this.btnClose);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(270, 54);
            this.Name = "Win11ControlBox";
            this.Size = new System.Drawing.Size(729, 54);
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected ImageButton btnClose;
        protected ImageButton btnMaximum;
        protected ImageButton btnMinimum;
    }
}
