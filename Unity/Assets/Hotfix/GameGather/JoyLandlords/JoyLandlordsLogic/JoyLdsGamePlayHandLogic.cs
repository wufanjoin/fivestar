using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace ETHotfix
{
    public static class JoyLdsGamePlayHandLogic
    {
        //压上一家的牌
        public static int PlayerPlayHandIsRational<T>(int lastPlayCardType, T lastHands, T playHands) where T: IList<int>
        {
            int playCardType = GetCardsType(playHands);
            if (playCardType == PlayCardType.WangZha)
            {
                return PlayCardType.WangZha;
            }
            if (playCardType == PlayCardType.ZhaDan&& lastPlayCardType!= PlayCardType.WangZha&& lastPlayCardType != PlayCardType.ZhaDan)
            {
                return PlayCardType.ZhaDan;
            }
            if (lastPlayCardType != playCardType)
            {
                return PlayCardType.None;
            }
            if (CompareSize(lastPlayCardType, lastHands, playHands))
            {
                return lastPlayCardType;
            }
            return PlayCardType.None;
        }

        //第一手 不用和上一手做比较 直接出牌
        public static int PlayerPlayHandIsRational<T>(T playHands) where T : IList<int>
        {
            return GetCardsType(playHands);
        }
        //比较牌的大小
        public static bool CompareSize<T>(int lastPlayCardType, T lastHandsNum, T playHandsNum) where T : IList<int>
        {
            List<int> lastHands = JoyLandlordsCardTool.CardConvertorSize(lastHandsNum);
            lastHands.Sort();
            List<int> playHands = JoyLandlordsCardTool.CardConvertorSize(playHandsNum);
            playHands.Sort();
            switch (lastPlayCardType)
            {
                case PlayCardType.DanZhang:
                case PlayCardType.DuiZi:
                case PlayCardType.SanZhang:
                case PlayCardType.ZhaDan:
                case PlayCardType.LianDui:
                    return playHands[0] > lastHands[0];
                case PlayCardType.SanDaiYi:
                case PlayCardType.SanDaiEr:
                    int last3MinSzie=GetSameNumMinToSize(lastHands, 3);
                    int play3MinSzie = GetSameNumMinToSize(playHands, 3);
                    return play3MinSzie > last3MinSzie;
                case PlayCardType.SiDaiEr:
                case PlayCardType.SiDaiErDui:
                    int last4MinSzie = GetSameNumMinToSize(lastHands, 4);
                    int play4MinSzie = GetSameNumMinToSize(playHands, 4);
                    return play4MinSzie > last4MinSzie;
                case PlayCardType.FeiJiBuDai:
                case PlayCardType.FeiJiDaiDanZhang:
                case PlayCardType.FeiJiDaiDuiZi:
                    if (lastHands.Count == playHands.Count)
                    {
                        int feiJiLast3MinSzie = GetSameNumMinToSize(lastHands, 3);
                        int feiJiPlay3MinSzie = GetSameNumMinToSize(playHands, 3);
                        return feiJiPlay3MinSzie > feiJiLast3MinSzie;
                    }
                    return false;
                case PlayCardType.ShunZi:
                    if (lastHands.Count == playHands.Count)
                    {
                        return playHands[0] > lastHands[0];
                    }
                    return false;
                case PlayCardType.WangZha:
                    return false;
                default:
                    break;
            }
            return false;
        }

        //得到这个牌里面相同数量 最小的牌 并转换成size
        public static int GetSameNumMinToSize<T>(T cards,int num) where T : IList<int>
        {
            List<int> sameCountList=new List<int>();
            Dictionary<int, int> sameCountDic = GetSameCount(cards,ref sameCountList);
            sameCountList.Clear();
            foreach (var sameCount in sameCountDic)
            {
                if (sameCount.Value == num)
                {
                    sameCountList.Add(sameCount.Key);
                }
            }
            sameCountList.Sort();
            if (sameCountList.Count > 0)
            {
                return JoyLandlordsCardTool.CardConvertorSize(sameCountList[0]);
            }
            else
            {
                return 0;
            }
        }
        //根据选中的牌 得到这个牌的类型
        public static int GetCardsType<T>(T cards) where T : IList<int>
        {
            if (cards.Count == 0)
            {
                return PlayCardType.None;
            }
            //单张
            if (cards.Count == 1)
            {
                return PlayCardType.DanZhang;
            }
            //获取牌大小不考虑花色
            List<int> cardSizes = JoyLandlordsCardTool.CardConvertorSize(cards);
            cardSizes.Sort();
            List<int> sameCount=new List<int>();
            Dictionary<int, int> sameCountDic=GetSameCount(cardSizes, ref sameCount);
            //对子或者王炸
            if (cards.Count == 2)
            {
                if (sameCount.Count==1&& sameCount[0]==2)
                {
                    return PlayCardType.DuiZi;
                }
                if (cardSizes[0] >= 53 && cardSizes[1] >= 53)
                {
                    return PlayCardType.WangZha;
                }
                return PlayCardType.None;
            }
            //三不带
            if (cards.Count == 3)
            {
                if (sameCount.Count == 1 && sameCount[0] == 3)
                {
                    return PlayCardType.SanZhang;
                }
                return PlayCardType.None;
            }
            //三带一或者炸弹
            if (cards.Count == 4)
            {
                if (sameCount.Count>1 && sameCount[1] == 3)
                {
                    return PlayCardType.SanDaiYi;
                }
                if (sameCount.Count ==1 && sameCount[0] == 4)
                {
                    return PlayCardType.ZhaDan;
                }
                return PlayCardType.None;
            }

            //判断是不是顺子
            if (IsShunZi(cardSizes))
            {
                return PlayCardType.ShunZi;
            }
            
            //三带二
            if (cards.Count == 5)
            {
                if (sameCount.Count == 2 && sameCount[0] == 2 && sameCount[1] == 3)
                {
                    return PlayCardType.SanDaiEr;
                }
                return PlayCardType.None;
            }
            //判断是不是飞机
            int cardFeiJiType = IsFeiJi(cardSizes, sameCountDic);
            if (cardFeiJiType != PlayCardType.None)
            {
                return cardFeiJiType;
            }
            //判断是不是连对
            if (IsLianDui(cardSizes, sameCount))
            {
                return PlayCardType.LianDui;
            }
            //四带二
            if (cards.Count == 6)
            {
                if (sameCount[sameCount.Count-1] == 4)
                {
                    return PlayCardType.SiDaiEr;
                }
                return PlayCardType.None;
            }
            //四带二对
            if (cards.Count == 8)
            {
                if (sameCount.Count == 3 && sameCount[2] == 4 && sameCount[0] == 2 && sameCount[1] == 2)
                {
                    return PlayCardType.SiDaiErDui;
                }
                return PlayCardType.None;
            }
            return PlayCardType.None;
        }

        //判断是不是顺子
        public static bool IsShunZi(List<int> cardSizes)
        {
            if (cardSizes[cardSizes.Count - 1] > 14)
            {
                return false;//2以上不能作为顺子
            }
            for (int i = 0; i < cardSizes.Count-1; i++)
            {
                if (cardSizes[i] != cardSizes[i + 1] - 1)
                {
                    return false;
                }
            }
            return true;
        }

        private static Dictionary<int,List<int>>  feiJiCardNumDic=new Dictionary<int, List<int>>()
        {
            {3,new List<int>()},
            {2,new List<int>()},
            {1,new List<int>()},
        };

        //判断是不是飞机
        public static int IsFeiJi(List<int> cardSizes, Dictionary<int, int> sameCountDic)
        {
            foreach (var feiJiCardNum in feiJiCardNumDic)
            {
                feiJiCardNum.Value.Clear();
            }
            foreach (var same in sameCountDic)
            {
                if (feiJiCardNumDic.ContainsKey(same.Value))
                {
                    feiJiCardNumDic[same.Value].Add(same.Key);
                }
            }
            feiJiCardNumDic[3].Sort();
            //先判断容易剔除的 3张的数量小于2就不是 第一和第二张 不是相邻的牌就不是
            if (feiJiCardNumDic[3].Count < 2 || Math.Abs(feiJiCardNumDic[3][0] - feiJiCardNumDic[3][1]) != 1)
            {
                return PlayCardType.None;
            }
            //2以上不能作为飞机
            if (feiJiCardNumDic[3][feiJiCardNumDic[3].Count - 1] > 14)
            {
                return PlayCardType.None;
            }
            //判断相同的3张是否 相连
            for (int i = 0; i < feiJiCardNumDic[3].Count-1; i++)
            {
                if (feiJiCardNumDic[3][i]+1 != feiJiCardNumDic[3][i + 1])
                {
                    return PlayCardType.None;
                }
            }
            //判断是否带对子
            if (feiJiCardNumDic[3].Count*5== cardSizes.Count)
            {
                return PlayCardType.FeiJiDaiDuiZi;
            }
            //判断是否带单张
            if (feiJiCardNumDic[3].Count*4 == cardSizes.Count)
            {
                return PlayCardType.FeiJiDaiDanZhang;
            }
            //判断是否带不带
            if (feiJiCardNumDic[3].Count * 3 == cardSizes.Count)
            {
                return PlayCardType.FeiJiBuDai;
            }
            return PlayCardType.None;
        }

        //判断是不是连队
        public static bool IsLianDui(List<int> cardSizes,List<int> sameCount)
        {
            if (cardSizes.Count % 2 != 0)
            {
                return false;
            }
            //2以上不能作为连队
            if (cardSizes[cardSizes.Count - 1] > 14)
            {
                return false;
            }
            for (int i = 0; i < cardSizes.Count-1; i++)
            {
                if (i % 2 == 0)
                {
                    if (cardSizes[i] != cardSizes[i + 1])
                    {
                        return false;
                    }
                }
                else
                {
                    if (cardSizes[i]+1 != cardSizes[i + 1])
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        //获取相同张数数量
        public static Dictionary<int, int> GetSameCount<T>(T cardSizes,ref List<int> sameCount) where T:IList<int>
        {
            Dictionary<int, int> sameCountDic = new Dictionary<int, int>();
            foreach (var size in cardSizes)
            {
                if (sameCountDic.ContainsKey(size))
                {
                    sameCountDic[size]++;
                }
                else
                {
                    sameCountDic[size] = 1;
                }
            }
            foreach (var same in sameCountDic)
            {
                sameCount?.Add(same.Value);
            }
            sameCount?.Sort();//默认就是从小到大排序
            return sameCountDic;
        }
    }
}
