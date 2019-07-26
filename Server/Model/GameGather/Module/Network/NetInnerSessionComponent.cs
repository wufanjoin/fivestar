using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ETHotfix;

namespace ETModel
{
    public class NetInnerSessionComponent : Component
    {
        //获取单服对应的Seesion 分布式 不能在服务器刚启动时候调用Get 因为有可能其他服务器还没启动
        public Session Get(AppType appType)
        {
            try
            {
                StartConfig startConfig;
                switch (appType)
                {
                    case AppType.Realm:
                        startConfig = StartConfigComponent.Instance.RealmConfig;
                        break;
                    case AppType.User:
                        startConfig = StartConfigComponent.Instance.UserConfig;
                        break;
                    case AppType.Lobby:
                        startConfig = StartConfigComponent.Instance.LobbyConfig;
                        break;
                    case AppType.DB:
                        startConfig = StartConfigComponent.Instance.DBConfig;
                        break;
                    case AppType.Location:
                        startConfig = StartConfigComponent.Instance.LocationConfig;
                        break;
                    case AppType.Match:
                        startConfig = StartConfigComponent.Instance.MatchConfig;
                        break;
                    case AppType.FriendsCircle:
                        startConfig = StartConfigComponent.Instance.FriendsCircleConfig;
                        break;
                    default:
                        throw new Exception("单服没有这个appType:" + appType);
                }
                return GetSession(startConfig);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
        //获取多服对应的Seesion
        public Session Get(AppType appType, int index)
        {
            try
            {
                StartConfig startConfig;
                switch (appType)
                {
                    case AppType.Map:
                        startConfig = StartConfigComponent.Instance.MapConfigs[index];
                        break;
                    case AppType.Gate:
                        startConfig = StartConfigComponent.Instance.GateConfigs[index];
                        break;
                    default:
                        throw new Exception("多服没有这个appType:" + appType);
                }
                return GetSession(startConfig);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        //给一个游戏获取一个游戏类型的服务器 暂时使用随机分配 固定为第一个
        public Session GetGameServerSession(long toyGameId)
        {
            try
            {
                StartConfig startConfig;
                switch (toyGameId)
                {
                    case ToyGameId.JoyLandlords:
                        startConfig = StartConfigComponent.Instance.JoyLandlordsConfigs[0];
                        break;
                    case ToyGameId.CardFiveStar:
                        startConfig = StartConfigComponent.Instance.CardFiveStartConfigs[0];
                        break;
                    default:
                        throw new Exception("没有这种类型游戏服务器" + toyGameId);

                }
                return GetSession(startConfig);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        //根据StartConfig获取Session
        public Session GetSession(StartConfig startConfig)
        {
            IPEndPoint innerAddress = startConfig.GetComponent<InnerConfig>().IPEndPoint;
            Session session = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);
            return session;
        }
    }
}
