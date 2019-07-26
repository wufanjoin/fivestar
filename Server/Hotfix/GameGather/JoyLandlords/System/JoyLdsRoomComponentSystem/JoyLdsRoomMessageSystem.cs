using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class JoyLdsRoomMessageSystem
    {
        //给房间内的所有玩家广播消息
        public static void BroadcastMssagePlayers(this JoyLdsRoom joyLdsRoom, IActorMessage iActorMessage)
        {
            foreach (var player in joyLdsRoom.pJoyLdsPlayerDic.Values)
            {
                player.SendMessageUser(iActorMessage);
            }
        }
        //给房间内的所有玩家广播消息 这是带手牌信息的消息
        public static void BroadcastMssagePlayers(this JoyLdsRoom joyLdsRoom, IHandActorMessage handActorMessage)
        {
            joyLdsRoom.pJoyLdsPlayerDic[handActorMessage.SeatIndex].SendMessageUser(handActorMessage);
            int handCount = handActorMessage.Hands.count;
            handActorMessage.Hands.Clear();
            handActorMessage.Hands.Add(handCount);
            foreach (var player in joyLdsRoom.pJoyLdsPlayerDic.Values)
            {
                if (handActorMessage.SeatIndex == player.pSeatIndex)
                {
                    continue;
                }
                player.SendMessageUser(handActorMessage);
            }
        }
        //广播叫地主结果
        public static void CallLanlordResult(this JoyLdsRoom joyLdsRoom, int seatIndex,bool isApproval)
        {
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_CallLanlord() { SeatIndex = seatIndex, Result= isApproval });
        }
        //广播抢地主结果
        public static void RobLanlordResult(this JoyLdsRoom joyLdsRoom, int seatIndex, bool isApproval)
        {
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_RobLanlord() { SeatIndex = seatIndex, Result = isApproval });
        }
        //广播可以叫地主消息
        public static void CanCallLanlord(this JoyLdsRoom joyLdsRoom, int seatIndex)
        {
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_CanCallLanlord() { SeatIndex = seatIndex });
            joyLdsRoom.CurrBeOperationSeatIndex = seatIndex;
        }

        //广播可以抢地主消息
        public static void CanRobLanlordBroadcast(this JoyLdsRoom joyLdsRoom, int seatIndex)
        {
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_CanRobLanlord() { SeatIndex = seatIndex });
            joyLdsRoom.CurrBeOperationSeatIndex = seatIndex;
        }
  
        //广播确定阵营消息
        public static void ConfirmCampBroadcast(this JoyLdsRoom joyLdsRoom, int landlordSeatIndex, RepeatedField<int> ldsHands, RepeatedField<int> threeCards)
        {
            RepeatedField<int> newLdsHands=new RepeatedField<int>(){ ldsHands.ToArray() };
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_ConfirmCamp() { LandlordSeatIndex = landlordSeatIndex, SeatIndex= landlordSeatIndex, Hands= newLdsHands, LandlordThreeCard= threeCards });
            joyLdsRoom.LandlordSeatIndex = landlordSeatIndex;
        }



        //广播玩家出牌消息
        public static void PlayCardBroadcast(this JoyLdsRoom joyLdsRoom, int seatIndex,int playCardType, RepeatedField<int> playCards)
        {
            Actor_JoyLds_PlayCard actorJoyLdsPlayCard = new Actor_JoyLds_PlayCard();
            actorJoyLdsPlayCard.SeatIndex = seatIndex;
            actorJoyLdsPlayCard.PlayCardType = playCardType;
            actorJoyLdsPlayCard.Cards=playCards;
            actorJoyLdsPlayCard.Hands.Add(joyLdsRoom.pJoyLdsPlayerDic[seatIndex].pHnads);
            joyLdsRoom.BroadcastMssagePlayers(actorJoyLdsPlayCard);

        }

        //判断玩家是不是第一手出牌 并广播可以出牌的消息
        public static void CanPlayCard(this JoyLdsRoom joyLdsRoom)
        {
            int playCardSeatIndex = SeatIndexTool.GetNextSeatIndex(joyLdsRoom.CurrBeOperationSeatIndex, JoyLdsRoom.RoomNumber-1);
            bool isFirst = playCardSeatIndex == joyLdsRoom.CurrPlayCardSeatIndex;
            if (isFirst)
            {
                joyLdsRoom.CurrPlayCardType = PlayCardType.None;
                joyLdsRoom.CurrPlayCardCards=null;
            }
            joyLdsRoom.CanPlayCardBroadcast(playCardSeatIndex, isFirst);
        }
        //广播玩家可以出牌的消息
        public static void CanPlayCardBroadcast(this JoyLdsRoom joyLdsRoom, int seatIndex, bool isFirst)
        {
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_CanPlayCard() { SeatIndex = seatIndex,IsFirst = isFirst });
            joyLdsRoom.CurrBeOperationSeatIndex = seatIndex;
        }
        //广播玩家不出牌的消息
        public static void DontPlayBroadcast(this JoyLdsRoom joyLdsRoom, int seatIndex)
        {
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_DontPlay() { SeatIndex = seatIndex });
        }
        //广播房间解散的消息
        public static void DissolveRoomBroadcast(this JoyLdsRoom joyLdsRoom)
        {
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_DissolveRoom());
            joyLdsRoom.Dispose();
        }
       
        //广播结算消息
        public static void GameResultBroadcast(this JoyLdsRoom joyLdsRoom,
            Actor_JoyLds_GameResult actorJoyLdsGameResult)
        {
            joyLdsRoom.BroadcastMssagePlayers(actorJoyLdsGameResult);//广播游戏结算的消息
        }
        //重新发牌
        public static void AnewDealBroadcast(this JoyLdsRoom joyLdsRoom)
        {
            joyLdsRoom.Reset();//重置一下游戏数据
            joyLdsRoom.BroadcastMssagePlayers(new Actor_JoyLds_AnewDeal());//要重新发牌的 消息
            joyLdsRoom.Deal();//发牌
        }
    }
}
