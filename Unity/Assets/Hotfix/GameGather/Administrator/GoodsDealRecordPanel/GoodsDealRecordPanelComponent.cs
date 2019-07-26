using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.GoodsDealRecordPanel)]
    public class GoodsDealRecordPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mDealRecordItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mDealRecordItemGo = rc.Get<GameObject>("DealRecordItemGo");
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
            U2C_GetGoodsDealRecord u2CGetGoodsDealRecord=(U2C_GetGoodsDealRecord) await SessionComponent.Instance.AdministratorCall(new C2U_GetGoodsDealRecord() { DealRecordUserId = AdministratorComponent.Ins.ExamineUser.UserId }) ;
            if (!string.IsNullOrEmpty(u2CGetGoodsDealRecord.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetGoodsDealRecord.Message);
                return;
            }
            if (u2CGetGoodsDealRecord.GoodsDealRecords.Count == 0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("记录为空");
               // return;
            }
            u2CGetGoodsDealRecord.GoodsDealRecords.Sort(GoodsDealRecordSort);
            mDealRecordItemGo.transform.parent.CreatorChildAndAddItem<DealRecordItem, GoodsDealRecord>(
                u2CGetGoodsDealRecord.GoodsDealRecords);//显示交易记录数据
        }

        public int GoodsDealRecordSort(GoodsDealRecord x, GoodsDealRecord y)
        {
            if (x.Time > y.Time)
            {
                return -1;
            }
            return 1;
        }
    }
}
