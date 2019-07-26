using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
 public static class NumberHelper
    {
        public static string ConvertorTenUnit(this long l)
        {
            if (l < 10000)
            {
                return l.ToString();
            }
           return (l / 10000.00f).ToString("#0.00") + "万";
        }

        public static string ConvertorTenUnit(this int i)
        {
            if (i < 10000)
            {
                return i.ToString();
            }
            return (i / 10000.00f).ToString("#0.00") + "万";
        }
    }
}
