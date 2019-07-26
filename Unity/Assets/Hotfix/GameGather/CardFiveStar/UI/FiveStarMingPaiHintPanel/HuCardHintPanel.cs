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
  public class HuCardHintPanel: InitBaseItem
  {
      public RectTransform _HintGoRectTrm;
      public Transform _HintCardGroupTrm;

      public virtual int _BaseWidth
      {
          get { return 66; }
      }
      public virtual int _StepSizeWidth
      {
          get { return 49; }
      }
      public virtual int _BaseHeight
      {
          get { return 15; }
      }
      public virtual int _StepSizeHeight
        {
          get { return 68; }
      }

      public int _HorizontalCount = 4;
        public override void Init(GameObject go)
        {
           base.Init(go);
            _HintGoRectTrm = gameObject.GetComponent<RectTransform>();
            _HintCardGroupTrm = gameObject.FindChild("HintCardGroup");
        }
        
      private List<CardFiveStarCard> _HintCards=new List<CardFiveStarCard>();
      public virtual void RefreshHintPanel(List<int> cards)
      {
          Show();
          TiaoZhengBgWidthAndHeight(cards);
          ClearCards();
          for (int i = 0; i < cards.Count; i++)
          {
              CardFiveStarCard card=CardFiveStarCardPool.Ins.Create(CardFiveStarCardType.Down_ZhiLi_ZhengMain, cards[i], _HintCardGroupTrm,
                  0.4f);
              _HintCards.Add(card);
            }
      }
    
      public void TiaoZhengBgWidthAndHeight(List<int> cards)
      {
          if (cards.Count < _HorizontalCount)
          {
              _HintGoRectTrm.sizeDelta=new Vector2(_BaseWidth+ _StepSizeWidth* cards.Count, _BaseHeight+ _StepSizeHeight);
          }
          else
          {
             int VerticalCount=cards.Count / (_HorizontalCount+1)+1;
             _HintGoRectTrm.sizeDelta = new Vector2(_BaseWidth + _StepSizeWidth * _HorizontalCount, _BaseHeight + _StepSizeHeight* VerticalCount);
          }
      }
      public void ClearCards()
      {
          foreach (var card in _HintCards)
          {
              card.Destroy();
          }
          _HintCards.Clear();
      }
    }
}
