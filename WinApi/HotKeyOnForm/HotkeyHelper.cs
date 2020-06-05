using System;
using System.Runtime.InteropServices;

namespace CSharpLib.WinApi.Form
{
    /// <summary>
    /// 热键帮助类
    /// </summary>
    internal class HotkeyHelper
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="virtualKey">按键</param>
        /// <param name="flags">按键标志</param>
        /// <param name="hEvent">热键事件</param>
        internal HotkeyHelper(uint virtualKey, HotkeyFlags flags, HotKeyEventHandler hEvent)
        {
            Id = WindowsApi.GlobalAddAtom(Guid.NewGuid().ToString());
            VirtualKey = virtualKey;
            Flags = flags;
            HotkeyActive = hEvent;
        }

        /// <summary>
        /// 唯一ID
        /// </summary>
        internal uint Id { get; }

        /// <summary>
        /// 按键
        /// </summary>
        internal uint VirtualKey { get; }

        /// <summary>
        /// 按键标志
        /// </summary>
        internal HotkeyFlags Flags { get; }

        /// <summary>
        /// 热键事件
        /// </summary>
        internal event HotKeyEventHandler HotkeyActive;

        /// <summary>
        /// 窗体句柄
        /// </summary>
        private IntPtr _hwnd;

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventArgs">事件参数</param>
        internal void Active(HotkeyEventArgs eventArgs)
        {
            HotkeyActive?.Invoke(eventArgs);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="hwnd">窗体句柄</param>
        /// <param name="name">热键Name</param>
        internal void Register(IntPtr hwnd, string name)
        {
            if (!HotKey.Register(hwnd, Id, Flags, VirtualKey))
            {
                WindowsApi.GlobalDeleteAtom(Id);

                var hr = Marshal.GetHRForLastWin32Error();
                var ex = Marshal.GetExceptionForHR(hr);

                //存在热键
                if ((uint)hr == 0x80070581)
                    throw new HotkeyAlreadyRegisteredException(name, ex);
                else
                    throw ex;
            }
            _hwnd = hwnd;
        }

        /// <summary>
        /// 注销
        /// </summary>
        internal void Unregister()
        {
            if (_hwnd != IntPtr.Zero)
            {
                HotKey.Unregister(_hwnd, Id);
                WindowsApi.GlobalDeleteAtom(Id);
                _hwnd = IntPtr.Zero;
            }
        }
    }
}