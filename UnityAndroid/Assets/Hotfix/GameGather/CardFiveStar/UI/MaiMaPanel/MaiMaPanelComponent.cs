using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.MaiMaPanel)]
    public class MaiMaPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private GameObject mMaiMaAnimationGo;
        private GameObject mMaiMaCardPointGo;
        private Text mSocreText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mMaiMaAnimationGo = rc.Get<GameObject>("MaiMaAnimationGo");
            mMaiMaCardPointGo = rc.Get<GameObject>("MaiMaCardPointGo");
            mSocreText = rc.Get<GameObject>("SocreText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
        }

        private CardFiveStarCard _MaiMaCard;
        public async Task ShowMaiMaCard(int card,int score)
        {
            Show();
            mSocreText.text = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                await ShowOneAnim();
            }
            if (_MaiMaCard == null)
            {
                _MaiMaCard = CardFiveStarCardPool.Ins.Create(CardFiveStarCardType.Down_ZhiLi_ZhengMain, card,
                    mMaiMaCardPointGo.transform, 0.8f);
                _MaiMaCard.LocalPositionZero();
            }
            else
            {
                _MaiMaCard.SetCardUI(card);
            }
            mSocreText.text = "J" + score;
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(1000);
             Hide();
            _MaiMaCard.SetActive(false);
        }

        public async Task ShowOneAnim()
        {
            mMaiMaAnimationGo.transform.GetChild(0).gameObject.SetActive(true);
            for (int i = 1; i < mMaiMaAnimationGo.transform.childCount; i++)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                mMaiMaAnimationGo.transform.GetChild(i - 1).gameObject.SetActive(false);
                mMaiMaAnimationGo.transform.GetChild(i).gameObject.SetActive(true);
            }
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
            mMaiMaAnimationGo.HideAllChild();
        }

        public override void MaskClickEvent()
        {
        }
    }
}
