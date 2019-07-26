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
    public class GetSignInAwardItemAwakeSystem : AwakeSystem<GetSignInAwardItem, GameObject, SignInAward>
    {
        public override void Awake(GetSignInAwardItem self, GameObject go, SignInAward data)
        {
            self.Awake(go, data, UIType.ActivityAndAnnouncementPanel);
        }
    }

    public  class GetSignInAwardItem :BaseItem<SignInAward>
    {
        private Text _NumberText;
        private Text _JewelAmountText;
        private GameObject _AlreadyGetGo;
        public override void Awake(GameObject go, SignInAward data, string uiType)
        {
            base.Awake(go, data, uiType);
            _NumberText = gameObject.FindChild("NumberText").GetComponent<Text>();
            _JewelAmountText = gameObject.FindChild("JewelAmountText").GetComponent<Text>();
            _AlreadyGetGo = gameObject.FindChild("AlreadyGetGo").gameObject;
            InitPanel();
        }

        public  void InitPanel()
        {
            _JewelAmountText.text = "x" + mData.Amount;
            _NumberText.text = $"第{mData.NumberDays}天";
            _AlreadyGetGo.SetActive(SignInActivityView._SingInDays >= mData.NumberDays);
        }
    }
}
