using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{

    [ObjectSystem]
    public class JoyLdsPlayerDestroySystem : DestroySystem<JoyLdsPlayer>
    {
        public override void Destroy(JoyLdsPlayer self) 
        {
            self.EndGame();
        }
    }

    public static class JoyLdsPlayerSystem
    {
        //开始游戏 
        public static void StartGame(this JoyLdsPlayer joyLdsPlayer, Actor_JoyLds_StartGame actorJoyLdsStartGame)
        {
           // joyLdsPlayer.SendUserGateMessage(
          //      new S2G_UserStartGame() {UserId = joyLdsPlayer.pUser.UserId, SessionActorId = joyLdsPlayer.Id});//通知用户所在的网关服开始游戏
            joyLdsPlayer.SendMessageUser(actorJoyLdsStartGame);//通知客户端开始游戏

        }
        //成为地主
        public static void TurnLandlord(this JoyLdsPlayer joyLdsPlayer, RepeatedField<int> landordThreeCard)
        {
            for (int i = 0; i < landordThreeCard.Count; i++)
            {
                joyLdsPlayer.pHnads.Add(landordThreeCard[i]);
            }
        }
        //结束游戏
        public static void EndGame(this JoyLdsPlayer joyLdsPlayer)
        {
          //  joyLdsPlayer.SendUserGateMessage(
             //   new S2G_UserEndGame() { UserId = joyLdsPlayer.pUser.UserId});//通知用户所在的网关服结束游戏
        }
        //发牌 记录自己的手牌并且发送给客户端发牌的消息
        public static void Deal(this JoyLdsPlayer joyLdsPlayer, RepeatedField<int> cards)
        {
            joyLdsPlayer.pHnads = cards;
            Actor_JoyLds_Deal actorJoyLdsDeal = new Actor_JoyLds_Deal();
            actorJoyLdsDeal.Cards.Add(joyLdsPlayer.pHnads.ToArray());
            joyLdsPlayer.SendMessageUser(actorJoyLdsDeal);
        }
        //玩家出牌
        public static void PlayHand(this JoyLdsPlayer joyLdsPlayer, RepeatedField<int> cards)
        {
            foreach (var card in cards)
            {
                if (joyLdsPlayer.pHnads.Contains(card))
                {
                    joyLdsPlayer.pHnads.Remove(card);
                }
                else
                {
                    Log.Info("要出的牌自己手中没有:"+ card);
                }
            }
        }
        //检测自己的手牌 是否为空
        public static bool IsHandEmpty(this JoyLdsPlayer joyLdsPlayer)
        {
            return joyLdsPlayer.pHnads.Count == 0;
        }
        //发送消息给客户端
        public static void SendMessageUser(this JoyLdsPlayer joyLdsPlayer, IActorMessage iActorMessage)
        {
            joyLdsPlayer.pUser.SendeSessionClientActor(iActorMessage);
        }
        //发送玩家所在的网关服
        public static void SendUserGateMessage(this JoyLdsPlayer joyLdsPlayer, IMessage iMessage)
        {
            joyLdsPlayer.pUser.SendUserGateSession(iMessage);
        }
        
    }
}
