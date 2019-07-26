using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
  public static class JoyLdsGameDealLogic
  {

        private readonly static List<int> mInitCards = new List<int>();
        private readonly static List<int> mTakeOutCardsOrder = new List<int>();

        //初始化牌的数组
        static JoyLdsGameDealLogic()
          {
              for (int i = 1; i <= 54; i++)
              {
                  mInitCards.Add(i);
              }
              mTakeOutCardsOrder.Add(17);
              mTakeOutCardsOrder.Add(17);
              mTakeOutCardsOrder.Add(17);
              mTakeOutCardsOrder.Add(3);
        }

        //获取一个分派牌的二维数组
        public static RepeatedField<RepeatedField<int>> DealCards()
        {
            RepeatedField<RepeatedField<int>> distrbuteCards=new RepeatedField<RepeatedField<int>>();
            RepeatedField<int> initCards = new RepeatedField<int>();
            mInitCards.ForEach(i => initCards.Add(i));//就是克隆mInitCards
            initCards.RandomBrakRank();
            for (int i = 0; i < mTakeOutCardsOrder.Count; i++)
            {
                distrbuteCards.Add(initCards.TakeOutCards(mTakeOutCardsOrder[i]));
            }
            return distrbuteCards;
        }




    }
}
