using System;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class FrienCircleRankingListView : BaseView
    {
        #region 脚本工具生成的代码
        private GameObject mRankingInfoItemGo;
        private GameObject mNoneRankingGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mRankingInfoItemGo = rc.Get<GameObject>("RankingInfoItemGo");
            mNoneRankingGo = rc.Get<GameObject>("NoneRankingGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            
        }

        private bool isNeedRfesh = false;//显示后是否需要刷新
        public void CutFrienCircleEvent(params object[] objs)
        {
            isNeedRfesh = true;
        }

        public override void Show()
        {
            base.Show();
            if (_CurrUIInFriendsCircleId != FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId)//如果当前UI对应的亲友圈ID是实际的亲友圈id不符就刷新
            {
                RefreshRankingInfo();
            }
        }
        private int _CurrUIInFriendsCircleId = 0;//当前界面显示的信息对应的亲友圈ID
        private async void RefreshRankingInfo()
        {
            F2C_GetRankingListInfo f2CGetRankingListInfo=(F2C_GetRankingListInfo)await SessionComponent.Instance.Call(new C2F_GetRankingListInfo()
            {
                FriendsCrircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId
            });
            SortRankingPlayerInfo(f2CGetRankingListInfo.PlayerInfos);
            mRankingInfoItemGo.transform.parent.CreatorChildAndAddItem<FrienCircleRankingPlayerInfoItem, RanKingPlayerInfo>(f2CGetRankingListInfo.PlayerInfos);
            mNoneRankingGo.SetActive(f2CGetRankingListInfo.PlayerInfos.Count==0);
            _CurrUIInFriendsCircleId = FrienCircleComponet.Ins.CuurSelectFriendsCircle.FriendsCircleId;
        }

        //按照总分给排行榜排序
        private void SortRankingPlayerInfo(RepeatedField<RanKingPlayerInfo> PlayerInfos)
        {
            RanKingPlayerInfo info;
            for (int i = 0; i < PlayerInfos.Count-1; i++)
            {
                for (int j = 0; j < PlayerInfos.Count - 1-i; j++)
                {
                    if (PlayerInfos[j].TotalScore < PlayerInfos[j + 1].TotalScore)
                    {
                        info = PlayerInfos[j];
                        PlayerInfos[j] = PlayerInfos[j + 1];
                        PlayerInfos[j + 1] = info;
                    }
                }
            }
        }
    }
}
