using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
 public  static class GeneralizeComponentSystem
    {
        public static async Task GetGreenGift(this GeneralizeComponent generalizeComponent,long getUserId, long initeUserId, IResponse iResponse)
        {
            if (getUserId == initeUserId)
            {
                iResponse.Message = "邀请人不能是自己";
                return;
            }
            if (await generalizeComponent.GreenGiftGetStatu(getUserId))
            {
                iResponse.Message = "已经领取过礼包";
                return;
            }
            User user=await UserHelp.QueryUserInfo(initeUserId);
            if (user == null)
            {
                iResponse.Message = "邀请码不对";
                return;
            }
            //存储个领取新手奖励信息
            GetGreenGiftInfo getGreenGiftInfo = ComponentFactory.Create<GetGreenGiftInfo>();
            getGreenGiftInfo.GetUserId = getUserId;
            getGreenGiftInfo.InviteUserId = initeUserId;
            getGreenGiftInfo.GetJewelNum = GeneralizeComponent.AwardJewelNum;
            await generalizeComponent.dbProxyComponent.Save(getGreenGiftInfo);

            //存储推广奖励信息
            GeneralizeAwardInfo  generalizeAwardInfo=await generalizeComponent.GetGeneralizeAwardInfo(initeUserId);
            if (generalizeAwardInfo == null)
            {
                generalizeAwardInfo = CreateGeneralizeAwardInfo(initeUserId);
            }
            generalizeAwardInfo.GeneralizeNumber++;
            generalizeAwardInfo.GetJewelTotalNum += GeneralizeComponent.AwardJewelNum;
            await generalizeComponent.dbProxyComponent.Save(generalizeAwardInfo);

            UserHelp.GoodsChange(getUserId, GoodsId.Jewel, GeneralizeComponent.AwardJewelNum, GoodsChangeType.GeneralizeAward,true);
            UserHelp.GoodsChange(initeUserId, GoodsId.Jewel, GeneralizeComponent.AwardJewelNum, GoodsChangeType.GeneralizeAward);
        }

        //获取新手礼包领取状态
        public static async Task<bool> GreenGiftGetStatu(this GeneralizeComponent generalizeComponent, long userId)
        {
            List<GetGreenGiftInfo> giftInfo = await generalizeComponent.dbProxyComponent.Query<GetGreenGiftInfo>(
                (getGreenGift) => getGreenGift.GetUserId == userId);
            return giftInfo.Count > 0;
        }
        //获取推广奖励信息
        public static async Task<GeneralizeAwardInfo> GetGeneralizeAwardInfo(this GeneralizeComponent generalizeComponent, long userId)
        {
            List<GeneralizeAwardInfo> giftInfo = await generalizeComponent.dbProxyComponent.Query<GeneralizeAwardInfo>(
                (generlie) => generlie.UserId == userId);
            if (giftInfo.Count > 0)
            {
                return giftInfo[0];
            }
            else
            {
                return null;
            }
        }
        //创建推广奖励信息
        private static GeneralizeAwardInfo CreateGeneralizeAwardInfo(long userId)
        {
            GeneralizeAwardInfo generalizeAwardInfo = ComponentFactory.Create<GeneralizeAwardInfo>();
            generalizeAwardInfo.UserId = userId;
            return generalizeAwardInfo;
        }
    }
}
