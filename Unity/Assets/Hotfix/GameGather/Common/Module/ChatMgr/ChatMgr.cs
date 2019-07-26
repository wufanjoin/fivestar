using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf;
using UnityEngine;

namespace ETHotfix
{
    public class ChatType
    {
        public const int Text=1;//文字
        public const int SpeedinessText = 2;//快捷文字
        public const int NormalExpression = 3;//正常动态表情
        public const int MagicExpression = 4;//魔法表情
        public const int Voice = 5;//语音
    }
  public class ChatMgr:Single<ChatMgr>
    {
        public void SendChatInfo(int type,string content)
        {
            ChatInfo chatInfo=new ChatInfo();
            chatInfo.ChatType = type;
            chatInfo.Content = content;
            SessionComponent.Instance.Send(new C2M_UserChat(){ChatInfo = chatInfo });  
        }

        public void ReceiveChatInfo(long sendUserId,ChatInfo chatInfo)
        {
            EventMsgMgr.SendEvent(CommEventID.ReceiveChatInfo, sendUserId, chatInfo);
        }

        public void RegisterVoiceBtn(GameObject voiceBtnGo)
        {
            new VoiceBtnEvnet().Init(voiceBtnGo);
        }
    }

    public class VoiceBtnEvnet
    {
        public void Init(GameObject voiceBtnGo)
        {
            PointerEvent _PointerEvent = voiceBtnGo.AddComponent<PointerEvent>();
            //注册事件委托
            _PointerEvent.OnPointerDownAction -= OnPointerDown;
            _PointerEvent.OnPointerUpAction -= OnPointerUp;
            _PointerEvent.OnPointerExitAction -= OnPointerExit;

            _PointerEvent.OnPointerDownAction += OnPointerDown;
            _PointerEvent.OnPointerUpAction += OnPointerUp;
            _PointerEvent.OnPointerExitAction += OnPointerExit;
        }

        private bool _isBeingRecord = false;
        public void OnPointerDown()
        {
            //Log.Debug("语音按钮按下");
            _isBeingRecord = true;
            SpeexRecordMgr.Ins.StartRecording();
            UIComponent.GetUiView<RecodPanelComponent>().ShowNomalPanel();
        }
        public void OnPointerUp()
        {
             //Log.Debug("语音按钮抬起");
            UIComponent.GetUiView<RecodPanelComponent>().Hide();
            if (_isBeingRecord)
            {
                ChatInfo chatInfo = new ChatInfo();
                chatInfo.ChatType = ChatType.Voice;
              
                chatInfo.VoiceContent = new ByteString(SpeexRecordMgr.Ins.EndRecording());
                if (chatInfo.VoiceContent.Length < 300)
                {
                    _isBeingRecord = false;
                    return;
                }
                SessionComponent.Instance.Send(new C2M_UserChat() { ChatInfo = chatInfo });
            }
            _isBeingRecord = false;

        }
        public void OnPointerExit()
        {
          //  Log.Debug("语音按钮划出");
            if (_isBeingRecord)
            {
                _isBeingRecord = false;
                SpeexRecordMgr.Ins.EndRecording();
                UIComponent.GetUiView<RecodPanelComponent>().ShowCancelPanel();
            }
           
        }
    }
}
