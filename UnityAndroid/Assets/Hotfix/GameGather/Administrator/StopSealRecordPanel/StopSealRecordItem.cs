using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class StopSealRecordItemAwakeSystem : AwakeSystem<StopSealRecordItem, GameObject, StopSealRecord>
    {
        public override void Awake(StopSealRecordItem self, GameObject go, StopSealRecord data)
        {
            self.Awake(go, data, UIType.StopSealRecordPanel);
        }
    }
    public class StopSealRecordItem : BaseItem<StopSealRecord>
    {
        #region 脚本工具生成的代码
        private Text mTypeText;
        private Text mTimeText;
        private Text mRemarkText;
        public override void Awake(GameObject go, StopSealRecord data, string uiType)
        {
            base.Awake(go, data, uiType);
            mTypeText = rc.Get<GameObject>("TypeText").GetComponent<Text>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mRemarkText = rc.Get<GameObject>("RemarkText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            if (mData.IsStopSeal)
            {
                mTypeText.text = "封号";
            }
            else
            {
                mTypeText.text = "解封";
            }
            mTimeText.text = TimeTool.ConvertLongToTimeDesc(mData.Time);
            mRemarkText.text = mData.Explain;
        }
    }
}
