using System;
using System.IO;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ETModel
{
	public static class BundleHelper
	{
		public static async ETTask DownloadBundle()
		{
			if (Define.IsAsync)
			{
				try
				{
				    Game.EventSystem.Run(EventIdType.LoadingBegin);
                    Game.Scene.AddComponent<BundleDownloaderComponent>();
                    await GameUpdateMgr.Ins.UpdateGame();
					Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle("StreamingAssets");
					ResourcesComponent.SetAssetBundleManifestObject((AssetBundleManifest)Game.Scene.GetComponent<ResourcesComponent>().BundleNameGetAsset("StreamingAssets", "AssetBundleManifest"));
				    Game.EventSystem.Run(EventIdType.LoadingFinish);
                }
				catch (Exception e)
				{
				    UILoadingComponent.Ins.ShowHint("网络无法连接,请检查您的网络", (bol) =>
				    {
				        Application.Quit();
				        Log.Error("退出了游戏");
				    }, PopHintOptionType.Single,"确 定");

                    Log.Error(e);
				}
			}
			else
			{
#if UNITY_EDITOR
                ResourcesComponent.RecordAssbundlsName(AssetDatabase.GetAllAssetBundleNames());
#endif
            }
        }

		public static string GetBundleMD5(string bundleName)
		{
			string path = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
			if (File.Exists(path))
			{
				return MD5Helper.FileMD5(path);
			}
			
			//if (streamingVersionConfig.FileInfoDict.ContainsKey(bundleName))
			//{
			//	return streamingVersionConfig.FileInfoDict[bundleName].MD5;	
			//}

			return "";
		}
	}
}
