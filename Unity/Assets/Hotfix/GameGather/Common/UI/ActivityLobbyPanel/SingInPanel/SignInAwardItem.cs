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
    public class SignInAwardItemAwakeSystem : AwakeSystem<SignInAwardItem, GameObject, SignInAward>
    {
        public override void Awake(SignInAwardItem self, GameObject go, SignInAward data)
        {
            self.Awake(go, data, UIType.ActivityLobbyPanel);
        }
    }
    public class SignInAwardItem : BaseItem<SignInAward>
    {
        private Text AwardNumText;

        private Image IconImage;

        private Text DayNumText;

        private GameObject DoneSignInGo;

        public static ActivityListBtnItem CurrSelectBtn;
        public override void Awake(GameObject go, SignInAward data, string uiType)
        {
            base.Awake(go, data, uiType);
            AwardNumText = rc.Get<GameObject>("AwardNumText").GetComponent<Text>();
            IconImage = rc.Get<GameObject>("IconImage").GetComponent<Image>();
            DayNumText = rc.Get<GameObject>("DayNumText").GetComponent<Text>();
            DoneSignInGo = rc.Get<GameObject>("DoneSignInGo");
            RegisterEvent();
            InitItem();
        }

        public void RegisterEvent()
        {
            //EventMsgMgr.RegisterEvent(CommEventID.UserFinshSignIn, UserFinshSignInEvent);
        }

        public void UserFinshSignInEvent(params object[] objs)
        {
            int dayNum = (int)objs[0];
            if (mData.NumberDays == dayNum)
            {
                mData.isDoneSignIn = true;
            }
            InitItem();
        }
        private void InitItem()
        {
            AwardNumText.text = "x"+mData.Amount;
           // IconImage.sprite =GetResoure(GoodsInfoTool.GetGoodsIcon(mData.GoodsType)) as Sprite;
            IconImage.sprite = GetResoure<Sprite>(GoodsInfoTool.GetGoodsIcon(mData.GoodsId));
            DayNumText.text = $"第{mData.NumberDays}天";
            DoneSignInGo.SetActive(mData.isDoneSignIn);
            
        }


    }
}
