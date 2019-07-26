using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
 public  static class FiveStarRoomOperationSystem
    {
        //玩家可以进行出牌操作
        public static void PlayerCanChuPai(this FiveStarRoom fiveStarRoom, Actor_FiveStar_CanPlayCard actorFiveStarCan)
        {
            fiveStarRoom.OverTime = fiveStarRoom.GetOverTime(actorFiveStarCan.SeatIndex, FiveStarOverTimeType.PlayCardType);//获取超时时间 
            fiveStarRoom.CanPlayCardPlayerIndex = actorFiveStarCan.SeatIndex;
            fiveStarRoom.EndCanOperateAndCanChuMessage = actorFiveStarCan;//记录最后一条玩家可操作消息
        }

        //玩家可以进行操作
        public static void PlayerCanOperate(this FiveStarRoom fiveStarRoom, Actor_FiveStar_CanOperate actorFiveStarCanOperate)
        {
            if (!fiveStarRoom.CanOperatePlayerIndex.Contains(actorFiveStarCanOperate.SeatIndex))
            {
                fiveStarRoom.CanOperatePlayerIndex.Add(actorFiveStarCanOperate.SeatIndex);
                fiveStarRoom.OverTime = fiveStarRoom.GetOverTime(actorFiveStarCanOperate.SeatIndex, FiveStarOverTimeType.OperateType);//超时时间 
            }
            fiveStarRoom.EndCanOperateAndCanChuMessage = actorFiveStarCanOperate;//记录最后一条玩家可操作消息
        }

        //获取超时时间
        public static long GetOverTime(this FiveStarRoom fiveStarRoom,int playerSeatIndex,int overTimeType)
        {
            if (fiveStarRoom.FiveStarPlayerDic[playerSeatIndex].IsLiangDao&& overTimeType== FiveStarOverTimeType.PlayCardType)
            {
                return FiveStarRoomComponent.CurrTime +1;//如果亮倒了 出牌 就一秒时间
            }
            if (!fiveStarRoom.RoomConfig.IsHaveOverTime)
            {
                return 0;//配置没有超时 直接返回0
            }
            if (fiveStarRoom.FiveStarPlayerDic[playerSeatIndex].IsAI)
            {
                return FiveStarRoomComponent.CurrTime + RandomTool.Random(1, 4);//如果是AI加个随机值
            }
            if (overTimeType == FiveStarOverTimeType.DaPiaoType)
            {
                return FiveStarRoomComponent.CurrTime + FiveStarOverTime.CanDaPiaoOverTime;
            }
            if (fiveStarRoom.FiveStarPlayerDic[playerSeatIndex].IsCollocation)
            {
                return FiveStarRoomComponent.CurrTime + 1;
            }
            
            if (overTimeType== FiveStarOverTimeType.PlayCardType)
            {
                return FiveStarRoomComponent.CurrTime + FiveStarOverTime.CanPlayCardOverTime;
            }
            else if (overTimeType == FiveStarOverTimeType.OperateType)
            {
                return FiveStarRoomComponent.CurrTime + FiveStarOverTime.CanOperateOverTime;
            }
            return 0;
        }

        //超时操作
        public static void OverTimeOperate(this FiveStarRoom fiveStarRoom)
        {
            //如果在打漂中
            if (fiveStarRoom.IsDaPiaoBeing)
            {
                for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
                {
                    fiveStarRoom.FiveStarPlayerDic[i].CollocationAIOperate();
                }
            }
            else if (fiveStarRoom.CanOperatePlayerIndex.Count>0)
            {
                int[] operatIndexs = fiveStarRoom.CanOperatePlayerIndex.ToArray();//必须先复制一份 因为胡牌操作会清空自己
                //可以操作的人
                for (int i = 0; i < operatIndexs.Length; i++)
                {
                    fiveStarRoom.FiveStarPlayerDic[operatIndexs[i]].CollocationAIOperate();
                }
            }
            else
            {
                //可以出牌的人 都进行一下操作
                fiveStarRoom.FiveStarPlayerDic[fiveStarRoom.CanPlayCardPlayerIndex].CollocationAIOperate();
            }
        }

        //添加可操作消息
        public static void AddOperateResults(this FiveStarRoom fiveStarRoom, Actor_FiveStar_OperateResult actorFiveStarOperateResult)
        {
            //选出以前操作里面的昨天操作级别 但是里面操作级别肯定都一样的所以取第一个就行了
            if (fiveStarRoom.BeforeOperateResults.Count > 0)
            {
                fiveStarRoom.intData = fiveStarRoom.BeforeOperateResults[0].OperateInfo.OperateType;
            }
            else
            {
                //以前没有操作 级别默认-1
                fiveStarRoom.intData = -1;
            }
            //如果操作优先级相同就同时操作
            if (fiveStarRoom.intData == actorFiveStarOperateResult.OperateInfo.OperateType)
            {
                fiveStarRoom.BeforeOperateResults.Add(actorFiveStarOperateResult);//先记录操作消息
            }
            //如果新消息优先级别高就删除以前的消息
            else if (fiveStarRoom.intData < actorFiveStarOperateResult.OperateInfo.OperateType)
            {
                fiveStarRoom.BeforeOperateResults.Clear();
                fiveStarRoom.BeforeOperateResults.Add(actorFiveStarOperateResult);//先记录操作消息
            }
            //如果新消息优先级低 就忽略
        }

        //玩家进行操作
        public static void PlayerOperate(this FiveStarRoom fiveStarRoom, int seatIndex, FiveStarOperateInfo fiveStarOperateInfo)
        {
            if (fiveStarRoom.CanOperatePlayerIndex.Contains(seatIndex))
            {
                fiveStarRoom.CanOperatePlayerIndex.Remove(seatIndex);
                Actor_FiveStar_OperateResult actorFiveStarOperateResult = new Actor_FiveStar_OperateResult();
                actorFiveStarOperateResult.SeatIndex = seatIndex;
                actorFiveStarOperateResult.OperateInfo = fiveStarOperateInfo;

                fiveStarRoom.AddOperateResults(actorFiveStarOperateResult);//添加操作消息
                fiveStarRoom.BroadcastOperateResults();//广播操作消息
            }
        }
    }
}
