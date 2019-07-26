using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.WinPrizeRecordPanel)]
    public class WinPrizeRecordPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mWinPrizeRecordItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mWinPrizeRecordItemGo = rc.Get<GameObject>("WinPrizeRecordItemGo");
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
            L2C_QueryWinPrizeRecord u2CGetGoodsDealRecord = (L2C_QueryWinPrizeRecord)await SessionComponent.Instance.AdministratorCall(new C2L_QueryWinPrizeRecord() { QueryUserId = AdministratorComponent.Ins.ExamineUser.UserId });
            if (!string.IsNullOrEmpty(u2CGetGoodsDealRecord.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetGoodsDealRecord.Message);
                return;
            }
            if (u2CGetGoodsDealRecord.Records.Count == 0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("记录为空");
               // return;
            }
            u2CGetGoodsDealRecord.Records.Sort(RecordSort);
            mWinPrizeRecordItemGo.transform.parent.CreatorChildAndAddItem<AdministratorWinPrizeRecordItem, WinPrizeRecord>(
                u2CGetGoodsDealRecord.Records);//显示交易记录数据
        }

        public int RecordSort(WinPrizeRecord x, WinPrizeRecord y)
        {
            if (x.Time > y.Time)
            {
                return -1;
            }
            return 1;
        }
    }
}
