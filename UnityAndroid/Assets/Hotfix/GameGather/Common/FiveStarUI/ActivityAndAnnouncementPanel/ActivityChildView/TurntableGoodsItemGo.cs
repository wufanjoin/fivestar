using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class TurntableGoodsItemGoAwakeSystem : AwakeSystem<TurntableGoodsItemGo, GameObject, TurntableGoods>
    {
        public override void Awake(TurntableGoodsItemGo self, GameObject go, TurntableGoods data)
        {
            self.Awake(go, data, UIType.ActivityAndAnnouncementPanel);
        }
    }

    public class TurntableGoodsItemGo : BaseItem<TurntableGoods>
    {
        #region 脚本工具生成的代码
        private Text mBeansJewelNumText;
        private GameObject mJewelIconGo;
        private GameObject mBeansIconGo;
        private Text mRedPacketNumText;
        private GameObject mThanksGo;
        private GameObject mRedPacketIconGo;
        public override void Awake(GameObject go, TurntableGoods data, string uiType)
        {
            base.Awake(go, data, uiType);
            mBeansJewelNumText = rc.Get<GameObject>("BeansJewelNumText").GetComponent<Text>();
            mJewelIconGo = rc.Get<GameObject>("JewelIconGo");
            mBeansIconGo = rc.Get<GameObject>("BeansIconGo");
            mRedPacketNumText = rc.Get<GameObject>("RedPacketNumText").GetComponent<Text>();
            mThanksGo = rc.Get<GameObject>("ThanksGo");
            mRedPacketIconGo = rc.Get<GameObject>("RedPacketIconGo");
            InitPanel();
        }
        #endregion

        public float _ChouZongRotaZ = 0;
        public void InitPanel()
        {
            int sibingIndex=gameObject.transform.parent.GetSiblingIndex();
            _ChouZongRotaZ = sibingIndex *45+22.5f;
            if (mData.GoodsId == GoodsId.Besans)
            {
                mBeansJewelNumText.text = mData.Amount+"豆";
            }
            else if (mData.GoodsId == GoodsId.Jewel)
            {
                mBeansJewelNumText.text = "钻石x" + mData.Amount;
            }
            else if (mData.GoodsId == GoodsId.HongBao)
            {
                mRedPacketNumText.text = "红包" + mData.Amount + "元";
            }
           
            mBeansJewelNumText.gameObject.SetActive(mData.GoodsId == GoodsId.Besans|| mData.GoodsId == GoodsId.Jewel);
            mRedPacketNumText.gameObject.SetActive(mData.GoodsId == GoodsId.HongBao);
            mJewelIconGo.SetActive(mData.GoodsId == GoodsId.Jewel);
            mBeansIconGo.SetActive(mData.GoodsId == GoodsId.Besans);
            mThanksGo.SetActive(mData.GoodsId == GoodsId.None);
            mRedPacketIconGo.SetActive(mData.GoodsId == GoodsId.HongBao);
        }
    }
}
