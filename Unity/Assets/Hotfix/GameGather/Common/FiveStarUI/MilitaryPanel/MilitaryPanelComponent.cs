using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.MilitaryPanel)]
    public class MilitaryPanelComponent : PopUpUIView
    {
        public static Color AddScoreColor=new Color(0.784f,0,0,1);//加分的颜色
        public static Color SubScoreColor = new Color(0, 0.784f, 0, 1);//减分的颜色
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mBigMiltaryViewGo;
        private GameObject mParticularMiltaryViewGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mBigMiltaryViewGo = rc.Get<GameObject>("BigMiltaryViewGo");
            mParticularMiltaryViewGo = rc.Get<GameObject>("ParticularMiltaryViewGo");
            InitPanel();
        }
        #endregion
        public override bool isShakeAnimation
        {
            get { return false; }//不显示抖动效果
        }
        public BigMiltaryView _BigMiltaryView;
        public ParticularMiltaryView _ParticularMiltaryView;
        public void InitPanel()
        {
            mCloseBtn.Add(CloseBtnEvent);
            _BigMiltaryView=mBigMiltaryViewGo.AddItem<BigMiltaryView>();
            _ParticularMiltaryView = mParticularMiltaryViewGo.AddItem<ParticularMiltaryView>();
        }

        public void CloseBtnEvent()
        {
            if (_ParticularMiltaryView.gameObject.activeInHierarchy)
            {
                _BigMiltaryView.Show();
                _ParticularMiltaryView.Hide();
            }
            else
            {
                Hide();
            }
        }
        public async void ShowMilitary(long userId,int friendCircleId=0)
        {
            Show();
            L2C_GetPlayerMiltary l2CGetPlayerMiltary=(L2C_GetPlayerMiltary)await  SessionComponent.Instance.Call(
                new C2L_GetPlayerMiltary() {FriendCircleId = friendCircleId, QueryUserId = userId});
            _BigMiltaryView.ShowBigMiltary(l2CGetPlayerMiltary.Miltarys); 
        }

        //查看回放
        public static async  void ExaminePlayBack(int recordId)
        {
            L2C_GetMiltaryRecordDataInfo l2CGetMiltaryRecordData = (L2C_GetMiltaryRecordDataInfo)await SessionComponent.Instance.Call(
                new C2L_GetMiltaryRecordDataInfo() { DataId = recordId });
            if (string.IsNullOrEmpty(l2CGetMiltaryRecordData.Message))
            {
                if (l2CGetMiltaryRecordData.RecordDataInfo == null)
                {
                    UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel("录像不存在");
                    return;
                }
                CardFiveStarVideoAisle.EnterRoom(l2CGetMiltaryRecordData.RecordDataInfo);
                UIComponent.GetUiView<MilitaryPanelComponent>().Hide();
            }
            else
            {
                UIComponent.GetUiView<NormalHintPanelComponent>().ShowHintPanel(l2CGetMiltaryRecordData.Message);
            }
        }
    }
}
