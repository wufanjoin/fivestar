using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace ETHotfix
{
  public  static partial class CardFiveStarHuPaiLogic
    {
        //获取 亮倒后的牌 可以与胡牌无关的牌
        public static RepeatedField<RepeatedField<int>> GetLiangDaoNoneHuCards(RepeatedField<int> hands)
        {
            RepeatedField<int> cards = new RepeatedField<int>(){ hands };

            int OriginalTingPaiCardCount = IsTingPai(cards).Count;//原有能听牌的张数
            Dictionary<int,int> cardInNumDic=new Dictionary<int, int>();
            RepeatedField<int> noneCards=new RepeatedField<int>();
            GetPaiInNum(cards, cardInNumDic);
            foreach (var cardInNum in cardInNumDic)
            {
                if (cardInNum.Value >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        cards.Remove(cardInNum.Key);
                       
                    }
                    if (IsTingPai(cards).Count == OriginalTingPaiCardCount)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            noneCards.Add(cardInNum.Key);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            cards.Add(cardInNum.Key);
                        }
                    }
                }
            }
            RepeatedField<RepeatedField<int>> originalCardAndNoneCards=new RepeatedField<RepeatedField<int>>();
            originalCardAndNoneCards.Add(cards);
            originalCardAndNoneCards.Add(noneCards);
            return originalCardAndNoneCards;
        }
    }
}
