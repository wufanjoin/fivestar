using System;

namespace ETModel
{
    public static class TimeTool
    {
        /// <summary>
        /// 一个的月的时间 就是30天
        /// </summary>
        public const long MonthsTime = 25920000000000;

        /// <summary>
        /// 一天的时间
        /// </summary>            
        public const long DaysTime = 864000000000;

        

        /// <summary>
        /// 一星期的时间
        /// </summary>               
        public const long WeekTime = 6048000000000;

        /// <summary>
        /// Ticks转换为毫秒 就是除以10000 
        /// </summary>
        /// <returns></returns>
        public static long TicksConvertMillisecond(long timeStamp)
        {
            return timeStamp / 10000;
        }

        //获取当前时间tiem
        public static DateTime GetCurrenDateTime()
        {
            return DateTime.Now;
        }

        //获取当前时间戳
        public static long GetCurrenTimeStamp() //除以10000 就是毫秒
        {
            return  DateTime.UtcNow.Ticks - _1970DataTimeTicks;
        }

        //获取下个星期一00:00的时间距离 现在 时间的时间戳
        public static long GetNextModayGapTimeStamp()
        {
            DateTime dateTime = GetCurrenDateTime();
            //星期日 按算应该算7 而程序定义是0 所以判断一下 手动更改
            int datweek = (int) dateTime.DayOfWeek;
            if (datweek == 0)
            {
                datweek = 7;
            }
            dateTime = dateTime.Date.AddDays(7- datweek + 1);//7-当前星期数 就是星期天00点 加1就是下星期一00点
            return ConvertDateTimeToLong(dateTime) - GetCurrenTimeStamp();
        }

        //判断时间戳 是不是今天
        public static bool TimeStampIsToday(long timeStamp)
        {
            DateTime nowTime = GetCurrenDateTime();
            DateTime dt = ConvertStringToDateTime(timeStamp);
            if (dt.Date == nowTime.Date)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static DateTime _1970DataTime=new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static long _1970DataTimeTicks = new System.DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToLong(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(_1970DataTime);
            long t = time.Ticks - startTime.Ticks;
            return t;
        }

        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            return ConvertStringToDateTime(long.Parse(timeStamp));
        }


        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static DateTime ConvertStringToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(_1970DataTime);
            return dtStart.AddTicks(timeStamp);
        }

        /// <summary>        
        /// 时间戳转为描述格式 （如：2019年9月5日 19:47:53）     
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static string ConvertLongToTimeDesc(long timeStamp)
        {
            DateTime dtStart = ConvertStringToDateTime(timeStamp);
            return dtStart.ToString("yyyy年MM月dd日 HH:mm:ss");
            return $"{dtStart.Year}年{dtStart.Month}月{dtStart.Day}日 {dtStart.Hour}:{dtStart.Minute}:{dtStart.Second}";
        }

        /// <summary>        
        /// DataTime描述格式 （如：2019年9月5日 19:47:53）     
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static string ConvertDataTimeToTimeDesc(DateTime dateTime)
        {
            return dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
        }
    }
}
