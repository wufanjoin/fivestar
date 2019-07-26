using ETModel;
namespace ETHotfix
{
//开始游戏通知
	[Message(JoyLandlordsOpcode.Actor_JoyLds_StartGame)]
	public partial class Actor_JoyLds_StartGame : IActorMessage {}

//发牌通知
	[Message(JoyLandlordsOpcode.Actor_JoyLds_Deal)]
	public partial class Actor_JoyLds_Deal : IActorMessage {}

//可以叫地主
	[Message(JoyLandlordsOpcode.Actor_JoyLds_CanCallLanlord)]
	public partial class Actor_JoyLds_CanCallLanlord : IActorMessage {}

//叫地主结果
	[Message(JoyLandlordsOpcode.Actor_JoyLds_CallLanlord)]
	public partial class Actor_JoyLds_CallLanlord : IActorMessage {}

//可以加倍
	[Message(JoyLandlordsOpcode.Actor_JoyLds_CanAddTwice)]
	public partial class Actor_JoyLds_CanAddTwice : IActorMessage {}

//加倍结果
	[Message(JoyLandlordsOpcode.Actor_JoyLds_AddTwice)]
	public partial class Actor_JoyLds_AddTwice : IActorMessage {}

//可以抢地主
	[Message(JoyLandlordsOpcode.Actor_JoyLds_CanRobLanlord)]
	public partial class Actor_JoyLds_CanRobLanlord : IActorMessage {}

//抢地主结果
	[Message(JoyLandlordsOpcode.Actor_JoyLds_RobLanlord)]
	public partial class Actor_JoyLds_RobLanlord : IActorMessage {}

//没人叫地主重新发牌
	[Message(JoyLandlordsOpcode.Actor_JoyLds_AnewDeal)]
	public partial class Actor_JoyLds_AnewDeal : IActorMessage {}

//游戏结束 玩家选择退出房间
	[Message(JoyLandlordsOpcode.Actor_JoyLds_OutRoom)]
	public partial class Actor_JoyLds_OutRoom : IActorMessage {}

//解散房间
	[Message(JoyLandlordsOpcode.Actor_JoyLds_DissolveRoom)]
	public partial class Actor_JoyLds_DissolveRoom : IActorMessage {}

//玩家准备
	[Message(JoyLandlordsOpcode.Actor_JoyLds_Prepare)]
	public partial class Actor_JoyLds_Prepare : IActorMessage {}

//确定地主和农民阵营
	[Message(JoyLandlordsOpcode.Actor_JoyLds_ConfirmCamp)]
	public partial class Actor_JoyLds_ConfirmCamp : IHandActorMessage {}

//可以出牌
	[Message(JoyLandlordsOpcode.Actor_JoyLds_CanPlayCard)]
	public partial class Actor_JoyLds_CanPlayCard : IActorMessage {}

//玩家出牌
	[Message(JoyLandlordsOpcode.Actor_JoyLds_PlayCard)]
	public partial class Actor_JoyLds_PlayCard : IHandActorMessage {}

//玩家不出牌
	[Message(JoyLandlordsOpcode.Actor_JoyLds_DontPlay)]
	public partial class Actor_JoyLds_DontPlay : IActorMessage {}

//游戏结果
	[Message(JoyLandlordsOpcode.Actor_JoyLds_GameResult)]
	public partial class Actor_JoyLds_GameResult : IActorMessage {}

//单个玩家计算结果
	[Message(JoyLandlordsOpcode.JoyLds_PlayerResult)]
	public partial class JoyLds_PlayerResult {}

//都地主玩家信息
	[Message(JoyLandlordsOpcode.JoyLds_PlayerInfo)]
	public partial class JoyLds_PlayerInfo {}

}
namespace ETHotfix
{
	public static partial class JoyLandlordsOpcode
	{
		 public const ushort Actor_JoyLds_StartGame = 21001;
		 public const ushort Actor_JoyLds_Deal = 21002;
		 public const ushort Actor_JoyLds_CanCallLanlord = 21003;
		 public const ushort Actor_JoyLds_CallLanlord = 21004;
		 public const ushort Actor_JoyLds_CanAddTwice = 21005;
		 public const ushort Actor_JoyLds_AddTwice = 21006;
		 public const ushort Actor_JoyLds_CanRobLanlord = 21007;
		 public const ushort Actor_JoyLds_RobLanlord = 21008;
		 public const ushort Actor_JoyLds_AnewDeal = 21009;
		 public const ushort Actor_JoyLds_OutRoom = 21010;
		 public const ushort Actor_JoyLds_DissolveRoom = 21011;
		 public const ushort Actor_JoyLds_Prepare = 21012;
		 public const ushort Actor_JoyLds_ConfirmCamp = 21013;
		 public const ushort Actor_JoyLds_CanPlayCard = 21014;
		 public const ushort Actor_JoyLds_PlayCard = 21015;
		 public const ushort Actor_JoyLds_DontPlay = 21016;
		 public const ushort Actor_JoyLds_GameResult = 21017;
		 public const ushort JoyLds_PlayerResult = 21018;
		 public const ushort JoyLds_PlayerInfo = 21019;
	}
}
