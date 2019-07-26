using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class FrienCircleRoomItemAwakeSystem : AwakeSystem<FrienCircleRoomItem, GameObject, RoomInfo>
    {
        public override void Awake(FrienCircleRoomItem self, GameObject go, RoomInfo data)
        {
            self.Awake(go, data, UIType.FriendCirclePanel);
        }
    }
    public class FrienCircleRoomItem : BaseItem<RoomInfo>
    {
        private RectTransform mSelfRectTransform;
        private Image mRoomBgImage;
        #region 脚本工具生成的代码
        private Text mWanFaDescText;
        private GameObject mPlayerInfoItemGo;
        private Button mJoinBtn;
        private GameObject mRoomNumberBgGo;
        private Text mRoomNumberText;
        public override void Awake(GameObject go, RoomInfo data, string uiType)
        {
            base.Awake(go, data, uiType);
            mSelfRectTransform = gameObject.GetComponent<RectTransform>();
            mRoomBgImage = gameObject.GetComponent<Image>();
            mWanFaDescText = rc.Get<GameObject>("WanFaDescText").GetComponent<Text>();
            mPlayerInfoItemGo = rc.Get<GameObject>("PlayerInfoItemGo");
            mJoinBtn = rc.Get<GameObject>("JoinBtn").GetComponent<Button>();
            mRoomNumberBgGo = rc.Get<GameObject>("RoomNumberBgGo");
            mRoomNumberText = rc.Get<GameObject>("RoomNumberText").GetComponent<Text>();
            InitPanel();
        }
        #endregion

        public FiveStarRoomConfig _FiveStarRoomConfig;
        public void InitPanel()
        {
            EventMsgMgr.RegisterEvent(CommEventID.FrienCircleCutShowWanFa, FrienCircleCutShowWanFaEvent);
            _FiveStarRoomConfig = FiveStarRoomConfigFactory.Create(mData.RoomConfigLists);
            InitRoomItemInfo();
            mJoinBtn.Add(JoinRoom);
        }
        public static Vector2 RoomNormalWanfaSize=new Vector2(1150,100);
        public static Vector2 RoomFreedomWanfaSize = new Vector2(1150, 133);
        public void InitRoomItemInfo()
        {
            mWanFaDescText.text = _FiveStarRoomConfig.GetWanFaDesc(false);
            mRoomNumberText.text = "房号\n" + mData.RoomId;
            mRoomNumberBgGo.SetActive(mData.RoomId != 0);
            
            
            if (mData.RoomConfigLists.Equals(FrienCircleComponet.Ins.CuurSelectFriendsCircle.DefaultWanFaCofigs))
            {
                WanFaType = FrienCircleWanFaType.Normal;
                mRoomBgImage.sprite = ResourcesComponent.Ins.GetResoure(UIType.FriendCirclePanel, "RoomNormalWanfaBg") as Sprite;
                mSelfRectTransform.sizeDelta = RoomNormalWanfaSize;
                mWanFaDescText.gameObject.SetActive(false);
            }
            else
            {
                WanFaType = FrienCircleWanFaType.Freedom;
                mRoomBgImage.sprite = ResourcesComponent.Ins.GetResoure(UIType.FriendCirclePanel, "RoomFreedomWanfaBg") as Sprite;
                mSelfRectTransform.sizeDelta = RoomFreedomWanfaSize;
                mWanFaDescText.gameObject.SetActive(true);
            }
            InitPlayerInfo();
            FrienCircleCutShowWanFaEvent();//初始化调一遍 玩法显示
        }

        private List<FrienCircleRoomPlayerInfoItem> _PlayerItems = new List<FrienCircleRoomPlayerInfoItem>();
        public void InitPlayerInfo()
        {
            Transform playerItemParent=mPlayerInfoItemGo.transform.parent;
            playerItemParent.CreatorChildCount(_FiveStarRoomConfig.RoomNumber);
            for (int i = 0; i < mData.MatchPlayerInfos.Count; i++)
            {
                FrienCircleRoomPlayerInfoItem frienCircleRoomPlayerInfoItem=playerItemParent.GetChild(i).AddItemIfHaveInit<FrienCircleRoomPlayerInfoItem, MatchPlayerInfo>(mData
                    .MatchPlayerInfos[i]);
                _PlayerItems.Add(frienCircleRoomPlayerInfoItem);
            }
            for (int i = mData.MatchPlayerInfos.Count; i < _FiveStarRoomConfig.RoomNumber; i++)
            {
                playerItemParent.GetChild(i).AddItemIfHaveInit<FrienCircleRoomPlayerInfoItem, MatchPlayerInfo>(null);
            }
        }

        public int WanFaType;//房间是不是默认玩法
        //玩家切换玩法
        public void FrienCircleCutShowWanFaEvent(params object[] objs)
        {
            if (mData == null)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(WanFaType==FrienCircleRoomListView.Ins.CurrSelectWanFa);
        }
        public  void JoinRoom()
        {
            if (0 == mData.RoomId)
            {
                FrienCircleComponet.Ins.CreateRoom(FrienCircleComponet.Ins.CuurSelectFriendsCircle.DefaultWanFaCofigs);
                return;
            }
            CardFiveStarAisle.JoinRoom(mData.RoomId);
        }
        public bool IsMayJoin()
        {
            return _PlayerItems.Count < _FiveStarRoomConfig.RoomNumber;
        }
    }
}
