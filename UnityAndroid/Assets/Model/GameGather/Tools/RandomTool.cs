using System;

namespace ETModel
{
    public static class RandomTool
    {
        private static Random rd = new Random();

        //不包含最大值
        public static int Random(int min, int max)
        {
            return rd.Next(min, max);
        }
        //姑且认为 也不包含最大值 但可以到 0.999999999999999999999999999999999
        public static double Random(double min, double max)
        {
            return rd.NextDouble() * (max - min) + min;
        }


    }
}
