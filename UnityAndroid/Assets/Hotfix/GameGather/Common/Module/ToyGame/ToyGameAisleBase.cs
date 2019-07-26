using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public abstract class ToyGameAisleBase
    {
        public long pToyGameType { private set; get; }

        private ToyGameComponent mToyGameComponent;
        protected ToyGameComponent pToyGameComponent
        {
            get
            {
                if (mToyGameComponent == null)
                {
                    mToyGameComponent=Game.Scene.GetComponent<ToyGameComponent>();
                }
                return mToyGameComponent;
            }
        }
        public virtual void Awake(long gameType)
        {
            pToyGameType = gameType;
        }

        public virtual void StartGame(params object[] objs)
        {
            pToyGameComponent.CurrToyGame = pToyGameType;
        }


        //一定调  不管调用进入其他游戏 还是调用结算本游戏 先调
        public virtual void EndAndStartOtherGame()
        {

        }

        //不一定调   玩家调用直接开启其他游戏就不会调 后调
        public virtual async void EndGame()
        {
            pToyGameComponent.StartGame(ToyGameId.Lobby);
        }

       
    }
}
