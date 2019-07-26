using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.ChatImportPanel)]
    public class ChatImportPanelComponent : PopUpUIView
    {
        #region 脚本工具生成的代码
        private Toggle mExpressionToggle;
        private Toggle mCharacterToggle;
        private InputField mChatTextInputField;
        private GameObject mSpeedinessWordGroupGo;
        private GameObject mExpressionGroupGo;
        private Button mSendBtn;
        private Text mSpeedinessText;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mExpressionToggle = rc.Get<GameObject>("ExpressionToggle").GetComponent<Toggle>();
            mCharacterToggle = rc.Get<GameObject>("CharacterToggle").GetComponent<Toggle>();
            mSendBtn = rc.Get<GameObject>("SendBtn").GetComponent<Button>();
            mChatTextInputField = rc.Get<GameObject>("ChatTextInputField").GetComponent<InputField>();
            mSpeedinessWordGroupGo = rc.Get<GameObject>("SpeedinessWordGroupGo");
            mExpressionGroupGo = rc.Get<GameObject>("ExpressionGroupGo");
            mSpeedinessText = rc.Get<GameObject>("SpeedinessText").GetComponent<Text>();
            InitPanel();
        }

        public static readonly Dictionary<int, string> _speedinessText = new Dictionary<int, string>()
        {
            {1,"再不出牌,我扣120了"},
            {2,"时间就是金钱，我的朋友"},
            {3,"小心别乱放炮，我要一定要自摸"},
            {4,"哪儿有你这样打牌的呀"},
            {5,"要吃要碰麻利点，别啰嗦"},
            {6,"这把牌啊，闭着眼睛打都能自摸"},
        };
        #endregion
        public void InitPanel()
        {
            mCharacterToggle.Add(CharacterToggleEvent);
            mSendBtn.Add(SendBtnEvent);
            InitSpeedinessGroup();
            InitExpressionGroup();
        }

        public void SendBtnEvent()
        {
            if (!string.IsNullOrEmpty(mChatTextInputField.text))
            {
                ChatMgr.Ins.SendChatInfo(ChatType.Text, mChatTextInputField.text);
                mChatTextInputField.text = string.Empty;
                Hide();
            }
        }
        public void InitSpeedinessGroup()
        {
            for (int i = 0; i < _speedinessText.Count - 1; i++)
            {
                GameObject.Instantiate(mSpeedinessText.gameObject, mSpeedinessWordGroupGo.transform);
            }
            for (int i = 0; i < mSpeedinessWordGroupGo.transform.childCount; i++)
            {
                mSpeedinessWordGroupGo.transform.GetChild(i).GetComponent<Text>().text = _speedinessText[i+1];
                ChatBtnClickEvent clickEvent =new ChatBtnClickEvent((i+1).ToString(), ChatType.SpeedinessText,this);
                mSpeedinessWordGroupGo.transform.GetChild(i).GetComponent<Button>().Add(clickEvent.ClickEvent,false);
            }
        }

        private List<ExpressionAnim> _ExpressionAnims = new List<ExpressionAnim>();
        public void InitExpressionGroup()
        {
            for (int i = 1; i <= ExpressionAnimPool._ExpressionAnimCount; i++)
            {
                ExpressionAnim anim = ExpressionAnimPool.Ins.Create(i, mExpressionGroupGo.transform);
                ChatBtnClickEvent clickEvent = new ChatBtnClickEvent(i.ToString(), ChatType.NormalExpression,this);
                anim.gameObject.AddComponent<Button>().Add(clickEvent.ClickEvent, false);
                _ExpressionAnims.Add(anim); 
            }
        }

        public void ExecuteAnims()
        {
            foreach (var anim in _ExpressionAnims)
            {
                anim.ExecuteAnim();
            }
        }
        public void CharacterToggleEvent(bool isOn)
        {
            mSpeedinessWordGroupGo.SetActive(isOn);
            mExpressionGroupGo.SetActive(!isOn);
            if (!isOn)
            {
                ExecuteAnims();
            }
        }

        public override void OnShow()
        {
            base.OnShow();
            if (mExpressionGroupGo.activeInHierarchy)
            {
                ExecuteAnims();
            }
        }
        private string ManSpedinessSoundPrefix = "manpeediness_";//男快捷语 音效前缀
        private string WoManSpedinessSoundPrefix = "womanpeediness_";//女快捷语 音效前缀
        //获取快捷语的 音效
        public AudioClip GetSpedinessSound(int sex,int spdinessId)
        {
            string defaulPrefixt = WoManSpedinessSoundPrefix;
            if (sex == SexType.Man)
            {
                defaulPrefixt = ManSpedinessSoundPrefix;
            }
           return GetResoure<AudioClip>(defaulPrefixt + spdinessId);
        }
    }

    public class ChatBtnClickEvent
    {
        public string _content;
        public int _chatType = 0;
        public UIView _uiView;

        public ChatBtnClickEvent(string content,int chatType, UIView uiView=null)
        {
            _content = content;
            _chatType = chatType;
            _uiView = uiView;
        }
        public void ClickEvent()
        {
            _uiView?.Hide();
            ChatMgr.Ins.SendChatInfo(_chatType, _content);
        }
    }
}
