using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ETModel
{
    public static class HttpTool
    {
        public static void DowndLoadProgress(string downLoadUrl, string storageLocaPath, Action<float> progress)
        {
            CoroutineMgr.StartCoroutinee(DowndLoadFile(downLoadUrl, storageLocaPath, progress));
        }

        private static IEnumerator DowndLoadFile(string downLoadUrl, string storageLocaPath, Action<float> progress)
        {
            WWW downloadOperation = new WWW(downLoadUrl);
            CoroutineMgr.StartCoroutinee(DowndLoadFileProg(downloadOperation, progress));
            yield return downloadOperation;
            if (downloadOperation.isDone && string.IsNullOrEmpty(downloadOperation.error))
            {
                //生成文件
                Byte[] b = downloadOperation.bytes;
                Log.Debug("下载完成:" + storageLocaPath);
                File.WriteAllBytes(storageLocaPath, b);
                progress.Invoke(1);
            }
            else
            {
                progress.Invoke(-1);
            }

        }

        private static IEnumerator DowndLoadFileProg(WWW downloadOperation, Action<float> progress)
        {
            while (downloadOperation.progress < 0.99f)
            {
                Log.Debug("下载进度:" + downloadOperation.progress);
                progress.Invoke(downloadOperation.progress);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

