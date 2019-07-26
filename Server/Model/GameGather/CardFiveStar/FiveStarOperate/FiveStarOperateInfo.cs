using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
   public partial class FiveStarOperateInfo:Entity
    {
        public override void Dispose()
        {
            base.Dispose();
            Card = 0;
            OperateType = 0;
            PlayCardIndex = 0;

        }
    }
}
