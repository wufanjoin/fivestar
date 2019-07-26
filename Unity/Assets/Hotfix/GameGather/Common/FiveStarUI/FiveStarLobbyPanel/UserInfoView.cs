using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class UserInfoView : BaseView
    {
        #region 脚本工具生成的代码
        private Image mHeadImage;
        private Button mJwelAddBtn;
        private Text mJwelNumText;
        private Text mNameText;
        private Text mIdText;
        private Button mHeadMaskBtn;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mJwelAddBtn = rc.Get<GameObject>("JwelAddBtn").GetComponent<Button>();
            mJwelNumText = rc.Get<GameObject>("JwelNumText").GetComponent<Text>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            mHeadMaskBtn = rc.Get<GameObject>("HeadMaskBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public async void InitPanel()
        {
            mJwelAddBtn.Add(JwelAddBtnEvent);
            RefreshUserInfo();
            EventMsgMgr.RegisterEvent(CommEventID.SelfUserInfoRefresh, RefreshUserInfo);
            mHeadMaskBtn.Add(HeadMaskBtnEvent);
        }

        public void HeadMaskBtnEvent()
        {
            UIComponent.GetUiView<UserInfoPanelComponent>().Show();
        }
        public async void RefreshUserInfo(params object[] objs)
        {
            User _user = UserComponent.Ins.pSelfUser;
            mJwelNumText.text = _user.Jewel.ToString();
            mHeadImage.sprite = await _user.GetHeadSprite();
            mNameText.text = _user.Name;
            mIdText.text = "ID:" + _user.UserId.ToString();
        }
        public void JwelAddBtnEvent()
        {
            UIComponent.GetUiView<ShopPanelComponent>().ShowGoodsList(GoodsId.Jewel,UIType.FiveStarLobbyPanel);
        }
    }
}
