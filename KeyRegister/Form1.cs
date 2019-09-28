using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using IWshRuntimeLibrary;

namespace KeyRegister
{
    public partial class Form1 : Form
    {       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            Resources.exePath = Application.StartupPath;            
            Resources.dicThisInt = XmlFile.ReadXmlFile();
            //没有读取到路径
            if (Resources.lnkPath != "0")
            {
                checkBox1.Checked = false;
            }

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;        
            dataGridView1.AutoGenerateColumns = false;
                       
            List<LnkModel> lnkList = new LnkPath().ListLnkModel(false);
            if (lnkList.Count != 0)   //读取到了 lnk 文件
            {
                //主键程序名  值是lnk对象
                foreach (LnkModel lnkModel in lnkList)
                {
                    //没有注册 放入
                    if (Resources.HandleDicThisInt(lnkModel.LnkName))
                    {
                        Resources.dicThis.Add(lnkModel.LnkName, lnkModel);
                    }
                }
                lnkList = Resources.DicThisToLnkList();
                dataGridView1.DataSource = lnkList;
            }
            else
            {
                MessageBox.Show("默认路径文件夹 lnkFile 不包含快捷方式文件，或有非 lnk 文件！请修改路径或放入 lnk 文件！", "选择提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBox1.Checked = false;
            }           

            //注册快捷键
            Resources.RegistrationKey(Handle);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            dataGridView1.AutoGenerateColumns = false;

            List<LnkModel> lnkList = new LnkPath().ListLnkModel(true);
            if (lnkList != null)
            {
                //主键程序名  值是lnk对象
                foreach (LnkModel lnkModel in lnkList)
                {
                    //没有注册 放入
                    if (Resources.HandleDicThisInt(lnkModel.LnkName) && Resources.HandleDicThis(lnkModel.LnkName))
                    {
                        Resources.dicThis.Add(lnkModel.LnkName, lnkModel);
                    }
                }
                lnkList = Resources.DicThisToLnkList();
                dataGridView1.DataSource = lnkList;
            }
            else
            {
                MessageBox.Show("请选择只包含有快捷方式的文件夹", "选择提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBox1.Checked = false;
            }
        }

        /// <summary>
        /// 窗体关闭事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //退出程序时缷载热键 
        {
            //循环卸载注册过的快捷键
            foreach (var item in Resources.dicThisInt)
            {
                SystemHotKey.UnregisterHotKey(Handle, item.Key);
            }

            //保存配置文件
            if (Resources.lnkPath != "0" || Resources.lnkPath != null || Resources.dicThisInt.Count != 0)
            {
                XmlFile.ObjListToXml(Resources.dicThisInt);
            }
        }

        /// <summary>
        ///  dgv  双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //获取程序名字
            string lnkName = dataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();

            //注册快捷键窗口
            keyAdd keyAdd = new keyAdd(lnkName, Handle);
            keyAdd.Show();
        }

        //重写WndProc()方法，通过监视系统消息，来调用过程  
        protected override void WndProc(ref Message m)//监视Windows消息  
        {
            const int WM_HOTKEY = 0x0312;//如果m.Msg的值为0x0312那么表示用户按下了热键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    SystemHotKey.ProcessHotkey(m);//按下热键时调用ProcessHotkey()函数  
                    break;
            }
            base.WndProc(ref m); //将系统消息传递自父类的WndProc  
        }

        /// <summary>
        /// 主窗体激活事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Activated(object sender, EventArgs e)
        {
            //改变数据后，重新绑定数据
            dataGridView1.DataSource = Resources.DicThisToLnkList();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)              //用户单击托盘图标，判断用户单击是不是左键
            {
                this.WindowState = FormWindowState.Normal;               //弹出最大化当前窗口
            }
        }

        /// <summary>
        /// 程序 结束事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 重写 最小化事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            //窗体最小化
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏主窗体
                this.Hide();
            }
        }

        /// <summary>
        /// 托盘图标 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //显示主窗体
            this.Show();
        }
    }
}


