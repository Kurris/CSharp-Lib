using System;
using System.Runtime.InteropServices;

namespace CSharpLib.WinApi.Form
{

    /// <summary>
    /// 注册热键异常事件
    /// </summary>
    [Serializable]
    public class HotkeyAlreadyRegisteredException : Exception
    {
        public HotkeyAlreadyRegisteredException(string name, Exception inner) : base($"当前热键名称:{name} " +inner.Message, inner)
        {
            Name = name;
            HResult = Marshal.GetHRForException(inner);
        }

        public string Name { get; }
    }

    /// <summary>
    /// 热键事件
    /// </summary>
    /// <param name="eventArgs"></param>
    public delegate void HotKeyEventHandler(HotkeyEventArgs eventArgs);

    /// <summary>
    /// 热键事件参数
    /// </summary>
    public class HotkeyEventArgs : EventArgs
    {
        internal HotkeyEventArgs(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public bool Handled { get; set; }
    }
}
