using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static partial class FiveStarRoomSystem
    {
        //存储大局战绩记录
        public static async Task SaveTotalMiltary(this FiveStarRoom fiveStarRoom)
        {
            //只有房卡才记录 如果一局小局信息都没有 就不用保存大局录像了
            if (fiveStarRoom.RoomType != RoomType.RoomCard|| fiveStarRoom.ParticularMiltarys.Count==0)
            {
                return;
            }
            Miltary miltary = ComponentFactory.Create<Miltary>();
            miltary.MiltaryId = FiveStarRoomComponent.Ins.GetMiltaryVideoId();//大局录像Id
            miltary.RoomNumber = fiveStarRoom.RoomId;//房号
            miltary.FriendCircleId = fiveStarRoom.FriendsCircleId;//所属亲友ID
            miltary.ToyGameId = ToyGameId.CardFiveStar;//游戏类型
            miltary.Time = TimeTool.GetCurrenTimeStamp();//当前时间
            miltary.PlayerInofs = fiveStarRoom.GetMiltaryPlayerInfo();//玩家信息
            for (int i = 0; i < miltary.PlayerInofs.Count; i++)
            {
                miltary.PlayerUserIds.Add(miltary.PlayerInofs[i].UserId);
            }
            fiveStarRoom.SaveMiltarySmallInfo(miltary.MiltaryId);
            await FiveStarRoomComponent.Ins.SaveVideo(miltary);//存储大局战绩到数据库
            miltary.Dispose();

        }
        //存储大局战绩小局信息到数据库
        private static async void SaveMiltarySmallInfo(this FiveStarRoom fiveStarRoom, int miltaryId)
        {
            MiltarySmallInfo miltarySmallInfo = ComponentFactory.Create<MiltarySmallInfo>();
            miltarySmallInfo.MiltaryId = miltaryId;
            miltarySmallInfo.ParticularMiltarys = fiveStarRoom.ParticularMiltarys;
            await FiveStarRoomComponent.Ins.SaveVideo(miltarySmallInfo);//存储大局战绩小局详情到数据库
            miltarySmallInfo.Dispose();
        }

        //存储小局战绩信息
        public static  void SaveParticularMiltary(this FiveStarRoom fiveStarRoom, Actor_FiveStar_SmallResult actorFiveStarSmall)
        {
            //只有房卡才记录
            if (fiveStarRoom.RoomType != RoomType.RoomCard)
            {
                return;
            }
            ParticularMiltary particularMiltary = ComponentFactory.Create<ParticularMiltary>();
            particularMiltary.DataId = fiveStarRoom.StartVideoDataId + fiveStarRoom.CurrOfficNum;//录像数据ID
            particularMiltary.Time = TimeTool.GetCurrenTimeStamp();//当前时间
            for (int i = 0; i < actorFiveStarSmall.SmallPlayerResults.Count; i++)
            {
                particularMiltary.GetScoreInfos.Add(actorFiveStarSmall.SmallPlayerResults[i].GetScore);//得分情况
            }
            fiveStarRoom.ParticularMiltarys.Add(particularMiltary);//添加到小局信息里面
            fiveStarRoom.SaveParticularMiltaryRecordData(particularMiltary.DataId); //存储当前小局对战具体信息
        }
        //存储当前小局对战具体信息
        private static async void SaveParticularMiltaryRecordData(this FiveStarRoom fiveStarRoom, int dataId)
        {
            fiveStarRoom.CurrParticularMiltaryRecordData.DataId = dataId;
            fiveStarRoom.CurrParticularMiltaryRecordData.ToyGameId = ToyGameId.CardFiveStar;
            await FiveStarRoomComponent.Ins.SaveVideo(fiveStarRoom.CurrParticularMiltaryRecordData);//存储当前小局对战具体信息到数据库
            fiveStarRoom.CurrParticularMiltaryRecordData.Dispose();
        }
        //获取大局战绩玩家信息
        private static RepeatedField<MiltaryPlayerInfo> GetMiltaryPlayerInfo(this FiveStarRoom fiveStarRoom)
        {
            RepeatedField<MiltaryPlayerInfo> playerInfos = new RepeatedField<MiltaryPlayerInfo>();
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                MiltaryPlayerInfo miltaryPlayerInfo = ComponentFactory.Create<MiltaryPlayerInfo>();
                miltaryPlayerInfo.Name = fiveStarRoom.FiveStarPlayerDic[i].User.Name;
                miltaryPlayerInfo.TotalScore = fiveStarRoom.FiveStarPlayerDic[i].NowScore;
                miltaryPlayerInfo.UserId = fiveStarRoom.FiveStarPlayerDic[i].User.UserId;
                playerInfos.Add(miltaryPlayerInfo);
            }
            return playerInfos;
        }
    }
}