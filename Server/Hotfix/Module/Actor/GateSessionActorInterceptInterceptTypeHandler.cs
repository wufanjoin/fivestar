using System;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
	/// <summary>
	/// gate session 拦截器，收到的actor消息直接转发给客户端
	/// </summary>
	[ActorInterceptTypeHandler(AppType.Gate, ActorInterceptType.GateSession)]
	public class GateSessionActorInterceptInterceptTypeHandler : IActorInterceptTypeHandler
	{
		public async Task Handle(Session session, Entity entity, object actorMessage)
		{
			try
			{
				IActorMessage iActorMessage = actorMessage as IActorMessage;

			    //连接客户端的Seeion
			    Session clientSession = entity as Session;



                //如果 是刷新物品消息 也要刷新一个网关的user 的物品数量
                switch (iActorMessage)
			    {
                    case Actor_UserGetGoods userGetGoods:
                        SendClient(clientSession, iActorMessage);
                        clientSession.GetComponent<SessionUserComponent>().user.RefreshGoods(userGetGoods.GetGoodsList);
                        break;
                    case Actor_CompelAccount compelAccount:
                        SendClient(clientSession, iActorMessage);
                        clientSession.GetComponent<SessionUserComponent>().user = null;
                        break;
			        case Actor_UserStartGame startGame:
			            clientSession.GetComponent<SessionUserComponent>().GamerSessionActorId = startGame.SessionActorId;
			            break;
			        case Actor_UserEndGame userEndGame:
			            clientSession.GetComponent<SessionUserComponent>().GamerSessionActorId =0;
			            break;
                    default:
                        SendClient(clientSession, iActorMessage);
                        break;
                        
                }
			    
                await Task.CompletedTask;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

        //把消息发送给客户端
	    public void SendClient(Session clientSession, IActorMessage iActorMessage)
	    {
	        iActorMessage.ActorId = 0;
	        clientSession.Send(iActorMessage);
        }
	}
}
