using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class WinPrizeRecordItemAwakeSystem : AwakeSystem<WinPrizeRecordItem, GameObject, WinPrizeRecord>
    {
        public override void Awake(WinPrizeRecordItem self, GameObject go, WinPrizeRecord data)
        {
            self.Awake(go, data, UIType.ActivityAndAnnouncementPanel);
        }
    }

    public class WinPrizeRecordItem : BaseItem<WinPrizeRecord>
    {
        #region 脚本工具生成的代码
        private Text mTimeText;
        private Text mNameText;
        private Text mStateText;
        public override void Awake(GameObject go, WinPrizeRecord data, string uiType)
        {
            base.Awake(go, data, uiType);
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mStateText = rc.Get<GameObject>("StateText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mTimeText.text = TimeTool.ConvertLongToTimeDesc(mData.Time);

            mNameText.text = GoodsInfoTool.GetGoodsName(mData.GoodsId)+ "x"+mData.Amount;
            if (mData.Type == 0)
            {
                mStateText.text = "未兑奖";
            }
            else
            {
                mStateText.text = "已兑奖";
            }
        }
    }
}
