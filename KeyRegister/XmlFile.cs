using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace KeyRegister
{
    /// <summary>
    /// XML 数据文件处理类
    /// </summary>
    public class XmlFile
    {
       　/// <summary>
        /// 保存数据到XML文件
        /// </summary>            
        public static void ObjListToXml()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlSM = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlSM);
            XmlElement xml = xmlDoc.CreateElement("", "KeyRegister", "");
            xmlDoc.AppendChild(xml);
            XmlNode gen = xmlDoc.SelectSingleNode("KeyRegister");
            if (Resources.dicThisInt != null)
            {
                foreach (var item in Resources.dicThisInt)
                {
                    XmlElement dicThisIntXml = xmlDoc.CreateElement("dicThisInt");

                    XmlElement x1 = xmlDoc.CreateElement("id");
                    x1.InnerText = item.Value.id.ToString();
                    dicThisIntXml.AppendChild(x1);

                    XmlElement x2 = xmlDoc.CreateElement("LnkName");
                    x2.InnerText = item.Value.LnkName;
                    dicThisIntXml.AppendChild(x2);

                    XmlElement x3 = xmlDoc.CreateElement("LnkPath");
                    x3.InnerText = item.Value.LnkPath;
                    dicThisIntXml.AppendChild(x3);

                    XmlElement x4 = xmlDoc.CreateElement("HotKey");
                    x4.InnerText = item.Value.HotKey;
                    dicThisIntXml.AppendChild(x4);

                    gen.AppendChild(dicThisIntXml);
                }
            }

            if (Resources.lnkPath != "0" && Resources.lnkPath != null)
            {
                XmlElement lnkPathXml = xmlDoc.CreateElement("lnkPath");
                XmlElement x1 = xmlDoc.CreateElement("path");
                x1.InnerText = Resources.lnkPath;
                lnkPathXml.AppendChild(x1);
                gen.AppendChild(lnkPathXml);
            }

            //保存配置文件到和程序同一目录
            xmlDoc.Save(Resources.exePath + "//config.xml");
        }

        /// <summary>
        /// 读取指定格式的xml文件,获取已注册过的快捷方式信息和文件夹路径
        /// </summary>        
        /// <returns> Dictionary 对象</returns>
        public static Dictionary<int, LnkModel> ReadXmlFile()
        {
            //读取不到配置文件
            if (!File.Exists(Resources.exePath + "\\config.xml"))
            {
                ChoiceLnkFile();
                return null;
            }

            Dictionary<int, LnkModel> dicThisInt = new Dictionary<int, LnkModel>();
            XmlDocument doc = new XmlDocument();
            doc.Load(Resources.exePath + "\\config.xml");           
            XmlNodeList list = doc.SelectNodes("/KeyRegister/dicThisInt");
            int MaxNum = 0;
            foreach (XmlElement item in list)
            {
                LnkModel lnkModel = new LnkModel();
                lnkModel.id = Convert.ToInt32(item.GetElementsByTagName("id")[0].InnerText);
                lnkModel.LnkName = item.GetElementsByTagName("LnkName")[0].InnerText;
                lnkModel.LnkPath = item.GetElementsByTagName("LnkPath")[0].InnerText;
                lnkModel.HotKey = item.GetElementsByTagName("HotKey")[0].InnerText;

                dicThisInt.Add(lnkModel.id, lnkModel);
                if (lnkModel.id > MaxNum)
                    MaxNum = lnkModel.id;
            }
            Resources.MaxNum = MaxNum + 1;
            //读取文件夹路径
            XmlNodeList listLnkPath = doc.SelectNodes("/KeyRegister/lnkPath");
            if (listLnkPath.Count == 0)
            {
                ChoiceLnkFile();
            }               
            else
            {
                foreach (XmlElement item in listLnkPath)
                {
                    try
                    {
                        Resources.lnkPath = item.GetElementsByTagName("path")[0].InnerText;
                    }
                    catch (Exception)
                    {
                        ChoiceLnkFile();
                    }
                    break;
                }
            }

            return dicThisInt;
        }


        /// <summary>
        /// 弹出选择文件夹框
        /// </summary>
        /// <returns>改变路径标记</returns>
        public static bool ChoiceLnkFile()
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.Description = "请选择只包含有“快捷方式”的文件夹，或空文件夹。";
            folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.Cancel)
                Application.Exit();
            if (dr == DialogResult.OK)
            {
                if (Resources.lnkPath == folderBrowserDialog1.SelectedPath)
                {
                    MessageBox.Show("选择的路径没有改变,不更新路径！", "选择提醒", MessageBoxButtons.OK);
                    return false;
                }
                else
                {
                    Resources.lnkPath = folderBrowserDialog1.SelectedPath;
                }
            }
            return true;
        }
    }
}
