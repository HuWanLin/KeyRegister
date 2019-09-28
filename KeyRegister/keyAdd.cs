using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyRegister
{
    public partial class keyAdd : Form
    {
        /// <summary>
        /// 程序名字
        /// </summary>
        public string LnkName { get; set; }
        public IntPtr MianIntPtr { get; set; }

        public keyAdd()
        {
            InitializeComponent();
        }

        public keyAdd(String name, IntPtr mainIntPtr)
        {
            InitializeComponent();
            LnkName = name;
            MianIntPtr = mainIntPtr;
        }

        //释放键发生 

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            //焦点放给按钮
            button1.Focus();
            SystemHotKey.KeyModifiers keysOne = new SystemHotKey.KeyModifiers();
            SystemHotKey.KeyModifiers keysTwo = new SystemHotKey.KeyModifiers();
            keysOne = SystemHotKey.KeyModifiers.Ctrl;
            keysTwo = SystemHotKey.KeyModifiers.Shift;

            if (e.Alt && e.Control)
            {
                keysOne = SystemHotKey.KeyModifiers.Alt;
                keysTwo = SystemHotKey.KeyModifiers.Ctrl;
            }
            else if (e.Alt && e.Shift)
            {
                keysOne = SystemHotKey.KeyModifiers.Alt;
                keysTwo = SystemHotKey.KeyModifiers.Shift;
            }
            else if (e.Control && e.Shift)
            {
                keysOne = SystemHotKey.KeyModifiers.Ctrl;
                keysTwo = SystemHotKey.KeyModifiers.Shift;
            }

            //注册快捷键
            Keys keySan = signKey(e);

            textBox1.Text = keysOne.ToString() + "+" + keysTwo.ToString() + "+" + keySan.ToString();

            Resources.keysOne = keysOne;
            Resources.keysTwo = keysTwo;
            Resources.keySan = keySan;
        }

        /// <summary>
        /// 判断输入的字母键
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Keys signKey(KeyEventArgs e)
        {
            Keys keys = new Keys();
            keys = Keys.Add;
            switch (e.KeyCode)
            {
                case Keys.A:
                    keys = Keys.A;
                    break;
                case Keys.B:
                    keys = Keys.B;
                    break;
                case Keys.C:
                    keys = Keys.C;
                    break;
                case Keys.D:
                    keys = Keys.D;
                    break;
                case Keys.E:
                    keys = Keys.E;
                    break;
                case Keys.F:
                    keys = Keys.F;
                    break;
                case Keys.G:
                    keys = Keys.G;
                    break;
                case Keys.H:
                    keys = Keys.H;
                    break;
                case Keys.I:
                    keys = Keys.I;
                    break;
                case Keys.J:
                    keys = Keys.J;
                    break;
                case Keys.K:
                    keys = Keys.K;
                    break;
                case Keys.L:
                    keys = Keys.L;
                    break;
                case Keys.M:
                    keys = Keys.M;
                    break;
                case Keys.N:
                    keys = Keys.N;
                    break;
                case Keys.O:
                    keys = Keys.O;
                    break;
                case Keys.P:
                    keys = Keys.P;
                    break;
                case Keys.Q:
                    keys = Keys.Q;
                    break;
                case Keys.R:
                    keys = Keys.R;
                    break;
                case Keys.S:
                    keys = Keys.S;
                    break;
                case Keys.T:
                    keys = Keys.T;
                    break;
                case Keys.U:
                    keys = Keys.U;
                    break;
                case Keys.V:
                    keys = Keys.V;
                    break;
                case Keys.W:
                    keys = Keys.W;
                    break;
                case Keys.X:
                    keys = Keys.X;
                    break;
                case Keys.Y:
                    keys = Keys.Y;
                    break;
                case Keys.Z:
                    keys = Keys.Z;
                    break;
                default:
                    keys = Keys.Add;
                    break;
            }

            return keys;
        }

        /// <summary>
        /// 输入框获得焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        /// <summary>
        /// 确定关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                bool sign = SystemHotKey.RegisterHotKey(MianIntPtr, Resources.MaxNum, (uint)Resources.keysOne | (uint)Resources.keysTwo, Resources.keySan);
                if (sign)
                {
                    Resources.dicThis[LnkName].HotKey = textBox1.Text;   //对应程序名的  快捷键  修改
                    Resources.dicThis[LnkName].id = Resources.MaxNum;    //对应程序名的  注册Id  修改
                    Resources.MaxNum++;    //注册Id++                    
                }               

                //放入已注册的 键值对 对象 
                Resources.Transformation();
            }
            this.Close();
        }
    }
}
