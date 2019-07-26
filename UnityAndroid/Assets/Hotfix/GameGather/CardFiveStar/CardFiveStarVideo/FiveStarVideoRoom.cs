using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class FiveStarVideoRoomAwakeSystem : AwakeSystem<FiveStarVideoRoom>
    {
        public override void Awake(FiveStarVideoRoom self)
        {
            self.Awake();
        }
    }
    public class FiveStarVideoRoom:Component
    {
        public static FiveStarVideoRoom Ins { get; private set; }
        private CardFiveStarRoomPanelComponent _roomPanel;
        private int CurrPlaySpeed = 1;
        public void Awake()
        {
            Ins = this;
            _roomPanel = UIComponent.GetUiView<CardFiveStarRoomPanelComponent>();
        }

        public override void Dispose()
        {
            base.Dispose();
            Ins = null;
            _PlayerDic.Clear();
        }
        //改变播放速度
        public void MultiplierSpeed()
        {
            CurrPlaySpeed *= 2;
            if (CurrPlaySpeed > 4)
            {
                CurrPlaySpeed = 1;
            }
            Time.timeScale = CurrPlaySpeed;
            UIComponent.GetUiView<CardFiveStarVideoPanelComponent>().SetMultiplierSpeed(CurrPlaySpeed);
        }
        //暂停
        public void Pause()
        {
            Time.timeScale = 0;
        }
        //播放
        public void Play()
        {
            Time.timeScale = CurrPlaySpeed;
        }
        //重新开始
        public void AnewStart()
        {
            ClearUIAndCoroutine();
            _Coroutines[0]=CoroutineMgr.StartCoroutinee(StarPlay(_VideoData));
        }
        //退出播放
        public void OutPlay()
        {
            ClearUIAndCoroutine();
            Time.timeScale = 1;
            Game.Scene.GetComponent<ToyGameComponent>().EndGame();
        }
        //清空面板和协程还要所有玩家的牌
        public void ClearUIAndCoroutine()
        {
            for (int i = 0; i < _Coroutines.Length; i++)
            {
                if (_Coroutines[i] != null)
                {
                    CoroutineMgr.StopCoroutinee(_Coroutines[i]);
                }
            }
            //隐藏明牌提示面板 和漂分面板 还要小结算面板
            UIComponent.GetUiView<FiveStarMingPaiHintPanelComponent>().Hide();
            UIComponent.GetUiView<SelectPiaoNumPanelComponent>().Hide();
            UIComponent.GetUiView<FiveStarSmallResultPanelComponent>().Hide();
            //隐藏所有牌 和漂分
            foreach (var player in _PlayerDic)
            {
                player.Value.HideAllCard();
                player.Value._PlayerHead.HidePiaoNum();
            }
        }

        private List<object> _VideoData;
        public IEnumerator StarPlay(List<object> datas)
        {
            UIComponent.GetUiView<CardFiveStarVideoPanelComponent>().SetTotalScheduleText(datas.Count - 1);//初始化不算步骤 所有减一
            _ResideNum = 84;//初始化牌数量
            _VideoData = datas;
            for (int i = 0; i < datas.Count; i++)
            {
                UIComponent.GetUiView<CardFiveStarVideoPanelComponent>().SetCurrScheduleText(i);
                object data = datas[i];
                if (data.GetType() == typeof(Video_GameInit))
                {
                    Video_GameInit meesage = data as Video_GameInit;
                    GameInit(meesage);
                }
                else if (data.GetType() == typeof(Video_Deal))
                {
                    Video_Deal meesage = data as Video_Deal;
                    Deal(meesage);
                    yield return new WaitForSeconds(2f);
                }
                else if (data.GetType() == typeof(Video_PiaoFen))
                {
                    Video_PiaoFen meesage = data as Video_PiaoFen;
                    _Coroutines[_Coroutines.Length-1]=CoroutineMgr.StartCoroutinee(PiaoFen(meesage));
                    yield return new WaitForSeconds(2f);
                }
                else if (data.GetType() == typeof(Actor_FiveStar_PlayCardResult))
                {
                    Actor_FiveStar_PlayCardResult meesage = data as Actor_FiveStar_PlayCardResult;
                    _PlayerDic[meesage.SeatIndex].Video_ChuCard(meesage.Card);
                    CurrEndChuCardSeatIndex = meesage.SeatIndex;
                    yield return new WaitForSeconds(1f);
                }
                else if (data.GetType() == typeof(Actor_FiveStar_MoPai))
                {
                    Actor_FiveStar_MoPai meesage = data as Actor_FiveStar_MoPai;
                    _PlayerDic[meesage.SeatIndex].Video_MoCard(meesage.Card);
                    ReduceCardTotalNum(1);//显示减少一张牌的数量
                    yield return new WaitForSeconds(0.5f);
                }
                else if (data.GetType() == typeof(Actor_FiveStar_OperateResult))
                {
                    Actor_FiveStar_OperateResult meesage = data as Actor_FiveStar_OperateResult;
                    _PlayerDic[meesage.SeatIndex].Video_PengGangHu(meesage.OperateInfo.OperateType, meesage.OperateInfo.Card, meesage.OperateInfo.PlayCardIndex);
                    yield return new WaitForSeconds(1f);
                }
                else if (data.GetType() == typeof(Actor_FiveStar_LiangDao))
                {
                    Actor_FiveStar_LiangDao meesage = data as Actor_FiveStar_LiangDao;
                    _PlayerDic[meesage.SeatIndex].Video_LiangDao();
                    yield return new WaitForSeconds(1f);
                }
                else if (data.GetType() == typeof(Actor_FiveStar_MaiMa))
                {
                    Actor_FiveStar_MaiMa meesage = data as Actor_FiveStar_MaiMa;
                    MaiMa(meesage);
                    yield return new WaitForSeconds(2f);
                }
                else if (data.GetType() == typeof(Actor_FiveStar_SmallResult))
                {
                    Actor_FiveStar_SmallResult meesage = data as Actor_FiveStar_SmallResult;
                    ShowSmallResult(meesage);
                }
            }
        }
        //当前最后出牌人的索引
        public int CurrEndChuCardSeatIndex = 0;//
        //移除最后出牌的人 最后出的一张牌
        public void RemoveEndChuCard()
        {
            _PlayerDic[CurrEndChuCardSeatIndex].RemoveEndChuCard();
        }
        //通过服务器索引获取玩家信息
        public CardFiveStarPlayer GetFiveStarPlayer(int serverSeatIndex)
        {
            if (_PlayerDic.ContainsKey(serverSeatIndex))
            {
                return _PlayerDic[serverSeatIndex];
            }
            return null;
        }
        public int GetClientSeatIndex(int servetSeatIndex)
        {
           return _PlayerDic[servetSeatIndex].ClientSeatIndex;
        }
      

        public FiveStarRoomConfig _RoomConfig;
        public Dictionary<int, CardFiveStarPlayer> _PlayerDic=new Dictionary<int, CardFiveStarPlayer>();
        public int _CurrRoomOffice;
        //录像房间信息初始化
        private void GameInit(Video_GameInit gameInit)
        {
            if (_PlayerDic.Count > 0)
            {
                return;
                
            }
           
            //初始化房间信息
            _RoomConfig = FiveStarRoomConfigFactory.Create(gameInit.RoomConfigs);
            string roomInfo="房号:" + gameInit.RoomNumber + "      " + gameInit.OfficeNumber + "/" +
                          _RoomConfig.MaxJuShu + "局      " + _RoomConfig.RoomNumber + "人局";
            _CurrRoomOffice = gameInit.OfficeNumber;
            _roomPanel.SetRoomInfo(roomInfo);
            //先隐藏所有玩家头像
            EventMsgMgr.SendEvent(CardFiveStarEventID.HideAllPlayer);

            //先记录出当前位置的服务器
            int selfSelfIndex = 0;
            for (int i = 0; i < gameInit.PlayerInfos.Count; i++)
            {
                if (gameInit.PlayerInfos[i].UserId == UserComponent.Ins.pUserId)
                {
                    selfSelfIndex = i;
                    break;
                }
            }
            //创建玩家头像信息
            for (int i = 0; i < gameInit.PlayerInfos.Count; i++)
            {
                User user=new User();
                user.Name = gameInit.PlayerInfos[i].Name;
                user.Icon= gameInit.PlayerInfos[i].Icon;
                user.UserId = gameInit.PlayerInfos[i].UserId;
                user.IsOnLine = true;
                CardFiveStarPlayer player = CardFiveStarPlayerFactory.Creator(user, i, selfSelfIndex, _RoomConfig.RoomNumber, _roomPanel.mPlayerUIsGo.transform, gameInit.PlayerInfos[i].NowScore);//创建用户
                _PlayerDic[gameInit.PlayerInfos[i].SeatIndex] = player;
            }
            
        }

        //买马操作
        private async void MaiMa(Actor_FiveStar_MaiMa maiMa)
        {
           await UIComponent.GetUiView<MaiMaPanelComponent>().ShowMaiMaCard(maiMa.Card, maiMa.Score);
        }
        public Coroutine[] _Coroutines=new Coroutine[6];
         //发牌操作
        private void Deal(Video_Deal deal)
        {
            for (int i = 0; i < deal.AllHands.Count; i++)
            {
                _Coroutines[i+1]=CoroutineMgr.StartCoroutinee(_PlayerDic[i].Video_Deal(deal.AllHands[i]));
                ReduceCardTotalNum(deal.AllHands[i].Count);
            }
        }
        //漂分操作
        private IEnumerator  PiaoFen(Video_PiaoFen piaoFen)
        {
           UIComponent.GetUiView<SelectPiaoNumPanelComponent>().ShowAwaitHint();
            TurntableTimeMgr.Ins.HideLigheAndRestTime(FiveStarOverTime.CanDaPiaoOverTime);//亮起可打漂时间
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < piaoFen.PiaoFens.Count; i++)
            {
                _PlayerDic[i].DaPiao(piaoFen.PiaoFens[i]);//显示漂分
            }
            UIComponent.GetUiView<SelectPiaoNumPanelComponent>().Hide();
        }

        private int _ResideNum = 0;
        //减少牌的总数量
        public void ReduceCardTotalNum(int cout)
        {
            _ResideNum -= cout;
            UIComponent.GetUiView<CardFiveStarRoomPanelComponent>().SetResidueNum(_ResideNum);
        }
        //显示小结算
        public void ShowSmallResult(Actor_FiveStar_SmallResult result)
        {
          
            if (result.RpcId != -1)
            {
                Actor_FiveStar_SmallResultHandler.AwayWinnerWinCard(result);
                result.RpcId = -1;
            }
            UIComponent.GetUiView<FiveStarSmallResultPanelComponent>().ShowSmallResult(result);
        }
    }
}
