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

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoGenerateColumns = false;

            Resources.exePath = Application.StartupPath;
            Resources.dicThisInt = XmlFile.ReadXmlFile();

            BindingDgv(false);
        }


        private void BindingDgv(bool flagReset)
        {
            List<LnkModel> lnkList = new LnkPath().ListLnkModel();
            if (lnkList == null)  //有其它的文件
            {
                DialogResult dr = MessageBox.Show("文件夹有非 lnk 文件！请修改路径或者删除非 lnk 文件。", "选择提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (dr == DialogResult.Cancel)
                {
                    Application.Exit();
                }
                if (dr == DialogResult.OK)
                {
                    XmlFile.ChoiceLnkFile();
                    BindingDgv(false);
                }
            }
            foreach (LnkModel lnkModel in lnkList)
            {
                if (Resources.HandleDicThisInt(lnkModel.LnkName))
                {
                    if (Resources.HandleDicThis(lnkModel.LnkName))
                    {
                        Resources.dicThis.Add(lnkModel.LnkName, lnkModel);
                    }
                }
            }
            List<LnkModel> lnkListData = Resources.DicThisToLnkList();
            if (flagReset)
            {
                List<LnkModel> lnkListString = new List<LnkModel>();
                foreach (LnkModel item in lnkListData)   //有的数据
                {
                    bool flag = true;
                    foreach (var item2 in lnkList)    //获取到的数据
                    {
                        if (item.LnkName == item2.LnkName)
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        lnkListString.Add(item);
                    }
                }
                foreach (LnkModel item in lnkListString)
                {
                    if (item.id == 0)  //没有注册过                    
                        Resources.dicThis.Remove(item.LnkName);
                    else
                        Resources.dicThisInt.Remove(item.id);
                }
                dataGridView1.DataSource = lnkList;
            }
            if (lnkList.Count == 0)
            {
                try
                {
                    Resources.dicThis.Clear();
                    Resources.dicThisInt.Clear();
                }
                catch (Exception)
                {
                    //throw;
                }
                lnkListData.Clear();
            }
            dataGridView1.DataSource = lnkListData;

            //注册快捷键
            Resources.RegistrationKey(Handle);
        }

        /// <summary>
        /// 更改文件夹 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            if (XmlFile.ChoiceLnkFile())
            {
                Uninstall();
                Resources.dicThis = new Dictionary<string, LnkModel>();
                Resources.dicThisInt = new Dictionary<int, LnkModel>();
                List<LnkModel> lnkList = new LnkPath().ListLnkModel();
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
                BindingDgv(false);
            }
        }

        /// <summary>
        /// 窗体关闭事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //退出程序时缷载热键 
        {

            Uninstall();
            XmlFile.ObjListToXml();
        }

        /// <summary>
        /// 循环卸载注册过的快捷键
        /// </summary>
        private void Uninstall()
        {
            if (Resources.dicThisInt != null)
            {
                foreach (var item in Resources.dicThisInt)
                {
                    SystemHotKey.UnregisterHotKey(Handle, item.Key);
                }
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

        /// <summary>
        /// 刷新单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindingDgv(true);
        }
    }
}



