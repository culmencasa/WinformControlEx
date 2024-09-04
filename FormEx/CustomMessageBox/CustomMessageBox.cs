using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Utils.UI;

namespace System.Windows.Forms
{
    /// <summary>
    /// 消息对话框
    /// </summary>
    public partial class CustomMessageBox : NonFrameForm
    {
        #region 注释

        // 自动生成的资源命名空间会加上文件夹名称(CustomMessageBox), 造成命名空间和类名重名. 
        // 在运行时则找不到对应的资源名, 进而报错. 
        // 由于上述原因, 图片资源放到了一个单独的资源文件里(CustomMessageBoxResource.resx)

        #endregion

        #region 静态属性

        public static IWin32Window DefaultOwner
        {
            get;
            set;
        }


        #endregion

        #region 静态方法

        /// <summary>
        /// 在当前窗体前显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult ShowMessage(string caption, string content, MessageBoxButtons buttons, MessageBoxIcon icon, IWin32Window owner = null)
        {
            if (owner == null)
            {
                CustomMessageBox box = new CustomMessageBox()
                {
                    Caption = caption,
                    Content = content,
                    BoxButton = buttons,
                    BoxIcon = icon
                };
                return box.ShowDialog();
            }
            else
            {
                Form formOwner = owner as Form;
                if (formOwner != null && formOwner.InvokeRequired)
                {
                    return (DialogResult)formOwner.Invoke((Func<DialogResult>)delegate {
                        return ShowMessage(caption, content, buttons, icon, formOwner);
                    });
                }
                else
                {
                    CustomMessageBox box = new CustomMessageBox()
                    {
                        Caption = caption,
                        Content = content,
                        BoxButton = buttons,
                        BoxIcon = icon
                    };
                    return box.ShowDialog(owner);
                }
            }
        }

        /// <summary>
        /// 在当前窗体前显示确认消息框
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowConfirm(string caption, string content)
        {
            return ShowMessage(caption, content, MessageBoxButtons.YesNo, MessageBoxIcon.Question, DefaultOwner);
        }

        public static DialogResult ShowError(string caption, string content)
        {
            return ShowMessage(caption, content, MessageBoxButtons.OK, MessageBoxIcon.Error, DefaultOwner);
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowInformation(string caption, string content)
        {
            return ShowMessage(caption, content, MessageBoxButtons.OK, MessageBoxIcon.Information, DefaultOwner);
        }

        public static DialogResult ShowWarning(string caption, string content)
        {
            return ShowMessage(caption, content, MessageBoxButtons.OK, MessageBoxIcon.Warning, DefaultOwner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="content"></param>
        public static void Show(string caption, string content)
        {
            CustomMessageBox box = new CustomMessageBox()
            {
                Caption = caption,
                Content = content,
                BoxButton = MessageBoxButtons.OK,
                BoxIcon = MessageBoxIcon.Information
            };
            box.FormClosed += (s, c) =>
            {
                //MessageLoopUtil.Stop();
            };
            box.Show(DefaultOwner);
            //MessageLoopUtil.Start();

            Application.DoEvents();
        }


        /// <summary>
        /// 绑定父级窗体
        /// </summary>
        /// <param name="windowTitle"></param>
        public static void LinkToWindowByItsTitle(string windowTitle)
        {
            DefaultOwner = FindWindowByTitle(windowTitle);
        }


        static IWin32Window FindWindowByTitle(string windowTitle)
        {
            IntPtr windowHandle = Win32.FindWindow(null, windowTitle);

            IWin32Window ownerWindow = null;
            if (windowHandle != IntPtr.Zero)
            {
                ownerWindow = new WindowWrapper(windowHandle);
            }

            return ownerWindow;
        }

        #endregion

        #region 构造

        protected CustomMessageBox()
        {
            InitializeComponent();
            Load += CustomMessageBox_Load;
        }


        #endregion

        #region 字段

        private MessageBoxButtons boxButton;

        #endregion

        #region 属性

        [Category(Consts.DefaultCategory)]
        public string Caption
        {
            get
            {
                return CaptionLabel.Text;
            }
            set
            {
                Text = value;
                CaptionLabel.Text = value;
            }
        }

        [Category(Consts.DefaultCategory)]
        public string Content
        {
            get => lblContent.Text;
            set
            {
                lblContent.Text = value;
            }
        }

        [Category(Consts.DefaultCategory)]
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

        /// <summary>
        /// 按钮类型
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public MessageBoxButtons BoxButton
        {
            get
            {
                return boxButton;
            }
            set
            {
                boxButton = value;
                ApplyButtons();
            }
        }


        /// <summary>
        /// 消息图标
        /// </summary>
        [Category(Consts.DefaultCategory)]
        public MessageBoxIcon BoxIcon
        {
            get;
            private set;
        }

        #endregion

        #region 方法



        /// <summary>
        /// 隐藏全部按钮
        /// </summary>
        private void HideAllButtons()
        {
            pnlControlArea.Controls.OfType<Button>().ToList().ForEach((x) => { x.Visible = false; });
        }

        /// <summary>
        /// 按需显示按钮
        /// </summary>
        private void ApplyButtons()
        {
            HideAllButtons();
            switch (BoxButton)
            {
                case MessageBoxButtons.OK:
                    btnOK.Visible = true;

                    pnlControlArea.Controls.Remove(btnCancel);
                    pnlControlArea.Controls.Remove(btnYes);
                    pnlControlArea.Controls.Remove(btnNo);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    btnCancel.Visible = true;
                    pnlControlArea.Controls.Remove(btnOK);
                    break;
                case MessageBoxButtons.OKCancel:
                    btnOK.Visible = true;
                    btnCancel.Visible = true;

                    pnlControlArea.Controls.Remove(btnYes);
                    pnlControlArea.Controls.Remove(btnNo);

                    break;
                case MessageBoxButtons.YesNo:
                    btnYes.Visible = true;
                    btnNo.Visible = true;


                    pnlControlArea.Controls.Remove(btnOK);
                    pnlControlArea.Controls.Remove(btnCancel);
                    break;
            }

            pnlControlArea.PerformLayout();
            pnlControlArea.Location = new Point((pnlStrechSpace.Width - pnlControlArea.Width - pnlStrechSpace.Padding.Right), pnlControlArea.Top);
        }

        /// <summary>
        /// 设置主题
        /// </summary>
        private void ApplyTheme()
        {
            Color primaryThemeColor = SystemColors.Control;
            Color accentColour = Color.DodgerBlue;
            Color textColor = SystemColors.ControlText;
            Image primaryIcon = CustomMessageBoxResource.information;

            switch (BoxIcon)
            {
                case MessageBoxIcon.Information:
                    {
                        primaryThemeColor = Color.FromArgb(255, 239, 241, 250);
                        accentColour = Color.FromArgb(56, 80, 183);
                        textColor = accentColour;
                    }
                    break;
                case MessageBoxIcon.Error:
                    {
                        primaryThemeColor = Color.FromArgb(254, 236, 240);
                        accentColour = Color.FromArgb(204, 15, 53);
                        textColor = accentColour;
                        primaryIcon = CustomMessageBoxResource.error;
                    }
                    break;
                case MessageBoxIcon.Question:
                    {
                        primaryThemeColor = Color.FromArgb(255, 255, 250, 235);
                        accentColour = Color.FromArgb(148, 108, 0);
                        textColor = accentColour;
                        primaryIcon = CustomMessageBoxResource.question;
                    }
                    break;
                case MessageBoxIcon.Warning:
                    {
                        primaryThemeColor = Color.FromArgb(255, 224, 138);
                        accentColour = Color.FromArgb(76, 67, 41);
                        textColor = accentColour;
                        primaryIcon = CustomMessageBoxResource.warning;
                    }
                    break;
                case MessageBoxIcon.None:
                default:
                    if (BoxIcon == MessageBoxIcon.Exclamation)
                    {
                        primaryThemeColor = Color.FromArgb(241, 70, 104);
                        accentColour = primaryThemeColor;
                        textColor = accentColour;

                        primaryIcon = CustomMessageBoxResource.exclamation;
                    }
                    else if (BoxIcon == MessageBoxIcon.Asterisk)
                    {
                        primaryThemeColor = Color.FromArgb(152, 154, 186);
                        accentColour = primaryThemeColor;
                        textColor = accentColour;

                        primaryIcon = CustomMessageBoxResource.exclamation;
                    }
                    else if (BoxIcon == MessageBoxIcon.Stop)
                    {
                        primaryThemeColor = Color.FromArgb(241, 70, 104);
                        accentColour = primaryThemeColor;
                        textColor = accentColour;
                        primaryIcon = CustomMessageBoxResource.exclamation;
                    }
                    else
                    {
                        primaryThemeColor = Color.FromArgb(241, 241, 241);
                        accentColour = Color.FromArgb(74, 74, 74);
                        textColor = accentColour;
                        primaryIcon = CustomMessageBoxResource.information;
                    }
                    break;
            }


            pnlStrechSpace.BackColor = primaryThemeColor;
            lblContent.ForeColor = textColor;
            IconControl.Image = primaryIcon;
            ColourIcon(primaryIcon, accentColour);

            this.BorderColor = accentColour;
        }

        /// <summary>
        /// 设置消息图标的颜色
        /// <!-- 本来想使用SVG, 但要引用大量的代码, 选了几个简单图标, 根据主题场景来更改颜色更简单些. -->
        /// </summary>        
        /// <param name="sourceImage"></param>
        /// <param name="targetColor"></param>
        private void ColourIcon(Image sourceImage, Color targetColor)
        {
            Bitmap image = new Bitmap(sourceImage);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor.A > 0)
                    {
                        Color newColor = Color.FromArgb(pixelColor.A, targetColor.R, targetColor.G, targetColor.B);
                        image.SetPixel(x, y, newColor);
                    }
                }
            }

            IconControl.Image = image;
        }

        #endregion

        #region 事件处理

        private void CustomMessageBox_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            // 避免消息框没有前置显示
            if (this.Owner == null)
            {
                Win32.SetForegroundWindow(this.Handle);
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (!Modal)
            {
                Close();
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            if (!Modal)
            {
                Close();
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            if (!Modal)
            {
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            if (!Modal)
            {
                Close();
            }
        }


        #endregion

    }

}
