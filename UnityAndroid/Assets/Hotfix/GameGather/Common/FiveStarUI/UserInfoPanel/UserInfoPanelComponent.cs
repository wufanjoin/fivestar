using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.UserInfoPanel)]
    public class UserInfoPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Image mHeadImage;
        private Text mNameText;
        private Text mIdText;
        private Text mIpText;
        private Button mCloseBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            mIpText = rc.Get<GameObject>("IpText").GetComponent<Text>();
            mCloseBtn= rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public async void InitPanel()
        {
            mCloseBtn.Add(Hide);
            User user=UserComponent.Ins.pSelfUser;
            mHeadImage.sprite =await user.GetHeadSprite();
            mNameText.text = user.Name;
            mIdText.text = "ID:" + user.UserId;
            mIpText.text = "IP:" + user.Ip;
        }
    }
}
