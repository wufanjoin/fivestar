using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static  class FiveStarPlayerDetectionCanOperationSystem
    {

        //检测能不能进行操作 
        public static bool IsCanOperate(this FiveStarPlayer fiveStarPlayer, int playCard = 0,int playCardIndex=0)
        {
            if (fiveStarPlayer.IsRestIn)//如果是在休息中 直接不能操作
            {
                return false;
            }
            fiveStarPlayer.canOperateLists.Clear();//可操作列表清空
            fiveStarPlayer.canGangCards.Clear();//可杠列表清空
            if (fiveStarPlayer.IsCanHu(playCard, playCardIndex))//检测能不能胡牌
            {
                fiveStarPlayer.canOperateLists.Add(FiveStarOperateType.FangChongHu);
            }
            if (playCard > 0)
            {
                //别人打牌的时候
                fiveStarPlayer.intData = fiveStarPlayer.IsCanPengAndGang(playCard);
                if (fiveStarPlayer.intData != 0)//检测能不能碰和暗杆
                {
                    if (fiveStarPlayer.IsLiangDao&& fiveStarPlayer.intData == FiveStarOperateType.MingGang
                        && fiveStarPlayer.LiangDaoNoneCards.Contains(playCard))
                    {
                        fiveStarPlayer.AddCanGangOpearte();
                    }
                    else if (fiveStarPlayer.intData == FiveStarOperateType.Peng)
                    {
                        fiveStarPlayer.canOperateLists.Add(FiveStarOperateType.Peng);
                    }
                    else if (fiveStarPlayer.intData == FiveStarOperateType.MingGang)
                    {
                        fiveStarPlayer.canOperateLists.Add(FiveStarOperateType.Peng);
                        fiveStarPlayer.AddCanGangOpearte();
                    }
                }
            }
            else
            {
                //自己摸牌的时候
                if (fiveStarPlayer.IsCanCaGang() || fiveStarPlayer.IsCanAnGang())
                {
                    fiveStarPlayer.AddCanGangOpearte();
                }
            }
            //广播可操作消息
            return fiveStarPlayer.canOperateLists.Count > 0;
        }
        //添加可以杠的操作
        public static void AddCanGangOpearte(this FiveStarPlayer fiveStarPlayer)
        {
            if (fiveStarPlayer.FiveStarRoom.ResidueCards.Count > 0)//摸的最后一张牌不能杠所以 不显示
            {
                fiveStarPlayer.canOperateLists.Add(FiveStarOperateType.MingGang);
            }
        }
        //添加可以杠的牌
        public static void AddCanGangCard(this FiveStarPlayer fiveStarPlayer, int cardSize,int gangType)
        {
            fiveStarPlayer.canGangCards[cardSize] = gangType;
        }

        //检测能不能暗杆
        public static bool IsCanAnGang(this FiveStarPlayer fiveStarPlayer)
        {
            fiveStarPlayer.intData = 0;
            for (int i = 0; i < fiveStarPlayer.Hands.Count - 1; i++)
            {
                if (fiveStarPlayer.Hands[i] == fiveStarPlayer.Hands[i + 1])
                {
                    fiveStarPlayer.intData++;
                }
                else
                {
                    if (fiveStarPlayer.intData >= 3)//3次相同 就表示有4张一样的
                    {
                        fiveStarPlayer.AddCanAnGangCard(fiveStarPlayer.Hands[i]);
                    }
                    fiveStarPlayer.intData = 0;
                }
            }
            if (fiveStarPlayer.intData >= 3)//3次相同 就表示有4张一样的
            {
                fiveStarPlayer.AddCanAnGangCard(fiveStarPlayer.Hands[fiveStarPlayer.Hands.Count - 1]);
            }
            return fiveStarPlayer.canGangCards.Count>0;
        }
        //添加可以暗杠的牌
        public static void AddCanAnGangCard(this FiveStarPlayer fiveStarPlayer,int card)
        {
            if (fiveStarPlayer.IsLiangDao)
            {
                if (!fiveStarPlayer.LiangDaoNoneCards.Contains(card) || fiveStarPlayer.MoEndHand != card)
                {
                    return;//如果玩家 亮倒了  而且 亮倒无关牌中 没有这张牌 他就不能暗杠这张牌 而且 只能暗杠杠摸的那种牌
                }
            }
            fiveStarPlayer.AddCanGangCard(card, FiveStarOperateType.AnGang);
        }
        //检测能不能碰或者杠
        public static int IsCanPengAndGang(this FiveStarPlayer fiveStarPlayer, int card)
        {
            fiveStarPlayer.intData = 0;
            for (int i = 0; i < fiveStarPlayer.Hands.Count; i++)
            {
                if (fiveStarPlayer.Hands[i] == card)
                {
                    fiveStarPlayer.intData++;
                }
            }
            if (fiveStarPlayer.intData == 2)
            {
                if (fiveStarPlayer.IsLiangDao)
                {
                    return FiveStarOperateType.None;
                }
                return FiveStarOperateType.Peng;
            }
            else if (fiveStarPlayer.intData == 3)
            {
                if (fiveStarPlayer.IsLiangDao&& !fiveStarPlayer.LiangDaoNoneCards.Contains(card))
                {
                    return FiveStarOperateType.None;
                }
                fiveStarPlayer.AddCanGangCard(card, FiveStarOperateType.MingGang);
                return FiveStarOperateType.MingGang;
            }
            return FiveStarOperateType.None;
        }

        //检测能不能擦杠
        public static bool IsCanCaGang(this FiveStarPlayer fiveStarPlayer)
        {
            for (int i = 0; i < fiveStarPlayer.OperateInfos.Count; i++)
            {
                if (fiveStarPlayer.OperateInfos[i].OperateType == FiveStarOperateType.Peng)
                {
                    if (fiveStarPlayer.Hands.Contains(fiveStarPlayer.OperateInfos[i].Card))
                    {
                        //如果亮倒了 擦杠的牌 只能是刚才自己摸的牌
                        if (fiveStarPlayer.IsLiangDao)
                        {
                            if (fiveStarPlayer.MoEndHand == fiveStarPlayer.OperateInfos[i].Card)
                            {
                                fiveStarPlayer.AddCanGangCard(fiveStarPlayer.OperateInfos[i].Card, FiveStarOperateType.CaGang);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        fiveStarPlayer.AddCanGangCard(fiveStarPlayer.OperateInfos[i].Card,FiveStarOperateType.CaGang);
                        return true;
                    }
                }
            }
            return false;
        }

        //检测能不能胡
        public static bool IsCanHu(this FiveStarPlayer fiveStarPlayer, int card = 0, int playCardIndex = 0)
        {
            if (card > 0)
            {
                fiveStarPlayer.Hands.Add(card);
            }
            fiveStarPlayer.boolData = CardFiveStarHuPaiLogic.IsHuPai(fiveStarPlayer.Hands);
            //不是自摸要判断 是否是平胡
            if (fiveStarPlayer.boolData && card > 0)
            {
                //如果有一家亮倒 可以胡
                if (fiveStarPlayer.IsLiangDao || fiveStarPlayer.FiveStarRoom.FiveStarPlayerDic[playCardIndex].IsLiangDao)
                {

                }
                //如果没有亮倒 并且胡牌的倍数小于 最小放冲胡的倍数就不能胡
                else if (CardFiveStarHuPaiLogic.GetMultiple(fiveStarPlayer.Hands, fiveStarPlayer.PengGangs, card, fiveStarPlayer.FiveStarRoom.IsGangShangCard) < FiveStarRoom.FaangChongHuMinHuCardMultiple)
                {
                    fiveStarPlayer.boolData = false;
                }
            }
            if (card > 0)
            {
                fiveStarPlayer.Hands.Remove(card);
            }
            return fiveStarPlayer.boolData;
        }
        //检测能不能听牌
        public static bool IsTingCard(this FiveStarPlayer fiveStarPlayer)
        {
           return CardFiveStarHuPaiLogic.IsCanTingPai(fiveStarPlayer.Hands);
        }

        //移除指定牌数量 如果不够移除 会还原被移除的牌
        public static bool RemoveCardCount(this FiveStarPlayer fiveStarPlayer, int card, int count)
        {
            fiveStarPlayer.boolData = true;
            for (int i = 0; i < count; i++)
            {
                if (fiveStarPlayer.Hands.Contains(card))
                {
                    fiveStarPlayer.Hands.Remove(card);
                }
                else
                {
                    fiveStarPlayer.boolData = false;
                    for (int j = 0; j < i; j++)
                    {
                        fiveStarPlayer.Hands.Add(card);
                    }
                }
            }
            return fiveStarPlayer.boolData;
        }
    }
}
