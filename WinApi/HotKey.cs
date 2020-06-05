using System;
using System.Runtime.InteropServices;

namespace CSharpLib.WinApi
{

    public class HotKey
    {
        /// <summary>
        /// 注销热键
        /// </summary>
        /// <param name="intPtr">窗体句柄</param>
        /// <param name="id">唯一ID</param>
        public static void Unregister(IntPtr intPtr, uint id)
        {
            if (intPtr != IntPtr.Zero)
            {
                if (!WindowsApi.UnregisterHotKey(intPtr, id))
                {
                    var hr = Marshal.GetHRForLastWin32Error();
                    throw Marshal.GetExceptionForHR(hr);
                }
            }
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="intPtr">窗体句柄</param>
        /// <param name="id">唯一ID</param>
        /// <param name="hotkeyModify">按键枚举</param>
        /// <param name="vk">按键</param>
        /// <returns>注册结果</returns>
        public static bool Register(IntPtr intPtr, uint id, HotkeyFlags hotkeyModify, uint vk)
        {
            if (intPtr != IntPtr.Zero)
            {
                return WindowsApi.RegisterHotKey(intPtr, id, (uint)hotkeyModify, vk);
            }
            return false;
        }
    }

    /// <summary>
    /// 按键枚举
    /// </summary>
    [Flags]
    public enum HotkeyFlags : uint
    {
        None = 0x0000,
        Alt = 0x0001,
        Control = 0x0002,
        Shift = 0x0004,
        Windows = 0x0008,
        NoRepeat = 0x4000
    }
}
