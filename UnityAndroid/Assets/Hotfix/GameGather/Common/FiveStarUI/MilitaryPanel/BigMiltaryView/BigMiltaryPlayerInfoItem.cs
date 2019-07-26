using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class BigMiltaryPlayerInfoItemAwakeSystem : AwakeSystem<BigMiltaryPlayerInfoItem, GameObject, MiltaryPlayerInfo>
    {
        public override void Awake(BigMiltaryPlayerInfoItem self, GameObject go, MiltaryPlayerInfo data)
        {
            self.Awake(go, data, UIType.MilitaryPanel);
        }
    }
    public class BigMiltaryPlayerInfoItem : BaseItem<MiltaryPlayerInfo>
    {
        #region 脚本工具生成的代码
        private Text mNameText;
        private GameObject mHouseIconGo;
        private Text mGetScoreText;
        public override void Awake(GameObject go, MiltaryPlayerInfo data, string uiType)
        {
            base.Awake(go, data, uiType);
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mHouseIconGo = rc.Get<GameObject>("HouseIconGo");
            mGetScoreText = rc.Get<GameObject>("GetScoreText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mHouseIconGo.SetActive(false);
            if (mData.TotalScore>=0)
            {
                mGetScoreText.color = MilitaryPanelComponent.AddScoreColor;
            }
            else
            {
                mGetScoreText.color = MilitaryPanelComponent.SubScoreColor;
            }
            mNameText.text = mData.Name;
            mGetScoreText.text = mData.TotalScore.ToString();
        }

        public void SetHouse()
        {
            mHouseIconGo.SetActive(true);
        }
    }
}
