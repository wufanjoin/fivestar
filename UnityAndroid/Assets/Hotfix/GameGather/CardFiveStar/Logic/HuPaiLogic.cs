using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ETModel;

namespace ETHotfix
{
    public static partial class CardFiveStarHuPaiLogic
    {

        public const int TiaoStartIndex = 1;
        public const int TongStartIndex = 10;
        public const int OtherStartIndex = 19;
        public const int JiangNum = 2;
        public const int KeZiNum = 3;

        private static readonly Dictionary<int, int> _PaiInNum = new Dictionary<int, int>();

        static int multiple = 0;
        //获得买码倍数
        public static int GetMaiMaMultiple(int card)
        {
            if (card < OtherStartIndex)
            {
                multiple=card % 9;
                if (multiple == 0)
                {
                    return 9;
                }
                return multiple;
            }
            return 10;
        }
        //是否可以胡牌
        public static bool IsHuPai(IList<int> cards)
        {
            if (cards.Count % KeZiNum != JiangNum)
            {
                return false;
            }
            GetPaiInNum(cards, _PaiInNum);
            if (!TentativeDetection(_PaiInNum)) return false; //初步检测一次
            foreach (var pai in _PaiInNum)
            {
                if (pai.Value >= JiangNum)
                {
                    if (ZuoJiangIsHuPai(pai.Key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //传一个将 判断用这个做将能否胡牌
        private static bool ZuoJiangIsHuPai(int cardNum)
        {
            Dictionary<int, int> shunziDictionary = new Dictionary<int, int>(_PaiInNum);
            shunziDictionary[cardNum] -= 2;
            return TiChuKeZiAndShunZi(shunziDictionary);
        }

        //从最的牌开始遍历 能剔除刻子就先剔除刻子 不能剔除就尝试剔除顺子
        public static bool TiChuKeZiAndShunZi(Dictionary<int, int> cards)
        {
            List<int> cardsKeys = new List<int>(cards.Keys);
            for (int i = 0; i < cardsKeys.Count; i++)
            {
                if (cards[cardsKeys[i]] > 0)
                {
                    while (cards[cardsKeys[i]] > 0)
                    {
                        if (cards[cardsKeys[i]] >= KeZiNum)
                        {
                            cards[cardsKeys[i]] -= KeZiNum;
                        }
                        else if ((cardsKeys[i] < TongStartIndex - 2 || cardsKeys[i] >= TongStartIndex) &&
                                 cardsKeys[i] < OtherStartIndex - 2
                                 && cards.ContainsKey(cardsKeys[i] + 1) && cards.ContainsKey(cardsKeys[i] + 2) &&
                                 cards[cardsKeys[i] + 1] > 0 && cards[cardsKeys[i] + 2] > 0)
                        {
                            cards[cardsKeys[i]]--;
                            cards[cardsKeys[i] + 1]--;
                            cards[cardsKeys[i] + 2]--;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //获取牌数和对应的张数
        public static void GetPaiInNum(IList<int> cards, Dictionary<int, int> paiInNum) 
        {
            cards.Sort();
            paiInNum.Clear();
            for (int i = 0; i < cards.Count; i++)
            {
                if (paiInNum.ContainsKey(cards[i]))
                {
                    paiInNum[cards[i]]++;
                }
                else
                {
                    paiInNum[cards[i]] = 1;
                }
            }
        }

        //初步判断能否胡牌 能 解决95% 不能胡牌的类型
        private static int tiaoPaiNum;

        private static int tongPaiNum;
        private static int OtherPaiNum;

        public static bool TentativeDetection(Dictionary<int, int> paiInNum)
        {
            tiaoPaiNum = 0;
            tongPaiNum = 0;
            OtherPaiNum = 0;
            foreach (var pai in paiInNum)
            {
                if (pai.Key < TongStartIndex)
                {
                    tiaoPaiNum += pai.Value;
                }
                else if (pai.Key < OtherStartIndex)
                {
                    tongPaiNum += pai.Value;
                }
                else
                {
                    OtherPaiNum += pai.Value;
                }
            }
            tiaoPaiNum %= 3;
            if (!(tiaoPaiNum == 0 || tiaoPaiNum == 2))
            {
                return false;
            }
            tongPaiNum %= 3;
            if (!(tongPaiNum == 0 || tongPaiNum == 2))
            {
                return false;
            }
            OtherPaiNum %= 3;
            if (!(OtherPaiNum == 0 || OtherPaiNum == 2))
            {
                return false;
            }
            return tiaoPaiNum + tongPaiNum + OtherPaiNum == 2;
        }
    }
}