using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
  public static class FiveStarRoomMessageSystem
    {
        //广播操作消息
        public static void BroadcastOperateResults(this FiveStarRoom fiveStarRoom)
        {
            //如果还有正在操作的玩家 就暂时不广播消息
            if (fiveStarRoom.CanOperatePlayerIndex.Count > 0)
            {
                return;
            }
            //先清空可胡牌人数
            fiveStarRoom.HuPaiPlayerSeatIndexs.Clear();

            fiveStarRoom.intData = fiveStarRoom.BeforeOperateResults.Count;
            //广播消息 并 执行具体操作
            foreach (var beforeOperate in fiveStarRoom.BeforeOperateResults)
            {
                fiveStarRoom.FiveStarPlayerDic[beforeOperate.SeatIndex].ExecuteOperate(beforeOperate.OperateInfo);//如果执行 错误 就视为执行放弃操作
                fiveStarRoom.RecordOperateInfo(beforeOperate);//记录操作消息
                //广播此次操作消息
                fiveStarRoom.BroadcastMssagePlayers(beforeOperate);
                if (--fiveStarRoom.intData <= 0)
                {
                    //操作完成 后续操作 
                    fiveStarRoom.FiveStarPlayerDic[beforeOperate.SeatIndex].OperateFinishFollow(beforeOperate.OperateInfo); //两个玩家同时操作 同时放弃 只用执行一次后续操作
                }
                //杠牌的话还有分数扣除
                fiveStarRoom.PlayerGangPaiGetScore(beforeOperate.SeatIndex, beforeOperate.OperateInfo.OperateType);
                //记录已经胡牌的人数
                if (beforeOperate.OperateInfo.OperateType == FiveStarOperateType.FangChongHu ||
                    beforeOperate.OperateInfo.OperateType == FiveStarOperateType.ZiMo)
                {
                    fiveStarRoom.HuPaiPlayerSeatIndexs.Add(beforeOperate.SeatIndex);
                }
            }
            //如果胡牌人数大于0 就直接结算
            if (fiveStarRoom.HuPaiPlayerSeatIndexs.Count > 0)
            {
                fiveStarRoom.PlayerHuPaiResult(fiveStarRoom.BeforeOperateResults[0].OperateInfo.OperateType, fiveStarRoom.HuPaiPlayerSeatIndexs.ToArray());
            }
            //处理完消息后清除列表
            fiveStarRoom.BeforeOperateResults.Clear();
        }

        //广播玩家买码
        public static int BoradcastPlayerMaiMa(this FiveStarRoom fiveStarRoom)
        {
            if (fiveStarRoom.ResidueCards.Count > 0)
            {

                Actor_FiveStar_MaiMa actorFiveStarMaiMa = new Actor_FiveStar_MaiMa();
                actorFiveStarMaiMa.Card = fiveStarRoom.ResidueCards[fiveStarRoom.ResidueCards.Count - 1];
                fiveStarRoom.MaiMaCard = actorFiveStarMaiMa.Card;//记录买马的牌
                int maiMaMulttiple = CardFiveStarHuPaiLogic.GetMaiMaMultiple(actorFiveStarMaiMa.Card);
                actorFiveStarMaiMa.Score = maiMaMulttiple * fiveStarRoom.RoomConfig.BottomScore;
                fiveStarRoom.RecordMaiMa(actorFiveStarMaiMa);//记录买马信息
                fiveStarRoom.BroadcastMssagePlayers(actorFiveStarMaiMa);
                return maiMaMulttiple;
            }
            return 0;
        }

        //给房间内的所有玩家广播消息
        public static void BroadcastMssagePlayers(this FiveStarRoom fiveStarRoom, IActorMessage iActorMessage)
        {

            foreach (var player in fiveStarRoom.FiveStarPlayerDic.Values)
            {
                player.SendMessageUser(iActorMessage);
            }
        }
        //给房间内的所有玩家广播消息 指定一个玩家不广播
        public static void BroadcastMssagePlayersDivideThisPlayer(this FiveStarRoom fiveStarRoom, int playerSeatIndex, IActorMessage iActorMessage)
        {
            foreach (var player in fiveStarRoom.FiveStarPlayerDic.Values)
            {
                if (playerSeatIndex == player.SeatIndex)
                {
                    continue;
                }
                player.SendMessageUser(iActorMessage);
            }
        }
    }
}
