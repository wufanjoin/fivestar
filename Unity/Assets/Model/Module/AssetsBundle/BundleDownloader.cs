using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class BundleDownloaderAwakeSystem : AwakeSystem<BundleDownloader>
    {
        public override void Awake(BundleDownloader self)
        {
            self.bundles = new Queue<string>();
            self.downloadedBundles = new HashSet<string>();
            self.downloadingBundle = "";
        }
    }
    /// <summary>
    /// 用来对比web端的资源，比较md5，对比下载资源的管理类
    /// </summary>
    public class BundleDownloader : Entity
    {
        private VersionConfig remoteVersionConfig;

        public Queue<string> bundles;

        public long TotalSize;

        public HashSet<string> downloadedBundles;

        public string downloadingBundle;

        public UnityWebRequestAsync webRequest;

        public Action pFinishAndDisposeCall;
        public Action<string> pLoserCall;
        //当前的进度
        public int Progress
        {
            get
            {
                if (this.TotalSize == 0)
                {
                    return 0;
                }
                long alreadyDownloadBytes = 0;
                foreach (string downloadedBundle in this.downloadedBundles)
                {
                    long size = this.remoteVersionConfig.FileInfoDict[downloadedBundle].Size;
                    alreadyDownloadBytes += size;
                }
                if (this.webRequest != null)
                {
                    alreadyDownloadBytes += (long)this.webRequest.Request.downloadedBytes;
                }
                return (int)(alreadyDownloadBytes * 100f / this.TotalSize);
            }
        }

        private string downloadFoldr = "";

        //对比本地资源和服务器资源
        public async ETTask StartAsync(string fileFoldr = "", string versionName = "Version.txt")
        {
            downloadFoldr = fileFoldr;
            // 获取远程的Version.txt
            string versionUrl = "";
            try
            {
                using (UnityWebRequestAsync webRequestAsync = ComponentFactory.Create<UnityWebRequestAsync>())
                {
                    versionUrl = Path.Combine(GlobalConfigComponent.Instance.GlobalProto.GetUrl(), "StreamingAssets", downloadFoldr, versionName);
                    await webRequestAsync.DownloadAsync(versionUrl);
                    remoteVersionConfig = JsonHelper.FromJson<VersionConfig>(webRequestAsync.Request.downloadHandler.text);
                }
            }
            catch (Exception e)
            {
                pLoserCall?.Invoke(e.ToString());
                throw new Exception($"url: {versionUrl}", e);
            }

            // 删掉远程不存在的文件
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(PathHelper.AppHotfixResPath, downloadFoldr));
            if (directoryInfo.Exists)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    if (remoteVersionConfig.FileInfoDict.ContainsKey(fileInfo.Name))
                    {
                        continue;
                    }
                    if (fileInfo.Name == versionName)
                    {
                        continue;
                    }
                    fileInfo.Delete();
                }
            }
            else
            {
                directoryInfo.Create();
            }

     
            // 对比MD5
            foreach (FileVersionInfo fileVersionInfo in remoteVersionConfig.FileInfoDict.Values)
            {
             
                // 对比md5
                string localFileMD5 = BundleHelper.GetBundleMD5(fileVersionInfo.File);
                if (fileVersionInfo.MD5 == localFileMD5)
                {
                    continue;
                }
                this.bundles.Enqueue(fileVersionInfo.File);
                this.TotalSize += fileVersionInfo.Size;
            }
            await DownloadAsync();
        }

        //开始下载资源
        private async ETTask DownloadAsync()
        {
            if (this.bundles.Count == 0 && this.downloadingBundle == "")
            {
                return;
            }
            try
            {
                while (true)
                {
                    if (this.bundles.Count == 0)
                    {
                        break;
                    }
                    this.downloadingBundle = this.bundles.Dequeue();
                    while (true)
                    {
                        try
                        {
                            using (this.webRequest = ComponentFactory.Create<UnityWebRequestAsync>())
                            {
                                string cmbinPath = Path.Combine(GlobalConfigComponent.Instance.GlobalProto.GetUrl(), "StreamingAssets", downloadFoldr, this.downloadingBundle);
                                await this.webRequest.DownloadAsync(cmbinPath);
                                byte[] data = this.webRequest.Request.downloadHandler.data;

                                string path = Path.Combine(PathHelper.AppHotfixResPath, downloadFoldr, this.downloadingBundle);
                                string directoryName = path.Substring(0, path.LastIndexOf('/'));
                                PathHelper.CreateDirectory(directoryName);//如果文件夹不存在就创建
                                using (FileStream fs = new FileStream(path, FileMode.Create))
                                {
                                    fs.Write(data, 0, data.Length);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error($"download bundle error: {this.downloadingBundle}\n{e}");
                            continue;
                        }
                        break;
                    }
                    this.downloadedBundles.Add(this.downloadingBundle);
                    this.downloadingBundle = "";
                    this.webRequest = null;
                }
            }
            catch (Exception e)
            {
                pLoserCall?.Invoke(e.ToString());
                Log.Error(e);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            bundles.Clear();
            downloadedBundles.Clear();
            remoteVersionConfig = null;
            TotalSize = 0;
            pFinishAndDisposeCall?.Invoke();
        }
    }
}
