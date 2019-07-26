using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class OptionItemGoAwakeSystem : AwakeSystem<OptionItem,GameObject, CardFiveStarRoomConfig>
    {
        public override void Awake(OptionItem self,GameObject go, CardFiveStarRoomConfig data)
        {
            self.Awake(go, data, UIType.CreateRoomPanel);
        }
    }

    public class OptionItem : BaseItem<CardFiveStarRoomConfig>
    {
        #region 脚本工具生成的代码
        private Text mTitleText;
        private GameObject mOptionToggleItem;
        public override void Awake(GameObject go, CardFiveStarRoomConfig data, string uiType)
        {
            base.Awake(go, data, uiType);
            mTitleText = rc.Get<GameObject>("TitleText").GetComponent<Text>();
            mOptionToggleItem = rc.Get<GameObject>("OptionToggleItem");
            InitPanel();
        }
        #endregion
        private Dictionary<int, OptionToggleItem> _OptionToggleItems=new Dictionary<int, OptionToggleItem>();
        public void InitPanel()
        {
            mTitleText.text = mData.Title+":";
            Transform optionToggleParent = mOptionToggleItem.transform.parent;
            
            for (int i = 0; i < mData.SelectConfigs.Count-1; i++)
            {
                GameObject.Instantiate(mOptionToggleItem, optionToggleParent);
            }
            for (int i = 0; i < optionToggleParent.childCount; i++)
            {
                OptionToggleItem optionToggleItem=optionToggleParent.GetChild(i)
                    .AddItemIfHaveInit<OptionToggleItem, RoomSingleSelectConfig>(mData.SelectConfigs[i]);
                _OptionToggleItems[mData.SelectConfigs[i].Value] = optionToggleItem;
            }
        }
       
        public int PitchValue
        {
            get {
                foreach (var optionToggle in _OptionToggleItems)
                {
                    if (optionToggle.Value.ToggleIsOn)
                    {
                        return optionToggle.Key;
                    }
                }
                Log.Error("创建房间选项Id"+mData.Id+"没有一个是选中的");
                return mData.DefaultValue;
            }
            set
            {
                if (_OptionToggleItems.ContainsKey(value))
                {
                    _OptionToggleItems[value].ToggleIsOn = true;
                }
                else
                {
                    foreach (var toggle in _OptionToggleItems.Values)
                    {
                        toggle.ToggleIsOn = true;
                        break;
                    }
                }
            }
        }

        public int ConsumeJewelCount
        {
            get
            {
                foreach (var optionToggle in _OptionToggleItems)
                {
                    if (optionToggle.Value.ToggleIsOn)
                    {
                        return optionToggle.Value.mData.NeedJewelNum;
                    }
                }
                Log.Error("创建房间选项Id" + mData.Id + "没有一个是选中的");
                return 0;
            }
        }
    }
}
