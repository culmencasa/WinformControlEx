using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils.UI;

namespace FormExCore
{
    public partial class OcnMessageBox : OcnForm
    {
        #region 静态方法

        /// <summary>
        /// 在当前窗体前显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult ShowMessage(string caption, string content, MessageBoxIcon icon)
        {
            // 找到当前活动窗体, 指定为Owner(在Owner之上激活)
            Form owner = FormManager.TryGetLatestActiveForm();
            if (owner != null)
            {
                // 判断是否需要线程回调
                if (owner.InvokeRequired)
                {
                    return owner.Invoke(()=>ShowMessage(content, caption, icon));
                }
                else
                {
                    OcnMessageBox box = new OcnMessageBox()
                    {
                        Caption = caption,
                        Content = content,
                        BoxButton = MessageBoxButtons.OK,
                        BoxIcon = icon
                    };
                    return box.ShowDialog(owner);
                }
            }
            else
            {
                // 如果没有办法找到当前活动窗体, 则不指定模式对话框的Owner(可能会丢失焦点)
                OcnMessageBox box = new OcnMessageBox()
                {
                    Caption = caption,
                    Content = content,
                    BoxButton = MessageBoxButtons.OK,
                    BoxIcon = icon
                };
                return box.ShowDialog();
            }
        }

        /// <summary>
        /// 在当前窗体前显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowMessage(string content)
        {
            string caption = "";
            return OcnMessageBox.ShowMessage(caption, content, MessageBoxIcon.Warning);            
        }

        /// <summary>
        /// 在当前窗体前显示确认消息框
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowConfirm(string caption, string content)
        {
            Form owner = FormManager.TryGetLatestActiveForm();
            if (owner != null)
            {
                if (owner.InvokeRequired)
                {
                    return owner.Invoke(() => ShowConfirm(caption, content));
                }
                else
                {
                    OcnMessageBox box = new OcnMessageBox()
                    {
                        Caption = caption,
                        Content = content,
                        BoxButton = MessageBoxButtons.YesNo,
                        BoxIcon = MessageBoxIcon.Question
                    };
                    return box.ShowDialog(owner);
                }
            }
            else
            {
                OcnMessageBox box = new OcnMessageBox()
                {
                    Caption = caption,
                    Content = content,
                    BoxButton = MessageBoxButtons.YesNo,
                    BoxIcon = MessageBoxIcon.Question
                };
                return box.ShowDialog();
            }
        }


        #endregion

        #region 构造

        public OcnMessageBox()
        {
            InitializeComponent();

            

            ControlBorder cb1 = new ControlBorder(btnOK);
            cb1.KeepBorderOnFocus = true;

            ControlBorder cb = new ControlBorder(btnClose);
            cb.Timing = ControlBorder.DrawBorderTiming.MouseDown;
            cb.KeepBorderOnFocus = true;
            //cb.DrawBorderCondition = () =>
            //{
            //    object keepStatus = btnClose.Tag;
            //    if (keepStatus == null)
            //    {
            //        if ((this.btnClose.Bms & ImageButton.ButtonMouseStatus.Pressed) == ImageButton.ButtonMouseStatus.Pressed)
            //        {
            //            btnClose.Tag = true;
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        if ((bool)keepStatus)
            //        {
            //            if ((this.btnClose.Bms & ImageButton.ButtonMouseStatus.Pressed) == ImageButton.ButtonMouseStatus.Pressed)
            //            {
            //                btnClose.Tag = false;
            //                return true;
            //            }
            //        }
            //        else
            //        {
            //            if ((this.btnClose.Bms & ImageButton.ButtonMouseStatus.Pressed) == ImageButton.ButtonMouseStatus.Pressed)
            //            {
            //                btnClose.Tag = true;
            //                return true;
            //            }
            //        }
            //    }

            //    return false;
            //};
            //cb.Apply();

            //cb.AddEventHandler("MouseDown", new MouseEventHandler((o, e) =>
            //{
            //    cb.DrawBorder();
            //}));

            //btnClose.ShowFocusLine = true;

        }

        #endregion

        #region 字段

        private MessageBoxButtons _boxButton;
        #endregion
        #region 公开属性

        public string Caption
        {
            get
            {
                return TitleText;
            }
            set
            {
                TitleText = value;
            }
        }

        public string Content
        {
            get
            {
                return lblContent.Text;
            }
            set
            {
                lblContent.Text = value;
            }
        }

        public MessageBoxButtons BoxButton
        {
            get
            {
                return _boxButton;
            }
            set
            {
                _boxButton = value;
                ApplyButtons();
            }
        }

        public MessageBoxIcon BoxIcon
        {
            get;
            set;
        }

        public Size DesignSize
        {
            get
            {
                return new Size(396, 201);
            }

        }

        #endregion

        #region 事件处理

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Modal)
            {
                DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void OcnMessageBox_Load(object sender, EventArgs e)
        {
            ApplyIcon();
        }

        #endregion


        #region 重写的方法

        protected override void FillTitleBarBackground(Graphics g)
        {
            if (!string.IsNullOrEmpty(Caption))
            {
                base.FillTitleBarBackground(g);
            }
        }

        protected override void ApplyDanger()
        {
            base.ApplyDanger();

            boxActionSeparator.LineColor = Presets.DangerColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }

        protected override void ApplyDark()
        {
            base.ApplyDark();

            boxActionSeparator.LineColor = Presets.DarkColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }

        protected override void ApplyInfo()
        {
            base.ApplyInfo();

            boxActionSeparator.LineColor = Presets.InfoColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }

        protected override void ApplyLight()
        {
            base.ApplyLight();

            boxActionSeparator.LineColor = Presets.LightColor ;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }

        protected override void ApplyPrimary()
        {
            base.ApplyPrimary();

            boxActionSeparator.LineColor = Presets.PrimaryColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }


        protected override void ApplySecondary()
        {
            base.ApplySecondary();

            boxActionSeparator.LineColor = Presets.SecondaryColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }

        protected override void ApplySuccess()
        {
            base.ApplySuccess();

            boxActionSeparator.LineColor = Presets.SuccessColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }

        protected override void ApplyWarning()
        {
            base.ApplyWarning();

            boxActionSeparator.LineColor = Presets.WarningColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;
        }


        #endregion

        #region 私有方法

        private void HideAllButtons()
        {
            pnlControlArea.Controls.OfType<OcnButton>().ToList().ForEach((x) => { x.Visible = false; });
        }

        private void ApplyButtons()
        {
            HideAllButtons();
            switch (BoxButton)
            {
                case MessageBoxButtons.OK:
                    btnOK.Visible = true;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    btnCancel.Visible = true;
                    break;
                case MessageBoxButtons.OKCancel:
                    btnOK.Visible = true;
                    btnCancel.Visible = true;
                    break;
                case MessageBoxButtons.YesNo:
                    btnYes.Visible = true;
                    btnNo.Visible = true;
                    break;
            }
        }

        private void ApplyIcon()
        {
            switch (BoxIcon)
            {
                case MessageBoxIcon.Information:
                    Theme = OcnThemes.Light;
                    break;
                case MessageBoxIcon.Error:
                    Theme = OcnThemes.Danger;
                    break;
                case MessageBoxIcon.Question:
                    Theme = OcnThemes.Info;
                    break;
                case MessageBoxIcon.Warning:
                    Theme = OcnThemes.Warning;
                    break;
                case MessageBoxIcon.None:
                    Theme = OcnThemes.Secondary;
                    break;
                default:
                    if (BoxIcon == MessageBoxIcon.Exclamation)
                    {
                        Theme = OcnThemes.Warning;
                    }
                    else if (BoxIcon == MessageBoxIcon.Asterisk)
                    {
                        Theme = OcnThemes.Primary;
                    }
                    else if (BoxIcon == MessageBoxIcon.Stop || BoxIcon == MessageBoxIcon.Hand)
                    {
                        Theme = OcnThemes.Danger;
                    }
                    else
                    {
                        Theme = OcnThemes.Primary;
                    }
                    break;
            }
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
