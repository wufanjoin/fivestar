using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class CommodityItemAwakeSystem : AwakeSystem<CommodityItem, GameObject, Commodity>
    {
        public override void Awake(CommodityItem self, GameObject go, Commodity data)
        {
            self.Awake(go, data, UIType.ShopPanel);
        }
    }
    public  class CommodityItem : BaseItem<Commodity>
    {
        private Text mPriceText;
        private Text mCommodityQuantityText;
        private Image mCommodityIconImage;
        private Button mBuyBtn;

        private GameObject mCNYIconGo;
        private GameObject mJewelIconGo;
        public override void Awake(GameObject go, Commodity data, string uiType)
        {
            base.Awake(go, data, uiType);
            mPriceText = rc.Get<GameObject>("PriceText").GetComponent<Text>();
            mCommodityQuantityText = rc.Get<GameObject>("CommodityQuantityText").GetComponent<Text>();
            mCommodityIconImage = rc.Get<GameObject>("CommodityIconImage").GetComponent<Image>();
            mBuyBtn = rc.Get<GameObject>("BuyBtn").GetComponent<Button>();
            mCNYIconGo = rc.Get<GameObject>("CNYIconGo");
            mJewelIconGo = rc.Get<GameObject>("JewelIconGo");
            InitPanel();
        }

        public  void InitPanel()
        {
            mPriceText.text = mData.Price.ToString();
            mCommodityQuantityText.text = mData.Amount.ToString()+ mData.ChineseType;
            mCommodityIconImage.sprite = mData.GetIconSprite();
            mCommodityIconImage.SetNativeSize();
            mCNYIconGo.SetActive(mData.MonetaryType==GoodsId.CNY);
            mJewelIconGo.SetActive(mData.MonetaryType == GoodsId.Jewel);
            mBuyBtn.Add(ShowBuyPop);
        }

        public void ShowBuyPop()
        {
            UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow($"是否花费{mData.Price}{mData.MoneyTypeStr}购买{mData.Amount}{mData.ChineseType}", IsSponsorBuy);
        }

        public async void IsSponsorBuy(bool bol)
        {
            if (bol)
            {
                
                if (mData.MonetaryType == GoodsId.Jewel)
                {
                    if (UserComponent.Ins.pSelfUser.Jewel < mData.Price)
                    {
                        UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("钻石不足");
                        return;
                    }
                }
                L2C_BuyCommodity l2CBuyCommodity=(L2C_BuyCommodity)await SessionComponent.Instance.Call(new C2L_BuyCommodity() {CommodityId = mData.CommodityId});
                if (!string.IsNullOrEmpty(l2CBuyCommodity.PrepayId))
                {
                    SdkMgr.Ins.WeChatpay(l2CBuyCommodity.PrepayId, l2CBuyCommodity.NonceStr);
                }
            }
            else
            {
                
            }
        }
    }
}
