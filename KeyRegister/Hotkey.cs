using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KeyRegister
{
    public delegate void HotkeyEventHandler(int HotKeyID);
    public class Hotkey : System.Windows.Forms.IMessageFilter
    {
        System.Collections.Hashtable keyIDs = new System.Collections.Hashtable();
        IntPtr hWnd;

        /// <summary>
        /// 消息对列
        /// </summary>
        public event HotkeyEventHandler OnHotkey;
        public enum KeyFlags
        {
            MOD_ALT = 0x1,
            MOD_CONTROL = 0x2,
            MOD_SHIFT = 0x4,
            MOD_WIN = 0x8
        }

        [DllImport("user32.dll")]
        public static extern UInt32 RegisterHotKey(IntPtr hWnd, UInt32 id, UInt32 fsModifiers, UInt32 vk);

        [DllImport("user32.dll")]
        public static extern UInt32 UnregisterHotKey(IntPtr hWnd, UInt32 id);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll")]
        public static extern UInt32 GlobalDeleteAtom(UInt32 nAtom);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hWnd">窗口标记</param>
        public Hotkey(IntPtr hWnd)
        {
            //窗口标记
            this.hWnd = hWnd;
            System.Windows.Forms.Application.AddMessageFilter(this);
        }

        /// <summary>
        /// 注册快捷键
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="keyflags"></param>
        /// <returns></returns>
        public int RegisterHotkey(System.Windows.Forms.Keys Key, uint fsModifiers)
        {
            UInt32 hotkeyid = GlobalAddAtom(System.Guid.NewGuid().ToString());
            RegisterHotKey((IntPtr)hWnd, hotkeyid,fsModifiers, (UInt32)Key);           
            keyIDs.Add(hotkeyid, hotkeyid);
            return (int)hotkeyid;
        }

        /// <summary>
        /// 注销快捷键
        /// </summary>
        public void UnregisterHotkeys()
        {
            System.Windows.Forms.Application.RemoveMessageFilter(this);
            foreach (UInt32 key in keyIDs.Values)
            {
                UnregisterHotKey(hWnd, key);
                GlobalDeleteAtom(key);
            }
        }

        /// <summary>
        /// 消息队列
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x312)
            {
                if (OnHotkey != null)
                {
                    foreach (UInt32 key in keyIDs.Values)
                    {
                        if ((UInt32)m.WParam == key)
                        {
                            OnHotkey((int)m.WParam);
                            return true;
                        }
                    }
                }
            }
            return false;
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