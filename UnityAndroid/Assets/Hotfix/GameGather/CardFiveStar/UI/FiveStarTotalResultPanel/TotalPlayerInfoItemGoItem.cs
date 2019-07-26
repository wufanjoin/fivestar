using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class TotalPlayerInfoItemGoItemAwakeSystem : AwakeSystem<TotalPlayerInfoItemGoItem, GameObject, FiveStarTotalPlayerResult>
    {
        public override void Awake(TotalPlayerInfoItemGoItem self, GameObject go, FiveStarTotalPlayerResult data)
        {
            self.Awake(go, data, UIType.FiveStarTotalResultPanel);
        }
    }
    public class TotalPlayerInfoItemGoItem : BaseItem<FiveStarTotalPlayerResult>
    {
        #region 脚本工具生成的代码
        private GameObject mBigWinLightGo;
        private Image mHeadImage;
        private Text mNameText;
        private Text mIDText;
        private GameObject mBigWinTextGo;
        private GameObject mHouseIconGo;
        private GameObject mOptimumPaoShouGo;
        private Text mTotalSocreText;
        private Text mZiMoNumText;
        private Text mFangChongNumText;
        private Text mGangPaiNumText;
        private Text mHuPaiNumText;
        public override void Awake(GameObject go, FiveStarTotalPlayerResult data, string uiType)
        {
            base.Awake(go, data, uiType);
            mBigWinLightGo = rc.Get<GameObject>("BigWinLightGo");
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIDText = rc.Get<GameObject>("IDText").GetComponent<Text>();
            mBigWinTextGo = rc.Get<GameObject>("BigWinTextGo");
            mHouseIconGo = rc.Get<GameObject>("HouseIconGo");
            mOptimumPaoShouGo = rc.Get<GameObject>("OptimumPaoShouGo");
            mTotalSocreText = rc.Get<GameObject>("TotalSocreText").GetComponent<Text>();
            mZiMoNumText = rc.Get<GameObject>("ZiMoNumText").GetComponent<Text>();
            mFangChongNumText = rc.Get<GameObject>("FangChongNumText").GetComponent<Text>();
            mGangPaiNumText = rc.Get<GameObject>("GangPaiNumText").GetComponent<Text>();
            mHuPaiNumText = rc.Get<GameObject>("HuPaiNumText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public  void InitPanel()
        {
        }

        public void SetUI(FiveStarTotalPlayerResult result)
        {
            Show();
            mData = result;
            ShowUserInfo();
            ShowSocre();
        }
        //显示用户信息
        private async void ShowUserInfo()
        {
            CardFiveStarPlayer cardPlayer = CardFiveStarRoom.Ins.GetFiveStarPlayer(mData.SeatIndex);
            //名字
            mNameText.text = cardPlayer._user.Name;
            //ID
            mIDText.text = "ID:" + cardPlayer._user.UserId;
            //头像
            mHeadImage.sprite =await cardPlayer._user.GetHeadSprite();
            //房主图标
            //mHouseIconGo.SetActive(cardPlayer._user.UserId == CardFiveStarRoom.Ins._RoomMasterUserId);
            mHouseIconGo.SetActive(false);
            //大赢家
            mBigWinLightGo.SetActive(UIComponent.GetUiView<FiveStarTotalResultPanelComponent>().bigWinSeatIndex == mData.SeatIndex);
            mBigWinTextGo.SetActive(UIComponent.GetUiView<FiveStarTotalResultPanelComponent>().bigWinSeatIndex == mData.SeatIndex);
            //最佳炮手
            mOptimumPaoShouGo.SetActive(UIComponent.GetUiView<FiveStarTotalResultPanelComponent>().PaoShouSeatIndex == mData.SeatIndex);
        }
        //显示分数
        private void ShowSocre()
        {
            mTotalSocreText.text = mData.TotalSocre.ToString();
            mZiMoNumText.text = mData.ZiMoCount.ToString();
            mFangChongNumText.text = mData.FangChongCount.ToString();
            mGangPaiNumText.text = mData.GangPaiCount.ToString();
            mHuPaiNumText.text = mData.HuPaiCount.ToString();
        }
    }
}
