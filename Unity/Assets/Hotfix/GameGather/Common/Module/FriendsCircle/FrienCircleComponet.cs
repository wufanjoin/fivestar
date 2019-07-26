using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [ObjectSystem]
    public class FrienCircleComponetSystem : AwakeSystem<FrienCircleComponet>
    {
        public override void Awake(FrienCircleComponet self)
        {
            self.Awake();
        }
    }

    public class FrienCircleComponet : Component
    {
        public static FrienCircleComponet Ins;

        public FriendsCircle CuurSelectFriendsCircle { get; set; }//当前选择的亲友圈信息

        public User CreateUser;//创建人信息 就是亲友圈主

        public RepeatedField<int> AlreadyJoinFrienCircleIds;//已经加入亲友圈的ID列表

        public int DefaultShowFrienCircleId = 0;//默认显示的亲友圈id

        public void Awake()
        {
            Ins = this;
        }


        public async Task ShowFrienCircle()
        {
            if (CuurSelectFriendsCircle == null|| CuurSelectFriendsCircle.FriendsCircleId==0)//如果当前显示的亲友圈 数据为空 就显示一个默认的亲友圈
            {
                await GetAlreadyJoinFrienCircleIds();
                await SetDefaultSelectFriendsCircle(DefaultShowFrienCircleId);
            }
            UIComponent.GetUiView<FriendCirclePanelComponent>().Show();
            if (CuurSelectFriendsCircle == null)
            {
                UIComponent.GetUiView<FriendCirclePanelComponent>().ShowNodeFrienCircleUI();
            }
            else
            {
                UIComponent.GetUiView<FriendCirclePanelComponent>().ShowHaveFrienCircleUI();
            }
        }
        //修改玩法
        public void AlteWanFaCall(RepeatedField<int> config)
        {
            CuurSelectFriendsCircle.defaultWanFaConfigInfo = null;
            CuurSelectFriendsCircle.DefaultWanFaCofigs = config;
            UIComponent.GetUiView<FriendCirclePanelComponent>().RoomListView.Show();
            UIComponent.GetUiView<FriendCirclePanelComponent>().RoomListView.Hide();
        }
        //获取自己已经加入的亲友圈ID列表
        public async Task GetAlreadyJoinFrienCircleIds()
        {
            F2C_GetSelfFriendsList f2CGetSelfFriendsList = (F2C_GetSelfFriendsList)await SessionComponent.Instance.Call(new C2F_GetSelfFriendsList());
            AlreadyJoinFrienCircleIds = f2CGetSelfFriendsList.FriendsCrircleIds;
        }
        //切换亲友圈
        public void  CutFrienCircle(FriendsCircle friendsCircle)
        {
            SetCurrShowFrienCirleInfo(friendsCircle);
        }
        //设置当前显示亲友圈的信息 并切换UI
        public async void SetCurrShowFrienCirleInfo(FriendsCircle friendsCircle)
        {
            CuurSelectFriendsCircle = friendsCircle;
            if (CuurSelectFriendsCircle != null)
            {
                CreateUser = await UserComponent.Ins.GetUserInfo(CuurSelectFriendsCircle.CreateUserId);
            }
            await ShowFrienCircle();
        }
        //获取并设置默认亲友圈信息
        public async Task SetDefaultSelectFriendsCircle(int frienCircleId=0)
        {
            if (!AlreadyJoinFrienCircleIds.Contains(frienCircleId))
            {
                if (AlreadyJoinFrienCircleIds.Count <= 0)
                {
                    return;
                }
                frienCircleId = AlreadyJoinFrienCircleIds[0];
            }
            CuurSelectFriendsCircle = await GetFriendsCircleInfo(frienCircleId);
            CreateUser = await UserComponent.Ins.GetUserInfo(CuurSelectFriendsCircle.CreateUserId);
        }
        //成功创建亲友圈
        public void SucceedCreatorFriendsCircle(FriendsCircle friendsCircle)
        {
            SetCurrShowFrienCirleInfo(friendsCircle);
        }
        //玩家退出亲友圈
        public async void OutFrienCircle(int friendsCircleId)
        {
            if (AlreadyJoinFrienCircleIds != null&&CuurSelectFriendsCircle.FriendsCircleId == friendsCircleId)
            {
                CuurSelectFriendsCircle = null;
                await ShowFrienCircle();
            }
        }
        //创建房间
        public void CreateRoom(RepeatedField<int> configs)
        {
            CardFiveStarAisle.CreateRoom(configs, CuurSelectFriendsCircle.FriendsCircleId);
        }
        private Dictionary<int, FriendsCircle> _FriendsCircleDic = new Dictionary<int, FriendsCircle>();
        //获取亲友圈信息
        public async Task<FriendsCircle> GetFriendsCircleInfo(int friendsCirleId)
        {
            if (_FriendsCircleDic.ContainsKey(friendsCirleId))
            {
                return _FriendsCircleDic[friendsCirleId];
            }
            RepeatedField<FriendsCircle> queryFriendsCircles = await QueryFriendsCircleInfo(friendsCirleId);
            if (queryFriendsCircles.Count > 0)
            {
                return queryFriendsCircles[0];
            }
            return null;
        }
        //获取亲友圈信息
        public async Task<RepeatedField<FriendsCircle>> GetFriendsCircleInfo(IList<int> friendsCirleIds)
        {
            for (int i = 0; i < friendsCirleIds.Count; i++)
            {
                if (!_FriendsCircleDic.ContainsKey(friendsCirleIds[i]))
                {
                    RepeatedField<FriendsCircle> queryFriendsCircles = await QueryFriendsCircleInfo(friendsCirleIds.ToArray());
                    return queryFriendsCircles;
                }
            }
            RepeatedField<FriendsCircle> friendsCircles = new RepeatedField<FriendsCircle>();
            for (int i = 0; i < friendsCirleIds.Count; i++)
            {
                friendsCircles.Add(await GetFriendsCircleInfo(friendsCirleIds[i]));
            }
            return friendsCircles;
        }

        //协议查询亲友圈信息
        private async Task<RepeatedField<FriendsCircle>> QueryFriendsCircleInfo(params int[] ids)
        {
            C2F_GetFriendsCircleInfo c2FGetFriendsCircleInfo = new C2F_GetFriendsCircleInfo();
            c2FGetFriendsCircleInfo.FriendsCrircleIds.Add(ids);
            F2C_GetFriendsCircleInfo f2CGetFriendsCircleInfo = (F2C_GetFriendsCircleInfo)await SessionComponent.Instance.Call(c2FGetFriendsCircleInfo);
            RepeatedField<FriendsCircle> friendsCircles = new RepeatedField<FriendsCircle>();
            for (int i = 0; i < f2CGetFriendsCircleInfo.FrienCircleInfos.Count; i++)
            {
                FriendsCircle friendsCircle=AddFriendsCircleInfo(f2CGetFriendsCircleInfo.FrienCircleInfos[i]);
                friendsCircles.Add(friendsCircle);
            }
            return friendsCircles;
        }
        //添加亲友圈信息
        public FriendsCircle AddFriendsCircleInfo(FriendsCircle friendsCircle)
        {
            FriendsCircle copyFriendsCircle = CopyFriendsCircle.Copy(friendsCircle);
            _FriendsCircleDic[copyFriendsCircle.FriendsCircleId] = copyFriendsCircle;
            return copyFriendsCircle;
        }

    }
}
