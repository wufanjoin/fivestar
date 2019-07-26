using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FrienCircleRoomPlayerInfoItemAwakeSystem : AwakeSystem<FrienCircleRoomPlayerInfoItem, GameObject, MatchPlayerInfo>
    {
        public override void Awake(FrienCircleRoomPlayerInfoItem self, GameObject go, MatchPlayerInfo data)
        {
            self.Awake(go, data, UIType.FriendCirclePanel);
        }
    }
    public class FrienCircleRoomPlayerInfoItem : BaseItem<MatchPlayerInfo>
    {
        #region 脚本工具生成的代码
        private Image mHeadImage;
        private GameObject mAwaitHintGo;
        private Text mNameText;
        private Text mIdText;
        public override void Awake(GameObject go, MatchPlayerInfo data, string uiType)
        {
            base.Awake(go, data, uiType);
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mAwaitHintGo = rc.Get<GameObject>("AwaitHintGo");
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public async void InitPanel()
        {
            mAwaitHintGo.SetActive(mData == null);
            mNameText.gameObject.SetActive(mData != null);
            mIdText.gameObject.SetActive(mData != null);
            mHeadImage.gameObject.SetActive(mData != null);
            if (mData == null)
            {
                return;
            }
            mHeadImage.sprite = await mData.User.GetHeadSprite();
            mNameText.text = mData.User.Name;
            mIdText.text ="ID:"+mData.User.UserId;
        }
    }
}
