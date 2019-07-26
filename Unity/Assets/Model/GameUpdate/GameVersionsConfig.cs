using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
  public  class GameVersionsConfig
  {
      public bool IsServerNormal;//服务器是否正常运行
      public string MaintainAnnouncement;//维护公告
      public string ServerAddress;//服务器地址
      public string ApkDownloadUrl;
      public string IOSDownloadUrl;
      public double Version;//游戏版本
      public double ColdUpdateVersion;//冷更新版本
      public List<ToyGameVersions> ToyGameVersionsArray;
  }
}
