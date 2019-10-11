using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyRegister
{
    /// <summary>
    /// 快捷方式实体类
    /// </summary>
    public class LnkModel
    {
        /// <summary>
        /// 快捷方式名字
        /// </summary>
        public string LnkName { get; set; }

        /// <summary>
        /// 快捷方式路径
        /// </summary>
        public string LnkPath { get; set; }

        public string HotKey { get; set; }

        public int id { get; set; }
    }
}
