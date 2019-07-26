using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class G2C_UserGetGoodsHandler : AMHandler<Actor_UserGetGoods>
    {
        protected override void Run(ETModel.Session session, Actor_UserGetGoods message)
        {
            List<GetGoodsOne> getGoodsOnes=new List<GetGoodsOne>();
            getGoodsOnes.AddRange(message.GetGoodsList.ToArray());
            Game.Scene.GetComponent<UserComponent>().GetGoodss(getGoodsOnes, message.IsShowHintPanel);
        }
    }
}
