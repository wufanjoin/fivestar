using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.AgencyMgrPanel)]
    public class AgencyMgrPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private InputField mMaiJiaIdInputField;
        private InputField mJewelNumInputField;
        private Button mConfirmBtn;
        private Text mJewelNumText;
        private Button mMarketRecordBtn;
        private GameObject mMarketRecordViewGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mMaiJiaIdInputField = rc.Get<GameObject>("MaiJiaIdInputField").GetComponent<InputField>();
            mJewelNumInputField = rc.Get<GameObject>("JewelNumInputField").GetComponent<InputField>();
            mConfirmBtn = rc.Get<GameObject>("ConfirmBtn").GetComponent<Button>();
            mJewelNumText = rc.Get<GameObject>("JewelNumText").GetComponent<Text>();
            mMarketRecordBtn = rc.Get<GameObject>("MarketRecordBtn").GetComponent<Button>();
            mMarketRecordViewGo = rc.Get<GameObject>("MarketRecordViewGo");
            InitPanel();
        }
        #endregion

        private MarketRecordView _MarketRecordView;
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            mConfirmBtn.Add(ConfirmBtnEvent);
            mMarketRecordBtn.Add(MarketRecordBtnEvent);
            _MarketRecordView=mMarketRecordViewGo.AddItem<MarketRecordView>();
            EventMsgMgr.RegisterEvent(CommEventID.SelfUserInfoRefresh, SetJewelNum);
        }

        public void MarketRecordBtnEvent()
        {
            _MarketRecordView.ShowRecord();
        }
        public void SetJewelNum(params object[] objs)
        {
            mJewelNumText.text = UserComponent.Ins.pSelfUser.Jewel.ToString();
        }
        public  void ConfirmBtnEvent()
        {
            try
            {
                maiJiaUserId=long.Parse(mMaiJiaIdInputField.text);
                maiChuJewelNum = int.Parse(mJewelNumInputField.text);
                if (maiChuJewelNum > UserComponent.Ins.pSelfUser.Jewel)
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("卖出钻石数量不能大于自身的钻石数量");
                    return;
                }
                UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow($"是否给ID:{maiJiaUserId}的玩家销售{maiChuJewelNum}钻石", MarketJewelAction);
                
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                throw;
            }
        }
        long maiJiaUserId = 0;
        int maiChuJewelNum = 0;
        private async void MarketJewelAction(bool isConfirm)
        {
            if (isConfirm)
            {
                L2C_SaleJewel l2CSaleJewel =
                    (L2C_SaleJewel)await SessionComponent.Instance.Call(
                        new C2L_SaleJewel() { MaiJiaUser = maiJiaUserId, JewelNum = maiChuJewelNum });
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(l2CSaleJewel.Message);
            }
        }
        public override async void OnShow()
        {
            base.OnShow();
            SetJewelNum();
        }
    }
}
