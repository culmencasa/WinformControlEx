using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    class MessageBoxEx
    {
        /// <summary>
        /// 在当前窗体前显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult ShowMessage(string content, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            // 找到当前活动窗体, 指定为Owner(在Owner之上激活)
            Form currentForm = Form.ActiveForm;
            if (currentForm == null)
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.TopLevel && item.Visible)
                    {
                        currentForm = item;
                        break;
                    }
                }
            }

            Form owner = currentForm;
            if (owner != null)
            {
                // 判断是否需要线程回调
                if (owner.InvokeRequired)
                {
                    return (DialogResult)owner.Invoke((Func<DialogResult>)delegate
                    {
                        return MessageBox.Show(owner, content, caption, buttons, icon);
                    });
                }
                else
                {
                    return MessageBox.Show(owner, content, caption, buttons, icon);
                }
            }
            else
            {
                // 如果没有办法找到当前活动窗体, 则不指定模式对话框的Owner(可能会丢失焦点)
                return MessageBox.Show(content, caption, buttons, icon);
            }
        }

        /// <summary>
        /// 在当前窗体前显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowMessage(string content)
        {
            string caption = "提醒";

            CultureInfo ci = CultureInfo.CurrentUICulture;
            if (ci.Name.Contains("en"))
            { 
                caption = "Information";
            }
            return MessageBoxEx.ShowMessage(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 在当前窗体前显示确认消息框
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowConfirm(string caption, string content)
        {
            Form currentForm = Form.ActiveForm;
            //if (currentForm == null)
            //{
            //    currentForm = TopLayerForm.FocusedForm;
            //}

            if (currentForm != null)
            {
                if (currentForm.InvokeRequired)
                {
                    return (DialogResult)currentForm.Invoke((Func<DialogResult>)delegate
                    {
                        return MessageBox.Show(currentForm, content, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    });
                }
                else
                {
                    return MessageBox.Show(currentForm, content, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
            }
            else
            {
                return MessageBox.Show(content, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
        }

        public static void ShowExceptionDialog()
        {
            //todo:
        }
    }
}
