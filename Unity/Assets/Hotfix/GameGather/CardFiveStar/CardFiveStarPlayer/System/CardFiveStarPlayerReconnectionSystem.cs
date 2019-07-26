using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ETHotfix
{
    //重连回复数据
    public partial class CardFiveStarPlayer
    {
        public void Reconnection(FiveStarPlayer fiveStarPlayer,int roomStateType,bool isDaPiaoBeing)
        {
            if (RoomStateType.GameIn == roomStateType)
            {
                if (isDaPiaoBeing)
                {
                    //更加自己打漂否 显示 等待打漂 还是选择打漂
                    if (fiveStarPlayer.IsAlreadyDaPiao)
                    { 
                        DaPiao(fiveStarPlayer.PiaoNum);
                    }
                    if (ClientSeatIndex == 0)
                    {
                        if (fiveStarPlayer.IsAlreadyDaPiao)
                        {
                            UIComponent.GetUiView<SelectPiaoNumPanelComponent>().ShowAwaitHint();//显示等待打漂
                        }
                        else
                        {
                            UIComponent.GetUiView<SelectPiaoNumPanelComponent>().ShowCanSelectPiaoNum(CardFiveStarRoom.Ins._config.MaxPiaoNum);//显示选择漂数
                        }
                    }
                }
                else
                {
                    ReduceCardInNum(fiveStarPlayer);//减少牌对应的数量 
                    //显示出的牌
                    for (int i = 0; i < fiveStarPlayer.PlayCards.Count; i++)
                    {
                        AddChuCard(fiveStarPlayer.PlayCards[i]);
                    }
                    //显示碰杠牌
                    for (int i = 0; i < fiveStarPlayer.OperateInfos.Count; i++)
                    {
                        AddPengGangServerIndex(fiveStarPlayer.OperateInfos[i].OperateType, fiveStarPlayer.OperateInfos[i].Card, fiveStarPlayer.OperateInfos[i].PlayCardIndex);
                    }
                    //显示亮倒的牌
                    if (fiveStarPlayer.IsLiangDao)
                    {
                        if (ClientSeatIndex == 0)
                        {
                            CardFiveStarHandComponent.Ins.RefreshHand(fiveStarPlayer.Hands);//刷新一下手牌
                        }
                        LiangDao(fiveStarPlayer.Hands,false);//显示亮倒的牌 和正常流程一样
                    }
                    else
                    {
                        ShowHands(fiveStarPlayer.Hands);//没有亮倒显示正常手牌
                    }
                    //根据是否漂分 显示打漂信息
                    if (CardFiveStarRoom.Ins._config.MaxPiaoNum>0)
                    {
                        DaPiao(fiveStarPlayer.PiaoNum);//显示漂数
                    }
                    UIComponent.GetUiView<SelectPiaoNumPanelComponent>().Hide();//隐藏打漂界面
                }
            }
            else if (RoomStateType.ReadyIn == roomStateType)
            {
               SetReadyState(fiveStarPlayer.ReadyState);
            }
            //显示现在的得分
            ScoreChang(fiveStarPlayer.NowScore);
            //赋值 自摸次数 放冲次数 杠牌次数 胡牌次数
            _ZiMoCount = fiveStarPlayer.ZiMoCount;
            _FangChongCount = fiveStarPlayer.FangChongCount;
            _GangPaiCount = fiveStarPlayer.GangPaiCount;
            _HuPaiCount = fiveStarPlayer.HuPaiCount;
        }

        //减少牌对应的数量
        public void ReduceCardInNum(FiveStarPlayer fiveStarPlayer)
        {
            CardFiveStarRoom.Ins.ReduceCardInNum(fiveStarPlayer.Hands);
            CardFiveStarRoom.Ins.ReduceCardInNum(fiveStarPlayer.PlayCards);
            for (int i = 0; i < fiveStarPlayer.OperateInfos.Count; i++)
            {
                PengGangReduceCardInNumChang(fiveStarPlayer.OperateInfos[i].OperateType,
                    fiveStarPlayer.OperateInfos[i].Card);
            }
        }
    }
}
