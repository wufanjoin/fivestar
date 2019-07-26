using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.MatchingRoomPanel)]
    public class MatchingRoomPanelComponent : NormalUIView
    {
        #region 脚本工具生成的代码
        private GameObject mRoomItemGo;
        private Button mCloseBtn;
        private Button mFastStartBtn;
        private Button mBeasnsAddBtn;
        private Text mBeasnsNumText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mRoomItemGo = rc.Get<GameObject>("RoomItemGo");
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mFastStartBtn = rc.Get<GameObject>("FastStartBtn").GetComponent<Button>();
            mBeasnsAddBtn = rc.Get<GameObject>("BeasnsAddBtn").GetComponent<Button>();
            mBeasnsNumText = rc.Get<GameObject>("BeasnsNumText").GetComponent<Text>();
            InitPanel();
        }
        #endregion
        public  void InitPanel()
        {
            mCloseBtn.Add(CloseBtnEvent);
            mBeasnsAddBtn.Add(BeasnsAddBtnEvent);
            mFastStartBtn.Add(FastStartBtnEvent);
            InitRoomList();
            SetBeasnsNum();
            EventMsgMgr.RegisterEvent(CommEventID.SelfUserInfoRefresh, SetBeasnsNum);

        }

        public void CloseBtnEvent()
        {
            UIComponent.GetUiView<FiveStarLobbyPanelComponent>().Show();
        }
        public void SetBeasnsNum(params  object[] objs)
        {
            mBeasnsNumText.text = UserComponent.Ins.pSelfUser.Beans.ToString();
        }
        public void BeasnsAddBtnEvent()
        {
            UIComponent.GetUiView<ShopPanelComponent>().ShowGoodsList(GoodsId.Besans, pViewType);
        }
        public void FastStartBtnEvent()
        {
            for (int i = _RoomLists.Count-1; i >= 0; i--)
            {
                if (_RoomLists[i].mData.BesansLowest< UserComponent.Ins.pSelfUser.Beans)
                {
                    _RoomLists[i].EnterRoom();
                    return;
                }
            }
            _RoomLists[0].EnterRoom();
        }

        public List<MathRoomItem> _RoomLists;
        private async void InitRoomList()
        {
            if (_MatchRoomConfigs==null)
            {
                await RequestMatchRoomConfigs();
            }
            Transform roomItemParent = mRoomItemGo.transform.parent;
            _RoomLists=roomItemParent.CreatorChildAndAddItem<MathRoomItem, MatchRoomConfig>(
                _MatchRoomConfigs);
        }

        private bool IsHaveReliefPayment = true;
        //开始匹配
        public async ETTask<bool> StartMatch(int roomId)
        {
            MatchRoomConfig matchRoomConfig=await GetMatchRoomConfig(roomId);
            if (IsHaveReliefPayment&&UserComponent.Ins.pSelfUser.Beans < 1000)
            {
                L2C_GetReliefPayment  l2CGetReliefPayment= (L2C_GetReliefPayment)await SessionComponent.Instance.Call(new C2L_GetReliefPayment());
                if (!string.IsNullOrEmpty(l2CGetReliefPayment.Message))
                {
                    IsHaveReliefPayment = false;
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(l2CGetReliefPayment.Message);
                }
                else
                {
                    UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("成功领取救济金 每日三次",null, PopOptionType.Single);
                }
            }
            if (matchRoomConfig.BesansLowest > UserComponent.Ins.pSelfUser.Beans)
            {
                UIComponent.GetUiView<PopUpHintPanelComponent>().ShowOptionWindow("豆子数低于房间最低限制 是否购买", ShowShop);
                return false;
            }
            M2C_StartMatch m2CStartMatch = (M2C_StartMatch)await SessionComponent.Instance.Call(new C2M_StartMatch() { MatchRoomId = roomId });
            if (!string.IsNullOrEmpty(m2CStartMatch.Message))
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(m2CStartMatch.Message);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ShowShop(bool bol)
        {
            if (bol)
            {
                UIComponent.GetUiView<ShopPanelComponent>().ShowGoodsList(GoodsId.Besans,UIType.CardFiveStarRoomPanel);
            }
        }

        //想服务器请求 所有匹配房间配置
        private RepeatedField<MatchRoomConfig> _MatchRoomConfigs;
        private async Task RequestMatchRoomConfigs()
        {
            L2C_GetMatchRoomConfigs l2CGetMatchRoomConfigs = (L2C_GetMatchRoomConfigs)await SessionComponent.Instance.Call(new C2L_GetMatchRoomConfigs() { ToyGameId = ToyGameId.CardFiveStar });
            _MatchRoomConfigs = l2CGetMatchRoomConfigs.MatchRoomConfigs;
        }

        //获得房间 配置
        public async ETTask<MatchRoomConfig> GetMatchRoomConfig(int roomId)
        {
            if (_MatchRoomConfigs == null)
            {
                await RequestMatchRoomConfigs();
            }
            for (int i = 0; i < _MatchRoomConfigs.Count; i++)
            {
                if (_MatchRoomConfigs[i].MatchRoomId == roomId)
                {
                    return _MatchRoomConfigs[i];
                }
            }
            Log.Error("获取匹配房间 配置信息错误");
            return null;
        }



    }
}
