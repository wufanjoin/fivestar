using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
   public static class LoadResoure
   {
       private static Dictionary<string, Sprite> _DownFinshDic = new Dictionary<string, Sprite>();
        public static async ETTask<Sprite> LoadSprite(string url,Action<Sprite> loadCall=null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                Log.Error("下载图像错误Url为空");
                return null;
            }
            if (_DownFinshDic.ContainsKey(url))
            {
                return _DownFinshDic[url];
            }
            WWW www=new WWW(url);
            while (!www.isDone)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(500);
            }
            if (string.IsNullOrEmpty(www.error))
            {
                Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
                loadCall?.Invoke(sprite);
                _DownFinshDic[url] = sprite;
                return sprite;
            }
            else
            {
                Log.Error("下载错误"+ www.error);
                loadCall?.Invoke(null);
                return null;
            }
        }
    }
}
