using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

using Sundata.Utils;

namespace Sundata.Utils.UI
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
        public static TForm Single<TForm>(bool createIfNotFound = true, Func<TForm, bool> keySelector = null, params object[] parameters) where TForm : Form, new()
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
                        }
                        else
                        {
                            instance = new TForm();
                        }
                        instance.FormClosed += delegate(object sender, FormClosedEventArgs e)
                        {
                            FormManager.FixedSingleFormCache.Remove(instance);
                        };
                        FormManager.FixedSingleFormCache.Add(instance);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return instance;
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="isShow"></param>
        public static void ShowWindow(bool isShow)
        {
            foreach (Form item in FixedSingleFormCache)
            {
                if (isShow)
                {
                    item.Show();
                    item.Activate();
                }
                else
                    item.Hide();
            }
        }

        /// <summary>
        /// 获取指定控件下的子控件集合中与指定条件匹配的窗体对象
        /// </summary>
        /// <typeparam name="TForm">要查询的窗体类型</typeparam>
        /// <param name="parent">作为父窗口的控件</param>
        /// <param name="keySelector">查询条件</param>
        /// <returns></returns>
        public static TForm Get<TForm>(this Control parent, Func<TForm, bool> keySelector = null) where TForm : Form, new()
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
                instance.TopLevel = false;
                instance.Dock = DockStyle.Fill;
                instance.FormBorderStyle = FormBorderStyle.None;
                parent.Controls.Add(instance);
            }


            return instance;
        }

        public static void CloseAll()
        {
            for (int i = FixedSingleFormCache.Count - 1; i >= 0; i--)
            {
                Form instance = FixedSingleFormCache[i];
                
                //ClearEvents(instance, "FormClsing");
                //instance.Close();
                instance.Dispose();
                instance = null;
            }
            FixedSingleFormCache.Clear();
        }

        #endregion

        static Delegate[] GetClosingEventInvocationList(Form target)
        {
            Delegate[] PaintEventInvocationList = null;

            Control ParentForm = target;
            if (ParentForm == null)
                return PaintEventInvocationList;

            string PaintEventKeyName = "EventClosing";

            PropertyInfo FormEventProperty; // 窗体事件属性信息
            EventHandlerList FormEventPropertyInstance; // 窗体事件属性对象
            FieldInfo PaintEventKeyField;   // Paint事件对应的键值字段信息
            Object PaintEventKeyInstance; // Paint事件对应的键值对象
            Delegate PaintEventDelegate;

            Type FormType = typeof(Control);
            Type BaeTypeOfPaintEvent = typeof(Control);


            FormEventProperty = FormType.GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            FormEventPropertyInstance = FormEventProperty.GetValue(ParentForm, null) as EventHandlerList;

            PaintEventKeyField = BaeTypeOfPaintEvent.GetField(PaintEventKeyName, BindingFlags.Static | BindingFlags.NonPublic);

            PaintEventKeyInstance = PaintEventKeyField.GetValue(ParentForm);
            PaintEventDelegate = FormEventPropertyInstance[PaintEventKeyInstance];

            if (PaintEventDelegate != null)
                PaintEventInvocationList = PaintEventDelegate.GetInvocationList();

            return PaintEventInvocationList;
        }


        public static void ClearEvents(this object ctrl, string eventName = "_EventAll")
        {
            if (ctrl == null) return;
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Static;
            EventInfo[] events = ctrl.GetType().GetEvents(bindingFlags);
            if (events == null || events.Length < 1) return;

            for (int i = 0; i < events.Length; i++)
            {
                try
                {
                    EventInfo ei = events[i];
                    //只删除指定的方法，默认是_EventAll，前面加_是为了和系统的区分，防以后雷同
                    //if (eventName != "_EventAll" && ei.Name != eventName) continue;

                    /********************************************************
                     * class的每个event都对应了一个同名(变了，前面加了Event前缀)的private的delegate类
                     * 型成员变量（这点可以用Reflector证实）。因为private成
                     * 员变量无法在基类中进行修改，所以为了能够拿到base 
                     * class中声明的事件，要从EventInfo的DeclaringType来获取
                     * event对应的成员变量的FieldInfo并进行修改
                     ********************************************************/
                    FieldInfo fi = ei.DeclaringType.GetField("Event" + ei.Name, bindingFlags);
                    if (fi != null)
                    {
                        // 将event对应的字段设置成null即可清除所有挂钩在该event上的delegate
                        fi.SetValue(ctrl, null);
                    }
                }
                catch { }
            }
        }

        static void ClearEvent2(Control pControl, string pEventName)
        {

            if (pControl == null) return;

            if (string.IsNullOrEmpty(pEventName)) return;



            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public

                | BindingFlags.Static | BindingFlags.NonPublic;//筛选

            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;

            Type controlType = typeof(System.Windows.Forms.Control);

            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);

            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(pControl, null);//事件列表

            FieldInfo fieldInfo = (typeof(Control)).GetField("Event" + pEventName, mFieldFlags);

            Delegate d = eventHandlerList[fieldInfo.GetValue(pControl)];



            if (d == null) return;

            EventInfo eventInfo = controlType.GetEvent(pEventName);



            foreach (Delegate dx in d.GetInvocationList())

                eventInfo.RemoveEventHandler(pControl, dx);//移除已订阅的pEventName类型事件



        }


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
        

    }
}
