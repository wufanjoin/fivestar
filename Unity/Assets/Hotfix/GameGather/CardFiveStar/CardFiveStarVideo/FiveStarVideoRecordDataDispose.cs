using System;
using System.Collections.Generic;

using Google.Protobuf.Collections;

namespace ETHotfix
{
  public static class FiveStarVideoRecordDataDispose
    {
        public static List<object> DisposeRecordData(RepeatedField<MiltaryRecordData> datas)
        {
            List<object> finshDispseDatas=new List<object>();
            for (int i = 0; i < datas.Count; i++)
            {
                byte[] bytes = datas[i].Data.bytes;
                switch (datas[i].Opcode)
                {
                    case FiveStarVideoOpcode.GameInit:
                        Video_GameInit gameInit = FromBytes<Video_GameInit>(bytes) as Video_GameInit;
                        Video_GameInitDispose(finshDispseDatas, gameInit);
                        break;
                    case FiveStarVideoOpcode.ChuCard:
                        finshDispseDatas.Add(FromBytes<Actor_FiveStar_PlayCardResult>(bytes));
                        break;
                    case FiveStarVideoOpcode.MoCard:
                        finshDispseDatas.Add(FromBytes<Actor_FiveStar_MoPai>(bytes));
                        break;
                    case FiveStarVideoOpcode.OperateInfo:
                        finshDispseDatas.Add(FromBytes<Actor_FiveStar_OperateResult>(bytes));
                        break;
                    case FiveStarVideoOpcode.LiangDao:
                        finshDispseDatas.Add(FromBytes<Actor_FiveStar_LiangDao>(bytes));
                        break;
                    case FiveStarVideoOpcode.MaiMa:
                        finshDispseDatas.Add(FromBytes<Actor_FiveStar_MaiMa>(bytes));
                        break;
                    case FiveStarVideoOpcode.SmallResult:
                        finshDispseDatas.Add(FromBytes<Actor_FiveStar_SmallResult>(bytes));
                        break;
                }
               
            }
            return finshDispseDatas;
        }

        public static void Video_GameInitDispose(List<object> objects, Video_GameInit gameInit)
        {
            objects.Add(gameInit);
            Video_Deal deal = new Video_Deal();
            Video_PiaoFen piaofen = new Video_PiaoFen();
            for (int j = 0; j < gameInit.PlayerInfos.Count; j++)
            {
                deal.AllHands.Add(gameInit.PlayerInfos[j].Hands);
                piaofen.PiaoFens.Add(gameInit.PlayerInfos[j].PiaoFen);
            }
            FiveStarRoomConfig foveConfig = FiveStarRoomConfigFactory.Create(gameInit.RoomConfigs);
            if (foveConfig.MaxPiaoNum > 0)
            {
                objects.Add(piaofen);
            }
            objects.Add(deal);
            
         
        }
        private static object FromBytes<T>(byte[] bytes) 
        {
           return ETHotfix.ProtobufHelper.FromBytes(typeof(T), bytes, 0, bytes.Length);
        }
    }
}
