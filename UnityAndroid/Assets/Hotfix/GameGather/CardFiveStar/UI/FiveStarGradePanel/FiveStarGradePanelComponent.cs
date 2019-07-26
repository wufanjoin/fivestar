using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{



    [UIComponent(UIType.FiveStarGradePanel)]
    public class FiveStarGradePanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Button mCloseBtn;
        private GameObject mPlayerGradeItemGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mCloseBtn = rc.Get<GameObject>("CloseBtn").GetComponent<Button>();
            mPlayerGradeItemGo = rc.Get<GameObject>("PlayerGradeItemGo");
            InitPanel();
        }
        #endregion
        public void InitPanel()
        {
            InitGradePlayerInfo();
            mCloseBtn.Add(Hide);
        }

        public void ShowPlayerGrade()
        {
            Show();
            CardFiveStarPlayer[] players = CardFiveStarRoom.Ins._ServerSeatIndexInPlayer.Values.ToArray();
            for (int i = 0; i < players.Length; i++)
            {
                gradePlayerItems[i].SetUI(players[i]);
            }
            for (int i = players.Length; i < gradePlayerItems.Count; i++)
            {
                gradePlayerItems[i].Hide();
            }
            
        }

        List<FiveStarPlayerGradeItem> gradePlayerItems=new List<FiveStarPlayerGradeItem>();
        public void InitGradePlayerInfo()
        {
            Transform gradePlayerParent = mPlayerGradeItemGo.transform.parent;
            for (int i = 0; i < 3; i++)
            {
                GameObject.Instantiate(mPlayerGradeItemGo, gradePlayerParent);
            }
            for (int i = 0; i < gradePlayerParent.childCount; i++)
            {
                FiveStarPlayerGradeItem gradeItem=gradePlayerParent.GetChild(i)
                    .AddItemIfHaveInit<FiveStarPlayerGradeItem, CardFiveStarPlayer>(new CardFiveStarDownPlayer());
                gradePlayerItems.Add(gradeItem);
            }
        }
    }
}
