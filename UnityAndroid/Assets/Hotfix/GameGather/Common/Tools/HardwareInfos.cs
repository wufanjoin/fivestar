using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    public static class PlatformType
    {
        public const int Android = 1;
        public const int IOS = 2;
        public const int PC = 3;
    }

    public static class HardwareInfos
    {
#if UNITY_ANDROID
         public static int pCurrentPlatform = PlatformType.Android;
#endif

#if UNITY_IPHONE
         public static int pCurrentPlatform = PlatformType.IOS;
#endif

#if UNITY_STANDALONE_WIN
        public static int pCurrentPlatform = PlatformType.PC;
#endif
        

        
    }
}

