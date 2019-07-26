using System.Collections;
using System.Collections.Generic;

namespace ETHotfix
{
    public static partial class CardFiveStarHuPaiLogic
    {
        public const int MaxCardIndex = 21;

        private static readonly Dictionary<int, int> _HuPaiHintPaiInNum = new Dictionary<int, int>();

        //胡牌提示算法
        public static Dictionary<int, List<int>> HuPaiTiShi(IList<int> cards) 
        {
            Dictionary<int, List<int>> hintDic = new Dictionary<int, List<int>>();
            GetPaiInNum(cards, _HuPaiHintPaiInNum);
            foreach (var pai in _HuPaiHintPaiInNum)
            {
                cards.Remove(pai.Key);
                List<int> tingPais = IsTingPai(cards);
                if (tingPais.Count > 0)
                {
                    hintDic[pai.Key] = tingPais;
                }
                cards.Add(pai.Key);
            }
            return hintDic;
        }

        //是否可以听牌
        public static List<int> IsTingPai(IList<int> cards) 
        {
            List<int> tingPais = new List<int>();
            if (cards.Count % KeZiNum != JiangNum - 1)
            {
                return tingPais;
            }
            for (int i = TiaoStartIndex; i < MaxCardIndex; i++)
            {
                cards.Add(i);
                if (IsHuPai(cards))
                {
                    tingPais.Add(i);
                }
                cards.Remove(i);
            }
            return tingPais;
        }

        //是否可以听牌 不需要知道听几张牌
        public static bool IsCanTingPai(IList<int> cards)
        {
            if (cards.Count % KeZiNum != JiangNum - 1)
            {
                return false;
            }
            for (int i = TiaoStartIndex; i < MaxCardIndex; i++)
            {
                cards.Add(i);
                if (IsHuPai(cards))
                {
                    cards.Remove(i);
                    return true;
                }
                cards.Remove(i);
            }
            return false;
        }

    }
}
