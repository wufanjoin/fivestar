using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.FiveStarMingPaiHintPanel)]
    public class FiveStarMingPaiHintPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private GameObject mDownMingPaiUIGo;
        private GameObject mRightMingPaiUIGo;
        private GameObject mUpMingPaiUIGo;
        private GameObject mLeftMingPaiUIGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mDownMingPaiUIGo = rc.Get<GameObject>("DownMingPaiUIGo");
            mUpMingPaiUIGo = rc.Get<GameObject>("UpMingPaiUIGo");
            mRightMingPaiUIGo = rc.Get<GameObject>("RightMingPaiUIGo");
            mLeftMingPaiUIGo = rc.Get<GameObject>("LeftMingPaiUIGo");
            InitPanel();
        }
        #endregion

        public Dictionary<int, PlayerHuCardHintUI> _PlayerHuCardHintDic = new Dictionary<int, PlayerHuCardHintUI>();
        public void InitPanel()
        {
            _PlayerHuCardHintDic.Add(0, mDownMingPaiUIGo.AddItem<PlayerHuCardHintUI>());
            _PlayerHuCardHintDic.Add(1, mRightMingPaiUIGo.AddItem<PlayerHuCardHintUI>());
            _PlayerHuCardHintDic.Add(2, mUpMingPaiUIGo.AddItem<PlayerHuCardHintUI>());
            _PlayerHuCardHintDic.Add(3, mLeftMingPaiUIGo.AddItem<PlayerHuCardHintUI>());
            for (int i = 0; i < _PlayerHuCardHintDic.Count; i++)
            {
                _PlayerHuCardHintDic[i].InitPanel(i);
            }
        }

        public int _SelectChuCard = -1;//出牌提示的时候 选择出的哪张牌
        //出牌时候的胡牌提示
        public void ShowSelfChuCardHint(List<int> cards,int chuCard)
        {
            _SelectChuCard = chuCard;
            Show();
            gameObject.transform.SetAsLastSibling();
            _PlayerHuCardHintDic[0].ShowChuCardHint(cards);
           
        }

        //听牌时候的胡牌提示
        public void ShowSelfTingHuCardHint(List<int> cards)
        {
            _SelectChuCard = -1;
            Show();
            _PlayerHuCardHintDic[0].ShowTingHuCardHint(cards);
        }

        //明牌时候胡牌提示
        public void ShowMingCardHint(int clientSeatIndex, List<int> cards)
        {
            Show();
            _PlayerHuCardHintDic[clientSeatIndex].ShowMingCardHuHint(cards);
        }
        //录像显示明牌提示
        public void Video_ShowMIngCardHint(int clientSeatIndex, List<int> cards)
        {
            Show();
            _PlayerHuCardHintDic[clientSeatIndex].Video_ShowMingCardHuHint(cards);
        }
        //隐藏胡牌提示
        public void HideHuCardHint(int clientSeatIndex)
        {
            Show();
            _PlayerHuCardHintDic[clientSeatIndex].Hide();
        }

        public override void OnHide()
        {
            base.OnHide();
            HideAllHuCardHint();
        }

        //隐藏所有胡牌提示
        public void HideAllHuCardHint()
        {
            foreach (var playerHuHint in _PlayerHuCardHintDic)
            {
                playerHuHint.Value.Hide();
            }
        }
    }
}
