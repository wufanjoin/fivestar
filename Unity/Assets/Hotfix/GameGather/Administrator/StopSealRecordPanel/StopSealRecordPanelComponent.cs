using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.StopSealRecordPanel)]
    public class StopSealRecordPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mStopSealRecordItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mStopSealRecordItemGo = rc.Get<GameObject>("StopSealRecordItemGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
        }
        public override async void OnShow()
        {
            base.OnShow();
            U2C_QueryStopSealRecord u2CGetGoodsDealRecord = (U2C_QueryStopSealRecord)await SessionComponent.Instance.AdministratorCall(new C2U_QueryStopSealRecord() { QueryUserId = AdministratorComponent.Ins.ExamineUser.UserId });
            if (!string.IsNullOrEmpty(u2CGetGoodsDealRecord.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetGoodsDealRecord.Message);
                return;
            }
            if (u2CGetGoodsDealRecord.StopSealRecords.Count == 0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("记录为空");
               // return;
            }
            u2CGetGoodsDealRecord.StopSealRecords.Sort(StopSealRecordSort);
            mStopSealRecordItemGo.transform.parent.CreatorChildAndAddItem<StopSealRecordItem, StopSealRecord>(
                u2CGetGoodsDealRecord.StopSealRecords);//显示交易记录数据
        }

        public int StopSealRecordSort(StopSealRecord x, StopSealRecord y)
        {
            if (x.Time > y.Time)
            {
                return -1;
            }
            return 1;
        }
    }
}
