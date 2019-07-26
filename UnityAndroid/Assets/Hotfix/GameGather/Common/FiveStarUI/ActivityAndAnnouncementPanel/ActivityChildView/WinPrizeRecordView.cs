using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class WinPrizeRecordView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mWinPrizeRecordItemGo;
        private GameObject mAwardParentGo;
        private GameObject mNodeRecordGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mWinPrizeRecordItemGo = rc.Get<GameObject>("WinPrizeRecordItemGo");
            mAwardParentGo = rc.Get<GameObject>("AwardParentGo");
            mNodeRecordGo = rc.Get<GameObject>("NodeRecordGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
        }
       // List<WinPrizeRecordItem> _RecordItems=new List<WinPrizeRecordItem>();
        public async  void  ShowRecord()
        {
            Show();
            L2C_GetWinPrizeRecord  l2CGetWinPrizeRecord= (L2C_GetWinPrizeRecord)await  SessionComponent.Instance.Call(new C2L_GetWinPrizeRecord());
            if (l2CGetWinPrizeRecord.Error == 0)
            {
                mNodeRecordGo.SetActive(l2CGetWinPrizeRecord.Records.Count==0);
                mAwardParentGo.SetActive(l2CGetWinPrizeRecord.Records.Count>0);
               Transform recordParent=mWinPrizeRecordItemGo.transform.parent;
                l2CGetWinPrizeRecord.Records.Sort(WinPrizeRecordTimeSort);//中奖记录 时间排序
                recordParent.CreatorChildAndAddItem<WinPrizeRecordItem, WinPrizeRecord>(l2CGetWinPrizeRecord.Records);
            }
        }
        //中奖记录 时间排序
        public int WinPrizeRecordTimeSort(WinPrizeRecord x, WinPrizeRecord y)
        {
            if (x.Time > y.Time)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
