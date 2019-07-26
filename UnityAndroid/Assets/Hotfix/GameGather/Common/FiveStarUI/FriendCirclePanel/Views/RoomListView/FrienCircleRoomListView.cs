using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class FrienCircleWanFaType
    {
        public const int Normal = 1;//正常玩法
        public const int Freedom = 2;//自由玩法
    }
    public class FrienCircleRoomListView : BaseView
    {
        #region 脚本工具生成的代码
        private Toggle mNormalWanFaToggle;
        private Toggle mFreedomWanFaToggle;
        private GameObject mWanFaDescBgGo;
        private GameObject mRoomItemGo;
        private GameObject mNoneRoomGo;
        private Button mFastJoinBtn;
        private Button mCreateRoomBtn;
        private Button mRecordPerformanceBtn;
        private Button mRefreshBtn;
        private Text mWanFaDescText;
        private Text mAnnouncementText;
        private Text mNormalWanFaText;
        private Text mWanFaText;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mNormalWanFaToggle = rc.Get<GameObject>("NormalWanFaToggle").GetComponent<Toggle>();
            mFreedomWanFaToggle = rc.Get<GameObject>("FreedomWanFaToggle").GetComponent<Toggle>();
            mWanFaDescBgGo = rc.Get<GameObject>("WanFaDescBgGo");
            mRoomItemGo = rc.Get<GameObject>("RoomItemGo");
            mNoneRoomGo = rc.Get<GameObject>("NoneRoomGo");
            mFastJoinBtn = rc.Get<GameObject>("FastJoinBtn").GetComponent<Button>();
            mCreateRoomBtn = rc.Get<GameObject>("CreateRoomBtn").GetComponent<Button>();
            mRecordPerformanceBtn = rc.Get<GameObject>("RecordPerformanceBtn").GetComponent<Button>();
            mRefreshBtn = rc.Get<GameObject>("RefreshBtn").GetComponent<Button>();
            mAnnouncementText = rc.Get<GameObject>("AnnouncementText").GetComponent<Text>();
            mWanFaDescText = rc.Get<GameObject>("WanFaDescText").GetComponent<Text>();
            mNormalWanFaText = rc.Get<GameObject>("NormalWanFaText").GetComponent<Text>();
            mWanFaText = rc.Get<GameObject>("WanFaText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public static FrienCircleRoomListView Ins { get; private set; }
        private BroadcastPlay _broadcastPlay = new BroadcastPlay();
        public void InitPanel()
        {
            Ins = this;

            mNormalWanFaToggle.Add(NormalWanFaToggleEvent);//正常房间
            mFreedomWanFaToggle.Add(FreedomWanFaToggleEvent);//自由创建的房间
            mRecordPerformanceBtn.Add(RecordPerformanceBtnEvent);//DOTO 战绩面板待实现
            mCreateRoomBtn.Add(CreateRoomBtnEvent);//创建房间
            mFastJoinBtn.Add(FastJoinBtnEvent);//快速加入
            mRefreshBtn.Add(RefreshBtnEvent);//刷新按钮 就是刷新房间列表

            _broadcastPlay.RegisterBroadcast(mAnnouncementText, FrienCircleComponet.Ins.CuurSelectFriendsCircle.Announcement);//公告赋值
            mNormalWanFaToggle.isOn = true;//默认显示 默认玩法
        }
        //刷新按钮 刷一下房间列表
        public void RefreshBtnEvent()
        {
            RefreshRoomList();
        }
        public void RecordPerformanceBtnEvent()
        {
            UIComponent.GetUiView<MilitaryPanelComponent>().ShowMilitary(UserComponent.Ins.pUserId, FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId);
        }
        //快速加入按钮
        public void FastJoinBtnEvent()
        {
            foreach (var roomItem in frienCircleRoomItems)
            {
                if (roomItem.IsMayJoin())
                {
                    roomItem.JoinRoom();
                    return;
                }
            }
        }

        //创建房间按钮
        public void CreateRoomBtnEvent()
        {
            UIComponent.GetUiView<CreateRoomPanelComponent>().ShowCraterRoomPanel(ShowCraterRoomPanelType.NormalCraterRoom, FrienCircleComponet.Ins.CreateRoom);
        }


        public int CurrSelectWanFa = FrienCircleWanFaType.Normal;
        public void NormalWanFaToggleEvent(bool isOn)
        {
            if (isOn)
            {
                CurrSelectWanFa = FrienCircleWanFaType.Normal;
                EventMsgMgr.SendEvent(CommEventID.FrienCircleCutShowWanFa);
                mWanFaDescBgGo.SetActive(true);
                mNoneRoomGo.SetActive(false);
            }
        }
        public void FreedomWanFaToggleEvent(bool isOn)
        {
            if (isOn)
            {
                CurrSelectWanFa = FrienCircleWanFaType.Freedom;
                EventMsgMgr.SendEvent(CommEventID.FrienCircleCutShowWanFa);
                mWanFaDescBgGo.SetActive(false);
                mNoneRoomGo.SetActive(true);
                for (int i = 0; i < mRoomItemGo.transform.parent.childCount; i++)
                {
                    if (mRoomItemGo.transform.parent.GetChild(i).gameObject.activeInHierarchy)
                    {
                        mNoneRoomGo.SetActive(false);
                        break;
                    }
                }
            }
        }

        public override void Show()
        {
            base.Show();
            RefreshRoomList();
        }

        private List<FrienCircleRoomItem> frienCircleRoomItems = new List<FrienCircleRoomItem>();
        //属性房间列表
        private async void RefreshRoomList()
        {
            mAnnouncementText.text = FrienCircleComponet.Ins.CuurSelectFriendsCircle.Announcement;
            _broadcastPlay.PlayBroadcast();//让公告滚动起来
            if (FrienCircleComponet.Ins.CuurSelectFriendsCircle == null)
            {
                return;
            }
            for (int i = 0; i < frienCircleRoomItems.Count; i++)
            {
                frienCircleRoomItems[i].mData = null;
            }
            frienCircleRoomItems.Clear();
            mWanFaDescText.text = FrienCircleComponet.Ins.CuurSelectFriendsCircle.GetDefaultWanFaConfigInfoasd().GetWanFaDesc(false);
            mNormalWanFaText.text = $"卡五星({FrienCircleComponet.Ins.CuurSelectFriendsCircle.GetDefaultWanFaConfigInfoasd().RoomNumber}人)";
            mWanFaText.text = mNormalWanFaText.text;

            M2C_GetFriendsCircleRoomList f2CGetFriendsCircleRooms = (M2C_GetFriendsCircleRoomList)await SessionComponent.Instance.Call(new C2M_GetFriendsCircleRoomList()
            {
                FriendsCircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId
            });
            Transform roomPatentTrm = mRoomItemGo.transform.parent;
            //显示默认创建房间玩法
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.RoomConfigLists.Add(FrienCircleComponet.Ins.CuurSelectFriendsCircle.DefaultWanFaCofigs); 
            Debug.Log(FrienCircleComponet.Ins.CuurSelectFriendsCircle.DefaultWanFaCofigs[1]);
            f2CGetFriendsCircleRooms.RoomInfos.Add(roomInfo);
            frienCircleRoomItems = roomPatentTrm.CreatorChildAndAddItem<FrienCircleRoomItem, RoomInfo>(f2CGetFriendsCircleRooms.RoomInfos);
            //显示默认玩法房间
           // ShowDefaultWanFaRoom();
        }


        //显示默认玩法房间
        public void ShowDefaultWanFaRoom()
        {

            GameObject roomItem = GameObject.Instantiate(mRoomItemGo, mRoomItemGo.transform.parent);
            //显示默认创建房间玩法
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.RoomConfigLists = FrienCircleComponet.Ins.CuurSelectFriendsCircle.DefaultWanFaCofigs;
            FrienCircleRoomItem defaultWanfaRoom = roomItem.AddItemIfHaveInit<FrienCircleRoomItem, RoomInfo>(roomInfo);
            frienCircleRoomItems.Add(defaultWanfaRoom);
            defaultWanfaRoom.gameObject.transform.SetAsFirstSibling();
            defaultWanfaRoom.mData.RoomConfigLists = FrienCircleComponet.Ins.CuurSelectFriendsCircle.DefaultWanFaCofigs;

        }
    }
}
