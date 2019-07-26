using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class AdministratorWinPrizeRecordItemAwakeSystem : AwakeSystem<AdministratorWinPrizeRecordItem, GameObject, WinPrizeRecord>
    {
        public override void Awake(AdministratorWinPrizeRecordItem self, GameObject go, WinPrizeRecord data)
        {
            self.Awake(go, data, UIType.WinPrizeRecordPanel);
        }
    }
    public class AdministratorWinPrizeRecordItem : BaseItem<WinPrizeRecord>
    {
        #region 脚本工具生成的代码
        private Text mGoodsNameText;
        private Text mGoodsAmountText;
        private Text mTimeText;
        private Text mStateText;
        private Text mRemarText;
        private Button mAlterStateBtn;
        public override void Awake(GameObject go, WinPrizeRecord data, string uiType)
        {
            base.Awake(go, data, uiType);
            mGoodsNameText = rc.Get<GameObject>("GoodsNameText").GetComponent<Text>();
            mGoodsAmountText = rc.Get<GameObject>("GoodsAmountText").GetComponent<Text>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mStateText = rc.Get<GameObject>("StateText").GetComponent<Text>();
            mRemarText = rc.Get<GameObject>("RemarText").GetComponent<Text>();
            mAlterStateBtn = rc.Get<GameObject>("AlterStateBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mAlterStateBtn.Add(AlterStateBtnEvent);
            mGoodsNameText.text = GoodsInfoTool.GetGoodsName(mData.GoodsId);
            mGoodsAmountText.text = mData.Amount.ToString();
            mTimeText.text = TimeTool.ConvertLongToTimeDesc(mData.Time);
            if (mData.Type == 0)
            {
                mStateText.text = "未兑奖";
            }
            else
            {
                mStateText.text = "已兑奖";
            }
            mRemarText.text = mData.Remark;
        }

        public  void AlterStateBtnEvent()
        {

            UIComponent.GetUiView<AlterInputTextPanelComponent>()
                .ShowAlterPanel(AlterStateAction, ShowAlterType.Double, "修改中奖状态", "类型1/0:", "备注:");
          
        }

        public async void AlterStateAction(string type,string remark)
        {
            int intType = 0;
            try
            {
                intType=int.Parse(type);
            }
            catch (Exception e)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("类型输入错误");
                return;
            }
         
            L2C_ChangeWinPrizeRecordState u2CGetGoodsDealRecord = (L2C_ChangeWinPrizeRecordState)await SessionComponent.Instance.AdministratorCall(new C2L_ChangeWinPrizeRecordState()
            {
                WinPrizeId = mData.WinPrizeId,
                Type = intType,
                Remark = remark
            });
            if (!string.IsNullOrEmpty(u2CGetGoodsDealRecord.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(u2CGetGoodsDealRecord.Message);
                return;
            }
            UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("修改成功");
        }
    }
}
