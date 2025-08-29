using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Utils.UI
{
    /// <summary>
    /// 热键管理器
    /// </summary>
    public class HotkeyManager : IDisposable
    {
        #region 常量
        
        // 当全局热键被激活时，系统会向对应的窗口发送 WM_HOTKEY 消息，其消息代码为 0x0312
        private const int WM_HOTKEY = 0x0312;

        #endregion

        #region 静态变量


        private static readonly Dictionary<int, Action> HotkeyActions = new Dictionary<int, Action>();

        private readonly HashSet<int> RegisteredHotkeys = new HashSet<int>();

        #endregion

        #region 字段
         
        private IntPtr _hWnd;

        #endregion

        #region 属性

        public bool IsDisposed
        {
            get;
            private set;
        }

        #endregion


        #region 构造

        public HotkeyManager(IntPtr hWnd)
        {
            _hWnd = hWnd;
        }

        #endregion

        #region 析构

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // 注销所有注册的热键  
                foreach (var hotkeyId in RegisteredHotkeys)
                {
                    Win32.UnregisterHotKey(_hWnd, hotkeyId);
                    HotkeyActions.Remove(hotkeyId);
                }
                RegisteredHotkeys.Clear();
                IsDisposed = true;
            }
        }

        #endregion

        #region 方法


        public bool RegisterHotKey(Keys key, KeyModifiers modifiers, Action action)
        {
            // 使用 key 和 modifiers 的组合作为唯一标识  
            int hotkeyId = (int)key | ((int)modifiers << 16); 

            // 如果之前已注册此热键，则先注销  
            if (RegisteredHotkeys.Contains(hotkeyId))
            {
                Win32.UnregisterHotKey(_hWnd, hotkeyId);
                HotkeyActions.Remove(hotkeyId);
                RegisteredHotkeys.Remove(hotkeyId); 
            }

            // 注册新的热键  
            if (Win32.RegisterHotKey(_hWnd, hotkeyId, (uint)modifiers, (uint)key))
            {
                HotkeyActions[hotkeyId] = action; 
                RegisteredHotkeys.Add(hotkeyId);  
                return true; 
            }
            return false;  
        }


        public void ProcessHotkeyMessage(Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                var hotkeyId = (int)m.WParam;

                if (HotkeyActions.ContainsKey(hotkeyId))
                {
                    var action = HotkeyActions[hotkeyId];
                    if (action != null)
                    {
                        action.Invoke();
                    } 
                }
            }
        }

        #endregion


    }

    [Flags]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }
}
