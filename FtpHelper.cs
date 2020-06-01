using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSharpLib
{
    public class FtpHelper
    {
        /// <summary>
        /// ftp的ip地址 (最终 ftp://192.168.2.115:21/xxxx)
        /// </summary>
        private static string _mftpUri = string.Empty;

        /// <summary>
        /// ftp用户名称(user)
        /// </summary>
        private static string _mftpUserName = string.Empty;

        /// <summary>
        /// ftp用户密码(123456)
        /// </summary>
        private static string _mftpPassword = string.Empty;

        /// <summary>
        /// ftp服务地址(192.168.2.115:21)
        /// </summary>
        private static string _mftpServerIP = string.Empty;

        /// <summary>
        /// ftp当前路径
        /// </summary>
        private static string _mftpRemotePath = string.Empty;

        /// <summary>
        /// 需要自动将ftpUri回退
        /// </summary>
        private static bool _mbneedGoBack = false;


        public string FtpUri { get => _mftpUri; }

        /// <summary>
        /// 全局实例成员
        /// </summary>
        private static FtpHelper _minstance = null;

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="IsRoot">true:绝对路径 false:相对路径</param>
        public static FtpHelper Redirect(string destination = "", bool IsRoot = false)
        {

            if (_minstance == null)
            {
                _mftpUri = MappingConfigHelper.GetValueByKey("FtpUri", true, "192.168.2.115");
                _mftpUserName = MappingConfigHelper.GetValueByKey("FtpUser", true, "USER");
                _mftpPassword = MappingConfigHelper.GetValueByKey("FtpPwd", true, "123456");

                _minstance = new FtpHelper(_mftpUri, _mftpUserName, _mftpPassword);
            }

           if (!string.IsNullOrEmpty(destination))
            {
                if (IsRoot)
                {
                    _mftpRemotePath = "/" + destination;
                    _mftpUri = "ftp://" + _mftpServerIP + "/" + _mftpRemotePath;
                }
                else
                {
                    if (_mftpRemotePath.EndsWith("//"))
                    {
                        _mftpRemotePath += destination;
                    }
                    else
                    {
                        _mftpRemotePath += "/" + destination;
                    }

                    _mftpUri = _mftpRemotePath;
                }
            }
            return _minstance;
        }

        /// <summary>
        /// 回退一级
        /// </summary>
        public static bool GoBack()
        {
            if (!_mbneedGoBack) return false;

            try
            {
                string sTemp = _mftpUri;
                _mftpUri = _mftpUri.Substring(0, _mftpUri.Length - 1);
                int iLastSpiltSecond = _mftpUri.LastIndexOf('/');
                _mftpUri = _mftpUri.Substring(0, iLastSpiltSecond + 1);
                if (!_mftpUri.Contains(sTemp.Substring(0, 15)))
                {
                    _mftpUri = sTemp;
                    return false;
                }
               else if (sTemp.EndsWith(_mftpServerIP)
                    || sTemp.EndsWith(_mftpServerIP+"/")
                    || sTemp.EndsWith(_mftpServerIP+"//"))
                {
                    _mftpUri = sTemp;
                    return false;
                }
                _mftpRemotePath = _mftpUri;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 用户登录凭证
        /// </summary>
        private static NetworkCredential FtpCredential => new NetworkCredential(_mftpUserName, _mftpPassword);


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="FtpServerIP"></param>
        /// <param name="FtpUserID"></param>
        /// <param name="FtpPassword"></param>
        /// <param name="FtpRemotePath"></param>
        private FtpHelper(string ftpuri, string ftpusername, string ftppassword)
        {
            _mftpServerIP = ftpuri;
            _mftpUserName = ftpusername;
            _mftpPassword = ftppassword;
            _mftpUri = "ftp://" + ftpuri + "//";

            _mftpRemotePath = _mftpUri;
        }


        /// <summary>
        /// Ftp Request
        /// </summary>
        /// <param name="sUri"></param>
        /// <returns></returns>
        private FtpWebRequest GetRequest()
        {
            try
            {
                FtpWebRequest Ftp = (FtpWebRequest)WebRequest.Create(new Uri(_mftpUri));
                Ftp.Credentials = FtpCredential;
                Ftp.KeepAlive = false;
                Ftp.Timeout = 1000 * 30;
                Ftp.UseBinary = true;

                return Ftp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filepath"></param>
        public void UploadFile(string filepath)
        {
            FileInfo fileInfo = new FileInfo(filepath);
            Redirect(fileInfo.Name);
            FtpWebRequest reqFTP = GetRequest();
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.ContentLength = fileInfo.Length;

            int iBuffLength = 1024 * 1024;
            byte[] arrBytes = new byte[iBuffLength];

            using (FileStream fs = fileInfo.OpenRead())
            {
                using (Stream s = reqFTP.GetRequestStream())
                {
                    while (true && iBuffLength == 1024 * 1024)
                    {
                        iBuffLength = fs.Read(arrBytes, 0, iBuffLength);
                        s.Write(arrBytes, 0, iBuffLength);
                    }
                }
            }
            GoBack();
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="destination">本地文件路径</param>
        public void DownloadFile(string destination)
        {
            try
            {
                FtpWebRequest reqFTP = GetRequest();
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using (Stream ftpStream = response.GetResponseStream())
                    {
                        int bufferLength = 1024 * 1024;
                        byte[] arrBytes = new byte[bufferLength];

                        using (FileStream outputStream = new FileStream(destination, FileMode.Create))
                        {
                            while ((bufferLength = ftpStream.Read(arrBytes, 0, bufferLength)) > 0)
                            {
                                outputStream.Write(arrBytes, 0, bufferLength);
                            }
                        }
                    }
                }
                GoBack();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ftpremotepath">ftp远程文件路径</param>
        /// <returns></returns>
        public bool Delete()
        {
            try
            {
                FtpWebRequest reqFTP = GetRequest();
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    GoBack();
                    return response.StatusCode == FtpStatusCode.FileActionOK;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>  
        /// 获取当前目录下明细(包含文件和文件夹)  
        /// </summary>  
        public List<string> GetAllDetail()
        {
            try
            {
                FtpWebRequest reqFTP = GetRequest();
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                using (WebResponse response = reqFTP.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        List<string> listinfo = new List<string>(20);

                        string sMsgline = string.Empty;

                        while (!string.IsNullOrEmpty(sMsgline = reader.ReadLine()))
                        {
                            listinfo.Add(sMsgline);
                        }

                        return listinfo;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取当前目录所有文件
        /// </summary>
        /// <returns></returns>
        public List<string> GetFiles()
        {
            try
            {
                List<string> listStr = GetAllDetail();


                return listStr.Where(x => x.IndexOf("<DIR>") < 0)
                               .Select(x => GetFileNameOfDetial(x)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取详细信息中的文件名
        /// </summary>
        /// <param name="str">ftp信息</param>
        /// <returns>文件名</returns>
        public string GetFileNameOfDetial(string str)
        {
            int ilastIndex = str.LastIndexOf(" ");
            return str.Substring(ilastIndex + 1, str.Length - (ilastIndex + 1));
        }

        /// <summary>  
        /// 判断当前目录下指定的文件是否存在  
        /// </summary>  
        /// <param name="FileName">文件名</param>  
        public bool FileExist(string FileName) => GetFiles().Contains(FileName);




        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName">文件夹名称</param>
        /// <returns></returns>
        public bool CreateDirectory()
        {
            try
            {
                FtpWebRequest reqFTP = GetRequest();
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    return response.StatusCode == FtpStatusCode.PathnameCreated;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>  
        /// 更改文件名  
        /// </summary> 
        public bool Rename(string newFilename)
        {
            try
            {
                FtpWebRequest reqFTP = GetRequest();
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    GoBack();
                    return response.StatusCode == FtpStatusCode.FileActionOK;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
