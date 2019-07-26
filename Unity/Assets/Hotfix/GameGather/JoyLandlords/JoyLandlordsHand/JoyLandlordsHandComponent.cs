using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [ObjectSystem]
    public class AwakeJoyLandlordsHandComponent : AwakeSystem<JoyLandlordsHandComponent>
    {
        public override void Awake(JoyLandlordsHandComponent self)
        {
            JoyLandlordsHandComponent.Ins = self;
            JoyLandlordsHandComponent.Ins.RegisterEvent();
        }
    }
    public class JoyLandlordsHandComponent : Component
    {
        public static JoyLandlordsHandComponent Ins;
        public List<JoyLandlordsHand> HandLists = new List<JoyLandlordsHand>();

        public void RegisterEvent()
        {
            EventMsgMgr.RegisterEvent(JoyLandlordsEventID.Deal, Deal);
        }
        public async void Deal(params object[] objs)
        {
            RepeatedField<int> cards = objs[0] as RepeatedField<int>;
            List<int> FromBigArriveSmallList=JoyLandlordsCardTool.CardInSizeSort(cards);
            ClearHandDic();
            for (int i = 0; i < FromBigArriveSmallList.Count; i++)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(200);
                HandLists.Add(JoyLandlordsHandFactory.Create(FromBigArriveSmallList[i],i));

            }
        }
        //用户成为地主获取三张地主牌
        public async void GetThreeHandCard(RepeatedField<int> hands, RepeatedField<int> threeHand)
        {
            await RefrshHand(hands);
            foreach (var cardNum in threeHand)
            {
                GetHand(cardNum).SetPitchStatu(true);
            } 
        }

        //根据牌数组刷新手牌
        public async Task RefrshHand<T>(T notSorthands) where T : IList<int>
        {
            List<int> hands = JoyLandlordsCardTool.CardInSizeSort(notSorthands);
            for (int i = 0; i < hands.Count; i++)
            {
                if (i >= HandLists.Count)
                {
                    HandLists.Add(JoyLandlordsHandFactory.Create(hands[i]));
                }
                HandLists[i].SetCardDataUI(hands[i],i);
            }
            for (int i = hands.Count; i < HandLists.Count; i++)
            {
                HandLists[i].Dispose();
            }
            if (HandLists.Count > hands.Count)
            {
                HandLists.RemoveRange(hands.Count, HandLists.Count - hands.Count);
            }
            foreach (var hand in HandLists)
            {
                hand.CancelStatu();
            }
            await Task.Delay(100);
        }
        //获得单张手牌
        private JoyLandlordsHand GetHand(int cardNum)
        {
            foreach (var hand in HandLists)
            {
                if (hand.mCardNum == cardNum)
                {
                    return hand;
                }
            }
            Log.Error("要得到的手牌 手牌列表里面没有CardNum:"+ cardNum);
            return null;
        }
        //获取现在选中牌的数组
        public RepeatedField<int> GetPithCards()
        {
            RepeatedField<int> cards=new RepeatedField<int>();
            foreach (var hand in HandLists)
            {
                if (hand.PitchStatu)
                {
                    cards.Add(hand.mCardNum);
                }
            }
            return cards;
        }

        //选择第一张牌
        private int FistHandIndex = -1;
        public void SelectFistPai(int handIndex)
        {
            if (FistHandIndex ==-1)
            {
                FistHandIndex = handIndex;
                HandLists[FistHandIndex].SetPrimaryStatu(true);
            }
        }

        private int minSelelctIndex = 0;
        private int maxSelelctIndex = 0;
        //拖到牌 改变牌的预选状态
        public void DragPai(int handIndex)
        {
            if (FistHandIndex == -1)
            {
                return;
            }

            if (handIndex > FistHandIndex)
            {
                minSelelctIndex = FistHandIndex;
                maxSelelctIndex = handIndex;
            }
            else if (handIndex <= FistHandIndex)
            {
                minSelelctIndex = handIndex;
                maxSelelctIndex = FistHandIndex;
            }

            for (int i = 0; i < HandLists.Count; i++)
            {
                HandLists[i].SetPrimaryStatu(i >= minSelelctIndex && i <= maxSelelctIndex);
            }
            EndHandIndex = handIndex;
        }

        public int EndHandIndex;
        //从该牌上移开
        public void WithdrawTheCard(int handIndex)
        {
            if (FistHandIndex == -1)
            {
                return;
            }
            if (EndHandIndex == handIndex && (maxSelelctIndex - minSelelctIndex) == 1)
            {
                DragPai(FistHandIndex);
            }
        }
        //确认选择
        public void ConfirmSelect()
        {
            if (FistHandIndex == -1)
            {
                return;
            }
            foreach (var han in HandLists)
            {
                han.SelectFinsh();
            }
            FistHandIndex = -1;
        }
        public void ClearHandDic()
        {
            foreach (var hand in HandLists)
            {
                hand.Dispose();
            }
            HandLists.Clear();
        }

        public override void Dispose()
        {
            ClearHandDic();
            base.Dispose();
        }
    }
}
