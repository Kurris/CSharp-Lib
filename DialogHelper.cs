using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLib
{
    public class DialogHelper
    {
        /// <summary>
        /// 文件选择
        /// </summary>
        public static string ChooseFile()
        {
            using (var dialog = new CommonOpenFileDialog()
            {
                Multiselect = false,
                IsFolderPicker = false,
            })
            {
                dialog.Controls.Add(new CommonFileDialogButton() { Text = "aaa" });
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 文件选择, 多个扩展名使用逗号隔开,如 txt,asm,csproject
        /// </summary>
        public static string ChooseFile(string filtername, string filterlist)
        {
            using (var dialog = new CommonOpenFileDialog()
            {
                Multiselect = false,
                IsFolderPicker = false,
            })
            {
                dialog.Filters.Add(new CommonFileDialogFilter(filtername, filterlist));

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 多文件选择
        /// </summary>
        /// <param name="multi"></param>
        public static List<string> ChooseFiles()
        {
            using (var dialog = new CommonOpenFileDialog()
            {
                Multiselect = true,
                IsFolderPicker = false,
            })
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileNames.ToList();
                }
                return null;
            }
        }



        /// <summary>
        /// 多文件选择,多个扩展名使用逗号隔开,如 txt,asm,csproject
        /// </summary>
        /// <param name="multi"></param>
        public static List<string> ChooseFiles(string filtername, string filterlist)
        {
            using (var dialog = new CommonOpenFileDialog()
            {
                Multiselect = true,
                IsFolderPicker = false,
            })
            {
                dialog.Filters.Add(new CommonFileDialogFilter(filtername, filterlist));

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileNames.ToList();
                }
                return null;
            }

        }



        /// <summary>
        /// 文件夹选择
        /// </summary>
        /// <param name="Multi">是否多选</param>
        /// <returns></returns>
        public static string ChooseFolder(bool Multi = false)
        {
            using (var dialog = new CommonOpenFileDialog()
            {
                Multiselect = Multi,
                IsFolderPicker = true,
            })
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
                return string.Empty;
            }

        }

        public static string SaveFile(string sFileName, string sDisPlayName, string sFilterlist)
        {
            using (var dialog = new CommonSaveFileDialog()
            {
                EnsureFileExists = true,
                EnsurePathExists = true,
                DefaultFileName = sFileName
            })
            {
                dialog.Filters.Add(new CommonFileDialogFilter(sDisPlayName, sFilterlist));
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
                return string.Empty;
            }

        }
    }
}
