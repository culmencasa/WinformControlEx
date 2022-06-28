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
    public partial class OcnMessageBox : Form
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

            this.MouseDown += OcnMessageBox_MouseDown;
            

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
        private OcnThemes _theme;


        #endregion

        #region 公开属性

        [Category("Look")]
        [Browsable(true)]
        public Color TitleBarBackColor
        {
            get
            {
                return TitleBar.BackColor;
            }
            set
            {
                TitleBar.BackColor = value;
            }
        }


        [Category("Custom")]
        public string Caption
        {
            get
            {
                return lblCaption.Text;
            }
            set
            {
                lblCaption.Text = value;
                this.Text = value;
            }
        }

        [Category("Custom")]
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

        [Category("Custom")]
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

        [Category("Custom")]
        public MessageBoxIcon BoxIcon
        {
            get;
            set;
        }

        [Category("Custom")]
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


            var framer = new DropShadow(this);
            framer.BorderRadius = 0;
            framer.ShadowRadius = 0;
            framer.BorderColor = ColorEx.DarkenColor(this.BackColor, 40);
            framer.ShadowOpacity = 1f;
            framer.Redraw(true);
            framer.Show();
        }

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


        private void OcnMessageBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.MakeMoves();
        }

        #endregion


        #region 主题相关

        /// <summary>
        /// 主题
        /// </summary>
        [Category("Look")]
        [Browsable(true)]
        public OcnThemes Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                bool changing = _theme != value;
                _theme = value;

                if (changing)
                {
                    OnThemeChanged();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OceanPresets Presets
        {
            get;
            private set;
        } = OceanPresets.Instance;

        protected bool ThemeApplied { get; set; }


        protected virtual void ApplyPrimary()
        {
            BackColor = Color.White;
            ForeColor = Presets.PrimaryColor;
            TitleBarBackColor = Color.FromArgb(108, 17, 150);


            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;

            boxActionSeparator.LineColor = Presets.PrimaryColor;
            btnClose.NormalColor = Color.White;
            btnClose.HoverColor = Color.Black;
            lblCaption.ForeColor = Color.White;
        }


        protected virtual void ApplySecondary()
        {
            BackColor = Color.White;
            ForeColor = Presets.SecondaryColor;
            TitleBarBackColor = Presets.SecondaryColor;

            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;

            boxActionSeparator.LineColor = Presets.SecondaryColor;
            btnClose.NormalColor = Color.White;
            btnClose.HoverColor = Color.Black;
            lblCaption.ForeColor = Color.White;
        }

        protected virtual void ApplySuccess()
        {
            BackColor = Color.White;
            ForeColor = Presets.SuccessColor;
            TitleBarBackColor = Presets.SuccessColor;

            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;

            boxActionSeparator.LineColor = Presets.SuccessColor;
            btnClose.NormalColor = Color.Black;
            btnClose.HoverColor = Color.White;
            lblCaption.ForeColor = Color.White;
        }

        protected virtual void ApplyDanger()
        {
            BackColor = Color.White;
            ForeColor = Presets.DangerColor;
            TitleBarBackColor = Presets.DangerColor;

            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;

            boxActionSeparator.LineColor = Presets.DangerColor;
            btnClose.NormalColor = Color.White;
            btnClose.HoverColor = Color.Black;
            lblCaption.ForeColor = Color.White;
        }

        protected virtual void ApplyWarning()
        {
            BackColor = Color.White;
            ForeColor = Presets.WarningColor;
            TitleBarBackColor = Presets.WarningColor;


            boxActionSeparator.LineColor = Presets.WarningColor;
            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;

            TitleBar.BackColor = Presets.WarningColor;
            btnClose.NormalColor = Color.Black;
            btnClose.HoverColor = Color.White;
            lblCaption.ForeColor = Color.White;
        }

        protected virtual void ApplyInfo()
        {
            BackColor = Color.White;
            ForeColor = Presets.InfoColor;
            TitleBarBackColor = Presets.InfoColor;

            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;

            boxActionSeparator.LineColor = Presets.InfoColor;
            btnClose.NormalColor = Color.Black;
            btnClose.HoverColor = Color.White;
            lblCaption.ForeColor = Color.White;
        }

        protected virtual void ApplyLight()
        {
            BackColor = Color.White;
            ForeColor = Color.Black;
            TitleBarBackColor = Presets.LightColor;

            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;

            boxActionSeparator.LineColor = Presets.LightColor;
            btnClose.NormalColor = Color.Black;
            btnClose.HoverColor = Color.White;
            lblCaption.ForeColor = Color.Black;
        }


        protected virtual void ApplyDark()
        {
            BackColor = Color.White;
            ForeColor = Presets.DarkColor;
            TitleBarBackColor = Presets.DarkColor;

            btnYes.Theme = Theme;
            btnNo.Theme = Theme;
            btnCancel.Theme = Theme;
            btnOK.Theme = Theme;

            boxActionSeparator.LineColor = Presets.DarkColor;
            TitleBar.BackColor = Presets.DarkColor;
            btnClose.NormalColor = Color.White;
            btnClose.HoverColor = Color.Black;
            lblCaption.ForeColor = Color.White;
        }


        #endregion

        #region 私有方法

        private void HideAllButtons()
        {
            pnlControlArea.Controls.OfType<OcnButton>().ToList().ForEach((x) => { x.Visible = false; });
        }

        protected virtual void OnThemeChanged()
        {
            // 防止在调用此方法时, 子类控件还未完全实例化而报空引用异常
            if (!this.IsHandleCreated)
            {
                EventHandler? onLoadRegister = null;
                onLoadRegister = delegate (object sender, EventArgs e)
                {
                    OnThemeChanged();
                    if (onLoadRegister != null)
                    {
                        this.HandleCreated -= onLoadRegister;
                    }
                };
                this.HandleCreated += onLoadRegister;
                return;
            }


            switch (Theme)
            {
                case OcnThemes.Primary:
                    ApplyPrimary();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Secondary:
                    ApplySecondary();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Success:
                    ApplySuccess();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Danger:
                    ApplyDanger();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Warning:
                    ApplyWarning();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Info:
                    ApplyInfo();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Light:
                    ApplyLight();
                    ThemeApplied = true;
                    break;
                case OcnThemes.Dark:
                    ApplyDark();
                    ThemeApplied = true;
                    break;
                default:

                    ThemeApplied = false;
                    break;
            }
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

    }
}
