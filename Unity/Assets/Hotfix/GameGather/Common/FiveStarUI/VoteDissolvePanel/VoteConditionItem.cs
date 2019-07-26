using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class VoteConditionItemAwakeSystem : AwakeSystem<VoteConditionItem, GameObject, CardFiveStarPlayer>
    {
        public override void Awake(VoteConditionItem self, GameObject go, CardFiveStarPlayer data)
        {
            self.Awake(go, data, UIType.VoteDissolvePanel);
        }
    }
    public class VoteConditionItem : BaseItem<CardFiveStarPlayer>
    {
        #region 脚本工具生成的代码
        private Text mNameText;
        private GameObject mConsentGo;
        private GameObject mTurnGo;
        private GameObject mConsiderGo;
        public override void Awake(GameObject go, CardFiveStarPlayer data, string uiType)
        {
            base.Awake(go, data, uiType);
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mConsentGo = rc.Get<GameObject>("ConsentGo");
            mTurnGo = rc.Get<GameObject>("TurnGo");
            mConsiderGo = rc.Get<GameObject>("ConsiderGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            mNameText.text = mData._user.Name;
        }

        public void SetVoteState(int voteState)
        {
            mConsentGo.SetActive(false);
            mTurnGo.SetActive(false);
            mConsiderGo.SetActive(false);
            if (VoteResultType.Consent == voteState)
            {
                mConsentGo.SetActive(true);
            }
            else if (VoteResultType.NoConsent == voteState)
            {
                mTurnGo.SetActive(true);
            }
            else if (VoteResultType.BeingVote == voteState)
            {
                mConsiderGo.SetActive(true);
            }
        }
    }
}
