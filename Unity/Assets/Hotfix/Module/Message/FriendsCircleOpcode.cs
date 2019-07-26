using ETModel;
namespace ETHotfix
{
//import "CommonModelMessage.proto";
//获取自己已加入的亲友圈列表
	[Message(FriendsCircleOpcode.C2F_GetSelfFriendsList)]
	public partial class C2F_GetSelfFriendsList : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_GetSelfFriendsList)]
	public partial class F2C_GetSelfFriendsList : IResponse {}

//申请加入亲友圈
	[Message(FriendsCircleOpcode.C2F_ApplyJoinFriendsCircle)]
	public partial class C2F_ApplyJoinFriendsCircle : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_ApplyJoinFriendsCircle)]
	public partial class F2C_ApplyJoinFriendsCircle : IResponse {}

//获取推荐亲友圈 每次固定5个
	[Message(FriendsCircleOpcode.C2F_GetRecommendFriendsCircle)]
	public partial class C2F_GetRecommendFriendsCircle : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_GetRecommendFriendsCircle)]
	public partial class F2C_GetRecommendFriendsCircle : IResponse {}

//获取指定亲友圈信息
	[Message(FriendsCircleOpcode.C2F_GetFriendsCircleInfo)]
	public partial class C2F_GetFriendsCircleInfo : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_GetFriendsCircleInfo)]
	public partial class F2C_GetFriendsCircleInfo : IResponse {}

//获取申请加入亲友圈的玩家信息
	[Message(FriendsCircleOpcode.C2F_GetFriendsCircleApplyJoinList)]
	public partial class C2F_GetFriendsCircleApplyJoinList : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_GetFriendsCircleApplyJoinList)]
	public partial class F2C_GetFriendsCircleApplyJoinList : IResponse {}

//处理申请信息
	[Message(FriendsCircleOpcode.C2F_DisposeApplyInfo)]
	public partial class C2F_DisposeApplyInfo : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_DisposeApplyInfo)]
	public partial class F2C_DisposeApplyInfo : IResponse {}

//获取排行榜信息
	[Message(FriendsCircleOpcode.C2F_GetRankingListInfo)]
	public partial class C2F_GetRankingListInfo : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_GetRankingListInfo)]
	public partial class F2C_GetRankingListInfo : IResponse {}

	[Message(FriendsCircleOpcode.RanKingPlayerInfo)]
	public partial class RanKingPlayerInfo {}

//打完一大局总结算 游戏服给亲友圈服发 玩家得分情况
	[Message(FriendsCircleOpcode.S2F_SendTotalPlayerInfo)]
	public partial class S2F_SendTotalPlayerInfo : IMessage {}

//打完一大局总结算 游戏服给亲友圈服发
	[Message(FriendsCircleOpcode.TotalPlayerInfo)]
	public partial class TotalPlayerInfo {}

//修改亲友圈玩法
	[Message(FriendsCircleOpcode.C2F_AlterWanFa)]
	public partial class C2F_AlterWanFa : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_AlterWanFa)]
	public partial class F2C_AlterWanFa : IResponse {}

//获取亲友圈成员列表
	[Message(FriendsCircleOpcode.C2F_GetMemberList)]
	public partial class C2F_GetMemberList : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_GetMemberList)]
	public partial class F2C_GetMemberList : IResponse {}

//修改推荐配置
	[Message(FriendsCircleOpcode.C2F_AlterIsRecommend)]
	public partial class C2F_AlterIsRecommend : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_AlterIsRecommend)]
	public partial class F2C_AlterIsRecommend : IResponse {}

//退出亲友圈
	[Message(FriendsCircleOpcode.C2F_OutFriendsCircle)]
	public partial class C2F_OutFriendsCircle : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_OutFriendsCircle)]
	public partial class F2C_OutFriendsCircle : IResponse {}

//把人踢出亲友圈
	[Message(FriendsCircleOpcode.C2F_KickOutFriendsCircle)]
	public partial class C2F_KickOutFriendsCircle : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_KickOutFriendsCircle)]
	public partial class F2C_KickOutFriendsCircle : IResponse {}

//操作管理权限
	[Message(FriendsCircleOpcode.C2F_ManageJurisdictionOperate)]
	public partial class C2F_ManageJurisdictionOperate : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_ManageJurisdictionOperate)]
	public partial class F2C_ManageJurisdictionOperate : IResponse {}

//修改亲友圈公告
	[Message(FriendsCircleOpcode.C2F_AlterAnnouncement)]
	public partial class C2F_AlterAnnouncement : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_AlterAnnouncement)]
	public partial class F2C_AlterAnnouncement : IResponse {}

//创建亲友圈
	[Message(FriendsCircleOpcode.C2F_CreatorFriendCircle)]
	public partial class C2F_CreatorFriendCircle : IUserRequest {}

	[Message(FriendsCircleOpcode.F2C_CreatorFriendCircle)]
	public partial class F2C_CreatorFriendCircle : IResponse {}

//亲友圈基本信息
	[Message(FriendsCircleOpcode.FriendsCircle)]
	public partial class FriendsCircle {}

}
namespace ETHotfix
{
	public static partial class FriendsCircleOpcode
	{
		 public const ushort C2F_GetSelfFriendsList = 20001;
		 public const ushort F2C_GetSelfFriendsList = 20002;
		 public const ushort C2F_ApplyJoinFriendsCircle = 20003;
		 public const ushort F2C_ApplyJoinFriendsCircle = 20004;
		 public const ushort C2F_GetRecommendFriendsCircle = 20005;
		 public const ushort F2C_GetRecommendFriendsCircle = 20006;
		 public const ushort C2F_GetFriendsCircleInfo = 20007;
		 public const ushort F2C_GetFriendsCircleInfo = 20008;
		 public const ushort C2F_GetFriendsCircleApplyJoinList = 20009;
		 public const ushort F2C_GetFriendsCircleApplyJoinList = 20010;
		 public const ushort C2F_DisposeApplyInfo = 20011;
		 public const ushort F2C_DisposeApplyInfo = 20012;
		 public const ushort C2F_GetRankingListInfo = 20013;
		 public const ushort F2C_GetRankingListInfo = 20014;
		 public const ushort RanKingPlayerInfo = 20015;
		 public const ushort S2F_SendTotalPlayerInfo = 20016;
		 public const ushort TotalPlayerInfo = 20017;
		 public const ushort C2F_AlterWanFa = 20018;
		 public const ushort F2C_AlterWanFa = 20019;
		 public const ushort C2F_GetMemberList = 20020;
		 public const ushort F2C_GetMemberList = 20021;
		 public const ushort C2F_AlterIsRecommend = 20022;
		 public const ushort F2C_AlterIsRecommend = 20023;
		 public const ushort C2F_OutFriendsCircle = 20024;
		 public const ushort F2C_OutFriendsCircle = 20025;
		 public const ushort C2F_KickOutFriendsCircle = 20026;
		 public const ushort F2C_KickOutFriendsCircle = 20027;
		 public const ushort C2F_ManageJurisdictionOperate = 20028;
		 public const ushort F2C_ManageJurisdictionOperate = 20029;
		 public const ushort C2F_AlterAnnouncement = 20030;
		 public const ushort F2C_AlterAnnouncement = 20031;
		 public const ushort C2F_CreatorFriendCircle = 20032;
		 public const ushort F2C_CreatorFriendCircle = 20033;
		 public const ushort FriendsCircle = 20034;
	}
}
