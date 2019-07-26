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
    public class DownHuHintCard : InitBaseItem
    {

        private Transform _CardPointGo;
        public CardFiveStarCard _card;
        private Text _DescText;
        public override void Init(GameObject go)
        {
            base.Init(go);
            _CardPointGo = gameObject.FindChild("CardPointGo");
            _DescText = gameObject.FindChild("DescText").GetComponent<Text>();
        }

        public void SetUI(int cardSize)
        {
            Show();
            if (_card == null)
            {
                _card=CardFiveStarCardPool.Ins.Create(CardFiveStarCardType.Down_ZhiLi_ZhengMain, cardSize, _CardPointGo,
                    0.6f);
                _card.LocalPositionZero();
            }
            else
            {
                _card.SetCardUI(cardSize);
            }
            int selectChuCard = UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>()._SelectChuCard;
            int multipleNum=CardFiveStarHandComponent.Ins.GetCardInMultiple(cardSize, selectChuCard);
            int residueCardNum = CardFiveStarRoom.Ins._AllCardResidueNum[cardSize];
            _DescText.text =
                $"{multipleNum}<color=#494949FF>倍</color>\n{residueCardNum}<color=#494949FF>张</color>";
        }

        public void ShowDescText()
        {
            
        }
    }
    public class DownHuCardHintPanel : HuCardHintPanel
    {
        public override int _BaseWidth
        {
            get { return 91; }
        }
        public override int _StepSizeWidth
        {
            get { return 134; }
        }
        public override int _BaseHeight
        {
            get { return 36; }
        }
        public override int _StepSizeHeight
        {
            get { return 99; }
        }

        private GameObject _SingleHintCard;
      
        public override void Init(GameObject go)
        {
            base.Init(go);
            _SingleHintCard = gameObject.FindChild("HintCardGroup/SingleHintCard").gameObject;
            _SingleHintCard.SetActive(false);
        }

        public List<DownHuHintCard> _DownHuHintCards = new List<DownHuHintCard>();
        public override void RefreshHintPanel(List<int> cards)
        {
            TiaoZhengBgWidthAndHeight(cards);
            for (int i = _DownHuHintCards.Count; i < cards.Count; i++)
            {
               GameObject hintCardGo=GameObject.Instantiate(_SingleHintCard, _SingleHintCard.transform.parent);
               DownHuHintCard downHuHintCard=hintCardGo.AddItem<DownHuHintCard>();
               _DownHuHintCards.Add(downHuHintCard);
            }
            for (int i = 0; i < cards.Count; i++)
            {
                _DownHuHintCards[i].SetUI(cards[i]);
            }
            for (int i = cards.Count; i < _DownHuHintCards.Count; i++)
            {
                _DownHuHintCards[i].Hide();
            }
        }
    }
}
