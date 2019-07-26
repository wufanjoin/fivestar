using ETModel;
namespace ETHotfix
{
//服务器进行热更
	[Message(GatherOuterOpcode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(GatherOuterOpcode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

//这个应该是测试用的协议
	[Message(GatherOuterOpcode.C2R_Ping)]
	public partial class C2R_Ping : IRequest {}

	[Message(GatherOuterOpcode.R2C_Ping)]
	public partial class R2C_Ping : IResponse {}

//去验证验证账号 请求秘钥
	[Message(GatherOuterOpcode.C2R_CommonLogin)]
	public partial class C2R_CommonLogin : IRequest {}

	[Message(GatherOuterOpcode.R2C_CommonLogin)]
	public partial class R2C_CommonLogin : IResponse {}

//拿着秘钥去登陆
	[Message(GatherOuterOpcode.C2G_GateLogin)]
	public partial class C2G_GateLogin : IRequest {}

	[Message(GatherOuterOpcode.G2C_GateLogin)]
	public partial class G2C_GateLogin : IResponse {}

//通知玩家得到了物品
	[Message(GatherOuterOpcode.Actor_UserGetGoods)]
	public partial class Actor_UserGetGoods : IActorMessage {}

//通知某个玩家上线
	[Message(GatherOuterOpcode.Actor_UserOnLine)]
	public partial class Actor_UserOnLine : IActorMessage {}

//通知某个玩家下线
	[Message(GatherOuterOpcode.Actor_UserOffline)]
	public partial class Actor_UserOffline : IActorMessage {}

//通知客户端被挤号了
	[Message(GatherOuterOpcode.Actor_CompelAccount)]
	public partial class Actor_CompelAccount : IActorMessage {}

//上传定位和IP信息
	[Message(GatherOuterOpcode.C2G_UploadingLocationIp)]
	public partial class C2G_UploadingLocationIp : IRequest {}

	[Message(GatherOuterOpcode.G2U_UploadingLocationIp)]
	public partial class G2U_UploadingLocationIp : IRequest {}

	[Message(GatherOuterOpcode.S2C_UploadingLocationIp)]
	public partial class S2C_UploadingLocationIp : IResponse {}

//心跳消息
	[Message(GatherOuterOpcode.C2G_Heartbeat)]
	public partial class C2G_Heartbeat : IMessage {}

}
namespace ETHotfix
{
	public static partial class GatherOuterOpcode
	{
		 public const ushort C2M_Reload = 11001;
		 public const ushort M2C_Reload = 11002;
		 public const ushort C2R_Ping = 11003;
		 public const ushort R2C_Ping = 11004;
		 public const ushort C2R_CommonLogin = 11005;
		 public const ushort R2C_CommonLogin = 11006;
		 public const ushort C2G_GateLogin = 11007;
		 public const ushort G2C_GateLogin = 11008;
		 public const ushort Actor_UserGetGoods = 11009;
		 public const ushort Actor_UserOnLine = 11010;
		 public const ushort Actor_UserOffline = 11011;
		 public const ushort Actor_CompelAccount = 11012;
		 public const ushort C2G_UploadingLocationIp = 11013;
		 public const ushort G2U_UploadingLocationIp = 11014;
		 public const ushort S2C_UploadingLocationIp = 11015;
		 public const ushort C2G_Heartbeat = 11016;
	}
}
