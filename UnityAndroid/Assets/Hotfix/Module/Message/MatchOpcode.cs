using ETModel;
namespace ETHotfix
{
//请求开始匹配
	[Message(MatchOpcode.C2M_StartMatch)]
	public partial class C2M_StartMatch : IUserActorRequest {}

	[Message(MatchOpcode.M2C_StartMatch)]
	public partial class M2C_StartMatch : IResponse {}

//请求创建创建房间
	[Message(MatchOpcode.C2M_CreateRoom)]
	public partial class C2M_CreateRoom : IUserActorRequest {}

	[Message(MatchOpcode.M2C_CreateRoom)]
	public partial class M2C_CreateRoom : IResponse {}

//请求加入房间
	[Message(MatchOpcode.C2M_JoinRoom)]
	public partial class C2M_JoinRoom : IUserActorRequest {}

	[Message(MatchOpcode.M2C_JoinRoom)]
	public partial class M2C_JoinRoom : IResponse {}

//请求退出房间
	[Message(MatchOpcode.C2M_OutRoom)]
	public partial class C2M_OutRoom : IUserRequest {}

	[Message(MatchOpcode.M2C_OutRoom)]
	public partial class M2C_OutRoom : IResponse {}

//其他玩家加入房间
	[Message(MatchOpcode.Actor_OtherJoinRoom)]
	public partial class Actor_OtherJoinRoom : IActorMessage {}

//其他玩家退出房间
	[Message(MatchOpcode.Actor_OtherOutRoom)]
	public partial class Actor_OtherOutRoom : IActorMessage {}

//匹配服通知游戏服暂停计时 就是暂停游戏
	[Message(MatchOpcode.Actor_PauseRoomGame)]
	public partial class Actor_PauseRoomGame : IActorMessage {}

//匹配服通知游戏服房间解散
	[Message(MatchOpcode.Actor_RoomDissolve)]
	public partial class Actor_RoomDissolve : IActorMessage {}

//游戏服通知匹配服房间解散
	[Message(MatchOpcode.S2M_RoomDissolve)]
	public partial class S2M_RoomDissolve : IMessage {}

//玩家投票解散房间选择
	[Message(MatchOpcode.Actor_VoteDissolveSelect)]
	public partial class Actor_VoteDissolveSelect : IUserRequest {}

//玩家投票解散房间结果
	[Message(MatchOpcode.Actor_VoteDissolveRoomResult)]
	public partial class Actor_VoteDissolveRoomResult : IActorMessage {}

//单个玩家投票信息
	[Message(MatchOpcode.VoteInfo)]
	public partial class VoteInfo {}

//玩家聊天请求
	[Message(MatchOpcode.C2M_UserChat)]
	public partial class C2M_UserChat : IUserRequest {}

	[Message(MatchOpcode.M2C_UserChat)]
	public partial class M2C_UserChat : IResponse {}

//广播Actor的聊天信息
	[Message(MatchOpcode.Actor_UserChatInfo)]
	public partial class Actor_UserChatInfo : IActorMessage {}

//聊天信息
	[Message(MatchOpcode.ChatInfo)]
	public partial class ChatInfo {}

//通知游戏服开始一局游戏
	[Message(MatchOpcode.M2S_StartGame)]
	public partial class M2S_StartGame : IRequest {}

//游戏通知匹配服 开始游戏了 传一个房间ID
	[Message(MatchOpcode.S2M_StartGame)]
	public partial class S2M_StartGame : IRequest {}

//请求亲友圈所有的房间列表
	[Message(MatchOpcode.C2M_GetFriendsCircleRoomList)]
	public partial class C2M_GetFriendsCircleRoomList : IUserRequest {}

	[Message(MatchOpcode.M2C_GetFriendsCircleRoomList)]
	public partial class M2C_GetFriendsCircleRoomList : IResponse {}

//发送给客户端 告诉客户端该用户正在游戏中
	[Message(MatchOpcode.Actor_BeingInGame)]
	public partial class Actor_BeingInGame : IActorMessage {}

//请求断线重连的数据
	[Message(MatchOpcode.C2M_GetReconnectionRoomInfo)]
	public partial class C2M_GetReconnectionRoomInfo : IUserRequest {}

	[Message(MatchOpcode.M2C_GetReconnectionRoomInfo)]
	public partial class M2C_GetReconnectionRoomInfo : IResponse {}

//发送给游戏服告诉他 玩家请求重连
	[Message(MatchOpcode.Actor_UserRequestReconnectionRoom)]
	public partial class Actor_UserRequestReconnectionRoom : IActorMessage {}

}
namespace ETHotfix
{
	public static partial class MatchOpcode
	{
		 public const ushort C2M_StartMatch = 19001;
		 public const ushort M2C_StartMatch = 19002;
		 public const ushort C2M_CreateRoom = 19003;
		 public const ushort M2C_CreateRoom = 19004;
		 public const ushort C2M_JoinRoom = 19005;
		 public const ushort M2C_JoinRoom = 19006;
		 public const ushort C2M_OutRoom = 19007;
		 public const ushort M2C_OutRoom = 19008;
		 public const ushort Actor_OtherJoinRoom = 19009;
		 public const ushort Actor_OtherOutRoom = 19010;
		 public const ushort Actor_PauseRoomGame = 19011;
		 public const ushort Actor_RoomDissolve = 19012;
		 public const ushort S2M_RoomDissolve = 19013;
		 public const ushort Actor_VoteDissolveSelect = 19014;
		 public const ushort Actor_VoteDissolveRoomResult = 19015;
		 public const ushort VoteInfo = 19016;
		 public const ushort C2M_UserChat = 19017;
		 public const ushort M2C_UserChat = 19018;
		 public const ushort Actor_UserChatInfo = 19019;
		 public const ushort ChatInfo = 19020;
		 public const ushort M2S_StartGame = 19021;
		 public const ushort S2M_StartGame = 19022;
		 public const ushort C2M_GetFriendsCircleRoomList = 19023;
		 public const ushort M2C_GetFriendsCircleRoomList = 19024;
		 public const ushort Actor_BeingInGame = 19025;
		 public const ushort C2M_GetReconnectionRoomInfo = 19026;
		 public const ushort M2C_GetReconnectionRoomInfo = 19027;
		 public const ushort Actor_UserRequestReconnectionRoom = 19028;
	}
}
