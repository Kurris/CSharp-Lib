using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using System.Collections.Generic;
using System.Linq;

namespace CSharpLib
{
    /// <summary>
    /// Dialog帮助类
    /// </summary>
    public class DialogHelper
    {
        /// <summary>
        /// 文件选择
        /// </summary>
        /// <param name="InitialDirectory">初始化目录</param>
        /// <returns>选择的文件</returns>
        public static string ChooseFile(string InitialDirectory = null)
        {
            return ChooseFile(null, InitialDirectory);
        }

        /// <summary>
        /// 文件选择
        /// </summary>
        /// <param name="FilterName">扩展名说明</param>
        /// <param name="FilterList">扩展名,如有多个,使用逗号隔开txt,asm,csproject</param>
        /// <param name="InitialDirectory">初始化目录</param>
        /// <returns>选择的文件</returns>
        public static string ChooseFile(string FilterName, string FilterList, string InitialDirectory = null)
        {
            return ChooseFile(new Dictionary<string, string>()
            {
                [FilterName] = FilterList
            }
            , InitialDirectory);
        }

        /// <summary>
        /// 文件选择
        /// </summary>
        /// <param name="DicFilterNameAndFilterlist">扩展名列表(扩展说明--扩展名)</param>
        /// <param name="InitialDirectory">初始化目录</param>
        /// <returns>选择的文件</returns>
        public static string ChooseFile(Dictionary<string, string> DicFilterNameAndFilterlist, string InitialDirectory = null)
        {
            return ChooseFiles(DicFilterNameAndFilterlist, InitialDirectory)?[0];
        }



        /// <summary>
        /// 多文件选择
        /// </summary>
        /// <returns>文件选择列表</returns>
        public static List<string> ChooseFiles(string InitialDirectory = null)
        {
            return ChooseFiles(null, InitialDirectory);
        }

        /// <summary>
        /// 多文件选择
        /// </summary>
        /// <param name="FilterName">扩展名说明</param>
        /// <param name="FilterList">扩展名,如有多个,使用逗号隔开txt,asm,csproject</param>
        /// <param name="InitialDirectory">初始化目录</param>
        /// <returns>文件选择列表</returns>
        public static List<string> ChooseFiles(string FilterName, string FilterList, string InitialDirectory = null)
        {
            return ChooseFiles(new Dictionary<string, string>()
            {
                [FilterName] = FilterList
            },
            InitialDirectory);
        }

        /// <summary>
        /// 多文件选择
        /// </summary>
        /// <param name="DicFilterNameAndFilterlist">扩展名列表(扩展说明--扩展名)</param>
        /// <param name="InitialDirectory">初始化目录</param>
        /// <returns>文件选择列表</returns>
        public static List<string> ChooseFiles(Dictionary<string, string> DicFilterNameAndFilterlist, string InitialDirectory = null)
        {
            return ChooseFilesOrFolders(false, true, InitialDirectory, DicFilterNameAndFilterlist, null)?.ToList();
        }


        /// <summary>
        /// 文件夹选择
        /// </summary>
        /// <param name="Multiselect">是否多选(默认单选)</param>
        /// <param name="InitialDirectory">初始目录</param>
        /// <returns>文件夹列表</returns>
        public static List<string> ChooseFolder(bool Multiselect = false, string InitialDirectory = null)
        {
            return ChooseFilesOrFolders(true, Multiselect, InitialDirectory, null, null)?.ToList();
        }

        /// <summary>
        /// 选择文件或文件夹
        /// </summary>
        /// <param name="Multiselect">是否可以多选</param>
        /// <param name="InitialDirectory">初始化文件夹</param>
        /// <param name="IsFolderPicker">文件选择还是文件夹选择</param>
        /// <param name="DicFilterNameAndFilterlist">扩展名说明和扩展名列表(使用逗号隔开,如jpg,png)</param>
        /// <returns></returns>
        public static IEnumerable<string> ChooseFilesOrFolders(
            bool IsFolderPicker
            , bool Multiselect
            , string InitialDirectory
            , Dictionary<string, string> DicFilterNameAndFilterlist
            , List<CommonFileDialogControl> Controls
            )
        {
            using (var dialog = new CommonOpenFileDialog()
            {
                Title = "标题",
                Multiselect = Multiselect,
                IsFolderPicker = IsFolderPicker,
                InitialDirectory = InitialDirectory,
            })
            {
                if (DicFilterNameAndFilterlist != null)
                {
                    foreach (var item in DicFilterNameAndFilterlist)
                    {
                        dialog.Filters.Add(new CommonFileDialogFilter(item.Key, item.Value));
                    }
                }

                if (Controls != null && Controls.Count > 0)
                {
                    foreach (CommonFileDialogControl ctrl in Controls)
                    {
                        dialog.Controls.Add(ctrl);
                    }
                }

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok) return dialog.FileNames;

                return null;
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sFileName">文件名</param>
        /// <param name="sDisplayName">扩展说明</param>
        /// <param name="sFilterlist">扩展名</param>
        /// <returns>保存文件的路径</returns>
        public static string SaveFile(string sFileName, string sDisplayName, string sFilterlist)
        {
            using (var dialog = new CommonSaveFileDialog()
            {
                Title = "标题",
                EnsureFileExists = true,
                EnsurePathExists = true,
                DefaultFileName = sFileName,
            })
            {
                dialog.Filters.Add(new CommonFileDialogFilter(sDisplayName, sFilterlist));

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok) return dialog.FileName;

                return string.Empty;
            }
        }
    }
}
