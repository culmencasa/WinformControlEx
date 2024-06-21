using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    /// <summary>
    /// 消息框
    /// <para>相比MessageBox加上了Owner, 以避免弹框后置</para> 
    /// </summary>
    public class MessageBoxEx
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
            Form owner = FormManager.TryGetLatestActiveForm();

            return ShowMessage(owner, content, caption, buttons, icon);
        }


        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="content"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult ShowMessage(Form owner, string content, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (owner != null && !owner.IsDisposed)
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
        /// 在当前窗体前显示确认消息框
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowConfirm(string content, string caption)
        {
            Form owner = FormManager.TryGetLatestActiveForm();

            return ShowMessage(owner, content, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 在当前窗体前显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DialogResult ShowInformation(string content)
        {
            string caption = "提示";

            CultureInfo ci = CultureInfo.CurrentUICulture;
            if (ci.Name.Contains("en"))
            {
                caption = "Information";
            }
            return ShowMessage(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DialogResult ShowWarning(string content)
        {
            string caption = "警告";

            CultureInfo ci = CultureInfo.CurrentUICulture;
            if (ci.Name.Contains("en"))
            {
                caption = "Warning";
            }
            return ShowMessage(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        public static DialogResult ShowError(string content)
        {
            string caption = "错误";

            CultureInfo ci = CultureInfo.CurrentUICulture;
            if (ci.Name.Contains("en"))
            {
                caption = "Error";
            }
            return ShowMessage(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
