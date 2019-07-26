using System;
using System.Collections.Generic;
using ETModel;
using MongoDB.Bson.Serialization.Conventions;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class BigMiltaryItemAwakeSystem : AwakeSystem<BigMiltaryItem, GameObject, Miltary>
    {
        public override void Awake(BigMiltaryItem self, GameObject go, Miltary data)
        {
            self.Awake(go, data, UIType.MilitaryPanel);
        }
    }
    public class BigMiltaryItem : BaseItem<Miltary>
    {
        #region 脚本工具生成的代码
        private Text mGameNameText;
        private Text mMiltaryIdText;
        private GameObject mPlayerInfoItemGo;
        private Text mTimeText;
        private Button mExamineParticularsBtn;
        public override void Awake(GameObject go, Miltary data, string uiType)
        {
            base.Awake(go, data, uiType);
            mGameNameText = rc.Get<GameObject>("GameNameText").GetComponent<Text>();
            mMiltaryIdText = rc.Get<GameObject>("MiltaryIdText").GetComponent<Text>();
            mPlayerInfoItemGo = rc.Get<GameObject>("PlayerInfoItemGo");
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mExamineParticularsBtn = rc.Get<GameObject>("ExamineParticularsBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mMiltaryIdText.text = "No." + mData.RoomNumber;
            mTimeText.text = TimeTool.ConvertLongToTimeDesc(mData.Time);
            mExamineParticularsBtn.Add(ExamineParticularsBtnEvent);
            List<BigMiltaryPlayerInfoItem>  players=mPlayerInfoItemGo.transform.parent.CreatorChildAndAddItem<BigMiltaryPlayerInfoItem, MiltaryPlayerInfo>(mData
                .PlayerInofs);
            players[0].SetHouse();
        }

        public async void ExamineParticularsBtnEvent()
        {
            L2C_GetMiltarySmallInfo l2CGetPlayerMiltary = (L2C_GetMiltarySmallInfo)await SessionComponent.Instance.Call(
                new C2L_GetMiltarySmallInfo() { MiltaryId=mData.MiltaryId });
            BigMiltaryView.Ins.Hide();
            if (l2CGetPlayerMiltary.MiltarySmallAllInfo != null)
            {
                ParticularMiltaryView.Ins.ShowParticularMilrary(mData, l2CGetPlayerMiltary.MiltarySmallAllInfo.ParticularMiltarys);
            }
        }
    }
}
