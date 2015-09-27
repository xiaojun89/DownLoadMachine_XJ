//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2015 , xiaojun
// 
// 修改记录
//		2015.09.27 作者：xiaojun (xiaojun_89 # 126_dot_com)
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility.DownLoad;

namespace DownLoadMachine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.txtUrl.Text = @"http://dlsw.baidu.com/sw-search-sp/soft/12/10086/kyrj_V2.8.4.8_setup.1442905310.exe";
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private Utility.DownLoad.DownLoad down;

        void down_SecondDownLoad(object sender, Utility.DownLoad.SecondDownLoadEventArgs e)
        {
            this.lblFileSize.Text = "文件总大小：    " + e.fileSize.ToString();
            this.lblAllTime.Text = "预计总耗时：    " + e.allTime.ToString();
            this.lblDownLoad.Text = "总下载：    " + e.downLoadSize.ToString();
            this.lblSpeed.Text = "当前速率：    " + e.speed.ToString();
            this.lblUseTime.Text = "已耗时：    " + ((int)e.useTime.TotalSeconds).ToString();
            this.progressBar1.Value = Convert.ToInt32(e.downLoadSize * 100 / e.fileSize);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (down == null || down.Status == DownLoadStatus.Idle)
            {
                HttpDownLoadFile file = (HttpDownLoadFile)DownLoadFileFactory.CreateDownLoadFile(DownLoadType.HttpDownLoad, this.txtUrl.Text);
                down = new DownLoad(file, this.txtLocalPath.Text);
                down.SecondDownLoad += new SecondDownLoadEventHandler(down_SecondDownLoad);
                down.Start();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (down != null)
            {
                down.Cancel();
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (down != null)
            {
                down.Pause();
            }
        }
    }
}