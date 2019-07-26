using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_GateLoginHandler : AMRpcHandler<C2G_GateLogin, G2C_GateLogin>
	{
		protected override async void Run(Session session, C2G_GateLogin message, Action<G2C_GateLogin> reply)
		{
		    G2C_GateLogin response = new G2C_GateLogin();
			try
			{
			    
                long userId = Game.Scene.GetComponent<GateSessionKeyComponent>().Get(message.Key);
			    //添加收取Actor消息组件 并且本地化一下 就是所有服务器都能向这个对象发 并添加一个消息拦截器
			    await session.AddComponent<MailBoxComponent, string>(ActorInterceptType.GateSession).AddLocation();
                //通知GateUserComponent组件和用户服玩家上线 并获取User实体
                User user =await Game.Scene.GetComponent<GateUserComponent>().UserOnLine(userId, session.Id);
			    if (user == null)
			    {
			        response.Message = "用户信息查询不到";
			        reply(response);
                    return;
                }
                //记录客户端session在User中
                user.AddComponent<UserClientSessionComponent>().session = session;
                //给Session组件添加下线监听组件和添加User实体
                session.AddComponent<SessionUserComponent>().user= user;
                //返回客户端User信息和 当前服务器时间
                response.User = user;
			    response.ServerTime = TimeTool.GetCurrenTimeStamp();
                reply(response);
            }
            catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}

	    public bool tsasdc(User s)
	    {
	        return true;
	    }
	}
}