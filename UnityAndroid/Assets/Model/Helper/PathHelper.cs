using System.IO;
using UnityEngine;

namespace ETModel
{
    public static class PathHelper
    {     /// <summary>
        ///应用程序外部资源路径存放路径(热更新资源路径)
        /// </summary>
        public static string AppHotfixResPath
        {
            get
            {
                string game = Application.productName;
                string path = AppResPath;
                if (Application.isMobilePlatform)
                {
                    path = $"{Application.persistentDataPath}/{game}/";
                }
                return path;
            }

        }

        /// <summary>
        /// 存放本地配置目录
        /// </summary>
        public static string LocalConfigPath
        {
            get { return AppHotfixResPath + "/LocalConfig"; }
        }

        /// <summary>
        /// 存放本地配置路径
        /// </summary>
        public static string LocalGameVersionsPath
        {
            get { return LocalConfigPath + "/LocalGameVersions.json"; }
        }


        /// <summary>
        /// 存放安卓冷更新apk路径
        /// </summary>
        public static string ApkSavePath
        {
            get { return Application.persistentDataPath + "/kawuxing.apk"; }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径
        /// </summary>
        public static string AppResPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

        /// <summary>
        /// 应用程序内部资源路径存放路径(www/webrequest专用)
        /// </summary>
        public static string AppResPath4Web
        {
            get
            {
#if UNITY_IOS
                return $"file://{Application.streamingAssetsPath}";
#else
                return Application.streamingAssetsPath;
#endif

            }
        }

        static PathHelper()
        {

            CreateDirectory(LocalConfigPath);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);//如果文件夹不存在就创建它
            }
        }
    }
}
