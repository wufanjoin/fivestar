using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
  public static class UserSystem
    {
        //发送消息给User对应的客户端 发给网关Sesssion会有拦截器 自动转发给客户端
        public static void SendeSessionClientActor(this User user, IActorMessage iActorMessage)
        {
            if (user.GetComponent<UserGateActorIdComponent>() != null)
            {
                long actorId = user.GetComponent<UserGateActorIdComponent>().ActorId;
                ActorHelp.SendeActor(actorId, iActorMessage);
            }
        }

        //获取User所在连接的网关Session
        public static void SendUserGateSession(this User user, IMessage iMessage)
        {
            long actorId = user.GetComponent<UserGateActorIdComponent>().ActorId;
            if (actorId == 0)
            {
                return;
            }
            IPEndPoint ipEndPoint = StartConfigComponent.Instance.GetInnerAddress(IdGenerater.GetAppId(actorId));
            Game.Scene.GetComponent<NetInnerComponent>().Get(ipEndPoint).Send(iMessage);//发送消息给用户所在的网关
        }

        //改变用户物品
        public static async Task GoodsChange(this User user,long goodId,int amount, int changeType, bool isShowHint = false,bool isSynchronizationGoods=true)
        {
            User newUser = await UserHelp.GoodsChange(user.UserId, goodId, amount, changeType, isShowHint);
            if (isSynchronizationGoods&&newUser != null)
            {
                user.Beans = newUser.Beans;
                user.Beans = newUser.Jewel;
            }
        }
        //改变用户物品
        public static async Task GoodsChange(this User user, List<GetGoodsOne> getGoodsOnes, int changeType, bool isShowHint=false)
        {
            User newUser=await UserHelp.GoodsChange(user.UserId, getGoodsOnes, changeType, isShowHint);
            if (newUser != null)
            {
                user.Beans = newUser.Beans;
                user.Beans = newUser.Jewel;
            }
        }

        //刷新 同步 物品数量
        public static void RefreshGoods(this User user, IList<GetGoodsOne> getGoodsOnes)
        {
            for (int i = 0; i < getGoodsOnes.Count; i++)
            {
                switch (getGoodsOnes[i].GoodsId)
                {
                    case GoodsId.Besans:
                        user.Beans = getGoodsOnes[i].NowAmount;
                        break;
                    case GoodsId.Jewel:
                        user.Jewel = getGoodsOnes[i].NowAmount;
                        break;
                }
            }
        }

        //更新物品 防止不同 只能用户服调用这个
        public static void UpdataGoods(this User user, List<GetGoodsOne> getGoodsOnes)
        {
            for (int i = 0; i < getGoodsOnes.Count; i++)
            {
                switch (getGoodsOnes[i].GoodsId)
                {
                    case GoodsId.Besans:
                        user.Beans += getGoodsOnes[i].GetAmount;
                        break;
                    case GoodsId.Jewel:
                        user.Jewel += getGoodsOnes[i].GetAmount;
                        break;
                }
            }
            if (user.Beans<0)
            {
                if (user.UserId > 1000)//UserId小于1000 肯定是AI机器人 豆子可以为负数
                {
                    user.Beans = 0;
                }
            }
            if (user.Jewel < 0)
            {
                user.Jewel = 0;
            }
        }
        //得到用户所对应的客户端Seesion
        public static Session GetUserClientSession(this User user)
        {
            return user.GetComponent<UserClientSessionComponent>().session;
        }
        //网关通知客户端玩家被挤号了
        public static void ByCompelAccount(this User user)
        {
            user.SendeSessionClientActor(new Actor_CompelAccount() { Message = "账号在别处登陆" });//客户端收到这条消息后要主动断开连接
        }
    }

}
