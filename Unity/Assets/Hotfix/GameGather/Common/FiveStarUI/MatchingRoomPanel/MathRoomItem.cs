using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class MathRoomItemAwakeSystem : AwakeSystem<MathRoomItem, GameObject, MatchRoomConfig>
    {
        public override void Awake(MathRoomItem self, GameObject go, MatchRoomConfig data)
        {
            self.Awake(go, data, UIType.MatchingRoomPanel);
        }
    }
    public class MathRoomItem : BaseItem<MatchRoomConfig>
    {
        #region 脚本工具生成的代码
        private Text mBottomScoreText;
        private Text mLimitingText;
        private Text mBottomScoreDescText;
        private Image mBgImage;
        private Button mItemButton;
        public override void Awake(GameObject go, MatchRoomConfig data, string uiType)
        {
            base.Awake(go, data, uiType);
            mBgImage = gameObject.GetComponent<Image>();
            mBottomScoreText = rc.Get<GameObject>("BottomScoreText").GetComponent<Text>();
            mLimitingText = rc.Get<GameObject>("LimitingText").GetComponent<Text>();
            mBottomScoreDescText = rc.Get<GameObject>("BottomScoreDescText").GetComponent<Text>();
            mItemButton = gameObject.GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mItemButton.Add(EnterRoom);
            if (mData.Name.Contains("初级场"))
            {
                mBgImage.sprite = GetResoure<Sprite>("matchingpanel_5");
            }
            else if (mData.Name.Contains("中级场"))
            {
                mBgImage.sprite = GetResoure<Sprite>("matchingpanel_7");
            }
            else if (mData.Name.Contains("高级场"))
            {
                mBgImage.sprite = GetResoure<Sprite>("matchingpanel_6");
            }
            mBgImage.SetNativeSize();
            mBottomScoreText.text =mData.BaseScore+"分";
            mLimitingText.text = "限制:" + mData.BesansLowest;
            mBottomScoreDescText.text= "底分:" + mData.BaseScore;
        }

        public void EnterRoom()
        {
            CardFiveStarAisle.MatchingEnterRoom(mData.MatchRoomId,mData.RoomConfigs);
        }
    }
}
