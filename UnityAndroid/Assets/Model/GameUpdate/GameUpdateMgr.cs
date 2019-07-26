using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public class GameUpdateMgr : Single<GameUpdateMgr>
    {
        public Action<float> JieYaResAction;//解压资源
        public Action<float,int,int> UpdateResAction;//更新资源
        public Action<float> DownPackeageAction;//下载安装包
        private bool isDownPackTask=true;


        public async ETTask UpdateGame()
        {
            //本来想StreamingAssets复制到热更目录 结果安卓平台只能用WWW异步加载文件 直接下载
            //if (!File.Exists(PathHelper.LocalGameVersionsPath))
            //{
            //    JieYaResAction?.Invoke(0);
            //    CopyStreamingAssetsToHotfixResPath();
            //}
            await GameVersionsConfigMgr.Ins.InitVersionsConfig();
            if (!GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.IsServerNormal)
            {
                UILoadingComponent.Ins.ShowHint(GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.MaintainAnnouncement, QuitGame,
                    PopHintOptionType.Single,"确定");
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(20000);
                QuitGame(true);
                return;   
            }
            if (GameVersionsConfigMgr.Ins.LocalGameVersionsConfig.Version>= GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.Version)
            {
                if (!GameVersionsConfigMgr.Ins.IsHaveLocalRes)
                {
                    //如果没有本地资源 是要热更的
                    await HotfixUpdate();
                }
                //不用更新 什么都不做
            }
            else if (GameVersionsConfigMgr.Ins.LocalGameVersionsConfig.Version < GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.ColdUpdateVersion)
            {
               
                UILoadingComponent.Ins.ShowHint("检测到新版本是否下载?", ColdUpdate);
                while (isDownPackTask)
                {
                    await Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                }
            }
            else
            {
                //热更
                await HotfixUpdate();
            }
        }

        public void QuitGame(bool isConfirm)
        {
            Application.Quit();
            Log.Error("退出游戏");
        }
        private void ColdUpdate(bool isColdUpdate)
        {
            if (!isColdUpdate)
            {
                QuitGame(true);
                 isDownPackTask = false;
                return;
            }
            if (Application.platform == RuntimePlatform.Android)
            {
                HttpTool.DowndLoadProgress(GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.ApkDownloadUrl, PathHelper.ApkSavePath, DownApkCall);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //直接发起安装ipa
                SdkMgr.Ins.installApk(GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.IOSDownloadUrl);
                QuitGame(true);
            }
            else
            {
                isDownPackTask = false;
            }
        }
        //下载
        public void DownApkCall(float prog)
        {
            DownPackeageAction?.Invoke(prog);
            
            if (prog == 1)
            {
                SdkMgr.Ins.installApk(PathHelper.ApkSavePath);
                isDownPackTask = false;
            }
            else if (prog == -1)
            {
                Application.Quit();//下载错误退出游戏
                isDownPackTask = false;
            }
        }
        private async ETTask HotfixUpdate()
        {
       
            BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
            for (int i = 0; i < GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.ToyGameVersionsArray.Count; i++)
            {
                for (int j = 0; j < GameVersionsConfigMgr.Ins.LocalGameVersionsConfig.ToyGameVersionsArray.Count; j++)
                {
                    //对比服务器版本
                    if (GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.ToyGameVersionsArray[i].Id ==
                        GameVersionsConfigMgr.Ins.LocalGameVersionsConfig.ToyGameVersionsArray[j].Id)
                    {
                        //如果本地版本比服务器版本 高或者相同 就不用更新
                        if (GameVersionsConfigMgr.Ins.LocalGameVersionsConfig.ToyGameVersionsArray[j].Version >=
                            GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.ToyGameVersionsArray[i].Version)
                        {
                            continue;
                        }
                        BundleDownSchedule bundleDownSchedule = new BundleDownSchedule();
                        UndateResActionInvoke(bundleDownSchedule,i+1, GameVersionsConfigMgr.Ins.ServerGameVersionsConfig.ToyGameVersionsArray.Count);
                        await bundleDownloaderComponent.StartAsync(GameVersionsConfigMgr.Ins.ServerGameVersionsConfig
                            .ToyGameVersionsArray[i],bundleDownSchedule);
                    }
                }
            }
            GameVersionsConfigMgr.Ins.SaveServerVersionsToLocal(); //更新完成 存储服务器版本信息到本地 
        }

        public async void UndateResActionInvoke(BundleDownSchedule bundleDownSchedule, int currNum, int totalNum)
        {
            bundleDownSchedule.pLoserCall += DownError;
            while (!bundleDownSchedule.IsFinishDown)
            {
                await Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
               //Log.Debug(bundleDownSchedule.Progress.ToString());
                UpdateResAction?.Invoke(bundleDownSchedule.Progress, currNum, totalNum);
            }
            Log.Debug("下载完成currNum:"+ currNum);
        }

        public void DownError(string error)
        {
            Log.Error("下载失败:"+ error);
            UILoadingComponent.Ins.ShowHint("下载失败 请重试", (bol) =>
            {
                Application.Quit();
                Log.Error("退出了游戏");
            }, PopHintOptionType.Single, "确 定");
        }
    }
}
