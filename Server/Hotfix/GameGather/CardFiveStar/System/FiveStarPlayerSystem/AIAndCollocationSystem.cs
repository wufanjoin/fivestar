using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class AIAndCollocationSystem
    {
        //设置托管的状态
        public static void SetCollocation(this FiveStarPlayer fiveStarPlayer, bool isCollocation)
        {
            if (fiveStarPlayer.IsCollocation == isCollocation)
            {
                return; //状态本来相同 就不做后续事件了
            }
            if (fiveStarPlayer.FiveStarRoom == null)
            {
                return;
            }
            //有超时才能 进入托管
            if (!fiveStarPlayer.FiveStarRoom.RoomConfig.IsHaveOverTime)
            {
                return;
            }
            if (fiveStarPlayer.FiveStarRoom.CurrRoomStateType == RoomStateType.ReadyIn)
            {
                isCollocation = false; //准备状态下 只能是 不托管状态
            }
            fiveStarPlayer.IsCollocation = isCollocation;
            if (fiveStarPlayer.IsCollocation)
            {
                fiveStarPlayer.CollocationAIOperate();
            }
            fiveStarPlayer.SendMessageUser(
                new Actor_FiveStar_CollocationChange() {IsCollocation = fiveStarPlayer.IsCollocation});
        }

        //托管和AI状态下出牌 判断出牌
        public static void AICollcationPlayCard(this FiveStarPlayer fiveStarPlayer,int playCard)
        {
            if (fiveStarPlayer.IsLiangDao)
            {
                fiveStarPlayer.PlayCard(fiveStarPlayer.MoEndHand); //如果亮倒 只能出最后摸的牌
                return;
            }
            if (!fiveStarPlayer.Hands.Contains(playCard) || fiveStarPlayer.FiveStarRoom.LiangDaoCanHuCards.Contains(playCard))
            {
                for (int i = 0; i < fiveStarPlayer.Hands.Count; i++)
                {
                    if (!fiveStarPlayer.FiveStarRoom.LiangDaoCanHuCards.Contains(fiveStarPlayer.Hands[i]))
                    {
                        fiveStarPlayer.PlayCard(fiveStarPlayer.Hands[i]);  //如果手牌中没有 最后摸的牌 或者摸的牌是放炮的牌 就出第一张手牌
                        return;
                    }
                }
                fiveStarPlayer.PlayCard(playCard);
                Log.Error("AI托管 手中的牌全都是放炮的牌");
            }
            else
            {
                fiveStarPlayer.PlayCard(playCard);
            }
        }
        //托管的默认操作
        public static void CollocationAIOperate(this FiveStarPlayer fiveStarPlayer)
        {
            if (fiveStarPlayer.FiveStarRoom.CurrRoomStateType == RoomStateType.GameIn)
            {
                fiveStarPlayer.boolData = true;
                if (fiveStarPlayer.FiveStarRoom.IsDaPiaoBeing && (!fiveStarPlayer.IsAlreadyDaPiao)) //如果在打漂中 并且自己没有打漂就打漂
                {
                    if (fiveStarPlayer.IsAI) //如果是AI就随便漂
                    {
                        fiveStarPlayer.DaPiao(
                            RandomTool.Random(0, fiveStarPlayer.FiveStarRoom.RoomConfig.MaxPiaoNum + 1)); //随机打漂
                    }
                    else
                    {
                        fiveStarPlayer.DaPiao(0); //默认是不漂
                    }
                }
                else if (fiveStarPlayer.IsCanPlayCard) //如果可以出牌 直接出 最后摸到的牌
                {
                    if (fiveStarPlayer.IsAI) //如果是AI出牌 保留手牌中有多张相同的
                    {
                        fiveStarPlayer.Hands.Sort(); //手牌排序
                        fiveStarPlayer.intData =fiveStarPlayer.Hands.IndexOf(fiveStarPlayer.MoEndHand); //获取摸到的牌 首次出现的位置

                        if (fiveStarPlayer.intData < fiveStarPlayer.Hands.Count - 2 &&fiveStarPlayer.Hands[fiveStarPlayer.intData + 1] == fiveStarPlayer.MoEndHand)
                        {
                            fiveStarPlayer.AICollcationPlayCard(fiveStarPlayer.Hands[fiveStarPlayer.Hands.Count - 1]); //摸到的牌有相同的 出牌组里最后一张牌
                        }
                        else
                        {
                            fiveStarPlayer.AICollcationPlayCard(fiveStarPlayer.MoEndHand); //没有相同的出摸到的牌 
                        }
                    }
                    else
                    {
                        fiveStarPlayer.AICollcationPlayCard(fiveStarPlayer.MoEndHand); //不是AI出牌直接出 最后摸到的牌 
                    }
                }
                else if (fiveStarPlayer.FiveStarRoom.CanOperatePlayerIndex.Contains(fiveStarPlayer.SeatIndex)
                ) //如果玩家可操作索引列表里面有自己 则直接操作
                {
                    FiveStarOperateInfo fiveStarOperateInfo;
                    if (fiveStarPlayer.canOperateLists.Contains(FiveStarOperateType.FangChongHu)) //如果可以胡 就胡
                    {
                        fiveStarOperateInfo = FiveStarOperateInfoFactory.Create(0, FiveStarOperateType.FangChongHu, 0);
                    }
                    else
                    {
                        fiveStarOperateInfo =
                            FiveStarOperateInfoFactory.Create(0, FiveStarOperateType.None, 0); //不能胡就放弃
                    }
                    if (fiveStarPlayer.IsAI) //如果是AI 就是能碰就碰 能杠就杠 因为发的牌会做特殊手脚
                    {
                        if (fiveStarPlayer.canOperateLists.Contains(FiveStarOperateType.Peng)) //如果可以胡 就胡
                        {
                            fiveStarOperateInfo.OperateType = FiveStarOperateType.Peng;
                        }
                        else if (fiveStarPlayer.canOperateLists.Contains(FiveStarOperateType.MingGang))
                        {
                            //杠牌 不仅需要传递 操作类型 还是传杠那张牌 之前记录亮了
                            foreach (var canGang in fiveStarPlayer.canGangCards)
                            {
                                fiveStarOperateInfo.Card = canGang.Key;
                                fiveStarOperateInfo.OperateType = canGang.Value;
                            }

                        }
                    }
                    fiveStarPlayer.OperatePengGangHu(fiveStarOperateInfo); //执行操作
                    //不能销毁 fiveStarPlayer 因为多人操作会保留一段时间
                }
                else
                {
                    fiveStarPlayer.boolData = false;
                }
                if (fiveStarPlayer.boolData)
                {
                    fiveStarPlayer.SetCollocation(true); //如果进行了 托管操作 就进入托管
                }
            }
        }

        public const int LiangDaoMoCount = 7; //在摸第几张牌的时候 亮倒

        public const int HuMoCount = 10; //在摸第几张牌的时候胡牌

        //玩家摸牌处理 摸牌之前调用
        public static int AIMoPaiDispose(this FiveStarPlayer fiveStarPlayer, int card)
        {
            if (!fiveStarPlayer.IsAI)
            {
                return card;
            }
            fiveStarPlayer.MoCardCount++;
            if (fiveStarPlayer.MoCardCount == HuMoCount)
            {
                int wincard = fiveStarPlayer.FiveStarRoom.ResidueCards[fiveStarPlayer.FiveStarRoom.ResidueCards.Count - 1];//获取必赢牌的 最后摸的牌
                fiveStarPlayer.FiveStarRoom.ResidueCards.Remove(wincard);
                fiveStarPlayer.FiveStarRoom.ResidueCards.Add(card);
                return wincard;
            }
            return card;
        }

        //玩家出牌 出完牌后调用
        public static void AIPlayCardDispose(this FiveStarPlayer fiveStarPlayer)
        {
            if (!fiveStarPlayer.IsAI)
            {
                return;
            }
            if (!fiveStarPlayer.IsLiangDao&&fiveStarPlayer.MoCardCount == LiangDaoMoCount)
            {
                //替换手牌

                List<int> newHands = fiveStarPlayer.FiveStarRoom.ResidueCards.GetRange(fiveStarPlayer.FiveStarRoom.ResidueCards.Count - 1 - fiveStarPlayer.Hands.Count,fiveStarPlayer.Hands.Count);//获取隐藏在 剩余牌尾部必赢的牌
                int wincard=fiveStarPlayer.FiveStarRoom.ResidueCards[fiveStarPlayer.FiveStarRoom.ResidueCards.Count - 1];//获取必赢牌的 最后摸的牌

                fiveStarPlayer.FiveStarRoom.ResidueCards.Remove(wincard);//删除必赢摸的牌
                for (int i = 0; i < newHands.Count; i++)
                {
                    fiveStarPlayer.FiveStarRoom.ResidueCards.Remove(newHands[i]);//删除必赢的牌
                }
                fiveStarPlayer.FiveStarRoom.ResidueCards.AddRange(fiveStarPlayer.Hands);//把现有的手牌添加到剩余牌数组里面
                fiveStarPlayer.FiveStarRoom.ResidueCards.Add(wincard);//添加必赢摸的牌 到最后


                fiveStarPlayer.Hands.Clear();//清除当前手牌
                fiveStarPlayer.Hands.Add(newHands);//添加必赢的牌
                fiveStarPlayer.LiangDao();//正常情况下决定可以亮倒
            }
        }

        public static List<int> GetRange(this RepeatedField<int> repFieldInt, int index, int count)
        {
            List<int> array=new List<int>();
            for (int i = index; i < index+count; i++)
            {
                array.Add(repFieldInt[i]);
            }
            return array;
        }
        //延迟打漂AI打漂
        public static async void AIDelayDaPiao(this FiveStarPlayer fiveStarPlayer)
        {
            if (!fiveStarPlayer.IsAI)//不是AI直接 返回
            {
                return;
            }
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(RandomTool.Random(1, 4) * 1000);
            fiveStarPlayer.CollocationAIOperate();
        }
    }
}
