using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyRegister
{
    /// <summary>
    /// 程序数据类
    /// </summary>
    public class Resources
    {
        /// <summary>
        /// 注册快捷键的Id
        /// </summary>
        public static int MaxNum = 1;

        /// <summary>
        /// 没有快捷键的Id的程序信息  主键是程序名字
        /// </summary>
        public static Dictionary<string, LnkModel> dicThis = new Dictionary<string, LnkModel>();

        /// <summary>
        /// 有快捷键的Id的程序信息  主键是程序注册Id
        /// </summary>
        public static Dictionary<int, LnkModel> dicThisInt = new Dictionary<int, LnkModel>();

        /// <summary>
        /// exe 路径
        /// </summary>
        public static String exePath;

        /// <summary>
        /// lnkPath 文件夹的位置 
        /// </summary>
        public static String lnkPath;

        /// <summary>
        /// 用户输入的第一个组合键
        /// </summary>
        public static SystemHotKey.KeyModifiers keysOne;

        /// <summary>
        /// 用户输入的第二个组合键
        /// </summary>
        public static SystemHotKey.KeyModifiers keysTwo;

        /// <summary>
        /// 用户输入的字母键
        /// </summary>
        public static Keys keySan;

        /// <summary>
        /// 判断已经注册过
        /// </summary>
        /// <param name="lnkModel"></param>
        public static bool HandleDicThisInt(String LnkName)
        {
            foreach (var item in dicThisInt)
            {
                if (LnkName == item.Value.LnkName)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断 没有注册过
        /// </summary>
        /// <param name="lnkModel"></param>
        public static bool HandleDicThis(String LnkName)
        {
            foreach (var item in dicThis)
            {
                if (LnkName == item.Key)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将注册的程序放到 dicThisInt
        /// </summary>
        public static void Transformation()
        {
            //移除标记
            ArrayList arrayList = new ArrayList();
            //放到 dicThisInt
            foreach (var item in dicThis)
            {
                if (item.Value.id != 0 && !dicThisInt.ContainsKey(item.Value.id))
                {
                    dicThisInt.Add(item.Value.id, item.Value);
                    arrayList.Add(item.Key);
                }
            }
            //移除注册过的程序信息
            foreach (String item in arrayList)
            {
                dicThis.Remove(item);
            }
        }

        /// <summary>
        /// Dictionary 转 List ; 包含 已注册 和 未注册
        /// </summary>
        /// <returns></returns>
        public static List<LnkModel> DicThisToLnkList()
        {
            List<LnkModel> lnkList = new List<LnkModel>();
            //未注册快捷键的程序 
            foreach (var item in Resources.dicThis)
            {
                LnkModel lnkModel = new LnkModel();
                lnkModel.LnkName = item.Value.LnkName;
                lnkModel.HotKey = item.Value.HotKey;

                lnkList.Add(lnkModel);
            }
            //已注册快捷键的程序 
            foreach (var item in Resources.dicThisInt)
            {
                LnkModel lnkModel = new LnkModel();
                lnkModel.LnkName = item.Value.LnkName;
                lnkModel.HotKey = item.Value.HotKey;

                lnkList.Add(lnkModel);
            }
            return lnkList;
        }
        
        public static void RegistrationKey(IntPtr mainIntPtr)
        {
            foreach (var item in Resources.dicThisInt)
            {
                String[] stringKey = item.Value.HotKey.Split('+');
                SystemHotKey.KeyModifiers[] key = StringToKey(stringKey);               
                SystemHotKey.RegisterHotKey(mainIntPtr, item.Key, (uint)key[0] | (uint)key[1], signKey(stringKey[2]));
            }
        }

        private static SystemHotKey.KeyModifiers[] StringToKey(String[] stringKey)
        {
            SystemHotKey.KeyModifiers[] key = new SystemHotKey.KeyModifiers[2];
            for (int i = 0; i < stringKey.Count()-1; i++)
            {
                if (stringKey[i] == "Ctrl")                
                    key[i] = SystemHotKey.KeyModifiers.Ctrl;                
                if (stringKey[i] == "Shift")
                    key[i] = SystemHotKey.KeyModifiers.Shift;
                if (stringKey[i] == "Alt")
                    key[i] = SystemHotKey.KeyModifiers.Alt;
            }
            return key;
        }

        /// <summary>
        /// 判断输入的字母键
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Keys signKey(String keyStirng)
        {
            Keys keys = new Keys();
            keys = Keys.Add;
            switch (keyStirng)
            {
                case "A":
                    keys = Keys.A;
                    break;
                case "B":
                    keys = Keys.B;
                    break;
                case "C":
                    keys = Keys.C;
                    break;
                case "D":
                    keys = Keys.D;
                    break;
                case "E":
                    keys = Keys.E;
                    break;
                case "F":
                    keys = Keys.F;
                    break;
                case "G":
                    keys = Keys.G;
                    break;
                case "H":
                    keys = Keys.H;
                    break;
                case "I":
                    keys = Keys.I;
                    break;
                case "J":
                    keys = Keys.J;
                    break;
                case "K":
                    keys = Keys.K;
                    break;
                case "L":
                    keys = Keys.L;
                    break;
                case "M":
                    keys = Keys.M;
                    break;
                case "N":
                    keys = Keys.N;
                    break;
                case "O":
                    keys = Keys.O;
                    break;
                case "P":
                    keys = Keys.P;
                    break;
                case "Q":
                    keys = Keys.Q;
                    break;
                case "R":
                    keys = Keys.R;
                    break;
                case "S":
                    keys = Keys.S;
                    break;
                case "T":
                    keys = Keys.T;
                    break;
                case "U":
                    keys = Keys.U;
                    break;
                case "V":
                    keys = Keys.V;
                    break;
                case "W":
                    keys = Keys.W;
                    break;
                case "X":
                    keys = Keys.X;
                    break;
                case "Y":
                    keys = Keys.Y;
                    break;
                case "Z":
                    keys = Keys.Z;
                    break;              
            }
            return keys;
        }

    }
}
