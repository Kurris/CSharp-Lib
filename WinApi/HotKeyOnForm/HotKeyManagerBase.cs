using System;
using System.Collections.Generic;

namespace CSharpLib.WinApi.Form
{
    public abstract class HotKeyManagerBase
    {
        /// <summary>
        /// 存取已定义的热键Name
        /// </summary>
        private readonly Dictionary<uint, string> _mdichotkeyNames = new Dictionary<uint, string>();

        /// <summary>
        /// 存取已定义的热键信息
        /// </summary>
        private readonly Dictionary<string, HotkeyHelper> _mdichotkeys = new Dictionary<string, HotkeyHelper>();

        /// <summary>
        /// 句柄
        /// </summary>
        private IntPtr _hwnd;


        internal static readonly IntPtr HwndMessage = (IntPtr)(-3);


        /// <summary>
        /// 添加或替换热键
        /// </summary>
        /// <param name="name">热键Name</param>
        /// <param name="virtualKey">按键</param>
        /// <param name="flags">按键标志</param>
        /// <param name="HotKeyActive">热键事件</param>
        internal void AddOrReplace(string name, uint virtualKey, HotkeyFlags flags, HotKeyEventHandler HotKeyActive)
        {
            var hotkeyHelper = new HotkeyHelper(virtualKey, flags, HotKeyActive);

            lock (_mdichotkeys)
            {
                Remove(name);
                _mdichotkeys.Add(name, hotkeyHelper);
                _mdichotkeyNames.Add(hotkeyHelper.Id, name);

                if (_hwnd != IntPtr.Zero)
                {
                    hotkeyHelper.Register(_hwnd, name);
                }
            }
        }

        /// <summary>
        /// 移除当前热键Name的信息,并且注销
        /// </summary>
        /// <param name="name">name</param>
        public void Remove(string name)
        {
            lock (_mdichotkeys)
            {
                if (_mdichotkeys.TryGetValue(name, out HotkeyHelper hotkeyHelper))
                {
                    _mdichotkeys.Remove(name);

                    _mdichotkeyNames.Remove(hotkeyHelper.Id);

                    if (_hwnd != IntPtr.Zero)
                    {
                        hotkeyHelper.Unregister();
                    }
                }
            }
        }

        /// <summary>
        /// 设置句柄
        /// </summary>
        /// <param name="hwnd"></param>
        internal void SetHwnd(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        /// <summary>
        /// 处理热键消息
        /// </summary>
        /// <param name="msg">消息类型</param>
        /// <param name="wParam">唯一ID句柄类型</param>
        /// <param name="handled"></param>
        /// <returns></returns>
        internal bool HandleHotkeyMessage(int msg, IntPtr wParam)
        {
            bool handled = false;
            //热键消息
            if (msg == 0x0312)
            {
                uint id = (uint)wParam.ToInt32();

                if (_mdichotkeyNames.TryGetValue(id, out string name))
                {
                    var arg = new HotkeyEventArgs(name);
                    _mdichotkeys[name].Active(arg);
                    handled = arg.Handled;
                }
            }
            return handled;
        }
    }
}
