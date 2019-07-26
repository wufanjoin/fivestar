using ETModel;
namespace ETHotfix
{
//请求公告
	[Message(LobbyOpcode.C2L_GetAnnouncement)]
	public partial class C2L_GetAnnouncement : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetAnnouncement)]
	public partial class L2C_GetAnnouncement : IResponse {}

//发起购买请求
	[Message(LobbyOpcode.C2L_BuyCommodity)]
	public partial class C2L_BuyCommodity : IUserRequest {}

	[Message(LobbyOpcode.L2C_BuyCommodity)]
	public partial class L2C_BuyCommodity : IResponse {}

//查询充值记录
	[Message(LobbyOpcode.C2L_QueryTopUpRecord)]
	public partial class C2L_QueryTopUpRecord : IAdministratorRequest {}

	[Message(LobbyOpcode.L2C_QueryTopUpRecord)]
	public partial class L2C_QueryTopUpRecord : IResponse {}

//充值补单
	[Message(LobbyOpcode.C2L_TopUpRepairOrder)]
	public partial class C2L_TopUpRepairOrder : IAdministratorRequest {}

	[Message(LobbyOpcode.L2C_TopUpRepairOrder)]
	public partial class L2C_TopUpRepairOrder : IResponse {}

//单次充值信息
	[Message(LobbyOpcode.TopUpRecord)]
	public partial class TopUpRecord {}

//向大厅服请求商品数据
	[Message(LobbyOpcode.C2L_GetCommodityList)]
	public partial class C2L_GetCommodityList : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetCommodityList)]
	public partial class L2C_GetCommodityList : IResponse {}

//向大厅服请求每次首次分享成功可以活动的钻石数量
	[Message(LobbyOpcode.C2L_GetTheFirstShareAward)]
	public partial class C2L_GetTheFirstShareAward : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetTheFirstShareAward)]
	public partial class L2C_GetTheFirstShareAward : IResponse {}

//每日分享朋友圈成功 领取奖励
	[Message(LobbyOpcode.C2L_GetEverydayShareAward)]
	public partial class C2L_GetEverydayShareAward : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetEverydayShareAward)]
	public partial class L2C_GetEverydayShareAward : IResponse {}

//客户端向大厅服请求签到奖励列表
	[Message(LobbyOpcode.C2L_GetSignInAwardList)]
	public partial class C2L_GetSignInAwardList : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetSignInAwardList)]
	public partial class L2C_GetSignInAwardList : IResponse {}

//客户端向大厅服请求今日签到
	[Message(LobbyOpcode.C2L_TodaySignIn)]
	public partial class C2L_TodaySignIn : IUserRequest {}

	[Message(LobbyOpcode.L2C_TodaySignIn)]
	public partial class L2C_TodaySignIn : IResponse {}

//请求游戏匹配房间配置
	[Message(LobbyOpcode.C2L_GetMatchRoomConfigs)]
	public partial class C2L_GetMatchRoomConfigs : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetMatchRoomConfigs)]
	public partial class L2C_GetMatchRoomConfigs : IResponse {}

//请求推广奖励信息
	[Message(LobbyOpcode.C2L_GetGenralizeInfo)]
	public partial class C2L_GetGenralizeInfo : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetGenralizeInfo)]
	public partial class L2C_GetGenralizeInfo : IResponse {}

//推广奖励信息
	[Message(LobbyOpcode.GeneralizeAwardInfo)]
	public partial class GeneralizeAwardInfo {}

//请求领取新手被邀请礼包的状态
	[Message(LobbyOpcode.C2L_GetGreenGiftStatu)]
	public partial class C2L_GetGreenGiftStatu : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetGreenGiftStatu)]
	public partial class L2C_GetGreenGiftStatu : IResponse {}

//领取新手被邀请礼包
	[Message(LobbyOpcode.C2L_GetGreenGift)]
	public partial class C2L_GetGreenGift : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetGreenGift)]
	public partial class L2C_GetGreenGift : IResponse {}

//获取客服信息
	[Message(LobbyOpcode.C2L_GetService)]
	public partial class C2L_GetService : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetService)]
	public partial class L2C_GetService : IResponse {}

	[Message(LobbyOpcode.ServiceInfo)]
	public partial class ServiceInfo {}

//获取是不是代理
	[Message(LobbyOpcode.C2L_GetAgencyStatu)]
	public partial class C2L_GetAgencyStatu : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetAgencyStatu)]
	public partial class L2C_GetAgencyStatu : IResponse {}

//卖出钻石
	[Message(LobbyOpcode.C2L_SaleJewel)]
	public partial class C2L_SaleJewel : IUserRequest {}

	[Message(LobbyOpcode.L2C_SaleJewel)]
	public partial class L2C_SaleJewel : IResponse {}

//获取销售记录
	[Message(LobbyOpcode.C2L_GetMarketRecord)]
	public partial class C2L_GetMarketRecord : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetMarketRecord)]
	public partial class L2C_GetMarketRecord : IResponse {}

//单个销售信息
	[Message(LobbyOpcode.MarketInfo)]
	public partial class MarketInfo {}

//请求转盘抽奖
	[Message(LobbyOpcode.C2L_TurntableDrawLottery)]
	public partial class C2L_TurntableDrawLottery : IUserRequest {}

	[Message(LobbyOpcode.L2C_TurntableDrawLottery)]
	public partial class L2C_TurntableDrawLottery : IResponse {}

//获取转盘抽奖物品列表
	[Message(LobbyOpcode.C2L_GetTurntableGoodss)]
	public partial class C2L_GetTurntableGoodss : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetTurntableGoodss)]
	public partial class L2C_GetTurntableGoodss : IResponse {}

//抽奖物品
	[Message(LobbyOpcode.TurntableGoods)]
	public partial class TurntableGoods {}

//获取可以免费抽奖的次数
	[Message(LobbyOpcode.C2L_GetFreeDrawLotteryCount)]
	public partial class C2L_GetFreeDrawLotteryCount : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetFreeDrawLotteryCount)]
	public partial class L2C_GetFreeDrawLotteryCount : IResponse {}

//获取抽奖记录
	[Message(LobbyOpcode.C2L_GetWinPrizeRecord)]
	public partial class C2L_GetWinPrizeRecord : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetWinPrizeRecord)]
	public partial class L2C_GetWinPrizeRecord : IResponse {}

	[Message(LobbyOpcode.WinPrizeRecord)]
	public partial class WinPrizeRecord {}

//查询大局战绩
	[Message(LobbyOpcode.C2L_GetPlayerMiltary)]
	public partial class C2L_GetPlayerMiltary : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetPlayerMiltary)]
	public partial class L2C_GetPlayerMiltary : IResponse {}

//大局战绩信息
	[Message(LobbyOpcode.Miltary)]
	public partial class Miltary {}

//大局玩家信息
	[Message(LobbyOpcode.MiltaryPlayerInfo)]
	public partial class MiltaryPlayerInfo {}

//查询大局里面的所有小局战绩
	[Message(LobbyOpcode.C2L_GetMiltarySmallInfo)]
	public partial class C2L_GetMiltarySmallInfo : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetMiltarySmallInfo)]
	public partial class L2C_GetMiltarySmallInfo : IResponse {}

//详细战绩信息
	[Message(LobbyOpcode.MiltarySmallInfo)]
	public partial class MiltarySmallInfo {}

//详细战绩里面没小局信息
	[Message(LobbyOpcode.ParticularMiltary)]
	public partial class ParticularMiltary {}

//查询录像数据
	[Message(LobbyOpcode.C2L_GetMiltaryRecordDataInfo)]
	public partial class C2L_GetMiltaryRecordDataInfo : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetMiltaryRecordDataInfo)]
	public partial class L2C_GetMiltaryRecordDataInfo : IResponse {}

//录像过程全部数据
	[Message(LobbyOpcode.ParticularMiltaryRecordDataInfo)]
	public partial class ParticularMiltaryRecordDataInfo {}

//录像过程中的单个数据
	[Message(LobbyOpcode.MiltaryRecordData)]
	public partial class MiltaryRecordData {}

//领取每日救济金
	[Message(LobbyOpcode.C2L_GetReliefPayment)]
	public partial class C2L_GetReliefPayment : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetReliefPayment)]
	public partial class L2C_GetReliefPayment : IResponse {}

//查询User信息
	[Message(LobbyOpcode.C2L_GetUserInfo)]
	public partial class C2L_GetUserInfo : IUserRequest {}

	[Message(LobbyOpcode.L2C_GetUserInfo)]
	public partial class L2C_GetUserInfo : IResponse {}

//查询抽奖记录
	[Message(LobbyOpcode.C2L_QueryWinPrizeRecord)]
	public partial class C2L_QueryWinPrizeRecord : IAdministratorRequest {}

	[Message(LobbyOpcode.L2C_QueryWinPrizeRecord)]
	public partial class L2C_QueryWinPrizeRecord : IResponse {}

//更改中奖记录
	[Message(LobbyOpcode.C2L_ChangeWinPrizeRecordState)]
	public partial class C2L_ChangeWinPrizeRecordState : IAdministratorRequest {}

	[Message(LobbyOpcode.L2C_ChangeWinPrizeRecordState)]
	public partial class L2C_ChangeWinPrizeRecordState : IResponse {}

//设置代理等级
	[Message(LobbyOpcode.C2L_SetAgencyLv)]
	public partial class C2L_SetAgencyLv : IAdministratorRequest {}

	[Message(LobbyOpcode.L2C_SetAgencyLv)]
	public partial class L2C_SetAgencyLv : IResponse {}

}
namespace ETHotfix
{
	public static partial class LobbyOpcode
	{
		 public const ushort C2L_GetAnnouncement = 18001;
		 public const ushort L2C_GetAnnouncement = 18002;
		 public const ushort C2L_BuyCommodity = 18003;
		 public const ushort L2C_BuyCommodity = 18004;
		 public const ushort C2L_QueryTopUpRecord = 18005;
		 public const ushort L2C_QueryTopUpRecord = 18006;
		 public const ushort C2L_TopUpRepairOrder = 18007;
		 public const ushort L2C_TopUpRepairOrder = 18008;
		 public const ushort TopUpRecord = 18009;
		 public const ushort C2L_GetCommodityList = 18010;
		 public const ushort L2C_GetCommodityList = 18011;
		 public const ushort C2L_GetTheFirstShareAward = 18012;
		 public const ushort L2C_GetTheFirstShareAward = 18013;
		 public const ushort C2L_GetEverydayShareAward = 18014;
		 public const ushort L2C_GetEverydayShareAward = 18015;
		 public const ushort C2L_GetSignInAwardList = 18016;
		 public const ushort L2C_GetSignInAwardList = 18017;
		 public const ushort C2L_TodaySignIn = 18018;
		 public const ushort L2C_TodaySignIn = 18019;
		 public const ushort C2L_GetMatchRoomConfigs = 18020;
		 public const ushort L2C_GetMatchRoomConfigs = 18021;
		 public const ushort C2L_GetGenralizeInfo = 18022;
		 public const ushort L2C_GetGenralizeInfo = 18023;
		 public const ushort GeneralizeAwardInfo = 18024;
		 public const ushort C2L_GetGreenGiftStatu = 18025;
		 public const ushort L2C_GetGreenGiftStatu = 18026;
		 public const ushort C2L_GetGreenGift = 18027;
		 public const ushort L2C_GetGreenGift = 18028;
		 public const ushort C2L_GetService = 18029;
		 public const ushort L2C_GetService = 18030;
		 public const ushort ServiceInfo = 18031;
		 public const ushort C2L_GetAgencyStatu = 18032;
		 public const ushort L2C_GetAgencyStatu = 18033;
		 public const ushort C2L_SaleJewel = 18034;
		 public const ushort L2C_SaleJewel = 18035;
		 public const ushort C2L_GetMarketRecord = 18036;
		 public const ushort L2C_GetMarketRecord = 18037;
		 public const ushort MarketInfo = 18038;
		 public const ushort C2L_TurntableDrawLottery = 18039;
		 public const ushort L2C_TurntableDrawLottery = 18040;
		 public const ushort C2L_GetTurntableGoodss = 18041;
		 public const ushort L2C_GetTurntableGoodss = 18042;
		 public const ushort TurntableGoods = 18043;
		 public const ushort C2L_GetFreeDrawLotteryCount = 18044;
		 public const ushort L2C_GetFreeDrawLotteryCount = 18045;
		 public const ushort C2L_GetWinPrizeRecord = 18046;
		 public const ushort L2C_GetWinPrizeRecord = 18047;
		 public const ushort WinPrizeRecord = 18048;
		 public const ushort C2L_GetPlayerMiltary = 18049;
		 public const ushort L2C_GetPlayerMiltary = 18050;
		 public const ushort Miltary = 18051;
		 public const ushort MiltaryPlayerInfo = 18052;
		 public const ushort C2L_GetMiltarySmallInfo = 18053;
		 public const ushort L2C_GetMiltarySmallInfo = 18054;
		 public const ushort MiltarySmallInfo = 18055;
		 public const ushort ParticularMiltary = 18056;
		 public const ushort C2L_GetMiltaryRecordDataInfo = 18057;
		 public const ushort L2C_GetMiltaryRecordDataInfo = 18058;
		 public const ushort ParticularMiltaryRecordDataInfo = 18059;
		 public const ushort MiltaryRecordData = 18060;
		 public const ushort C2L_GetReliefPayment = 18061;
		 public const ushort L2C_GetReliefPayment = 18062;
		 public const ushort C2L_GetUserInfo = 18063;
		 public const ushort L2C_GetUserInfo = 18064;
		 public const ushort C2L_QueryWinPrizeRecord = 18065;
		 public const ushort L2C_QueryWinPrizeRecord = 18066;
		 public const ushort C2L_ChangeWinPrizeRecordState = 18067;
		 public const ushort L2C_ChangeWinPrizeRecordState = 18068;
		 public const ushort C2L_SetAgencyLv = 18069;
		 public const ushort L2C_SetAgencyLv = 18070;
	}
}
