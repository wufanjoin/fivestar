using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [ObjectSystem]
    public class VersionsShowComponentAwakeSystem : AwakeSystem<VersionsShowComponent>
    {
        public override void Awake(VersionsShowComponent self)
        {
            self.Awake();
        }
    }
    public class VersionsShowComponent : Component
    {
        public void Awake()
        {
            GameObject versionShow = new GameObject("versionText", typeof(RectTransform), typeof(Text));
            versionShow.GetComponent<Text>().text = "v" + GameVersionsConfigMgr.Ins.Version;
            versionShow.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            versionShow.GetComponent<Text>().fontSize = 18;
            versionShow.GetComponent<Text>().color = VectorHelper.GetLucencyWhiteColor(0.6f);
            versionShow.transform.SetParent(GameObject.Find("Global/UI/ErrorCanvas").transform);
            versionShow.transform.localScale = Vector3.one;
            versionShow.transform.localPosition = new Vector3(635, -385, 0);
        }
    }
}
