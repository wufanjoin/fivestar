using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace ETModel
{


    public static class IListHelper
    {

        //打乱一个数组
        public static void RandomBrakRank<T>(this IList<T> cards) 
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int ranomValue = RandomTool.Random(0, cards.Count);
                T interim = cards[ranomValue];
                cards[ranomValue] = cards[i];
                cards[i] = interim;
            }
        }
        //从数组中取出一些数组 并删除原有数据
        public static RepeatedField<T> TakeOutCards<T>(this IList<T> cards, int cardNum)
        {
            RepeatedField<T> taskOutCards = new RepeatedField<T>();
            for (int i = 0; i < cardNum; i++)
            {
                taskOutCards.Add(cards[0]);
                cards.RemoveAt(0);
            }
            return taskOutCards;
        }
        public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {//外层循环控制排序趟数
                for (int j = 0; j < list.Count - 1 - i; j++)
                {//内层循环控制每一趟排序多少次
                    if (comparison(list[j], list[j + 1])>0)
                    {
                        T temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }
        public static void Sort(this IList<int> list)
        {
            list.Sort(ComparisonInt);
        }
        //public static void Sort<T>(this T list, Comparison<int> comparison = null) where T : IList<int>
        //{
        //    if (comparison == null) comparison = ComparisonInt;
        //    list.Sort<T, int>(comparison);
        //}

        //public static void Sort<T>(this T list, Comparison<long> comparison=null) where T : IList<long>
        //{
        //    if (comparison == null) comparison = ComparisonLong;
        //    list.Sort<T, long>(comparison);
        //}

        //public static void Sort<T>(this T list, Comparison<double> comparison = null) where T : IList<double>
        //{
        //    if (comparison == null) comparison = ComparisonDouble;
        //    list.Sort<T, double>(comparison);
        //}

        //public static void Sort<T>(this T list, Comparison<float> comparison = null) where T : IList<float>
        //{
        //    if (comparison == null) comparison = ComparisonFloat;
        //    list.Sort<T, float>(comparison);
        //}

        public static int ComparisonFloat(float x, float y)
        {
            if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public static int ComparisonDouble(double x, double y)
        {
            if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public static int ComparisonLong(long x, long y)
        {
            if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public static int ComparisonInt(int x, int y)
        {
            if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
