using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.TopUpRecordPanel)]
    public class TopUpRecordPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mTopUpRecordItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mTopUpRecordItemGo = rc.Get<GameObject>("TopUpRecordItemGo");
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
            L2C_QueryTopUpRecord u2CGetGoodsDealRecord = (L2C_QueryTopUpRecord)await SessionComponent.Instance.AdministratorCall(new C2L_QueryTopUpRecord() { QueryUserId = AdministratorComponent.Ins.ExamineUser.UserId });
            if (!string.IsNullOrEmpty(u2CGetGoodsDealRecord.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetGoodsDealRecord.Message);
                return;
            }
            if (u2CGetGoodsDealRecord.TopUpRecords.Count == 0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("记录为空");
                //return;
            }
            u2CGetGoodsDealRecord.TopUpRecords.Sort(TopUpRecordsSort);
            mTopUpRecordItemGo.transform.parent.CreatorChildAndAddItem<TopUpRecordItem, TopUpRecord>(
                u2CGetGoodsDealRecord.TopUpRecords);//显示交易记录数据
        }

        public int TopUpRecordsSort(TopUpRecord x, TopUpRecord y)
        {
            if (x.Time > y.Time)
            {
                return -1;
            }
            return 1;
        }
    }
}
