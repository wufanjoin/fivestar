using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
  public static partial class StringHelper
    {
        public static string OmitStr(this string str,int retainNum)
        {
            if (str.Length <= retainNum+2)
            {
                return str;
            }
          return  str.Substring(0, retainNum) + "...";
        }
    }
}
