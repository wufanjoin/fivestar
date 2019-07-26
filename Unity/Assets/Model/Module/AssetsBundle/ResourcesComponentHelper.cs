using System;
using System.Collections.Generic;

using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class ResourcesComponentAwake : AwakeSystem<ResourcesComponent>
    {
        public override void Awake(ResourcesComponent self)
        {
            ResourcesComponent.Ins = self;
        }
    }
    public static class ResourcesComponentHelper
    {
        //省略 前缀 加载AB包
        public static void AwayPrefixLoadBundle(this ResourcesComponent resourcesComponent, string assetBundleName)
        {

            assetBundleName = assetBundleName.ToLower();
            if (ResourcesComponent.bundlesNameDictionary.ContainsKey(assetBundleName))
            {
                resourcesComponent.LoadBundle(ResourcesComponent.bundlesNameDictionary[assetBundleName] + "/" + assetBundleName);
            }
            else
            {
                resourcesComponent.LoadBundle(assetBundleName);
            }
        }
        //省略 前缀 加载资源
        public static UnityEngine.Object AwayPrefixGetAsset(this ResourcesComponent resourcesComponent, string assetBundleName, string prefab)
        {
            assetBundleName = assetBundleName.ToLower();
            if (ResourcesComponent.bundlesNameDictionary.ContainsKey(assetBundleName))
            {
                return resourcesComponent.BundleNameGetAsset(ResourcesComponent.bundlesNameDictionary[assetBundleName] + "/" + assetBundleName, prefab);
            }
            else
            {
                return resourcesComponent.BundleNameGetAsset(assetBundleName, prefab);
            }
        }
        //省略 前缀 卸载AB包
        public static void AwayPrefixUnloadBundle(this ResourcesComponent resourcesComponent, string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();
            if (ResourcesComponent.bundlesNameDictionary.ContainsKey(assetBundleName))
            {
                resourcesComponent.UnloadBundle(ResourcesComponent.bundlesNameDictionary[assetBundleName] + "/" + assetBundleName);
            }
            else
            {
                resourcesComponent.UnloadBundle(assetBundleName);
            }
        }

        //从AB包中 加载资源 如果AB没有加载 会先加载AB包
        public static readonly Dictionary<string, UnityEngine.Object> cacheResRecord = new Dictionary<string, UnityEngine.Object>();
        private static string[] abBundleNames = new[] { "common", "" };
        public static UnityEngine.Object GetResoure(this ResourcesComponent resourcesComponent, string uiType, string resName)
        {
            try
            {
                if (cacheResRecord.ContainsKey(resName))
                {
                    return cacheResRecord[resName];
                }
                abBundleNames[1] = uiType;
                foreach (var bundleName in abBundleNames)
                {
                    GameObject ReferenceCollectorGo = (GameObject)resourcesComponent.AwayPrefixGetAsset($"{bundleName}.unity3d", "ReferenceCollector");
                    UnityEngine.Object bundleGameObject = ReferenceCollectorGo.Get<UnityEngine.Object>(resName);
                    if (bundleGameObject != null)
                    {
                        cacheResRecord.Add(resName, bundleGameObject);
                        return bundleGameObject;
                    }
                }


            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
            return null;
        }
    }
}
