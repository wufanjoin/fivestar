using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static partial class FiveStarRoomSystem
    {
        //开始游戏
        public static void StartGame(this FiveStarRoom fiveStarRoom)
        {
            Actor_FiveStar_StartGame actorFiveStarStart = new Actor_FiveStar_StartGame();
            actorFiveStarStart.RoomConfigs = fiveStarRoom.RoomConfig.Configs;
            actorFiveStarStart.RoomId = fiveStarRoom.RoomId;
            if (fiveStarRoom.RoomType == RoomType.Match)
            {
                actorFiveStarStart.RoomId = fiveStarRoom.MathRoomId;
            }

            foreach (var player in fiveStarRoom.FiveStarPlayerDic.Values)
            {
                //因为有些对象不能进行序列化 就必须重新复制一份
                actorFiveStarStart.PlayerInfos.Add(FiveStarPlayerFactory.CopySerialize(player));
            }
            foreach (var player in fiveStarRoom.FiveStarPlayerDic)
            {
                player.Value.StartGame(actorFiveStarStart);
            }
            //第一局随机出一个休息的玩家
            fiveStarRoom.NextRestSeatIndex = RandomTool.Random(1, fiveStarRoom.RoomConfig.RoomNumber);//不能为0 因为0要第一个摸牌
            fiveStarRoom.SmallStartGame();//小局开始游戏
            //用来发送消息的序列化对象 放入简易对象池 不能直接销毁 因为会改变一些东西
            foreach (var player in actorFiveStarStart.PlayerInfos)
            {
                FiveStarPlayerFactory.DisposeSerializePlayer(player);
            }
        }

        //小局开始游戏
        public static void SmallStartGame(this FiveStarRoom fiveStarRoom)
        {
            fiveStarRoom.CurrRoomStateType = RoomStateType.GameIn;//更改房间游戏状态
            Actor_FiveStar_SmallStartGame actorFiveStarStart = new Actor_FiveStar_SmallStartGame();
            actorFiveStarStart.CurrOfficNum = ++fiveStarRoom.CurrOfficNum;//当前局数
            fiveStarRoom.BroadcastMssagePlayers(actorFiveStarStart);//广播小局游戏开始
            fiveStarRoom.PlayerRest();//玩家轮休
            if (fiveStarRoom.RoomConfig.MaxPiaoNum > 0)
            {
                fiveStarRoom.BroadcastCanDaPiao();//广播可以打漂
            }
            else
            {
                fiveStarRoom.Deal();//发牌
            }
        }

        //玩家轮休
        public static void PlayerRest(this FiveStarRoom fiveStarRoom)
        {
            if (fiveStarRoom.RoomNumber != 4)
            {
                return;//不是4人房直接 返回
            }
            fiveStarRoom.FiveStarPlayerDic[fiveStarRoom.NextRestSeatIndex].IsRestIn = true;
            fiveStarRoom.BroadcastMssagePlayers(new Actor_FiveStar_PlayerRest() { RestSeatIndex = fiveStarRoom.NextRestSeatIndex });
            fiveStarRoom.CurrRestSeatIndex = fiveStarRoom.NextRestSeatIndex;
        }
        //发牌
        public static async void Deal(this FiveStarRoom fiveStarRoom)
        {
            fiveStarRoom.intData = fiveStarRoom.RoomNumber;
            if (fiveStarRoom.intData == 4)
            {
                fiveStarRoom.intData--;//4人房实际只有3个人 发牌
            }
            RepeatedField<RepeatedField<int>> dealCards;
            if (fiveStarRoom.IsHaveAI)
            {
                dealCards = CardFiveGameDealLogic.AIDealCards(fiveStarRoom.intData);//如果有AI就用AI发牌
            }
            else
            {
                dealCards = CardFiveGameDealLogic.DealCards(fiveStarRoom.intData);//如果没有有AI就正常发牌
            }
            fiveStarRoom.ResidueCards = dealCards[fiveStarRoom.intData];

            //四人房 在休息玩家的位置 多个一个空的数组
            if (fiveStarRoom.RoomNumber == 4)
            {
                RepeatedField<RepeatedField<int>> fourCards = new RepeatedField<RepeatedField<int>>();
                for (int i = 0; i < dealCards.Count; i++)
                {
                    if (i == fiveStarRoom.CurrRestSeatIndex)
                    {
                        fourCards.Add(new RepeatedField<int>());
                    }
                    fourCards.Add(dealCards[i]);
                }
                dealCards = fourCards;
            }
            //发牌
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                fiveStarRoom.FiveStarPlayerDic[i].Deal(dealCards[i]);
            }
            fiveStarRoom.DealFinishRecordGameInitInfo();//发完牌记录初始化信息
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(2000);//延迟两秒在摸牌
            fiveStarRoom.PlayerMoPai(fiveStarRoom.FirstMoCardSeatIndex);//发完牌第一个玩家摸牌
        }

        //通知所有玩家打漂
        public static void BroadcastCanDaPiao(this FiveStarRoom fiveStarRoom)
        {
            fiveStarRoom.IsDaPiaoBeing = true;
            fiveStarRoom.OverTime = fiveStarRoom.GetOverTime(0, FiveStarOverTimeType.DaPiaoType);//获取超时时间
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                fiveStarRoom.FiveStarPlayerDic[i].AIDelayDaPiao();//AI延迟打漂
            }
            fiveStarRoom.EntiretyOprationAleadyNum = 0;
            if (fiveStarRoom.RoomNumber == 4)
            {
                fiveStarRoom.EntiretyOprationAleadyNum++;//如果是4人 默认一个人已经做了操作
            }
            fiveStarRoom.BroadcastMssagePlayers(new Actor_FiveStar_CanDaPiao() { MaxPiaoNum = fiveStarRoom.RoomConfig.MaxPiaoNum });
        }

        //玩家选择打漂
        public static void PlayerDaPiao(this FiveStarRoom fiveStarRoom)
        {
            fiveStarRoom.EntiretyOprationAleadyNum++;
            if (fiveStarRoom.EntiretyOprationAleadyNum >= fiveStarRoom.RoomNumber)
            {
                fiveStarRoom.IsDaPiaoBeing = false;//不在打漂中了
                fiveStarRoom.Deal();//发牌
            }
        }
        //玩家准备
        public static void PlayerReady(this FiveStarRoom fiveStarRoom)
        {
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                if (!fiveStarRoom.FiveStarPlayerDic[i].ReadyState)
                {
                    return;
                }
            }
            fiveStarRoom.SmallStartGame();//开始小局游戏
        }

        //玩家进行出牌
        public static void PlayerPlayCard(this FiveStarRoom fiveStarRoom, int seatIndex, int card)
        {
            fiveStarRoom.GangHuNum--;//给杠上开的标记减1
            fiveStarRoom.CurrChuPaiIndex = seatIndex;
            fiveStarRoom.CurrChuPaiCard = card;
            fiveStarRoom.boolData = false;
            for (int i = 0; i < fiveStarRoom.RoomNumber; i++)
            {
                if (seatIndex == i)
                {
                    continue;
                }
                if (fiveStarRoom.FiveStarPlayerDic[i].IsCanOperate(card))//检测其他玩家能不能进行操作
                {
                    fiveStarRoom.FiveStarPlayerDic[i].CanOperate(FiveStarOperateType.MoCard);//广播可操作消息
                    fiveStarRoom.boolData = true;
                }
            }
            if (!fiveStarRoom.boolData)
            {
                fiveStarRoom.PlayerMoPai();
            }
        }

        //通知玩家摸牌
        public static void PlayerMoPai(this FiveStarRoom fiveStarRoom, int seatIndex = -1)
        {
            if (seatIndex >= 0)
            {
                fiveStarRoom.LastMoPaiSeatIndex = seatIndex;
            }
            else
            {
                fiveStarRoom.LastMoPaiSeatIndex = fiveStarRoom.GetNextSeatIndexExcludeRest(fiveStarRoom.CurrChuPaiIndex, fiveStarRoom.RoomNumber - 1);
            }
            if (fiveStarRoom.ResidueCards.Count <= 0)
            {
                fiveStarRoom.LiuJu();//流局
                return;
            }
            fiveStarRoom.FiveStarPlayerDic[fiveStarRoom.LastMoPaiSeatIndex].MoPai(fiveStarRoom.ResidueCards[0]);
            fiveStarRoom.ResidueCards.RemoveAt(0);//牌数组减掉 摸的哪张牌
        }

        //获取下个玩家索引 排序休息玩家索引
        public static int GetNextSeatIndexExcludeRest(this FiveStarRoom fiveStarRoom, int currSeatIndexint, int maxSeatIndex)
        {
            int nextSeat = SeatIndexTool.GetNextSeatIndex(currSeatIndexint, maxSeatIndex);
            if (fiveStarRoom.RoomNumber == 4 && nextSeat == fiveStarRoom.CurrRestSeatIndex)
            {
                nextSeat = SeatIndexTool.GetNextSeatIndex(nextSeat, maxSeatIndex);
            }
            return nextSeat;
        }

        //添加亮倒玩家可以胡的牌
        public static void AddLiangDaoCanHuCards(this FiveStarRoom fiveStarRoom, IList<int> cards)
        {
            fiveStarRoom.LiangDaoCanHuCards.Add(cards.ToArray());
        }

        //玩家买码
        public static int PlayerMaiMa(this FiveStarRoom fiveStarRoom, int huPaiIndex, int operateType)
        {
            if (operateType != FiveStarOperateType.ZiMo)
            {
                return 0;
            }
            if (fiveStarRoom.RoomConfig.MaiMaType == FiveStarMaiMaType.ZiMo)
            {
                return fiveStarRoom.BoradcastPlayerMaiMa();
            }
            else if (fiveStarRoom.RoomConfig.MaiMaType == FiveStarMaiMaType.LiangDaoZiMo && fiveStarRoom.FiveStarPlayerDic[huPaiIndex].IsLiangDao)
            {
                return fiveStarRoom.BoradcastPlayerMaiMa();
            }
            return 0;
        }

        //亮倒增加的倍数
        public static int LiangDaoAddMultiple(this FiveStarRoom fiveStarRoom, int huCardSeatIndex, int subSocreIndex)
        {
            if (fiveStarRoom.FiveStarPlayerDic[huCardSeatIndex].IsLiangDao || fiveStarRoom.FiveStarPlayerDic[subSocreIndex].IsLiangDao)
            {
                return 2;
            }
            return 1;
        }

        //获取玩家信息
        public static FiveStarPlayer GetPlayerInfoUserIdIn(this FiveStarRoom fiveStarRoom, long userId)
        {
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                if (fiveStarRoom.FiveStarPlayerDic[i].User.UserId == userId)
                {
                    return fiveStarRoom.FiveStarPlayerDic[i];
                }
            }
            return null;
        }
    }
}
