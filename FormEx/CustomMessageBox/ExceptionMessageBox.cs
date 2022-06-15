using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    /// <summary>
    /// 一个显示异常消息的对话框
    /// </summary>
    public partial class ExceptionMessageBox : Form
    {
        public static void Show(Exception ex)
        {
            ExceptionMessageBox box = new ExceptionMessageBox(ex);
            Form owner = FormManager.TryGetLatestActiveForm();
            box.ShowDialog(owner);
        }

        #region 构造

        public ExceptionMessageBox()
        {
            InitializeComponent();
        }


        public ExceptionMessageBox(Exception exception) :this()
        {

            var temp = exception;
            while (temp != null && temp.InnerException != null)
            {
                temp = temp.InnerException;
            }

            this.ExceptionObject = temp;
            this.DialogCaption = "异常消息";
            this.DialogContent = temp.Message;

            this.txtDetails.Text = exception.StackTrace;
        }

        #endregion

        #region 字段

        private Panel pnlIconArea;
        private PictureBox pbExceptionIcon;
        private Panel pnlTextArea;
        private Panel pnlStrechSpace;
        private Label lblContent;
        private Button btnOK;
        protected IContainer components;
        private Button btnShowDetails;
        private TextBox txtDetails;
        private Panel pnlLockSpace;
        private Panel pnlControlArea;
        private Panel pnlSunkBorderContainer;
        private int _actualHeight;

        #endregion

        #region 属性

        [Category("Custom")]
        public string DialogCaption
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        [Category("Custom")]
        public string DialogContent
        {
            get => lblContent.Text;
            set
            {
                lblContent.Text = value;
            }
        }

        [Category("Custom")]
        public string OKButtonText
        {
            get
            {
                return btnOK.Text;
            }
            set
            {
                btnOK.Text = value;
            }

        }

        [Category("Custom")]
        public Image ExceptionIcon
        {
            get
            {
                return pbExceptionIcon.Image;
            }
            set
            {
                pbExceptionIcon.Image = value;
            }
        }

        [Category("Custom")]
        public Exception ExceptionObject
        {
            get;
            set;
        }

        #endregion

        #region 方法

        public new DialogResult Show()
        {
            return ShowDialog();
        }

        #endregion

        #region 设计器生成的代码

        private void InitializeComponent()
        {
            this.pnlIconArea = new System.Windows.Forms.Panel();
            this.pbExceptionIcon = new System.Windows.Forms.PictureBox();
            this.pnlTextArea = new System.Windows.Forms.Panel();
            this.lblContent = new System.Windows.Forms.Label();
            this.pnlStrechSpace = new System.Windows.Forms.Panel();
            this.pnlSunkBorderContainer = new System.Windows.Forms.Panel();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.pnlControlArea = new System.Windows.Forms.Panel();
            this.btnShowDetails = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlLockSpace = new System.Windows.Forms.Panel();
            this.pnlIconArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptionIcon)).BeginInit();
            this.pnlTextArea.SuspendLayout();
            this.pnlStrechSpace.SuspendLayout();
            this.pnlSunkBorderContainer.SuspendLayout();
            this.pnlControlArea.SuspendLayout();
            this.pnlLockSpace.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIconArea
            // 
            this.pnlIconArea.Controls.Add(this.pbExceptionIcon);
            this.pnlIconArea.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIconArea.Location = new System.Drawing.Point(0, 0);
            this.pnlIconArea.Name = "pnlIconArea";
            this.pnlIconArea.Padding = new System.Windows.Forms.Padding(20, 20, 10, 20);
            this.pnlIconArea.Size = new System.Drawing.Size(78, 111);
            this.pnlIconArea.TabIndex = 0;
            // 
            // pbExceptionIcon
            // 
            this.pbExceptionIcon.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbExceptionIcon.Image = global::System.Windows.Forms.Properties.Resources.exception;
            this.pbExceptionIcon.Location = new System.Drawing.Point(20, 20);
            this.pbExceptionIcon.Name = "pbExceptionIcon";
            this.pbExceptionIcon.Size = new System.Drawing.Size(48, 48);
            this.pbExceptionIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbExceptionIcon.TabIndex = 0;
            this.pbExceptionIcon.TabStop = false;
            // 
            // pnlTextArea
            // 
            this.pnlTextArea.Controls.Add(this.lblContent);
            this.pnlTextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTextArea.Location = new System.Drawing.Point(78, 0);
            this.pnlTextArea.Name = "pnlTextArea";
            this.pnlTextArea.Padding = new System.Windows.Forms.Padding(10, 25, 30, 30);
            this.pnlTextArea.Size = new System.Drawing.Size(334, 111);
            this.pnlTextArea.TabIndex = 0;
            // 
            // lblContent
            // 
            this.lblContent.AutoEllipsis = true;
            this.lblContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblContent.Location = new System.Drawing.Point(10, 25);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(294, 56);
            this.lblContent.TabIndex = 0;
            this.lblContent.Text = "这是一条异常消息 ";
            this.lblContent.UseCompatibleTextRendering = true;
            // 
            // pnlStrechSpace
            // 
            this.pnlStrechSpace.BackColor = System.Drawing.SystemColors.Control;
            this.pnlStrechSpace.Controls.Add(this.pnlSunkBorderContainer);
            this.pnlStrechSpace.Controls.Add(this.pnlControlArea);
            this.pnlStrechSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStrechSpace.Location = new System.Drawing.Point(0, 111);
            this.pnlStrechSpace.Name = "pnlStrechSpace";
            this.pnlStrechSpace.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlStrechSpace.Size = new System.Drawing.Size(412, 50);
            this.pnlStrechSpace.TabIndex = 0;
            // 
            // pnlSunkBorderContainer
            // 
            this.pnlSunkBorderContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSunkBorderContainer.Controls.Add(this.txtDetails);
            this.pnlSunkBorderContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSunkBorderContainer.Location = new System.Drawing.Point(10, 48);
            this.pnlSunkBorderContainer.Name = "pnlSunkBorderContainer";
            this.pnlSunkBorderContainer.Size = new System.Drawing.Size(392, 0);
            this.pnlSunkBorderContainer.TabIndex = 103;
            // 
            // txtDetails
            // 
            this.txtDetails.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetails.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtDetails.HideSelection = false;
            this.txtDetails.Location = new System.Drawing.Point(0, 0);
            this.txtDetails.MaxLength = 6553500;
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetails.Size = new System.Drawing.Size(392, 0);
            this.txtDetails.TabIndex = 101;
            this.txtDetails.Visible = false;
            // 
            // pnlControlArea
            // 
            this.pnlControlArea.Controls.Add(this.btnShowDetails);
            this.pnlControlArea.Controls.Add(this.btnOK);
            this.pnlControlArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControlArea.Location = new System.Drawing.Point(10, 0);
            this.pnlControlArea.Name = "pnlControlArea";
            this.pnlControlArea.Size = new System.Drawing.Size(392, 48);
            this.pnlControlArea.TabIndex = 102;
            // 
            // btnShowDetails
            // 
            this.btnShowDetails.Location = new System.Drawing.Point(0, 8);
            this.btnShowDetails.Name = "btnShowDetails";
            this.btnShowDetails.Size = new System.Drawing.Size(95, 32);
            this.btnShowDetails.TabIndex = 100;
            this.btnShowDetails.Text = "查看详情";
            this.btnShowDetails.UseVisualStyleBackColor = true;
            this.btnShowDetails.Click += new System.EventHandler(this.btnShowDetails_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(310, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 32);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlLockSpace
            // 
            this.pnlLockSpace.Controls.Add(this.pnlTextArea);
            this.pnlLockSpace.Controls.Add(this.pnlIconArea);
            this.pnlLockSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLockSpace.Location = new System.Drawing.Point(0, 0);
            this.pnlLockSpace.Name = "pnlLockSpace";
            this.pnlLockSpace.Size = new System.Drawing.Size(412, 111);
            this.pnlLockSpace.TabIndex = 1;
            // 
            // ExceptionMessageBox
            // 
            this.AcceptButton = this.btnOK;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(412, 161);
            this.Controls.Add(this.pnlStrechSpace);
            this.Controls.Add(this.pnlLockSpace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(335, 200);
            this.Name = "ExceptionMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.ExceptionMessageBox_Load);
            this.pnlIconArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptionIcon)).EndInit();
            this.pnlTextArea.ResumeLayout(false);
            this.pnlStrechSpace.ResumeLayout(false);
            this.pnlSunkBorderContainer.ResumeLayout(false);
            this.pnlSunkBorderContainer.PerformLayout();
            this.pnlControlArea.ResumeLayout(false);
            this.pnlLockSpace.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion


        #region 事件处理

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnShowDetails_Click(object sender, EventArgs e)
        {
            if (txtDetails.Visible)
            {
                txtDetails.Visible = false;
                this.Height = _actualHeight;
            }
            else
            {
                txtDetails.Visible = true;
                this.Height = _actualHeight + 200;
            }
        }

        private void ExceptionMessageBox_Load(object sender, EventArgs e)
        {
            _actualHeight = this.Height;
        }

        #endregion
    }
}
