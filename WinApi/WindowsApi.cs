using System;
using System.Runtime.InteropServices;

namespace CSharpLib.WinApi
{
    /// <summary>
    /// Windows系统API
    /// </summary>
    internal class WindowsApi
    {
        /// <summary>
        /// 热键注册
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="id">唯一ID</param>
        /// <param name="fsModifiers">按键标志</param>
        /// <param name="vk">按键</param>
        /// <returns>注册是否成功</returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, uint id, uint fsModifiers, uint vk);


        /// <summary>
        /// 注销热键
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="id">唯一ID</param>
        /// <returns>注销结果</returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, uint id);

        /// <summary>
        /// GuidID转换成全局唯一Id
        /// </summary>
        /// <param name="lpStr">字符串</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern uint GlobalAddAtom(string lpStr);

        /// <summary>
        /// 删除该全局唯一ID
        /// </summary>
        /// <param name="uInt16">Id</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern uint GlobalDeleteAtom(uint uInt16);


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">当前句柄</param>
        /// <param name="wMsg">消息类型</param>
        /// <param name="wParam">消息内容</param>
        /// <param name="lParam">内容句柄</param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
    }
}
