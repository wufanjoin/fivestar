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
    public class GetGoodsOneItemAwakeSystem : AwakeSystem<GetGoodsOneItem, GameObject, GetGoodsOne>
    {
        public override void Awake(GetGoodsOneItem self, GameObject go, GetGoodsOne data)
        {
            self.Awake(go, data, UIType.GetGoodsHintPanel);
        }
    }
    public class GetGoodsOneItem:BaseItem<GetGoodsOne>
    {
        private Text mQuantityText;
        private Image mIconImage;
        public override void Awake(GameObject go, GetGoodsOne data, string uiType)
        {
            base.Awake(go, data, uiType);
            mQuantityText = rc.Get<GameObject>("QuantityText").GetComponent<Text>();
            mIconImage = rc.Get<GameObject>("IconImage").GetComponent<Image>();
            InitPanel();
        }

        public  void InitPanel()
        {
            mQuantityText.text = "x"+mData.GetAmount;
            mIconImage.sprite = GetResoure<Sprite>(GoodsInfoTool.GetGoodsIcon(mData.GoodsId));
            mIconImage.SetNativeSize();
        }
    }
}
