using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyRegister
{
    public class LnkPath
    {
        /// <summary>
        /// 获取 lnk 的路径
        /// </summary>
        /// <param name="path"> lnk 的路径</param>
        /// <returns></returns>
        private string ObtainLnkPath(string path)
        {
            try
            {
                //快捷方式文件的路径 = @"d:\Test.lnk";
                if (System.IO.File.Exists(path))
                {
                    WshShell shell = new WshShell();
                    IWshShortcut iwShortcut = (IWshShortcut)shell.CreateShortcut(path);
                    //快捷方式文件指向的路径.Text = 当前快捷方式文件IWshShortcut类.TargetPath;
                    //快捷方式文件指向的目标目录.Text = 当前快捷方式文件IWshShortcut类.WorkingDirectory;
                    return iwShortcut.TargetPath;
                }
                return "0";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        /// <summary>
        /// 读取文件夹的下的所有 lnk
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        public List<string> getPath(string path)
        {
            List<string> list = new List<string>();//定义list变量，存放获取到的路径
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                list.Add(f.FullName);//添加文件的路径到列表
            }
            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                getPath(d.FullName);
                list.Add(d.FullName);//添加文件夹的路径到列表
            }
            return list;
        }

        /// <summary>
        /// 默认路径读取
        /// </summary>
        /// <param name="sign">是默认路径</param>
        /// <param name="obj">是不是单击事件调用</param>
        /// <returns></returns>
        public List<LnkModel> ListLnkModel(bool obj)
        {
            List<LnkModel> lnkList = new List<LnkModel>();
            List<string> list = new List<string>();//定义list变量，存放获取到的路径        
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            string path = @"C:\";

            if (!obj)
            {
                //读取到配置文件的路径
                if (Resources.lnkPath != "0" && Resources.lnkPath != null)
                {
                    path = Resources.lnkPath;   //使用配置文件读取 lnk 所在的文件夹
                }
                else
                {                   
                        //使用默认路径 创建 lnkFile 文件夹
                        path = Resources.exePath + "\\lnkFile";
                        DirectoryInfo dir = new DirectoryInfo(path);
                        dir.Create();                   
                }
            }
            else
            {
                //使用用户自己选择的路径
                if (obj == true && folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = folderBrowserDialog1.SelectedPath;
                    Resources.lnkPath = path;
                }
            }            

            list = getPath(path);
            foreach (String item in list)
            {
                LnkModel lnkModel = new LnkModel();
                String lnkPath = ObtainLnkPath(item);
                if (lnkPath == "0")
                    return null;
                lnkModel.LnkPath = lnkPath.Substring(0, lnkPath.LastIndexOf("\\"));
                lnkModel.LnkName = lnkPath.Substring(lnkPath.LastIndexOf("\\") + 1, lnkPath.Count() - lnkModel.LnkPath.Count() - 1);

                lnkList.Add(lnkModel);
            }
            return lnkList;
        }
    }
}
