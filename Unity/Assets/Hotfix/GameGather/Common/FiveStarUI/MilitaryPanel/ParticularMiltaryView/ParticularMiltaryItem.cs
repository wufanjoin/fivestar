using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class ParticularMiltaryItemAwakeSystem : AwakeSystem<ParticularMiltaryItem, GameObject, ParticularMiltary>
    {
        public override void Awake(ParticularMiltaryItem self, GameObject go, ParticularMiltary data)
        {
            self.Awake(go, data, UIType.MilitaryPanel);
        }
    }
    public class ParticularMiltaryItem : BaseItem<ParticularMiltary>
    {
        #region 脚本工具生成的代码
        private Text mSerialNumberText;
        private Text mFightTimeText;
        private GameObject mPlayerScoreGo;
        private Button mShareBtn;
        private Button mLookBackBtn;
        public override void Awake(GameObject go, ParticularMiltary data, string uiType)
        {
            base.Awake(go, data, uiType);
            mSerialNumberText = rc.Get<GameObject>("SerialNumberText").GetComponent<Text>();
            mFightTimeText = rc.Get<GameObject>("FightTimeText").GetComponent<Text>();
            mPlayerScoreGo = rc.Get<GameObject>("PlayerScoreGo");
            mShareBtn = rc.Get<GameObject>("ShareBtn").GetComponent<Button>();
            mLookBackBtn = rc.Get<GameObject>("LookBackBtn").GetComponent<Button>();
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mFightTimeText.text = TimeTool.ConvertLongToTimeDesc(mData.Time);
            mSerialNumberText.text = (gameObject.transform.GetSiblingIndex() + 1).ToString();
            InitScore();
            mShareBtn.Add(ShareBtnEvent);
            mLookBackBtn.Add(LookBackBtn);
        }

        public void ShareBtnEvent()
        {
            ShareUrlMgr.VideoShare(mData.DataId);
        }
        public void LookBackBtn()
        {
            MilitaryPanelComponent.ExaminePlayBack(mData.DataId);
        }
        public void InitScore()
        {
            Transform scoreParent = mPlayerScoreGo.transform.parent;
            scoreParent.CreatorChildCount(mData.GetScoreInfos.Count);
            for (int i = 0; i < mData.GetScoreInfos.Count; i++)
            {
                Text textScore = scoreParent.GetChild(i).GetComponent<Text>();
                if (mData.GetScoreInfos[i] >= 0)
                {
                    textScore.color = MilitaryPanelComponent.AddScoreColor;
                }
                else
                {
                    textScore.color = MilitaryPanelComponent.SubScoreColor;
                }
                textScore.text = mData.GetScoreInfos[i].ToString();
            }
        }
    }
}
