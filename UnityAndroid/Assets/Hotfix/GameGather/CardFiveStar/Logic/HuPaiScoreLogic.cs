using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public class CardFiveStarHuPaiType
    {
        /// <summary>碰碰胡</summary>
        public const int PengPengHu = 1;

        /// <summary>卡五星</summary>
        public const int CardFiveStar = 2;

        /// <summary>>明四归</summary>
        public const int MingSiGui = 3;

        /// <summary>暗四归</summary>
        public const int AnSiGui = 4;

        /// <summary>清一色</summary>
        public const int QingYiSe = 5;

        /// <summary>七对</summary>
        public const int QiDui = 6;

        /// <summary>猛七对</summary>
        public const int MengQiDui = 7;

        /// <summary>小三元</summary>
        public const int XiaoSanYuan = 8;

        /// <summary>大三元</summary>
        public const int DaSanYuan = 9;

        /// <summary>平胡</summary>
        public const int PingHu = 10;

        /// <summary>查叫赔付</summary>
        public const int ChaJiaoPeiFu = 11;

        /// <summary>亮倒赔付</summary>
        public const int LiangDaoPeiFu = 12;

        /// <summary>杠上胡 杠上开 和杠上花都用这个</summary>
        public const int GangShangHu = 13;

        public static string GetHuPaiTypesDesc(RepeatedField<int> typs,bool isZimo=false)
        {
            string desc = string.Empty;
            for (int i = 0; i < typs.Count; i++)
            {
                switch (typs[i])
                {
                    case CardFiveStarHuPaiType.PengPengHu:
                        desc += "碰碰胡  ";
                        break;
                    case CardFiveStarHuPaiType.CardFiveStar:
                        desc += "卡五星  ";
                        break;
                    case CardFiveStarHuPaiType.MingSiGui:
                        desc += "明四归  ";
                        break;
                    case CardFiveStarHuPaiType.AnSiGui:
                        desc += "暗四归  ";
                        break;
                    case CardFiveStarHuPaiType.QingYiSe:
                        desc += "清一色  ";
                        break;
                    case CardFiveStarHuPaiType.QiDui:
                        desc += "七对  ";
                        break;
                    case CardFiveStarHuPaiType.MengQiDui:
                        desc += "龙七对  ";
                        break;
                    case CardFiveStarHuPaiType.XiaoSanYuan:
                        desc += "小三元  ";
                        break;
                    case CardFiveStarHuPaiType.DaSanYuan:
                        desc += "大三元  ";
                        break;
                    case CardFiveStarHuPaiType.PingHu:
                        desc += "平胡  ";
                        break;
                    case CardFiveStarHuPaiType.ChaJiaoPeiFu:
                        desc += "查叫赔付  ";
                        break;
                    case CardFiveStarHuPaiType.LiangDaoPeiFu:
                        desc += "亮倒赔付  ";
                        break;
                    case CardFiveStarHuPaiType.GangShangHu:
                        if (isZimo)
                        {
                            desc += "杠上花  ";
                        }
                        else
                        {
                            desc += "杠上开  ";
                        }
                        break;
                }
            }

            return desc;
        }

    }

    public static partial class CardFiveStarHuPaiLogic
    {
        private static readonly Dictionary<int,int> _HuPaiTypeGetSocreInfo=new Dictionary<int, int>();
        static CardFiveStarHuPaiLogic()
        {
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.PengPengHu, 2);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.CardFiveStar, 2);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.MingSiGui, 2);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.AnSiGui, 4);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.QingYiSe, 4);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.QiDui, 4);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.MengQiDui, 8);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.XiaoSanYuan, 4);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.DaSanYuan, 8);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.PingHu, 1);
            _HuPaiTypeGetSocreInfo.Add(CardFiveStarHuPaiType.GangShangHu, 2);
        }

        //根据胡的牌和吃碰杠的牌 得到底分总倍数 winCard赢的牌 也包含在cards里面 所以cards%3==2
        public static int GetMultiple(IList<int> cards, IList<int> pengGangs, int winCard,bool isGangHu=false,int maxMultiple=0)
        {
            RepeatedField<int> huTypes = GetHuPaiTypes(cards, pengGangs, winCard, isGangHu);
            return GetMultiple(huTypes);
        }

        //根据胡牌类型 得到底分总倍数
        public static int GetMultiple(IList<int> huPaiTypes, int maxMultiple = 0)
        {
            int multile = 1;
            foreach (var huPaiType in huPaiTypes)
            {
                multile*=_HuPaiTypeGetSocreInfo[huPaiType];
            }
            if (maxMultiple > 0)
            {
                if (multile > maxMultiple)
                {
                    multile = maxMultiple;
                }
            }
            return multile;
        }
        //得到胡牌类型
        private static readonly Dictionary<int, int> _HuPaiScorePaiInNum = new Dictionary<int, int>();

        //根据牌的类型得到胡牌类型
        public static RepeatedField<int> GetHuPaiTypes(IList<int> cards, IList<int> pengGangs, int winCard, bool isGangHu = false) 
        {
            RepeatedField<int> huPaiTypes = new RepeatedField<int>();
            GetPaiInNum(cards, _HuPaiScorePaiInNum);
            //判断碰碰胡
            if (IsPengPengHu(_HuPaiScorePaiInNum))
            {
                huPaiTypes.Add(CardFiveStarHuPaiType.PengPengHu);
            }
            //判断卡五星
            if (IsCardFiveStar(cards, winCard))
            {
                huPaiTypes.Add(CardFiveStarHuPaiType.CardFiveStar);
            }
            //判断明四归
            if (IsMingSiGui(cards, pengGangs))
            {
                huPaiTypes.Add(CardFiveStarHuPaiType.MingSiGui);
            }
            //判断暗四归
            if (IsAnSiGui(_HuPaiScorePaiInNum))
            {
                huPaiTypes.Add(CardFiveStarHuPaiType.AnSiGui);
            }
            //判断清一色
            if (IsQingYiSe(cards, _HuPaiScorePaiInNum, pengGangs))
            {
                huPaiTypes.Add(CardFiveStarHuPaiType.QingYiSe);
            }
            //判断七对和猛七对
            int QiDuiType = IsQiDuiAndMeng(cards, _HuPaiScorePaiInNum);
            if (QiDuiType != 0)
            {
                huPaiTypes.Add(QiDuiType);
            }
            //判断大三元和小三元
            int SanYuanType = IsSanYuan(_HuPaiScorePaiInNum, pengGangs);
            if (SanYuanType != 0)
            {
                huPaiTypes.Add(SanYuanType);
            }
            //判断是不是平胡
            if (huPaiTypes.Count <= 0)
            {
                huPaiTypes.Add(CardFiveStarHuPaiType.PingHu);
            }
            //是不是杠上胡
            if (isGangHu)
            {
                huPaiTypes.Add(CardFiveStarHuPaiType.GangShangHu);
            }
            return huPaiTypes;
        }

        //判断是不是碰碰胡
        public static bool IsPengPengHu(Dictionary<int, int> paiInNum)
        {
            bool isHaveJiang = false;
            foreach (var pai in paiInNum)
            {
                if (pai.Value==2)
                {
                    if (isHaveJiang)//碰碰胡 只有一种牌 相同数为2 两个就不是碰碰胡
                    {
                        return false;
                    }
                    isHaveJiang = true;
                }
                if (pai.Value != 2 && pai.Value != 3)
                {
                    return false;
                }
            }
            return true;
        }

        //判断是不是卡五星
        public static bool IsCardFiveStar<T>(T cards, int card) where T : IList<int>
        {
            bool isCardFiveStar = false;
            if (card % 9 == 5)
            {
                if (cards.Contains(card + 1) && cards.Contains(card - 1))
                {
                    cards.Remove(card);
                    cards.Remove(card - 1);
                    cards.Remove(card + 1);
                    if (IsHuPai(cards))
                    {
                        isCardFiveStar = true;
                    }
                    cards.Add(card);
                    cards.Add(card - 1);
                    cards.Add(card + 1);
                }
            }
            return isCardFiveStar;
        }

        //判断是不是明四归
        public static bool IsMingSiGui(IList<int> cards, IList<int> pengGangs) 
        {
            foreach (var pengGangcard in pengGangs)
            {
                if (cards.Contains(pengGangcard))
                {
                    return true;
                }
            }
            return false;
        }

        //判断是不是暗四归
        public static bool IsAnSiGui(Dictionary<int, int> paiInNum)
        {
            foreach (var pai in paiInNum)
            {
                if (pai.Value >= 4)
                {
                    return true;
                }
            }
            return false;
        }

        //判断是不是清一色
        public static bool IsQingYiSe(IList<int> cards, Dictionary<int, int> paiInNum, IList<int> pengGangs) 
        {
            int finallyCard = cards[cards.Count - 1];
            if (finallyCard < TongStartIndex)
            {
                finallyCard = TongStartIndex - 1;
            }
            else if (finallyCard < OtherStartIndex)
            {
                finallyCard = OtherStartIndex - 1;
            }
            else
            {
                return false;
            }
            foreach (var pai in paiInNum)
            {
                if (pai.Key > finallyCard || pai.Key <= finallyCard - 9)
                {
                    return false;
                }
            }
            foreach (var pai in pengGangs)
            {
                if (pai > finallyCard || pai <= finallyCard - 9)
                {
                    return false;
                }
            }
            return true;
        }

        //判断是不是七对或者猛七对
        public static int IsQiDuiAndMeng(IList<int> cards, Dictionary<int, int> paiInNum) 
        {
            if (cards.Count != 14)
            {
                return 0;
            }
            bool isMengQidui = false;
            foreach (var pai in paiInNum)
            {
                if (pai.Value != 2 || pai.Value != 4)
                {
                    return 0;
                }
                if (pai.Value == 4)
                {
                    isMengQidui = true;
                }
            }
            if (isMengQidui)
            {
                return CardFiveStarHuPaiType.MengQiDui;
            }
            else
            {
                return CardFiveStarHuPaiType.QiDui;
            }
        }

        //判断是不是三元
        public static int IsSanYuan(Dictionary<int, int> paiInNum, IList<int> pengGangs)
        {
            if ((paiInNum.ContainsKey(OtherStartIndex)|| pengGangs.Contains(OtherStartIndex)) 
                && (paiInNum.ContainsKey(OtherStartIndex + 1) || pengGangs.Contains(OtherStartIndex+1))
                && (paiInNum.ContainsKey(OtherStartIndex + 2) || pengGangs.Contains(OtherStartIndex +2)))
            {
                if ((paiInNum[OtherStartIndex] >= KeZiNum || pengGangs.Contains(OtherStartIndex))
                    && (paiInNum[OtherStartIndex+1] >= KeZiNum || pengGangs.Contains(OtherStartIndex+1))
                    && (paiInNum[OtherStartIndex+2] >= KeZiNum || pengGangs.Contains(OtherStartIndex+2)))
                {
                    return CardFiveStarHuPaiType.DaSanYuan;
                }
                else
                {
                    return CardFiveStarHuPaiType.XiaoSanYuan;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
