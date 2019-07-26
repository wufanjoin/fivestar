using System;
using System.Collections.Generic;
using System.Reflection;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class ToyGameComponentAwakeSystem : ETHotfix.AwakeSystem<ToyGameComponent>
    {
        public override void Awake(ToyGameComponent self)
        {
            self.Awake();
        }
    }
    public class ToyGameComponent : Component
    {
        public long CurrToyGame = ToyGameId.None;
        private readonly Dictionary<long, ToyGameAisleBase> mGameAisleBaseDic=new Dictionary<long, ToyGameAisleBase>();
        public void Awake()
        {
            mGameAisleBaseDic.Clear();
            List<Type> types = Game.EventSystem.GetTypes();

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(ToyGameAttribute), false);
           
                if (attrs.Length == 0)
                {
                    continue;
                }
                ToyGameAttribute toyGameAttribute= attrs[0] as ToyGameAttribute;
                ToyGameAisleBase toyGameAisleBase = Activator.CreateInstance(type) as ToyGameAisleBase;
                toyGameAisleBase.Awake(toyGameAttribute.Type);
                mGameAisleBaseDic.Add(toyGameAttribute.Type, toyGameAisleBase);
            }
        }

        public void StartGame(long gameType,params object[] objs)
        {
            if (mGameAisleBaseDic.ContainsKey(gameType))
            {
                if (CurrToyGame != ToyGameId.None)
                {
                    mGameAisleBaseDic[CurrToyGame].EndAndStartOtherGame();
                }
                mGameAisleBaseDic[gameType].StartGame(objs);
            }
            else
            {
                Log.Error("想要进入的游戏不存在:"+ gameType);
            }
        }
        public void EndGame()
        {
            if (mGameAisleBaseDic.ContainsKey(CurrToyGame))
            {
                mGameAisleBaseDic[CurrToyGame].EndGame();
            }
            else
            {
                Log.Error("系统错误,目前状态游戏不存在:" + CurrToyGame);
            }
        }

     
    }
}


