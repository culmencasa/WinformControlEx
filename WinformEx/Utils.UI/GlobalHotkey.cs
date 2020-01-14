using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sundata.Utils.UI
{
    public class GlobalHotkey
    {
        private int modifier;
        private int key;
        private IntPtr hWnd;
        private int id;


        // 该字段用于注册热键后, 在注册的窗体上接收WndProc消息
        public const int WM_Hotkey_MsgId = 0x0312;

        public GlobalHotkey(int modifier, Keys key, Form form)
        {
            this.modifier = modifier;
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
        }
        
        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }

        public bool Unregiser()
        {
            return UnregisterHotKey(hWnd, id);
        }

        public override int GetHashCode()
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
