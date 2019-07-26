using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
 public  static  class JoyLdsRoomFlowSystem
    {
        //开始游戏
        public static void StartGame(this JoyLdsRoom joyLdsRoom)
        {
            joyLdsRoom.Reset();//每次开始游戏前清空一下数据
            Actor_JoyLds_StartGame actorJoyLdsStartGame = new Actor_JoyLds_StartGame();
            actorJoyLdsStartGame.PlayerInfos.Clear();
            foreach (var player in joyLdsRoom.pJoyLdsPlayerDic.Values)
            {
                JoyLds_PlayerInfo joyLdsPlayer = new JoyLds_PlayerInfo();
                joyLdsPlayer.User = player.pUser;
                joyLdsPlayer.SeatIndex = player.pSeatIndex;
                actorJoyLdsStartGame.PlayerInfos.Add(joyLdsPlayer);
            }
            foreach (var player in joyLdsRoom.pJoyLdsPlayerDic.Values)
            {
                player.StartGame(actorJoyLdsStartGame);
            }
            joyLdsRoom.Deal();//发了开始游戏的消息发 发牌消息
            joyLdsRoom.CurrRoomStateType = JoyLdsRoomStateType.GameIn;
        }

        //发牌
        public static void Deal(this JoyLdsRoom joyLdsRoom)
        {
            RepeatedField<RepeatedField<int>> distrbuteCards = JoyLdsGameDealLogic.DealCards();
            joyLdsRoom.LdsThreeCard = distrbuteCards[distrbuteCards.count-1];
            for (int i = 0; i < distrbuteCards.Count&&i< joyLdsRoom.pJoyLdsPlayerDic.Count; i++)
            {
                joyLdsRoom.pJoyLdsPlayerDic[i].Deal(distrbuteCards[i]);
            }
            joyLdsRoom.CanCallOrRobLandlord();
        }
        //玩家叫地主
        public static void PlayerCallLandlord(this JoyLdsRoom joyLdsRoom,int seatIndex,bool isApproval)
        {
            if (seatIndex != joyLdsRoom.CurrBeOperationSeatIndex)
            {
                Log.Info("不归这个玩家操作索引:"+ seatIndex);
                return;
            }
            if (isApproval)
            {
                if (joyLdsRoom.StartCallLandlordSeatIndex== seatIndex)
                {
                    joyLdsRoom.StartPlayerIsCallLds = true;
                }
                if (joyLdsRoom.SelectCallOrRobLandlordSeatIndex < 0)
                {
                    joyLdsRoom.SelectCallOrRobLandlordSeatIndex = seatIndex;
                }
                else
                {
                    Log.Info("已经有人叫过地主了 不能重复叫 当前违规叫地主人索引:" + seatIndex);
                }
            }
            joyLdsRoom.CallLanlordResult(seatIndex, isApproval);
            joyLdsRoom.CanCallOrRobLandlord();
        }
        //玩家抢地主
        public static void PlayerRobLandlord(this JoyLdsRoom joyLdsRoom, int seatIndex, bool isApproval)
        {
            if (seatIndex != joyLdsRoom.CurrBeOperationSeatIndex)
            {
                Log.Info("不归这个玩家操作索引:" + seatIndex);
                return;
            }
            if (isApproval)
            {
                joyLdsRoom.SelectCallOrRobLandlordSeatIndex = seatIndex;
            }
            joyLdsRoom.RobLanlordResult(seatIndex, isApproval);
            joyLdsRoom.CanCallOrRobLandlord();
        }
        //通知玩家叫地主或者抢地主
        public static void CanCallOrRobLandlord(this JoyLdsRoom joyLdsRoom)
        {
            if (joyLdsRoom.CurrBeOperationSeatIndex < 0)//判断当前有没有人选择叫或抢地主
            {
                //当前选择一个人叫地主
                joyLdsRoom.CanCallLanlord(joyLdsRoom.StartCallLandlordSeatIndex);
                joyLdsRoom.CurrBeOperationSeatIndex = joyLdsRoom.StartCallLandlordSeatIndex;
            }
            else
            {
                if (joyLdsRoom.IsTheEndOnceCallLds)//只有起始玩家第二次选择的时候才会进来
                {
                    joyLdsRoom.ConfirmLandlord(); //确定谁是地主
                    return;
                }
                joyLdsRoom.CurrBeOperationSeatIndex = SeatIndexTool.GetNextSeatIndex(joyLdsRoom.CurrBeOperationSeatIndex, JoyLdsRoom.RoomNumber - 1);//获取当前选择的下个玩家索引
                if (joyLdsRoom.CurrBeOperationSeatIndex == joyLdsRoom.StartCallLandlordSeatIndex)//如果下个选择的玩家就是起始玩家 说明已经叫过一轮了
                {
                    if (joyLdsRoom.SelectCallOrRobLandlordSeatIndex < 0)
                    {
                        joyLdsRoom.AnewDealBroadcast();//小于0表示没有一个人叫过地主
                    }
                    else
                    {
                        if (joyLdsRoom.StartPlayerIsCallLds && joyLdsRoom.SelectCallOrRobLandlordSeatIndex != joyLdsRoom.StartCallLandlordSeatIndex)//如果叫完一轮 最开始玩家也叫地主了 最优先的玩家 不是最开始的玩家 他还可以选择抢不抢
                        {
                            joyLdsRoom.IsTheEndOnceCallLds = true;
                            joyLdsRoom.CanRobLanlordBroadcast(joyLdsRoom.StartCallLandlordSeatIndex);
                            return;
                        }
                        joyLdsRoom.ConfirmLandlord(); //确定谁是地主
                    }
                    return;
                }
                if (joyLdsRoom.SelectCallOrRobLandlordSeatIndex < 0)
                {
                    joyLdsRoom.CanCallLanlord(joyLdsRoom.CurrBeOperationSeatIndex);//如果没人叫地主广播 可以叫地主的消息
                }
                else
                {
                    joyLdsRoom.CanRobLanlordBroadcast(joyLdsRoom.CurrBeOperationSeatIndex);//如果有人叫地主广播 可以抢地主的消息
                }
            }
        }

        //确定谁是地主
        public static void ConfirmLandlord(this JoyLdsRoom joyLdsRoom)
        {
            JoyLdsPlayer ldsPlayer = joyLdsRoom.pJoyLdsPlayerDic[joyLdsRoom.SelectCallOrRobLandlordSeatIndex];//要成为地主的玩家
            ldsPlayer.TurnLandlord(joyLdsRoom.LdsThreeCard);//玩家成为地主
            joyLdsRoom.ConfirmCampBroadcast(joyLdsRoom.SelectCallOrRobLandlordSeatIndex, ldsPlayer.pHnads, joyLdsRoom.LdsThreeCard);//直接广播确定地主的消息
            joyLdsRoom.CanPlayCardBroadcast(joyLdsRoom.LandlordSeatIndex, true);//确定完地主后马上就广播可以出牌的消息
        }

        //玩家出牌
        public static void PlayCard(this JoyLdsRoom joyLdsRoom, int seatIndex, RepeatedField<int> cards)
        {
            int playCardType = 0;
            if (joyLdsRoom.CurrPlayCardType == PlayCardType.None)
            {
                playCardType = JoyLdsGamePlayHandLogic.PlayerPlayHandIsRational(cards);
            }
            else if(joyLdsRoom.CurrBeOperationSeatIndex == seatIndex)
            {

                playCardType = JoyLdsGamePlayHandLogic.PlayerPlayHandIsRational(joyLdsRoom.CurrPlayCardType,joyLdsRoom.CurrPlayCardCards, cards);
            }
            if (playCardType!= PlayCardType.None)
            {
                joyLdsRoom.AttirmPlayCard(seatIndex, playCardType, cards);
            }
            else
            {
                Log.Info("玩家索引:" + seatIndex + "  出牌不合理");
            }
        }

        //玩家选择不出
        public static void DontPlay(this JoyLdsRoom joyLdsRoom, int seatIndex)
        {
            if (joyLdsRoom.CurrBeOperationSeatIndex == seatIndex)
            {
                joyLdsRoom.DontPlayBroadcast(seatIndex);//广播玩家不出牌的消息
                joyLdsRoom.CanPlayCard();//广播玩家可以出牌的消息
            }
            else
            {
                Log.Info("违规操作 玩家无法操作 不出牌 玩家索引:" + seatIndex);
            }
        }

        //检测没有问题 玩家确认出牌
        public static void AttirmPlayCard(this JoyLdsRoom joyLdsRoom, int seatIndex, int playCardType, RepeatedField<int> cards)
        {
            joyLdsRoom.AddMultiple(playCardType);//增加倍数
            joyLdsRoom.pJoyLdsPlayerDic[seatIndex].PlayHand(cards);//玩家对象出牌
            joyLdsRoom.PlayCardBroadcast(seatIndex, playCardType,cards);//广播玩家出牌的消息
            if (joyLdsRoom.pJoyLdsPlayerDic[seatIndex].IsHandEmpty())
            {
                //手牌为空了
                joyLdsRoom.GameResult(seatIndex);
                joyLdsRoom.CurrRoomStateType = JoyLdsRoomStateType.Preparation;
                return;
            }
            joyLdsRoom.CurrPlayCardType = playCardType;//记录出牌的类型
            joyLdsRoom.CurrPlayCardSeatIndex = seatIndex;//记录出牌玩家的索引
            joyLdsRoom.CurrPlayCardCards = cards;//记录玩家的出牌信息
            joyLdsRoom.CanPlayCard();//广播玩家可以出牌的消息
        }
        //根据当前倍数和谁赢了算出应该扣的豆子

        public static void GameResult(this JoyLdsRoom joyLdsRoom, int winSeatIndex)
        {
            bool isWinLandlord = winSeatIndex == joyLdsRoom.LandlordSeatIndex;//是不是地主赢了
            List<bool> winningValue = new List<bool>
            {
                !isWinLandlord,!isWinLandlord,!isWinLandlord //如果是地主赢了 就模认全是为输钱 (赢钱)
            };
            winningValue[joyLdsRoom.LandlordSeatIndex] = isWinLandlord; //然后把地主设为赢钱   (输钱)

            Actor_JoyLds_GameResult actorJoyLdsGameResult = new Actor_JoyLds_GameResult();
            actorJoyLdsGameResult.WinSeatIndex = winSeatIndex;
            for (int i = 0; i < winningValue.Count; i++)
            {
                JoyLds_PlayerResult joyLdsPlayerResult = new JoyLds_PlayerResult();
                joyLdsPlayerResult.SeatIndex = i;
                if (winningValue[i])
                {
                    if (isWinLandlord)
                    {
                        joyLdsPlayerResult.GetBeabs = (int)(joyLdsRoom.CurrMultiple * 2 * JoyLdsRoom.MultipleWinBeans);//自己赢了 地主赢了 自己就是地主
                    }
                    else
                    {
                        joyLdsPlayerResult.GetBeabs = (int)(joyLdsRoom.CurrMultiple * JoyLdsRoom.MultipleWinBeans);//自己赢了 地主输了 自己就是农民
                    }
                }
                else
                {
                    if (isWinLandlord)
                    {
                        joyLdsPlayerResult.GetBeabs = (int)(joyLdsRoom.CurrMultiple * -1 * JoyLdsRoom.MultipleWinBeans);//自己输了 地主赢了 自己就是农民
                    }
                    else
                    {
                        joyLdsPlayerResult.GetBeabs = (int)(joyLdsRoom.CurrMultiple * -2 * JoyLdsRoom.MultipleWinBeans);//自己输了 地主输了 自己就是地主
                    }
                }
                actorJoyLdsGameResult.PlayerResults.Add(joyLdsPlayerResult);
            }
            joyLdsRoom.GameResultBroadcast(actorJoyLdsGameResult);//广播输赢消息
        }


        //倍数增加
        public static void AddMultiple(this JoyLdsRoom joyLdsRoom, int playCardType)
        {
            switch (playCardType)
            {
                case PlayCardType.ZhaDan:
                    joyLdsRoom.CurrMultiple *= JoyLdsRoom.BombMultiple;
                    break;
                case PlayCardType.WangZha:
                    joyLdsRoom.CurrMultiple *= JoyLdsRoom.KingBombMultiple;
                    break;
            }
        }
        //玩家准备
        public static void PlayerPrepare(this JoyLdsRoom joyLdsRoom, int seatIndex)
        {
            if (joyLdsRoom.pJoyLdsPlayerDic[seatIndex].pUser.Beans < joyLdsRoom.BesansLowest)
            {
                //玩家豆子不够 自己解散房间
                joyLdsRoom.DissolveRoomBroadcast();
            }
        }
        //玩家退出房间
        public static void PlayerOutRoom(this JoyLdsRoom joyLdsRoom)
        {
            //解散房间
            joyLdsRoom.DissolveRoomBroadcast();
        }
    }
}
