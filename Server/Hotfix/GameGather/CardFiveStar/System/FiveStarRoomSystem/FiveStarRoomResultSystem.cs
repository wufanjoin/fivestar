using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FiveStarRoomDestroySystem : DestroySystem<FiveStarRoom>
    {
        public override void Destroy(FiveStarRoom self)
        {
            //给匹配服发消息 房间销毁了
            Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.Match).Send(new S2M_RoomDissolve()
            {
                RoomId = self.RoomId,
                CurrOfficNum=self.CurrOfficNum,
                CurrRoomStateType = self.CurrRoomStateType
            });
            Game.Scene.GetComponent<FiveStarRoomComponent>().RemoveRoom(self.RoomId);
        }
    }
    public static partial class FiveStarRoomSystem
    {
        //游戏流局
        public static void LiuJu(this FiveStarRoom fiveStarRoom)
        {
            fiveStarRoom.PlayerHuPaiResult(0);//先正常计算 杠分和一些得分
        }

        //玩家胡牌进行结算 小结算
        public static void PlayerHuPaiResult(this FiveStarRoom fiveStarRoom, int operateType, params int[] seatIndexs)
        {
            fiveStarRoom.InitSamllResultInfo();
            for (int i = 0; i < fiveStarRoom.RoomNumber; i++)
            {
                fiveStarRoom.SmallPlayerResults[i].SeatIndex = i;//玩家索引
                fiveStarRoom.SmallPlayerResults[i].SamllGangPaiScore = fiveStarRoom.FiveStarPlayerDic[i].SmallGangScore;//小局杠牌得分
                fiveStarRoom.SmallPlayerResults[i].Hands = fiveStarRoom.FiveStarPlayerDic[i].Hands;//玩家手牌
                fiveStarRoom.SmallPlayerResults[i].PengGangInfos = fiveStarRoom.FiveStarPlayerDic[i].OperateInfos;//碰杠操作信息
                if (seatIndexs.Contains(i))
                {
                    if (FiveStarOperateType.ZiMo == operateType)
                    {
                        fiveStarRoom.PlayerResultCalculateGrade(fiveStarRoom.FiveStarPlayerDic[i].MoEndHand, i, operateType);//算出胡牌得分
                    }
                    else if (FiveStarOperateType.FangChongHu == operateType)
                    {
                        fiveStarRoom.PlayerResultCalculateGrade(fiveStarRoom.CurrChuPaiCard, i, operateType);//算出胡牌得分
                    }
                }
            }
            //如果是胡牌人数是0 就要算 查叫 和亮倒赔付
            if (seatIndexs.Length == 0)
            {
                fiveStarRoom.ChaJiaoLiangDaoPeiFu();
            }
            //分数赋值
            for (int j = 0; j < fiveStarRoom.RoomNumber; j++)
            {
                fiveStarRoom.FiveStarPlayerDic[j].NowScoreChange(fiveStarRoom.SmallPlayerResults[j].GetScore);//现在的分数要加上胡牌得到分数
                fiveStarRoom.SmallPlayerResults[j].GetScore += fiveStarRoom.SmallPlayerResults[j].SamllGangPaiScore;//最终得分要算上小局杠牌得分
                fiveStarRoom.SmallPlayerResults[j].NowScore = fiveStarRoom.FiveStarPlayerDic[j].NowScore;//现在的分数
            }
            fiveStarRoom.SamllResultsFollow(seatIndexs);//处理小结算后续事件
        }
      
        //直接发送小结算
        public static  void SamllResultsFollow(this FiveStarRoom fiveStarRoom, int[] huCardSeatIndexs)
        {
            //发送小结算信息
            fiveStarRoom.SendSamllResults();
            //如果是匹配服直接销毁房间
            if (fiveStarRoom.RoomType == RoomType.Match)
            {
                fiveStarRoom.Dispose();
                return;
            }
            //算出下局最先摸牌的玩家索引 和4人房休息玩家索引
            fiveStarRoom.CalcuateFirstMoCardAndRestPlayerSeat(huCardSeatIndexs);

            fiveStarRoom.LittleRoundClearData();//如果是房卡模式 正常打下去 清空一下小局应该清空的数据
            //房间总结算
            if (fiveStarRoom.JudgeIsRoomTotalResult())
            {
                fiveStarRoom.RoomTotalResult();//如果进入总结算 房间直接就销毁了
                return;
            }
            fiveStarRoom.CurrRoomStateType = RoomStateType.ReadyIn;//更改房间游戏状态为准备中
            //所有玩家取消托管
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                fiveStarRoom.FiveStarPlayerDic[i].SetCollocation(false);
            }
        }
        //算出下局最先摸牌的玩家索引 和休息玩家索引
        public static void CalcuateFirstMoCardAndRestPlayerSeat(this FiveStarRoom fiveStarRoom, int[] huCardSeatIndexs)
        {
            if (fiveStarRoom.RoomNumber == 4)
            {
                if (huCardSeatIndexs.Length == 1)
                {
                    fiveStarRoom.FirstMoCardSeatIndex = fiveStarRoom.CurrRestSeatIndex;//4人房 做庄的就是上局休息的
                    fiveStarRoom.NextRestSeatIndex = huCardSeatIndexs[0];//4人房  下局休息 就是这局胡的
                }
                else if (huCardSeatIndexs.Length == 2)
                {
                    fiveStarRoom.NextRestSeatIndex = fiveStarRoom.CurrRestSeatIndex;//4人房  如果一炮多想下局休息的还是不变
                    fiveStarRoom.FirstMoCardSeatIndex = fiveStarRoom.CurrChuPaiIndex;//4人房  如果一炮多想 做庄的就是放炮的
                }
            }
            else
            {
                if (huCardSeatIndexs.Length == 1)
                {
                    fiveStarRoom.FirstMoCardSeatIndex = huCardSeatIndexs[0];//3人房 坐庄就是胡的人
                }
                else if (huCardSeatIndexs.Length == 2)
                {
                    fiveStarRoom.FirstMoCardSeatIndex = fiveStarRoom.CurrChuPaiIndex;//3人房 一炮多向 坐庄就是放炮的人
                }
            }
        }
            //计算下局谁坐庄先摸牌 和如果是四人房谁 休息
        
        //发送小结算消息
        public static void SendSamllResults(this FiveStarRoom fiveStarRoom)
        {
            Actor_FiveStar_SmallResult actorFiveStarSmall = new Actor_FiveStar_SmallResult();
            actorFiveStarSmall.SmallPlayerResults = fiveStarRoom.SmallPlayerResults;
            actorFiveStarSmall.MaiMaCard=fiveStarRoom.MaiMaCard;//买马的牌 赋值
            fiveStarRoom.RecordSmallResult(actorFiveStarSmall);//记录小结算信息
            fiveStarRoom.BroadcastMssagePlayers(actorFiveStarSmall);
        }
        //判断房间按正常流程需不需要总结算总结算
        public static bool JudgeIsRoomTotalResult(this FiveStarRoom fiveStarRoom)
        {
            if (fiveStarRoom.RoomType == RoomType.Match)
            {
                return false;
            }
            if (fiveStarRoom.RoomConfig.EndType == FiveStarEndType.JuShu)
            {
                return fiveStarRoom.CurrOfficNum >= fiveStarRoom.RoomConfig.MaxJuShu;
            }
            else
            {
                for (int i = 0; i < fiveStarRoom.RoomNumber; i++)
                {
                    if (fiveStarRoom.FiveStarPlayerDic[i].NowScore <= fiveStarRoom.RoomConfig.TuoDiFen)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        //房间总结算 解散投票也会走这
        public static async void RoomTotalResult(this FiveStarRoom fiveStarRoom)
        {
            Actor_FiveStar_TotalResult actorFiveStarTotalResult = new Actor_FiveStar_TotalResult();
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                actorFiveStarTotalResult.TotalPlayerResults.Add(FiveStarTotalPlayerResultFactory.Create(fiveStarRoom.FiveStarPlayerDic[i]));
            }
            fiveStarRoom.BroadcastMssagePlayers(actorFiveStarTotalResult);
            await fiveStarRoom.SaveTotalMiltary();//房间总结算 存储大局对战记录 
            fiveStarRoom.SendMiltaryFriendsCircle();//发送战绩给亲友圈服
            fiveStarRoom.Dispose();//直接销毁房间
        }
        //发送战绩给亲友圈服
        public static  void SendMiltaryFriendsCircle(this FiveStarRoom fiveStarRoom)
        {
            if (fiveStarRoom.FriendsCircleId > 0)
            {
                S2F_SendTotalPlayerInfo scFSendTotalPlayerInfo = new S2F_SendTotalPlayerInfo();
                scFSendTotalPlayerInfo.FriendsCrircleId = fiveStarRoom.FriendsCircleId;
                for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
                {
                    TotalPlayerInfo totalPlayerInfo=new TotalPlayerInfo();
                    totalPlayerInfo.UserId = fiveStarRoom.FiveStarPlayerDic[i].User.UserId;
                    totalPlayerInfo.TotalScore = fiveStarRoom.FiveStarPlayerDic[i].NowScore;
                    scFSendTotalPlayerInfo.TotalPlayerInfos.Add(totalPlayerInfo);
                }
                Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.FriendsCircle).Send(scFSendTotalPlayerInfo);
            }
        }
    }
}
