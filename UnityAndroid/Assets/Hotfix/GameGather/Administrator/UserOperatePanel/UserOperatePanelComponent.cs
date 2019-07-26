using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.UserOperatePanel)]
    public class UserOperatePanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private Text mUserIdText;
        private Text mNameText;
        private Text mLoginTimeText;
        private Text mStopSealText;
        private Text mOnlineStateText;
        private Text mJewelText;
        private Text mBeansText;
        private Text mAgecyLvText;
        private Button mGoodsDealRecordBtn;
        private Button mWinPrizeRecordBtn;
        private Button mStopSealRecordBtn;
        private Button mTopUpRecordBtn;
        private Button mAgencyOperateBtn;
        private Button mAddGoodsBtn;
        private Button mStopSealBtn;
        private Button mRelieveStopSealBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mUserIdText = rc.Get<GameObject>("UserIdText").GetComponent<Text>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mLoginTimeText = rc.Get<GameObject>("LoginTimeText").GetComponent<Text>();
            mStopSealText = rc.Get<GameObject>("StopSealText").GetComponent<Text>();
            mOnlineStateText = rc.Get<GameObject>("OnlineStateText").GetComponent<Text>();
            mJewelText = rc.Get<GameObject>("JewelText").GetComponent<Text>();
            mBeansText = rc.Get<GameObject>("BeansText").GetComponent<Text>();
            mAgecyLvText = rc.Get<GameObject>("AgecyLvText").GetComponent<Text>();
            mGoodsDealRecordBtn = rc.Get<GameObject>("GoodsDealRecordBtn").GetComponent<Button>();
            mWinPrizeRecordBtn = rc.Get<GameObject>("WinPrizeRecordBtn").GetComponent<Button>();
            mStopSealRecordBtn = rc.Get<GameObject>("StopSealRecordBtn").GetComponent<Button>();
            mTopUpRecordBtn = rc.Get<GameObject>("TopUpRecordBtn").GetComponent<Button>();
            mAgencyOperateBtn = rc.Get<GameObject>("AgencyOperateBtn").GetComponent<Button>();
            mAddGoodsBtn = rc.Get<GameObject>("AddGoodsBtn").GetComponent<Button>();
            mStopSealBtn = rc.Get<GameObject>("StopSealBtn").GetComponent<Button>();
            mRelieveStopSealBtn = rc.Get<GameObject>("RelieveStopSealBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(CloseBtnEvent);
            mGoodsDealRecordBtn.Add(UIComponent.GetUiView<GoodsDealRecordPanelComponent>().Show);
            mWinPrizeRecordBtn.Add(UIComponent.GetUiView<WinPrizeRecordPanelComponent>().Show);
            mStopSealRecordBtn.Add(UIComponent.GetUiView<StopSealRecordPanelComponent>().Show);
            mTopUpRecordBtn.Add(UIComponent.GetUiView<TopUpRecordPanelComponent>().Show);

            mAgencyOperateBtn.Add(mAgencyOperateBtnEvent);
            mAddGoodsBtn.Add(AddGoodsBtnEvent);
            mStopSealBtn.Add(StopSealBtnEvent);
            mRelieveStopSealBtn.Add(RelieveStopSealBtnEvent);

        }

        //设置代理
        public void mAgencyOperateBtnEvent()
        {
            UIComponent.GetUiView<AlterInputTextPanelComponent>()
                .ShowAlterPanel(AgencyOperateAction, ShowAlterType.Single, "设置代理等级", "等级:");
        }
        //设置代理回调
        public async void AgencyOperateAction(string lv,string emply)
        {
            int agencyLv = 0;
            try
            {
                agencyLv = int.Parse(lv);
            }
            catch (Exception e)
            {
                Log.Error("等级格式输入错误");
                return;
            }
            L2C_SetAgencyLv l2CSetAgency =
                (L2C_SetAgencyLv) await SessionComponent.Instance.AdministratorCall(new C2L_SetAgencyLv()
                {
                    AgencyUserId = AdministratorComponent.Ins.ExamineUser.UserId,
                    AgencyLv = agencyLv
                });
            if (!string.IsNullOrEmpty(l2CSetAgency.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(l2CSetAgency.Message);
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("操作成功");
            }
        }

        //增加物品按钮事件
        public void AddGoodsBtnEvent()
        {
            UIComponent.GetUiView<AlterInputTextPanelComponent>()
                .ShowAlterPanel(AddGoodsAction, ShowAlterType.Double, "增加物品", "钻石:", "豆子:");
        }

        //增加物品回调
        public async void AddGoodsAction(string jewel,string beans)
        {
            long goodsId = 0;
            int amount = 0;
            try
            {
                if (!string.IsNullOrEmpty(jewel))
                {
                    goodsId = GoodsId.Jewel;
                    amount=Int32.Parse(jewel);
                }
                else if (!string.IsNullOrEmpty(beans))
                {
                    goodsId = GoodsId.Besans;
                    amount = Int32.Parse(beans);
                }
            }
            catch (Exception e)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("输入格式出错");
                return;
            }
            if (goodsId == 0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("输入格式出错");
                return;
            }
            U2C_ChangeUserGoods u2CChangeUserGoods =
                (U2C_ChangeUserGoods) await SessionComponent.Instance.AdministratorCall(
                    new C2U_ChangeUserGoods() {ChangeUserUserId = AdministratorComponent.Ins.ExamineUser.UserId, GoodsId = goodsId, Amount = amount});
            if (!string.IsNullOrEmpty(u2CChangeUserGoods.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CChangeUserGoods.Message);
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("操作成功");
            }
        }
         //封号按钮事件
        public void StopSealBtnEvent()
        {
  
            UIComponent.GetUiView<AlterInputTextPanelComponent>().ShowAlterPanel(StopSealAction, ShowAlterType.Single, "封号", "备注:");
        }
        //封号回调
        public  void StopSealAction(string remark,string empty)
        {
            CallStopSealAndReliveMessage(true, remark);
        }

        //解封按钮事件
        public void RelieveStopSealBtnEvent()
        {
            UIComponent.GetUiView<AlterInputTextPanelComponent>().ShowAlterPanel(RelieveStopSealAction, ShowAlterType.Single, "解封", "备注:");
        }
        //解封回调
        public void RelieveStopSealAction(string remark, string empty)
        {
            CallStopSealAndReliveMessage(false, remark);
        }
        //发送封号解封消息
        public async void CallStopSealAndReliveMessage(bool isStopSweal, string remark)
        {
            StopSealRecord stopSealRecord = new StopSealRecord();
            stopSealRecord.IsStopSeal = isStopSweal;
            stopSealRecord.Explain = remark;
            stopSealRecord.StopSealUserId = AdministratorComponent.Ins.ExamineUser.UserId;
            C2U_SetIsStopSeal c2USetIsStop = new C2U_SetIsStopSeal();
            c2USetIsStop.StopSeal = stopSealRecord;
            U2C_SetIsStopSeal u2CSetIsStopSeal = (U2C_SetIsStopSeal)await SessionComponent.Instance.AdministratorCall(c2USetIsStop);
            if (!string.IsNullOrEmpty(u2CSetIsStopSeal.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CSetIsStopSeal.Message);
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("操作成功");
            }
        }
  
        //关闭按钮事件
        public void CloseBtnEvent()
        {
            UIComponent.GetUiView<AdministratorHomePanelComponent>().Show();
        }

        public override void OnShow()
        {
            base.OnShow();
            mUserIdText.text = AdministratorComponent.Ins.ExamineUser.UserId.ToString();
            mNameText.text = AdministratorComponent.Ins.ExamineUser.Name;
            mLoginTimeText.text = TimeTool.ConvertLongToTimeDesc(AdministratorComponent.Ins.LastLoginTime);

            if (AdministratorComponent.Ins.IsStopSeal)
            {
                mStopSealText.text = "封号中";
            }
            else
            {
                mStopSealText.text = "正常";
            }

            if (AdministratorComponent.Ins.ExamineUser.IsOnLine)
            {
                mOnlineStateText.text = "在线";
            }
            else
            {
                mOnlineStateText.text = "下线";
            }
            mJewelText.text = AdministratorComponent.Ins.ExamineUser.Jewel.ToString();
            mBeansText.text = AdministratorComponent.Ins.ExamineUser.Beans.ToString();
            mAgecyLvText.text = "代理等级:" + AdministratorComponent.Ins.AgecyLv;
        }
    }
}
