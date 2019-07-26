using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETModel;

namespace ETHotfix
{
   public static class FiveStarPlayerMessageSystem
    {
        //广播玩家可以操作信息
        public static void CanOperate(this FiveStarPlayer fiveStarPlayer, int qiOprateNextStep)
        {

            Actor_FiveStar_CanOperate actorFiveStarCanOperate = new Actor_FiveStar_CanOperate();
            fiveStarPlayer.SendMessageOtherUser(actorFiveStarCanOperate);
            actorFiveStarCanOperate.SeatIndex = fiveStarPlayer.SeatIndex;
            actorFiveStarCanOperate.CanOperateLits = fiveStarPlayer.canOperateLists;
            actorFiveStarCanOperate.CanGangLits.Add(fiveStarPlayer.canGangCards.Keys.ToArray());
            fiveStarPlayer.SendMessageUser(actorFiveStarCanOperate);
            fiveStarPlayer.FiveStarRoom.QiOperateNextStep = qiOprateNextStep;
            fiveStarPlayer.FiveStarRoom.PlayerCanOperate(actorFiveStarCanOperate);//告诉房间玩家可以操作
        }

        //广播玩家可以出牌的消息
        public static void CanChuPai(this FiveStarPlayer fiveStarPlayer)
        {
            fiveStarPlayer.IsCanPlayCard = true;
            Actor_FiveStar_CanPlayCard actorFiveStarCan =
                new Actor_FiveStar_CanPlayCard() { SeatIndex = fiveStarPlayer.SeatIndex };
            fiveStarPlayer.FiveStarRoom.PlayerCanChuPai(actorFiveStarCan);
            fiveStarPlayer.FiveStarRoom.BroadcastMssagePlayers(actorFiveStarCan);
        }

        //发给房间里面其他玩家消息
        public static void SendMessageOtherUser(this FiveStarPlayer fiveStarPlayer, IActorMessage iActorMessage)
        {
            fiveStarPlayer.FiveStarRoom.BroadcastMssagePlayersDivideThisPlayer(fiveStarPlayer.SeatIndex, iActorMessage);
        }

        //发送消息给网关 网关会根据消息转发给客户端
        public static void SendMessageUser(this FiveStarPlayer fiveStarPlayer, IActorMessage iActorMessage)
        {
            if (fiveStarPlayer.IsAI)
            {
                return;
            }
            if (fiveStarPlayer.User != null)
            {
                fiveStarPlayer.User.SendeSessionClientActor(iActorMessage);
            }

        }

    }
}
