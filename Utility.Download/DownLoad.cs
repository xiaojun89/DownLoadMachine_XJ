//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2015 , xiaojun
// 
// 修改记录
//		2011.12.12 原作者：lusens (lusens@foxmail.com)
//		2015.09.27 修改者：xiaojun (xiaojun_89 # 126_dot_com)
//------------------------------------------------------------

using System;
using System.IO;
using System.Threading;

namespace Utility.DownLoad
{
    /// <summary>
    /// 下载状态
    /// </summary>
    public enum DownLoadStatus
    {
        Initial,//初始化
        Idle,//空闲
        Downloading,//下载中
        Pausing,//暂停中
        Paused,//已暂停
        Canceling,//取消中
        Canceled,//已取消
        Completed//完成
    }

    /// <summary>
    /// 下载类
    /// </summary>
    public class DownLoad
    {
        #region 变量
        //准备下载的文件
        private IDownLoadFile _file;

        //准备下载的文件转换的流
        private Stream stream = null;

        //下载状态
        private DownLoadStatus status;
        public DownLoadStatus Status
        {
            get
            {
                return status;
            }
        }

        //下载文件在本机的保存位置
        private string localAdress;

        //lock锁对象
        static object locker = new object();

        //下载文件在内存中缓存的大小
        private int cacheSize;

        //读取下载文件流使用的buffer大小
        private int bufferSize;

        //已下载大小
        private long downLoadSize;

        //已下载大小复制标记
        private long downLoadSizeFlag;

        //上一秒时已下载总大小
        private long BeforSecondDownLoadSize;

        //下载已耗时
        private TimeSpan useTime;

        //最后一次下载时间
        private DateTime lastStartTime;

        //预计下载总耗时
        private TimeSpan allTime;

        //当前下载速度
        private double speed;

        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="file"></param>
        public DownLoad(IDownLoadFile file, string localAdress)
            : this(file, localAdress, 131072, 1048576)//131072=1024*128; 1048576=1024*1024
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="file"></param>
        /// <param name="localAdress"></param>
        /// <param name="bufferSize"></param>
        public DownLoad(IDownLoadFile file, string localAdress, int bufferSize)
            : this(file, localAdress, bufferSize, 1048576)
        { }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="file"></param>
        /// <param name="localAdress"></param>
        /// <param name="bufferSize"></param>
        /// <param name="cacheSize"></param>
        public DownLoad(IDownLoadFile file, string localAdress, int bufferSize, int cacheSize)
        {
            this._file = file;
            this.stream = _file.GetFileStream();
            this.localAdress = localAdress;
            this.status = DownLoadStatus.Idle;
            this.cacheSize = cacheSize;
            this.bufferSize = bufferSize;
            this.downLoadSize = 0;
            this.useTime = TimeSpan.Zero;
            this.allTime = TimeSpan.Zero;
            this.speed = 0.00;
            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.Start();
        }

        /// <summary>
        /// 定时器刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            OnDownLoad();
        }

        #endregion

        #region 控制下载状态

        /// <summary>
        /// 开始下载文件
        /// </summary>
        public void Start()
        {
            //检查文件是否存在
            CheckFileOrCreateFile();
            // 只有空闲的下载客户端才能开始
            if (this.status != DownLoadStatus.Idle)
                throw new ApplicationException("只有空闲的下载客户端才能开始.");
            // 开始在后台线程下载
            BeginDownload();
        }

        /// <summary>
        /// 暂停下载
        /// </summary>
        public void Pause()
        {
            if (this.status != DownLoadStatus.Downloading)
                throw new ApplicationException("只有正在下载的客户端才能暂停.");

            // 后台线程会查看状态，如果状态时暂停的，
            // 下载将会被暂停并且状态将随之改为暂停.
            this.status = DownLoadStatus.Pausing;
        }

        /// <summary>
        /// 重新开始下载.
        /// </summary>
        public void Resume()
        {
            // 只有暂停的客户端才能重新下载.
            if (this.status != DownLoadStatus.Paused)
                throw new ApplicationException("只有暂停的客户端才能重新下载.");

            // 开始在后台线程进行下载.
            BeginDownload();
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        public void Cancel()
        {
            // 只有正在下载的或者是暂停的客户端才能被取消.
            if (this.status != DownLoadStatus.Paused && this.status != DownLoadStatus.Downloading)
                throw new ApplicationException("只有正在下载的或者是暂停的客户端才能被取消.");

            // 后台线程将查看状态.如果是正在取消，
            // 那么下载将被取消并且状态将改成已取消.
            this.status = DownLoadStatus.Canceling;
        }

        #endregion

        /// <summary>
        /// 创建一个线程下载数据.
        /// </summary>
        private void BeginDownload()
        {
            ThreadStart threadStart = new ThreadStart(Download);
            Thread downloadThread = new Thread(threadStart);
            /* Tips：前台线程和后台线程
             * 前台线程 (Foreground Threads):  前台线程可以阻止程序退出。除非所有前台线程都结束，否则 CLR不会关闭程序。
             * 后台线程 (Background Threads):  有时候也叫 Daemon Thread 。他被 CLR 认为是不重要的执行路径，可以在任何时候舍弃。
             * 因此当所有的前台线程结束，即使还有后台线程在执行， CLR 也会关闭程序。
             */
            downloadThread.IsBackground = true;
            downloadThread.Start();
        }

        /// <summary>
        /// 具体的下载方法
        /// </summary>
        private void Download()
        {
            //进入下载状态
            this.status = DownLoadStatus.Downloading;

            //最近一次开始下载时间点
            this.lastStartTime = DateTime.Now;

            //读取服务器响应流缓存
            byte[] downloadBuffer = new byte[bufferSize];

            //服务器实际相应的字节流大小
            int bytesSize = 0;

            //实际使用缓存的大小
            long cache = 0;

            //内存缓存
            MemoryStream downloadCache = new MemoryStream(cacheSize);
            while (true)
            {
                bytesSize = stream.Read(downloadBuffer, 0, downloadBuffer.Length);

                //如果不在下载状态 或 本地缓存放不下 或 读到的字节数为0
                if (this.status != DownLoadStatus.Downloading || cache + bytesSize >= cacheSize || bytesSize == 0)
                {
                    //为避免写入文件时，当前文件正被另一进程占用而写入失败，进行五次写入尝试
                    int tryTimes = 0;
                    for (; tryTimes < 5; tryTimes++)
                    {
                        if (WriteCacheToFile(downloadCache, (int)cache))
                        {
                            downLoadSize += cache;

                            //本地缓存写入磁盘后，应清空本地缓存（或把当前位置移回到MemoryStream的头部）
                            cache = 0;
                            downloadCache.Seek(0, SeekOrigin.Begin);
                            break;
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                    if (tryTimes >= 5)
                    {
                        Console.WriteLine("DownLoad Failed");
                        break;
                    }
                }

                //如果不在下载状态 或 文件下载完成
                if (this.status != DownLoadStatus.Downloading || downLoadSize >= _file.FileSize)
                {
                    if (downLoadSize == _file.FileSize)
                    {
                        Console.WriteLine("Perfect completed!");
                    }

                    break;
                }
                //注：如果不在下载状态，那么本次bytesSize大小的数据并没有写入磁盘
                downloadCache.Write(downloadBuffer, 0, bytesSize);
                cache += bytesSize;
                downLoadSizeFlag += bytesSize;
            }
            //更改状态
            ChangeStatus();

            //清理资源
            if (stream != null)
                stream.Close();
            if (downloadCache != null)
                downloadCache.Close();
            Console.WriteLine("completed!");
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        private void CheckFileOrCreateFile()
        {
            lock (locker)
            {
                //检查文件是否存在，需要重设计业务逻辑
                if (File.Exists(localAdress))
                {
                    if (status == DownLoadStatus.Paused)
                    {
                        return;
                    }
                    else
                    {
                        File.Delete(localAdress);
                    }
                }
                using (FileStream fileStream = File.Create(localAdress))
                {
                    long createdSize = 0;
                    byte[] buffer = new byte[4096];

                    //在磁盘中建立一个空文件，大小与源文件相同
                    while (createdSize < _file.FileSize)
                    {
                        int bufferSize = (_file.FileSize - createdSize) < 4096 ? (int)(_file.FileSize - createdSize) : 4096;
                        fileStream.Write(buffer, 0, bufferSize);
                        createdSize += bufferSize;
                    }
                }
            }
        }

        /// <summary>
        /// 将内存流写入磁盘
        /// </summary>
        /// <param name="downloadCache">文件在磁盘的板寸位置</param>
        /// <param name="cachedSize">cache的大小</param>
        private bool WriteCacheToFile(MemoryStream downloadCache, int cachedSize)
        {
            bool result = false;
            lock (locker)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(localAdress, FileMode.Open)) //, FileAccess.ReadWrite, FileShare.ReadWrite
                    {
                        byte[] cacheContent = new byte[cachedSize];
                        downloadCache.Seek(0, SeekOrigin.Begin);

                        //以后要验证考虑是否会出现“即使尚未到达流的末尾，实现仍可以随意返回少于所请求的字节”的情况
                        // http://www.cnblogs.com/skyivben/archive/2009/05/26/1489244.html
                        downloadCache.Read(cacheContent, 0, cachedSize);
                        fileStream.Seek(downLoadSize, SeekOrigin.Begin);
                        fileStream.Write(cacheContent, 0, cachedSize);
                        //fileStream.Flush();
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新下载状态
        /// </summary>
        private void ChangeStatus()
        {
            if (this.status == DownLoadStatus.Pausing)
            {
                this.status = DownLoadStatus.Paused;
            }
            else if (this.status == DownLoadStatus.Canceling)
            {
                this.status = DownLoadStatus.Canceled;
            }
            else
            {
                this.status = DownLoadStatus.Completed;
                return;
            }
        }

        /// <summary>
        /// 更新下载所用时间
        /// </summary>
        private void ChangeTime()
        {
            if (this.status == DownLoadStatus.Downloading)
            {
                DateTime now = DateTime.Now;
                if (now != lastStartTime)
                {
                    useTime = useTime.Add(now - lastStartTime);
                    lastStartTime = now;
                }
            }
        }

        #region 每秒发生事件
        public event SecondDownLoadEventHandler SecondDownLoad;

        //更新下载时的当前进展状态
        public void OnDownLoad()
        {
            if (SecondDownLoad != null)
            {
                ChangeTime();
                this.speed = (downLoadSizeFlag - BeforSecondDownLoadSize) / 1024;
                BeforSecondDownLoadSize = downLoadSizeFlag;
                long temp = 0;
                if (downLoadSizeFlag != 0)
                {
                    temp = this._file.FileSize / downLoadSizeFlag * (long)this.useTime.TotalSeconds * 10000000;
                }
                this.allTime = new TimeSpan(temp);
                SecondDownLoadEventArgs e = new SecondDownLoadEventArgs(downLoadSizeFlag / 1024, useTime, allTime, speed, this._file.FileSize / 1024);
                SecondDownLoad(this, e);
            }
        }
        #endregion
    }

    /// <summary>
    /// 传递下载文件的当前进展进展
    /// </summary>
    public class SecondDownLoadEventArgs : EventArgs
    {
        public long fileSize;
        public TimeSpan allTime;
        public TimeSpan useTime;
        public long downLoadSize;
        public double speed;

        public SecondDownLoadEventArgs(long DownLoadSize, TimeSpan UseTime, TimeSpan AllTime, double DownloadSpeed, long FileSize)
        {
            this.downLoadSize = DownLoadSize;
            this.useTime = UseTime;
            this.allTime = AllTime;
            this.speed = DownloadSpeed;
            this.fileSize = FileSize;
        }
    }

    public delegate void SecondDownLoadEventHandler(object sender, Utility.DownLoad.SecondDownLoadEventArgs e);
}