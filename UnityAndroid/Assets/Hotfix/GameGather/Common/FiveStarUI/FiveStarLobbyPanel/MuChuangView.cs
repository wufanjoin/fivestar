using System;
using System.Collections.Generic;
using DG.Tweening;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class LanternAnim
    {
        private GameObject _AigletGo;
        public GameObject gameObject;
        public LanternAnim(GameObject go)
        {
            gameObject = go;
            _AigletGo = go.FindChild("AigletGo").gameObject;
            PlayAnim();
        }

        public const float LanternRange = 3.00f;//灯笼的摇晃幅度
        public const float AigletRange = 18.00f;//吊坠的摇晃幅度

        private Vector3 _LanternVector = new Vector3(0, 0, 0);
        private Vector3 _AigletVector = new Vector3(0, 0, 0);
        private static int RangePlus = 1;
        private bool isAnimIn;
        public async void PlayAnim()
        {
            if (isAnimIn)
            {
                return;
            }

            isAnimIn = true;
            while (gameObject.activeInHierarchy)
            {
                
                _LanternVector.z = (float)RandomTool.Random(LanternRange/2, LanternRange) * RangePlus;
                _AigletVector.z = (float)RandomTool.Random(AigletRange/2, AigletRange) * RangePlus;
                gameObject.transform.DOLocalRotate(_LanternVector, 3f);
                _AigletGo.transform.DOLocalRotate(_AigletVector, 3f);
                RangePlus = RangePlus * -1;
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(2000);
            }
            isAnimIn = false;
        }
    }
    public class MuChuangView : BaseView
    {
        #region 脚本工具生成的代码
        private Button mJoinRoomBtn;
        private Button mCreateRoomBtn;
        private Button mMatchBtn;
        private Button mPublicityBtn;//推广
        private Button mRelativesCircleBtn;//亲友圈
        private Button mTournamentBtn;//比赛
        public override void Init(GameObject go)
        {
            base.Init(go);
            mJoinRoomBtn = rc.Get<GameObject>("JoinRoomBtn").GetComponent<Button>();
            mCreateRoomBtn = rc.Get<GameObject>("CreateRoomBtn").GetComponent<Button>();
            mMatchBtn = rc.Get<GameObject>("MatchBtn").GetComponent<Button>();
            mPublicityBtn = rc.Get<GameObject>("PublicityBtn").GetComponent<Button>();
            mRelativesCircleBtn = rc.Get<GameObject>("RelativesCircleBtn").GetComponent<Button>();
            mTournamentBtn = rc.Get<GameObject>("TournamentBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        List<LanternAnim> _allLanternAnims=new List<LanternAnim>();
        public void InitPanel()
        {
            mJoinRoomBtn.Add(JoinRoomBtnEvent,false);
            mCreateRoomBtn.Add(CreateRoomBtnEvent, false);
            mMatchBtn.Add(MatchBtnEvent, false);
            _allLanternAnims.Add(new LanternAnim(mJoinRoomBtn.gameObject));
            _allLanternAnims.Add(new LanternAnim(mCreateRoomBtn.gameObject));
            _allLanternAnims.Add(new LanternAnim(mMatchBtn.gameObject));

            mPublicityBtn.Add(PublicityBtnEvent);
            mRelativesCircleBtn.Add(RelativesCircleBtnEvent);
            mTournamentBtn.Add(TournamentBtnEvent);
        }

        public override async void Show()
        {
            base.Show();
            for (int i = 0; i < _allLanternAnims.Count; i++)
            {
                _allLanternAnims[i].PlayAnim();
            }
        }

        public void JoinRoomBtnEvent()
        {
            UIComponent.GetUiView<JoinRoomPanelComponent>().ShowInputPanel(ShowInputNumberType.RoomNumber, CardFiveStarAisle.JoinRoom);
        }
        
        public void CreateRoomBtnEvent()
        {
            UIComponent.GetUiView<CreateRoomPanelComponent>().ShowCraterRoomPanel(ShowCraterRoomPanelType.NormalCraterRoom, CardFiveStarAisle.CreateRoom);
        }


        public void MatchBtnEvent()
        {
            UIComponent.GetUiView<MatchingRoomPanelComponent>().Show();
        }
        public void PublicityBtnEvent()
        {
            UIComponent.GetUiView<GeneralizePanelComponent>().Show();
        }
        public async void RelativesCircleBtnEvent()
        {
            //C2F_GetFriendsCircleInfo c2FGetFriendsCircleInfo = new C2F_GetFriendsCircleInfo();
           // c2FGetFriendsCircleInfo.FriendsCrircleIds.Add(110212);
           // F2C_GetFriendsCircleInfo f2CGetFriendsCircleInfo = (F2C_GetFriendsCircleInfo)await SessionComponent.Instance.Call(c2FGetFriendsCircleInfo);
            await Game.Scene.GetComponent<FrienCircleComponet>().ShowFrienCircle();
        }

        public void TournamentBtnEvent()
        {
            UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("暂未开放");
        }
    }
}
