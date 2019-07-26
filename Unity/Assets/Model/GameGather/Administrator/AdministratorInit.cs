using System;
using System.IO;
using System.Threading;
using Google.Protobuf;
using UnityEngine;
using LitJson;

namespace ETModel
{
    public class AdministratorInit : MonoBehaviour
    {
        private AdministratorHotfix Hotfix=new AdministratorHotfix();
        public UnityEngine.TextAsset Hotfix_dll;

        public UnityEngine.TextAsset Hotfix_pdb;
        private void Start()
        {
            this.StartAsync().Coroutine();
        }
        private async ETVoid StartAsync()
        {

            try
            {



                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

                DontDestroyOnLoad(gameObject);
                Game.EventSystem.Add(DLLType.Model, typeof(AdministratorInit).Assembly);

                Game.Scene.AddComponent<TimerComponent>();
                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<PlayerComponent>();
                Game.Scene.AddComponent<UnitComponent>();
                Game.Scene.AddComponent<ClientFrameComponent>();
                Game.Scene.AddComponent<UIComponent>();

                gameObject.AddComponent<CoroutineMgr>();//添加协程管理类


                //加载热更项目
                //Hotfix.LoadHotfixAssembly(Hotfix_dll, Hotfix_pdb);

                //消息分派组件
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();
                //直接添加Session组件
                Game.Scene.AddComponent<SessionComponent>();

                //执行热更项目
               // Hotfix.GotoHotfix();
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