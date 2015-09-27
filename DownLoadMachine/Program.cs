//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2015 , xiaojun
// 
// 修改记录
//		2015.09.27 作者：xiaojun (xiaojun_89 # 126_dot_com)
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DownLoadMachine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
