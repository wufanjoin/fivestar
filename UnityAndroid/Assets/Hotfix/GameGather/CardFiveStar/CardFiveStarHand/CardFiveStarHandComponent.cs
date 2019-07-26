using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class CardFiveStarHandComponentComponent : AwakeSystem<CardFiveStarHandComponent,Transform,Transform>
    {
        public override void Awake(CardFiveStarHandComponent self, Transform handParent, Transform newHandParent)
        {
            CardFiveStarHandComponent.Ins = self;
            CardFiveStarHandComponent.Ins.RegisterEvent();
            CardFiveStarHandComponent.Ins._HandPatent = handParent;
            CardFiveStarHandComponent.Ins._NewHand = CardFiveStarCardFactory.CreateNewHand(6, newHandParent);
            CardFiveStarHandComponent.Ins._NewHand.SetActive(false);
            CardFiveStarHandComponent.Ins._NewChuCardAnim = CardFiveStarCardFactory.CreateNewHand(6, newHandParent);
            CardFiveStarHandComponent.Ins._NewChuCardAnim.SetActive(false);
        }
    }

    public partial class CardFiveStarHandComponent : Component
    {
        public static CardFiveStarHandComponent Ins;
        public List<CardFiveStarHand> _HandLists = new List<CardFiveStarHand>();//手牌数组
        public CardFiveStarNewHand _NewHand;
        public CardFiveStarNewHand _NewChuCardAnim;
        public Transform _HandPatent;
        public void RegisterEvent()
        {
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.PlayerSelectLiangCardState, PlayerSelectLiangCardStateEvent);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.ZhiSeZi, ZhiSeZiEvent);
        }
        //开始置塞子动画了
        public void ZhiSeZiEvent(params object[] objs)
        {
            _IsCanChuCard = false;//牌局开始 肯定是 不可以出牌的状态
            _IsDealCardIn = true;//表示动画中了
        }
        private bool _isSelectLiang = false;
        //玩家选择 是否亮
        public void PlayerSelectLiangCardStateEvent(params object[] objs)
        {
            _isSelectLiang = (bool)objs[0];
        }

       



        public bool _IsDealCardIn=false;//是否在发牌中 置塞子也算
        //发牌动画
        public async void Deal(RepeatedField<int> hands)
        {
            _IsDealCardIn = true;
            ClearHands();
            for (int i = 0; i < hands.Count; i++)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                _HandLists.Add(CardFiveStarCardPool.Ins.CreateHand(hands[i], i, _HandPatent));
                CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.MoCard);//摸牌 音效
            }
            RefreshHand(hands);//发完牌 直接刷新手牌
            _IsDealCardIn = false;
            RepairDealMoCard();//发完牌检测一下 是否修补摸牌消息
            if (_IsCanChuCard)
            {
                CanChuCard();//发完牌检测一下 是否修可以出牌的消息
            }
        }


        //直接刷新手牌 和胡牌提示 和一些条件判断
        public void RefreshHand(RepeatedField<int> hands)
        {
            hands.Sort();
            _NewestHands = hands;//不管刷新 UI 数据是要记录的
            if (_PlayCardAnimIn)
            {
                return;//正在播放打牌动画 不刷新手牌
            }
            //每次刷新牌 都要检测一下能不能胡牌 胡牌提示  亮倒了 也是要刷新胡牌提示的
            DetectionIsTingCard(_NewestHands);
            if (CardFiveStarRoom.Ins.GetUserPlayerInfo().IsLiangDao)
            {
                return;//亮倒状态下 不用刷新手牌UI
            }
            RefreshHandUI(_NewestHands);//刷新手牌UI
            if (_IsCanChuCard)//如果当前可以出牌 刷新一下出牌的胡牌提示
            {
                DetectionChuCardHintAndLiang();//检测出牌的胡牌提示和 是否可以亮倒
            }
        }
        //刷新手牌UI 只管手牌UI
        public void RefreshHandUI(RepeatedField<int> hands)
        {
            //先直接清除所有手牌
            ClearHands();
            _HandPatent.GetComponent<HorizontalLayoutGroup>().enabled = true;//刷新牌之前启用网格
            int moreCard = -1;
            if (hands.Count % 3 == 2)//如果是 碰杠之后刷新 手牌 会多一张 移到新手牌那
            {
                if (_IsCanChuCard&&hands.Contains(_NewHand.CardSize))//若果当前 是自己出牌 尽量摸牌 位置放原有牌
                {
                    moreCard = _NewHand.CardSize;
                }
                else
                {
                    moreCard = hands[hands.Count - 1];
                }
                hands.Remove(moreCard);
            }
            //更新手牌UI 刷新牌
            for (int i = 0; i < hands.Count; i++)
            {
                _HandLists.Add(CardFiveStarCardPool.Ins.CreateHand(hands[i], i, _HandPatent));
            }
            _NewHand.SetActive(false);
            if (moreCard > 0)//如果多的牌 就放在 摸的牌的显示
            {
                _NewHand.SetCardUI(moreCard);
                hands.Add(moreCard);
            }
        }
        //检测是否能胡牌并显示胡牌提示
        public void DetectionIsTingCard(IList<int> hands)
        {
            List<int> tingCards = CardFiveStarHuPaiLogic.IsTingPai(hands);
            if (tingCards.Count > 0)
            {
                UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().ShowSelfTingHuCardHint(tingCards);
            }
        }

        //发完牌检测一下 没有之前记录的膜牌消息
        private void RepairDealMoCard()
        {
            if (_DealMoCardSize > 0)
            {
                MoCard(_DealMoCardSize);
                _DealMoCardSize = -1;
            }
        }
        private int _DealMoCardSize = -1;
        //摸牌
        public void MoCard(int card)
        {
            //如果在发牌中 不能执行 摸牌 并记录 如果手牌为空就表示 还没发牌
            if (_IsDealCardIn|| _NewestHands==null)
            {
                _DealMoCardSize = card;
                return;
            }
            _NewestHands.Add(card);
            _NewHand.SetCardUI(card);
        }
        //清除所有手牌
        public void ClearHands()
        {
            CancelPartyHand();//清除所有手牌 也要取消预选牌
            foreach (var hand in _HandLists)
            {
                hand.Destroy();
            }
            _HandLists.Clear();
        }
        //取消预选牌
        public void CancelPartyHand()
        {
            //牌落下
            if (_partyChuHand != null)
            {
                _partyChuHand.SetPitchStatu(false);//如果有选中的牌 就让牌落下
                _partyChuHand = null;
            }
        }
   
        //是否可以出牌
        public bool _IsCanChuCard = false;

        //收到可以出牌的消息
        public async void CanChuCard()
        {
            CancelPartyHand();//取消预选牌
            _IsCanChuCard = true;
            //判断有没有完成发牌
            if (_IsDealCardIn|| _NewestHands==null)
            {
                return;
            }
            DetectionChuCardHintAndLiang();//检测出牌的胡牌提示和 是否可以亮倒
            //如果已经亮牌了 等待一秒 自动出牌
            if (CardFiveStarRoom.Ins.GetUserPlayerInfo().IsLiangDao)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(500);
                 RequestPlayCard(_NewHand);//自动出 刚摸的牌
            }
        }
        //胡的牌 和对应的倍数 每次重新检测都要清除
        private Dictionary<int, Dictionary<int,int>> _HuCardInMultiple=new Dictionary<int, Dictionary<int, int>>();
        //获取打这张牌的胡牌倍数
        public int GetCardInMultiple(int winCard, int chuCard=-1)
        {
            if (_HuCardInMultiple.ContainsKey(chuCard))
            {
                if (_HuCardInMultiple[chuCard].ContainsKey(winCard))
                {
                    return _HuCardInMultiple[chuCard][winCard];
                }
            }
            RepeatedField<int> copyHand=new RepeatedField<int>(){ _NewestHands };
            if (chuCard > 0)
            {
                copyHand.Remove(chuCard);
            }
            copyHand.Add(winCard);

           int  multiple=CardFiveStarHuPaiLogic.GetMultiple(copyHand, CardFiveStarRoom.Ins.GetUserPlayerInfo()._PengGangArray,winCard);
            if (_HuCardInMultiple.ContainsKey(chuCard))
            {
                _HuCardInMultiple[chuCard].Add(winCard, multiple);
            }
            else
            {
                _HuCardInMultiple.Add(chuCard,new Dictionary<int, int>(){{ winCard , multiple } });
            }

            return multiple;
        }
        //检测出牌的胡牌提示和 是否可以亮倒
        public void DetectionChuCardHintAndLiang()
        {
            _isSelectLiang = false;
            if (CardFiveStarRoom.Ins.GetUserPlayerInfo().IsLiangDao)
            {
                return;//已经亮了 就不用检测了
            }
            _HuCardInMultiple.Clear();//清空 胡的牌 和对应的倍数
            UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().HideHuCardHint(0);//先隐藏胡牌提示
            Dictionary<int, List<int>> chuCardHuDic = CardFiveStarHuPaiLogic.HuPaiTiShi(_NewestHands);
            if (chuCardHuDic.Count > 0)
            {
                EventMsgMgr.SendEvent(CardFiveStarEventID.CanChuCardHaveHuHint, chuCardHuDic);
                UIComponent.GetUiView<CanOpertionPanelComponent>().ShowCanOpration(FiveStarOperateType.Liang);
            }
        }


        //收到出牌消息
        public async void  ChuCard(int card)
        {
            UIComponent.GetUiView<CanOpertionPanelComponent>().Hide();//隐藏可操作列表
            _IsCanChuCard = false;
            EventMsgMgr.SendEvent(CardFiveStarEventID.UserChuCard);
            UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().HideHuCardHint(0);//隐藏胡牌提示
            _NewHand.SetActive(false);
            if (UpChuCardHand==null|| UpChuCardHand.CardSize != card)//托管状态下 上个出牌的对象可能不准需要判断一下
            {
                UpChuCardHand = _NewHand;
            }
            await  PlayCardAnimation();
            CancelPartyHand();//出完牌取消预选牌
        }

        public CardFiveStarHand UpChuCardHand;//上个选择出牌的手牌
        public CardFiveStarHand _partyChuHand;//预选出牌手牌
        //选择出牌
        public void PointerDownHand(CardFiveStarHand clickHand)
        {
            CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.ClickCard);//点牌 音效
            if (_partyChuHand == clickHand)
            {
                if (_IsCanChuCard)//当前可以出牌就发送出牌请求
                {
                    RequestPlayCard(_partyChuHand);//向服务器请求出牌
                }
            }
            else
            {
                CancelPartyHand();//取消之前的预选牌

                _partyChuHand = clickHand;
                //开启检测鼠标位置出牌
                StartDetecionMousePlayCard();
                //牌弹起
                _partyChuHand.SetPitchStatu(true);
            }
        }

       
        //向服务器请求出牌
        public bool RequestPlayCard(CardFiveStarHand hand)
        {
            if (!_IsCanChuCard) //当前可以出牌就发送出牌请求
            {
                return false;//不可以出牌 就直接返回
            }
            EndDetecionMousePlayCard(); //结束鼠标位置检测 出牌
            if (!CardFiveStarRoom.Ins.GetUserPlayerInfo().IsLiangDao)//如果自己 亮倒 就只能出 摸的牌 不用检测
            {
                if (CardFiveStarRoom.Ins._LiangDaoCanHuCards.Contains(hand.CardSize))//如果 出的牌是 炮牌 要判断一下
                {
                    bool isAllPaoCard = true;
                    for (int i = 0; i < _NewestHands.Count; i++)//遍历手牌 只要有一张 不是放炮的牌 就不可出炮牌
                    {
                        if (!CardFiveStarRoom.Ins._LiangDaoCanHuCards.Contains(_NewestHands[i]))
                        {
                            isAllPaoCard = false;
                            break;
                        }
                    }
                    if (!isAllPaoCard)
                    {
                        UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("不能打放炮的牌");
                        //DOTO 加个提示等待操作的时候
                        return false;
                    }
                }
                
            }
            //发送出牌请求
            SessionComponent.Instance.Send(new Actor_FiveStar_PlayCardResult() { Card = hand.CardSize });
            UpChuCardHand = hand;
            //发送亮倒请求
            if (_isSelectLiang)
            {
                SessionComponent.Instance.Send(new Actor_FiveStar_LiangDao());
            }
            return true;
        }
  
    }
}
