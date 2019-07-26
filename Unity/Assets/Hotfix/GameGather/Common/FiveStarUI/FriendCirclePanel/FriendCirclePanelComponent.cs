using System;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.FriendCirclePanel)]
    public class FriendCirclePanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private GameObject mOptionLisViewGo;
        private Button mCloseBtn;
        private GameObject mViewPointGo;
        private GameObject mTopViewGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mOptionLisViewGo = rc.Get<GameObject>("OptionLisViewGo");
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mViewPointGo = rc.Get<GameObject>("ViewPointGo");
            mTopViewGo = rc.Get<GameObject>("TopViewGo");
            InitPanel();
        }
        #endregion

        private FrienCircleRoomListView _RoomListView;

        public FrienCircleRoomListView RoomListView
        {
            get
            {
                if (_RoomListView == null)
                {
                    _RoomListView=GameObject.Instantiate(GetResoure<GameObject>("RoomListView"), mViewPointGo.transform)
                        .AddItem<FrienCircleRoomListView>();
                }
                return _RoomListView;
            }
        }
        private FrienCircleRequestListView _RequestListView;
        public FrienCircleRequestListView RequestListView
        {
            get
            {
                if (_RequestListView == null)
                {
                    _RequestListView = GameObject.Instantiate(GetResoure<GameObject>("RequestListView"), mViewPointGo.transform)
                        .AddItem<FrienCircleRequestListView>();
                }
                return _RequestListView;
            }
        }
        private FrienCircleRankingListView _RankingListView;
        public FrienCircleRankingListView RankingListView
        {
            get
            {
                if (_RankingListView == null)
                {
                    _RankingListView = GameObject.Instantiate(GetResoure<GameObject>("RankingListView"), mViewPointGo.transform)
                        .AddItem<FrienCircleRankingListView>();
                }
                return _RankingListView;
            }
        }
        private NoneFriendCircleView _NoneFriendCircleView;
        public NoneFriendCircleView NoneFriendCircleView
        {
            get
            {
                if (_NoneFriendCircleView == null)
                {
                    _NoneFriendCircleView = GameObject.Instantiate(GetResoure<GameObject>("NoneFriendCircleView"), mViewPointGo.transform)
                        .AddItem<NoneFriendCircleView>();
                }
                return _NoneFriendCircleView;
            }
        }

        private FrienCircleManageView _ManageView;
        public FrienCircleManageView ManageView
        {
            get
            {
                if (_ManageView == null)
                {
                    _ManageView = GameObject.Instantiate(GetResoure<GameObject>("ManageView"), mViewPointGo.transform)
                        .AddItem<FrienCircleManageView>();
                }
                return _ManageView;
            }
        }
        private FriendCircleTopView _TopView;
        public FriendCircleTopView TopView
        {
            get
            {
                if (_TopView == null)
                {
                    _TopView = mTopViewGo.AddItem<FriendCircleTopView>();
                }
                return _TopView;
            }
        }
        private FriendCircleOptionLisView _OptionLisView;
        public FriendCircleOptionLisView OptionLisView
        {
            get
            {
                if (_OptionLisView == null)
                {
                    _OptionLisView = mOptionLisViewGo.AddItem<FriendCircleOptionLisView>();
                }
                return _OptionLisView;
            }
        }
        public static FriendCirclePanelComponent Ins { private set; get; }
        public void InitPanel()
        {
            Ins = this;
            mCloseBtn.Add(CloseBtnEvent);
            HideManageView();
            _TopView = mTopViewGo.AddItem<FriendCircleTopView>();
            _OptionLisView = mOptionLisViewGo.AddItem<FriendCircleOptionLisView>();
        }

        public void CloseBtnEvent()
        {
            Game.Scene.GetComponent<ToyGameComponent>().StartGame(ToyGameId.Lobby);
        }
        //显示无亲友圈的UI
        public void ShowNodeFrienCircleUI()
        {
            HideAllView();
            NoneFriendCircleView.Show();
        }
        //显示有亲友圈的UI
        public void ShowHaveFrienCircleUI()
        {
            HideAllView();
            TopView.ShowFriendCircleInfo();//显示上面的 亲友信息
            OptionLisView.ShowStateSet();//设置状态 默认toggle显示房间
            RoomListView.Show();//刷新房间列表
        }
        public override void OnHide()
        {
            base.OnHide();
        }

        public void HideManageView()
        {
            _RoomListView?.Hide();
            _RequestListView?.Hide();
            _RankingListView?.Hide();
            _ManageView?.Hide();
        }
        public void HideAllView()
        {
            HideManageView();
            _TopView?.Hide();
            _OptionLisView?.Hide();
            _NoneFriendCircleView?.Hide();
        }
        public void CutFrienCircleEvent(params object[] objs)
        {

        }

    }
}
