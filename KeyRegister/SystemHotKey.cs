using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;



namespace KeyRegister
{
    /// <summary>
    /// 调用 API 的类
    /// </summary>
    public class SystemHotKey
    {
        /// <summary>
        /// 组合键枚举
        /// </summary>
        public enum KeyModifiers   
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            Windows = 8
        }

        /* 
         * RegisterHotKey函数原型及说明： 
         * BOOL RegisterHotKey( 
         * HWND hWnd,         // window to receive hot-key notification 
         * int id,            // identifier of hot key 
         * UINT fsModifiers, // key-modifier flags 
         * UINT vk            // virtual-key code); 
         * 参数 id为你自己定义的一个ID值 
         * 对一个线程来讲其值必需在0x0000 - 0xBFFF范围之内,十进制为0~49151 
         * 对DLL来讲其值必需在0xC000 - 0xFFFF 范围之内,十进制为49152~65535 
         * 在同一进程内该值必须唯一参数 fsModifiers指明与热键联合使用按键 
         * 可取值为：MOD_ALT MOD_CONTROL MOD_WIN MOD_SHIFT参数，或数字0为无，1为Alt,2为Control，4为Shift，8为Windows 
         * vk指明热键的虚拟键码
         */

        [System.Runtime.InteropServices.DllImport("user32.dll")] //申明API函数  

        public static extern bool RegisterHotKey(
         IntPtr hWnd, // handle to window   
         int id, // hot key identifier   
         uint fsModifiers, // key-modifier options   
         Keys vk // virtual-key code   
        );

        [System.Runtime.InteropServices.DllImport("user32.dll")] //申明API函数  
        public static extern bool UnregisterHotKey(
         IntPtr hWnd, // handle to window 
         int id // hot key identifier  

        );

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);

        /// <summary>
        /// 响应函数
        /// </summary>
        /// <param name="m"></param>
        public static void ProcessHotkey(Message m) //按下设定的键时调用该函数 
        {
            IntPtr id = m.WParam; //IntPtr用于表示指针或句柄的平台特定类型              

            for (int i = 1; i < Resources.MaxNum; i++)  //注册了多少个循环多少次
            {              
                if (id.ToString() == i.ToString())
                {
                    //读取信息，打开程序
                    String txtOne = Resources.dicThisInt[i].LnkPath;
                    string txtTwo = Resources.dicThisInt[i].LnkName;                   
                    ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder(txtOne), new StringBuilder(""), new StringBuilder(txtTwo), 1);
                }
            }
        }

        /// <summary>
        /// 调用外部 exe
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpszOp"></param>
        /// <param name="lpszFile"></param>
        /// <param name="lpszParams"></param>
        /// <param name="lpszDir"></param>
        /// <param name="FsShowCmd"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);
    }
}
