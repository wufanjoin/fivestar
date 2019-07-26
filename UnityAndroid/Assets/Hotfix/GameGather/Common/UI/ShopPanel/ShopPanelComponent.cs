using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.ShopPanel)]
    public class ShopPanelComponent:NormalUIView
    {
        #region 脚本工具生成的代码
        private Button mBesansBtn;
        private Button mJewelBtn;
        private GameObject mCommodityItemGo;
        private GameObject mBesansGroupGo;
        private GameObject mJewlGroupGo;


        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mBesansBtn=rc.Get<GameObject>("BesansBtn").GetComponent<Button>();
            mJewelBtn=rc.Get<GameObject>("JewelBtn").GetComponent<Button>();
            mCommodityItemGo=rc.Get<GameObject>("CommodityItemGo");
            mBesansGroupGo=rc.Get<GameObject>("BesansGroupGo");
            mJewlGroupGo=rc.Get<GameObject>("JewlGroupGo");
            InitPanel();
        }
        #endregion

        public async void InitPanel()
        {
            this.InitChildView(UIType.LobbyTopPanel);
            mBesansBtn.Add(()=> ShowGoodsList(GoodsId.Besans));
            mJewelBtn.Add(() => ShowGoodsList(GoodsId.Jewel));

            UIComponent.GetUiView<LoadingIconPanelComponent>().Show();
            L2C_GetCommodityList g2CGetCommodity =(L2C_GetCommodityList)await SessionComponent.Instance.Session.Call(new C2L_GetCommodityList());
            UIComponent.GetUiView<LoadingIconPanelComponent>().Hide();
            if (g2CGetCommodity.Error == 0)
            {
                foreach (var beans in g2CGetCommodity.BeansList)
                {
                   GameObject go=GameObject.Instantiate(mCommodityItemGo, mBesansGroupGo.transform);
                   ComponentFactory.Create<CommodityItem,GameObject, Commodity>(go, beans);
                }
                foreach (var jewe in g2CGetCommodity.JewelList)
                {
                    GameObject go = GameObject.Instantiate(mCommodityItemGo, mJewlGroupGo.transform);
                    ComponentFactory.Create<CommodityItem, GameObject, Commodity>(go, jewe);
                }
                mCommodityItemGo.SetActive(false);
            }
        }

        private string _returnUItype = string.Empty;
        //关闭商店时候返回的UItyp
        public void ShowGoodsList(long goodsId,string uiType="")
        {
            if (!string.IsNullOrEmpty(uiType))
            {
                _returnUItype = uiType;
            }
            Show();
            switch (goodsId)
            {
                case GoodsId.Besans:
                    mBesansGroupGo.SetActive(true);
                    mJewlGroupGo.SetActive(false);

                    mJewelBtn.GetComponent<Image>().sprite = GetResoure<Sprite>("ShopBtnBgNoPitch");
                    mBesansBtn.GetComponent<Image>().sprite = GetResoure<Sprite>("ShopBtnBgPitch");
                    break;
                case GoodsId.Jewel:
                    mBesansGroupGo.SetActive(false);
                    mJewlGroupGo.SetActive(true);

                    mBesansBtn.GetComponent<Image>().sprite = GetResoure<Sprite>("ShopBtnBgNoPitch");
                    mJewelBtn.GetComponent<Image>().sprite = GetResoure<Sprite>("ShopBtnBgPitch");
                    break;
            }
        }

        //关闭商店返回上衣层级
        public void CloseShop()
        {
            if (string.IsNullOrEmpty(_returnUItype))
            {
                UIComponent.GetUiView<FiveStarLobbyPanelComponent>().Show();
            }
            else
            {
                UIComponent.GetUiView(_returnUItype).Show();
            }
            _returnUItype = string.Empty;
        }
        
    }
}
