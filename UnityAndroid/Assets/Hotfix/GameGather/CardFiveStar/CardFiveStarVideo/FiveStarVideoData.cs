using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public class Video_Deal
    {
        public List<RepeatedField<int>> AllHands=new List<RepeatedField<int>>();
    }

    public class Video_PiaoFen
    {
        public List<int> PiaoFens=new List<int>();
    }
}
