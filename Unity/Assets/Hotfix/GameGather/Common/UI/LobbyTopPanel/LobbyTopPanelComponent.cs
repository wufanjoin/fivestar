using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.LobbyTopPanel)]
    public class LobbyTopPanelComponent:ChildUIView
    {
        #region 脚本工具生成的代码
        private Button mExitBtn;
        private Text mNameText;
        private Text mIdText;
        private Image mHeadImage;
        private Text mBeansQuantityText;
        private Button mBeansGetBtn;
        private Text mJewelQuantityText;
        private Button mJewelGetBtn;
        private Button mShareBtn;
        private Button mSettingBtn;
        private Button mHeadMaskBtn;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mExitBtn=rc.Get<GameObject>("ExitBtn").GetComponent<Button>();
            mNameText=rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIdText=rc.Get<GameObject>("IdText").GetComponent<Text>();
            mHeadImage=rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mBeansQuantityText=rc.Get<GameObject>("BeansQuantityText").GetComponent<Text>();
            mBeansGetBtn=rc.Get<GameObject>("BeansGetBtn").GetComponent<Button>();
            mJewelQuantityText=rc.Get<GameObject>("JewelQuantityText").GetComponent<Text>();
            mJewelGetBtn=rc.Get<GameObject>("JewelGetBtn").GetComponent<Button>();
            mShareBtn=rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            mSettingBtn=rc.Get<GameObject>("SettingBtn").GetComponent<Button>();
            mHeadMaskBtn= rc.Get<GameObject>("HeadMaskBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mExitBtn.Add(ExitBtnEenvt);
            mShareBtn.Add(ShareBtnEenvt);
            mSettingBtn.Add(SettingBtnEenvt);
            mBeansGetBtn.Add(BeansGetBtnEvent);
            mJewelGetBtn.Add(JewelGetBtnEvent);
            mHeadMaskBtn.Add(HeadMaskBtnEvent);
            InitUserInfo(Game.Scene.GetComponent<UserComponent>().pSelfUser);//初始化玩家信息
        }

        public override void GameInit()
        {
            base.GameInit();
            EventMsgMgr.RegisterEvent(CommEventID.SelfUserInfoRefresh, SelfUserInfoRefreshEvent);
        }
        private void SelfUserInfoRefreshEvent(params object[] objs)
        {
           InitUserInfo(Game.Scene.GetComponent<UserComponent>().pSelfUser);
        }
        public async void InitUserInfo(User user)
        {
            if (pViewState == ViewState.Node)
            {
                return;
            }
            mNameText.text = user.Name;
            mHeadImage.sprite =await user.GetHeadSprite();
            mBeansQuantityText.text = user.Beans.ConvertorTenUnit();
            mJewelQuantityText.text = user.Jewel.ConvertorTenUnit();
            mIdText.text ="ID:"+user.UserId.ToString();
        }
        public void HeadMaskBtnEvent()
        {
            UIComponent.GetUiView<PlayerInfoPanelComponent>().ShowUserInfo(Game.Scene.GetComponent<UserComponent>().pSelfUser);
        }
        public void JewelGetBtnEvent()
        {
            UIComponent.GetUiView<ShopPanelComponent>().ShowGoodsList(GoodsId.Jewel);
        }
        public void BeansGetBtnEvent()
        {
            UIComponent.GetUiView<ShopPanelComponent>().ShowGoodsList(GoodsId.Besans);
            
        }
        private void ExitBtnEenvt()
        {
            
            if (pParentUIType == UIType.LobbyPanel)
            {
                UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("是否退出到登陆界面", (bol) =>
                {
                    if (bol)
                    {
                        Game.Scene.GetComponent<ToyGameComponent>().EndGame();
                    }
                });
               
            }
            else if (pParentUIType == UIType.ShopPanel)
            {
                UIComponent.GetUiView<ShopPanelComponent>().CloseShop();
            }
            else if (pParentUIType == UIType.BaseHallPanel)
            {
                Game.Scene.GetComponent<ToyGameComponent>().EndGame();
            }
        }
        private void ShareBtnEenvt()
        {
            mUIComponent.Show(UIType.SharePanel);
        }
        private void SettingBtnEenvt()
        {
            mUIComponent.Show(UIType.SettingPanel);
        }

        public  override async void OnShow()
        {
            base.OnShow();
        }

    
    }
}
