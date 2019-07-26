

using System;
using System.Reflection;
using ETModel;

namespace ETHotfix
{
    [Config((int)AppType.ClientH)]
    public partial class AnnouncementConfigCategory : ACategory<AnnouncementConfig>
    {
    }

    public class AnnouncementConfig : IConfig
    {
        public const int NormalAnnouncementType = 1;
        public const int ForbidGambleType = 2;
        public long Id { get; set; }
        public string Name;
        public string Title;
        public string Content;
        public int Type;
    }
}
