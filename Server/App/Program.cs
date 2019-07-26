using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using ETModel;
using NLog;

namespace App
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			// 异步方法全部会回掉到主线程
			SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
			
			try
			{			
				Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);
				Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());

				Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
				StartConfig startConfig = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId).StartConfig;
			    

                if (!options.AppType.Is(startConfig.AppType))
				{
					Log.Error("命令行参数apptype与配置不一致");
					return;
				}

				IdGenerater.AppId = options.AppId;

				LogManager.Configuration.Variables["appType"] = startConfig.AppType.ToString();
				LogManager.Configuration.Variables["appId"] = startConfig.AppId.ToString();
				LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType,-8}";
				LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:D3}";

				Log.Info($"server start........................ {startConfig.AppId} {startConfig.AppType}");

				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatherComponent>();

			    Game.Scene.AddComponent<ActorMessageSenderComponent>();//发送Actor消息
			    Game.Scene.AddComponent<ActorMessageDispatherComponent>();//接收Actor消息并处理
			    Game.Scene.AddComponent<LocationProxyComponent>();//实体添加MailBoxComponent组件接收Actor消息 需要这个
			    Game.Scene.AddComponent<DBProxyComponent>();//用于操作数据库
			    Game.Scene.AddComponent<ConfigComponent>();//配置文件读取组件
                //Game.Scene.AddComponent<ActorLocationSenderComponent>();//也是发送Actor消息 但是仅限于当前服务器 用不着
                //自己添加组件
                Game.Scene.AddComponent<NetInnerSessionComponent>();//获取服务器内部之间的Session

                // 根据不同的AppType添加不同的组件
                OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
				InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
				ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();

			    Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);//不需要测试服务器 所有服务器必须要有这个 内网组件
                switch (startConfig.AppType)
				{
					case AppType.Manager:
						Game.Scene.AddComponent<AppManagerComponent>();
						
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.Realm:
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<RealmGateAddressComponent>();
					    //与客户端有连接都要加
					    Game.Scene.AddComponent<HeartbeatMgrComponent>();//心跳管理组件 验证服 也是要加的
                        break;
					case AppType.Gate:
						Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<GateSessionKeyComponent>();
					    //网关
					    Game.Scene.AddComponent<GateUserComponent>();//网关管理用户的组件
					    //与客户端有连接都要加
					    Game.Scene.AddComponent<HeartbeatMgrComponent>();//心跳管理组件 验证服 也是要加的
                        break;
					case AppType.Location:
						Game.Scene.AddComponent<LocationComponent>();
						break;
					case AppType.Map:
						Game.Scene.AddComponent<UnitComponent>();
						Game.Scene.AddComponent<ServerFrameComponent>();
					    //与客户端有连接都要加
					    Game.Scene.AddComponent<HeartbeatMgrComponent>();//心跳管理组件 验证服 也是要加的
                        break;
					case AppType.AllServer:
						Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<UnitComponent>();
				

						Game.Scene.AddComponent<LocationComponent>();//本地实体组件 没用过
					
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);//外网地址组件
						Game.Scene.AddComponent<AppManagerComponent>();//服务器管理组件
						Game.Scene.AddComponent<RealmGateAddressComponent>();//验证服组件
						Game.Scene.AddComponent<GateSessionKeyComponent>();//网关秘钥组件
						Game.Scene.AddComponent<ServerFrameComponent>();//帧同步组件

					    //DB服
					    Game.Scene.AddComponent<DBComponent>();
					    Game.Scene.AddComponent<DBCacheComponent>();
                        //Game.Scene.AddComponent<HttpComponent>();

                        //自己添加组件
                        //网关
                        Game.Scene.AddComponent<GateUserComponent>();//网关管理用户的组件
                        //用户服
                        Game.Scene.AddComponent<UserComponent>();//用户服管理组件
					    Game.Scene.AddComponent<UserConfigComponent>();//用户配置组件
                        //大厅服
					    Game.Scene.AddComponent<ShoppingCommodityComponent>();//商品配置组件
					    Game.Scene.AddComponent<GameLobby>();//游戏大厅
					    Game.Scene.AddComponent<TopUpComponent>();//充值组件
                        //匹配服
                        Game.Scene.AddComponent<GameMatchRoomConfigComponent>();//匹配游戏房间配置表
					    Game.Scene.AddComponent<MatchRoomComponent>();//匹配服房间组件
			            //卡五星游戏服
					    Game.Scene.AddComponent<FiveStarRoomComponent>();//卡五星房间组件
                        //亲友圈服
					    Game.Scene.AddComponent<FriendsCircleComponent>();//亲友圈组件
                        //与客户端有连接都要加
					    Game.Scene.AddComponent<HeartbeatMgrComponent>();//心跳管理组件 验证服 也是要加的
                        break;
					case AppType.Benchmark:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<BenchmarkComponent, string>(clientConfig.Address);
						break;
					case AppType.BenchmarkWebsocketServer:
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.BenchmarkWebsocketClient:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<WebSocketBenchmarkComponent, string>(clientConfig.Address);
						break;
                    case AppType.Lobby:
                        //大厅服
                        Game.Scene.AddComponent<ShoppingCommodityComponent>();//商品配置组件
                        Game.Scene.AddComponent<GameLobby>();//游戏大厅
                        Game.Scene.AddComponent<TopUpComponent>();//充值组件
                        Game.Scene.AddComponent<GameMatchRoomConfigComponent>();//匹配游戏房间配置表 玩家获取匹配房间列表 所有也是要加
                        break;
                    case AppType.User:
                        //用户服
                        Game.Scene.AddComponent<UserComponent>();//用户服管理组件
                        Game.Scene.AddComponent<UserConfigComponent>();//用户配置组件
                        break;
				    case AppType.Match:
				        //匹配服
				        Game.Scene.AddComponent<GameMatchRoomConfigComponent>();//匹配游戏房间配置表
				        Game.Scene.AddComponent<MatchRoomComponent>();//匹配服房间组件
                        break;
                    case AppType.CardFiveStar:
                        //卡五星游戏服
                        Game.Scene.AddComponent<FiveStarRoomComponent>();//卡五星房间组件
                        break;
                    case AppType.FriendsCircle:
                        //亲友圈服
                        Game.Scene.AddComponent<FriendsCircleComponent>();//亲友圈组件
                        break;
                    case AppType.DB:
                        //DB服
                        Game.Scene.AddComponent<DBComponent>();
                        Game.Scene.AddComponent<DBCacheComponent>();
                        break;
                    default:
						throw new Exception($"命令行参数没有设置正确的AppType: {startConfig.AppType}");
				}

				while (true)
				{
					try
					{
						Thread.Sleep(1);
						OneThreadSynchronizationContext.Instance.Update();
						Game.EventSystem.Update();
					}
					catch (Exception e)
					{
						Log.Error(e);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
