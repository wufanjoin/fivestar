using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace ETHotfix
{
    //切换状态时候的UI事件
    public partial class CardFiveStarPlayer
    {
        public virtual void RegisterEvent()
        {
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.Deal, Deal);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.HideAllPlayer, HideAllPlayer);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.CutMatchIn, CutMatchIn);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.CutReadyIn, CutReadyIn);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.CutGameIn, CutGameIn);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.CutBeginStartPrepare, CutBeginStartPrepare);
            EventMsgMgr.RegisterEvent(CardFiveStarEventID.CutRoomCardEnterReadyIn, CutRoomCardEnterReadyIn);

        }
        //切换到匹配中的状态
        public virtual void CutMatchIn(params object[] objs)
        {
            if (ClientSeatIndex != 0)
            {
                Hide();
            }
            else
            {
                _PlayerHead._PiaoNumText.gameObject.SetActive(false);
                HideAllCard();
            }
        }
        //隐藏所有玩家
        public virtual void HideAllPlayer(params object[] objs)
        {
            Hide();
        }
        //切换准备匹配的状态
        public virtual void CutBeginStartPrepare(params object[] objs)
        {

        }
        //房卡模式等人齐的状态
        public virtual void CutRoomCardEnterReadyIn(params object[] objs)
        {
            SetReadyState(true);
        }
        //切换游戏中的状态
        public void CutGameIn(params object[] objs)
        {
            SetReadyState(false);//准备
            
        }
        //切换准备中的状态
        public  void CutReadyIn(params object[] objs)
        {
            HideAllCard();
            _PlayerHead._PiaoNumText.gameObject.SetActive(false);//漂分
        }
        //显示表情
        public  void ShowExpression(int expressionIndex)
        {
            _PlayerHead.ShowExpression(expressionIndex);
        }
        //获取头像的绝对位置
        public Vector3 GetHeadPosition()
        {
            return _PlayerHead.gameObject.transform.position;
        }
    }
}
