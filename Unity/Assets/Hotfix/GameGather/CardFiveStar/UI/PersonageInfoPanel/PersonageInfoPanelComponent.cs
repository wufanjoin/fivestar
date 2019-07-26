using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.PersonageInfoPanel)]
    public class PersonageInfoPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Text mIDText;
        private Text mIPText;
        private Text mLocationText;
        private Text mDistanceText;
        private Image mHeadImage;
        private GameObject mMagicExpressionGroupGo;
        private GameObject mMagicExpreesionGo;
        private GameObject mUserInfoPanelGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mIDText = rc.Get<GameObject>("IDText").GetComponent<Text>();
            mIPText = rc.Get<GameObject>("IPText").GetComponent<Text>();
            mLocationText = rc.Get<GameObject>("LocationText").GetComponent<Text>();
            mDistanceText = rc.Get<GameObject>("DistanceText").GetComponent<Text>();
            mHeadImage = rc.Get<GameObject>("HeadImage").GetComponent<Image>();
            mMagicExpressionGroupGo = rc.Get<GameObject>("MagicExpressionGroupGo");
            mMagicExpreesionGo = rc.Get<GameObject>("MagicExpreesionGo");
            mUserInfoPanelGo = rc.Get<GameObject>("UserInfoPanelGo");
            InitPanel();
        }
        #endregion

        public override bool isShakeAnimation
        {
            get { return false; }
        }
        public void InitPanel()
        {
            InitMagicGroup();
        }
        private List<string> _MagicExpreesionTypes = new List<string>()
        {
            MagicExpressionsType.JiDan,
            MagicExpressionsType.MeiGui,
            MagicExpressionsType.ZanJie,
            MagicExpressionsType.ZhaDan,
            MagicExpressionsType.ZhuanTou
        };

        //初始化魔法表情列表
        public void InitMagicGroup()
        {
            for (int i = 1; i < _MagicExpreesionTypes.Count; i++)
            {
                GameObject.Instantiate(mMagicExpreesionGo, mMagicExpressionGroupGo.transform);
            }
            for (int i = 0; i < _MagicExpreesionTypes.Count; i++)
            {
                MagicExpressionBtnClick clickEvent = new MagicExpressionBtnClick(_MagicExpreesionTypes[i]);
                mMagicExpressionGroupGo.transform.GetChild(i).gameObject.AddComponent<Button>().Add(clickEvent.ClickEvnet, false);


                Image image = mMagicExpressionGroupGo.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                image.sprite = ResourcesComponent.Ins.GetResoure("common", MagicExpressionsAnim.IconSpritePrefix + _MagicExpreesionTypes[i]) as Sprite;
                image.SetNativeSize();
            }
        }


        public override float MaskLucencyValue
        {
            get
            {
                return 0f;
            }
        }


        public User _CurrShowUser;
        public async void ShowUserInfo(int seatIndex, Transform panelParent, User user)
        {
            _CurrShowUser = user;
            Show();
            mIDText.text = "ID:" + user.GetShowUserId();
            mIPText.text = "IP:" + user.Ip;
            string loctionAddress = user.GetLocationAddress();
            if (string.IsNullOrEmpty(loctionAddress))
            {
                loctionAddress = "位置未知";
                ;
            }
            mLocationText.text = loctionAddress;
            double distance = user.GetDistance(UserComponent.Ins.pSelfUser);
            string distanceDesc = string.Empty;
            if (distance < 0)
            {
                distanceDesc = "距离未知";
            }
            else if (distance < 1000)
            {
                distanceDesc = distance + "米";
            }
            else
            {
                distanceDesc = distance / 1000.00f + "公里";
            }
            mDistanceText.text = distanceDesc;
            //224 130
            mUserInfoPanelGo.transform.SetParent(panelParent);
            mUserInfoPanelGo.transform.localPosition = Vector3.zero;
            mUserInfoPanelGo.transform.SetParent(gameObject.transform);
            if (seatIndex == 0)
            {
                mUserInfoPanelGo.GetComponent<RectTransform>().sizeDelta = new Vector2(395, 130);//rect.size=new Vector2(367,224);
                mMagicExpressionGroupGo.SetActive(false);
                mDistanceText.gameObject.SetActive(false);
            }
            else
            {
                mUserInfoPanelGo.GetComponent<RectTransform>().sizeDelta = new Vector2(395, 221);
                mMagicExpressionGroupGo.SetActive(true);
                mDistanceText.gameObject.SetActive(true);
            }
            mHeadImage.sprite = null;//最后赋值头像
            mHeadImage.sprite = await user.GetHeadSprite();//最后赋值头像
        }

    }

    public class MagicExpressionBtnClick
    {
        public string _type;
        public PersonageInfoPanelComponent _PersonageInfoPanel;
        public MagicExpressionBtnClick(string type)
        {
            _type = type;
            _PersonageInfoPanel = UIComponent.GetUiView<PersonageInfoPanelComponent>();
        }

        public void ClickEvnet()
        {
            _PersonageInfoPanel.Hide();
            ChatMgr.Ins.SendChatInfo(ChatType.MagicExpression, _type + GlobalConstant.ParameteSeparator + _PersonageInfoPanel._CurrShowUser.UserId);
        }
    }
}
