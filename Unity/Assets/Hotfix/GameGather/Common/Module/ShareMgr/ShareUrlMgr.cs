using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
  public  class ShareUrlMgr 
  {
      public const string HomeUrl = @"http://kwx11.com";

      public const string ShareTitle = "【卡五星】";

      public const string ShareContent = "卡五星麻将火爆上线,点击下载,4人玩法更符合当地玩法,盘满钵盈笑开怀!";

        //正常分享
      public static void NormalShare(int shareType)
      {
          SdkMgr.Ins.WeChatShareUrl(HomeUrl, ShareTitle, ShareContent, shareType);
      }

        //分享房间
      public static void RoomShare(int roomId, FiveStarRoomConfig config, int existingNumber)
      {
          string url = HomeUrl + "?roomId=" + roomId;
          string title = $"{ShareTitle}  房号-{roomId},已有（{existingNumber}/{config.RoomNumber}）人";
          string desc = config.GetWanFaDesc(true,roomId);
          SdkMgr.Ins.WeChatShareUrl(url, title, desc, WxShareSceneType.Friend);
      }

      public const string videoShareContent = "分享了精彩回放,点击战绩-观看他人回放输入回放码可查看精彩回放。";
        //分享录像
      public static void VideoShare(int videoId)
      {
          string title = ShareTitle + "  回放码:" + videoId;
          string desc = $"【{UserComponent.Ins.pSelfUser.Name}】" + videoShareContent;
          SdkMgr.Ins.WeChatShareUrl(HomeUrl, title, desc, WxShareSceneType.Friend);
        }
  }
}
