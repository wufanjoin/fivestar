using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.PlayerInfoPanel)]
    public class PlayerInfoPanelComponent:PopUpUIView
    {
        #region 脚本工具生成的代码
        private Image mHeadImage;
        private Text mBeansQuantityText;
        private Text mJewelQuantityText;
        private Text mNameText;
        private Text mIDText;
        private Button mCloseBtn;
        private GameObject mMagicExpressionGroupGo;
        private GameObject mMagicExprssionItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mHeadImage=rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mBeansQuantityText=rc.Get<GameObject>("BeansQuantityText").GetComponent<Text>();
            mJewelQuantityText=rc.Get<GameObject>("JewelQuantityText").GetComponent<Text>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIDText = rc.Get<GameObject>("IDText").GetComponent<Text>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mMagicExpressionGroupGo =rc.Get<GameObject>("MagicExpressionGroupGo");
            mMagicExprssionItemGo=rc.Get<GameObject>("MagicExprssionItemGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
        }

        public async void ShowUserInfo(User user)
        {
            Show();
            mHeadImage.sprite = await user.GetHeadSprite();
            mBeansQuantityText.text = user.Beans.ToString();
            mJewelQuantityText.text = user.Jewel.ToString();
            mNameText.text = user.Name;
            mIDText.text = "ID:" + user.UserId;
           
            mMagicExpressionGroupGo.SetActive(!(Game.Scene.GetComponent<UserComponent>().pSelfUser == user));
        }
    }
}
