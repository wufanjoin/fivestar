using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [UIComponent(UIType.ShowChatInfoPanel)]
    public class ShowChatInfoPanelComponent : ChildUIView
    {
        #region 脚本工具生成的代码
        private GameObject mDownChatUIGo;
        private GameObject mRightChatUIGo;
        private GameObject mUpChatUIGo;
        private GameObject mDownLeftUIGo;
        public override void Awake()
        {
            base.Awake();
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mDownChatUIGo = rc.Get<GameObject>("DownChatUIGo");
            mRightChatUIGo = rc.Get<GameObject>("RightChatUIGo");
            mUpChatUIGo = rc.Get<GameObject>("UpChatUIGo");
            mDownLeftUIGo = rc.Get<GameObject>("DownLeftUIGo");
            InitPanel();
        }
        #endregion

        Dictionary<int, ChatShowUIItem> _cahtUIDic=new Dictionary<int, ChatShowUIItem>();
        public override void GameInit()
        {
            base.GameInit();
            EventMsgMgr.RegisterEvent(CommEventID.ReceiveChatInfo, ReceiveChatInfoEvnet);
        }

        public void ReceiveChatInfoEvnet(params object[] objs)
        {

            if (Game.Scene.GetComponent<ToyGameComponent>().CurrToyGame != ToyGameId.CardFiveStar)
            {
                Log.Error("当前游戏类型 只有卡五星 收到到聊天消息时 却不在卡五星游戏中:"+ Game.Scene.GetComponent<ToyGameComponent>().CurrToyGame);
                return;
            }
            long userId = (long)objs[0];
            ChatInfo chatInfo = objs[1] as ChatInfo;
            if (chatInfo.ChatType == ChatType.Text|| chatInfo.ChatType == ChatType.SpeedinessText)
            {
                ShowChatTextContent(userId, chatInfo);
            }
            else if (chatInfo.ChatType == ChatType.Voice)
            {
                AudioClip clip=SpeexRecordMgr.Ins.BytesToAudioClip(chatInfo.VoiceContent.bytes);
                ShowChatVoiceContent(userId, clip);
            }
            else if (chatInfo.ChatType == ChatType.NormalExpression)
            {
                CardFiveStarPlayer cardFiveStarPlayer = CardFiveStarRoom.Ins.GetPlayerInfoUserIdIn(userId);
                cardFiveStarPlayer.ShowExpression(int.Parse(chatInfo.Content));
            }
            else if (chatInfo.ChatType == ChatType.MagicExpression)
            {
                string[] contens=chatInfo.Content.Split(GlobalConstant.ParameteSeparator);
                string magicType = contens[0];
                long byMagicUserId = long.Parse(contens[1]);
                CardFiveStarPlayer sendPlayer = CardFiveStarRoom.Ins.GetPlayerInfoUserIdIn(userId);
                CardFiveStarPlayer receivePlayer = CardFiveStarRoom.Ins.GetPlayerInfoUserIdIn(byMagicUserId);

                UIComponent.GetUiView<MagicExpressionsPanelComponent>().ShowMagicExpressions(sendPlayer.GetHeadPosition(), receivePlayer.GetHeadPosition(), magicType);
            }
        }
        public void InitPanel()
        {
            _cahtUIDic.Add(0, new ChatShowUIItem(mDownChatUIGo,0));
            _cahtUIDic.Add(1, new ChatShowUIItem(mRightChatUIGo,1));
            _cahtUIDic.Add(2, new ChatShowUIItem(mUpChatUIGo,2));
            _cahtUIDic.Add(3, new ChatShowUIItem(mDownLeftUIGo,3));
        }
        //显示聊天 普通文字内容 和快捷文字
        public void ShowChatTextContent(long userId,ChatInfo chatInfo)
        {
            Show();
            CardFiveStarPlayer cardFiveStarPlayer=CardFiveStarRoom.Ins.GetPlayerInfoUserIdIn(userId);
            int clientSeat= cardFiveStarPlayer.ClientSeatIndex;
            _cahtUIDic[clientSeat].ShowChatContent(cardFiveStarPlayer._user.Sex, chatInfo);
        }
        //显示聊天语音内容
        public void ShowChatVoiceContent(long userId, AudioClip clip)
        {
            Show();
            int clientSeat = CardFiveStarRoom.Ins.GetPlayerInfoUserIdIn(userId).ClientSeatIndex;
            _cahtUIDic[clientSeat].ShowVoiceChat(clip);
        }
    }

    public class ChatShowUIItem
    {
        private Text _ChatText;
        private Transform _ChatVoiceAnimPointGo;
        private GameObject ChatVoiceAnimGo;
        public GameObject gameObject;
        private HorizontalLayoutGroup _chatLayoutGroup;
        private int _ClientIndex;
        public ChatShowUIItem(GameObject go,int clientIndex)
        {
            gameObject = go;
            _ChatText = gameObject.FindChild("ChatText").GetComponent<Text>();
            _ChatVoiceAnimPointGo = gameObject.FindChild("ChatVoiceAnimPointGo");
            _chatLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
            _ClientIndex = clientIndex;
        }

        public async Task ChatLayoutGroupSize()
        {
            gameObject.transform.position-=new Vector3(1000,0,0);
            gameObject.SetActive(true);
            _chatLayoutGroup.enabled = false;
            await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(200);
            _chatLayoutGroup.enabled = true;
            gameObject.transform.position += new Vector3(1000, 0, 0);
        }
        //显示文字内容 快捷语 和普通文字
        public async void ShowChatContent(int sex,ChatInfo chatInfo)
        {
          
            _ChatText.gameObject.SetActive(true);
            _ChatVoiceAnimPointGo.gameObject.SetActive(false);
            if (chatInfo.ChatType == ChatType.Text)
            {
                _ChatText.text = chatInfo.Content;
            }
            else if (chatInfo.ChatType == ChatType.SpeedinessText)
            {
                int speedinessId = int.Parse(chatInfo.Content);
                _ChatText.text = ChatImportPanelComponent._speedinessText[speedinessId];
                AudioClip clip=UIComponent.GetUiView<ChatImportPanelComponent>().GetSpedinessSound(sex, speedinessId);
                Game.Scene.GetComponent<MusicSoundComponent>().PlaySound(clip);
            }
            await ChatLayoutGroupSize();
            DelayHide(5000);
        }

        //显示语音内容
        public async void ShowVoiceChat(AudioClip clip)
        {
            _ChatText.gameObject.SetActive(false);
            _ChatVoiceAnimPointGo.gameObject.SetActive(true);
           
            Game.Scene.GetComponent<MusicSoundComponent>().PlayVoice(clip);
            int awiatTime = (int)(clip.length * 1000);

            await ChatLayoutGroupSize();
            InitChatVoiceAnimGo();//语音波浪跳动
            DelayHide(awiatTime);
        }


        //延迟隐藏
        public  void DelayHide(int time)
        {
            DelayActionTool.ExecuteDelayAction(DelayActionId.ChatFrameHide + _ClientIndex, time, Hide);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public async void InitChatVoiceAnimGo()
        {
            if (ChatVoiceAnimGo == null)
            {
                GameObject prefab=ResourcesComponent.Ins.GetResoure(UIType.ShowChatInfoPanel, "ChatVoiceAnimGo") as GameObject;
                ChatVoiceAnimGo = GameObject.Instantiate(prefab, _ChatVoiceAnimPointGo);
            }
            int i = 0;
            while (gameObject.activeInHierarchy)
            {
                await ETModel.Game.Scene.GetComponent<TimerComponent>().WaitAsync(150);
                if (i >= ChatVoiceAnimGo.transform.childCount)
                {
                    ChatVoiceAnimGo.HideAllChild();
                    i = 0;
                }
                ChatVoiceAnimGo.transform.GetChild(i++).gameObject.SetActive(true);
                
            }
        }
       
       
    }
}
