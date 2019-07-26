using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class DefaultUIFactory
    {
        public static UI Create(Scene scene, string type, GameObject gameObject,UIView uiView)
        {
            try
            {
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.AwayPrefixLoadBundle($"{type}.unity3d");
                GameObject bundleGameObject = (GameObject)resourcesComponent.AwayPrefixGetAsset($"{type}.unity3d", $"{type}");
                GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
                go.layer = LayerMask.NameToLayer(LayerNames.UI);
                UI ui = ComponentFactory.Create<UI,string,GameObject>(type,go);
                ui.AddComponent(uiView);
                //resourcesComponent.AwayPrefixUnloadBundle($"{type}.unity3d");
                return ui;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }

        public static void Remove(string type)
        {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().AwayPrefixUnloadBundle($"{type}.unity3d");
        }
    }
}