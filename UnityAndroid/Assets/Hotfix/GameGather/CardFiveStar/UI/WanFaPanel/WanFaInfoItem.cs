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
    public class WanFaInfoItemAwakeSystem : AwakeSystem<WanFaInfoItem, GameObject, WanFaInfoData>
    {
        public override void Awake(WanFaInfoItem self, GameObject go, WanFaInfoData data)
        {
            self.Awake(go, data, UIType.WanFaPanel);
        }
    }

    public class WanFaInfoItem : BaseItem<WanFaInfoData>
    {
        private Toggle _ConfirmToggle;
        private Text _DescText;
        public override void Awake(GameObject go, WanFaInfoData data,string uiType)
        {
            base.Awake(go, data, uiType);
            _ConfirmToggle = gameObject.FindChild("ConfirmToggle").GetComponent<Toggle>();
            _DescText = gameObject.FindChild("DescText").GetComponent<Text>();
            SetUI();
        }

        public void SetUI()
        {
            Show();
            _DescText.text = mData.desc;
            _ConfirmToggle.isOn = mData.isConfirm;
        }
    }

    public struct WanFaInfoData
    {
        public bool isConfirm;
        public string desc;

        public WanFaInfoData(bool confirm,string descInfo)
        {
            isConfirm = confirm;
            desc = descInfo;
        }
    }
}
