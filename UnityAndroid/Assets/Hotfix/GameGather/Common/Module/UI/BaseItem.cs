using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class BaseItem<T> : InitBaseItem
    {
        public T mData;
       
        protected ReferenceCollector rc;

        public string pViewType { private set; get; }

        public UnityEngine.Object GetResoure(string resName)
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            return resourcesComponent.GetResoure(pViewType, resName);
        }

        public T1 GetResoure<T1>(string resName) where T1: UnityEngine.Object
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            return resourcesComponent.GetResoure(pViewType, resName) as T1;
        }
        public virtual void Awake(GameObject go, T data,string uiType)
        {
            mData = data;
            Init(go);
            pViewType = uiType;
            rc = gameObject.GetComponent<ReferenceCollector>();
        }

    }
}
