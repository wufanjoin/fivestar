using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [ObjectSystem]
    public class OptionToggleItemAwakeSystem : AwakeSystem<OptionToggleItem, GameObject, RoomSingleSelectConfig>
    {
        public override void Awake(OptionToggleItem self, GameObject go, RoomSingleSelectConfig data)
        {
            self.Awake(go, data, UIType.CreateRoomPanel);
        }
    }

    public class OptionToggleItem : BaseItem<RoomSingleSelectConfig>
    {
        #region 脚本工具生成的代码
        private Toggle mSelectToggle;
        private Text mDescText;
        public override void Awake(GameObject go, RoomSingleSelectConfig data, string uiType)
        {
            base.Awake(go, data, uiType);
            mSelectToggle = rc.Get<GameObject>("SelectToggle").GetComponent<Toggle>();
            mDescText = rc.Get<GameObject>("DescText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mDescText.text = mData.Describe;
            mSelectToggle.Add(ToggleEvent);
        }

        public void ToggleEvent(bool isOn)
        {
            if (isOn)
            {
                if(mData.NeedJewelNum!=0)
                {
                    UIComponent.GetUiView<CreateRoomPanelComponent>().ConsumeJewelAmountChange();
                }
            }
        }
        public bool ToggleIsOn
        {
            get { return mSelectToggle.isOn; }
            set { mSelectToggle.isOn = value; }
        }
    }
}
