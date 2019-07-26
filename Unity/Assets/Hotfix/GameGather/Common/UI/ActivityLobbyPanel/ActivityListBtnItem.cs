using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class ActivityListBtnItemAwakeSystem : AwakeSystem<ActivityListBtnItem, GameObject, ActivityInfo>
    {
        public override void Awake(ActivityListBtnItem self, GameObject go, ActivityInfo data)
        {
            self.Awake(go, data, UIType.ActivityLobbyPanel);
        }
    }
    public class ActivityListBtnItem: BaseItem<ActivityInfo>
    {
        private Text mNameText;

        private Button mSelctBtn;

        public static ActivityListBtnItem CurrSelectBtn;
        public override void Awake(GameObject go, ActivityInfo data, string uiType)
        {
            base.Awake(go, data, uiType);
            mNameText = rc.Get<GameObject>("NameText").GetComponent<Text>();
            mSelctBtn = gameObject.GetComponent<Button>();
            InitItem();
        }


        public  void InitItem()
        {
            //mNameText.text = mData.Name;
            mSelctBtn.Add(SelectSelfEvnet);
        }

        public void SelectSelfEvnet()
        {
            if (CurrSelectBtn != null)
            {
                CurrSelectBtn.CancelSelect();
            }
            gameObject.GetComponent<Image>().sprite = GetResoure<Sprite>("ActivityBtnPitchState");
            mData.ActivityPanel.SetActive(true);
            mSelctBtn.enabled = false;
            CurrSelectBtn = this;
        }

        public void CancelSelect()
        {
            gameObject.GetComponent<Image>().sprite = GetResoure<Sprite>("ActivityBtnNoPitchState");
            mSelctBtn.enabled = true;
            mData.ActivityPanel.SetActive(false);
        }
    }
}
