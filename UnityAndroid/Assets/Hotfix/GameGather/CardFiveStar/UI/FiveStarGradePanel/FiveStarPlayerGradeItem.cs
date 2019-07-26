using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FiveStarPlayerGradeItemAwakeSystem : AwakeSystem<FiveStarPlayerGradeItem, GameObject, CardFiveStarPlayer>
    {
        public override void Awake(FiveStarPlayerGradeItem self, GameObject go, CardFiveStarPlayer data)
        {
            self.Awake(go, data, UIType.FiveStarGradePanel);
        }
    }

    public class FiveStarPlayerGradeItem : BaseItem<CardFiveStarPlayer>
    {
        #region 脚本工具生成的代码
        private Text mZiMoNumText;
        private Text mFangChongNumText;
        private Text mHuCardNumText;
        private Text mTotalScoreText;
        private Image mHeadImage;
        private Text mNameText;
        private Text mIDText;
        private Text mGangCardNumText;
        public override void Awake(GameObject go, CardFiveStarPlayer data, string uiType)
        {
            base.Awake(go, data, uiType);
            mZiMoNumText = rc.Get<GameObject>("ZiMoNumText").GetComponent<Text>();
            mFangChongNumText = rc.Get<GameObject>("FangChongNumText").GetComponent<Text>();
            mHuCardNumText = rc.Get<GameObject>("HuCardNumText").GetComponent<Text>();
            mTotalScoreText = rc.Get<GameObject>("TotalScoreText").GetComponent<Text>();
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIDText = rc.Get<GameObject>("IDText").GetComponent<Text>();
            mGangCardNumText = rc.Get<GameObject>("GangCardNumText").GetComponent<Text>();
            InitPanel();
        }
        #endregion

        public  void InitPanel()
        {
        }
        public async void SetUI(CardFiveStarPlayer player)
        {
            Show();
            mData = player;
            mZiMoNumText.text = player._ZiMoCount.ToString();
            mFangChongNumText.text = player._FangChongCount.ToString();
            mHuCardNumText.text = player._HuPaiCount.ToString();
            mGangCardNumText.text = player._GangPaiCount.ToString();
            mTotalScoreText.text = player._NowSocre.ToString();

            mHeadImage.sprite = await player._user.GetHeadSprite();
            mIDText.text = "ID:"+player._user.UserId;
            mNameText.text = player._user.Name;
        }
        
    }
}
