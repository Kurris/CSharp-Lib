using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace CSharpLib
{

    public class WindowsServiceHelper
    {
        /// <summary>
        /// WindowsServiceHelper构造函数
        /// </summary>
        public WindowsServiceHelper()
        {

        }

        /// <summary>
        /// WindowsServiceHelper构造函数
        /// </summary>
        /// <param name="FilePath">服务exe全路径</param>
        public WindowsServiceHelper(string FilePath)
        {
            _mspath = FilePath;

            _msserviceName = ServiceController.GetServices().Any(s => s.ServiceName.Equals(Path.GetFileNameWithoutExtension(_mspath), StringComparison.OrdinalIgnoreCase))
                ? throw new FileNotFoundException(_mspath)
                : Path.GetFileNameWithoutExtension(_mspath);
        }

        private string _msserviceName = string.Empty;
        private string _mspath = string.Empty;

        /// <summary>
        /// 服务exe全路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return _mspath;
            }
            set
            {
                _mspath = value;

                _msserviceName = ServiceController.GetServices().Any(s => s.ServiceName.Equals(Path.GetFileNameWithoutExtension(_mspath), StringComparison.OrdinalIgnoreCase))
                    ? throw new FileNotFoundException(_mspath)
                    : Path.GetFileName(_mspath);
            }
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        /// <returns>安装状态</returns>
        public IDictionary SetupService()
        {
            using (AssemblyInstaller assemblyInstaller = new AssemblyInstaller())
            {
                assemblyInstaller.UseNewContext = true;
                assemblyInstaller.Path = _mspath;
                IDictionary savedstate = new Hashtable();
                assemblyInstaller.Install(savedstate);
                assemblyInstaller.Commit(savedstate);
                return savedstate;
            }
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <returns>移除状态</returns>
        public IDictionary RemoveService()
        {

            using (AssemblyInstaller assemblyInstaller = new AssemblyInstaller())
            {
                assemblyInstaller.UseNewContext = true;
                assemblyInstaller.Path = _mspath;
                IDictionary savedstate = new Hashtable();
                assemblyInstaller.Uninstall(savedstate);
                return savedstate;
            }
        }

        /// <summary>
        /// 获取服务状态(使用后请注意释放ServiceController的资源,避免内存泄漏)
        /// </summary>
        /// <param name="controller">服务实例</param>
        /// <returns>服务状态</returns>
        public ServiceControllerStatus GetServiceStatus(out ServiceController controller)
        {
            controller = new ServiceController(_msserviceName);
            return controller.Status;
        }

        /// <summary>
        /// 开启服务,如果服务已经开启,则不处理
        /// </summary>
        /// <param name="args">参数</param>
        public void StartService(string[] args)
        {
            ServiceControllerStatus controllerStatus = GetServiceStatus(out ServiceController controller);

            using (controller)
            {
                if (controllerStatus != ServiceControllerStatus.Running
                               || controllerStatus != ServiceControllerStatus.StartPending)
                {
                    controller.Start(args);
                }
            }
        }

        /// <summary>
        /// 停止服务,如果服务已经停止,则不处理
        /// </summary>
        public void StopService()
        {
            ServiceControllerStatus controllerStatus = GetServiceStatus(out ServiceController controller);

            using (controller)
            {
                if (controllerStatus != ServiceControllerStatus.Stopped
                    || controllerStatus != ServiceControllerStatus.StopPending)
                {
                    controller.Stop();
                }
            }
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        /// <param name="args">参数/param>
        public void RestartService(string[] args)
        {
            ServiceControllerStatus controllerStatus = GetServiceStatus(out ServiceController controller);

            using (controller)
            {
                if (controllerStatus != ServiceControllerStatus.Running
                               || controllerStatus != ServiceControllerStatus.StartPending)
                {
                    controller.Start(args);
                }
                else
                {
                    StopService();
                    System.Threading.Thread.Sleep(50);
                    StartService(args);
                }
            }
        }
    }
}
