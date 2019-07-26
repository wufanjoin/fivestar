using ETModel;
namespace ETHotfix
{
//设置封号 和解封
	[Message(UserOpcode.C2U_SetIsStopSeal)]
	public partial class C2U_SetIsStopSeal : IAdministratorRequest {}

	[Message(UserOpcode.U2C_SetIsStopSeal)]
	public partial class U2C_SetIsStopSeal : IResponse {}

//更改用户 物品
	[Message(UserOpcode.C2U_ChangeUserGoods)]
	public partial class C2U_ChangeUserGoods : IAdministratorRequest {}

	[Message(UserOpcode.U2C_ChangeUserGoods)]
	public partial class U2C_ChangeUserGoods : IResponse {}

//获取在线人数
	[Message(UserOpcode.C2U_GetOnLineNumber)]
	public partial class C2U_GetOnLineNumber : IAdministratorRequest {}

	[Message(UserOpcode.U2C_GetOnLineNumber)]
	public partial class U2C_GetOnLineNumber : IResponse {}

//获取用户交易记录
	[Message(UserOpcode.C2U_GetGoodsDealRecord)]
	public partial class C2U_GetGoodsDealRecord : IAdministratorRequest {}

	[Message(UserOpcode.U2C_GetGoodsDealRecord)]
	public partial class U2C_GetGoodsDealRecord : IResponse {}

//查询用户信息
	[Message(UserOpcode.C2U_QueryUserInfo)]
	public partial class C2U_QueryUserInfo : IAdministratorRequest {}

	[Message(UserOpcode.U2C_QueryUserInfo)]
	public partial class U2C_QueryUserInfo : IResponse {}

//查询封号记录
	[Message(UserOpcode.C2U_QueryStopSealRecord)]
	public partial class C2U_QueryStopSealRecord : IAdministratorRequest {}

	[Message(UserOpcode.U2C_QueryStopSealRecord)]
	public partial class U2C_QueryStopSealRecord : IResponse {}

//单次封号信息
	[Message(UserOpcode.StopSealRecord)]
	public partial class StopSealRecord {}

//单次用户钻石交易记录
	[Message(UserOpcode.GoodsDealRecord)]
	public partial class GoodsDealRecord {}

}
namespace ETHotfix
{
	public static partial class UserOpcode
	{
		 public const ushort C2U_SetIsStopSeal = 17001;
		 public const ushort U2C_SetIsStopSeal = 17002;
		 public const ushort C2U_ChangeUserGoods = 17003;
		 public const ushort U2C_ChangeUserGoods = 17004;
		 public const ushort C2U_GetOnLineNumber = 17005;
		 public const ushort U2C_GetOnLineNumber = 17006;
		 public const ushort C2U_GetGoodsDealRecord = 17007;
		 public const ushort U2C_GetGoodsDealRecord = 17008;
		 public const ushort C2U_QueryUserInfo = 17009;
		 public const ushort U2C_QueryUserInfo = 17010;
		 public const ushort C2U_QueryStopSealRecord = 17011;
		 public const ushort U2C_QueryStopSealRecord = 17012;
		 public const ushort StopSealRecord = 17013;
		 public const ushort GoodsDealRecord = 17014;
	}
}
