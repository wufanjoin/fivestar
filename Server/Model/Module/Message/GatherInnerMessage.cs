using ETModel;
using ETHotfix;
using System.Collections.Generic;
namespace ETModel
{
//验证服向网关服请求秘钥
	[Message(GatherInnerOpcode.R2G_GetLoginKey)]
	public partial class R2G_GetLoginKey: IRequest
	{
		public int RpcId { get; set; }

		public long UserId { get; set; }

	}

	[Message(GatherInnerOpcode.G2R_GetLoginKey)]
	public partial class G2R_GetLoginKey: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public long Key { get; set; }

	}

//验证服向用户服验证用户
	[Message(GatherInnerOpcode.R2U_VerifyUser)]
	public partial class R2U_VerifyUser: IRequest
	{
		public int RpcId { get; set; }

		public int LoginType { get; set; }

		public int PlatformType { get; set; }

		public string DataStr { get; set; }

	}

	[Message(GatherInnerOpcode.U2R_VerifyUser)]
	public partial class U2R_VerifyUser: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public long UserId { get; set; }

		public string Password { get; set; }

	}

//网关通知其他服玩家下线
	[Message(GatherInnerOpcode.G2S_UserOffline)]
	public partial class G2S_UserOffline: IMessage
	{
		public int RpcId { get; set; }

		public long UserId { get; set; }

	}

//网关通知其他服玩家上线
	[Message(GatherInnerOpcode.G2S_UserOnline)]
	public partial class G2S_UserOnline: IMessage
	{
		public int RpcId { get; set; }

		public long UserId { get; set; }

		public long SessionActorId { get; set; }

	}

//任意服务器通知用户服 玩家得到了物品
	[Message(GatherInnerOpcode.S2U_UserGetGoods)]
	public partial class S2U_UserGetGoods: IRequest
	{
		public int RpcId { get; set; }

		public long UserId { get; set; }

		public List<GetGoodsOne> GetGoodsList = new List<GetGoodsOne>();

		public bool isShowHintPanel { get; set; }

		public int GoodsChangeType { get; set; }

	}

	[Message(GatherInnerOpcode.U2S_UserGetGoods)]
	public partial class U2S_UserGetGoods: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public User user { get; set; }

	}

//获取一批不在线的玩家 当AI机器人
	[Message(GatherInnerOpcode.M2U_GetAIUser)]
	public partial class M2U_GetAIUser: IRequest
	{
		public int RpcId { get; set; }

	}

	[Message(GatherInnerOpcode.U2M_GetAIUser)]
	public partial class U2M_GetAIUser: IResponse
	{
		public int RpcId { get; set; }

		public int Error { get; set; }

		public string Message { get; set; }

		public List<User> users = new List<User>();

	}

//通知网关 玩家开始游戏
	[Message(GatherInnerOpcode.Actor_UserStartGame)]
	public partial class Actor_UserStartGame: IActorMessage
	{
		public int RpcId { get; set; }

		public long ActorId { get; set; }

		public long SessionActorId { get; set; }

	}

//通知网关 玩家结束游戏
	[Message(GatherInnerOpcode.Actor_UserEndGame)]
	public partial class Actor_UserEndGame: IActorMessage
	{
		public int RpcId { get; set; }

		public long ActorId { get; set; }

	}

//网关服发给游戏服 通知用户下线
	[Message(GatherInnerOpcode.Actor_UserOffLine)]
	public partial class Actor_UserOffLine: IActorMessage
	{
		public int RpcId { get; set; }

		public long ActorId { get; set; }

	}

//匹配服 通知其他服务器 玩家完成一局房卡游戏
	[Message(GatherInnerOpcode.M2S_UserFinishRoomCardGame)]
	public partial class M2S_UserFinishRoomCardGame: IMessage
	{
		public int RpcId { get; set; }

		public List<long> UserIds = new List<long>();

	}

//大厅服 通知其他服务器 到了周一00点刷新
	[Message(GatherInnerOpcode.L2S_WeekRefresh)]
	public partial class L2S_WeekRefresh: IMessage
	{
		public int RpcId { get; set; }

	}

}
