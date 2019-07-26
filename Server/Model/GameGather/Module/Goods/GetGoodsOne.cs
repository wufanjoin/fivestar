using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
   public partial class GetGoodsOne: Entity
    {
        public GetGoodsOne()
        {
            
        }
        public void SetGoodsOne(long goodsId,int amount)
        {
            GoodsId = goodsId;
            GetAmount = amount;
        }
    }
}
