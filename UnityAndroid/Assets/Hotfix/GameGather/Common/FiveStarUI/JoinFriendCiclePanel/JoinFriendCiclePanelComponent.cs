using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class JoinFrienPanelShowType
    {
        public const int Join=1;
        public const int Cut = 2;
    }
    [UIComponent(UIType.JoinFriendCiclePanel)]
    public class JoinFriendCiclePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mFriendCicleItemGo;
        private Button mInquireFriendCicleBtn;
        private Button mCutRecemmendBtn;
        private GameObject mNoneFrienCileGo;
        private Text mTitleText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();

            mFriendCicleItemGo = rc.Get<GameObject>("FriendCicleItemGo");
            mNoneFrienCileGo = rc.Get<GameObject>("NoneFrienCileGo");
            mInquireFriendCicleBtn = rc.Get<GameObject>("InquireFriendCicleBtn").GetComponent<Button>();
            mCutRecemmendBtn = rc.Get<GameObject>("CutRecemmendBtn").GetComponent<Button>();
            mTitleText = rc.Get<GameObject>("TitleText").GetComponent<Text>();
            InitPanel();
        }
        #endregion

        public int CuurShowType = 0;
        public void InitPanel()
        {
            mCloseBtn.Add(Hide);
            mInquireFriendCicleBtn.Add(InquireFriendCicleBtnEvent);
            mCutRecemmendBtn.Add(ShowRecommendFriendsCircle);
        }

        private void InquireFriendCicleBtnEvent()
        {
            UIComponent.GetUiView<JoinRoomPanelComponent>().ShowInputPanel(ShowInputNumberType.FriendCicleId, InputFinshCall);
        }

        public void ShowPanel(int type)
        {
            Show();
            CuurShowType = type;
            mInquireFriendCicleBtn.gameObject.SetActive(CuurShowType == JoinFrienPanelShowType.Join);
            mCutRecemmendBtn.gameObject.SetActive(CuurShowType == JoinFrienPanelShowType.Join);
            if (CuurShowType == JoinFrienPanelShowType.Join)
            {
                ShowRecommendFriendsCircle();
                mTitleText.text = "加入亲友圈";
            }
            else
            {
                ShowCutFriendsCircList();
                mTitleText.text = "切换亲友圈";
            }
        }
        private int _CuurRecommendFriendsCircleStartIndex = 0;

        private bool _NoneOtherRecommend=false;

        public List<FriendsCircleAndCreateUser> _AllRecommendFriendsCircleInfo = new List<FriendsCircleAndCreateUser>();
        //显示推荐亲友圈
        private async void ShowRecommendFriendsCircle()
        {
            if (_NoneOtherRecommend)
            {
                ShowFriendCicleList(_AllRecommendFriendsCircleInfo);
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("已显示全部推荐亲友圈"); //之前请求已经是全部的了 直接从本地取所有推荐亲友圈
                return;
            }
            C2F_GetRecommendFriendsCircle c2FGetFriendsCircleInfo = new C2F_GetRecommendFriendsCircle();
            c2FGetFriendsCircleInfo.StartIndex = _CuurRecommendFriendsCircleStartIndex;
            _CuurRecommendFriendsCircleStartIndex += 5;
            F2C_GetRecommendFriendsCircle f2CGetFriendsCircle = (F2C_GetRecommendFriendsCircle)await SessionComponent.Instance.Call(c2FGetFriendsCircleInfo);
            if (f2CGetFriendsCircle.FriendsCircleInfos.Count < 5)
            {
                _NoneOtherRecommend = true;
            }
            //不限显示自己已经加入的推荐亲友圈
            ExcludeAlreadyJoinFrienCircle(f2CGetFriendsCircle.FriendsCircleInfos);
         
            List<FriendsCircleAndCreateUser> friendsCircleAndCreates =await FriendsCircleAndCreateUserFactory.Create(f2CGetFriendsCircle.FriendsCircleInfos);
            for (int i = 0; i < friendsCircleAndCreates.Count; i++)
            {
                _AllRecommendFriendsCircleInfo.Add(friendsCircleAndCreates[i]);//记录所有的推荐亲友圈
            }
            ShowFriendCicleList(friendsCircleAndCreates);
        }
        //排除已经加入的亲友圈
        public void ExcludeAlreadyJoinFrienCircle(RepeatedField<FriendsCircle> friendsCircleInfos)
        {
            for (int i = 0; i < FrienCircleComponet.Ins.AlreadyJoinFrienCircleIds.Count; i++)
            {
                for (int j = 0; j < friendsCircleInfos.Count; j++)
                {
                    if (FrienCircleComponet.Ins.AlreadyJoinFrienCircleIds.Contains(friendsCircleInfos[j].FriendsCircleId))
                    {
                        friendsCircleInfos.RemoveAt(j);
                        break;
                    }
                }
            }
        }
        //切换已经拥有的亲友圈
        private async void ShowCutFriendsCircList()
        {
            await  FrienCircleComponet.Ins.GetAlreadyJoinFrienCircleIds();//刷新一下所在亲友圈的信息
            ShowFriendCicleList(FrienCircleComponet.Ins.AlreadyJoinFrienCircleIds);
        }
        //查询指定亲友圈
        public void InputFinshCall(int id)
        {
            ShowFriendCicleList(new List<int>(){id},true);
        }
        //显示亲友圈列表
        private async void ShowFriendCicleList(IList<int> friendCirleIds,bool isInputQuery=false)
        {
            RepeatedField<FriendsCircle>  friendsCircles=await  FrienCircleComponet.Ins.GetFriendsCircleInfo(friendCirleIds);
            if (isInputQuery)
            {
                if (friendsCircles.Count > 0)
                {
                    UIComponent.GetUiView<JoinRoomPanelComponent>().Hide();
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("查询成功");
                    ShowFriendCicleList(await FriendsCircleAndCreateUserFactory.Create(friendsCircles));
                }
                else
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("亲友圈不存在");
                }
                mNoneFrienCileGo.SetActive(false);
            }
            else
            {
                List<FriendsCircleAndCreateUser> friendsCircleAndCreates =await FriendsCircleAndCreateUserFactory.Create(friendsCircles);
                ShowFriendCicleList(friendsCircleAndCreates);
            }
        }

       
        //显示亲友圈列表
        private void ShowFriendCicleList(IList<FriendsCircleAndCreateUser> fiFriendsCircleAndCreateUsers)
        {
            mFriendCicleItemGo.transform.parent.CreatorChildAndAddItem<FriendCicleItem, FriendsCircleAndCreateUser>(fiFriendsCircleAndCreateUsers);
            mNoneFrienCileGo.SetActive(fiFriendsCircleAndCreateUsers.Count == 0);
        }
    }
}
