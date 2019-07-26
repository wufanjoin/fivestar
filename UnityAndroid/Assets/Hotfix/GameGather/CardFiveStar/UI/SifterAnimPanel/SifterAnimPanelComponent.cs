using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.SifterAnimPanel)]
    public class SifterAnimPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码

        private Image mAnimImage;
        private Image mSifterAImage;
        private Image mSifterBImage;
        private List<Sprite> _sifterSprites=new List<Sprite>();
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mAnimImage = rc.Get<GameObject>("AnimImage").GetComponent<Image>();
            mSifterAImage = rc.Get<GameObject>("SifterAImage").GetComponent<Image>();
            mSifterBImage = rc.Get<GameObject>("SifterBImage").GetComponent<Image>();
            for (int i = 1; i < 18; i++)
            {
                _sifterSprites.Add(rc.Get<Sprite>("sifter_"+i));
            }
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            
        }

        public async ETTask RandomPlaySifterNum()
        {
           await PlaySifterAnim(RandomTool.Random(1, 7), RandomTool.Random(1, 7));
        }
        public async ETTask PlaySifterAnim(int sifterA,int siferB)
        {
            Show();
            CardFiveStarSoundMgr.Ins.PlaySound(SexType.None, FiveStarOperateType.ZhiSeZi);
            mAnimImage.gameObject.SetActive(true);
            mSifterAImage.gameObject.SetActive(false);
            mSifterBImage.gameObject.SetActive(false);
            for (int i = 0; i < 11; i++)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                mAnimImage.sprite = _sifterSprites[i];
                mAnimImage.SetNativeSize();
            }
            mAnimImage.gameObject.SetActive(false);
            mSifterAImage.gameObject.SetActive(true);
            mSifterBImage.gameObject.SetActive(true);
            mSifterAImage.sprite = _sifterSprites[sifterA + 10];
            mSifterAImage.SetNativeSize();
            mSifterBImage.sprite = _sifterSprites[siferB + 10];
            mSifterBImage.SetNativeSize();
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
            Hide();
        }
    }
}
