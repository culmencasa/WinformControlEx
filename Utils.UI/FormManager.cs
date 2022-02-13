using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace System.Windows.Forms
{
    /// <summary>
    /// 窗体管理器
    /// </summary>
    public static class FormManager
    {
        // 窗体缓存
        static List<Form> FixedSingleFormCache = new List<Form>();

        #region 公开方法
        
        /// <summary>
        /// 获取一个与指定条件匹配的窗体对象，如果未找到默认会创建一个(非Mdi)
        /// </summary>
        /// <typeparam name="TForm">窗体类型</typeparam>
        /// <param name="createIfNotFound">如果主动创建</param>
        /// <param name="keySelector">要查找的条件表达式</param>
        /// <param name="parameters">实例化的参数</param>
        /// <returns></returns>
        public static TForm Single<TForm>(bool createIfNotFound, Func<TForm, bool> keySelector, params object[] parameters) where TForm : Form, new()
        {
            TForm instance = null;

            try
            {
                foreach (Form form in FormManager.FixedSingleFormCache)
                {
                    if (form is TForm)
                    {
                        if (form.IsDisposed)
                            continue;

                        TForm comparingTarget = form as TForm;
                        if (keySelector != null)
                        {
                            if (keySelector(comparingTarget))
                                instance = comparingTarget;
                            else
                                continue;
                        }
                        else
                        {
                            if (comparingTarget != null)
                                instance = comparingTarget;
                        }
                        break;
                    }
                }


                if (createIfNotFound)
                {
                    if (instance == null)
                    {
                        if (parameters.Length > 0)
                        {
                            instance = Activator.CreateInstance(typeof(TForm), parameters) as TForm;
                            instance.GotFocus += Form_GotFocus;
                            instance.Activated += Form_Activated;
                        }
                        else
                        {
                            instance = new TForm();
                            instance.GotFocus += Form_GotFocus;
                            instance.Activated += Form_Activated;
                        }
                        instance.FormClosed += delegate(object sender, FormClosedEventArgs e)
                        {
                            FormManager.FixedSingleFormCache.Remove(instance);
                        };
                        FormManager.FixedSingleFormCache.Add(instance);
                    }
                }

            }
            catch
            {
                throw;
            }

            return instance;
        }

        public static TForm Single<TForm>() where TForm : Form, new()
        {
            return Single<TForm>(true, null);
        }


        public static TForm Single<TForm>(object constructorArguments) where TForm : Form, new()
        {
            if (constructorArguments == null)
            {
                return Single<TForm>(true, null);
            }

            if (constructorArguments is object[])
            {
                return Single<TForm>(true, null, (object[])constructorArguments);
            }

            return Single<TForm>(true, null, new object[] { constructorArguments });
        }

        /// <summary>
        /// 将所有窗体设置为显示或隐藏
        /// </summary>
        /// <param name="isShow"></param>
        public static void ToggleVisibilityAll(bool isShow)
        {
            foreach (Form item in FixedSingleFormCache)
            {
                if (isShow)
                {
                    item.Show();
                    item.Activate();
                }
                else
                {
                    item.Hide();
                }
            }
        }

        /// <summary>
        /// 获取指定控件下的子控件集合中与指定条件匹配的窗体对象
        /// </summary>
        /// <typeparam name="TForm">要查询的窗体类型</typeparam>
        /// <param name="parent">作为父窗口的控件</param>
        /// <param name="keySelector">查询条件</param>
        /// <returns></returns>
        public static TForm Get<TForm>(this Control parent, Func<TForm, bool> keySelector) where TForm : Form, new()
        {
            TForm instance = null;

            foreach (TForm form in parent.Controls.OfType<TForm>())
            {
                if (form.IsDisposed)
                    continue;

                TForm comparingTarget = form as TForm;
                if (keySelector != null)
                {
                    if (keySelector(comparingTarget))
                        instance = comparingTarget;
                }
                else
                {
                    if (comparingTarget != null)
                        instance = comparingTarget;
                }
                break;
            }


            if (instance == null)
            {
                instance = new TForm();
                instance.GotFocus += Form_GotFocus;
                instance.Activated += Form_Activated;
                instance.TopLevel = false;
                instance.Dock = DockStyle.Fill;
                instance.FormBorderStyle = FormBorderStyle.None;
                parent.Controls.Add(instance);
            }


            return instance;
        }

        /// <summary>
        /// 关闭所有窗体 
        /// </summary>
        public static void CloseAll()
        {
            for (int i = FixedSingleFormCache.Count - 1; i >= 0; i--)
            {
                Form instance = FixedSingleFormCache[i];
                
                //ClearEvents(instance, "FormClsing");
                //instance.Close();
                instance.Dispose();
            }
            FixedSingleFormCache.Clear();
        }


        /// <summary>
        /// 获取最近打开的顶层窗体(不支持MDI)
        /// </summary>
        /// <returns></returns>
        public static Form TryGetLastActiveForm()
        {
            // 方法1 (不一定有效)
            Form lastActiveForm = Form.ActiveForm;
            if (lastActiveForm == null)
            {
                // 方法2
                try
                {
                    IntPtr intPtr = Win32.GetActiveWindow();
                    lastActiveForm = Control.FromHandle(intPtr) as Form;
                }
                catch
                {
                    lastActiveForm = null;
                }


                // 方法3 (如果动态修改过窗体属性, 有可能找不到)
                //if (lastActiveForm == null)
                //{
                //    for (int index = Application.OpenForms.Count - 1; index >= 0; index--)
                //    {
                //        // 一般按索引的先后顺序, 最先遍历到的是最早打开的窗体, 例如index=0可能是主窗体.
                //        Form item = Application.OpenForms[index];

                //        if (item == null || !item.IsHandleCreated || item.IsDisposed)
                //            continue;

                //        if (item.TopLevel && item.Visible && item.Focused)
                //        {
                //            lastActiveForm = item;
                //            break;
                //        }
                //    }
                //}
            }


            return lastActiveForm;
        }


        #endregion



        #region 焦点所在窗体

        /// <summary>
        /// 获取父窗体
        /// </summary>
        /// <returns></returns>
        public static Form GetTopForm(Control parentControl)
        {
            if (parentControl == null)
                return null;

            Form topForm = null;
            while (topForm == null)
            {
                topForm = parentControl as Form;
                if (topForm == null)
                {
                    return GetTopForm(parentControl.Parent);
                }
            }

            return topForm;
        }


        public static Form FocusedForm { get; set; }

        private static void Form_Activated(object sender, EventArgs e)
        {
            FocusedForm = sender as Form;
        }

        static void Form_GotFocus(object sender, EventArgs e)
        {
            FocusedForm = sender as Form;
        }

        #endregion


    }
}
