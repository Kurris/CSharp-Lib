using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLib
{
    public class ZipHelper
    {

        #region 压缩文件夹  

        /// <summary>   
        /// 压缩文件夹   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹  全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>     
        public static bool ZipDirectory(string folderToZip, string zipedFile, string password = "")
        {
            if (!Directory.Exists(folderToZip)) throw new FileNotFoundException(folderToZip);

            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile)))
            {
                zipStream.SetLevel(6);

                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

                string sRoot = Path.GetDirectoryName(folderToZip);

                return ZipRecursionDirectory(sRoot, folderToZip, zipStream, string.Empty);
            }
        }

        /// <summary>   
        /// 递归压缩文件夹的内部方法   
        /// </summary>   
        /// <param name="folderToZip">要压缩的文件夹路径</param>   
        /// <param name="zipStream">压缩输出流</param>   
        /// <param name="parentFolderName">此文件夹的上级文件夹</param>   
        /// <returns>递归结果</returns>   
        private static bool ZipRecursionDirectory(string root, string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            try
            {
                Crc32 crc = new Crc32();

                string entName = folderToZip.Replace(root, string.Empty) + "/";

                string[] filesOrfolders = Directory.GetFileSystemEntries(folderToZip);
                foreach (string fileorfolder in filesOrfolders)
                {
                    if (File.Exists(fileorfolder))
                    {
                        using (FileStream fs = File.OpenRead(fileorfolder))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            ZipEntry entry = new ZipEntry(entName + Path.GetFileName(fileorfolder));
                            entry.DateTime = File.GetLastWriteTime(fileorfolder);
                            entry.Size = fs.Length;

                            crc.Reset();
                            crc.Update(buffer);

                            entry.Crc = crc.Value;
                            zipStream.PutNextEntry(entry);
                            zipStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        ZipRecursionDirectory(root, fileorfolder, zipStream, folderToZip);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 压缩文件

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileToZip">要压缩的文件全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public static bool ZipFile(string fileToZip, string zipedFile, string password = "")
        {
            try
            {
                if (!File.Exists(fileToZip)) throw new FileNotFoundException(fileToZip);

                using (FileStream fs = File.OpenRead(fileToZip))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile)))
                    {
                        if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

                        ZipEntry entry = new ZipEntry(Path.GetFileName(fileToZip));
                        entry.DateTime = File.GetLastWriteTime(fileToZip);
                        zipStream.SetLevel(6);
                        zipStream.PutNextEntry(entry);

                        zipStream.Write(buffer, 0, buffer.Length);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 解压  

        /// <summary>   
        /// 解压压缩文件到指定目录
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        public static bool UnZip(string fileToUnZip, string zipedFolder, string password = "")
        {
            try
            {
                if (!File.Exists(fileToUnZip)) throw new FileNotFoundException(fileToUnZip);

                if (!Directory.Exists(zipedFolder)) Directory.CreateDirectory(zipedFolder);

                using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(fileToUnZip)))
                {
                    if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

                    ZipEntry entry = null;

                    while ((entry = zipStream.GetNextEntry()) != null)
                    {
                        if (string.IsNullOrEmpty(entry.Name)) continue;

                        string fileName = Path.Combine(zipedFolder, entry.Name);
                        fileName = fileName.Replace('/', '\\');

                        if (fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        string sfilePathParent = Path.GetDirectoryName(fileName);
                        if (!Directory.Exists(sfilePathParent))
                        {
                            Directory.CreateDirectory(sfilePathParent);
                        }

                        using (FileStream fs = File.Create(fileName))
                        {
                            int size = 2048;
                            byte[] data = new byte[size];
                            while ((size = zipStream.Read(data, 0, data.Length)) > 0)
                            {
                                fs.Write(data, 0, size);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}