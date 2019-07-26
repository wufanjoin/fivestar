using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    //正常打牌的事件
    public partial class CardFiveStarPlayer
    {
        private const int DealCardCount=13;//每次固定发牌数量
        //发牌
        public virtual async void Deal(params object[] objs)
        {
            if ((!gameObject.activeInHierarchy)||IsRestIn)
            {
                return;
            }
            ClearHand();
            CardFiveStarRoom.Ins.ReduceCardTotalNum(DealCardCount);//减少牌的总数
            for (int i = 0; i < DealCardCount; i++)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                _hands.Add(CardFiveStarCardPool.Ins.Create(HandType, _HandGroup.transform));
            }
        }
        //摸牌
        public virtual void MoCard(int card)
        {
            CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.MoCard);//摸牌 音效
            CardFiveStarRoom.Ins.ReduceCardTotalNum(1);//减少牌的总数
            ShowMoCard(card);//显示摸的牌
        }
        //出牌
        public virtual void ChuCard(int card)
        {
            CardFiveStarRoom.Ins.ReduceCardInNum(card);//减少牌对应的数量
            CoroutineMgr.StartCoroutinee(ChuCardAnimSound(card, _NewHandPointGo.transform.position));//出牌动画 和音效
            HideMoCard();
        }
        //出牌动画和音效
       public IEnumerator ChuCardAnimSound(int card, Vector3 animCardStatPoint, float scaleSize = 1f)
        {
            CardFiveStarSoundMgr.Ins.PlaySound(_user.Sex, FiveStarOperateType.ChuCard, card);//出牌 喊话 音效
            CardFiveStarCard newChuCard = AddChuCard(card);
            CardFiveStarCard animCard = CardFiveStarCardPool.Ins.Create(ChuCardType, card, _NewHandPointGo.transform);
            animCard.gameObject.transform.position = animCardStatPoint;
            newChuCard.SetImageActive(false);
            yield return new WaitForFixedUpdate();
            animCard.gameObject.transform.DOMove(newChuCard.gameObject.transform.position, 0.4f);
            animCard.gameObject.transform.DOScale(VectorHelper.GetSameVector3(scaleSize), 0.4f);
            yield return new WaitForSeconds(0.4f);
            newChuCard.SetImageActive(true);
            ChuCardJianTouMgr.Ins.Show(newChuCard.gameObject, ChuCardJianTouYAxleOffset);
            animCard.Destroy();
            CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.ChuCardFall);//出牌落地 音效
        }
        //碰杠胡
        public virtual void PengGangHu(int operateType, int cardSize, int playCardSeatIndex)
        {
            if (operateType == FiveStarOperateType.CaGang || operateType == FiveStarOperateType.AnGang || operateType == FiveStarOperateType.MingGang)
            {
                _GangPaiCount++;//杠牌次数加1
            }
            //显示碰杠的牌和特效 和音效
            ShowPengGangCardSpecial(operateType, cardSize, playCardSeatIndex);
            //碰 明杠 暗杠 之后手牌数量减3 还有牌数量的变化
            PengGangHandAndCardNumChang(operateType, cardSize);
            //判断 是否 是否处于亮下的杠牌 如果是就变化手牌
            JudgeLiangDaoGangHandChange(operateType, cardSize);
            //碰和明杠要去掉 最后出牌玩家落在地上的牌
            if (operateType == FiveStarOperateType.Peng || operateType == FiveStarOperateType.MingGang)
            {
                CardFiveStarRoom.Ins.RemoveEndChuCard();
            }
        }
        //显示碰杠的牌 和操作特效 和音效
        public void ShowPengGangCardSpecial(int operateType, int cardSize, int playCardServerSeatIndex)
        {
            OperationSpecialMgr.Ins.ShowSpecial(operateType, _OperationSpecialPoint);//碰杠胡 特效
            CardFiveStarSoundMgr.Ins.PlaySound(_user.Sex, operateType);//碰杠胡 音效
            //擦杠把之前碰的UI变成擦杠
            if (operateType == FiveStarOperateType.CaGang)
            {
                for (int i = 0; i < _PengGangs.Count; i++)
                {
                    if (_PengGangs[i]._CardSize == cardSize)
                    {
                        _PengGangs[i].SetUI(FiveStarOperateType.CaGang, cardSize);
                    }
                }
            }
            //碰 暗杠 明杠 要加上去
            else if (operateType == FiveStarOperateType.Peng || operateType == FiveStarOperateType.AnGang || operateType == FiveStarOperateType.MingGang)
            {
                AddPengGangServerIndex(operateType, cardSize, playCardServerSeatIndex);
            }
        }
        //添加碰杠信息 传递服务器索引就行
        public void AddPengGangServerIndex(int operateType, int cardSize, int playCardseatIndex)
        {
            int clientSeatIndex = 0;
            if (playCardseatIndex >= 0)
            {
                if (CardFiveStarRoom.Ins != null)
                {
                    clientSeatIndex = CardFiveStarRoom.Ins.GetClientSeatIndex(playCardseatIndex);
                }
                else
                {
                    clientSeatIndex=FiveStarVideoRoom.Ins.GetClientSeatIndex(playCardseatIndex);
                }
            }
            AddPengGang(operateType, cardSize, clientSeatIndex);
        }
        //判断 亮倒 情况下 杠之后 玩家手牌变化
        public void JudgeLiangDaoGangHandChange(int operateType,int cardSize)
        {
            if (IsLiangDao)
            {
                if (operateType == FiveStarOperateType.AnGang || operateType == FiveStarOperateType.MingGang)
                {
                    LiangDaoGangHandChange(cardSize);
                }
            }
        }
        //亮倒 情况下 杠之后 玩家手牌变化
        public virtual void LiangDaoGangHandChange(int cardSize)
        {
            for (int i = 0; i < 3; i++)
            {
                _liangCards[0].Destroy();
                _liangCards.RemoveAt(0);//其他玩家 不关胡的牌 都放在数组的前面
            }
        }
        //碰杠要减少对应牌数
        public void PengGangReduceCardInNumChang(int operateType, int cardSize)
        {
            if (operateType == FiveStarOperateType.MingGang)
            {
                CardFiveStarRoom.Ins.ReduceCardInNum(new List<int>() { cardSize, cardSize, cardSize });//减少牌对应的数量
            }
            else if (operateType == FiveStarOperateType.CaGang)
            {
                CardFiveStarRoom.Ins.ReduceCardInNum(new List<int>() { cardSize });//减少牌对应的数量
            }
            else if (operateType == FiveStarOperateType.Peng)
            {
                CardFiveStarRoom.Ins.ReduceCardInNum(new List<int>() { cardSize, cardSize });//减少牌对应的数量
            }
        }
        //碰杠胡后手牌变化
        public virtual void PengGangHandAndCardNumChang(int operateType, int cardSize)
        {
            
            PengGangReduceCardInNumChang(operateType, cardSize);
            if (_hands.Count < 4)
            {
                Log.Debug($"玩家{ClientSeatIndex}手牌小于4 进行碰杠胡的操作{operateType}");
                return;
            }
            if (operateType == FiveStarOperateType.Peng)
            {
                ShowMoCard(0);//其他玩家 碰牌之后要显示摸的牌
            }
            if (operateType == FiveStarOperateType.MingGang || operateType == FiveStarOperateType.AnGang
                || operateType == FiveStarOperateType.Peng)
            {
                for (int i = 0; i < 3; i++)
                {
                    _hands[0].Destroy();
                    _hands.RemoveAt(0);
                }
            }
        }

        //亮倒
        public  void LiangDao(RepeatedField<int> hands,bool isShowSpecial=true)
        {
            IsLiangDao = true;
            if (isShowSpecial)
            {
                OperationSpecialMgr.Ins.ShowSpecial(FiveStarOperateType.Liang, _OperationSpecialPoint);//亮倒 特效
                CardFiveStarSoundMgr.Ins.PlaySound(_user.Sex, FiveStarOperateType.Liang);//亮倒 音效
            }
            List<int> tingCards = CardFiveStarHuPaiLogic.IsTingPai(hands);
            UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().ShowMingCardHint(ClientSeatIndex, tingCards);
            ClearHand();
            ShowLiangDaoCards(hands);
            CardFiveStarRoom.Ins.AddLiangDaoCanHuCards(ClientSeatIndex,tingCards);
            if (ClientSeatIndex != 0)
            {
                CardFiveStarRoom.Ins.ReduceCardInNum(hands); //删除牌的剩余数量
            }
            EventMsgMgr.SendEvent(CardFiveStarEventID.PlayerLiangDao);
        }

        //显示亮倒的牌
        public virtual void ShowLiangDaoCards(RepeatedField<int> cards)
        {
            RepeatedField<RepeatedField<int>>  liangDaoCards=CardFiveStarHuPaiLogic.GetLiangDaoNoneHuCards(cards);
            ClearLiangCards();
            AddLiangCards(AnGangCardType, liangDaoCards[1]);
            liangDaoCards[0].Sort();
            AddLiangCards(LaingCardType, liangDaoCards[0]);
        }

        //打漂
        public virtual void DaPiao(int piaoNum)
        {
            _PiaoSocreNum = piaoNum;
            _PlayerHead.ShowPiaoNum(_PiaoSocreNum);
        }

        //玩家可以操作
        public virtual void CanOperate(RepeatedField<int> oprateLits, RepeatedField<int> gangLits)
        {

            if (ClientSeatIndex==0&&oprateLits.Count > 0)
            {
                TurntableTimeMgr.Ins.ShowLightAndResetTime(ClientSeatIndex, FiveStarOverTime.CanOperateOverTime);
                UIComponent.GetUiView<CanOpertionPanelComponent>().ShowCanOpration(oprateLits, gangLits);
            }
            else
            {
                
                TurntableTimeMgr.Ins.ResetTime(FiveStarOverTime.CanOperateOverTime);
            }

        }

        public const int _canChuCardTime = 12;//可出牌剩余时间
        //玩家可以出牌
        public virtual void CanChuCard()
        {
            TurntableTimeMgr.Ins.ShowLightAndResetTime(ClientSeatIndex, FiveStarOverTime.CanPlayCardOverTime);
        }

        //玩家轮休
        public void Rest()
        {
            IsRestIn = true;
            _RestHintGo.SetActive(true);
        }
        //玩家分数变化
        public virtual void ScoreChang(int nowScore, int getScore=0, bool isShowGetScore = false)
        {
            _NowSocre = nowScore;
            _PlayerHead.ScoreChang(getScore, isShowGetScore);
        }
        //玩家准备
        public virtual void Ready()
        {
            SetReadyState(true);
        }

        //小结算
        public virtual void SmallResult(FiveStar_SmallPlayerResult playerResult)
        {
            ScoreChang(playerResult.NowScore);
            ClearHand();//清除手牌
            HideMoCard();//隐藏摸的牌
          
            switch (playerResult.PlayerResultType)
            {
                case FiveStarPlayerResultType.FangChong:
                    _FangChongCount++;//放冲次数
                    break;
                case FiveStarPlayerResultType.HuFangChong:
                    _HuPaiCount++;//胡牌次数
                    break;
                case FiveStarPlayerResultType.ZiMoHu:
                    _HuPaiCount++;//胡牌次数
                    _ZiMoCount++;//自摸次数
                    break;
                case FiveStarPlayerResultType.Normal:
                    break;
            }

            ShowLiangCards(playerResult.Hands);//显示亮的牌 
            if (playerResult.PlayerResultType==FiveStarPlayerResultType.HuFangChong|| playerResult.PlayerResultType == FiveStarPlayerResultType.ZiMoHu)
            {
                ShowWinCard(playerResult.WinCard);//显示赢的牌
            }
        }

    }
}
