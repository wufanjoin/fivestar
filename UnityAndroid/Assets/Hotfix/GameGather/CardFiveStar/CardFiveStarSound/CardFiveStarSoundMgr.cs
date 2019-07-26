using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
  public  class CardFiveStarSoundMgr:Single<CardFiveStarSoundMgr>

    {
        public override void Init()
        {
            base.Init();
            ResourcesComponent.Ins.AwayPrefixLoadBundle(UIType.CardFiveStarSound+".unity3d");
        }

        //播放音效
        private Dictionary<string, AudioClip> _LoadSoundDic=new Dictionary<string, AudioClip>();
        public void PlaySound(int sex,int oprateType,int cardSize=0)
        {
            string soundResName = "";
            switch (sex)
            {
                case SexType.Man:
                    soundResName += "man_";
                    break;
                case SexType.WoMan:
                    soundResName += "woman_";
                    break;
                default:
                    soundResName += "woman_";
                    break;
            }
            switch (oprateType)
            {
                case FiveStarOperateType.Peng:
                    soundResName += "peng";
                    break;
                case FiveStarOperateType.MingGang:
                case FiveStarOperateType.AnGang:
                case FiveStarOperateType.CaGang:
                    soundResName += "gang";
                    break;
                case FiveStarOperateType.FangChongHu:
                case FiveStarOperateType.ZiMo:
                    soundResName += "hu";
                    break;
                case FiveStarOperateType.Liang:
                    soundResName += "liang";
                    break;
                case FiveStarOperateType.MoCard:
                    soundResName = "mopai";
                    break;
                case FiveStarOperateType.ChuCard:
                    soundResName += cardSize;
                    break;
                case FiveStarOperateType.ChuCardFall:
                    soundResName = "dapai";
                    break;
                case FiveStarOperateType.ClickCard:
                    soundResName = "dianpai";
                    break;
                case FiveStarOperateType.GameStart:
                    soundResName = "gamestart";
                    break;
                case FiveStarOperateType.ZhiSeZi:
                    soundResName = "zhisezi";
                    break;
            }
            AudioClip soundClip;
            if (_LoadSoundDic.ContainsKey(soundResName))
            {
                soundClip = _LoadSoundDic[soundResName];
            }
            else
            {
                soundClip = ResourcesComponent.Ins.GetResoure(UIType.CardFiveStarSound, soundResName) as AudioClip;
                _LoadSoundDic.Add(soundResName, soundClip);
            }
          
            MusicSoundComponent.Ins.PlaySound(soundClip);
        }
    }
}
