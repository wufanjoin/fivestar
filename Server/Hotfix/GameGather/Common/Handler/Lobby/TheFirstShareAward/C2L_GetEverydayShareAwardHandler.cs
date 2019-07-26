using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 每日分享朋友圈成功 领取奖励
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetEverydayShareAwardHandler : AMRpcHandler<C2L_GetEverydayShareAward, L2C_GetEverydayShareAward>
    {
        protected override async void Run(Session session, C2L_GetEverydayShareAward message, Action<L2C_GetEverydayShareAward> reply)
        {
            L2C_GetEverydayShareAward response = new L2C_GetEverydayShareAward();
            try
            {
                List<EverydayShareInfo>  everydayShareInfos=await GameLobby.Ins.dbProxyComponent.Query<EverydayShareInfo>(every => every.UserId == message.UserId);
                if (everydayShareInfos.Count>0)
                {
                    if (TimeTool.TimeStampIsToday(everydayShareInfos[0].ShareTime))
                    {
                        response.Message = "今日已经领取过奖励";
                        reply(response);
                        return;
                    }
                    everydayShareInfos[0].ShareTime = TimeTool.GetCurrenTimeStamp();
                }
                else
                {
                    EverydayShareInfo everydayShareInfo=ComponentFactory.Create<EverydayShareInfo>();
                    everydayShareInfo.UserId = message.UserId;
                    everydayShareInfo.ShareTime = TimeTool.GetCurrenTimeStamp();
                    everydayShareInfos.Add(everydayShareInfo);
                }
                UserHelp.GoodsChange(message.UserId, GoodsId.Jewel, GameLobby._TheFirstShareAwarNum, GoodsChangeType.EverydayShare, true);
                await GameLobby.Ins.dbProxyComponent.Save(everydayShareInfos[0]);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
