using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;


namespace ETModel
{
    public class GameVersionsConfigMgr : Single<GameVersionsConfigMgr>
    {
        internal  GameVersionsConfig LocalGameVersionsConfig { private set; get; }//热更项目 不能直接访问 会报错
        internal GameVersionsConfig ServerGameVersionsConfig { private set; get; }//热更项目 不能直接访问 会报错

        public float Version
        {
            get
            {
                if (LocalGameVersionsConfig == null)
                {
                    return 0;
                }
                return (float)LocalGameVersionsConfig.Version;
            }
        }

        public string ServerAddress
        {
            get
            {
                if (ServerGameVersionsConfig == null)
                {
                    return GlobalConfigComponent.Instance.GlobalProto.Address;
                }
                return ServerGameVersionsConfig.ServerAddress;
            }
        }
        public async ETTask InitVersionsConfig()
        {
            await InitServerVersions();//先初始化服务器版本信息
            InitLocalVersions();//在初始化本地版本信息
        }
        //初始化服务器版本配置信息
        public async ETTask InitServerVersions()
        {
            using (UnityWebRequestAsync webRequestAsync = ComponentFactory.Create<UnityWebRequestAsync>())
            {
                string serverGameVersionsUrl =GlobalConfigComponent.Instance.GlobalProto.GetGameVersionsUrl();
                await webRequestAsync.DownloadAsync(serverGameVersionsUrl);
                ServerGameVersionsConfig = JsonMapper.ToObject<GameVersionsConfig>(webRequestAsync.Request.downloadHandler.text);
            }
        }

        public bool IsHaveLocalRes = false;//是否有本地资源
        //初始化本地版本配置信息
        public  void InitLocalVersions()
        {
            //如果本地有配置就读表
            if (File.Exists(PathHelper.LocalGameVersionsPath))
            {
                LocalGameVersionsConfig = JsonMapper.ToObject<GameVersionsConfig>(File.ReadAllText(PathHelper.LocalGameVersionsPath));
                IsHaveLocalRes = true;
            }
            else
            {
                LocalGameVersionsConfig = JsonMapper.ToObject<GameVersionsConfig>(JsonMapper.ToJson(ServerGameVersionsConfig));//这就是复制了一下对象
                LocalGameVersionsConfig.Version =double.Parse(Application.version);//版本就是游戏版本
                for (int i = 0; i < LocalGameVersionsConfig.ToyGameVersionsArray.Count; i++)
                {
                    LocalGameVersionsConfig.ToyGameVersionsArray[i].Version = 0;//是不会带其他游戏的
                }
                IsHaveLocalRes = false;
            }
           
        }
        //更新完成 存储服务器版本信息到本地 
        public void SaveServerVersionsToLocal()
        {
            PathHelper.CreateDirectory(PathHelper.LocalConfigPath);//创建本地存储目录
            LocalGameVersionsConfig = ServerGameVersionsConfig;//本地版本 等于服务器版本
            File.WriteAllText(PathHelper.LocalGameVersionsPath, JsonMapper.ToJson(ServerGameVersionsConfig));//存储到本地

        }
    }
}
