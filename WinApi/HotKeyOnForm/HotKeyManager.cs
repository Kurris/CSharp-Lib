using System.Windows.Forms;

namespace CSharpLib.WinApi.Form
{
    /// <summary>
    /// 热键管理类
    /// </summary>
    public class HotkeyManager : HotKeyManagerBase
    {
        #region Singleton Implementation

        private static HotkeyManager _mInstance = null;
        public static HotkeyManager Instance
        {
            get
            {
                if (_mInstance == null)
                {
                    _mInstance = new HotkeyManager();
                }
                return _mInstance;
            }
        }

        #endregion

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private HotkeyManager()
        {
            MessageWindow _mmessageWindow = new MessageWindow(this);
            SetHwnd(_mmessageWindow.Handle);
        }

        /// <summary>
        /// 添加或替换热键
        /// </summary>
        /// <param name="name">热键Name</param>
        /// <param name="keys">用户按键</param>
        /// <param name="noRepeat">是否不能重复</param>
        /// <param name="handler">热键事件</param>
        public void AddOrReplace(string name, Keys keys, bool noRepeat, HotKeyEventHandler handler)
        {
            var flags = GetFlags(keys, noRepeat);
            var vk = unchecked((uint)(keys & ~Keys.Modifiers));
            AddOrReplace(name, vk, flags, handler);
        }

        /// <summary>
        /// 添加或替换热键
        /// </summary>
        /// <param name="name">热键Name</param>
        /// <param name="keys">用户按键</param>
        /// <param name="handler">热键事件</param>
        public void AddOrReplace(string name, Keys keys, HotKeyEventHandler handler)
        {
            AddOrReplace(name, keys, false, handler);
        }

        /// <summary>
        /// 取按键标志
        /// </summary>
        /// <param name="hotkey">用户定义的热键</param>
        /// <param name="noRepeat">是否重复</param>
        /// <returns></returns>
        private HotkeyFlags GetFlags(Keys hotkey, bool noRepeat)
        {
            var noMod = hotkey & ~Keys.Modifiers;
            var flags = HotkeyFlags.None;
            if (hotkey.HasFlag(Keys.Alt))
                flags |= HotkeyFlags.Alt;
            if (hotkey.HasFlag(Keys.Control))
                flags |= HotkeyFlags.Control;
            if (hotkey.HasFlag(Keys.Shift))
                flags |= HotkeyFlags.Shift;
            if (noMod == Keys.LWin || noMod == Keys.RWin)
                flags |= HotkeyFlags.Windows;
            if (noRepeat)
                flags |= HotkeyFlags.NoRepeat;
            return flags;
        }

        private class MessageWindow : ContainerControl
        {
            private readonly HotkeyManager _mhotkeyManager;

            public MessageWindow(HotkeyManager hotkeyManager)
            {
                _mhotkeyManager = hotkeyManager;
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    var parameters = base.CreateParams;
                    parameters.Parent = HwndMessage;
                    return parameters;
                }
            }
            /// <summary>
            /// 消息处理
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                if (!_mhotkeyManager.HandleHotkeyMessage(m.Msg, m.WParam))
                {
                    base.WndProc(ref m);
                }
            }
        }
    }
}
