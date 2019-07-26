using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if !ILRuntime
using System.Reflection;
#endif

namespace ETModel
{
    public sealed  class AdministratorHotfix : Object
    {
#if !ILRuntime
        private Assembly assembly;


        private IStaticMethod start;
        private List<Type> hotfixTypes;

        public Action Update;
        public Action LateUpdate;
        public Action OnApplicationQuit;

        public void GotoHotfix()
        {

            this.start.Run();
        }

        public List<Type> GetHotfixTypes()
        {
            return this.hotfixTypes;
        }

        public void LoadHotfixAssembly(TextAsset dll, TextAsset pdb)
        {
            Game.Scene.GetComponent<ResourcesComponent>().LoadBundle($"code.unity3d");
            GameObject code = (GameObject)Game.Scene.GetComponent<ResourcesComponent>().GetAsset("code.unity3d", "Code");

            byte[] assBytes = dll.bytes;
            byte[] pdbBytes = pdb.bytes;


            Log.Debug($"当前使用的是Mono模式");

            this.assembly = Assembly.Load(assBytes, pdbBytes);

            Type hotfixInit = this.assembly.GetType("ETHotfix.AdministratorInit");
            this.start = new MonoStaticMethod(hotfixInit, "Start");

            this.hotfixTypes = this.assembly.GetTypes().ToList();

            Game.Hotfix.SetHotfixTypes(this.hotfixTypes);
        }
#endif
    }
}