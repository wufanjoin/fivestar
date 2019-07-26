using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class DealRecordItemAwakeSystem : AwakeSystem<DealRecordItem, GameObject, GoodsDealRecord>
    {
        public override void Awake(DealRecordItem self, GameObject go, GoodsDealRecord data)
        {
            self.Awake(go, data, UIType.GoodsDealRecordPanel);
        }
    }
    public class DealRecordItem:BaseItem<GoodsDealRecord>
    {
        #region 脚本工具生成的代码
        private Text mJewelNumText;
        private Text mFinishNumText;
        private Text mTimeText;
        private Text mTypeText;
        public override void Awake(GameObject go, GoodsDealRecord data, string uiType)
        {
            base.Awake(go, data, uiType);
            mJewelNumText = rc.Get<GameObject>("JewelNumText").GetComponent<Text>();
            mFinishNumText = rc.Get<GameObject>("FinishNumText").GetComponent<Text>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mTypeText = rc.Get<GameObject>("TypeText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mJewelNumText.text = mData.Amount.ToString();
            mFinishNumText.text=mData.FinishNowAmount.ToString();
            mTimeText.text = TimeTool.ConvertLongToTimeDesc(mData.Time);
            switch (mData.Type)
            {
                case GoodsChangeType.RoomCard:
                    mTypeText.text = "房卡扣除";
                    break;
                case GoodsChangeType.ShopPurchase:
                    mTypeText.text = "商城购买";
                    break;
                case GoodsChangeType.AgencyDeal:
                    mTypeText.text = "代理交易";
                    break;
                case GoodsChangeType.SignIn:
                    mTypeText.text = "签到获得";
                    break;
                case GoodsChangeType.EverydayShare:
                    mTypeText.text = "每日签到";
                    break;
                case GoodsChangeType.GeneralizeAward:
                    mTypeText.text = "推广奖励";
                    break;
                case GoodsChangeType.DrawLottery:
                    mTypeText.text = "抽奖获得";
                    break;
                case GoodsChangeType.Administrator:
                    mTypeText.text = "后台添加";
                    break;
            }
        }
    }
}
