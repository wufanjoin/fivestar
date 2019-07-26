using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.GeneralizePanel)]
    public class GeneralizePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mNoviceGetJewelBtn;
        private GameObject mShareTextItemGo;
        private Text mShareText;
        private Button mNoviceHairpinBtn;
        private Text mInviteNumberText;
        private Text mAccumulativeJewelNumText;
        private Button mCloseBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mNoviceGetJewelBtn = rc.Get<GameObject>("NoviceGetJewelBtn").GetComponent<Button>();
            mShareTextItemGo = rc.Get<GameObject>("ShareTextItemGo");
            mShareText = rc.Get<GameObject>("ShareText").GetComponent<Text>();
            mNoviceHairpinBtn = rc.Get<GameObject>("NoviceHairpinBtn").GetComponent<Button>();
            mInviteNumberText = rc.Get<GameObject>("InviteNumberText").GetComponent<Text>();
            mAccumulativeJewelNumText = rc.Get<GameObject>("AccumulativeJewelNumText").GetComponent<Text>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            mNoviceHairpinBtn.Add(NoviceHairpinBtnEvent);
            InitHairpin();
            InitGenralizeInfo();
            mNoviceGetJewelBtn.Add(NoviceGetJewelBtnEvent);
        }

        public static int GreenGiftStatu = -1;
        public async void NoviceGetJewelBtnEvent()
        {
            if (GreenGiftStatu < 0)
            {
                L2C_GetGreenGiftStatu l2CGetGreenGift = (L2C_GetGreenGiftStatu)await SessionComponent.Instance.Call(new C2L_GetGreenGiftStatu());
                if (l2CGetGreenGift.IsHaveGet)
                {
                    GreenGiftStatu = 1;
                }
                else
                {
                    GreenGiftStatu = 0;
                }
            }
           

            if (GreenGiftStatu>0)
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("您已经领取过该礼包");
            }
            else
            {
                UIComponent.GetUiView<GetInviteCodePanelComponent>().Show();
            }
           
        }
        //初始化发卡面板
        public void InitHairpin()
        {
            Transform shadreTextParentTrm = mShareTextItemGo.transform.parent;
            GameObject  codeShareGo=GameObject.Instantiate(mShareTextItemGo, shadreTextParentTrm);
            mShareTextItemGo.AddItem<CopyTextItem>().InitItem(ShareUrlMgr.HomeUrl);
            long code = UserComponent.Ins.pUserId;
            codeShareGo.AddItem<CopyTextItem>().InitItem("邀请码:"+code, ShareUrlMgr.HomeUrl+"?code="+code.ToString());
        }
        //初始化奖励信息
        public async void InitGenralizeInfo()
        {
            L2C_GetGenralizeInfo l2CGetGenralizeInfo=(L2C_GetGenralizeInfo)await SessionComponent.Instance.Call(new C2L_GetGenralizeInfo());
            if (l2CGetGenralizeInfo.AwardInfo == null)
            {
                l2CGetGenralizeInfo.AwardInfo = new GeneralizeAwardInfo();
            }
            mInviteNumberText.text = $"已成功邀请 <color=#FF0000FF>{l2CGetGenralizeInfo.AwardInfo.GeneralizeNumber}</color> 人";
            mAccumulativeJewelNumText.text = l2CGetGenralizeInfo.AwardInfo.GetJewelTotalNum.ToString();
        }
        //分享发卡链接
        public void NoviceHairpinBtnEvent()
        {
            ShareUrlMgr.NormalShare(WxShareSceneType.Friend);
        }
    }
}
