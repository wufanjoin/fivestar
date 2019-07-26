using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ETHotfix
{

 public  static partial class  JoyLandlordsCardTool
    {
        //注意牌是从1开始的

        //获取牌的类型 花色
        public static int CardConvertorType(int card)
        {
            return (card - 1) / 13;
        }
        //获取牌的大小 如（3.4.5.6）K是12 A是14 2是15
        public static int CardConvertorSize(int card)
        {
            if (card >= 53)
            {
                return card;
            }
            int size = card % 13;
            if (size < 3)
            {
               size += 13;
            }
           
            return size;
        }

        //获取牌的大小 如（3.4.5.6）
        public static List<int> CardConvertorSize<T>(T cards) where T:IList<int>
        {
            List<int> sizes=new List<int>();
            foreach (var card in cards)
            {
                sizes.Add(CardConvertorSize(card));
            }
            return sizes;
        }

        public static List<int> CardInSizeSort<T>(T cards) where T : IList<int>
        {
            List<int> cardsList = new List<int>();
            cardsList.AddRange(cards.ToArray());
            cardsList.Sort(FromBigArriveSmall);
            return cardsList;
        }

        public static int FromBigArriveSmall(int x,int y)
        {
            if (CardConvertorSize(x) > CardConvertorSize(y))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

    }

}
