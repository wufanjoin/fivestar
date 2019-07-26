using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class CardFiveGameDealLogic
    {
        private readonly static List<int> mInitCards = new List<int>();

        private const int DealCardCount = 13;
        //初始化牌的数组
        static CardFiveGameDealLogic()
        {
            for (int i = 1; i <= 21; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    mInitCards.Add(i);
                }
            }

        }
        //获取一个 发牌的二维数组 数组的最后一个是剩余牌数组
        public static RepeatedField<RepeatedField<int>> DealCards(int number)
        {
            //return TestDealCards();
            RepeatedField<RepeatedField<int>> distrbuteCards = new RepeatedField<RepeatedField<int>>();
            RepeatedField<int> initCards = new RepeatedField<int>() { mInitCards };
            initCards.RandomBrakRank();
            int count = 0;
            for (int i = 0; i < number; i++)
            {
                distrbuteCards.Add(initCards.TakeOutCards(DealCardCount));
            }
            distrbuteCards.Add(initCards);
            return distrbuteCards;
        }
        //如果是匹配模式 并且有AI参与
        public static RepeatedField<RepeatedField<int>> AIDealCards(int number)
        {
            RepeatedField<RepeatedField<int>> distrbuteCards = new RepeatedField<RepeatedField<int>>();
            RepeatedField<int> initCards = new RepeatedField<int>() { mInitCards };

            //获取必赢的牌
            RepeatedField<int> sureWinCards = new RepeatedField<int>();
            for (int i = 0; i < 4; i++)
            {
                int shunZiStart = RandomTool.Random(1, 17);
                while (shunZiStart == 8 || shunZiStart == 9)
                {
                    shunZiStart = RandomTool.Random(1, 17);
                }
                for (int j = 0; j < 3; j++)
                {
                    sureWinCards.Add(shunZiStart + j);
                    initCards.Remove(shunZiStart + j);
                }
            }
            int jiangNum = RandomTool.Random(1, 22);
            while (sureWinCards.Contains(jiangNum))
            {
                jiangNum = RandomTool.Random(1, 22);
            }
            sureWinCards.Add(jiangNum);
            sureWinCards.Add(jiangNum);
            initCards.Remove(jiangNum);
            initCards.Remove(jiangNum);
            int winCardIndex = RandomTool.Random(0, sureWinCards.Count);
            int winCard = sureWinCards[winCardIndex];
            sureWinCards.Remove(winCard);
            sureWinCards.Add(winCard);
            //将牌打乱获取正常玩家的牌
            initCards.RandomBrakRank();
            for (int i = 0; i < number; i++)
            {
                distrbuteCards.Add(initCards.TakeOutCards(DealCardCount));
            }
            initCards.AddRange(sureWinCards);
            distrbuteCards.Add(initCards);
            return distrbuteCards;
        }

       
        //测试的时候使用的发牌逻辑
        public static RepeatedField<RepeatedField<int>> TestDealCards()
        {
            RepeatedField<RepeatedField<int>> distrbuteCards = new RepeatedField<RepeatedField<int>>();

            RepeatedField<int> hand1 = new RepeatedField<int>() { 3,3,3,2 };
            RepeatedField<int> hand2 = new RepeatedField<int>() { 2,3,2,3 };
            RepeatedField<int> hand3 = new RepeatedField<int>() { 2,3,2,3 };

            RepeatedField<int> cards = new RepeatedField<int>();

            cards.Add(2);
            cards.Add(5);
            cards.Add(5);
            cards.Add(5);
            cards.Add(6);
            for (int i = 11; i <= 21; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    cards.Add(i);
                }
            }
            distrbuteCards.Add(hand1);
            distrbuteCards.Add(hand2);
            distrbuteCards.Add(hand3);
            distrbuteCards.Add(cards);
            return distrbuteCards;
        }
    }
}
