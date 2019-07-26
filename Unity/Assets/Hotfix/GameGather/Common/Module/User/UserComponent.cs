using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETHotfix;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using Component = ETHotfix.Component;

namespace ETHotfix
{

    [ObjectSystem]
    public class UserComponentAwakeSystem : AwakeSystem<UserComponent>
    {
        public override void Awake(UserComponent self)
        {
            UserComponent.Ins = self;
            self.Awake();
        }
    }
    public class UserComponent: Component
    {
        public static UserComponent Ins;
        public long pUserId
        {
            get { return pSelfUser.UserId; }
        }
        public User pSelfUser { private set; get; }

        public long pRoomId { private set; get; }
        public void Awake()
        {
            
        }

        public void SetRoomId(long roomId)
        {
            pRoomId = roomId;
        }
        public void SetSelfUser(User user)
        {
            pSelfUser = user;
            AddUserInfo(user);
            EventMsgMgr.SendEvent(CommEventID.SelfUserInfoRefresh);
        }


        public void GetGoodss(List<GetGoodsOne> goodss,bool isShowHintPanel)
        {
            foreach (var goods in goodss)
            {
                switch (goods.GoodsId)
                {
                    case GoodsId.Jewel:
                        pSelfUser.Jewel = goods.NowAmount;
                        break;
                    case GoodsId.Besans:
                        pSelfUser.Beans = goods.NowAmount;
                        break;
                }
            }
            if (isShowHintPanel)
            {
                EventMsgMgr.SendEvent(CommEventID.GetGodds, goodss);
            }
            EventMsgMgr.SendEvent(CommEventID.SelfUserInfoRefresh);
        }

        private Dictionary<long, User> _UserDic = new Dictionary<long, User>();

        private void AddUserInfo(User user)
        {
            User newuser= CopyUser.Copy(user);
             _UserDic[newuser.UserId] = newuser;
        }

        private async ETTask<RepeatedField<User>> QueryUserInfo(params long[] ids)
        {
            C2L_GetUserInfo c2LGetUserInfo = new C2L_GetUserInfo();
            c2LGetUserInfo.QueryUserIds.Add(ids);
            L2C_GetUserInfo l2CGetUserInfo = (L2C_GetUserInfo)await SessionComponent.Instance.Call(c2LGetUserInfo);
            for (int i = 0; i < l2CGetUserInfo.UserInfos.Count; i++)
            {
                AddUserInfo(l2CGetUserInfo.UserInfos[i]);
            }
            RepeatedField<User> queryUsers = new RepeatedField<User>();
            queryUsers.Add(l2CGetUserInfo.UserInfos);
            l2CGetUserInfo.UserInfos.Clear();
            return queryUsers;
        }
        public async ETTask<User>  GetUserInfo(long userId)
        {
            if (_UserDic.ContainsKey(userId))
            {
                return _UserDic[userId];
            }
            RepeatedField<User> users=await QueryUserInfo(userId);
            if (users.Count > 0)
            {
                return users[0];
            }
            return null;
        }
        public async ETTask<RepeatedField<User>> GetUserInfo(IList<long> userIds)
        {
            for (int i = 0; i < userIds.Count; i++)
            {
                if (!_UserDic.ContainsKey(userIds[i]))
                {
                    RepeatedField<User> queryUsers = await QueryUserInfo(userIds.ToArray());
                    return queryUsers;
                }
            }
            RepeatedField<User> users = new RepeatedField<User>();
            for (int i = 0; i < userIds.Count; i++)
            {
                users.Add(await GetUserInfo(userIds[i])); 
            }
            return users;
        }
    }

}
