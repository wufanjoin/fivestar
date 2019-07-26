

namespace ETModel
{
    //[Config(AppType.ClientM| AppType.ClientH)]
    //public partial class GameVersionsConfigCategory : ACategory<GameVersionsConfig>
    //{
    //}

    public class ToyGameVersions //: IConfig
    {
        public long Id { get; set; }
        public string Name;
        public double Version;
        public string DownloadFolder;
        public string VersionFile;
       // public bool IsNeedRenewal = true;
    }
}
