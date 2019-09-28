using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace KeyRegister
{
    public class XmlFile
    {
       　/// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="dicThisInt">信息对象</param>
        /// <param name="sign">保存 lnk 文件夹路径</param>
        public static void ObjListToXml(Dictionary<int, LnkModel> dicThisInt)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlSM = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlSM);
            XmlElement xml = xmlDoc.CreateElement("", "KeyRegister", "");
            xmlDoc.AppendChild(xml);
            XmlNode gen = xmlDoc.SelectSingleNode("KeyRegister");

            foreach (var item in dicThisInt)
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

            //保存用户设置的路径
            if (Resources.lnkPath !="0" && Resources.lnkPath!=null)
            {
                XmlElement lnkPathXml = xmlDoc.CreateElement("lnkPath");
                XmlElement x1 = xmlDoc.CreateElement("path");
                x1.InnerText = Resources.lnkPath;
                lnkPathXml.AppendChild(x1);
                gen.AppendChild(lnkPathXml);
            }

            //保存配置文件到和 exe 同一目录
            xmlDoc.Save(Resources.exePath+"//config.xml");
        }

        /// <summary>
        /// 记取指定格式的 xml 文件 获取已注册和 lnk 文件夹路径
        /// </summary>
        /// <param name="path">xml文件的路径</param>
        /// <returns> Dictionary 对象</returns>
        public static Dictionary<int, LnkModel> ReadXmlFile()
        {
            Dictionary<int, LnkModel> dicThisInt = new Dictionary<int, LnkModel>();

            XmlDocument doc = new XmlDocument();
            //没有配置文件
            if (!File.Exists(Resources.exePath + "\\config.xml"))           
                return dicThisInt;         

            doc.Load(Resources.exePath + "\\config.xml");
            //从 根结点 选取
            XmlNodeList list = doc.SelectNodes("/KeyRegister/dicThisInt");
            int MaxNum = 0;
            foreach (XmlElement item in list)
            {
                LnkModel lnkModel = new LnkModel();
                lnkModel.id = Convert.ToInt32(item.GetElementsByTagName("id")[0].InnerText);
                lnkModel.LnkName = item.GetElementsByTagName("LnkName")[0].InnerText;
                lnkModel.LnkPath = item.GetElementsByTagName("LnkPath")[0].InnerText;
                lnkModel.HotKey = item.GetElementsByTagName("HotKey")[0].InnerText;

                //保存到已注册
                dicThisInt.Add(lnkModel.id, lnkModel);
                if (lnkModel.id > MaxNum)
                    MaxNum = lnkModel.id;
            }
            Resources.MaxNum = MaxNum + 1;

            XmlNodeList listLnkPath = doc.SelectNodes("/KeyRegister/lnkPath");
            if (listLnkPath.Count==0)
            {
                Resources.lnkPath = "0";
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
                        Resources.lnkPath = "0";
                    }
                    break;
                }
            }                      

            return dicThisInt;
        }
    }
}
