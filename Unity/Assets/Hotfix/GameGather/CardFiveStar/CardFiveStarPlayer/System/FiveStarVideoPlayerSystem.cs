using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace ETHotfix
{
    public partial class CardFiveStarPlayer
    {
        public RepeatedField<int> _VideoHands;//录像玩家手牌
        //录像发牌
        public virtual IEnumerator Video_Deal(RepeatedField<int> hands)
        {
            _VideoHands =new RepeatedField<int>(){ hands }; 
            for (int i = 0; i < hands.Count; i++)
            {
                AddLiangCards(hands[i]);
                CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.MoCard);//摸牌 音效
                yield return new WaitForSeconds(0.1f);
            }
            Video_RefreshHand();//显示亮的牌
        }

        public int _VideoMoCardSize = 0;
        //录像摸牌
        public virtual void Video_MoCard(int card)
        {
            ShowWinCard(card);//显示普通赢的牌
            _VideoHands.Add(card);
            _VideoMoCardSize = card;//记录摸的那张牌 出牌的时候要
            CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.MoCard);//摸牌 音效
            TurntableTimeMgr.Ins.ShowLightAndResetTime(ClientSeatIndex,FiveStarOverTime.CanPlayCardOverTime);//亮起可出牌操作
        }

        //录像出牌
        public virtual void Video_ChuCard(int card)
        {
            HideWinCard();
            _VideoHands.Remove(card);
            float animCardSize = 1f;
            if (ClientSeatIndex == 0)
            {
                animCardSize = 0.5f;
            }
            if (card == _VideoMoCardSize)
            {
                CoroutineMgr.StartCoroutinee(ChuCardAnimSound(card, _WinCard.gameObject.transform.position, animCardSize));
            }
            else
            {
                for (int i = 0; i < _liangCards.Count; i++)
                {
                    if (_liangCards[i].CardSize == card)
                    {
                        CoroutineMgr.StartCoroutinee(ChuCardAnimSound(card, _liangCards[i].gameObject.transform.position, animCardSize));
                        break;
                    }
                }
            }
            Video_RefreshHand();
        }
        //录像刷新手牌
        public void Video_RefreshHand()
        {
            _VideoHands.Sort();
            if (_VideoHands.Count%3==2)
            {
                int winCard = _VideoHands[_VideoHands.Count - 1];
                _VideoHands.Remove(winCard);
                ShowWinCard(winCard);//如果是在 碰杠之后 会进这 挪一张牌到赢牌位置
                ShowLiangCards(_VideoHands);//刷新手牌
                _VideoHands.Add(winCard);
                return;
            }
            ShowLiangCards(_VideoHands);//刷新手牌
        }
        //录像碰 杠 胡 牌
        public virtual void Video_PengGangHu(int operateType, int card,int chuCardServerIndex)
        {
            if (operateType == FiveStarOperateType.None)
            {
                return;
            }
            //显示碰杠的牌和特效
            ShowPengGangCardSpecial(operateType, card, chuCardServerIndex);
            //根据操作类型减去手牌
            if (operateType == FiveStarOperateType.CaGang)
            {
                Video_MinusHands(card,1);
            }
            else if (operateType == FiveStarOperateType.Peng)
            {
                Video_MinusHands(card, 2);
            }
            else if (operateType == FiveStarOperateType.MingGang)
            {
                Video_MinusHands(card, 3);
            }
            else if (operateType == FiveStarOperateType.AnGang)
            {
                Video_MinusHands(card, 4);
            }
            //碰和明杠要去掉 最后出牌玩家落在地上的牌
            if (operateType == FiveStarOperateType.Peng || operateType == FiveStarOperateType.MingGang)
            {
                FiveStarVideoRoom.Ins.RemoveEndChuCard();
            }
            HideWinCard();
            Video_RefreshHand();//刷新手牌
        }

        //减去相同手牌数量
        public void Video_MinusHands(int card,int count)
        {
            for (int i = 0; i < count; i++)
            {
                _VideoHands.Remove(card);
            }
        }

        //亮倒
        public void Video_LiangDao()
        {
            IsLiangDao = true;
            OperationSpecialMgr.Ins.ShowSpecial(FiveStarOperateType.Liang, _OperationSpecialPoint);
            List<int> tingCards = CardFiveStarHuPaiLogic.IsTingPai(_VideoHands);
            //显示亮牌图标
            UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().Video_ShowMIngCardHint(ClientSeatIndex, tingCards);
        }
    }
}
