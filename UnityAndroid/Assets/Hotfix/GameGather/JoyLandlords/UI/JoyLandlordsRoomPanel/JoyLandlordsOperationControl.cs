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
    public class JoyLandlordsOperationType
    {
        public const int PlayCard = 1;//正常出牌
        public const int NodePlayCard = 2;//要不起
        public const int MustPlayCard = 3;//第一手必须出牌
        public const int RobJoyLandlords = 4;//抢地主
        public const int CallJoyLandlords = 5;//叫地主
        public const int AddTwice=6;//加倍
        public const int NodeAddTwice = 7;//无法加倍
        public const int StartGame = 8;//开始游戏
    }
    public class JoyLandlordsOperationControl : Single<JoyLandlordsOperationControl>
    {

        private GameObject mOperationBtnParentGo;
        private Dictionary<int,List<object>> TypeDictionary=new Dictionary<int, List<object>>();
        private Button NoPlayBtn;//不出
        private Button CantAffordToBtn;//要不起
        private Button HintBtn;//提示
        private Button PlayCardBtn;//出牌
        private Button RobLandlordsBtn;//抢地主
        private Button NoRobBtnBtn;//不抢
        private Button AddTwiceBtn;//加倍
        private Button NoAddTwiceBtn;//不加倍
        private Button CallLandlordsBtn;//叫地主
        private Button NoCallBtn;//不叫
        private Button StartGameBtn;//开始游戏
        private AlarmClock mAlarmClock;//时钟
        public void Init(GameObject operationBtnParentGo,Button startGameBtn)
        {
            mOperationBtnParentGo = operationBtnParentGo;
            StartGameBtn = startGameBtn;
            InitBtn();
            InitTypeDictionary();
        }

        private void InitTypeDictionary()
        {
            TypeDictionary.Add(JoyLandlordsOperationType.PlayCard, new List<object>() { NoPlayBtn, mAlarmClock, PlayCardBtn });
            TypeDictionary.Add(JoyLandlordsOperationType.NodePlayCard, new List<object>() { mAlarmClock, CantAffordToBtn });
            TypeDictionary.Add(JoyLandlordsOperationType.MustPlayCard, new List<object>() { mAlarmClock, PlayCardBtn });
            TypeDictionary.Add(JoyLandlordsOperationType.RobJoyLandlords, new List<object>() { RobLandlordsBtn, mAlarmClock, NoRobBtnBtn });
            TypeDictionary.Add(JoyLandlordsOperationType.CallJoyLandlords, new List<object>() { CallLandlordsBtn, mAlarmClock, NoCallBtn });
            TypeDictionary.Add(JoyLandlordsOperationType.AddTwice, new List<object>() { AddTwiceBtn, mAlarmClock, NoAddTwiceBtn });
            TypeDictionary.Add(JoyLandlordsOperationType.NodeAddTwice, new List<object>() { mAlarmClock, NoAddTwiceBtn });
            TypeDictionary.Add(JoyLandlordsOperationType.StartGame, new List<object>() { StartGameBtn });
            ShowBtnType(JoyLandlordsOperationType.StartGame);
        }


        private GameObject PlayCardsBlueBtnPrefab;
        private GameObject PlayCardsYellowBtnPrefab;
        public void InitBtn()
        {
            mAlarmClock = AlarmClockFactory.Create(mOperationBtnParentGo.transform);//初始化计时器
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            PlayCardsBlueBtnPrefab = resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "PlayCardsBlueBtn") as GameObject;
            PlayCardsYellowBtnPrefab = resourcesComponent.GetResoure(UIType.JoyLandlordsRoomPanel, "PlayCardsYellowBtn") as GameObject;

            NoPlayBtn = CreateBtn(PlayCardsYellowBtnPrefab, "不出", DontPlay);
            CantAffordToBtn = CreateBtn(PlayCardsYellowBtnPrefab, "要不起", DontPlay);
            HintBtn = CreateBtn(PlayCardsYellowBtnPrefab, "提示", Hinit);
            RobLandlordsBtn = CreateBtn(PlayCardsYellowBtnPrefab, "抢地主", RobLanlord);
            NoRobBtnBtn = CreateBtn(PlayCardsYellowBtnPrefab, "不抢", NoRobLanlord);
            AddTwiceBtn = CreateBtn(PlayCardsYellowBtnPrefab, "加倍", AddTwice);
            NoAddTwiceBtn = CreateBtn(PlayCardsYellowBtnPrefab, "不加倍", NoAddTwice);
            CallLandlordsBtn = CreateBtn(PlayCardsYellowBtnPrefab, "叫地主", CallLanlord);
            NoCallBtn = CreateBtn(PlayCardsYellowBtnPrefab, "不叫", NoCallLanlord);
            PlayCardBtn = CreateBtn(PlayCardsBlueBtnPrefab, "出牌", PlayCard);
            StartGameBtn.Add(StarMathch);//开始匹配
           
        }
        
        public Button CreateBtn(GameObject prefab,string content,Action evenAction)
        {
            Button button=GameObject.Instantiate(prefab, mOperationBtnParentGo.transform).GetComponent<Button>();
            button.SetText(content);
            button.Add(evenAction);
            return button;
        }

        public void StarMathch()
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.StarMathch);
        }
        public void CallLanlord()
        {
            SessionComponent.Instance.Send(new Actor_JoyLds_CallLanlord() { Result = true });
        }
        public void NoCallLanlord()
        {
            SessionComponent.Instance.Send(new Actor_JoyLds_CallLanlord() { Result = false });
        }
        public void AddTwice()
        {
            SessionComponent.Instance.Send(new Actor_JoyLds_AddTwice() { Result = true });
        }
        public void NoAddTwice()
        {
            SessionComponent.Instance.Send(new Actor_JoyLds_AddTwice() { Result = false });
        }
        public void RobLanlord()
        {
            SessionComponent.Instance.Send(new Actor_JoyLds_RobLanlord() { Result = true });
        }
        public void NoRobLanlord()
        {
            SessionComponent.Instance.Send(new Actor_JoyLds_RobLanlord() { Result = false });
        }
        public void Hinit()
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.PlayCardHint);
        }
        public void DontPlay()
        {
            SessionComponent.Instance.Send(new Actor_JoyLds_DontPlay());
        }
        public void PlayCard()
        {
            EventMsgMgr.SendEvent(JoyLandlordsEventID.RequestPlayCard);
        }
        public void HideBtnAll()
        {
            mOperationBtnParentGo.SetActive(false);
        }
        public void ShowBtnType(int joyLandlordsOperationType,int residueTime=0)
        {
            mOperationBtnParentGo.SetActive(true);
            mOperationBtnParentGo.HideAllChild();
            List<object> buttons = TypeDictionary[joyLandlordsOperationType];
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].GetType() == typeof(Button))
                {
                    Button button = buttons[i] as Button;
                    button.gameObject.SetActive(true);
                    button.transform.SetSiblingIndex(i);
                }
                else
                {
                    AlarmClock  alarmClock=buttons[i] as AlarmClock;
                    alarmClock.Show(residueTime, i);
                }
            }
           
        }

    }
}
