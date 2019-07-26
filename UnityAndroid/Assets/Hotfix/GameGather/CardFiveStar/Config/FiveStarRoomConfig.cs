using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    //结束方式
    public class FiveStarEndType
    {
        public const int JuShu=1;//局数
        public const int FenTuoDi = 2;//分拖底
    }

    //房费支付方式
    public class FiveStarPayMoneyType
    {
        public const int FangZhu = 1;//房主
        public const int JunTan = 2;//均摊
        public const int DaYingJia = 3;//大赢家
    }

    //买马方式
    public class FiveStarMaiMaType
    {
        public const int ZiMo = 2;//自摸
        public const int LiangDaoZiMo = 1;//亮倒自摸亮倒
        public const int BuMai = 0;//不买马
    }

    //外十五
    public class WaiShiWuType
    {
        public const int No = 0;//不是
        public const int Yes = 1;//是
    }

    /// <summary>
    /// 卡五星房间配置信息
    /// </summary>
    public class FiveStarRoomConfig:Entity
    {
        public RepeatedField<int> Configs;//配置信息
        public int EndType = 0;//结束方式
        public int MaxJuShu = 0;//最大局数
        public int TuoDiFen= 0;//拖底分数
        public int PayMoneyType = 0;//房费支付方式
        public int RoomNumber = 0;//房间人数
        public int BottomScore = 0;//底分
        public int MaxPiaoNum = 0;//最高漂数
        public int MaiMaType = 0;//买马方式
        public int WaiShiWuType = 0;//是否十五
        public int FengDingFanShu = 0;//封顶番数
        public bool IsHaveOverTime = false;//是否有超时

#if !SERVER
        public const string DescInterval = "  ";

        private static CardFiveStarRoomConfig payConfig;
        public static CardFiveStarRoomConfig _PayConfig
        {
            get
            {
                if (payConfig == null)
                {
                    payConfig = (CardFiveStarRoomConfig)Game.Scene.GetComponent<ConfigComponent>()
                        .Get(typeof(CardFiveStarRoomConfig), CardFiveStarRoomConfig.PayMoneyId);
                }
                return payConfig;
            }
        }

        public static CardFiveStarRoomConfig maiMaConfig;

        public static CardFiveStarRoomConfig _MaiMaConfig
        {
            get
            {
                if (maiMaConfig == null)
                {
                    maiMaConfig = (CardFiveStarRoomConfig)Game.Scene.GetComponent<ConfigComponent>()
                        .Get(typeof(CardFiveStarRoomConfig), CardFiveStarRoomConfig.MaiMaId);
                }
                return maiMaConfig;
            }
        }
        //获得玩法描述
        public string GetWanFaDesc(bool isShowPayMoeyType=true,int roomId = 0,int friendsCrircleId = 0)
        {
            string desc = "卡五星"+ DescInterval;
            if (roomId > 0)
            {
                desc += $"房号:{roomId}"+ DescInterval;
            }
            if (friendsCrircleId > 0)
            {
                desc += $"亲友圈:{friendsCrircleId}"+ DescInterval;
            }
            desc += $"{MaxJuShu}局" + DescInterval;
            if (isShowPayMoeyType&&friendsCrircleId == 0)
            {
                desc += $"房费:{_PayConfig.GetDescValueIn(PayMoneyType)}" + DescInterval;
            }
            desc += $"{RoomNumber}人局" + DescInterval;
            desc += $"{MaxPiaoNum}漂" + DescInterval;
            desc += _MaiMaConfig.GetDescValueIn(MaiMaType) + DescInterval;

            if (WaiShiWuType > 0)
            {
                desc += "自带一漂" + DescInterval;
            }
            if (FengDingFanShu ==0|| FengDingFanShu>1000)
            {
                desc += "不封顶" + DescInterval;
            }
            else
            {
               
                desc += $"{FengDingFanShu}倍封顶" + DescInterval;
            }
            return desc;
        }
#endif
    }
}
