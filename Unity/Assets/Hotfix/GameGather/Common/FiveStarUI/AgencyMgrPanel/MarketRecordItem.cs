using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class MarketRecordItemAwakeSystem : AwakeSystem<MarketRecordItem, GameObject, MarketInfo>
    {
        public override void Awake(MarketRecordItem self, GameObject go, MarketInfo data)
        {
            self.Awake(go, data, UIType.AgencyMgrPanel);
        }
    }
    public class MarketRecordItem : BaseItem<MarketInfo>
    {
        #region 脚本工具生成的代码
        private Text mIdText;
        private Text mNameText;
        private Text mTimeText;
        private Text mNumText;
        public override void Awake(GameObject go, MarketInfo data, string uiType)
        {
            base.Awake(go, data, uiType);
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mNumText = rc.Get<GameObject>("NumText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mIdText.text ="买家ID:"+mData.MaiJiaUserId.ToString();
            mNameText.text = mData.MaiJiaName;
            mTimeText.text=TimeTool.ConvertLongToTimeDesc(mData.Time);
            mNumText.text = "卖出:" + mData.JewelNum;
        }
    
    }
}
