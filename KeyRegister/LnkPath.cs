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
        /// 获取 exe 的路径
        /// </summary>
        /// <param name="path"> lnk 的路径</param>
        /// <returns>exe的路径</returns>
        private string ObtainLnkPath(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    WshShell shell = new WshShell();
                    IWshShortcut iwShortcut = (IWshShortcut)shell.CreateShortcut(path);
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
        /// <returns>所有文件的全路径</returns>
        public List<string> getPath(string path)
        {
            List<string> list = new List<string>();//定义list变量，存放获取到的路径            
            try
            {
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
            }
            catch (Exception)
            {
                //throw;
            }
            return list;
        }

        /// <summary>
        /// 路径文件读取
        /// </summary>             
        /// <returns></returns>
        public List<LnkModel> ListLnkModel()
        {
            List<LnkModel> lnkList = new List<LnkModel>();
            List<string> list = new List<string>();//定义list变量，存放获取到的路径                                                           
            list = getPath(Resources.lnkPath);
            foreach (String item in list)
            {
                LnkModel lnkModel = new LnkModel();
                String lnkPath = ObtainLnkPath(item);
                if (lnkPath == "0")
                    return null;
                lnkModel.LnkPath = lnkPath.Substring(0, lnkPath.LastIndexOf(".exe")+4);
                //lnkModel.LnkPath = lnkPath.Substring(0,LnkPath);
                lnkModel.LnkName = item.Substring(item.LastIndexOf("\\") + 1, item.Count() - item.LastIndexOf("\\") - 5);

                lnkList.Add(lnkModel);
            }
            return lnkList;
        }
    }
}
