using ETModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETHotfix
{
    public static class FiveStarPlayerPengGangHuOperationSystem
    {
        //玩家执行操作
        public static void ExecuteOperate(this FiveStarPlayer fiveStarPlayer, FiveStarOperateInfo operateInfo)
        {
            fiveStarPlayer.boolData = false;
            if (operateInfo.OperateType == FiveStarOperateType.MingGang)
            {
                if (!fiveStarPlayer.canGangCards.ContainsKey(operateInfo.Card))
                {
                    Log.Error("玩家要杠的牌 不在可杠列表里面" + operateInfo.Card);
                    return;
                }
                operateInfo.OperateType = fiveStarPlayer.canGangCards[operateInfo.Card];//玩家只发明杠 需要服务器判断是什么杠
            }

            if (fiveStarPlayer.IsLiangDao)//如果玩家亮倒了 可以胡 却选择不胡 强制胡
            {
                if (operateInfo.OperateType == FiveStarOperateType.None && fiveStarPlayer.canOperateLists.Contains(FiveStarOperateType.FangChongHu))
                {
                    operateInfo.OperateType = FiveStarOperateType.FangChongHu;
                }
            }

            switch (operateInfo.OperateType)
            {
                case FiveStarOperateType.None:
                    fiveStarPlayer.boolData = true;
                    break;
                case FiveStarOperateType.Peng:
                    operateInfo.Card = fiveStarPlayer.FiveStarRoom.CurrChuPaiCard;
                    fiveStarPlayer.boolData = fiveStarPlayer.PengOrMingGangOrAnGang(fiveStarPlayer.FiveStarRoom.CurrChuPaiCard, 2, operateInfo.OperateType);
                    break;
                case FiveStarOperateType.MingGang:
                    operateInfo.Card = fiveStarPlayer.FiveStarRoom.CurrChuPaiCard;
                    fiveStarPlayer.boolData = fiveStarPlayer.PengOrMingGangOrAnGang(fiveStarPlayer.FiveStarRoom.CurrChuPaiCard, 3, operateInfo.OperateType);
                    break;
                case FiveStarOperateType.AnGang:
                    fiveStarPlayer.boolData = fiveStarPlayer.PengOrMingGangOrAnGang(operateInfo.Card, 4, operateInfo.OperateType); //暗杠是可以有多个选择 要客户端传
                    break;
                case FiveStarOperateType.CaGang:
                    fiveStarPlayer.boolData = fiveStarPlayer.CaGang(operateInfo.Card); //擦杠是可以有多个选择 要客户端传
                    break;
                case FiveStarOperateType.FangChongHu:
                case FiveStarOperateType.ZiMo:
                    operateInfo.Card = 0;
                    operateInfo.OperateType = FiveStarOperateType.ZiMo;
                    if (fiveStarPlayer.Hands.Count % 3 == 1)
                    {
                        operateInfo.Card = fiveStarPlayer.FiveStarRoom.CurrChuPaiCard;
                        operateInfo.OperateType = FiveStarOperateType.FangChongHu;
                    }
                    fiveStarPlayer.boolData = fiveStarPlayer.HuPai(operateInfo.Card, fiveStarPlayer.FiveStarRoom.CurrChuPaiIndex);
                    break;

            }
            if (!fiveStarPlayer.boolData)
            {
                Log.Error("操作错误 视为放弃操作");
                operateInfo.OperateType = FiveStarOperateType.None;
            }
        }

        //操作完成后续
        public static void OperateFinishFollow(this FiveStarPlayer fiveStarPlayer, FiveStarOperateInfo operateInfo)
        {
            //执行碰 明杠 放冲胡 玩家 最后打牌要被数组移除
            switch (operateInfo.OperateType)
            {
                case FiveStarOperateType.Peng:
                case FiveStarOperateType.MingGang:
                case FiveStarOperateType.FangChongHu:
                    fiveStarPlayer.FiveStarRoom.FiveStarPlayerDic[fiveStarPlayer.FiveStarRoom.CurrChuPaiIndex].PlayCardByEatOff();//玩家打出牌被吃掉
                    break;
            }
            //操作完成后续
            switch (operateInfo.OperateType)
            {
                case FiveStarOperateType.None://玩家不操作
                    if (fiveStarPlayer.FiveStarRoom.QiOperateNextStep == FiveStarOperateType.MoCard)
                    {
                        fiveStarPlayer.FiveStarRoom.PlayerMoPai();//可出牌的人 和当前出牌的是同一个 证明 刚刚摸牌玩家已经出牌了 所以按正常流程摸牌
                    }
                    else if (fiveStarPlayer.FiveStarRoom.QiOperateNextStep == FiveStarOperateType.ChuCard)
                    {
                        fiveStarPlayer.FiveStarRoom.FiveStarPlayerDic[fiveStarPlayer.FiveStarRoom.LastMoPaiSeatIndex].CanChuPai(); //最后摸牌的玩家可以出牌
                    }
                    break;
                case FiveStarOperateType.Peng:
                    fiveStarPlayer.SendNewestHands();//发送玩家最新的手牌信息
                    fiveStarPlayer.CanChuPai();//碰了就可以出牌
                    break;
                case FiveStarOperateType.MingGang:
                case FiveStarOperateType.AnGang:
                case FiveStarOperateType.CaGang:
                    fiveStarPlayer.SendNewestHands();//发送玩家最新的手牌信息
                    fiveStarPlayer.FiveStarRoom.PlayerMoPai(fiveStarPlayer.SeatIndex);//杠的话就摸一张牌
                    break;
            }
        }

        //玩家碰 明杠 暗杠
        public static bool PengOrMingGangOrAnGang(this FiveStarPlayer fiveStarPlayer, int card, int count, int operateType)
        {
            if (fiveStarPlayer.RemoveCardCount(card, count))
            {
                FiveStarOperateInfo fiveStarOperateInfo = FiveStarOperateInfoFactory.Create(card, operateType,
                    fiveStarPlayer.FiveStarRoom.CurrChuPaiIndex);
                fiveStarPlayer.OperateInfos.Add(fiveStarOperateInfo);
                return true;
            }
            return false;
        }

        //玩家擦杠
        public static bool CaGang(this FiveStarPlayer fiveStarPlayer, int card)
        {
            for (int i = 0; i < fiveStarPlayer.OperateInfos.Count; i++)
            {
                if (fiveStarPlayer.OperateInfos[i].OperateType == FiveStarOperateType.Peng &&
                    fiveStarPlayer.OperateInfos[i].Card == card)
                {
                    if (fiveStarPlayer.RemoveCardCount(card, 1))
                    {
                        fiveStarPlayer.OperateInfos[i].OperateType = FiveStarOperateType.CaGang;
                        return true;
                    }
                }
            }
            return false;
        }

        //玩家胡牌
        public static bool HuPai(this FiveStarPlayer fiveStarPlayer, int card = 0, int playCardIndex = 0)
        {
            if (fiveStarPlayer.IsCanHu(card, playCardIndex))
            {
                if (card > 0)
                {
                    fiveStarPlayer.Hands.Add(card);
                }
                return true;
            }
            return false;
        }
    }

}
