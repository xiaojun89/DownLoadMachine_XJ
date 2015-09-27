//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2015 , xiaojun
// 
// 修改记录
//		2015.09.27 作者：xiaojun (xiaojun_89 # 126_dot_com)
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Utility.DownLoad
{
    public class HttpDownLoadFile : IDownLoadFile
    {
        private Stream stream;
        private long fileSize;
        public HttpDownLoadFile(string url)
        {
            HttpWebRequest fileRequest = (HttpWebRequest)WebRequest.Create(url);

            //获取目标文件的大小
            fileSize = ((HttpWebResponse)fileRequest.GetResponse()).ContentLength;

            HttpWebResponse fileResponse = (HttpWebResponse)fileRequest.GetResponse();
            stream = fileResponse.GetResponseStream();
        }

        #region IDownLoadFile Members

        public Stream GetFileStream()
        {
            return stream;
        }

        public long FileSize
        {
            get { return fileSize; }
        }

        #endregion
    }

    public class DownLoadFileFactory
    {
        public static IDownLoadFile CreateDownLoadFile(DownLoadType DLT, string URL)
        {
            IDownLoadFile _file;
            switch (DLT)
            {
                case DownLoadType.HttpDownLoad:
                    _file = new HttpDownLoadFile(URL);
                    break;
                default:
                    _file = new HttpDownLoadFile(URL);
                    break;
            }
            return _file;
        }
    }

    public interface IDownLoadFile
    {
        Stream GetFileStream();
        long FileSize { get; }
    }

    public enum DownLoadType
    {
        HttpDownLoad
    }

}
