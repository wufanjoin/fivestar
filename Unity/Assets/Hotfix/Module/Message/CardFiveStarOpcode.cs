using ETModel;
namespace ETHotfix
{
//开始游戏通知
	[Message(CardFiveStarOpcode.Actor_FiveStar_StartGame)]
	public partial class Actor_FiveStar_StartGame : IActorMessage {}

//小局开始游戏
	[Message(CardFiveStarOpcode.Actor_FiveStar_SmallStartGame)]
	public partial class Actor_FiveStar_SmallStartGame : IActorMessage {}

//玩家准备
	[Message(CardFiveStarOpcode.Actor_FiveStar_PlayerReady)]
	public partial class Actor_FiveStar_PlayerReady : IActorMessage {}

//玩家轮休通知
	[Message(CardFiveStarOpcode.Actor_FiveStar_PlayerRest)]
	public partial class Actor_FiveStar_PlayerRest : IActorMessage {}

//玩家4人局 索引回归正常通知
	[Message(CardFiveStarOpcode.Actor_FiveStar_NormalSeatIndex)]
	public partial class Actor_FiveStar_NormalSeatIndex : IActorMessage {}

//发牌通知
	[Message(CardFiveStarOpcode.Actor_FiveStar_Deal)]
	public partial class Actor_FiveStar_Deal : IActorMessage {}

//玩家买马
	[Message(CardFiveStarOpcode.Actor_FiveStar_MaiMa)]
	public partial class Actor_FiveStar_MaiMa : IActorMessage {}

//可以打漂
	[Message(CardFiveStarOpcode.Actor_FiveStar_CanDaPiao)]
	public partial class Actor_FiveStar_CanDaPiao : IActorMessage {}

//打漂结果
	[Message(CardFiveStarOpcode.Actor_FiveStar_DaPiaoResult)]
	public partial class Actor_FiveStar_DaPiaoResult : IActorMessage {}

//可以进行碰杠胡
	[Message(CardFiveStarOpcode.Actor_FiveStar_CanOperate)]
	public partial class Actor_FiveStar_CanOperate : IActorMessage {}

//玩家进行碰杠胡操作
	[Message(CardFiveStarOpcode.Actor_FiveStar_OperateResult)]
	public partial class Actor_FiveStar_OperateResult : IActorMessage {}

//玩家操作信息
	[Message(CardFiveStarOpcode.FiveStarOperateInfo)]
	public partial class FiveStarOperateInfo {}

//玩家亮倒 不用服务器 通知 客户端自行判断
	[Message(CardFiveStarOpcode.Actor_FiveStar_LiangDao)]
	public partial class Actor_FiveStar_LiangDao : IActorMessage {}

//可以出牌
	[Message(CardFiveStarOpcode.Actor_FiveStar_CanPlayCard)]
	public partial class Actor_FiveStar_CanPlayCard : IActorMessage {}

//玩家出牌结果
	[Message(CardFiveStarOpcode.Actor_FiveStar_PlayCardResult)]
	public partial class Actor_FiveStar_PlayCardResult : IActorMessage {}

//玩家摸牌
	[Message(CardFiveStarOpcode.Actor_FiveStar_MoPai)]
	public partial class Actor_FiveStar_MoPai : IActorMessage {}

//刷新玩家手牌信息 每次出牌玩家都会收到
	[Message(CardFiveStarOpcode.Actor_FiveStar_NewestHands)]
	public partial class Actor_FiveStar_NewestHands : IActorMessage {}

//打牌过分中分数变化 一般只有杠牌才有
	[Message(CardFiveStarOpcode.Actor_FiveStar_ScoreChange)]
	public partial class Actor_FiveStar_ScoreChange : IActorMessage {}

//小结算
	[Message(CardFiveStarOpcode.Actor_FiveStar_SmallResult)]
	public partial class Actor_FiveStar_SmallResult : IActorMessage {}

//单个玩家小局结算结果
	[Message(CardFiveStarOpcode.FiveStar_SmallPlayerResult)]
	public partial class FiveStar_SmallPlayerResult {}

//总结算
	[Message(CardFiveStarOpcode.Actor_FiveStar_TotalResult)]
	public partial class Actor_FiveStar_TotalResult : IActorMessage {}

//单个玩家总结算结果
	[Message(CardFiveStarOpcode.FiveStarTotalPlayerResult)]
	public partial class FiveStarTotalPlayerResult {}

//托管状态改变
	[Message(CardFiveStarOpcode.Actor_FiveStar_CollocationChange)]
	public partial class Actor_FiveStar_CollocationChange : IActorMessage {}

//重连数据
	[Message(CardFiveStarOpcode.Actor_FiveStar_Reconnection)]
	public partial class Actor_FiveStar_Reconnection : IActorMessage {}

//卡五星玩家信息
	[Message(CardFiveStarOpcode.FiveStarPlayer)]
	public partial class FiveStarPlayer {}

//-----------------往下全是录像过程的具体数据协议-------------
//小局游戏初始化
	[Message(CardFiveStarOpcode.Video_GameInit)]
	public partial class Video_GameInit {}

	[Message(CardFiveStarOpcode.Video_PlayerInfo)]
	public partial class Video_PlayerInfo {}

}
namespace ETHotfix
{
	public static partial class CardFiveStarOpcode
	{
		 public const ushort Actor_FiveStar_StartGame = 22001;
		 public const ushort Actor_FiveStar_SmallStartGame = 22002;
		 public const ushort Actor_FiveStar_PlayerReady = 22003;
		 public const ushort Actor_FiveStar_PlayerRest = 22004;
		 public const ushort Actor_FiveStar_NormalSeatIndex = 22005;
		 public const ushort Actor_FiveStar_Deal = 22006;
		 public const ushort Actor_FiveStar_MaiMa = 22007;
		 public const ushort Actor_FiveStar_CanDaPiao = 22008;
		 public const ushort Actor_FiveStar_DaPiaoResult = 22009;
		 public const ushort Actor_FiveStar_CanOperate = 22010;
		 public const ushort Actor_FiveStar_OperateResult = 22011;
		 public const ushort FiveStarOperateInfo = 22012;
		 public const ushort Actor_FiveStar_LiangDao = 22013;
		 public const ushort Actor_FiveStar_CanPlayCard = 22014;
		 public const ushort Actor_FiveStar_PlayCardResult = 22015;
		 public const ushort Actor_FiveStar_MoPai = 22016;
		 public const ushort Actor_FiveStar_NewestHands = 22017;
		 public const ushort Actor_FiveStar_ScoreChange = 22018;
		 public const ushort Actor_FiveStar_SmallResult = 22019;
		 public const ushort FiveStar_SmallPlayerResult = 22020;
		 public const ushort Actor_FiveStar_TotalResult = 22021;
		 public const ushort FiveStarTotalPlayerResult = 22022;
		 public const ushort Actor_FiveStar_CollocationChange = 22023;
		 public const ushort Actor_FiveStar_Reconnection = 22024;
		 public const ushort FiveStarPlayer = 22025;
		 public const ushort Video_GameInit = 22026;
		 public const ushort Video_PlayerInfo = 22027;
	}
}
