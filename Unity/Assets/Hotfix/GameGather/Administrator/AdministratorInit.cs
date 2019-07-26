using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class AdministratorInit
    {

        public static async ETTask<Sprite> etSprite()
        {
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(500);
            return null;
        }
        public static void Start()
        {


            try
            {

                // 注册热更层回调
                ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };


                Game.Scene.AddComponent<UIComponent>();
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();

                //直接添加Session组件
                Game.Scene.AddComponent<SessionComponent>();

                Log.Debug("成功初始化");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }



        public static void OnApplicationQuit()
        {
            Game.Close();
        }
    }
}