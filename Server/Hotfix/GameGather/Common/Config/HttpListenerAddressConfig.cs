using System;
using System.Collections.Generic;
using System.Reflection;
using ETModel;

namespace ETHotfix
{


    [Config((int)AppType.Lobby)]
    public  class HttpListenerAddressConfigCategory : ACategory<HttpListenerAddressConfig>
    {
    }

    public class HttpListenerAddressConfig : IConfig
    {

        public long Id { get; set; }
        public string Address;
        public string OuterPostAddress;
    }


}
