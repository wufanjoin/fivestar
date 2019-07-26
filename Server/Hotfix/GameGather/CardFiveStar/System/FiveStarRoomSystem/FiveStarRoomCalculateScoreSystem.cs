using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
  public static class FiveStarRoomCalculateScoreSystem
    {
        //玩家胡牌进行分数计算
        public static void PlayerResultCalculateGrade(this FiveStarRoom fiveStarRoom, int winCard, int huPaiIndex, int operateType)
        {
            fiveStarRoom.FiveStarPlayerDic[huPaiIndex].HuPaiCount++;//胡牌次数加1
            fiveStarRoom.SmallPlayerResults[huPaiIndex].WinCard = winCard;//记录赢的哪张牌
            fiveStarRoom.SmallPlayerResults[huPaiIndex].HuPaiTypes = fiveStarRoom.FiveStarPlayerDic[huPaiIndex].GetHuPaiType(winCard, ref fiveStarRoom.intData);//记录胡牌类型 获得赢牌的倍数
            fiveStarRoom.intData2 = fiveStarRoom.PlayerMaiMa(huPaiIndex, operateType);//判断能不能买码 能就广播买码消息 并返回买码的倍数
            if (FiveStarOperateType.ZiMo == operateType)
            {
                fiveStarRoom.ZiMoCalculateScore(huPaiIndex);//自摸分数计算
            }
            else if (FiveStarOperateType.FangChongHu == operateType)
            {
                fiveStarRoom.FangChongHuCalculateScore(huPaiIndex); //放冲胡分数计算
            }
        }

        //自摸分数计算
        private static void ZiMoCalculateScore(this FiveStarRoom fiveStarRoom, int huPaiIndex)
        {
            fiveStarRoom.SmallPlayerResults[huPaiIndex].PlayerResultType = FiveStarPlayerResultType.ZiMoHu;//改变胡牌人的类型

            fiveStarRoom.FiveStarPlayerDic[huPaiIndex].ZiMoCount++;//自摸次数加1
            for (int j = 0; j < fiveStarRoom.RoomNumber; j++)
            {
                if (j == fiveStarRoom.CurrRestSeatIndex|| huPaiIndex==j)
                {
                    continue;//休息玩家不参与分数计算 自己不参与分数计算
                }
                fiveStarRoom.ResultCalculateScore(huPaiIndex, j, fiveStarRoom.intData, fiveStarRoom.intData2);//计算分数
            }
        }

        //放冲胡分数计算
        private static void FangChongHuCalculateScore(this FiveStarRoom fiveStarRoom, int huPaiIndex)
        {
            fiveStarRoom.SmallPlayerResults[huPaiIndex].PlayerResultType = FiveStarPlayerResultType.HuFangChong;//改变胡牌人的类型

            //计算玩家小结算得分
            fiveStarRoom.ResultCalculateScore(huPaiIndex, fiveStarRoom.CurrChuPaiIndex, fiveStarRoom.intData, fiveStarRoom.intData2);//计算分数

            fiveStarRoom.SmallPlayerResults[fiveStarRoom.CurrChuPaiIndex].PlayerResultType = FiveStarPlayerResultType.FangChong;//记录放冲人的类型
            fiveStarRoom.FiveStarPlayerDic[fiveStarRoom.CurrChuPaiIndex].FangChongCount++;//放冲次数加1
        }
        //计算分数
        private static void ResultCalculateScore(this FiveStarRoom fiveStarRoom, int huPaiIndex,int deductIndex, int paiTypeMultiple, int maiMaMultiple)
        {
            int unwelcomeCappingMultiple= maiMaMultiple+ fiveStarRoom.FiveStarPlayerDic[huPaiIndex].PiaoNum + fiveStarRoom.FiveStarPlayerDic[deductIndex].PiaoNum;//不受封顶番数 影响的 买马 和两人的漂数

            int effectCappingMultiple= paiTypeMultiple * fiveStarRoom.LiangDaoAddMultiple(huPaiIndex, deductIndex);//受封顶番数影响 基础倍数乘以 亮倒的倍数 

            if (effectCappingMultiple > fiveStarRoom.RoomConfig.FengDingFanShu)
            {
                effectCappingMultiple = fiveStarRoom.RoomConfig.FengDingFanShu;//如果 基础倍数加上亮倒倍数 超过封顶番数 则就是封顶番数
            }
            //上面算了 打漂的 倍数 亮倒的倍数 还要赢牌的倍数
            fiveStarRoom.SmallReultPlayerGetScore(huPaiIndex, deductIndex, (unwelcomeCappingMultiple + effectCappingMultiple) * fiveStarRoom.RoomConfig.BottomScore);
        }

        //查叫和亮倒赔付 计算分数
        public static void ChaJiaoLiangDaoPeiFu(this FiveStarRoom fiveStarRoom)
        {
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                if (i == fiveStarRoom.CurrRestSeatIndex)
                {
                    continue;//休息玩家没有手牌 不用赔付 但会收到别人的赔付
                }
                if (fiveStarRoom.FiveStarPlayerDic[i].IsLiangDao)
                {
                    fiveStarRoom.ChaJiaoLiangSmallResultGet(i, fiveStarRoom.RoomConfig.BottomScore * 2);
                    fiveStarRoom.SmallPlayerResults[i].HuPaiTypes.Add(CardFiveStarHuPaiType.LiangDaoPeiFu);
                }
                else if (!fiveStarRoom.FiveStarPlayerDic[i].IsTingCard())
                {
                    fiveStarRoom.ChaJiaoLiangSmallResultGet(i, fiveStarRoom.RoomConfig.BottomScore);
                    fiveStarRoom.SmallPlayerResults[i].HuPaiTypes.Add(CardFiveStarHuPaiType.ChaJiaoPeiFu);
                }
            }
        }
        //查叫和亮倒 赔付减分
        private static void ChaJiaoLiangSmallResultGet(this FiveStarRoom fiveStarRoom, int subScoreIndex, int scoreValue)
        {
            for (int j = 0; j < fiveStarRoom.RoomNumber; j++)
            {
                //计算玩家小结算得分
                fiveStarRoom.SmallReultPlayerGetScore(j, subScoreIndex, scoreValue);
            }
        }

        //玩家杠牌分数变化 和记录杠牌次数
        public static void PlayerGangPaiGetScore(this FiveStarRoom fiveStarRoom, int oprateSeateIndex, int oprateType)
        {
            if (oprateType != FiveStarOperateType.AnGang &&
                oprateType != FiveStarOperateType.CaGang &&
                oprateType != FiveStarOperateType.MingGang)
            {
                return;
            }
        
            fiveStarRoom.FiveStarPlayerDic[oprateSeateIndex].GangPaiCount++;//杠牌次数加1
            Actor_FiveStar_ScoreChange actorFiveStarScoreChange = new Actor_FiveStar_ScoreChange();
            for (int i = 0; i < fiveStarRoom.RoomNumber; i++)
            {
                actorFiveStarScoreChange.GetScore.Add(0);
            }
            fiveStarRoom.intData = 1;//默认 不是杠上杠就是1
            if (fiveStarRoom.IsGangShangCard)//如果是杠上杠
            {
                fiveStarRoom.intData = 2;//分数在乘以2
            }
            switch (oprateType)
            {
                case FiveStarOperateType.AnGang:
                    fiveStarRoom.CaGangAnGangScoreChang(actorFiveStarScoreChange, oprateSeateIndex,
                        fiveStarRoom.RoomConfig.BottomScore * 2 * fiveStarRoom.intData);//分数变化
                    break;
                case FiveStarOperateType.CaGang:
                    fiveStarRoom.CaGangAnGangScoreChang(actorFiveStarScoreChange, oprateSeateIndex,
                        fiveStarRoom.RoomConfig.BottomScore * fiveStarRoom.intData);//分数变化
                    break;
                case FiveStarOperateType.MingGang:
                    actorFiveStarScoreChange.GetScore[oprateSeateIndex] += fiveStarRoom.RoomConfig.BottomScore * 2 * fiveStarRoom.intData;
                    actorFiveStarScoreChange.GetScore[fiveStarRoom.CurrChuPaiIndex] -= fiveStarRoom.RoomConfig.BottomScore * 2 * fiveStarRoom.intData;
                    break;
                default:
                    return;
            }
            fiveStarRoom.GangHuNum = 2;//标记为 刚杠过
            for (int i = 0; i < fiveStarRoom.RoomNumber; i++)
            {
                fiveStarRoom.FiveStarPlayerDic[i].SmallGangScore += actorFiveStarScoreChange.GetScore[i];//记录杠牌得分
                fiveStarRoom.FiveStarPlayerDic[i].NowScoreChange(actorFiveStarScoreChange.GetScore[i]);//现在的分数变化
                actorFiveStarScoreChange.NowScore.Add(fiveStarRoom.FiveStarPlayerDic[i].NowScore);
            }
            //广播分数变化消息
            fiveStarRoom.BroadcastMssagePlayers(actorFiveStarScoreChange);
        }
        //擦杠和暗杠分数变化
        public static void CaGangAnGangScoreChang(this FiveStarRoom fiveStarRoom, Actor_FiveStar_ScoreChange actorFiveStarScoreChange, int gangCardIndex, int socre)
        {
            for (int i = 0; i < fiveStarRoom.RoomNumber; i++)
            {
                if (i == fiveStarRoom.CurrRestSeatIndex)
                {
                    continue;//休息的玩家 不参与杠牌计分
                }
                actorFiveStarScoreChange.GetScore[i] -= socre;//其他玩家 减去这个分数
                actorFiveStarScoreChange.GetScore[gangCardIndex] += socre;//杠牌玩家加上这个分数
            }
        }
        //小结算某个玩家得分
        public static void SmallReultPlayerGetScore(this FiveStarRoom fiveStarRoom,int getScoreIndex,int deducetScoreIndex,int socre)
        {
            //如果是匹配房间 要判断一下
            if (fiveStarRoom.RoomType==RoomType.Match)
            {
                //玩家扣除的豆子 不能超过自身拥有的豆子
                if (socre > fiveStarRoom.FiveStarPlayerDic[deducetScoreIndex].User.Beans)
                {
                    socre = (int)fiveStarRoom.FiveStarPlayerDic[deducetScoreIndex].User.Beans;
                }
                //玩家获得的豆子 不能超过自身拥有的豆子
                else if (socre > fiveStarRoom.FiveStarPlayerDic[getScoreIndex].User.Beans)
                {
                    socre = (int)fiveStarRoom.FiveStarPlayerDic[getScoreIndex].User.Beans;
                }
            }
            fiveStarRoom.SmallPlayerResults[getScoreIndex].GetScore += socre;//得分的人 加上这个分数
            fiveStarRoom.SmallPlayerResults[deducetScoreIndex].GetScore -= socre;//减分的人 减去这个分
        }
    }
}
