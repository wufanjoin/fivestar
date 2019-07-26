using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static partial class FiveStarRoomSystem
    {
        //记录每小局初始化信息
        public static void DealFinishRecordGameInitInfo(this FiveStarRoom fiveStarRoom)
        {
            //只有房卡才记录
            if (fiveStarRoom.RoomType != RoomType.RoomCard)
            {
                return;
            }
            Video_GameInit videoGameInit=new Video_GameInit();
            videoGameInit.RoomConfigs = fiveStarRoom.RoomConfig.Configs;
            videoGameInit.RoomNumber = fiveStarRoom.RoomId;
            videoGameInit.PlayerInfos = fiveStarRoom.GetVideoPlayers();
            videoGameInit.OfficeNumber = fiveStarRoom.CurrOfficNum;
            //记录初始化信息
            fiveStarRoom.RecordInfo(FiveStarVideoOpcode.GameInit, videoGameInit);
        }

  
        //初始化玩家信息
        private static RepeatedField<Video_PlayerInfo> GetVideoPlayers(this FiveStarRoom fiveStarRoom)
        {
            RepeatedField<Video_PlayerInfo> playerInfos=new RepeatedField<Video_PlayerInfo>();
            for (int i = 0; i < fiveStarRoom.FiveStarPlayerDic.Count; i++)
            {
                Video_PlayerInfo player=ComponentFactory.Create<Video_PlayerInfo>();
                player.NowScore = fiveStarRoom.FiveStarPlayerDic[i].NowScore;
                player.Hands = fiveStarRoom.FiveStarPlayerDic[i].Hands;
                player.Icon = fiveStarRoom.FiveStarPlayerDic[i].User.Icon;
                player.PiaoFen = fiveStarRoom.FiveStarPlayerDic[i].PiaoNum;
                player.UserId = fiveStarRoom.FiveStarPlayerDic[i].User.UserId;
                player.Name = fiveStarRoom.FiveStarPlayerDic[i].User.Name;
                player.SeatIndex = i;
                playerInfos.Add(player);
            }
            return playerInfos;
        }
        //记录操作信息
        private static void RecordInfo(this FiveStarRoom fiveStarRoom, int opcode, Google.Protobuf.IMessage iMessage)
        {
            //只有房卡才记录
            if (fiveStarRoom.RoomType != RoomType.RoomCard)
            {
                return;
            }
            MiltaryRecordData miltaryRecordData = ComponentFactory.Create<MiltaryRecordData>();
            miltaryRecordData.Opcode = opcode;
            miltaryRecordData.Data =new ByteString(ProtobufHelper.ToBytes(iMessage)); ; 
            fiveStarRoom.CurrParticularMiltaryRecordData.MiltaryRecordDatas.Add(miltaryRecordData);

        }
        //记录碰杠胡操作信息信息
        public static void RecordOperateInfo(this FiveStarRoom fiveStarRoom, Actor_FiveStar_OperateResult operateResult)
        {
            fiveStarRoom.RecordInfo(FiveStarVideoOpcode.OperateInfo, operateResult);
        }
        //记录小结算信息
        public static  void RecordSmallResult(this FiveStarRoom fiveStarRoom, Actor_FiveStar_SmallResult smallResult)
        {
            fiveStarRoom.RecordInfo(FiveStarVideoOpcode.SmallResult, smallResult);
            fiveStarRoom.SaveParticularMiltary(smallResult);//正常小结算直接存储
        }
        //记录摸牌信息
        public static void RecordMoCard(this FiveStarRoom fiveStarRoom, Actor_FiveStar_MoPai moCardInfo)
        {
            fiveStarRoom.RecordInfo(FiveStarVideoOpcode.MoCard, moCardInfo);
        }
        //记录出牌信息
        public static void RecordChuCard(this FiveStarRoom fiveStarRoom, Actor_FiveStar_PlayCardResult chuCardResult)
        {
            fiveStarRoom.RecordInfo(FiveStarVideoOpcode.ChuCard, chuCardResult);
        }
        //记录亮倒信息信息
        public static void RecordLiangDao(this FiveStarRoom fiveStarRoom, Actor_FiveStar_LiangDao chuCardResult)
        {
            fiveStarRoom.RecordInfo(FiveStarVideoOpcode.LiangDao, chuCardResult);
        }
        //记录买马信息
        public static void RecordMaiMa(this FiveStarRoom fiveStarRoom, Actor_FiveStar_MaiMa maiMa)
        {
            fiveStarRoom.RecordInfo(FiveStarVideoOpcode.MaiMa, maiMa);
        }
    }
}