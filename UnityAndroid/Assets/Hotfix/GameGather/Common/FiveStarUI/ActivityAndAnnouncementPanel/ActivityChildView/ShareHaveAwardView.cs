using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class ShareHaveAwardView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mFriendShareBtn;
        private Text mAwardInfoText;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mFriendShareBtn = rc.Get<GameObject>("FriendShareBtn").GetComponent<Button>();
            mAwardInfoText = rc.Get<GameObject>("AwardInfoText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public async void InitPanel()
        {
            mFriendShareBtn.Add(FriendShareBtnEvent);
            L2C_GetTheFirstShareAward l2CGetTheFirstShareAward =(L2C_GetTheFirstShareAward)await SessionComponent.Instance.Call(new C2L_GetTheFirstShareAward());
            mAwardInfoText.text = $"每日首次分享到朋友圈可获得<color=#FCFF38FF> {l2CGetTheFirstShareAward.JeweleAmount} </color>钻石";
        }

        public async void FriendShareBtnEvent()
        {
            ShareUrlMgr.NormalShare(WxShareSceneType.Circle);//分享到朋友圈
            L2C_GetEverydayShareAward l2CGetTheFirstShareAward = (L2C_GetEverydayShareAward)await SessionComponent.Instance.Call(new C2L_GetEverydayShareAward());
            if (!string.IsNullOrEmpty(l2CGetTheFirstShareAward.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(l2CGetTheFirstShareAward.Message);
            }
        }
    }
}
