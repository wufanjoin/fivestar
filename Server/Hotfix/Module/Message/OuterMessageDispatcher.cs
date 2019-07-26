using System;
using ETModel;
using Google.Protobuf;

namespace ETHotfix
{
	public class OuterMessageDispatcher: IMessageDispatcher
	{
		public async void Dispatch(Session session, ushort opcode, object message)
		{
			try
			{
                SessionHeartbeatComponent sessionHeartbeatComponent = session.GetComponent<SessionHeartbeatComponent>();
			    SessionUserComponent sessionUserComponent = session.GetComponent<SessionUserComponent>();
                if (sessionHeartbeatComponent == null)
                {
                    session.Dispose();//心跳组件 没有 直接销毁
                    return;
                }
                sessionHeartbeatComponent.UpReceiveMessageDistance = 0;//重置上次收到消息的时间
			    //如果没有挂载SessionUserComponent组件 需要判断一下是不是登陆消息
                if (sessionUserComponent == null)
			    {
			        if (message.GetType() != typeof(C2G_GateLogin) && message.GetType() != typeof(C2R_CommonLogin))
			        {
			            session.Dispose();//发其他 消息 却没有 SessionUserComponent 组件 绝对不走正常
                        return;
			        }
                }
			    //如果收到 一秒收到的消息 大于规定的消息 就认定是DOSS攻击 直接销毁
			    if (++sessionHeartbeatComponent.SecondTotalMessageNum >=
			        SessionHeartbeatComponent.DestroySeesiontSecondTotalNum)
			    {
                    //断开连接
			        sessionHeartbeatComponent.DisposeSession();
                    //直接封号
			      //  UserHelp.StopSealOrRelieve(sessionUserComponent.UserId,true,"DOSS攻击封号"); //不要封号容易误封
                    return;
			    }
                switch (message)
				{
                    case IActorMessage irActorMessage:
                        {
                            long sessionActorId= sessionUserComponent.GamerSessionActorId;
                            if (sessionActorId==0)
                            {
                                return;
                            }
                            ActorMessageSender actorLocationSender = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(sessionActorId);
				            actorLocationSender.Send(irActorMessage);
                            return;
				        }
                    case IUserRequest iUserRequest:
                        {
                            if (iUserRequest is IUserActorRequest userActorRequest)
                            {
                                userActorRequest.SessionActorId = session.Id;
                                userActorRequest.User = sessionUserComponent.user;
                            }
                            if (iUserRequest is IAdministratorRequest administratorRequest)
                            {
                                if (!AdministratorHelp.VerifyAdministrator(administratorRequest))
                                {
                                    //如果账号密码错误 直接返回 不如直接封号
                                    return;
                                }
                            }
                            iUserRequest.UserId = sessionUserComponent.UserId;
                            AppType appType = (AppType)(1 << ((opcode - 10000) / 1000));
                            Session serverSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(appType);
                            int rpcId = iUserRequest.RpcId; // 这里要保存客户端的rpcId
                            IResponse response = await serverSession.Call(iUserRequest);
                            response.RpcId = rpcId;
                            session.Reply(response);
                            return;
                        }
                        
                    case C2G_Heartbeat c2GHeartbeat:
                        return;

                }
				Game.Scene.GetComponent<MessageDispatherComponent>().Handle(session, new MessageInfo(opcode, message));
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
