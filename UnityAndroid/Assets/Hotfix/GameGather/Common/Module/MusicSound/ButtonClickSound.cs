using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
 public static class ButtonClickSound
 {
     private static AudioClip clickSound;
        public static void RegisterPlayButtonSound()
        {
            clickSound = ResourcesComponent.Ins.GetResoure("common", "clickbtnsound") as AudioClip;
            ButtonAnimationEffect.ButtonPointerUpAction -= PlayClickSound;
            ButtonAnimationEffect.ButtonPointerUpAction += PlayClickSound;
        }

        private static void PlayClickSound()
        {
            MusicSoundComponent.Ins.PlaySound(clickSound);
        }
    }
}
