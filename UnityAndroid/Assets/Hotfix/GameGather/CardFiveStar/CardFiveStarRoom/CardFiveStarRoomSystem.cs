using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public partial class CardFiveStarRoom 
    {
        //玩家可以打漂 通知
        public void CanDaPiao(int maxPiaoNum)
        {
            TurntableTimeMgr.Ins.HideLigheAndRestTime(FiveStarOverTime.CanDaPiaoOverTime);
            if (GetUserPlayerInfo().IsRestIn)
            {
                UIComponent.GetUiView<SelectPiaoNumPanelComponent>().ShowAwaitHint();//如果是自己休息 就直接显示等待打漂
                return;
            }
            UIComponent.GetUiView<SelectPiaoNumPanelComponent>().ShowCanSelectPiaoNum(maxPiaoNum);//显示可以漂数
        }

        //玩家打漂
        public void PlayerDaPiao(int seatIndex, int piaoNum)
        {
            _ServerSeatIndexInPlayer[seatIndex].DaPiao(piaoNum);
        }

        //玩家可以进行 吃碰杠胡的操作
        public void PlayerCanOperate(int seatIndex, RepeatedField<int> oprateLits, RepeatedField<int> gangLits)
        {
            _ServerSeatIndexInPlayer[seatIndex].CanOperate(oprateLits, gangLits);
        }

        //玩家进行 吃碰杠胡的操作
        public void PlayerOperate(int seatIndex, FiveStarOperateInfo operateInfo)
        {
            UIComponent.GetUiView<CanOpertionPanelComponent>().Hide();//每次有操作结果 就隐藏可操作面板
            _ServerSeatIndexInPlayer[seatIndex].PengGangHu(operateInfo.OperateType, operateInfo.Card,
                operateInfo.PlayCardIndex);
        }

        //玩家亮倒
        public void PlayerLiangDao(int seatIndex, RepeatedField<int> hands)
        {
            _ServerSeatIndexInPlayer[seatIndex].LiangDao(hands);
        }

        //玩家可以出牌
        public void PlayerCanPlayCard(int seatIndex)
        {
            _ServerSeatIndexInPlayer[seatIndex].CanChuCard();
        }

        //玩家出牌
        public void PlayerPlayCard(int seatIndex, int card)
        {
            iEndChuCardSize = card;
            _EndChuCardSeatIndex = seatIndex;
            _ServerSeatIndexInPlayer[seatIndex].ChuCard(card);
        }
        //去掉最后出牌玩家的索引
        public void RemoveEndChuCard()
        {
            _ServerSeatIndexInPlayer[_EndChuCardSeatIndex].RemoveEndChuCard();
        }

        //玩家摸牌
        public void MoCard(int seatIndex, int card)
        {
            _ServerSeatIndexInPlayer[seatIndex].MoCard(card);
        }

        //玩家分数变化 只有杠牌的时候才会有
        public void ScoreChang(RepeatedField<int> getScores, RepeatedField<int> nowScores)
        {
            for (int i = 0; i < getScores.Count; i++)
            {
                _ServerSeatIndexInPlayer[i].ScoreChang(nowScores[i],getScores[i],true);
            }

        }
        //玩家准备
        public void PlayerReady(int seatIndex)
        {
            _ServerSeatIndexInPlayer[seatIndex].Ready();

        }
        //小局开始
        public void SmallStarGame(Actor_FiveStar_SmallStartGame message)
        {
            _RoomState = RoomStateType.GameIn;
            _CuurRoomOffice = message.CurrOfficNum;
            _roomPanel.CutGameInUI();
            CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.GameStart);//小局 开始音效
            SetUserPlayerAsLastSibling();
        }
        //设置自己的节点为最后
        public void SetUserPlayerAsLastSibling()
        {
            if (_ServerSeatIndexInPlayer.ContainsKey(0))
            {
                _ServerSeatIndexInPlayer[0].gameObject.transform.SetAsLastSibling();
            }
        }
        //游戏小结算
        public void SmllResult(Actor_FiveStar_SmallResult message)
        {
            _RoomState = RoomStateType.ReadyIn;
            for (int i = 0; i < message.SmallPlayerResults.Count; i++)
            {
                _ServerSeatIndexInPlayer[message.SmallPlayerResults[i].SeatIndex]
                    .SmallResult(message.SmallPlayerResults[i]);//显示该玩家的牌
            }
        }

        //回归正常索引
        public void NormalPlayerSeatIndex(RepeatedField<long> newIndexInUser)
        {
            CardFiveStarPlayer[] allPlayers = _ServerSeatIndexInPlayer.Values.ToArray();
            _ServerSeatIndexInPlayer.Clear();
            for (int i = 0; i < newIndexInUser.Count; i++)
            {
                for (int j = 0; j < allPlayers.Length; j++)
                {
                    if (allPlayers[j]._user.UserId == newIndexInUser[i])
                    {
                        _ServerSeatIndexInPlayer[i] = allPlayers[j];
                    }
                }
            }
        }
        //玩家轮休
        public void PlayerRest(int restSeatIndex)
        {
            _ServerSeatIndexInPlayer[restSeatIndex].Rest();//玩家信息
        }
    }
}
