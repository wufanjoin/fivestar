using System;
using System.IO;
using System.Threading;
using Google.Protobuf;
using UnityEngine;
using LitJson;

namespace ETModel
{
	public class Init : MonoBehaviour
	{
	    public bool isNetworkBundle = false;

	    public static bool IsAdministrator = true;

        private void Start()
	    {
	        this.StartAsync().Coroutine();
	    }
        private async ETVoid StartAsync()
		{

            try
			{
#if UNITY_EDITOR
                Define.IsAsync = isNetworkBundle;
#endif

#if PLATFORM_STANDALONE_WIN
                Log.Debug("修改分辨率了啊");
			    Screen.SetResolution(640, 360, false);
#endif


				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

				DontDestroyOnLoad(gameObject);
				Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

			    Game.Scene.AddComponent<TimerComponent>();
                Game.Scene.AddComponent<GlobalConfigComponent>();
				Game.Scene.AddComponent<NetOuterComponent>();
				Game.Scene.AddComponent<ResourcesComponent>();
				Game.Scene.AddComponent<PlayerComponent>();
				Game.Scene.AddComponent<UnitComponent>();
				Game.Scene.AddComponent<ClientFrameComponent>();
				Game.Scene.AddComponent<UIComponent>();

			    gameObject.AddComponent<CoroutineMgr>();//添加协程管理类
                // 下载ab包
                await BundleHelper.DownloadBundle();

			    //加载热更项目
			    Game.Hotfix.LoadHotfixAssembly();
                // 加载配置
                Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
			    Game.Scene.AddComponent<ConfigComponent>();
			    Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                //消息分派组件
                Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatcherComponent>();
			    //直接添加Session组件
			    Game.Scene.AddComponent<SessionComponent>();

                //执行热更项目
                Game.Hotfix.GotoHotfix();
                //Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
            }
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			OneThreadSynchronizationContext.Instance.Update();
			Game.Hotfix.Update?.Invoke();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.Hotfix.LateUpdate?.Invoke();
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Hotfix.OnApplicationQuit?.Invoke();
			Game.Close();
		}
	}
}