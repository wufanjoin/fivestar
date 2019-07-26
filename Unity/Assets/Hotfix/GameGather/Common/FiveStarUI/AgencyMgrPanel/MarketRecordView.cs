using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class MarketRecordView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mRecordItemGo;
        private GameObject mMarketRecordMaskGo;
        private GameObject mNoneRecordGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mRecordItemGo = rc.Get<GameObject>("RecordItemGo");
            mMarketRecordMaskGo = rc.Get<GameObject>("MarketRecordMaskGo");
            mNoneRecordGo = rc.Get<GameObject>("NoneRecordGo");
            InitPanel();
        }
        #endregion

        public void InitPanel()
        {
            mCloseBtn.Add(Hide);

        }

        public void ShowRecord()
        {
            Show();
            RefreshMarketRecord();
        }
        public async void RefreshMarketRecord()
        {
            L2C_GetMarketRecord l2CGetMarketRecord =
                (L2C_GetMarketRecord)await SessionComponent.Instance.Call(new C2L_GetMarketRecord());
            mMarketRecordMaskGo.SetActive(l2CGetMarketRecord.MarketInfos.Count != 0);
            mNoneRecordGo.SetActive(l2CGetMarketRecord.MarketInfos.Count == 0);
            Transform recordItemParent = mRecordItemGo.transform.parent;
            l2CGetMarketRecord.MarketInfos.Sort(MarketSort);
            recordItemParent.CreatorChildAndAddItem<MarketRecordItem, MarketInfo>(l2CGetMarketRecord.MarketInfos);
        }

        public int MarketSort(MarketInfo x, MarketInfo y)
        {
            if (x.Time > y.Time)
            {
                return -1;
            }
            return 1;
        }

    }
}
