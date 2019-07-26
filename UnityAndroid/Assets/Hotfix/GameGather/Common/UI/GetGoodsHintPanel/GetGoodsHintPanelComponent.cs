using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.GetGoodsHintPanel)]
    public class GetGoodsHintPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mAffirmBtn;
        private GameObject mGoodsItemGo;
        public override float MaskLucencyValue
        {
            get
            {
                return 0.8f;
            }
        }

        public override void GameInit()
        {
            base.GameInit();
            EventMsgMgr.RegisterEvent(CommEventID.GetGodds, GetGoddsEvnet);
        }
        public void GetGoddsEvnet(params object[] objs)
        {
            ShowGetGoods(objs[0] as List<GetGoodsOne>);
        }
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mAffirmBtn = rc.Get<GameObject>("AffirmBtn").GetComponent<Button>();
            mGoodsItemGo = rc.Get<GameObject>("GoodsItemGo");
            InitPanel();
        }
        #endregion

      
        public void InitPanel()
        {
            mAffirmBtn.Add(Hide);
        }
        public void ShowGetGoods(long goodId,int amount)
        {
            GetGoodsOne getGoodsOne = new GetGoodsOne();
            getGoodsOne.GoodsId = goodId;
            getGoodsOne.GetAmount = amount;
            List<GetGoodsOne> getGoodsOnes=new List<GetGoodsOne>(){ getGoodsOne };
            ShowGetGoods(getGoodsOnes);
        }
        public void ShowGetGoods(List<GetGoodsOne> getGoods)
        {
            Show();
            mGoodsItemGo.CopyThisNum(getGoods.Count);
            Transform goodsItemParent=mGoodsItemGo.transform.parent;
            for (int i = 0; i < goodsItemParent.childCount; i++)
            {
                goodsItemParent.GetChild(i).AddItemIfHaveInit<GetGoodsOneItem, GetGoodsOne>(getGoods[i]);
            }
        }
    }
}
