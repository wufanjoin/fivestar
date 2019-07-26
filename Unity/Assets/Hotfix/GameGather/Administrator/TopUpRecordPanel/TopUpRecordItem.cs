using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class TopUpRecordItemAwakeSystem : AwakeSystem<TopUpRecordItem, GameObject, TopUpRecord>
    {
        public override void Awake(TopUpRecordItem self, GameObject go, TopUpRecord data)
        {
            self.Awake(go, data, UIType.TopUpRecordPanel);
        }
    }
    public class TopUpRecordItem : BaseItem<TopUpRecord>
    {
        #region 脚本工具生成的代码
        private Text mOrderIdText;
        private Text mMoneyText;
        private Text mGoodsNameText;
        private Text mAmountText;
        private Text mTimeText;
        private Text mRemarkText;
        private Button mRepairOrderBtn;
        public override void Awake(GameObject go, TopUpRecord data, string uiType)
        {
            base.Awake(go, data, uiType);
            mOrderIdText = rc.Get<GameObject>("OrderIdText").GetComponent<Text>();
            mMoneyText = rc.Get<GameObject>("MoneyText").GetComponent<Text>();
            mGoodsNameText = rc.Get<GameObject>("GoodsNameText").GetComponent<Text>();
            mAmountText = rc.Get<GameObject>("AmountText").GetComponent<Text>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mRemarkText = rc.Get<GameObject>("RemarkText").GetComponent<Text>();
            mRepairOrderBtn = rc.Get<GameObject>("RepairOrderBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mOrderIdText.text = mData.OrderId;
            mMoneyText.text = mData.Money.ToString();
            mGoodsNameText.text = GoodsInfoTool.GetGoodsName(mData.GoodsId);
            mAmountText.text = mData.GoodsAmount.ToString();
            mTimeText.text = TimeTool.ConvertLongToTimeDesc(mData.Time);
            switch (mData.TopUpState)
            {
                case TopUpStateType.NoPay:
                    mRemarkText.text = "未支付";
                    break;
                case TopUpStateType.AlreadyPay:
                    mRemarkText.text = "完成交易";
                    break;
                case TopUpStateType.RepairOrder:
                    mRemarkText.text = "补单成功";
                    break;
                default:
                    break;
            }
            
            mRepairOrderBtn.Add(RepairOrderBtnEvent);
        }

        public async void RepairOrderBtnEvent()
        {
            if (TopUpStateType.NoPay != mData.TopUpState)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("交易已完成 无法补单");
                return;
            }
            L2C_TopUpRepairOrder u2CGetGoodsDealRecord = (L2C_TopUpRepairOrder)await SessionComponent.Instance.AdministratorCall(new C2L_TopUpRepairOrder() { OrderId =mData.OrderId });
            if (!string.IsNullOrEmpty(u2CGetGoodsDealRecord.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetGoodsDealRecord.Message);
                return;
            }
            UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("补单成功");
        }
    }
}
