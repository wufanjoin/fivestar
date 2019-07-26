using System.IO;

namespace ETModel
{
	public class GlobalProto
	{
		public string AssetBundleServerUrl;
		public string Address;

		public string GetUrl()
		{
			string url = this.AssetBundleServerUrl;
            //#if UNITY_EDITOR
            //	        url += "PC/";
#if UNITY_ANDROID
            url += "Android/";
#elif UNITY_IOS
            url += "IOS/";
#elif UNITY_WEBGL
			url += "WebGL/";
#elif UNITY_STANDALONE_OSX
			url += "MacOS/";
#else
            url += "PC/";
#endif
            return url;
		}

        //获取游戏版本配置连接
	    public string GetGameVersionsUrl()
	    {
	        return Path.Combine(this.AssetBundleServerUrl, "GameVersions.json");
        }


    }
}
