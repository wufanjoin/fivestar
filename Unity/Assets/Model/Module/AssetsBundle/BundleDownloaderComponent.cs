using System;


namespace ETModel
{
    [ObjectSystem]
    public class UiBundleDownloaderComponentAwakeSystem : AwakeSystem<BundleDownloaderComponent>
    {
        public override void Awake(BundleDownloaderComponent self)
        {
            //self.pDownloaders = new List<BundleDownloader>();
        }
    }

    public class BundleDownSchedule
    {
        public Action pFinishAndDisposeCall;
        public Action<string> pLoserCall;
        private BundleDownloader mBundleDownloader;
        public bool IsFinishDown = false;
        public int Progress
        {
            get
            {
                if (mBundleDownloader == null)
                {
                    return 0;
                }
                return mBundleDownloader.Progress;
            }
        }

        public void SetBundleDownloader(BundleDownloader bundleDownloader)
        {
            mBundleDownloader = bundleDownloader;
            mBundleDownloader.pFinishAndDisposeCall += pFinishAndDisposeCall;
            mBundleDownloader.pFinishAndDisposeCall += FinishAciton;
            mBundleDownloader.pLoserCall += pLoserCall;
        }

        private void FinishAciton()
        {
            IsFinishDown = true;
        }
    }
    /// <summary>
    /// 用来对比web端的资源，比较md5，对比下载资源的管理类
    /// </summary>
    public class BundleDownloaderComponent : Component
    {
        //public List<BundleDownloader> pDownloaders;


        //开始下载
        public async ETTask StartAsync(ToyGameVersions toyGameVersions, BundleDownSchedule downloader = null)
        {
            await StartAsync(toyGameVersions.DownloadFolder, toyGameVersions.VersionFile, downloader);
        }
        //开始下载
        public async ETTask StartAsync(string fileFoldr = "", string versionName = "Version.txt", BundleDownSchedule downloader = null)
        {
            BundleDownloader bundleDownloader = ComponentFactory.Create<BundleDownloader>();
            downloader?.SetBundleDownloader(bundleDownloader);
            await bundleDownloader.StartAsync(fileFoldr, versionName);
            DisposeBundleDownloader(bundleDownloader);
        }
        //销毁bundle下载工具对象
        public void DisposeBundleDownloader(BundleDownloader bundleDownloader)
        {
            bundleDownloader.Dispose();
        }
    }
}
