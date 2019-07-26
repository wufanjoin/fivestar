using ETModel;
namespace ETHotfix
{
	[Message(CommonModelOpcode.User)]
	public partial class User {}

	[Message(CommonModelOpcode.Commodity)]
	public partial class Commodity {}

	[Message(CommonModelOpcode.SignInAward)]
	public partial class SignInAward {}

	[Message(CommonModelOpcode.UserSingInState)]
	public partial class UserSingInState {}

	[Message(CommonModelOpcode.GetGoodsOne)]
	public partial class GetGoodsOne {}

//房间信息
	[Message(CommonModelOpcode.RoomInfo)]
	public partial class RoomInfo {}

//游戏匹配房间配置
	[Message(CommonModelOpcode.MatchRoomConfig)]
	public partial class MatchRoomConfig {}

//匹配到的玩家信息
	[Message(CommonModelOpcode.MatchPlayerInfo)]
	public partial class MatchPlayerInfo {}

}
namespace ETHotfix
{
	public static partial class CommonModelOpcode
	{
		 public const ushort User = 10001;
		 public const ushort Commodity = 10002;
		 public const ushort SignInAward = 10003;
		 public const ushort UserSingInState = 10004;
		 public const ushort GetGoodsOne = 10005;
		 public const ushort RoomInfo = 10006;
		 public const ushort MatchRoomConfig = 10007;
		 public const ushort MatchPlayerInfo = 10008;
	}
}
