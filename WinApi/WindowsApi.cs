using System;
using System.Runtime.InteropServices;

namespace CSharpLib.WinApi
{
    /// <summary>
    /// Windows系统API
    /// </summary>
    public class WindowsApi
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
        public static extern bool RegisterHotKey(IntPtr hWnd, uint id, uint fsModifiers, uint vk);


        /// <summary>
        /// 注销热键
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="id">唯一ID</param>
        /// <returns>注销结果</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, uint id);

        /// <summary>
        /// GuidID转换成全局唯一Id
        /// </summary>
        /// <param name="lpStr">字符串</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint GlobalAddAtom(string lpStr);

        /// <summary>
        /// 删除该全局唯一ID
        /// </summary>
        /// <param name="uInt16">Id</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint GlobalDeleteAtom(uint uInt16);


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">当前句柄</param>
        /// <param name="wMsg">消息类型</param>
        /// <param name="wParam">消息内容</param>
        /// <param name="lParam">内容句柄</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

        /// <summary>
        /// 设置父窗体
        /// </summary>
        /// <param name="hWndChild">子窗体句柄</param>
        /// <param name="hWndNewParent">父窗体句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>
        /// 窗体展示
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="nCmdShow">Max:3 Min:6 Normal:1</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// 查找窗体句柄
        /// </summary>
        /// <param name="lpClassName">类名</param>
        /// <param name="lpWindowName">窗体标题</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 设置窗体标题栏
        /// </summary>
        /// <param name="handle">窗体句柄</param>
        /// <param name="oldStyle">旧样式</param>
        /// <param name="newStyle">新样式</param>
        /// <remarks>
        /// 无边框  句柄,-16, 369164288
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern void SetWindowLong(IntPtr handle, int oldStyle, int newStyle);

        /// <summary>
        /// 结束任务
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="fShutDown">是否立即关闭</param>
        /// <param name="fForce">焦点</param>
        /// <remarks>
        /// fShutDown = true will kill the window instantly; false will show the message box before closing for Saving Changes
        /// </remarks>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EndTask(IntPtr hWnd, bool fShutDown, bool fForce);

        /// <summary>
        /// 获取焦点窗体句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 设置窗体获取焦点
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        /// <summary>
        /// 设置窗体位置
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="hWndInsertAfter">BOTTOM：值为1，将窗口置于Z序的底部。如果参数hWnd标识了一个顶层窗口，则窗口失去顶级位置，并且被置在其他窗口的底部。NOTOPMOST：值为-2，将窗口置于所有非顶层窗口之上（即在所有顶层窗口之后）。如果窗口已经是非顶层窗口则该标志不起作用。TOP：值为0，将窗口置于Z序的顶部。TOPMOST：值为-1，将窗口置于所有非顶层窗口之上。即使窗口未被激活窗口也将保持顶级位置。</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="cX">宽度</param>
        /// <param name="cY">高度</param>
        /// <param name="wFlags">具体操作</param>
        /// <remarks>
        /// Move:0x0001 ,Size:0x002 ,Hide:0x0080, Show:0x0040 , SetTop:1|2
        /// </remarks>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cX, int cY, int wFlags);

        /// <summary>
        /// 获取窗体Rectangle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out WinStruts.Rect lpRect);
    }
}
