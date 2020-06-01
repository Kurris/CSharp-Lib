using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLib
{
    /// <summary>
    /// 文件/文件夹帮助相关
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 递归计算文件夹下的所有文件个数
        /// </summary>
        /// <param name="Folder">文件夹</param>
        /// <returns>文件数</returns>
        public static int CalculateFileCount(string Folder)
        {
            if (!Directory.Exists(Folder)) throw new FileNotFoundException(Folder + "不是文件夹或不存在");

            int icount = 0;
            List<string> listPath = Directory.GetFileSystemEntries(Folder).ToList();
            foreach (string fileorfolder in listPath)
            {
                if (File.Exists(fileorfolder))
                {
                    icount++;
                }
                else
                {
                    icount += CalculateFileCount(fileorfolder);
                }
            }
            return icount;
        }

        /// <summary>
        /// 递归复制文件
        /// </summary>
        /// <param name="FolderSource">源文件夹路径</param>
        /// <param name="FolderDestination">目标文件夹路径</param>
        /// <param name="DeleteWhenExists">但存在时是否删除</param>
        public void FolderToFolder(string FolderSource, string FolderDestination, bool DeleteWhenExists = false)
        {
            if (!Directory.Exists(FolderSource)) throw new FileNotFoundException(FolderSource + "不是文件夹或不存在");

            string[] arrAllFiles = Directory.GetFileSystemEntries(FolderSource);

            foreach (string fileOrfolderPath in arrAllFiles)
            {
                string sFileName = Path.GetFileName(fileOrfolderPath);

                string sNewPath = Path.Combine(FolderDestination, sFileName);

                if (File.Exists(fileOrfolderPath))
                {
                    if (!File.Exists(sNewPath))
                    {
                        File.Copy(fileOrfolderPath, sNewPath);
                    }
                }
                else
                {
                    string NowFolderSource = Path.GetFileName(fileOrfolderPath);
                    string NewFolderDestination = Path.Combine(FolderDestination, NowFolderSource);

                    if (!Directory.Exists(NewFolderDestination))
                    {
                        Directory.CreateDirectory(NewFolderDestination);
                    }

                    FolderToFolder(fileOrfolderPath, NewFolderDestination, DeleteWhenExists);
                }
            }
        }

        /// <summary>
        /// 获取当前路径到其某个上级文件夹的文件夹列表
        /// </summary>
        /// <param name="FolderDestination">目标文件夹名</param>
        /// <param name="BeginFolderPath">开始寻找的路径</param>
        /// <param name="Folders">文件夹列表,为null则自动初始化</param>
        /// <returns>文件夹列表</returns>
        public static List<string> GetParentFolderUntillXXX(string FolderDestination, string BeginFolderPath, List<string> Folders = null)
        {
            List<string> listFolder = null;
            if (Folders == null)
            {
                listFolder = new List<string>();
                listFolder.Add(Path.GetFileName(BeginFolderPath));
            }
            else
            {
                listFolder = Folders;
            }

            DirectoryInfo parentFolderInfo = Directory.GetParent(BeginFolderPath);

            if (parentFolderInfo.Name.Equals(FolderDestination, StringComparison.OrdinalIgnoreCase))
            {
                return listFolder;
            }
            else
            {
                listFolder.Add(parentFolderInfo.Name);
                GetParentFolderUntillXXX(FolderDestination, parentFolderInfo.FullName, listFolder);
            }

            listFolder.Reverse();
            return listFolder;
        }

        /// <summary>
        /// 比较俩个文件的版本号大小,FilePath1 = FilePath2 返回 0;FilePath1 > FilePath2 返回 1;FilePath1 < FilePath2 返回 -1;
        /// </summary>
        /// <param name="FilePath1">第一个文件</param>
        /// <param name="FilePath2">第二个问价</param>
        /// <returns>比较结果</returns>
        public static int CompareFileVersion(string FilePath1, string FilePath2)
        {
            FileVersionInfo versionInfo1 = FileVersionInfo.GetVersionInfo(FilePath1);
            if (string.IsNullOrEmpty(versionInfo1.FileVersion)) throw new ArgumentException(FilePath1 + " 不存在文件版本");

            FileVersionInfo versionInfo2 = FileVersionInfo.GetVersionInfo(FilePath2);
            if (string.IsNullOrEmpty(versionInfo2.FileVersion)) throw new ArgumentException(FilePath1 + " 不存在文件版本");


            string ver1 = versionInfo1.FileVersion;
            string ver2 = versionInfo2.FileVersion;

            Version version1 = new Version(ver1);
            Version version2 = new Version(ver2);
            if (version1 == version2)
            {
                return 0;
            }
            else if (version1 > version2)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
