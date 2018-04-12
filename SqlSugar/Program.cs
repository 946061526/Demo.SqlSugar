using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlSugarEx
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            testSugar();
        }

        static void testSugar()
        {

            SqlSugarContext _context = new SqlSugarContext();

            GroupInfo entity = new GroupInfo()
            {
                GroupName = "group1",
                Remark = "test1"
            };
            _context.Insert(entity, it => new { it.Id });
        }
    }
}
