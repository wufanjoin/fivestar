using System;
using ETModel;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    public class ParticularMiltaryView : BaseView
    {
        #region 脚本工具生成的代码
        private Text mIdText;
        private Text mGmaeNameText;
        private Text mTimeText;
        private Text mPlayerNameText;
        private GameObject mParticularMiltaryItemGo;
        public override void Init(GameObject go)
        {
            base.Init(go);
            mIdText = rc.Get<GameObject>("IdText").GetComponent<Text>();
            mGmaeNameText = rc.Get<GameObject>("GmaeNameText").GetComponent<Text>();
            mTimeText = rc.Get<GameObject>("TimeText").GetComponent<Text>();
            mPlayerNameText = rc.Get<GameObject>("PlayerNameText").GetComponent<Text>();
            mParticularMiltaryItemGo = rc.Get<GameObject>("ParticularMiltaryItemGo");
            Ins = this;
            InitPanel();
        }
        #endregion
        public static ParticularMiltaryView Ins { get; private set; }
        public void InitPanel()
        {
           
        }

        public void ShowParticularMilrary(Miltary  miltary,RepeatedField<ParticularMiltary> miltarySmallInfos)
        {
            Show();
            mIdText.text ="No."+ miltary.RoomNumber;
            mTimeText.text = TimeTool.ConvertLongToTimeDesc(miltary.Time);
            Transform nameParent=mPlayerNameText.transform.parent;
            nameParent.CreatorChildCount(miltary.PlayerInofs.Count);
            for (int i = 0; i < miltary.PlayerInofs.Count; i++)
            {
                nameParent.GetChild(i).GetComponent<Text>().text = miltary.PlayerInofs[i].Name;
            }
            mParticularMiltaryItemGo.transform.parent
                .CreatorChildAndAddItem<ParticularMiltaryItem, ParticularMiltary>(miltarySmallInfos);
        }
    }
}
