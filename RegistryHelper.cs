using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLib
{
    /// <summary>
    /// 注册表帮助类
    /// </summary>
    public class RegistryHelper
    {
        /// <summary>
        /// 设置应用程序开机自动开启
        /// </summary>
        /// <param name="ExecutablePath">程序可执行文件</param>
        public void SetAutoStart(string ExecutablePath)
        {
            if (string.IsNullOrEmpty(ExecutablePath)) throw new ArgumentNullException();
            if (!File.Exists(ExecutablePath)) throw new FileNotFoundException(ExecutablePath);
            if (!Path.GetExtension(ExecutablePath).Equals(".exe", StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("无效执行文件");

            //获取注册表Current_user行          
            using (RegistryKey user = Registry.CurrentUser)
            {
                //打开程序自动运行目录
                using (RegistryKey run = user.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
                {
                    //添加自定义项和程序启动执行文件路径
                    run.SetValue(Path.GetFileNameWithoutExtension(ExecutablePath), ExecutablePath);
                }
            }
        }
    }
}
