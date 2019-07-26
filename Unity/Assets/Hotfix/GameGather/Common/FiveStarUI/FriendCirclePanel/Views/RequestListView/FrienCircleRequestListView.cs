using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class FrienCircleRequestListView : BaseView
    {
        #region 脚本工具生成的代码
        private GameObject mRequestItemGo;
        private Text mDynamicContentText;
        private GameObject mNoneRequestGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mRequestItemGo = rc.Get<GameObject>("RequestItemGo");
            mNoneRequestGo = rc.Get<GameObject>("NoneRequestGo");
            mDynamicContentText = rc.Get<GameObject>("DynamicContentText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            //亲友圈动态暂时不做
            InitRequestList();
        }
        public override  void Show()
        {
            base.Show();
            InitRequestList();
        }
        public async void InitRequestList()
        {
            F2C_GetFriendsCircleApplyJoinList f2CGetFriendsCircle=(F2C_GetFriendsCircleApplyJoinList)await SessionComponent.Instance.Call(new C2F_GetFriendsCircleApplyJoinList()
            {
                FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId
            });
            Transform itemParent = mRequestItemGo.transform.parent;
            RepeatedField<User> users =await UserComponent.Ins.GetUserInfo(f2CGetFriendsCircle.ApplyJoinUserIdList);
            itemParent.CreatorChildAndAddItem<FrienCircleRequestPlayerItem, User>(users);
            mNoneRequestGo.SetActive(users.Count==0);
        }
    }
}
