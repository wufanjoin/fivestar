using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [ObjectSystem]
    public class FiveStarPlayerDestroySystem : DestroySystem<FiveStarPlayer>
    {
        public override void Destroy(FiveStarPlayer self)
        {
            //有可能 这个对象只是给客户端发消息用的
            if (self.User.UserId == 0)
            {
                return;
            }
            //给网关服发消息 用户不在游戏中了
            self.SendMessageUser(new Actor_UserEndGame());//通知用户所在的网关服结束游戏
        }
    }
    public static  class FiveStarPlayerSystem
    {
        //开始游戏 
        public static void StartGame(this FiveStarPlayer fiveStarPlayer, Actor_FiveStar_StartGame actorJoyLdsStartGame)
        {
            fiveStarPlayer.SendGateStartGame();//通知用户所在的网关服开始游戏
            fiveStarPlayer.SendMessageUser(actorJoyLdsStartGame);//通知客户端开始游戏
        }
        //通知用户所在的网关服开始游戏 就是把 游戏服的对象id发过去
        public static void SendGateStartGame(this FiveStarPlayer fiveStarPlayer)
        {
            fiveStarPlayer.SendMessageUser(new Actor_UserStartGame() {SessionActorId = fiveStarPlayer.Id });//通知用户所在的网关服开始游戏
        }
        //发牌
        public static void Deal(this FiveStarPlayer fiveStarPlayer, RepeatedField<int> cards)
        {
            fiveStarPlayer.Hands = cards;
            fiveStarPlayer.SendMessageUser(new Actor_FiveStar_Deal() { Cards = cards });//广播发牌的消息
        }

        //玩家出牌
        public static void PlayCard(this FiveStarPlayer fiveStarPlayer, int card)
        {
            if (!fiveStarPlayer.IsCanPlayCard)
            {
                return;//不是这个玩家 出牌 直接return
            }
            if (fiveStarPlayer.IsLiangDao && fiveStarPlayer.MoEndHand != card) //如果玩家亮倒 就只能出 最后摸的一张牌
            {
                Log.Error("玩家亮倒后 出的牌 不是 最后摸的牌" + card);
                card = fiveStarPlayer.MoEndHand;//直接帮他出最后摸到的牌
            }
            if (fiveStarPlayer.IsLiangDao && fiveStarPlayer.MoEndHand == card)
            {
                //如果是亮倒 状态下 并且出的是摸的最后一张牌 就不要检测 是不是放炮牌
            }
            else
            {
                if (fiveStarPlayer.FiveStarRoom.LiangDaoCanHuCards.Contains(card))
                {
                    for (int i = 0; i < fiveStarPlayer.Hands.Count; i++)
                    {
                        if (!fiveStarPlayer.FiveStarRoom.LiangDaoCanHuCards.Contains(fiveStarPlayer.Hands[i]))
                        {
                            Log.Error("玩家打出的牌 有亮倒玩家胡");//在没有亮倒情况 除非手中所有的牌都是放炮的牌 就不会进来 就不会return 才可以打 
                            return;
                        }
                    }
                }
            }

            if (!fiveStarPlayer.Hands.Contains(card))
            {
                Log.Error("玩家出的牌手牌中没有" + card);
                return;
            }
            if (fiveStarPlayer.FiveStarRoom.CanPlayCardPlayerIndex == fiveStarPlayer.SeatIndex)
            {
                if (fiveStarPlayer.Hands.Contains(card))
                {
                    fiveStarPlayer.Hands.Remove(card);
                    Actor_FiveStar_PlayCardResult actorFiveStarPlayCardResult = new Actor_FiveStar_PlayCardResult()
                    {
                        SeatIndex = fiveStarPlayer.SeatIndex,
                        Card = card
                    };
                    fiveStarPlayer.FiveStarRoom.RecordChuCard(actorFiveStarPlayCardResult);//记录出牌消息
                    fiveStarPlayer.FiveStarRoom.BroadcastMssagePlayers(actorFiveStarPlayCardResult);
                }
            }
            fiveStarPlayer.PlayCards.Add(card);
            fiveStarPlayer.FiveStarRoom.PlayerPlayCard(fiveStarPlayer.SeatIndex, card);
            fiveStarPlayer.IsCanPlayCard = false;
            fiveStarPlayer.SendNewestHands();//发送玩家最新的手牌信息
            fiveStarPlayer.AIPlayCardDispose();//AI出牌处理
        }

        public static void SendNewestHands(this FiveStarPlayer fiveStarPlayer)
        {
            fiveStarPlayer.SendMessageUser(new Actor_FiveStar_NewestHands() { Hands = fiveStarPlayer.Hands });//发送玩家最新的手牌信息
        }
        //玩家打出的牌 被别人吃掉
        public static void PlayCardByEatOff(this FiveStarPlayer fiveStarPlayer)
        {
            fiveStarPlayer.PlayCards.RemoveAt(fiveStarPlayer.PlayCards.Count - 1);
        }

        //玩家亮倒
        public static void LiangDao(this FiveStarPlayer fiveStarPlayer)
        {
            if (fiveStarPlayer.IsLiangDao)
            {
                return;
            }
            if (fiveStarPlayer.IsCanLiangDaoAndHuCardsAndNoneCards())
            {
                fiveStarPlayer.IsLiangDao = true;
                Actor_FiveStar_LiangDao actorFiveStarLiangDao = new Actor_FiveStar_LiangDao();
                actorFiveStarLiangDao.SeatIndex = fiveStarPlayer.SeatIndex;
                actorFiveStarLiangDao.Hnads = fiveStarPlayer.Hands;
                fiveStarPlayer.FiveStarRoom.RecordLiangDao(actorFiveStarLiangDao);
                fiveStarPlayer.FiveStarRoom.BroadcastMssagePlayers(actorFiveStarLiangDao);//广播玩家亮倒信息
            }
        }
        //获取亮倒无关牌 和可以胡的牌 返回 是否可以亮牌
        public static bool IsCanLiangDaoAndHuCardsAndNoneCards(this FiveStarPlayer fiveStarPlayer)
        {
            List<int> huCards = CardFiveStarHuPaiLogic.IsTingPai(fiveStarPlayer.Hands);
            if (huCards.Count > 0)
            {
                fiveStarPlayer.LiangDaoNoneCards = CardFiveStarHuPaiLogic.GetLiangDaoNoneHuCards(fiveStarPlayer.Hands)[1];
                fiveStarPlayer.FiveStarRoom.AddLiangDaoCanHuCards(huCards);
            }
            return huCards.Count > 0;
        }
        //玩家打漂
        public static void DaPiao(this FiveStarPlayer fiveStarPlayer, int piaoNum)
        {
            if (fiveStarPlayer.IsRestIn)
            {
                return;//休息中不能打漂
            }
            if (!fiveStarPlayer.IsAlreadyDaPiao)
            {
                if (fiveStarPlayer.FiveStarRoom.IsDaPiaoBeing)
                {
                    fiveStarPlayer.PiaoNum = piaoNum;
                    //广播所有的消息
                    fiveStarPlayer.FiveStarRoom.BroadcastMssagePlayers(new Actor_FiveStar_DaPiaoResult()
                    {
                        SeatIndex = fiveStarPlayer.SeatIndex,
                        SelectPiaoNum = piaoNum
                    });
                    fiveStarPlayer.FiveStarRoom.PlayerDaPiao();//告诉房间玩家打漂
                }
            }
            fiveStarPlayer.IsAlreadyDaPiao = true;
        }

        //玩家操作结果 先告诉房间
        public static void OperatePengGangHu(this FiveStarPlayer fiveStarPlayer, FiveStarOperateInfo operateInfo)
        {
            operateInfo.PlayCardIndex = fiveStarPlayer.FiveStarRoom.CurrChuPaiIndex;
            fiveStarPlayer.FiveStarRoom.PlayerOperate(fiveStarPlayer.SeatIndex, operateInfo);
        }


        //玩家摸牌
        public static void MoPai(this FiveStarPlayer fiveStarPlayer, int card)
        {
            card = fiveStarPlayer.AIMoPaiDispose(card);//AI摸牌处理
            fiveStarPlayer.MoEndHand = card;//记录摸的最后一张手牌
            fiveStarPlayer.Hands.Add(card);
            //广播给客户端摸牌消息 但是只有摸的人 牌才是对的
            Actor_FiveStar_MoPai actorFiveStarMoPai = new Actor_FiveStar_MoPai();
            actorFiveStarMoPai.SeatIndex = fiveStarPlayer.SeatIndex;
            actorFiveStarMoPai.Card = card;
            fiveStarPlayer.FiveStarRoom.RecordMoCard(actorFiveStarMoPai);//记录摸牌消息
            fiveStarPlayer.SendMessageUser(actorFiveStarMoPai);
            actorFiveStarMoPai.Card = -1;
            fiveStarPlayer.SendMessageOtherUser(actorFiveStarMoPai);

            fiveStarPlayer.Hands.Sort();//每次检测前要把手牌排序一下
            if (fiveStarPlayer.IsCanOperate())
            {
                fiveStarPlayer.CanOperate(FiveStarOperateType.ChuCard);
            }
            else
            {
                fiveStarPlayer.CanChuPai();
            }
        }


        //玩家胡牌获取胡牌的类型和 总倍数
        public static RepeatedField<int> GetHuPaiType(this FiveStarPlayer fiveStarPlayer, int winCard, ref int totalMultiple)
        {
            fiveStarPlayer.boolData = fiveStarPlayer.Hands.Count % 3 == 1;//如果是1证明需要 赢的那张牌 才能形成胡牌
            if (fiveStarPlayer.boolData)
            {
                fiveStarPlayer.Hands.Add(winCard);
            }
            RepeatedField<int> types = CardFiveStarHuPaiLogic.GetHuPaiTypes(fiveStarPlayer.Hands, fiveStarPlayer.PengGangs, winCard, fiveStarPlayer.FiveStarRoom.IsGangShangCard);
            totalMultiple = CardFiveStarHuPaiLogic.GetMultiple(types, fiveStarPlayer.FiveStarRoom.RoomConfig.FengDingFanShu);
            if (fiveStarPlayer.boolData)
            {
                fiveStarPlayer.Hands.Remove(winCard);
            }
            return types;
        }




        //玩家准备
        public static void Ready(this FiveStarPlayer fiveStarPlayer)
        {
            if (fiveStarPlayer.FiveStarRoom.CurrRoomStateType == RoomStateType.ReadyIn)
            {
                fiveStarPlayer.ReadyState = true;
                fiveStarPlayer.FiveStarRoom.BroadcastMssagePlayers(new Actor_FiveStar_PlayerReady() { SeatIndex = fiveStarPlayer.SeatIndex });
                fiveStarPlayer.FiveStarRoom.PlayerReady();
            }
        }

        //玩家分数变化
        public static  void NowScoreChange(this FiveStarPlayer fiveStarPlayer,int getScore)
        {
            fiveStarPlayer.NowScore += getScore;
            if (fiveStarPlayer.FiveStarRoom.RoomType == RoomType.Match)
            {
                fiveStarPlayer.User.Beans += getScore;//收到加减豆子 自己这边手动 加减豆子
                fiveStarPlayer.User.GoodsChange(GoodsId.Besans, getScore, GoodsChangeType.None, false,false);//不要同步物品 因为如果是AI 是可能身处多个牌局的
            }
        }


    }
}
